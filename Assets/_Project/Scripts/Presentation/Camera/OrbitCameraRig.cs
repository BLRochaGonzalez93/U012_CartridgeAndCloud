using UnityEngine;
using VRMGames.CartridgeAndCloud.Application.Camera;

namespace VRMGames.CartridgeAndCloud.Presentation.Camera
{
    [RequireComponent(typeof(UnityEngine.Camera))]
    public sealed class OrbitCameraRig : MonoBehaviour
    {
        [SerializeField]
        private Transform _target;

        [SerializeField]
        private float _yawDegrees;

        [SerializeField]
        private float _pitchDegrees = 45f;

        [SerializeField]
        private float _distance = 18f;

        [SerializeField]
        private float _minimumPitchDegrees = 25f;

        [SerializeField]
        private float _maximumPitchDegrees = 75f;

        [SerializeField]
        private float _minimumDistance = 6f;

        [SerializeField]
        private float _maximumDistance = 24f;

        private OrbitCameraState _state;
        private bool _stateInitialized;

        public Transform Target => _target;

        public OrbitCameraState CurrentState
        {
            get
            {
                EnsureState();
                return _state;
            }
        }

        private void Awake()
        {
            EnsureState();
        }

        private void LateUpdate()
        {
            ApplyPose();
        }

        public void Configure(
            Transform target,
            float yawDegrees,
            float pitchDegrees,
            float distance,
            OrbitCameraConstraints constraints)
        {
            _target = target;
            _minimumPitchDegrees =
                constraints.MinimumPitchDegrees;
            _maximumPitchDegrees =
                constraints.MaximumPitchDegrees;
            _minimumDistance =
                constraints.MinimumDistance;
            _maximumDistance =
                constraints.MaximumDistance;

            _state = OrbitCameraCalculator.CreateState(
                yawDegrees,
                pitchDegrees,
                distance,
                constraints);

            _stateInitialized = true;
            SynchronizeSerializedState();
            ApplyPose();
        }

        public void SetTarget(Transform target)
        {
            _target = target;
            ApplyPose();
        }

        public void ApplyOrbitInput(
            float yawDeltaDegrees,
            float pitchDeltaDegrees)
        {
            EnsureState();

            _state = OrbitCameraCalculator.ApplyOrbit(
                _state,
                yawDeltaDegrees,
                pitchDeltaDegrees,
                CreateConstraints());

            SynchronizeSerializedState();
            ApplyPose();
        }

        public void ApplyZoomInput(float zoomDelta)
        {
            EnsureState();

            _state = OrbitCameraCalculator.ApplyZoom(
                _state,
                zoomDelta,
                CreateConstraints());

            SynchronizeSerializedState();
            ApplyPose();
        }

        public void SnapToTarget()
        {
            EnsureState();
            ApplyPose();
        }

        private void ApplyPose()
        {
            if (_target == null)
            {
                return;
            }

            EnsureState();

            Quaternion rotation = Quaternion.Euler(
                _state.PitchDegrees,
                _state.YawDegrees,
                0f);

            Vector3 focusPosition = _target.position;
            Vector3 cameraPosition =
                focusPosition +
                rotation * Vector3.back * _state.Distance;

            transform.SetPositionAndRotation(
                cameraPosition,
                rotation);
        }

        private void EnsureState()
        {
            if (_stateInitialized)
            {
                return;
            }

            _state = OrbitCameraCalculator.CreateState(
                _yawDegrees,
                _pitchDegrees,
                _distance,
                CreateConstraints());

            _stateInitialized = true;
            SynchronizeSerializedState();
        }

        private OrbitCameraConstraints CreateConstraints()
        {
            return new OrbitCameraConstraints(
                _minimumPitchDegrees,
                _maximumPitchDegrees,
                _minimumDistance,
                _maximumDistance);
        }

        private void SynchronizeSerializedState()
        {
            _yawDegrees = _state.YawDegrees;
            _pitchDegrees = _state.PitchDegrees;
            _distance = _state.Distance;
        }
    }
}
