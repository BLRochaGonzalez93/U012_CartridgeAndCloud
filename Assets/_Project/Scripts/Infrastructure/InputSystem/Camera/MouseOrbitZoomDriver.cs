using UnityEngine;
using VRMGames.CartridgeAndCloud.Application.InputContexts;
using VRMGames.CartridgeAndCloud.Presentation.Camera;

namespace VRMGames.CartridgeAndCloud.Infrastructure.InputSystem.Camera
{
    [RequireComponent(typeof(OrbitCameraRig))]
    public sealed class MouseOrbitZoomDriver :
        MonoBehaviour,
        IInputContextConsumer
    {
        [SerializeField, Min(0f)]
        private float _orbitSensitivity = 0.2f;

        [SerializeField, Min(0f)]
        private float _zoomSensitivity = 0.5f;

        [SerializeField]
        private bool _allowStandaloneGameplay;

        private OrbitCameraRig _cameraRig;
        private IInputContextService _inputContextService;

        public bool IsInputEnabled =>
            _inputContextService != null
                ? _inputContextService.CurrentContext ==
                  InputContextId.Gameplay
                : _allowStandaloneGameplay;

        private void Awake()
        {
            _cameraRig = GetComponent<OrbitCameraRig>();
        }

        public void Initialize(
            IInputContextService inputContextService)
        {
            _inputContextService = inputContextService;
        }

        public void Configure(
            float orbitSensitivity,
            float zoomSensitivity,
            bool allowStandaloneGameplay)
        {
            _orbitSensitivity =
                Mathf.Max(0f, orbitSensitivity);

            _zoomSensitivity =
                Mathf.Max(0f, zoomSensitivity);

            _allowStandaloneGameplay =
                allowStandaloneGameplay;
        }

        public bool ApplyOrbitDelta(Vector2 pointerDelta)
        {
            if (!IsInputEnabled)
            {
                return false;
            }

            EnsureRig();

            _cameraRig.ApplyOrbitInput(
                pointerDelta.x * _orbitSensitivity,
                -pointerDelta.y * _orbitSensitivity);

            return true;
        }

        public bool ApplyZoomDelta(float scrollDelta)
        {
            if (!IsInputEnabled)
            {
                return false;
            }

            EnsureRig();

            _cameraRig.ApplyZoomInput(
                scrollDelta * _zoomSensitivity);

            return true;
        }

        private void EnsureRig()
        {
            if (_cameraRig == null)
            {
                _cameraRig =
                    GetComponent<OrbitCameraRig>();
            }
        }
    }
}
