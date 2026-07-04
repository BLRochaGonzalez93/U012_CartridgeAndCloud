using System;
using VRMGames.CartridgeAndCloud.Domain.Products;

namespace VRMGames.CartridgeAndCloud.Domain.Inventory
{
    public sealed class InventoryTransferService
    {
        private readonly ProductDefinitionRegistry _productDefinitions;

        public InventoryTransferService(
            ProductDefinitionRegistry productDefinitions)
        {
            _productDefinitions = productDefinitions ??
                throw new ArgumentNullException(
                    nameof(productDefinitions));
        }

        public InventoryTransferResult Transfer(
            InventoryContainer source,
            InventoryContainer destination,
            ProductDefinitionId productId,
            Quantity quantity)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            if (string.IsNullOrWhiteSpace(productId.Value))
            {
                throw new ArgumentException(
                    "Product definition ID must be initialized.",
                    nameof(productId));
            }

            Quantity sourceBefore =
                source.GetQuantity(productId);

            Quantity destinationBefore =
                destination.GetQuantity(productId);

            long totalBefore =
                (long)source.UsedCapacity + destination.UsedCapacity;

            if (quantity.IsZero)
            {
                return InventoryTransferResult.Failure(
                    InventoryTransferFailureReason.InvalidQuantity,
                    productId,
                    quantity,
                    sourceBefore,
                    destinationBefore,
                    totalBefore);
            }

            if (ReferenceEquals(source, destination) ||
                source.Id == destination.Id)
            {
                return InventoryTransferResult.Failure(
                    InventoryTransferFailureReason.SameContainer,
                    productId,
                    quantity,
                    sourceBefore,
                    destinationBefore,
                    totalBefore);
            }

            if (!_productDefinitions.Contains(productId))
            {
                return InventoryTransferResult.Failure(
                    InventoryTransferFailureReason.ProductDefinitionMissing,
                    productId,
                    quantity,
                    sourceBefore,
                    destinationBefore,
                    totalBefore);
            }

            if (sourceBefore.IsZero)
            {
                return InventoryTransferResult.Failure(
                    InventoryTransferFailureReason.SourceProductMissing,
                    productId,
                    quantity,
                    sourceBefore,
                    destinationBefore,
                    totalBefore);
            }

            if (quantity > sourceBefore)
            {
                return InventoryTransferResult.Failure(
                    InventoryTransferFailureReason
                        .InsufficientSourceQuantity,
                    productId,
                    quantity,
                    sourceBefore,
                    destinationBefore,
                    totalBefore);
            }

            if (quantity.Value > destination.AvailableCapacity)
            {
                return InventoryTransferResult.Failure(
                    InventoryTransferFailureReason
                        .DestinationCapacityExceeded,
                    productId,
                    quantity,
                    sourceBefore,
                    destinationBefore,
                    totalBefore);
            }

            source.RemoveValidated(productId, quantity);
            destination.AddValidated(productId, quantity);

            Quantity sourceAfter =
                source.GetQuantity(productId);

            Quantity destinationAfter =
                destination.GetQuantity(productId);

            long totalAfter =
                (long)source.UsedCapacity + destination.UsedCapacity;

            if (totalAfter != totalBefore)
            {
                throw new InvalidOperationException(
                    "Inventory transfer violated unit conservation.");
            }

            return InventoryTransferResult.Success(
                productId,
                quantity,
                sourceBefore,
                sourceAfter,
                destinationBefore,
                destinationAfter,
                totalBefore,
                totalAfter);
        }
    }

    public enum InventoryTransferFailureReason
    {
        None = 0,
        InvalidQuantity = 1,
        SameContainer = 2,
        ProductDefinitionMissing = 3,
        SourceProductMissing = 4,
        InsufficientSourceQuantity = 5,
        DestinationCapacityExceeded = 6
    }

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
