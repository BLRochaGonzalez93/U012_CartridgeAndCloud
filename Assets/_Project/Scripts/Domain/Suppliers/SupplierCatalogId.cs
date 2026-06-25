using System;

namespace VRMGames.CartridgeAndCloud.Domain.Suppliers
{
    public readonly struct SupplierCatalogId :
        IEquatable<SupplierCatalogId>
    {
        public string Value { get; }

        public SupplierCatalogId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "Supplier catalog ID cannot be empty.",
                    nameof(value));
            }

            Value = value;
        }

        public bool Equals(SupplierCatalogId other)
        {
            return string.Equals(
                Value,
                other.Value,
                StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is SupplierCatalogId other &&
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
            SupplierCatalogId left,
            SupplierCatalogId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(
            SupplierCatalogId left,
            SupplierCatalogId right)
        {
            return !left.Equals(right);
        }
    }
}
