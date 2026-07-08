using System;
using VRMGames.CartridgeAndCloud.Domain.Displays;
using VRMGames.CartridgeAndCloud.Domain.Inventory;
using VRMGames.CartridgeAndCloud.Domain.Products;

namespace VRMGames.CartridgeAndCloud.Application.Displays
{
    public sealed class DisplayRestockService
    {
        private readonly ProductDefinitionRegistry _productDefinitions;
        private readonly InventoryTransferService _transferService;

        public DisplayRestockService(
            ProductDefinitionRegistry productDefinitions)
        {
            _productDefinitions = productDefinitions ??
                throw new ArgumentNullException(
                    nameof(productDefinitions));

            _transferService =
                new InventoryTransferService(productDefinitions);
        }

        public DisplayRestockResult Restock(
            InventoryContainer source,
            DisplayInstance display,
            Quantity quantity)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (display == null)
            {
                throw new ArgumentNullException(nameof(display));
            }

            ProductDefinitionId productId =
                display.HasAssignedProduct
                    ? display.AssignedProductId
                    : default(ProductDefinitionId);

            Quantity sourceBefore =
                display.HasAssignedProduct
                    ? source.GetQuantity(productId)
                    : Quantity.Zero;

            Quantity displayBefore =
                display.HasAssignedProduct
                    ? display.Inventory.GetQuantity(productId)
                    : Quantity.Zero;

            int visibleBefore = display.VisibleUnitCount;

            if (quantity.IsZero)
            {
                return Failure(
                    DisplayRestockFailureReason.InvalidQuantity,
                    productId,
                    quantity,
                    sourceBefore,
                    displayBefore,
                    visibleBefore);
            }

            if (!IsAllowedSourceType(source.Type))
            {
                return Failure(
                    DisplayRestockFailureReason
                        .SourceContainerTypeNotAllowed,
                    productId,
                    quantity,
                    sourceBefore,
                    displayBefore,
                    visibleBefore);
            }

            if (!display.HasAssignedProduct)
            {
                return Failure(
                    DisplayRestockFailureReason
                        .DisplayHasNoAssignedProduct,
                    productId,
                    quantity,
                    sourceBefore,
                    displayBefore,
                    visibleBefore);
            }

            if (!_productDefinitions.Contains(productId))
            {
                return Failure(
                    DisplayRestockFailureReason
                        .ProductDefinitionMissing,
                    productId,
                    quantity,
                    sourceBefore,
                    displayBefore,
                    visibleBefore);
            }

            if (sourceBefore.IsZero)
            {
                return Failure(
                    DisplayRestockFailureReason.SourceProductMissing,
                    productId,
                    quantity,
                    sourceBefore,
                    displayBefore,
                    visibleBefore);
            }

            if (display.Inventory.AvailableCapacity == 0)
            {
                return Failure(
                    DisplayRestockFailureReason.DisplayAlreadyFull,
                    productId,
                    quantity,
                    sourceBefore,
                    displayBefore,
                    visibleBefore);
            }

            int transferableUnits = Math.Min(
                quantity.Value,
                Math.Min(
                    sourceBefore.Value,
                    display.Inventory.AvailableCapacity));

            if (transferableUnits < 1)
            {
                return Failure(
                    DisplayRestockFailureReason.TransferRejected,
                    productId,
                    quantity,
                    sourceBefore,
                    displayBefore,
                    visibleBefore);
            }

            Quantity transferredQuantity =
                new Quantity(transferableUnits);

            InventoryTransferResult transfer =
                _transferService.Transfer(
                    source,
                    display.Inventory,
                    productId,
                    transferredQuantity);

            if (!transfer.Succeeded)
            {
                return Failure(
                    DisplayRestockFailureReason.TransferRejected,
                    productId,
                    quantity,
                    sourceBefore,
                    displayBefore,
                    visibleBefore);
            }

            return DisplayRestockResult.Success(
                productId,
                quantity,
                transferredQuantity,
                transfer.SourceQuantityBefore,
                transfer.SourceQuantityAfter,
                transfer.DestinationQuantityBefore,
                transfer.DestinationQuantityAfter,
                visibleBefore,
                display.VisibleUnitCount);
        }

        public DisplayRestockResult RestockToCapacity(
            InventoryContainer source,
            DisplayInstance display)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (display == null)
            {
                throw new ArgumentNullException(nameof(display));
            }

            if (!display.HasAssignedProduct)
            {
                return Restock(
                    source,
                    display,
                    new Quantity(1));
            }

            ProductDefinitionId productId =
                display.AssignedProductId;

            Quantity available = source.GetQuantity(productId);

            if (display.Inventory.AvailableCapacity == 0 ||
                available.IsZero)
            {
                return Restock(
                    source,
                    display,
                    new Quantity(1));
            }

            int units = Math.Min(
                available.Value,
                display.Inventory.AvailableCapacity);

            return Restock(
                source,
                display,
                new Quantity(units));
        }

        private static bool IsAllowedSourceType(
            InventoryContainerType type)
        {
            return type == InventoryContainerType.Storage;
        }

        private static DisplayRestockResult Failure(
            DisplayRestockFailureReason failureReason,
            ProductDefinitionId productId,
            Quantity requestedQuantity,
            Quantity sourceQuantity,
            Quantity displayQuantity,
            int visibleUnits)
        {
            return DisplayRestockResult.Failure(
                failureReason,
                productId,
                requestedQuantity,
                sourceQuantity,
                displayQuantity,
                visibleUnits);
        }
    }
}
