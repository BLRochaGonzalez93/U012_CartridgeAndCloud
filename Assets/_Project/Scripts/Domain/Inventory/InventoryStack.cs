using System;
using VRMGames.CartridgeAndCloud.Domain.Products;

namespace VRMGames.CartridgeAndCloud.Domain.Inventory
{
    public readonly struct InventoryStack :
        IEquatable<InventoryStack>
    {
        public ProductDefinitionId ProductId { get; }

        public Quantity Quantity { get; }

        public InventoryStack(
            ProductDefinitionId productId,
            Quantity quantity)
        {
            if (string.IsNullOrWhiteSpace(productId.Value))
            {
                throw new ArgumentException(
                    "Product definition ID must be initialized.",
                    nameof(productId));
            }

            if (quantity.IsZero)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(quantity),
                    "Inventory stacks must contain at least one unit.");
            }

            ProductId = productId;
            Quantity = quantity;
        }

        public bool Equals(InventoryStack other)
        {
            return ProductId == other.ProductId &&
                   Quantity == other.Quantity;
        }

        public override bool Equals(object obj)
        {
            return obj is InventoryStack other &&
                   Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (ProductId.GetHashCode() * 397) ^
                       Quantity.GetHashCode();
            }
        }

        public override string ToString()
        {
            return $"{ProductId}: {Quantity}";
        }

        public static bool operator ==(
            InventoryStack left,
            InventoryStack right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(
            InventoryStack left,
            InventoryStack right)
        {
            return !left.Equals(right);
        }
    }
}
