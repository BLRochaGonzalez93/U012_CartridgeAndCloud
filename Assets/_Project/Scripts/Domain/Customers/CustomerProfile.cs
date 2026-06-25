using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VRMGames.CartridgeAndCloud.Domain.Products;

namespace VRMGames.CartridgeAndCloud.Domain.Customers
{
    public sealed class CustomerProfile
    {
        private readonly ReadOnlyCollection<ProductCategoryId>
            _preferredCategoryIds;

        public CustomerProfileId Id { get; }

        public string DisplayNameKey { get; }

        public IReadOnlyList<ProductCategoryId> PreferredCategoryIds =>
            _preferredCategoryIds;

        public int SpawnWeight { get; }

        public int PatienceSeconds { get; }

        public int BrowseStopCount { get; }

        public float WalkSpeed { get; }

        public CustomerProfile(
            CustomerProfileId id,
            string displayNameKey,
            IEnumerable<ProductCategoryId> preferredCategoryIds,
            int spawnWeight,
            int patienceSeconds,
            int browseStopCount,
            float walkSpeed)
        {
            if (string.IsNullOrWhiteSpace(id.Value))
            {
                throw new ArgumentException(
                    "Customer profile ID must be initialized.",
                    nameof(id));
            }

            if (string.IsNullOrWhiteSpace(displayNameKey))
            {
                throw new ArgumentException(
                    "Display-name key cannot be empty.",
                    nameof(displayNameKey));
            }

            if (preferredCategoryIds == null)
            {
                throw new ArgumentNullException(
                    nameof(preferredCategoryIds));
            }

            if (spawnWeight <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(spawnWeight));
            }

            if (patienceSeconds <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(patienceSeconds));
            }

            if (browseStopCount <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(browseStopCount));
            }

            if (walkSpeed <= 0f ||
                float.IsNaN(walkSpeed) ||
                float.IsInfinity(walkSpeed))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(walkSpeed));
            }

            List<ProductCategoryId> categories =
                new List<ProductCategoryId>();
            HashSet<ProductCategoryId> unique =
                new HashSet<ProductCategoryId>();

            foreach (ProductCategoryId categoryId in preferredCategoryIds)
            {
                if (string.IsNullOrWhiteSpace(categoryId.Value))
                {
                    throw new ArgumentException(
                        "Preferred category IDs must be initialized.",
                        nameof(preferredCategoryIds));
                }

                if (!unique.Add(categoryId))
                {
                    throw new ArgumentException(
                        $"Preferred category {categoryId} is duplicated.",
                        nameof(preferredCategoryIds));
                }

                categories.Add(categoryId);
            }

            Id = id;
            DisplayNameKey = displayNameKey;
            SpawnWeight = spawnWeight;
            PatienceSeconds = patienceSeconds;
            BrowseStopCount = browseStopCount;
            WalkSpeed = walkSpeed;
            _preferredCategoryIds =
                new ReadOnlyCollection<ProductCategoryId>(categories);
        }

        public bool Prefers(ProductCategoryId categoryId)
        {
            return _preferredCategoryIds.Contains(categoryId);
        }
    }
}
