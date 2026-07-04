using VRMGames.CartridgeAndCloud.Domain.Grid;

namespace VRMGames.CartridgeAndCloud.Application.Placement
{
    public static class PlacementPreviewCalculator
    {
        public static PlacementPreviewState Calculate(
            GridCoordinate anchor,
            GridSize baseSize,
            GridRotation rotation,
            GridBounds bounds)
        {
            GridRotationExtensions.Validate(rotation);

            GridFootprint footprint = new GridFootprint(baseSize);
            GridSize orientedSize = footprint.GetOrientedSize(rotation);

            bool isWithinBounds = bounds.ContainsFootprint(
                anchor,
                footprint,
                rotation);

            return new PlacementPreviewState(
                anchor,
                rotation,
                orientedSize,
                (orientedSize.Width - 1) * 0.5f,
                (orientedSize.Depth - 1) * 0.5f,
                isWithinBounds);
        }
    }

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
