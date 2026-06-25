using UnityEngine;
using VRMGames.CartridgeAndCloud.Infrastructure.InputSystem.Actions;
using VRMGames.CartridgeAndCloud.Presentation.Placement;

namespace VRMGames.CartridgeAndCloud.Infrastructure.InputSystem.Placement
{
    public sealed class PlacementInputActionDriver : MonoBehaviour
    {
        [SerializeField]
        private InputActionContextRouter _contextRouter;

        [SerializeField]
        private PlacementPreviewController _previewController;

        public InputActionContextRouter ContextRouter => _contextRouter;
        public PlacementPreviewController PreviewController => _previewController;

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
                _previewController?.ClearPreview();
                return;
            }

            ProjectInputActions actions = _contextRouter.Actions;

            ApplyFrame(
                new PlacementInputFrame(
                    actions.PointerPosition.ReadValue<Vector2>(),
                    actions.RotatePlacementCounterClockwise.WasPressedThisFrame(),
                    actions.RotatePlacementClockwise.WasPressedThisFrame()));
        }

        public void Configure(
            InputActionContextRouter contextRouter,
            PlacementPreviewController previewController)
        {
            _contextRouter = contextRouter;
            _previewController = previewController;
        }

        public bool ApplyFrame(PlacementInputFrame frame)
        {
            ResolveReferences();

            if (_contextRouter == null ||
                !_contextRouter.IsGameplayMapEnabled ||
                _previewController == null)
            {
                return false;
            }

            bool handled =
                _previewController.TryUpdatePreviewFromScreenPosition(
                    frame.PointerPosition);

            if (frame.RotateCounterClockwisePressed !=
                frame.RotateClockwisePressed)
            {
                if (frame.RotateCounterClockwisePressed)
                {
                    _previewController.RotateCounterClockwise();
                }
                else
                {
                    _previewController.RotateClockwise();
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
                    Object.FindFirstObjectByType<InputActionContextRouter>();

                if (_contextRouter == null)
                {
                    _contextRouter =
                        gameObject.AddComponent<InputActionContextRouter>();

                    _contextRouter.Configure(
                        allowStandaloneGameplay: true);
                }
            }

            if (_previewController == null)
            {
                _previewController =
                    GetComponent<PlacementPreviewController>();

                if (_previewController == null)
                {
                    _previewController =
                        Object.FindFirstObjectByType<PlacementPreviewController>();
                }
            }
        }
    }
}
