using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using VRMGames.CartridgeAndCloud.Domain.Customers;

namespace VRMGames.CartridgeAndCloud.Infrastructure.Customers
{
    /// <summary>
    /// Drives the technical customer view along its navigation plan.
    /// Movement is constrained to the horizontal plane captured at configuration time.
    /// </summary>
    public sealed class CustomerTechnicalAgentView : MonoBehaviour
    {
        private CustomerInstance _customer;
        private Vector3[] _waypoints;
        private float _walkSpeed;
        private float _arrivalTolerance;
        private float _wholeSecondAccumulator;
        private float _dwellRemaining;
        private bool _completionRaised;
        private bool _destroyOnCompletion;
        private Quaternion _targetRotation;
        private float _groundY;
        private NavMeshAgent _agent;
        private int _agentTargetIndex = -1;

        [SerializeField, Min(0f)]
        private float _rotationSpeedDegrees = 540f;

        public event Action<CustomerTechnicalAgentView> Completed;

        public CustomerInstance Customer => _customer;

        public bool IsConfigured =>
            _customer != null && _waypoints != null;

        public void Configure(
            CustomerInstance customer,
            IEnumerable<Vector3> waypoints,
            float walkSpeed,
            float arrivalTolerance,
            bool destroyOnCompletion)
        {
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer));
            }

            if (waypoints == null)
            {
                throw new ArgumentNullException(nameof(waypoints));
            }

            if (walkSpeed <= 0f || float.IsNaN(walkSpeed) ||
                float.IsInfinity(walkSpeed))
            {
                throw new ArgumentOutOfRangeException(nameof(walkSpeed));
            }

            if (arrivalTolerance < 0f ||
                float.IsNaN(arrivalTolerance) ||
                float.IsInfinity(arrivalTolerance))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(arrivalTolerance));
            }

            Vector3[] copied = new List<Vector3>(waypoints).ToArray();
            if (copied.Length != customer.NavigationPlan.Count)
            {
                throw new ArgumentException(
                    "Waypoint count must match the navigation plan.",
                    nameof(waypoints));
            }

            _customer = customer;
            _waypoints = copied;
            _walkSpeed = walkSpeed;
            _arrivalTolerance = arrivalTolerance;
            _destroyOnCompletion = destroyOnCompletion;
            _wholeSecondAccumulator = 0f;
            _dwellRemaining = 0f;
            _completionRaised = false;

            _groundY = transform.position.y;
            _agent = GetComponent<NavMeshAgent>();
            if (_agent == null)
            {
                _agent = gameObject.AddComponent<NavMeshAgent>();
            }
            _agent.radius = 0.32f;
            _agent.height = 1.8f;
            _agent.baseOffset = 0f;
            _agent.speed = _walkSpeed;
            _agent.angularSpeed = _rotationSpeedDegrees;
            _agent.acceleration = 12f;
            _agent.stoppingDistance = Mathf.Max(0.05f, _arrivalTolerance);
            _agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
            _agentTargetIndex = -1;

            if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                _agent.Warp(hit.position);
            }
        }

        public void Tick(float deltaTime)
        {
            if (!IsConfigured)
            {
                return;
            }

            if (deltaTime < 0f || float.IsNaN(deltaTime) ||
                float.IsInfinity(deltaTime))
            {
                throw new ArgumentOutOfRangeException(nameof(deltaTime));
            }

            if (_customer.State == CustomerState.Despawned)
            {
                RaiseCompletion();
                return;
            }

            TickPatience(deltaTime);
            if (_customer.State == CustomerState.Leaving)
            {
                _dwellRemaining = 0f;
            }

            if (_dwellRemaining > 0f)
            {
                _dwellRemaining = Mathf.Max(0f, _dwellRemaining - deltaTime);
                if (_dwellRemaining > 0f)
                {
                    if (_agent != null && _agent.isOnNavMesh)
                    {
                        _agent.isStopped = true;
                    }
                    return;
                }

                CompleteCurrentTarget();
                return;
            }

            int index = _customer.CurrentTargetIndex;
            if (index < 0 || index >= _waypoints.Length)
            {
                throw new InvalidOperationException(
                    $"Customer target index {index} is outside the waypoint array.");
            }

            Vector3 target = _waypoints[index];

            if (_agent != null && _agent.isOnNavMesh &&
                NavMesh.SamplePosition(target, out NavMeshHit targetHit, 2f, NavMesh.AllAreas))
            {
                _agent.speed = _walkSpeed;
                _agent.stoppingDistance = Mathf.Max(0.05f, _arrivalTolerance);

                if (_agentTargetIndex != index)
                {
                    _agentTargetIndex = index;
                    _agent.isStopped = false;
                    _agent.SetDestination(targetHit.position);
                }

                if (_agent.pathPending ||
                    _agent.remainingDistance > _agent.stoppingDistance + 0.03f)
                {
                    return;
                }

                _agent.isStopped = true;
            }
            else
            {
                Vector3 currentPosition = transform.position;
                currentPosition.y = _groundY;
                target.y = _groundY;
                Vector3 direction = target - currentPosition;
                direction.y = 0f;
                if (direction.sqrMagnitude > 0.0001f)
                {
                    _targetRotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
                    transform.rotation = Quaternion.RotateTowards(
                        transform.rotation,
                        _targetRotation,
                        _rotationSpeedDegrees * deltaTime);
                }

                Vector3 nextPosition = Vector3.MoveTowards(
                    currentPosition,
                    target,
                    _walkSpeed * deltaTime);
                nextPosition.y = _groundY;
                transform.position = nextPosition;

                if (Vector3.Distance(nextPosition, target) > _arrivalTolerance)
                {
                    return;
                }
            }

            int dwell = _customer.CurrentTarget.DwellSeconds;
            if (dwell > 0)
            {
                _dwellRemaining = dwell;
                return;
            }

            CompleteCurrentTarget();
        }

        private void Update()
        {
            Tick(Time.deltaTime);
        }

        private void TickPatience(float deltaTime)
        {
            if (_customer.State != CustomerState.Browsing)
            {
                _wholeSecondAccumulator = 0f;
                return;
            }

            _wholeSecondAccumulator += deltaTime;
            int wholeSeconds = Mathf.FloorToInt(_wholeSecondAccumulator);
            if (wholeSeconds <= 0)
            {
                return;
            }

            _wholeSecondAccumulator -= wholeSeconds;
            _customer.AdvancePatience(wholeSeconds);
        }

        private void CompleteCurrentTarget()
        {
            CustomerTransitionResult result =
                _customer.ArriveAtCurrentTarget();

            if (!result.Succeeded)
            {
                return;
            }

            if (_customer.State == CustomerState.Despawned)
            {
                RaiseCompletion();
            }
        }

        private void RaiseCompletion()
        {
            if (_completionRaised)
            {
                return;
            }

            _completionRaised = true;
            Completed?.Invoke(this);

            if (_destroyOnCompletion)
            {
                Destroy(gameObject);
            }
        }
    }
}
