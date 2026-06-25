using System;

namespace VRMGames.CartridgeAndCloud.Domain.Customers
{
    public readonly struct CustomerNavigationPointId : IEquatable<CustomerNavigationPointId>
    {
        public string Value { get; }

        public CustomerNavigationPointId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "Customer navigation point ID cannot be empty.",
                    nameof(value));
            }

            Value = value;
        }

        public bool Equals(CustomerNavigationPointId other)
        {
            return string.Equals(
                Value,
                other.Value,
                StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is CustomerNavigationPointId other && Equals(other);
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

        public static bool operator ==(CustomerNavigationPointId left, CustomerNavigationPointId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CustomerNavigationPointId left, CustomerNavigationPointId right)
        {
            return !left.Equals(right);
        }
    }
}
