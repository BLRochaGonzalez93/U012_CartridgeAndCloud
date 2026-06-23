using System;

namespace VRMGames.CartridgeAndCloud.Domain.Identifiers
{
    public readonly struct StableId : IEquatable<StableId>
    {
        public string Value { get; }

        public StableId(string value)
        {
            if (string.IsNullOrWhiteSpace(value) ||
                !Guid.TryParseExact(value, "N", out _))
            {
                throw new ArgumentException(
                    "StableId must be a non-empty 32-character GUID in N format.",
                    nameof(value));
            }

            Value = value;
        }

        public static StableId New()
        {
            return new StableId(Guid.NewGuid().ToString("N"));
        }

        public static StableId Parse(string value)
        {
            return new StableId(value);
        }

        public bool Equals(StableId other)
        {
            return string.Equals(Value, other.Value, StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is StableId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value == null ? 0 : StringComparer.Ordinal.GetHashCode(Value);
        }

        public override string ToString()
        {
            return Value ?? string.Empty;
        }

        public static bool operator ==(StableId left, StableId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(StableId left, StableId right)
        {
            return !left.Equals(right);
        }
    }
}
