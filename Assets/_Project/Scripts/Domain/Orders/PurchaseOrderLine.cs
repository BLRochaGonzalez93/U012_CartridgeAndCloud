using System;
using VRMGames.CartridgeAndCloud.Domain.Inventory;
using VRMGames.CartridgeAndCloud.Domain.Products;

namespace VRMGames.CartridgeAndCloud.Domain.Orders
{
    public sealed class PurchaseOrderLine
    {
        public ProductDefinitionId ProductId { get; }

        public int BoxCount { get; }

        public Quantity UnitsPerBox { get; }

        public Quantity OrderedQuantity { get; }

        public int UnitCostCents { get; }

        public long TotalCostCents { get; }

        public PurchaseOrderLine(
            ProductDefinitionId productId,
            int boxCount,
            Quantity unitsPerBox,
            int unitCostCents)
        {
            if (string.IsNullOrWhiteSpace(productId.Value))
            {
                throw new ArgumentException(
                    "Product definition ID must be initialized.",
                    nameof(productId));
            }

            if (boxCount <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(boxCount));
            }

            if (unitsPerBox.IsZero)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(unitsPerBox));
            }

            if (unitCostCents <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(unitCostCents));
            }

            ProductId = productId;
            BoxCount = boxCount;
            UnitsPerBox = unitsPerBox;
            UnitCostCents = unitCostCents;
            OrderedQuantity =
                new Quantity(
                    checked(boxCount * unitsPerBox.Value));

            TotalCostCents =
                checked(
                    (long)OrderedQuantity.Value * unitCostCents);
        }
    }
}
