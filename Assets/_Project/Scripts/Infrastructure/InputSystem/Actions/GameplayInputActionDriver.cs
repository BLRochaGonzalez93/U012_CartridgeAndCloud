using UnityEngine;
using UnityEngine.InputSystem;
using VRMGames.CartridgeAndCloud.Application.Placement;
using VRMGames.CartridgeAndCloud.Presentation.Camera;
using VRMGames.CartridgeAndCloud.Presentation.Placement;
using VRMGames.CartridgeAndCloud.Presentation.PlayerMovement;
using VRMGames.CartridgeAndCloud.Presentation.Interaction;
using VRMGames.CartridgeAndCloud.Presentation.PlayerAgency;
using VRMGames.CartridgeAndCloud.Infrastructure.Store;

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

        [SerializeField]
        private PlayerWorldInteractionController
            _interactionController;

        [SerializeField]
        private PlacementRuntimeController _placementRuntimeController;

        [SerializeField]
        private PlayerToolWheelInputBridge _toolWheelInputBridge;

        [SerializeField]
        private PlayerMovementInputBridge _playerMovementInputBridge;

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

        public PlayerWorldInteractionController
            InteractionController =>
                _interactionController;

        public PlacementRuntimeController PlacementRuntimeController =>
            _placementRuntimeController;

        public float OrbitSensitivity =>
            _orbitSensitivity;

        public float ZoomSensitivity =>
            _zoomSensitivity;

        private void Awake()
        {
            ApplyCentralSettings();
            ResolveReferences();
        }

        private void OnDisable()
        {
            _playerMovementInputBridge?
                .SetSprintRequested(false);
        }

        private void Update()
        {
            ResolveReferences();

            if (_contextRouter == null ||
                !_contextRouter.IsGameplayMapEnabled)
            {
                _playerMovementInputBridge?
                    .SetSprintRequested(false);
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
                    actions.Interact
                        .WasPressedThisFrame(),
                    actions.Sprint.IsPressed(),
                    actions.ToolWheel
                        .WasPressedThisFrame(),
                    actions.ToolWheel.IsPressed(),
                    actions.ToolWheel
                        .WasReleasedThisFrame(),
                    actions.ToolWheelNavigate
                        .ReadValue<Vector2>(),
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
            PlayerWorldInteractionController
                interactionController,
            float orbitSensitivity,
            float zoomSensitivity)
        {
            _contextRouter = contextRouter;
            _destinationInput = destinationInput;
            _cameraRig = cameraRig;
            _interactionController =
                interactionController;
            _orbitSensitivity =
                Mathf.Max(0f, orbitSensitivity);
            _zoomSensitivity =
                Mathf.Max(0f, zoomSensitivity);
        }


        public void ApplySettings(
            StoreRuntimeSettingsAsset settings)
        {
            if (settings == null)
            {
                return;
            }

            _orbitSensitivity =
                settings.CameraOrbitSensitivity;

            _zoomSensitivity =
                settings.CameraZoomSensitivity;
        }

        private void ApplyCentralSettings()
        {
            StoreRuntimeAssetRegistry registry =
                StoreRuntimeAssetRegistry
                    .FindLoaded();

            ApplySettings(registry?.Settings);
        }

        public void SetPlacementRuntimeController(
            PlacementRuntimeController placementRuntimeController)
        {
            _placementRuntimeController =
                placementRuntimeController;
        }

        public bool ApplyFrame(
            GameplayInputFrame frame)
        {
            ResolveReferences();

            if (_contextRouter == null ||
                !_contextRouter.IsGameplayMapEnabled)
            {
                _playerMovementInputBridge?
                    .SetSprintRequested(false);
                return false;
            }

            bool handled = false;
            bool placementModeActive =
                _placementRuntimeController != null &&
                _placementRuntimeController
                    .IsPlacementModeActive;

            if (_toolWheelInputBridge != null)
            {
                if (placementModeActive)
                {
                    _toolWheelInputBridge.Cancel();
                }
                else
                {
                    _toolWheelInputBridge.ApplyInput(
                        frame.ToolWheelPressed,
                        frame.ToolWheelHeld,
                        frame.ToolWheelReleased,
                        frame.PointerPosition,
                        frame.ToolWheelNavigation);

                    if (_toolWheelInputBridge.IsOpen ||
                        frame.ToolWheelPressed ||
                        frame.ToolWheelReleased)
                    {
                        _playerMovementInputBridge?
                            .SetSprintRequested(false);
                        _interactionController?.ClearTarget();
                        return true;
                    }
                }
            }

            _playerMovementInputBridge?.SetSprintRequested(
                frame.SprintHeld && !placementModeActive);

            if (_interactionController != null)
            {
                if (placementModeActive)
                {
                    _interactionController.ClearTarget();
                }
                else
                {
                    _interactionController
                        .UpdateTargetFromScreenPosition(
                            frame.PointerPosition);

                    if (frame.InteractionPressed)
                    {
                        _interactionController
                            .TryRequestCurrentInteraction();
                        handled = true;
                    }
                }
            }

            if (GameplayDestinationInputPolicy
                    .ShouldSetDestination(
                        frame.DestinationPressed,
                        placementModeActive) &&
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

            if (_interactionController == null)
            {
                _interactionController =
                    Object.FindFirstObjectByType<
                        PlayerWorldInteractionController>();
            }

            if (_placementRuntimeController == null)
            {
                _placementRuntimeController =
                    Object.FindFirstObjectByType<
                        PlacementRuntimeController>();
            }

            if (_toolWheelInputBridge == null)
            {
                _toolWheelInputBridge =
                    Object.FindFirstObjectByType<
                        PlayerToolWheelInputBridge>();
            }

            if (_playerMovementInputBridge == null)
            {
                _playerMovementInputBridge =
                    Object.FindFirstObjectByType<
                        PlayerMovementInputBridge>();
            }
        }
    }

    public readonly struct GameplayInputFrame
    {
        public Vector2 PointerPosition { get; }
        public bool DestinationPressed { get; }
        public bool InteractionPressed { get; }
        public bool SprintHeld { get; }
        public bool ToolWheelPressed { get; }
        public bool ToolWheelHeld { get; }
        public bool ToolWheelReleased { get; }
        public Vector2 ToolWheelNavigation { get; }
        public bool OrbitHeld { get; }
        public Vector2 OrbitDelta { get; }
        public float ZoomDelta { get; }

        public GameplayInputFrame(
            Vector2 pointerPosition,
            bool destinationPressed,
            bool interactionPressed,
            bool sprintHeld,
            bool toolWheelPressed,
            bool toolWheelHeld,
            bool toolWheelReleased,
            Vector2 toolWheelNavigation,
            bool orbitHeld,
            Vector2 orbitDelta,
            float zoomDelta)
        {
            PointerPosition = pointerPosition;
            DestinationPressed = destinationPressed;
            InteractionPressed = interactionPressed;
            SprintHeld = sprintHeld;
            ToolWheelPressed = toolWheelPressed;
            ToolWheelHeld = toolWheelHeld;
            ToolWheelReleased = toolWheelReleased;
            ToolWheelNavigation = toolWheelNavigation;
            OrbitHeld = orbitHeld;
            OrbitDelta = orbitDelta;
            ZoomDelta = zoomDelta;
        }
    }
}
