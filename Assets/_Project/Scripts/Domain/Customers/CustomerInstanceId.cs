using System;

namespace VRMGames.CartridgeAndCloud.Domain.Customers
{
    public readonly struct CustomerInstanceId : IEquatable<CustomerInstanceId>
    {
        public string Value { get; }

        public CustomerInstanceId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "Customer instance ID cannot be empty.",
                    nameof(value));
            }

            Value = value;
        }

        public bool Equals(CustomerInstanceId other)
        {
            return string.Equals(
                Value,
                other.Value,
                StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is CustomerInstanceId other && Equals(other);
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

        public static bool operator ==(CustomerInstanceId left, CustomerInstanceId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CustomerInstanceId left, CustomerInstanceId right)
        {
            return !left.Equals(right);
        }
    }
}
