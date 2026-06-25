using System;
using System.Collections.Generic;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.Customers;

namespace VRMGames.CartridgeAndCloud.Infrastructure.Customers
{
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
                _dwellRemaining = Mathf.Max(
                    0f,
                    _dwellRemaining - deltaTime);
                if (_dwellRemaining > 0f)
                {
                    return;
                }

                CompleteCurrentTarget();
                return;
            }

            int index = _customer.CurrentTargetIndex;
            Vector3 target = _waypoints[index];
            transform.position = Vector3.MoveTowards(
                transform.position,
                target,
                _walkSpeed * deltaTime);

            if (Vector3.Distance(transform.position, target) >
                _arrivalTolerance)
            {
                return;
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
            int wholeSeconds = Mathf.FloorToInt(
                _wholeSecondAccumulator);
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
