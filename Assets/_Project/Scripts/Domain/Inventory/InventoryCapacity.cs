using System;

namespace VRMGames.CartridgeAndCloud.Domain.Inventory
{
    public readonly struct InventoryCapacity :
        IEquatable<InventoryCapacity>
    {
        public int Units { get; }

        public InventoryCapacity(int units)
        {
            if (units < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(units),
                    "Inventory capacity cannot be negative.");
            }

            Units = units;
        }

        public bool Equals(InventoryCapacity other)
        {
            return Units == other.Units;
        }

        public override bool Equals(object obj)
        {
            return obj is InventoryCapacity other &&
                   Equals(other);
        }

        public override int GetHashCode()
        {
            return Units;
        }

        public override string ToString()
        {
            return Units.ToString();
        }

        public static bool operator ==(
            InventoryCapacity left,
            InventoryCapacity right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(
            InventoryCapacity left,
            InventoryCapacity right)
        {
            return !left.Equals(right);
        }
    }
}
