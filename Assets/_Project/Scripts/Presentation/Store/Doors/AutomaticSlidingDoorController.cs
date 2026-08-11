using System;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Presentation.Interaction;
namespace VRMGames.CartridgeAndCloud.Presentation.Store.Doors
{
    public sealed class AutomaticSlidingDoorController :
        MonoBehaviour,
        IWorldInteractionTarget
    {
        [SerializeField]
        private AutomaticDoorParts _parts;

        [SerializeField]
        private AutomaticDoorSensor _sensor;

        private Transform _leftPanel;
        private Transform _rightPanel;
        private Vector3 _leftClosed;
        private Vector3 _rightClosed;
        private Vector3 _leftOpen;
        private Vector3 _rightOpen;
        [SerializeField]
        private float _panelTravelDistance = 1f;

        [SerializeField]
        private float _sensorDistance = 2f;

        [SerializeField]
        private float _speed = 2.5f;
        private bool _isOpen;
        private Collider[] _panelColliders = Array.Empty<Collider>();

        public bool IsOpen => _isOpen;

        public string InteractionId =>
            "store-entrance-door";

        public WorldInteractionKind InteractionKind =>
            WorldInteractionKind.Door;

        public Transform InteractionTransform =>
            transform;

        public float InteractionRange => 2f;

        public int InteractionPriority => 80;

        public bool IsInteractionAvailable => true;

        public Vector3 GetInteractionPoint(
            Vector3 actorPosition)
        {
            return WorldInteractionGeometry.ResolveClosestPoint(
                transform,
                actorPosition);
        }

        public event Action<bool> OpenStateChanged;

        private void Awake()
        {
            _parts = _parts != null ? _parts : GetComponent<AutomaticDoorParts>();
            _sensor = _sensor != null
                ? _sensor
                : GetComponentInChildren<AutomaticDoorSensor>(true);

            if (_parts != null &&
                _parts.LeftPanel != null &&
                _parts.RightPanel != null)
            {
                Configure(
                    _parts.LeftPanel,
                    _parts.RightPanel,
                    _panelTravelDistance,
                    _sensorDistance,
                    _speed);
            }
        }

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

            _parts = _parts != null ? _parts : GetComponent<AutomaticDoorParts>();
            _sensor = _sensor != null
                ? _sensor
                : GetComponentInChildren<AutomaticDoorSensor>(true);
            if (_sensor == null)
            {
                throw new InvalidOperationException(
                    "AutomaticDoor.prefab requires its authored sensor.");
            }

            _sensor.Configure(sensorDistance);

            _panelTravelDistance = panelTravelDistance;
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

            Collider[] leftColliders =
                leftPanel.GetComponentsInChildren<Collider>(true);
            Collider[] rightColliders =
                rightPanel.GetComponentsInChildren<Collider>(true);
            _panelColliders = new Collider[
                leftColliders.Length + rightColliders.Length];
            leftColliders.CopyTo(_panelColliders, 0);
            rightColliders.CopyTo(
                _panelColliders,
                leftColliders.Length);
            SetPanelCollidersEnabled(true);
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
                SetPanelCollidersEnabled(!_isOpen);
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
                    Time.deltaTime);

            _rightPanel.localPosition =
                Vector3.MoveTowards(
                    _rightPanel.localPosition,
                    rightTarget,
                    _speed *
                    Time.deltaTime);
        }


        private void SetPanelCollidersEnabled(bool enabled)
        {
            foreach (Collider panelCollider in _panelColliders)
            {
                if (panelCollider != null)
                {
                    panelCollider.enabled = enabled;
                }
            }
        }

        private bool HasCharacterNearby()
        {
            return _sensor != null && _sensor.HasCharacter;
        }
    }
}
