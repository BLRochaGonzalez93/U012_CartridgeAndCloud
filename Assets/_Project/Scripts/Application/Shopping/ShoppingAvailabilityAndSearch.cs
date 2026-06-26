using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VRMGames.CartridgeAndCloud.Domain.Displays;
using VRMGames.CartridgeAndCloud.Domain.Inventory;
using VRMGames.CartridgeAndCloud.Domain.Products;
using VRMGames.CartridgeAndCloud.Domain.Shopping;

namespace VRMGames.CartridgeAndCloud.Application.Shopping
{
    public sealed class ShoppingAvailabilitySnapshot
    {
        public Quantity OnHand { get; }
        public Quantity Reserved { get; }
        public Quantity Available { get; }

        public ShoppingAvailabilitySnapshot(Quantity onHand, Quantity reserved)
        {
            if (reserved > onHand)
                throw new InvalidOperationException("Reserved quantity exceeds on-hand quantity.");
            OnHand = onHand;
            Reserved = reserved;
            Available = new Quantity(onHand.Value - reserved.Value);
        }
    }

    public sealed class ShoppingAvailabilityService
    {
        private readonly ShoppingReservationRegistry _reservations;

        public ShoppingAvailabilityService(ShoppingReservationRegistry reservations)
        {
            _reservations = reservations ??
                throw new ArgumentNullException(nameof(reservations));
        }

        public ShoppingAvailabilitySnapshot GetAvailability(DisplayInstance display)
        {
            if (display == null) throw new ArgumentNullException(nameof(display));
            if (!display.HasAssignedProduct)
                return new ShoppingAvailabilitySnapshot(Quantity.Zero, Quantity.Zero);

            ProductDefinitionId productId = display.AssignedProductId;
            Quantity onHand = display.Inventory.GetQuantity(productId);
            Quantity reserved = _reservations.GetActiveReservedQuantity(display.Id, productId);
            return new ShoppingAvailabilitySnapshot(onHand, reserved);
        }
    }

    public sealed class ShoppingCandidate
    {
        public DisplayInstanceId DisplayId { get; }
        public ProductDefinitionId ProductId { get; }
        public ProductCategoryId CategoryId { get; }
        public int PreferenceRank { get; }
        public Quantity AvailableQuantity { get; }

        public ShoppingCandidate(
            DisplayInstanceId displayId,
            ProductDefinitionId productId,
            ProductCategoryId categoryId,
            int preferenceRank,
            Quantity availableQuantity)
        {
            DisplayId = displayId;
            ProductId = productId;
            CategoryId = categoryId;
            PreferenceRank = preferenceRank;
            AvailableQuantity = availableQuantity;
        }
    }

    public sealed class ShoppingSearchService
    {
        private readonly ProductDefinitionRegistry _products;
        private readonly ShoppingAvailabilityService _availability;
        private readonly ShoppingPolicy _policy;

        public ShoppingSearchService(
            ProductDefinitionRegistry products,
            ShoppingReservationRegistry reservations,
            ShoppingPolicy policy)
        {
            _products = products ?? throw new ArgumentNullException(nameof(products));
            _availability = new ShoppingAvailabilityService(
                reservations ?? throw new ArgumentNullException(nameof(reservations)));
            _policy = policy ?? throw new ArgumentNullException(nameof(policy));
        }

        public IReadOnlyList<ShoppingCandidate> FindCandidates(
            ShoppingIntent intent,
            DisplayInstanceRegistry displays)
        {
            if (intent == null) throw new ArgumentNullException(nameof(intent));
            if (displays == null) throw new ArgumentNullException(nameof(displays));

            List<ShoppingCandidate> candidates = new List<ShoppingCandidate>();
            foreach (DisplayInstance display in displays.Instances)
            {
                if (!display.HasAssignedProduct) continue;
                ProductDefinitionId productId = display.AssignedProductId;
                if (!_products.TryGet(productId, out ProductDefinition product)) continue;

                ShoppingAvailabilitySnapshot snapshot = _availability.GetAvailability(display);
                if (snapshot.Available.IsZero) continue;

                bool preferred = intent.TryGetPreferenceRank(product.CategoryId, out int rank);
                if (!preferred)
                {
                    if (!_policy.AllowFallbackCategories) continue;
                    rank = 1000;
                }

                candidates.Add(new ShoppingCandidate(
                    display.Id,
                    product.Id,
                    product.CategoryId,
                    rank,
                    snapshot.Available));
            }

            candidates.Sort(CompareCandidates);
            return new ReadOnlyCollection<ShoppingCandidate>(candidates);
        }

        private static int CompareCandidates(ShoppingCandidate left, ShoppingCandidate right)
        {
            int rank = left.PreferenceRank.CompareTo(right.PreferenceRank);
            if (rank != 0) return rank;
            int product = StringComparer.Ordinal.Compare(left.ProductId.Value, right.ProductId.Value);
            if (product != 0) return product;
            return StringComparer.Ordinal.Compare(left.DisplayId.Value, right.DisplayId.Value);
        }
    }
}
