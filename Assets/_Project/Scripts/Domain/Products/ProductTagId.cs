using System;

namespace VRMGames.CartridgeAndCloud.Domain.Products
{
    public readonly struct ProductTagId :
        IEquatable<ProductTagId>
    {
        public string Value { get; }

        public ProductTagId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "Product tag ID cannot be empty.",
                    nameof(value));
            }

            Value = value;
        }

        public bool Equals(ProductTagId other)
        {
            return string.Equals(
                Value,
                other.Value,
                StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is ProductTagId other &&
                   Equals(other);
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
            ProductTagId left,
            ProductTagId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(
            ProductTagId left,
            ProductTagId right)
        {
            return !left.Equals(right);
        }
    }
}
