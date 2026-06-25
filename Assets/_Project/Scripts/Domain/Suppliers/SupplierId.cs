using System;

namespace VRMGames.CartridgeAndCloud.Domain.Suppliers
{
    public readonly struct SupplierId :
        IEquatable<SupplierId>
    {
        public string Value { get; }

        public SupplierId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "Supplier ID cannot be empty.",
                    nameof(value));
            }

            Value = value;
        }

        public bool Equals(SupplierId other)
        {
            return string.Equals(
                Value,
                other.Value,
                StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is SupplierId other &&
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
            SupplierId left,
            SupplierId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(
            SupplierId left,
            SupplierId right)
        {
            return !left.Equals(right);
        }
    }
}
