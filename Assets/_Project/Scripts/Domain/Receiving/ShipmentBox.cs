using System;
using VRMGames.CartridgeAndCloud.Domain.Inventory;
using VRMGames.CartridgeAndCloud.Domain.Orders;
using VRMGames.CartridgeAndCloud.Domain.Products;

namespace VRMGames.CartridgeAndCloud.Domain.Receiving
{
    public sealed class ShipmentBox
    {
        public ShipmentBoxId Id { get; }

        public PurchaseOrderId OrderId { get; }

        public ProductDefinitionId ProductId { get; }

        public Quantity Quantity { get; }

        public bool IsReceived { get; private set; }

        public ShipmentBox(
            ShipmentBoxId id,
            PurchaseOrderId orderId,
            ProductDefinitionId productId,
            Quantity quantity)
        {
            if (string.IsNullOrWhiteSpace(id.Value))
            {
                throw new ArgumentException(
                    "Shipment box ID must be initialized.",
                    nameof(id));
            }

            if (string.IsNullOrWhiteSpace(orderId.Value))
            {
                throw new ArgumentException(
                    "Purchase order ID must be initialized.",
                    nameof(orderId));
            }

            if (string.IsNullOrWhiteSpace(productId.Value))
            {
                throw new ArgumentException(
                    "Product definition ID must be initialized.",
                    nameof(productId));
            }

            if (quantity.IsZero)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(quantity));
            }

            Id = id;
            OrderId = orderId;
            ProductId = productId;
            Quantity = quantity;
        }

        internal void MarkReceived()
        {
            if (IsReceived)
            {
                throw new InvalidOperationException(
                    "Shipment box was already received.");
            }

            IsReceived = true;
        }
    }
}
