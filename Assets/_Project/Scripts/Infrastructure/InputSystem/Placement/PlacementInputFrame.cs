using UnityEngine;

namespace VRMGames.CartridgeAndCloud.Infrastructure.InputSystem.Placement
{
    public readonly struct PlacementInputFrame
    {
        public Vector2 PointerPosition { get; }
        public bool ToggleModePressed { get; }
        public bool ConfirmPressed { get; }
        public bool RotateCounterClockwisePressed { get; }
        public bool RotateClockwisePressed { get; }
        public bool CancelPressed { get; }
        public bool RemovePressed { get; }

        public PlacementInputFrame(
            Vector2 pointerPosition,
            bool rotateCounterClockwisePressed,
            bool rotateClockwisePressed)
            : this(
                pointerPosition,
                toggleModePressed: false,
                confirmPressed: false,
                rotateCounterClockwisePressed:
                    rotateCounterClockwisePressed,
                rotateClockwisePressed:
                    rotateClockwisePressed,
                cancelPressed: false,
                removePressed: false)
        {
        }

        public PlacementInputFrame(
            Vector2 pointerPosition,
            bool toggleModePressed,
            bool confirmPressed,
            bool rotateCounterClockwisePressed,
            bool rotateClockwisePressed,
            bool cancelPressed,
            bool removePressed)
        {
            PointerPosition = pointerPosition;
            ToggleModePressed = toggleModePressed;
            ConfirmPressed = confirmPressed;
            RotateCounterClockwisePressed =
                rotateCounterClockwisePressed;
            RotateClockwisePressed =
                rotateClockwisePressed;
            CancelPressed = cancelPressed;
            RemovePressed = removePressed;
        }
    }
}
