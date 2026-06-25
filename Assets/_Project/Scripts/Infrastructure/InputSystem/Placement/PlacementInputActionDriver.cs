using UnityEngine;
using VRMGames.CartridgeAndCloud.Infrastructure.InputSystem.Actions;
using VRMGames.CartridgeAndCloud.Presentation.Placement;

namespace VRMGames.CartridgeAndCloud.Infrastructure.InputSystem.Placement
{
    public sealed class PlacementInputActionDriver :
        MonoBehaviour
    {
        [SerializeField]
        private InputActionContextRouter _contextRouter;

        [SerializeField]
        private PlacementPreviewController _previewController;

        [SerializeField]
        private PlacementRuntimeController _runtimeController;

        public InputActionContextRouter ContextRouter =>
            _contextRouter;

        public PlacementPreviewController PreviewController =>
            _previewController;

        public PlacementRuntimeController RuntimeController =>
            _runtimeController;

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
                _runtimeController?.CancelPlacement();
                _previewController?.ClearPreview();
                return;
            }

            ProjectInputActions actions =
                _contextRouter.Actions;

            ApplyFrame(
                new PlacementInputFrame(
                    actions.PointerPosition
                        .ReadValue<Vector2>(),
                    actions.TogglePlacementMode
                        .WasPressedThisFrame(),
                    actions.SetDestination
                        .WasPressedThisFrame(),
                    actions
                        .RotatePlacementCounterClockwise
                        .WasPressedThisFrame(),
                    actions
                        .RotatePlacementClockwise
                        .WasPressedThisFrame(),
                    actions.CancelPlacement
                        .WasPressedThisFrame(),
                    actions.RemovePlacement
                        .WasPressedThisFrame()));
        }

        public void Configure(
            InputActionContextRouter contextRouter,
            PlacementPreviewController previewController)
        {
            Configure(
                contextRouter,
                previewController,
                runtimeController: null);
        }

        public void Configure(
            InputActionContextRouter contextRouter,
            PlacementPreviewController previewController,
            PlacementRuntimeController runtimeController)
        {
            _contextRouter = contextRouter;
            _previewController = previewController;
            _runtimeController = runtimeController;
        }

        public bool ApplyFrame(
            PlacementInputFrame frame)
        {
            ResolveReferences();

            if (_contextRouter == null ||
                !_contextRouter.IsGameplayMapEnabled ||
                _previewController == null)
            {
                return false;
            }

            if (_runtimeController == null)
            {
                return ApplyLegacyPreviewFrame(frame);
            }

            bool handled = false;

            if (frame.ToggleModePressed)
            {
                _runtimeController
                    .TogglePlacementMode();

                handled = true;
            }

            if (frame.RemovePressed)
            {
                handled |=
                    _runtimeController
                        .TryRemoveAtScreenPosition(
                            frame.PointerPosition);
            }

            if (frame.CancelPressed)
            {
                _runtimeController
                    .CancelPlacement();

                return true;
            }

            if (!_runtimeController
                    .IsPlacementModeActive)
            {
                return handled;
            }

            handled |=
                _runtimeController
                    .TryUpdatePreviewFromScreenPosition(
                        frame.PointerPosition);

            if (frame.RotateCounterClockwisePressed !=
                frame.RotateClockwisePressed)
            {
                if (frame.RotateCounterClockwisePressed)
                {
                    _runtimeController
                        .RotateCounterClockwise();
                }
                else
                {
                    _runtimeController
                        .RotateClockwise();
                }

                handled = true;
            }

            if (frame.ConfirmPressed)
            {
                handled |=
                    _runtimeController
                        .TryConfirmCurrentPlacement();
            }

            return handled;
        }

        private bool ApplyLegacyPreviewFrame(
            PlacementInputFrame frame)
        {
            bool handled =
                _previewController
                    .TryUpdatePreviewFromScreenPosition(
                        frame.PointerPosition);

            if (frame.RotateCounterClockwisePressed !=
                frame.RotateClockwisePressed)
            {
                if (frame.RotateCounterClockwisePressed)
                {
                    _previewController
                        .RotateCounterClockwise();
                }
                else
                {
                    _previewController
                        .RotateClockwise();
                }

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

            if (_previewController == null)
            {
                _previewController =
                    GetComponent<
                        PlacementPreviewController>();

                if (_previewController == null)
                {
                    _previewController =
                        Object.FindFirstObjectByType<
                            PlacementPreviewController>();
                }
            }

            if (_runtimeController == null)
            {
                _runtimeController =
                    GetComponent<
                        PlacementRuntimeController>();

                if (_runtimeController == null)
                {
                    _runtimeController =
                        Object.FindFirstObjectByType<
                            PlacementRuntimeController>();
                }
            }
        }
    }
}
