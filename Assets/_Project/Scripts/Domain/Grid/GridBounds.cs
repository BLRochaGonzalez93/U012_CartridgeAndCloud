using System;

namespace VRMGames.CartridgeAndCloud.Domain.Grid
{
    public readonly struct GridBounds
    {
        public GridCoordinate Minimum { get; }
        public GridSize Size { get; }

        public int MaximumXExclusive =>
            Minimum.X + Size.Width;

        public int MaximumZExclusive =>
            Minimum.Z + Size.Depth;

        public GridBounds(
            GridCoordinate minimum,
            GridSize size)
        {
            Minimum = minimum;
            Size = size;
        }

        public bool Contains(
            GridCoordinate cell)
        {
            return cell.X >= Minimum.X &&
                   cell.Z >= Minimum.Z &&
                   (long)cell.X <
                       (long)Minimum.X + Size.Width &&
                   (long)cell.Z <
                       (long)Minimum.Z + Size.Depth;
        }

        public bool ContainsFootprint(
            GridCoordinate anchor,
            GridFootprint footprint,
            GridRotation rotation)
        {
            if (footprint == null)
            {
                throw new ArgumentNullException(
                    nameof(footprint));
            }

            GridSize orientedSize =
                footprint.GetOrientedSize(rotation);

            long footprintMaximumX =
                (long)anchor.X +
                orientedSize.Width;

            long footprintMaximumZ =
                (long)anchor.Z +
                orientedSize.Depth;

            return anchor.X >= Minimum.X &&
                   anchor.Z >= Minimum.Z &&
                   footprintMaximumX <=
                       (long)Minimum.X + Size.Width &&
                   footprintMaximumZ <=
                       (long)Minimum.Z + Size.Depth;
        }
    }
}
