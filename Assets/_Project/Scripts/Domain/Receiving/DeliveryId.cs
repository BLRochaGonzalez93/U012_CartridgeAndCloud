using System;

namespace VRMGames.CartridgeAndCloud.Domain.Receiving
{
    public readonly struct DeliveryId :
        IEquatable<DeliveryId>
    {
        public string Value { get; }

        public DeliveryId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "Delivery ID cannot be empty.",
                    nameof(value));
            }

            Value = value;
        }

        public bool Equals(DeliveryId other)
        {
            return string.Equals(
                Value,
                other.Value,
                StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is DeliveryId other &&
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
            DeliveryId left,
            DeliveryId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(
            DeliveryId left,
            DeliveryId right)
        {
            return !left.Equals(right);
        }
    }
}
