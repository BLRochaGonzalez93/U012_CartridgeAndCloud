using VRMGames.CartridgeAndCloud.Domain.Products;

namespace VRMGames.CartridgeAndCloud.Domain.Inventory
{
    public sealed class InventoryTransferResult
    {
        public bool Succeeded { get; }

        public InventoryTransferFailureReason FailureReason { get; }

        public ProductDefinitionId ProductId { get; }

        public Quantity RequestedQuantity { get; }

        public Quantity SourceQuantityBefore { get; }

        public Quantity SourceQuantityAfter { get; }

        public Quantity DestinationQuantityBefore { get; }

        public Quantity DestinationQuantityAfter { get; }

        public long TotalUnitsBefore { get; }

        public long TotalUnitsAfter { get; }

        private InventoryTransferResult(
            bool succeeded,
            InventoryTransferFailureReason failureReason,
            ProductDefinitionId productId,
            Quantity requestedQuantity,
            Quantity sourceQuantityBefore,
            Quantity sourceQuantityAfter,
            Quantity destinationQuantityBefore,
            Quantity destinationQuantityAfter,
            long totalUnitsBefore,
            long totalUnitsAfter)
        {
            Succeeded = succeeded;
            FailureReason = failureReason;
            ProductId = productId;
            RequestedQuantity = requestedQuantity;
            SourceQuantityBefore = sourceQuantityBefore;
            SourceQuantityAfter = sourceQuantityAfter;
            DestinationQuantityBefore = destinationQuantityBefore;
            DestinationQuantityAfter = destinationQuantityAfter;
            TotalUnitsBefore = totalUnitsBefore;
            TotalUnitsAfter = totalUnitsAfter;
        }

        public static InventoryTransferResult Success(
            ProductDefinitionId productId,
            Quantity requestedQuantity,
            Quantity sourceQuantityBefore,
            Quantity sourceQuantityAfter,
            Quantity destinationQuantityBefore,
            Quantity destinationQuantityAfter,
            long totalUnitsBefore,
            long totalUnitsAfter)
        {
            return new InventoryTransferResult(
                true,
                InventoryTransferFailureReason.None,
                productId,
                requestedQuantity,
                sourceQuantityBefore,
                sourceQuantityAfter,
                destinationQuantityBefore,
                destinationQuantityAfter,
                totalUnitsBefore,
                totalUnitsAfter);
        }

        public static InventoryTransferResult Failure(
            InventoryTransferFailureReason failureReason,
            ProductDefinitionId productId,
            Quantity requestedQuantity,
            Quantity sourceQuantity,
            Quantity destinationQuantity,
            long totalUnits)
        {
            return new InventoryTransferResult(
                false,
                failureReason,
                productId,
                requestedQuantity,
                sourceQuantity,
                sourceQuantity,
                destinationQuantity,
                destinationQuantity,
                totalUnits,
                totalUnits);
        }
    }
}
