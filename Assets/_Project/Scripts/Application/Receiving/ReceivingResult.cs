using VRMGames.CartridgeAndCloud.Domain.Inventory;
using VRMGames.CartridgeAndCloud.Domain.Orders;
using VRMGames.CartridgeAndCloud.Domain.Products;
using VRMGames.CartridgeAndCloud.Domain.Receiving;

namespace VRMGames.CartridgeAndCloud.Application.Receiving
{
    public sealed class ReceivingResult
    {
        public bool Succeeded { get; }

        public ReceivingFailureReason FailureReason { get; }

        public ShipmentBoxId BoxId { get; }

        public ProductDefinitionId ProductId { get; }

        public Quantity Quantity { get; }

        public Quantity DestinationBefore { get; }

        public Quantity DestinationAfter { get; }

        public DeliveryStatus DeliveryStatus { get; }

        public PurchaseOrderStatus OrderStatus { get; }

        private ReceivingResult(
            bool succeeded,
            ReceivingFailureReason failureReason,
            ShipmentBoxId boxId,
            ProductDefinitionId productId,
            Quantity quantity,
            Quantity destinationBefore,
            Quantity destinationAfter,
            DeliveryStatus deliveryStatus,
            PurchaseOrderStatus orderStatus)
        {
            Succeeded = succeeded;
            FailureReason = failureReason;
            BoxId = boxId;
            ProductId = productId;
            Quantity = quantity;
            DestinationBefore = destinationBefore;
            DestinationAfter = destinationAfter;
            DeliveryStatus = deliveryStatus;
            OrderStatus = orderStatus;
        }

        public static ReceivingResult Success(
            ShipmentBox box,
            Quantity destinationBefore,
            Quantity destinationAfter,
            DeliveryStatus deliveryStatus,
            PurchaseOrderStatus orderStatus)
        {
            return new ReceivingResult(
                true,
                ReceivingFailureReason.None,
                box.Id,
                box.ProductId,
                box.Quantity,
                destinationBefore,
                destinationAfter,
                deliveryStatus,
                orderStatus);
        }

        public static ReceivingResult Failure(
            ReceivingFailureReason failureReason,
            ShipmentBoxId boxId,
            ProductDefinitionId productId,
            Quantity quantity,
            Quantity destinationQuantity,
            DeliveryStatus deliveryStatus,
            PurchaseOrderStatus orderStatus)
        {
            return new ReceivingResult(
                false,
                failureReason,
                boxId,
                productId,
                quantity,
                destinationQuantity,
                destinationQuantity,
                deliveryStatus,
                orderStatus);
        }
    }

    public enum ReceivingFailureReason
    {
        None = 0,
        OrderNotDelivered = 1,
        DeliveryOrderMismatch = 2,
        DeliverySupplierMismatch = 3,
        BoxNotFound = 4,
        BoxAlreadyReceived = 5,
        ProductDefinitionMissing = 6,
        DestinationCapacityExceeded = 7,
        InventoryMutationRejected = 8,
        DeliveryMutationRejected = 9
    }
}
