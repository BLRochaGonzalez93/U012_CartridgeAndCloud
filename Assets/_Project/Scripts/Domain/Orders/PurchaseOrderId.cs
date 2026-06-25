using System;

namespace VRMGames.CartridgeAndCloud.Domain.Orders
{
    public readonly struct PurchaseOrderId :
        IEquatable<PurchaseOrderId>
    {
        public string Value { get; }

        public PurchaseOrderId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "Purchase order ID cannot be empty.",
                    nameof(value));
            }

            Value = value;
        }

        public bool Equals(PurchaseOrderId other)
        {
            return string.Equals(
                Value,
                other.Value,
                StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is PurchaseOrderId other &&
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
            PurchaseOrderId left,
            PurchaseOrderId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(
            PurchaseOrderId left,
            PurchaseOrderId right)
        {
            return !left.Equals(right);
        }
    }
}
