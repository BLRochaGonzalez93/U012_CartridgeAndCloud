using System;

namespace VRMGames.CartridgeAndCloud.Domain.Customers
{
    public readonly struct CustomerProfileId : IEquatable<CustomerProfileId>
    {
        public string Value { get; }

        public CustomerProfileId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "Customer profile ID cannot be empty.",
                    nameof(value));
            }

            Value = value;
        }

        public bool Equals(CustomerProfileId other)
        {
            return string.Equals(
                Value,
                other.Value,
                StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is CustomerProfileId other && Equals(other);
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

        public static bool operator ==(CustomerProfileId left, CustomerProfileId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CustomerProfileId left, CustomerProfileId right)
        {
            return !left.Equals(right);
        }
    }
}
