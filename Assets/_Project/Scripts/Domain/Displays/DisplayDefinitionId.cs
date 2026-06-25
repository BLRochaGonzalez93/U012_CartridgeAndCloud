using System;

namespace VRMGames.CartridgeAndCloud.Domain.Displays
{
    public readonly struct DisplayDefinitionId : IEquatable<DisplayDefinitionId>
    {
        public string Value { get; }

        public DisplayDefinitionId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "Display definition ID cannot be empty.",
                    nameof(value));
            }

            Value = value;
        }

        public bool Equals(DisplayDefinitionId other)
        {
            return string.Equals(Value, other.Value, StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is DisplayDefinitionId other && Equals(other);
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
            DisplayDefinitionId left,
            DisplayDefinitionId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(
            DisplayDefinitionId left,
            DisplayDefinitionId right)
        {
            return !left.Equals(right);
        }
    }
}
