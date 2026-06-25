using System;

namespace VRMGames.CartridgeAndCloud.Domain.Displays
{
    public readonly struct DisplayInstanceId : IEquatable<DisplayInstanceId>
    {
        public string Value { get; }

        public DisplayInstanceId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "Display instance ID cannot be empty.",
                    nameof(value));
            }

            Value = value;
        }

        public bool Equals(DisplayInstanceId other)
        {
            return string.Equals(Value, other.Value, StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is DisplayInstanceId other && Equals(other);
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

        public static bool operator ==(
            DisplayInstanceId left,
            DisplayInstanceId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(
            DisplayInstanceId left,
            DisplayInstanceId right)
        {
            return !left.Equals(right);
        }
    }
}
