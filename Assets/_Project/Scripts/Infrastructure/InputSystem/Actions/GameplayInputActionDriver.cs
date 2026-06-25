using UnityEngine;
using UnityEngine.InputSystem;
using VRMGames.CartridgeAndCloud.Presentation.Camera;
using VRMGames.CartridgeAndCloud.Presentation.PlayerMovement;

namespace VRMGames.CartridgeAndCloud.Infrastructure.InputSystem.Actions
{
    public sealed class GameplayInputActionDriver :
        MonoBehaviour
    {
        [SerializeField]
        private InputActionContextRouter _contextRouter;

        [SerializeField]
        private ClickDestinationInput _destinationInput;

        [SerializeField]
        private OrbitCameraRig _cameraRig;

        [SerializeField, Min(0f)]
        private float _orbitSensitivity = 0.2f;

        [SerializeField, Min(0f)]
        private float _zoomSensitivity = 0.5f;

        public InputActionContextRouter ContextRouter =>
            _contextRouter;

        public ClickDestinationInput DestinationInput =>
            _destinationInput;

        public OrbitCameraRig CameraRig =>
            _cameraRig;

        public float OrbitSensitivity =>
            _orbitSensitivity;

        public float ZoomSensitivity =>
            _zoomSensitivity;

        private void Awake()
        {
            ResolveReferences();
        }

        private void Update()
        {
            ResolveReferences();

            if (_contextRouter == null ||
                !_contextRouter.IsGameplayMapEnabled)
            {
                return;
            }

            ProjectInputActions actions =
                _contextRouter.Actions;

            GameplayInputFrame frame =
                new GameplayInputFrame(
                    actions.PointerPosition
                        .ReadValue<Vector2>(),
                    actions.SetDestination
                        .WasPressedThisFrame(),
                    actions.OrbitHold.IsPressed(),
                    actions.OrbitDelta
                        .ReadValue<Vector2>(),
                    actions.Zoom
                        .ReadValue<Vector2>().y);

            ApplyFrame(frame);
        }

        public void Configure(
            InputActionContextRouter contextRouter,
            ClickDestinationInput destinationInput,
            OrbitCameraRig cameraRig,
            float orbitSensitivity,
            float zoomSensitivity)
        {
            _contextRouter = contextRouter;
            _destinationInput = destinationInput;
            _cameraRig = cameraRig;
            _orbitSensitivity =
                Mathf.Max(0f, orbitSensitivity);
            _zoomSensitivity =
                Mathf.Max(0f, zoomSensitivity);
        }

        public bool ApplyFrame(
            GameplayInputFrame frame)
        {
            ResolveReferences();

            if (_contextRouter == null ||
                !_contextRouter.IsGameplayMapEnabled)
            {
                return false;
            }

            bool handled = false;

            if (frame.DestinationPressed &&
                _destinationInput != null)
            {
                handled |=
                    _destinationInput
                        .TrySetDestinationFromScreenPosition(
                            frame.PointerPosition);
            }

            if (frame.OrbitHeld &&
                frame.OrbitDelta.sqrMagnitude >
                0.000001f &&
                _cameraRig != null)
            {
                _cameraRig.ApplyOrbitInput(
                    frame.OrbitDelta.x *
                    _orbitSensitivity,
                    -frame.OrbitDelta.y *
                    _orbitSensitivity);

                handled = true;
            }

            if (Mathf.Abs(frame.ZoomDelta) >
                0.001f &&
                _cameraRig != null)
            {
                _cameraRig.ApplyZoomInput(
                    frame.ZoomDelta *
                    _zoomSensitivity);

                handled = true;
            }

            return handled;
        }

        private void ResolveReferences()
        {
            if (_contextRouter == null)
            {
                _contextRouter =
                    Object.FindFirstObjectByType<
                        InputActionContextRouter>();

                if (_contextRouter == null)
                {
                    _contextRouter =
                        gameObject.AddComponent<
                            InputActionContextRouter>();

                    _contextRouter.Configure(
                        allowStandaloneGameplay: true);
                }
            }

            if (_destinationInput == null)
            {
                _destinationInput =
                    Object.FindFirstObjectByType<
                        ClickDestinationInput>();
            }

            if (_cameraRig == null)
            {
                _cameraRig =
                    GetComponent<OrbitCameraRig>();
            }
        }
    }
}
