using System;

namespace VRMGames.CartridgeAndCloud.Domain.Grid
{
    public readonly struct GridSize :
        IEquatable<GridSize>
    {
        public int Width { get; }
        public int Depth { get; }

        public int Area => Width * Depth;

        public GridSize(
            int width,
            int depth)
        {
            if (width <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(width),
                    width,
                    "Grid width must be greater than zero.");
            }

            if (depth <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(depth),
                    depth,
                    "Grid depth must be greater than zero.");
            }

            Width = width;
            Depth = depth;
        }

        public GridSize GetOriented(
            GridRotation rotation)
        {
            GridRotationExtensions.Validate(rotation);

            bool swapsAxes =
                rotation == GridRotation.Degrees90 ||
                rotation == GridRotation.Degrees270;

            return swapsAxes
                ? new GridSize(Depth, Width)
                : this;
        }

        public bool Equals(GridSize other)
        {
            return Width == other.Width &&
                   Depth == other.Depth;
        }

        public override bool Equals(object obj)
        {
            return obj is GridSize other &&
                   Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Width * 397) ^ Depth;
            }
        }

        public override string ToString()
        {
            return $"{Width}x{Depth}";
        }

        public static bool operator ==(
            GridSize left,
            GridSize right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(
            GridSize left,
            GridSize right)
        {
            return !left.Equals(right);
        }
    }
}
