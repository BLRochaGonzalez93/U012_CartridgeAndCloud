namespace VRMGames.CartridgeAndCloud.Application.Camera
{
    public readonly struct OrbitCameraState
    {
        public float YawDegrees { get; }
        public float PitchDegrees { get; }
        public float Distance { get; }

        public OrbitCameraState(
            float yawDegrees,
            float pitchDegrees,
            float distance)
        {
            YawDegrees = yawDegrees;
            PitchDegrees = pitchDegrees;
            Distance = distance;
        }
    }
}
