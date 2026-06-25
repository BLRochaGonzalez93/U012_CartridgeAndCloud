using System;
using VRMGames.CartridgeAndCloud.Domain.Products;

namespace VRMGames.CartridgeAndCloud.Domain.Orders
{
    public readonly struct PurchaseOrderRequestLine
    {
        public ProductDefinitionId ProductId { get; }

        public int BoxCount { get; }

        public PurchaseOrderRequestLine(
            ProductDefinitionId productId,
            int boxCount)
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
                    nameof(boxCount),
                    "Requested box count must be greater than zero.");
            }

            ProductId = productId;
            BoxCount = boxCount;
        }
    }
}
