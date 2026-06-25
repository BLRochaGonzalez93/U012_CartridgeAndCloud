using System;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Application.PlayerMovement;

namespace VRMGames.CartridgeAndCloud.Presentation.PlayerMovement
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class ClickToMoveAgent : MonoBehaviour
    {
        [SerializeField, Min(0f)]
        private float _moveSpeed = 4f;

        [SerializeField, Min(0f)]
        private float _rotationSpeedDegrees = 720f;

        [SerializeField, Min(0f)]
        private float _stoppingDistance = 0.1f;

        [SerializeField]
        private float _gravity = -20f;

        private CharacterController _characterController;
        private Vector3 _destination;
        private float _verticalVelocity;
        private bool _hasDestination;

        public bool HasDestination => _hasDestination;

        public Vector3 Destination => _destination;

        public float MoveSpeed => _moveSpeed;

        private void Awake()
        {
            _characterController =
                GetComponent<CharacterController>();
        }

        private void Update()
        {
            Tick(Time.deltaTime);
        }

        public void Configure(
            float moveSpeed,
            float rotationSpeedDegrees,
            float stoppingDistance,
            float gravity)
        {
            if (moveSpeed < 0f)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(moveSpeed));
            }

            if (rotationSpeedDegrees < 0f)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(rotationSpeedDegrees));
            }

            if (stoppingDistance < 0f)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(stoppingDistance));
            }

            _moveSpeed = moveSpeed;
            _rotationSpeedDegrees = rotationSpeedDegrees;
            _stoppingDistance = stoppingDistance;
            _gravity = gravity;
        }

        public void SetDestination(Vector3 worldPosition)
        {
            _destination = new Vector3(
                worldPosition.x,
                transform.position.y,
                worldPosition.z);

            _hasDestination = true;
        }

        public void CancelDestination()
        {
            _hasDestination = false;
        }

        public void Tick(float deltaTime)
        {
            if (deltaTime <= 0f)
            {
                return;
            }

            EnsureController();

            Vector3 horizontalDelta = Vector3.zero;

            if (_hasDestination)
            {
                Vector3 currentPosition = transform.position;

                PlanarMovementStep step =
                    PlanarMovementCalculator.Calculate(
                        currentPosition.x,
                        currentPosition.z,
                        _destination.x,
                        _destination.z,
                        _moveSpeed,
                        deltaTime,
                        _stoppingDistance);

                horizontalDelta = new Vector3(
                    step.DeltaX,
                    0f,
                    step.DeltaZ);

                RotateTowards(horizontalDelta, deltaTime);
            }

            if (_characterController.isGrounded &&
                _verticalVelocity < 0f)
            {
                _verticalVelocity = -2f;
            }

            _verticalVelocity += _gravity * deltaTime;

            Vector3 displacement =
                horizontalDelta +
                Vector3.up * (_verticalVelocity * deltaTime);

            _characterController.Move(displacement);

            if (_hasDestination &&
                IsWithinStoppingDistance(transform.position))
            {
                _hasDestination = false;
            }
        }

        private void RotateTowards(
            Vector3 horizontalDelta,
            float deltaTime)
        {
            if (horizontalDelta.sqrMagnitude <= 0.000001f)
            {
                return;
            }

            Quaternion targetRotation =
                Quaternion.LookRotation(
                    horizontalDelta.normalized,
                    Vector3.up);

            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                _rotationSpeedDegrees * deltaTime);
        }

        private bool IsWithinStoppingDistance(
            Vector3 currentPosition)
        {
            Vector2 current = new Vector2(
                currentPosition.x,
                currentPosition.z);

            Vector2 target = new Vector2(
                _destination.x,
                _destination.z);

            return Vector2.Distance(current, target) <=
                   _stoppingDistance + 0.001f;
        }

        private void EnsureController()
        {
            if (_characterController == null)
            {
                _characterController =
                    GetComponent<CharacterController>();
            }
        }
    }
}
