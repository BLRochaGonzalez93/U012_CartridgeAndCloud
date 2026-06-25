using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VRMGames.CartridgeAndCloud.Domain.Inventory;
using VRMGames.CartridgeAndCloud.Domain.Products;

namespace VRMGames.CartridgeAndCloud.Domain.Displays
{
    public sealed class DisplayDefinition
    {
        private readonly ReadOnlyCollection<ProductCategoryId>
            _allowedCategories;

        public DisplayDefinitionId Id { get; }

        public string DisplayNameKey { get; }

        public InventoryCapacity Capacity { get; }

        public int VisibleUnitLimit { get; }

        public IReadOnlyList<ProductCategoryId> AllowedCategories =>
            _allowedCategories;

        public string PlacementDefinitionId { get; }

        public DisplayDefinition(
            DisplayDefinitionId id,
            string displayNameKey,
            InventoryCapacity capacity,
            int visibleUnitLimit,
            IEnumerable<ProductCategoryId> allowedCategories,
            string placementDefinitionId)
        {
            if (string.IsNullOrWhiteSpace(id.Value))
            {
                throw new ArgumentException(
                    "Display definition ID must be initialized.",
                    nameof(id));
            }

            if (string.IsNullOrWhiteSpace(displayNameKey))
            {
                throw new ArgumentException(
                    "Display-name key cannot be empty.",
                    nameof(displayNameKey));
            }

            if (capacity.Units <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(capacity),
                    "Display capacity must be greater than zero.");
            }

            if (visibleUnitLimit <= 0 ||
                visibleUnitLimit > capacity.Units)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(visibleUnitLimit),
                    "Visible-unit limit must be positive and not exceed capacity.");
            }

            if (allowedCategories == null)
            {
                throw new ArgumentNullException(nameof(allowedCategories));
            }

            if (string.IsNullOrWhiteSpace(placementDefinitionId))
            {
                throw new ArgumentException(
                    "Placement definition ID cannot be empty.",
                    nameof(placementDefinitionId));
            }

            List<ProductCategoryId> categories =
                new List<ProductCategoryId>();

            HashSet<ProductCategoryId> unique =
                new HashSet<ProductCategoryId>();

            foreach (ProductCategoryId category in allowedCategories)
            {
                if (string.IsNullOrWhiteSpace(category.Value))
                {
                    throw new ArgumentException(
                        "Allowed categories must be initialized.",
                        nameof(allowedCategories));
                }

                if (!unique.Add(category))
                {
                    throw new ArgumentException(
                        $"Allowed category {category} is duplicated.",
                        nameof(allowedCategories));
                }

                categories.Add(category);
            }

            Id = id;
            DisplayNameKey = displayNameKey;
            Capacity = capacity;
            VisibleUnitLimit = visibleUnitLimit;
            PlacementDefinitionId = placementDefinitionId;
            _allowedCategories =
                new ReadOnlyCollection<ProductCategoryId>(categories);
        }

        public bool CanAccept(ProductDefinition product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            if (_allowedCategories.Count == 0)
            {
                return true;
            }

            for (int index = 0; index < _allowedCategories.Count; index++)
            {
                if (_allowedCategories[index] == product.CategoryId)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
