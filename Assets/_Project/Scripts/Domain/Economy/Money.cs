using System;

namespace VRMGames.CartridgeAndCloud.Domain.Economy
{
    public readonly struct CurrencyCode :
        IEquatable<CurrencyCode>
    {
        public string Value { get; }

        public CurrencyCode(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "Currency code cannot be empty.",
                    nameof(value));
            }

            string normalized =
                value.Trim().ToUpperInvariant();

            if (normalized.Length != 3)
            {
                throw new ArgumentException(
                    "Currency code must contain exactly three characters.",
                    nameof(value));
            }

            for (int index = 0;
                 index < normalized.Length;
                 index++)
            {
                if (normalized[index] < 'A' ||
                    normalized[index] > 'Z')
                {
                    throw new ArgumentException(
                        "Currency code must contain only ASCII letters.",
                        nameof(value));
                }
            }

            Value = normalized;
        }

        public bool Equals(CurrencyCode other)
        {
            return string.Equals(
                Value,
                other.Value,
                StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is CurrencyCode other &&
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
            CurrencyCode left,
            CurrencyCode right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(
            CurrencyCode left,
            CurrencyCode right)
        {
            return !left.Equals(right);
        }
    }

    public readonly struct Money :
        IEquatable<Money>,
        IComparable<Money>
    {
        public long MinorUnits { get; }

        public CurrencyCode Currency { get; }

        public bool IsZero => MinorUnits == 0;

        public bool IsPositive => MinorUnits > 0;

        public bool IsNegative => MinorUnits < 0;

        public Money(
            long minorUnits,
            CurrencyCode currency)
        {
            if (string.IsNullOrWhiteSpace(currency.Value))
            {
                throw new ArgumentException(
                    "Currency must be initialized.",
                    nameof(currency));
            }

            MinorUnits = minorUnits;
            Currency = currency;
        }

        public static Money Zero(CurrencyCode currency)
        {
            return new Money(0, currency);
        }

        public Money Add(Money other)
        {
            EnsureSameCurrency(other);

            return new Money(
                checked(MinorUnits + other.MinorUnits),
                Currency);
        }

        public Money Subtract(Money other)
        {
            EnsureSameCurrency(other);

            return new Money(
                checked(MinorUnits - other.MinorUnits),
                Currency);
        }

        public Money Multiply(int multiplier)
        {
            if (multiplier < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(multiplier));
            }

            return new Money(
                checked(MinorUnits * multiplier),
                Currency);
        }

        public int CompareTo(Money other)
        {
            EnsureSameCurrency(other);
            return MinorUnits.CompareTo(other.MinorUnits);
        }

        public bool Equals(Money other)
        {
            return MinorUnits == other.MinorUnits &&
                Currency == other.Currency;
        }

        public override bool Equals(object obj)
        {
            return obj is Money other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (MinorUnits.GetHashCode() * 397) ^
                    Currency.GetHashCode();
            }
        }

        public override string ToString()
        {
            return $"{Currency} {MinorUnits}";
        }

        public static bool operator ==(
            Money left,
            Money right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(
            Money left,
            Money right)
        {
            return !left.Equals(right);
        }

        public static bool operator <(
            Money left,
            Money right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator >(
            Money left,
            Money right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator <=(
            Money left,
            Money right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >=(
            Money left,
            Money right)
        {
            return left.CompareTo(right) >= 0;
        }

        private void EnsureSameCurrency(Money other)
        {
            if (Currency != other.Currency)
            {
                throw new InvalidOperationException(
                    $"Cannot combine {Currency} and {other.Currency}.");
            }
        }
    }
}
