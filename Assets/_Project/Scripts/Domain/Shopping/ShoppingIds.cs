using System;

namespace VRMGames.CartridgeAndCloud.Domain.Shopping
{
    public readonly struct ShoppingIntentId : IEquatable<ShoppingIntentId>
    {
        public string Value { get; }
        public ShoppingIntentId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Shopping intent ID cannot be empty.", nameof(value));
            Value = value;
        }
        public bool Equals(ShoppingIntentId other) =>
            string.Equals(Value, other.Value, StringComparison.Ordinal);
        public override bool Equals(object obj) =>
            obj is ShoppingIntentId other && Equals(other);
        public override int GetHashCode() =>
            StringComparer.Ordinal.GetHashCode(Value ?? string.Empty);
        public override string ToString() => Value ?? string.Empty;
        public static bool operator ==(ShoppingIntentId left, ShoppingIntentId right) => left.Equals(right);
        public static bool operator !=(ShoppingIntentId left, ShoppingIntentId right) => !left.Equals(right);
    }

    public readonly struct ShoppingReservationId : IEquatable<ShoppingReservationId>
    {
        public string Value { get; }
        public ShoppingReservationId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Shopping reservation ID cannot be empty.", nameof(value));
            Value = value;
        }
        public bool Equals(ShoppingReservationId other) =>
            string.Equals(Value, other.Value, StringComparison.Ordinal);
        public override bool Equals(object obj) =>
            obj is ShoppingReservationId other && Equals(other);
        public override int GetHashCode() =>
            StringComparer.Ordinal.GetHashCode(Value ?? string.Empty);
        public override string ToString() => Value ?? string.Empty;
        public static bool operator ==(ShoppingReservationId left, ShoppingReservationId right) => left.Equals(right);
        public static bool operator !=(ShoppingReservationId left, ShoppingReservationId right) => !left.Equals(right);
    }

    public readonly struct ShoppingCartId : IEquatable<ShoppingCartId>
    {
        public string Value { get; }
        public ShoppingCartId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Shopping cart ID cannot be empty.", nameof(value));
            Value = value;
        }
        public bool Equals(ShoppingCartId other) =>
            string.Equals(Value, other.Value, StringComparison.Ordinal);
        public override bool Equals(object obj) =>
            obj is ShoppingCartId other && Equals(other);
        public override int GetHashCode() =>
            StringComparer.Ordinal.GetHashCode(Value ?? string.Empty);
        public override string ToString() => Value ?? string.Empty;
        public static bool operator ==(ShoppingCartId left, ShoppingCartId right) => left.Equals(right);
        public static bool operator !=(ShoppingCartId left, ShoppingCartId right) => !left.Equals(right);
    }
}
