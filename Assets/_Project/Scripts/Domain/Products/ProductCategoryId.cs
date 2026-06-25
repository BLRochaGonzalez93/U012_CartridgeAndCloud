using System;

namespace VRMGames.CartridgeAndCloud.Domain.Products
{
    public readonly struct ProductCategoryId :
        IEquatable<ProductCategoryId>
    {
        public string Value { get; }

        public ProductCategoryId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "Product category ID cannot be empty.",
                    nameof(value));
            }

            Value = value;
        }

        public bool Equals(ProductCategoryId other)
        {
            return string.Equals(
                Value,
                other.Value,
                StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is ProductCategoryId other &&
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
            ProductCategoryId left,
            ProductCategoryId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(
            ProductCategoryId left,
            ProductCategoryId right)
        {
            return !left.Equals(right);
        }
    }
}
