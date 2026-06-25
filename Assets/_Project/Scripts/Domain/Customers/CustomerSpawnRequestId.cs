using System;

namespace VRMGames.CartridgeAndCloud.Domain.Customers
{
    public readonly struct CustomerSpawnRequestId : IEquatable<CustomerSpawnRequestId>
    {
        public string Value { get; }

        public CustomerSpawnRequestId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "Customer spawn request ID cannot be empty.",
                    nameof(value));
            }

            Value = value;
        }

        public bool Equals(CustomerSpawnRequestId other)
        {
            return string.Equals(
                Value,
                other.Value,
                StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is CustomerSpawnRequestId other && Equals(other);
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

        public static bool operator ==(CustomerSpawnRequestId left, CustomerSpawnRequestId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CustomerSpawnRequestId left, CustomerSpawnRequestId right)
        {
            return !left.Equals(right);
        }
    }
}
