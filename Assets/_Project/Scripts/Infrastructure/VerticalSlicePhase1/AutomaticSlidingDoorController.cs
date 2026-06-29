using System;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.VerticalSlicePhase1;

namespace VRMGames.CartridgeAndCloud.Infrastructure.VerticalSlicePhase1
{
    public sealed class AutomaticSlidingDoorController :
        MonoBehaviour
    {
        private Transform _leftPanel;
        private Transform _rightPanel;
        private Vector3 _leftClosed;
        private Vector3 _rightClosed;
        private Vector3 _leftOpen;
        private Vector3 _rightOpen;
        private float _sensorDistance;
        private float _speed;
        private bool _isOpen;

        public bool IsOpen => _isOpen;

        public event Action<bool> OpenStateChanged;

        public void Configure(
            Transform leftPanel,
            Transform rightPanel,
            float panelTravelDistance,
            float sensorDistance,
            float speed)
        {
            if (leftPanel == null)
            {
                throw new ArgumentNullException(
                    nameof(leftPanel));
            }

            if (rightPanel == null)
            {
                throw new ArgumentNullException(
                    nameof(rightPanel));
            }

            if (panelTravelDistance <= 0f)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(panelTravelDistance));
            }

            if (sensorDistance <= 0f)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(sensorDistance));
            }

            if (speed <= 0f)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(speed));
            }

            _leftPanel = leftPanel;
            _rightPanel = rightPanel;
            _leftClosed =
                leftPanel.localPosition;
            _rightClosed =
                rightPanel.localPosition;
            _leftOpen =
                _leftClosed +
                Vector3.left *
                panelTravelDistance;
            _rightOpen =
                _rightClosed +
                Vector3.right *
                panelTravelDistance;
            _sensorDistance =
                sensorDistance;
            _speed = speed;
        }

        private void Update()
        {
            if (_leftPanel == null ||
                _rightPanel == null)
            {
                return;
            }

            bool shouldOpen =
                HasCharacterNearby();

            if (shouldOpen != _isOpen)
            {
                _isOpen = shouldOpen;
                OpenStateChanged?.Invoke(
                    _isOpen);
            }

            Vector3 leftTarget =
                _isOpen
                    ? _leftOpen
                    : _leftClosed;

            Vector3 rightTarget =
                _isOpen
                    ? _rightOpen
                    : _rightClosed;

            _leftPanel.localPosition =
                Vector3.MoveTowards(
                    _leftPanel.localPosition,
                    leftTarget,
                    _speed *
                    Time.unscaledDeltaTime);

            _rightPanel.localPosition =
                Vector3.MoveTowards(
                    _rightPanel.localPosition,
                    rightTarget,
                    _speed *
                    Time.unscaledDeltaTime);
        }

        private bool HasCharacterNearby()
        {
            Phase1CharacterPresence[] characters =
                UnityEngine.Object
                    .FindObjectsByType<
                        Phase1CharacterPresence>(
                            FindObjectsInactive
                                .Exclude,
                            FindObjectsSortMode.None);

            float squaredDistance =
                _sensorDistance *
                _sensorDistance;

            foreach (Phase1CharacterPresence
                     character in characters)
            {
                if ((character.transform.position -
                     transform.position)
                    .sqrMagnitude <= squaredDistance)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
