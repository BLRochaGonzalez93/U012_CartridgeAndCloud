namespace VRMGames.CartridgeAndCloud.Application.Grid
{
    public readonly struct GridWorldPosition
    {
        public float X { get; }
        public float Z { get; }

        public GridWorldPosition(
            float x,
            float z)
        {
            X = x;
            Z = z;
        }
    }
}
