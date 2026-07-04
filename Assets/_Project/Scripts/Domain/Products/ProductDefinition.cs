using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VRMGames.CartridgeAndCloud.Domain.Products
{
    public sealed class ProductDefinition
    {
        private readonly ReadOnlyCollection<ProductTagId> _tags;

        public ProductDefinitionId Id { get; }

        public string DisplayNameKey { get; }

        public ProductCategoryId CategoryId { get; }

        public IReadOnlyList<ProductTagId> Tags => _tags;

        public ProductDefinition(
            ProductDefinitionId id,
            string displayNameKey,
            ProductCategoryId categoryId,
            IEnumerable<ProductTagId> tags)
        {
            if (string.IsNullOrWhiteSpace(id.Value))
            {
                throw new ArgumentException(
                    "Product definition ID must be initialized.",
                    nameof(id));
            }

            if (string.IsNullOrWhiteSpace(displayNameKey))
            {
                throw new ArgumentException(
                    "Display-name key cannot be empty.",
                    nameof(displayNameKey));
            }

            if (string.IsNullOrWhiteSpace(categoryId.Value))
            {
                throw new ArgumentException(
                    "Product category ID must be initialized.",
                    nameof(categoryId));
            }

            if (tags == null)
            {
                throw new ArgumentNullException(nameof(tags));
            }

            List<ProductTagId> tagList =
                new List<ProductTagId>();

            HashSet<ProductTagId> uniqueTags =
                new HashSet<ProductTagId>();

            foreach (ProductTagId tag in tags)
            {
                if (string.IsNullOrWhiteSpace(tag.Value))
                {
                    throw new ArgumentException(
                        "Product tags must be initialized.",
                        nameof(tags));
                }

                if (!uniqueTags.Add(tag))
                {
                    throw new ArgumentException(
                        $"Product tag {tag} is duplicated.",
                        nameof(tags));
                }

                tagList.Add(tag);
            }

            Id = id;
            DisplayNameKey = displayNameKey;
            CategoryId = categoryId;
            _tags = new ReadOnlyCollection<ProductTagId>(tagList);
        }
    }

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
