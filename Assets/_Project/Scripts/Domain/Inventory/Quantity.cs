using System;

namespace VRMGames.CartridgeAndCloud.Domain.Inventory
{
    public readonly struct Quantity :
        IEquatable<Quantity>,
        IComparable<Quantity>
    {
        public static Quantity Zero => new Quantity(0);

        public int Value { get; }

        public bool IsZero => Value == 0;

        public Quantity(int value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(value),
                    "Quantity cannot be negative.");
            }

            Value = value;
        }

        public Quantity Add(Quantity other)
        {
            return new Quantity(
                checked(Value + other.Value));
        }

        public Quantity Subtract(Quantity other)
        {
            if (other.Value > Value)
            {
                throw new InvalidOperationException(
                    "Quantity subtraction cannot produce a negative value.");
            }

            return new Quantity(Value - other.Value);
        }

        public int CompareTo(Quantity other)
        {
            return Value.CompareTo(other.Value);
        }

        public bool Equals(Quantity other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            return obj is Quantity other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static Quantity operator +(
            Quantity left,
            Quantity right)
        {
            return left.Add(right);
        }

        public static Quantity operator -(
            Quantity left,
            Quantity right)
        {
            return left.Subtract(right);
        }

        public static bool operator ==(
            Quantity left,
            Quantity right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(
            Quantity left,
            Quantity right)
        {
            return !left.Equals(right);
        }

        public static bool operator <(
            Quantity left,
            Quantity right)
        {
            return left.Value < right.Value;
        }

        public static bool operator >(
            Quantity left,
            Quantity right)
        {
            return left.Value > right.Value;
        }

        public static bool operator <=(
            Quantity left,
            Quantity right)
        {
            return left.Value <= right.Value;
        }

        public static bool operator >=(
            Quantity left,
            Quantity right)
        {
            return left.Value >= right.Value;
        }
    }
}
