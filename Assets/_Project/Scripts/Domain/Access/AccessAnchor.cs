using System;
using VRMGames.CartridgeAndCloud.Domain.Grid;

namespace VRMGames.CartridgeAndCloud.Domain.Access
{
    public sealed class AccessAnchor
    {
        public AccessAnchorId Id { get; }
        public GridCoordinate Cell { get; }

        public AccessAnchor(
            AccessAnchorId id,
            GridCoordinate cell)
        {
            if (string.IsNullOrWhiteSpace(
                    id.Value))
            {
                throw new ArgumentException(
                    "Access anchor ID cannot be empty.",
                    nameof(id));
            }

            Id = id;
            Cell = cell;
        }
    }

    public readonly struct AccessAnchorId :
        IEquatable<AccessAnchorId>
    {
        public string Value { get; }

        public AccessAnchorId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "Access anchor ID cannot be empty.",
                    nameof(value));
            }

            Value = value;
        }

        public bool Equals(AccessAnchorId other)
        {
            return string.Equals(
                Value,
                other.Value,
                StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is AccessAnchorId other &&
                   Equals(other);
        }

        public override int GetHashCode()
        {
            return StringComparer.Ordinal.GetHashCode(
                Value);
        }

        public override string ToString()
        {
            return Value;
        }

        public static bool operator ==(
            AccessAnchorId left,
            AccessAnchorId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(
            AccessAnchorId left,
            AccessAnchorId right)
        {
            return !left.Equals(right);
        }
    }
}
