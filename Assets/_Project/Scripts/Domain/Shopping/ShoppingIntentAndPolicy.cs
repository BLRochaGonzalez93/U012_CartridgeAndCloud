using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VRMGames.CartridgeAndCloud.Domain.Customers;
using VRMGames.CartridgeAndCloud.Domain.Products;

namespace VRMGames.CartridgeAndCloud.Domain.Shopping
{
    public sealed class ShoppingPolicy
    {
        public int MaxCartUnits { get; }
        public int MaxUnitsPerReservation { get; }
        public bool AllowFallbackCategories { get; }

        public ShoppingPolicy(int maxCartUnits, int maxUnitsPerReservation, bool allowFallbackCategories)
        {
            if (maxCartUnits <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxCartUnits));
            if (maxUnitsPerReservation <= 0 || maxUnitsPerReservation > maxCartUnits)
                throw new ArgumentOutOfRangeException(nameof(maxUnitsPerReservation));
            MaxCartUnits = maxCartUnits;
            MaxUnitsPerReservation = maxUnitsPerReservation;
            AllowFallbackCategories = allowFallbackCategories;
        }
    }

    public sealed class ShoppingIntent
    {
        private readonly ReadOnlyCollection<ProductCategoryId> _preferredCategories;
        public ShoppingIntentId Id { get; }
        public CustomerInstanceId CustomerId { get; }
        public IReadOnlyList<ProductCategoryId> PreferredCategories => _preferredCategories;
        public int DesiredUnits { get; }

        public ShoppingIntent(
            ShoppingIntentId id,
            CustomerInstanceId customerId,
            IEnumerable<ProductCategoryId> preferredCategories,
            int desiredUnits)
        {
            if (string.IsNullOrWhiteSpace(customerId.Value))
                throw new ArgumentException("Customer ID must be initialized.", nameof(customerId));
            if (preferredCategories == null)
                throw new ArgumentNullException(nameof(preferredCategories));
            if (desiredUnits <= 0)
                throw new ArgumentOutOfRangeException(nameof(desiredUnits));

            List<ProductCategoryId> categories = new List<ProductCategoryId>();
            HashSet<ProductCategoryId> unique = new HashSet<ProductCategoryId>();
            foreach (ProductCategoryId category in preferredCategories)
            {
                if (string.IsNullOrWhiteSpace(category.Value))
                    throw new ArgumentException("Preferred categories must be initialized.", nameof(preferredCategories));
                if (!unique.Add(category))
                    throw new ArgumentException($"Preferred category {category} is duplicated.", nameof(preferredCategories));
                categories.Add(category);
            }

            Id = id;
            CustomerId = customerId;
            DesiredUnits = desiredUnits;
            _preferredCategories = new ReadOnlyCollection<ProductCategoryId>(categories);
        }

        public bool TryGetPreferenceRank(ProductCategoryId categoryId, out int rank)
        {
            for (int index = 0; index < _preferredCategories.Count; index++)
            {
                if (_preferredCategories[index] == categoryId)
                {
                    rank = index;
                    return true;
                }
            }
            rank = -1;
            return false;
        }
    }
}
