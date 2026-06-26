using System;

namespace VRMGames.CartridgeAndCloud.Domain.Checkout
{
    public readonly struct CheckoutQueueEntryId :
        IEquatable<CheckoutQueueEntryId>
    {
        public string Value { get; }

        public CheckoutQueueEntryId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "Checkout queue entry ID cannot be empty.",
                    nameof(value));
            }

            Value = value;
        }

        public bool Equals(CheckoutQueueEntryId other)
        {
            return string.Equals(
                Value,
                other.Value,
                StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is CheckoutQueueEntryId other &&
                Equals(other);
        }

        public override int GetHashCode()
        {
            return StringComparer.Ordinal.GetHashCode(
                Value ?? string.Empty);
        }

        public override string ToString()
        {
            return Value ?? string.Empty;
        }

        public static bool operator ==(
            CheckoutQueueEntryId left,
            CheckoutQueueEntryId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(
            CheckoutQueueEntryId left,
            CheckoutQueueEntryId right)
        {
            return !left.Equals(right);
        }
    }

    public readonly struct CheckoutStationId :
        IEquatable<CheckoutStationId>
    {
        public string Value { get; }

        public CheckoutStationId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "Checkout station ID cannot be empty.",
                    nameof(value));
            }

            Value = value;
        }

        public bool Equals(CheckoutStationId other)
        {
            return string.Equals(
                Value,
                other.Value,
                StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is CheckoutStationId other &&
                Equals(other);
        }

        public override int GetHashCode()
        {
            return StringComparer.Ordinal.GetHashCode(
                Value ?? string.Empty);
        }

        public override string ToString()
        {
            return Value ?? string.Empty;
        }

        public static bool operator ==(
            CheckoutStationId left,
            CheckoutStationId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(
            CheckoutStationId left,
            CheckoutStationId right)
        {
            return !left.Equals(right);
        }
    }

    public readonly struct CheckoutTransactionId :
        IEquatable<CheckoutTransactionId>
    {
        public string Value { get; }

        public CheckoutTransactionId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "Checkout transaction ID cannot be empty.",
                    nameof(value));
            }

            Value = value;
        }

        public bool Equals(CheckoutTransactionId other)
        {
            return string.Equals(
                Value,
                other.Value,
                StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is CheckoutTransactionId other &&
                Equals(other);
        }

        public override int GetHashCode()
        {
            return StringComparer.Ordinal.GetHashCode(
                Value ?? string.Empty);
        }

        public override string ToString()
        {
            return Value ?? string.Empty;
        }

        public static bool operator ==(
            CheckoutTransactionId left,
            CheckoutTransactionId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(
            CheckoutTransactionId left,
            CheckoutTransactionId right)
        {
            return !left.Equals(right);
        }
    }

    public sealed class CheckoutPolicy
    {
        public int MaxQueueLength { get; }

        public CheckoutPolicy(int maxQueueLength)
        {
            if (maxQueueLength <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(maxQueueLength));
            }

            MaxQueueLength = maxQueueLength;
        }
    }
}
