using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VRMGames.CartridgeAndCloud.Domain.Customers;
using VRMGames.CartridgeAndCloud.Domain.Displays;
using VRMGames.CartridgeAndCloud.Domain.Inventory;
using VRMGames.CartridgeAndCloud.Domain.Products;

namespace VRMGames.CartridgeAndCloud.Domain.Shopping
{
    public enum ShoppingCartMutationFailureReason
    {
        None = 0,
        ReservationNotActive = 1,
        ReservationOwnedByDifferentCustomer = 2,
        ReservationAlreadyInCart = 3,
        CartCapacityExceeded = 4,
        ReservationNotInCart = 5
    }

    public sealed class ShoppingCartMutationResult
    {
        public bool Succeeded { get; }
        public ShoppingCartMutationFailureReason FailureReason { get; }
        public int TotalUnitsBefore { get; }
        public int TotalUnitsAfter { get; }

        private ShoppingCartMutationResult(
            bool succeeded,
            ShoppingCartMutationFailureReason reason,
            int before,
            int after)
        {
            Succeeded = succeeded;
            FailureReason = reason;
            TotalUnitsBefore = before;
            TotalUnitsAfter = after;
        }

        public static ShoppingCartMutationResult Success(int before, int after) =>
            new ShoppingCartMutationResult(true, ShoppingCartMutationFailureReason.None, before, after);

        public static ShoppingCartMutationResult Failure(
            ShoppingCartMutationFailureReason reason,
            int unchanged) =>
            new ShoppingCartMutationResult(false, reason, unchanged, unchanged);
    }

    public sealed class ShoppingCartLine
    {
        public ShoppingReservationId ReservationId { get; }
        public DisplayInstanceId DisplayId { get; }
        public ProductDefinitionId ProductId { get; }
        public Quantity Quantity { get; }

        public ShoppingCartLine(ShoppingReservation reservation)
        {
            ReservationId = reservation.Id;
            DisplayId = reservation.DisplayId;
            ProductId = reservation.ProductId;
            Quantity = reservation.Quantity;
        }
    }

    public sealed class ShoppingCart
    {
        private readonly Dictionary<ShoppingReservationId, ShoppingCartLine> _lines =
            new Dictionary<ShoppingReservationId, ShoppingCartLine>();

        public ShoppingCartId Id { get; }
        public CustomerInstanceId CustomerId { get; }
        public int CapacityUnits { get; }
        public int TotalUnits { get; private set; }
        public int LineCount => _lines.Count;
        public bool IsEmpty => TotalUnits == 0;
        public IReadOnlyList<ShoppingCartLine> Lines => CreateSnapshot();

        public ShoppingCart(ShoppingCartId id, CustomerInstanceId customerId, int capacityUnits)
        {
            if (string.IsNullOrWhiteSpace(customerId.Value))
                throw new ArgumentException("Customer ID must be initialized.", nameof(customerId));
            if (capacityUnits <= 0)
                throw new ArgumentOutOfRangeException(nameof(capacityUnits));
            Id = id;
            CustomerId = customerId;
            CapacityUnits = capacityUnits;
        }

        public ShoppingCartMutationResult TryAdd(ShoppingReservation reservation)
        {
            if (reservation == null) throw new ArgumentNullException(nameof(reservation));
            int before = TotalUnits;
            if (!reservation.IsActive)
                return ShoppingCartMutationResult.Failure(
                    ShoppingCartMutationFailureReason.ReservationNotActive, before);
            if (reservation.CustomerId != CustomerId)
                return ShoppingCartMutationResult.Failure(
                    ShoppingCartMutationFailureReason.ReservationOwnedByDifferentCustomer, before);
            if (_lines.ContainsKey(reservation.Id))
                return ShoppingCartMutationResult.Failure(
                    ShoppingCartMutationFailureReason.ReservationAlreadyInCart, before);
            int next = checked(TotalUnits + reservation.Quantity.Value);
            if (next > CapacityUnits)
                return ShoppingCartMutationResult.Failure(
                    ShoppingCartMutationFailureReason.CartCapacityExceeded, before);
            _lines.Add(reservation.Id, new ShoppingCartLine(reservation));
            TotalUnits = next;
            return ShoppingCartMutationResult.Success(before, TotalUnits);
        }

        public ShoppingCartMutationResult TryRemove(ShoppingReservationId reservationId)
        {
            int before = TotalUnits;
            if (!_lines.TryGetValue(reservationId, out ShoppingCartLine line))
                return ShoppingCartMutationResult.Failure(
                    ShoppingCartMutationFailureReason.ReservationNotInCart, before);
            _lines.Remove(reservationId);
            TotalUnits -= line.Quantity.Value;
            return ShoppingCartMutationResult.Success(before, TotalUnits);
        }

        public bool ContainsReservation(ShoppingReservationId reservationId) =>
            _lines.ContainsKey(reservationId);

        private IReadOnlyList<ShoppingCartLine> CreateSnapshot()
        {
            List<ShoppingCartLine> lines = new List<ShoppingCartLine>(_lines.Values);
            lines.Sort((left, right) =>
                StringComparer.Ordinal.Compare(left.ReservationId.Value, right.ReservationId.Value));
            return new ReadOnlyCollection<ShoppingCartLine>(lines);
        }
    }

    public enum CustomerShoppingState
    {
        Searching = 0,
        HoldingReservations = 1,
        ReadyForCheckout = 2,
        Abandoned = 3
    }

    public sealed class CustomerShoppingSession
    {
        public CustomerInstanceId CustomerId { get; }
        public ShoppingIntent Intent { get; }
        public ShoppingCart Cart { get; }
        public CustomerShoppingState State { get; private set; }

        public CustomerShoppingSession(
            CustomerInstanceId customerId,
            ShoppingIntent intent,
            ShoppingCart cart)
        {
            if (intent == null) throw new ArgumentNullException(nameof(intent));
            if (cart == null) throw new ArgumentNullException(nameof(cart));
            if (intent.CustomerId != customerId || cart.CustomerId != customerId)
                throw new ArgumentException("Intent and cart must belong to the session customer.");
            CustomerId = customerId;
            Intent = intent;
            Cart = cart;
            State = CustomerShoppingState.Searching;
        }

        public bool TryMarkHoldingReservations()
        {
            if (State != CustomerShoppingState.Searching || Cart.IsEmpty) return false;
            State = CustomerShoppingState.HoldingReservations;
            return true;
        }

        public bool TryMarkReadyForCheckout()
        {
            if ((State != CustomerShoppingState.Searching &&
                 State != CustomerShoppingState.HoldingReservations) ||
                Cart.IsEmpty)
                return false;
            State = CustomerShoppingState.ReadyForCheckout;
            return true;
        }

        public bool TryAbandon()
        {
            if (State == CustomerShoppingState.Abandoned) return false;
            State = CustomerShoppingState.Abandoned;
            return true;
        }
    }
}
