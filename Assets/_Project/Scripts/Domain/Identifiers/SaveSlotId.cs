using System;

namespace VRMGames.CartridgeAndCloud.Domain.Identifiers
{
    public readonly struct SaveSlotId : IEquatable<SaveSlotId>
    {
        public const int MinimumValue = 0;
        public const int MaximumValue = 2;

        public int Value { get; }

        public SaveSlotId(int value)
        {
            if (value < MinimumValue || value > MaximumValue)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(value),
                    value,
                    $"Save slot must be between {MinimumValue} and {MaximumValue}.");
            }

            Value = value;
        }

        public bool Equals(SaveSlotId other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            return obj is SaveSlotId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value;
        }

        public override string ToString()
        {
            return $"Slot {Value}";
        }

        public static bool operator ==(SaveSlotId left, SaveSlotId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SaveSlotId left, SaveSlotId right)
        {
            return !left.Equals(right);
        }
    }
}
