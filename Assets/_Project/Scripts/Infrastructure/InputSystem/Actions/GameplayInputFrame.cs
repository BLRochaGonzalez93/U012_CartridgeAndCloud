using UnityEngine;

namespace VRMGames.CartridgeAndCloud.Infrastructure.InputSystem.Actions
{
    public readonly struct GameplayInputFrame
    {
        public Vector2 PointerPosition { get; }
        public bool DestinationPressed { get; }
        public bool OrbitHeld { get; }
        public Vector2 OrbitDelta { get; }
        public float ZoomDelta { get; }

        public GameplayInputFrame(
            Vector2 pointerPosition,
            bool destinationPressed,
            bool orbitHeld,
            Vector2 orbitDelta,
            float zoomDelta)
        {
            PointerPosition = pointerPosition;
            DestinationPressed = destinationPressed;
            OrbitHeld = orbitHeld;
            OrbitDelta = orbitDelta;
            ZoomDelta = zoomDelta;
        }
    }
}
