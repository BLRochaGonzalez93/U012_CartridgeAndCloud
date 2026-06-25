using System;

namespace VRMGames.CartridgeAndCloud.Domain.Grid
{
    public readonly struct GridCoordinate :
        IEquatable<GridCoordinate>
    {
        public int X { get; }
        public int Z { get; }

        public GridCoordinate(int x, int z)
        {
            X = x;
            Z = z;
        }

        public GridCoordinate Offset(
            int deltaX,
            int deltaZ)
        {
            return new GridCoordinate(
                X + deltaX,
                Z + deltaZ);
        }

        public bool Equals(GridCoordinate other)
        {
            return X == other.X &&
                   Z == other.Z;
        }

        public override bool Equals(object obj)
        {
            return obj is GridCoordinate other &&
                   Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Z;
            }
        }

        public override string ToString()
        {
            return $"({X}, {Z})";
        }

        public static bool operator ==(
            GridCoordinate left,
            GridCoordinate right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(
            GridCoordinate left,
            GridCoordinate right)
        {
            return !left.Equals(right);
        }
    }
}
