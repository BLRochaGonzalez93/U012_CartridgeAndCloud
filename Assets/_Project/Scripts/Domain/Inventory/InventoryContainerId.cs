using System;

namespace VRMGames.CartridgeAndCloud.Domain.Inventory
{
    public readonly struct InventoryContainerId :
        IEquatable<InventoryContainerId>
    {
        public string Value { get; }

        public InventoryContainerId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "Inventory container ID cannot be empty.",
                    nameof(value));
            }

            Value = value;
        }

        public bool Equals(InventoryContainerId other)
        {
            return string.Equals(
                Value,
                other.Value,
                StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is InventoryContainerId other &&
                   Equals(other);
        }

        public override int GetHashCode()
        {
            return Value == null
                ? 0
                : StringComparer.Ordinal.GetHashCode(Value);
        }

        public override string ToString()
        {
            return Value ?? string.Empty;
        }

        public static bool operator ==(
            InventoryContainerId left,
            InventoryContainerId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(
            InventoryContainerId left,
            InventoryContainerId right)
        {
            return !left.Equals(right);
        }
    }
}
