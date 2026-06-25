using System;
using VRMGames.CartridgeAndCloud.Domain.Displays;
using VRMGames.CartridgeAndCloud.Domain.Inventory;
using VRMGames.CartridgeAndCloud.Domain.Products;

namespace VRMGames.CartridgeAndCloud.Application.Displays
{
    public sealed class DisplayReturnStockService
    {
        private readonly InventoryTransferService _transferService;

        public DisplayReturnStockService(
            ProductDefinitionRegistry productDefinitions)
        {
            if (productDefinitions == null)
            {
                throw new ArgumentNullException(
                    nameof(productDefinitions));
            }

            _transferService =
                new InventoryTransferService(productDefinitions);
        }

        public DisplayReturnStockResult Return(
            DisplayInstance display,
            InventoryContainer destination,
            Quantity quantity)
        {
            return ReturnInternal(
                display,
                destination,
                quantity,
                clearAssignment: false);
        }

        public DisplayReturnStockResult ReturnAllAndClear(
            DisplayInstance display,
            InventoryContainer destination)
        {
            if (display == null)
            {
                throw new ArgumentNullException(nameof(display));
            }

            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            if (!display.HasAssignedProduct)
            {
                return DisplayReturnStockResult.Failure(
                    DisplayReturnStockFailureReason
                        .DisplayHasNoAssignedProduct,
                    default(ProductDefinitionId),
                    Quantity.Zero,
                    Quantity.Zero);
            }

            ProductDefinitionId productId =
                display.AssignedProductId;

            Quantity quantity =
                display.Inventory.GetQuantity(productId);

            if (quantity.IsZero)
            {
                Quantity destinationBefore =
                    destination.GetQuantity(productId);

                DisplayClearAssignmentResult clear =
                    display.TryClearAssignment();

                if (!clear.Succeeded)
                {
                    return DisplayReturnStockResult.Failure(
                        DisplayReturnStockFailureReason
                            .ClearAssignmentRejected,
                        productId,
                        Quantity.Zero,
                        destinationBefore);
                }

                return DisplayReturnStockResult.Success(
                    productId,
                    Quantity.Zero,
                    Quantity.Zero,
                    Quantity.Zero,
                    destinationBefore,
                    destinationBefore,
                    assignmentCleared: true);
            }

            return ReturnInternal(
                display,
                destination,
                quantity,
                clearAssignment: true);
        }

        private DisplayReturnStockResult ReturnInternal(
            DisplayInstance display,
            InventoryContainer destination,
            Quantity quantity,
            bool clearAssignment)
        {
            if (display == null)
            {
                throw new ArgumentNullException(nameof(display));
            }

            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            ProductDefinitionId productId =
                display.HasAssignedProduct
                    ? display.AssignedProductId
                    : default(ProductDefinitionId);

            Quantity displayBefore =
                display.HasAssignedProduct
                    ? display.Inventory.GetQuantity(productId)
                    : Quantity.Zero;

            Quantity destinationBefore =
                display.HasAssignedProduct
                    ? destination.GetQuantity(productId)
                    : Quantity.Zero;

            if (quantity.IsZero)
            {
                return Failure(
                    DisplayReturnStockFailureReason.InvalidQuantity,
                    productId,
                    displayBefore,
                    destinationBefore);
            }

            if (!IsAllowedDestinationType(destination.Type))
            {
                return Failure(
                    DisplayReturnStockFailureReason
                        .DestinationContainerTypeNotAllowed,
                    productId,
                    displayBefore,
                    destinationBefore);
            }

            if (!display.HasAssignedProduct)
            {
                return Failure(
                    DisplayReturnStockFailureReason
                        .DisplayHasNoAssignedProduct,
                    productId,
                    displayBefore,
                    destinationBefore);
            }

            if (displayBefore.IsZero)
            {
                return Failure(
                    DisplayReturnStockFailureReason.DisplayStockMissing,
                    productId,
                    displayBefore,
                    destinationBefore);
            }

            if (quantity > displayBefore)
            {
                return Failure(
                    DisplayReturnStockFailureReason
                        .InsufficientDisplayQuantity,
                    productId,
                    displayBefore,
                    destinationBefore);
            }

            if (quantity.Value > destination.AvailableCapacity)
            {
                return Failure(
                    DisplayReturnStockFailureReason
                        .DestinationCapacityExceeded,
                    productId,
                    displayBefore,
                    destinationBefore);
            }

            InventoryTransferResult transfer =
                _transferService.Transfer(
                    display.Inventory,
                    destination,
                    productId,
                    quantity);

            if (!transfer.Succeeded)
            {
                return Failure(
                    DisplayReturnStockFailureReason.TransferRejected,
                    productId,
                    displayBefore,
                    destinationBefore);
            }

            bool assignmentCleared = false;

            if (clearAssignment)
            {
                DisplayClearAssignmentResult clear =
                    display.TryClearAssignment();

                if (!clear.Succeeded)
                {
                    InventoryTransferResult rollback =
                        _transferService.Transfer(
                            destination,
                            display.Inventory,
                            productId,
                            quantity);

                    if (!rollback.Succeeded)
                    {
                        throw new InvalidOperationException(
                            "Display return rollback failed.");
                    }

                    return Failure(
                        DisplayReturnStockFailureReason
                            .ClearAssignmentRejected,
                        productId,
                        displayBefore,
                        destinationBefore);
                }

                assignmentCleared = true;
            }

            return DisplayReturnStockResult.Success(
                productId,
                quantity,
                transfer.SourceQuantityBefore,
                transfer.SourceQuantityAfter,
                transfer.DestinationQuantityBefore,
                transfer.DestinationQuantityAfter,
                assignmentCleared);
        }

        private static bool IsAllowedDestinationType(
            InventoryContainerType type)
        {
            return type == InventoryContainerType.Storage ||
                   type == InventoryContainerType.Transit;
        }

        private static DisplayReturnStockResult Failure(
            DisplayReturnStockFailureReason failureReason,
            ProductDefinitionId productId,
            Quantity displayQuantity,
            Quantity destinationQuantity)
        {
            return DisplayReturnStockResult.Failure(
                failureReason,
                productId,
                displayQuantity,
                destinationQuantity);
        }
    }
}
