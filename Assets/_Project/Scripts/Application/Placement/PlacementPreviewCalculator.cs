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
}
