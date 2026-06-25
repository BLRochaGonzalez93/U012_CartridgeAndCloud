using System;

namespace VRMGames.CartridgeAndCloud.Domain.Placement
{
    public readonly struct PlacementInstanceId :
        IEquatable<PlacementInstanceId>
    {
        public string Value { get; }

        public PlacementInstanceId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "Placement ID cannot be empty.",
                    nameof(value));
            }

            Value = value;
        }

        public bool Equals(PlacementInstanceId other)
        {
            return string.Equals(
                Value,
                other.Value,
                StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is PlacementInstanceId other &&
                   Equals(other);
        }

        public override int GetHashCode()
        {
            return StringComparer.Ordinal.GetHashCode(Value);
        }

        public override string ToString()
        {
            return Value;
        }

        public static bool operator ==(
            PlacementInstanceId left,
            PlacementInstanceId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(
            PlacementInstanceId left,
            PlacementInstanceId right)
        {
            return !left.Equals(right);
        }
    }
}
