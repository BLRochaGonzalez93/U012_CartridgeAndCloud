using System;

namespace VRMGames.CartridgeAndCloud.Domain.Displays
{
    public readonly struct RestockTaskId : IEquatable<RestockTaskId>
    {
        public string Value { get; }

        public RestockTaskId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "Restock task ID cannot be empty.",
                    nameof(value));
            }

            Value = value;
        }

        public bool Equals(RestockTaskId other)
        {
            return string.Equals(Value, other.Value, StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is RestockTaskId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value == null
                ? 0
                : StringComparer.Ordinal.GetHashCode(Value);
        }

        public override string ToString()
        {
            return Value ?? string.Empty;
        }

        public static bool operator ==(RestockTaskId left, RestockTaskId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RestockTaskId left, RestockTaskId right)
        {
            return !left.Equals(right);
        }
    }
}
