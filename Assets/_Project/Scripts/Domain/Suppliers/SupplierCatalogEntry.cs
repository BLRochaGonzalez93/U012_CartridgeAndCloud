using System;
using VRMGames.CartridgeAndCloud.Domain.Inventory;
using VRMGames.CartridgeAndCloud.Domain.Products;

namespace VRMGames.CartridgeAndCloud.Domain.Suppliers
{
    public sealed class SupplierCatalogEntry
    {
        public ProductDefinitionId ProductId { get; }

        public int UnitCostCents { get; }

        public Quantity UnitsPerBox { get; }

        public int MinimumBoxes { get; }

        public int MaximumBoxes { get; }

        public SupplierCatalogEntry(
            ProductDefinitionId productId,
            int unitCostCents,
            Quantity unitsPerBox,
            int minimumBoxes,
            int maximumBoxes)
        {
            if (string.IsNullOrWhiteSpace(productId.Value))
            {
                throw new ArgumentException(
                    "Product definition ID must be initialized.",
                    nameof(productId));
            }

            if (unitCostCents <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(unitCostCents),
                    "Supplier unit cost must be greater than zero.");
            }

            if (unitsPerBox.IsZero)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(unitsPerBox),
                    "Units per box must be greater than zero.");
            }

            if (minimumBoxes <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(minimumBoxes),
                    "Minimum boxes must be greater than zero.");
            }

            if (maximumBoxes < minimumBoxes)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(maximumBoxes),
                    "Maximum boxes cannot be below the minimum.");
            }

            ProductId = productId;
            UnitCostCents = unitCostCents;
            UnitsPerBox = unitsPerBox;
            MinimumBoxes = minimumBoxes;
            MaximumBoxes = maximumBoxes;
        }

        public bool CanOrder(int boxCount)
        {
            return boxCount >= MinimumBoxes &&
                   boxCount <= MaximumBoxes;
        }

        public Quantity GetOrderedQuantity(int boxCount)
        {
            if (!CanOrder(boxCount))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(boxCount),
                    "Box count is outside supplier limits.");
            }

            return new Quantity(
                checked(UnitsPerBox.Value * boxCount));
        }

        public long GetTotalCostCents(int boxCount)
        {
            Quantity quantity = GetOrderedQuantity(boxCount);

            return checked(
                (long)quantity.Value * UnitCostCents);
        }
    }
}
