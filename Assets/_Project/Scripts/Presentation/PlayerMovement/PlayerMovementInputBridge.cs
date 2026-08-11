using System;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Application.PlayerAgency;
using VRMGames.CartridgeAndCloud.Application.PlayerMovement;

namespace VRMGames.CartridgeAndCloud.Presentation.PlayerMovement
{
    /// <summary>
    /// Applies player movement intent to the click-to-move agent while keeping
    /// the physical carry authority outside Presentation.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ClickToMoveAgent))]
    public sealed class PlayerMovementInputBridge : MonoBehaviour
    {
        [SerializeField, Min(1f)]
        private float _sprintMultiplier = 1.6f;

        private ClickToMoveAgent _agent;
        private PlayerCarryLoadService _carryLoad;
        private bool _sprintRequested;
        private bool _isSprinting;
        private bool _isSprintBlocked;

        public bool SprintRequested => _sprintRequested;
        public bool IsSprinting => _isSprinting;
        public bool IsSprintBlocked => _isSprintBlocked;
        public float SprintMultiplier => _sprintMultiplier;

        public event Action MovementStateChanged;

        private void Awake()
        {
            _agent = GetComponent<ClickToMoveAgent>();
        }

        private void OnDisable()
        {
            SetSprintRequested(false);
        }

        private void OnDestroy()
        {
            if (_carryLoad != null)
            {
                _carryLoad.LoadChanged -= HandleLoadChanged;
            }
        }

        public void Configure(PlayerCarryLoadService carryLoad)
        {
            if (_carryLoad != null)
            {
                _carryLoad.LoadChanged -= HandleLoadChanged;
            }

            _carryLoad = carryLoad ??
                throw new ArgumentNullException(nameof(carryLoad));
            _carryLoad.LoadChanged += HandleLoadChanged;
            ApplyMovementState();
        }

        public void SetSprintRequested(bool requested)
        {
            if (_sprintRequested == requested)
            {
                return;
            }

            _sprintRequested = requested;
            ApplyMovementState();
        }

        private void HandleLoadChanged()
        {
            ApplyMovementState();
        }

        private void ApplyMovementState()
        {
            if (_agent == null)
            {
                _agent = GetComponent<ClickToMoveAgent>();
            }

            bool blockedByLoad =
                _carryLoad != null && _carryLoad.BlocksSprint;

            PlayerSprintDecision decision =
                PlayerSprintPolicy.Evaluate(
                    _sprintRequested,
                    blockedByLoad,
                    Mathf.Max(1f, _sprintMultiplier));

            bool changed =
                _isSprinting != decision.Allowed ||
                _isSprintBlocked !=
                    (decision.Requested && blockedByLoad);

            _isSprinting = decision.Allowed;
            _isSprintBlocked =
                decision.Requested && blockedByLoad;

            _agent.SetSpeedMultiplier(
                decision.SpeedMultiplier);

            if (changed)
            {
                MovementStateChanged?.Invoke();
            }
        }
    }
}
