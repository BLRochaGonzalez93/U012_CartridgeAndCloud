using UnityEngine;

namespace VRMGames.CartridgeAndCloud.Infrastructure.InputSystem.Placement
{
    public readonly struct PlacementInputFrame
    {
        public Vector2 PointerPosition { get; }
        public bool RotateCounterClockwisePressed { get; }
        public bool RotateClockwisePressed { get; }

        public PlacementInputFrame(
            Vector2 pointerPosition,
            bool rotateCounterClockwisePressed,
            bool rotateClockwisePressed)
        {
            PointerPosition = pointerPosition;
            RotateCounterClockwisePressed =
                rotateCounterClockwisePressed;
            RotateClockwisePressed =
                rotateClockwisePressed;
        }
    }
}
