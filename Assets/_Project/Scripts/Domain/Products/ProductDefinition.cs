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
}
