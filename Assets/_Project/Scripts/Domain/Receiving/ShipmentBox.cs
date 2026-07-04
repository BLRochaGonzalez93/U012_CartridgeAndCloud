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

    public readonly struct ShipmentBoxId :
        IEquatable<ShipmentBoxId>
    {
        public string Value { get; }

        public ShipmentBoxId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "Shipment box ID cannot be empty.",
                    nameof(value));
            }

            Value = value;
        }

        public bool Equals(ShipmentBoxId other)
        {
            return string.Equals(
                Value,
                other.Value,
                StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is ShipmentBoxId other &&
                   Equals(other);
        }

        public override int GetHashCode()
        {
            return StringComparer.Ordinal.GetHashCode(
                Value);
        }

        public override string ToString()
        {
            return Value;
        }

        public static bool operator ==(
            ShipmentBoxId left,
            ShipmentBoxId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(
            ShipmentBoxId left,
            ShipmentBoxId right)
        {
            return !left.Equals(right);
        }
    }
}
