using VRMGames.CartridgeAndCloud.Domain.Grid;

namespace VRMGames.CartridgeAndCloud.Application.Placement
{
    public readonly struct PlacementPreviewState
    {
        public GridCoordinate Anchor { get; }
        public GridRotation Rotation { get; }
        public GridSize OrientedSize { get; }
        public float CenterOffsetCellsX { get; }
        public float CenterOffsetCellsZ { get; }
        public bool IsWithinBounds { get; }

        public PlacementPreviewState(
            GridCoordinate anchor,
            GridRotation rotation,
            GridSize orientedSize,
            float centerOffsetCellsX,
            float centerOffsetCellsZ,
            bool isWithinBounds)
        {
            Anchor = anchor;
            Rotation = rotation;
            OrientedSize = orientedSize;
            CenterOffsetCellsX = centerOffsetCellsX;
            CenterOffsetCellsZ = centerOffsetCellsZ;
            IsWithinBounds = isWithinBounds;
        }
    }
}
