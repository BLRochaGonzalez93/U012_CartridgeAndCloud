namespace VRMGames.CartridgeAndCloud.Application.PlayerMovement
{
    public readonly struct PlanarMovementStep
    {
        public float DeltaX { get; }
        public float DeltaZ { get; }
        public bool IsWithinStoppingDistance { get; }

        public bool HasMovement =>
            DeltaX != 0f ||
            DeltaZ != 0f;

        public PlanarMovementStep(
            float deltaX,
            float deltaZ,
            bool isWithinStoppingDistance)
        {
            DeltaX = deltaX;
            DeltaZ = deltaZ;
            IsWithinStoppingDistance = isWithinStoppingDistance;
        }
    }
}
