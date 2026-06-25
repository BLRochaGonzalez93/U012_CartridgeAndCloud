using System;

namespace VRMGames.CartridgeAndCloud.Domain.Receiving
{
    public readonly struct ShipmentBoxId :
        IEquatable<ShipmentBoxId>
    {
        public string Value { get; }

        public ShipmentBoxId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "Shipment box ID cannot be empty.",
                    nameof(value));
            }

            Value = value;
        }

        public bool Equals(ShipmentBoxId other)
        {
            return string.Equals(
                Value,
                other.Value,
                StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is ShipmentBoxId other &&
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
            ShipmentBoxId left,
            ShipmentBoxId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(
            ShipmentBoxId left,
            ShipmentBoxId right)
        {
            return !left.Equals(right);
        }
    }
}
