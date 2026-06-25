using System;

namespace VRMGames.CartridgeAndCloud.Domain.Products
{
    public readonly struct ProductDefinitionId :
        IEquatable<ProductDefinitionId>
    {
        public string Value { get; }

        public ProductDefinitionId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "Product definition ID cannot be empty.",
                    nameof(value));
            }

            Value = value;
        }

        public bool Equals(ProductDefinitionId other)
        {
            return string.Equals(
                Value,
                other.Value,
                StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is ProductDefinitionId other &&
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
            ProductDefinitionId left,
            ProductDefinitionId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(
            ProductDefinitionId left,
            ProductDefinitionId right)
        {
            return !left.Equals(right);
        }
    }
}
