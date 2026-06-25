using System;
using VRMGames.CartridgeAndCloud.Domain.Inventory;
using VRMGames.CartridgeAndCloud.Domain.Orders;
using VRMGames.CartridgeAndCloud.Domain.Products;
using VRMGames.CartridgeAndCloud.Domain.Receiving;

namespace VRMGames.CartridgeAndCloud.Application.Receiving
{
    public sealed class ReceivingService
    {
        private readonly ProductDefinitionRegistry _productDefinitions;

        public ReceivingService(
            ProductDefinitionRegistry productDefinitions)
        {
            _productDefinitions = productDefinitions ??
                throw new ArgumentNullException(
                    nameof(productDefinitions));
        }

        public ReceivingResult ReceiveBox(
            PurchaseOrder order,
            Delivery delivery,
            ShipmentBoxId boxId,
            InventoryContainer destination)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }

            if (delivery == null)
            {
                throw new ArgumentNullException(nameof(delivery));
            }

            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            if (delivery.OrderId != order.Id)
            {
                return FailureWithoutBox(
                    ReceivingFailureReason.DeliveryOrderMismatch,
                    boxId,
                    delivery,
                    order);
            }

            if (delivery.SupplierId != order.SupplierId)
            {
                return FailureWithoutBox(
                    ReceivingFailureReason.DeliverySupplierMismatch,
                    boxId,
                    delivery,
                    order);
            }

            if (!delivery.TryGetBox(boxId, out ShipmentBox box))
            {
                return FailureWithoutBox(
                    ReceivingFailureReason.BoxNotFound,
                    boxId,
                    delivery,
                    order);
            }

            Quantity destinationBefore =
                destination.GetQuantity(box.ProductId);

            if (box.IsReceived)
            {
                return ReceivingResult.Failure(
                    ReceivingFailureReason.BoxAlreadyReceived,
                    box.Id,
                    box.ProductId,
                    box.Quantity,
                    destinationBefore,
                    delivery.Status,
                    order.Status);
            }

            if (order.Status != PurchaseOrderStatus.Delivered)
            {
                return ReceivingResult.Failure(
                    ReceivingFailureReason.OrderNotDelivered,
                    box.Id,
                    box.ProductId,
                    box.Quantity,
                    destinationBefore,
                    delivery.Status,
                    order.Status);
            }

            if (!_productDefinitions.Contains(box.ProductId))
            {
                return ReceivingResult.Failure(
                    ReceivingFailureReason.ProductDefinitionMissing,
                    box.Id,
                    box.ProductId,
                    box.Quantity,
                    destinationBefore,
                    delivery.Status,
                    order.Status);
            }

            if (box.Quantity.Value > destination.AvailableCapacity)
            {
                return ReceivingResult.Failure(
                    ReceivingFailureReason.DestinationCapacityExceeded,
                    box.Id,
                    box.ProductId,
                    box.Quantity,
                    destinationBefore,
                    delivery.Status,
                    order.Status);
            }

            InventoryMutationResult inventoryMutation =
                destination.TryAdd(
                    box.ProductId,
                    box.Quantity);

            if (!inventoryMutation.Succeeded)
            {
                return ReceivingResult.Failure(
                    ReceivingFailureReason.InventoryMutationRejected,
                    box.Id,
                    box.ProductId,
                    box.Quantity,
                    destinationBefore,
                    delivery.Status,
                    order.Status);
            }

            DeliveryMutationResult deliveryMutation =
                delivery.TryMarkBoxReceived(box.Id);

            if (!deliveryMutation.Succeeded)
            {
                RollBackInventory(destination, box);

                return ReceivingResult.Failure(
                    ReceivingFailureReason.DeliveryMutationRejected,
                    box.Id,
                    box.ProductId,
                    box.Quantity,
                    destinationBefore,
                    delivery.Status,
                    order.Status);
            }

            if (delivery.Status == DeliveryStatus.Received)
            {
                PurchaseOrderTransitionResult orderTransition =
                    order.MarkReceived();

                if (!orderTransition.Succeeded)
                {
                    throw new InvalidOperationException(
                        "Validated order receipt transition was rejected.");
                }
            }

            Quantity destinationAfter =
                destination.GetQuantity(box.ProductId);

            if (destinationAfter.Value - destinationBefore.Value !=
                box.Quantity.Value)
            {
                throw new InvalidOperationException(
                    "Receiving did not add exactly the box contents.");
            }

            return ReceivingResult.Success(
                box,
                destinationBefore,
                destinationAfter,
                delivery.Status,
                order.Status);
        }

        private static ReceivingResult FailureWithoutBox(
            ReceivingFailureReason failureReason,
            ShipmentBoxId boxId,
            Delivery delivery,
            PurchaseOrder order)
        {
            return ReceivingResult.Failure(
                failureReason,
                boxId,
                default(ProductDefinitionId),
                Quantity.Zero,
                Quantity.Zero,
                delivery.Status,
                order.Status);
        }

        private static void RollBackInventory(
            InventoryContainer destination,
            ShipmentBox box)
        {
            InventoryMutationResult rollback =
                destination.TryRemove(
                    box.ProductId,
                    box.Quantity);

            if (!rollback.Succeeded)
            {
                throw new InvalidOperationException(
                    "Receiving rollback failed.");
            }
        }
    }
}
