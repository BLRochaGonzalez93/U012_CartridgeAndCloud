using System;

namespace VRMGames.CartridgeAndCloud.Domain.Access
{
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
