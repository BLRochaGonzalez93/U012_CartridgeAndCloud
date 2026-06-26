using System;
using VRMGames.CartridgeAndCloud.Domain.Customers;
using VRMGames.CartridgeAndCloud.Domain.Displays;
using VRMGames.CartridgeAndCloud.Domain.Inventory;
using VRMGames.CartridgeAndCloud.Domain.Products;
using VRMGames.CartridgeAndCloud.Domain.Shopping;

namespace VRMGames.CartridgeAndCloud.Application.Shopping
{
    public enum ShoppingReservationFailureReason
    {
        None = 0,
        DuplicateReservationId = 1,
        InvalidQuantity = 2,
        QuantityExceedsPolicy = 3,
        DisplayHasNoAssignedProduct = 4,
        ProductMismatch = 5,
        ProductDefinitionMissing = 6,
        InsufficientAvailableQuantity = 7,
        ReservationNotFound = 8,
        ReservationNotActive = 9
    }

    public sealed class ShoppingReservationResult
    {
        public bool Succeeded { get; }
        public ShoppingReservationFailureReason FailureReason { get; }
        public ShoppingReservation Reservation { get; }
        public Quantity AvailableBefore { get; }
        public Quantity AvailableAfter { get; }

        private ShoppingReservationResult(
            bool succeeded,
            ShoppingReservationFailureReason reason,
            ShoppingReservation reservation,
            Quantity before,
            Quantity after)
        {
            Succeeded = succeeded;
            FailureReason = reason;
            Reservation = reservation;
            AvailableBefore = before;
            AvailableAfter = after;
        }

        public static ShoppingReservationResult Success(
            ShoppingReservation reservation,
            Quantity before,
            Quantity after) =>
            new ShoppingReservationResult(
                true,
                ShoppingReservationFailureReason.None,
                reservation,
                before,
                after);

        public static ShoppingReservationResult Failure(
            ShoppingReservationFailureReason reason,
            Quantity unchanged) =>
            new ShoppingReservationResult(false, reason, null, unchanged, unchanged);
    }

    public sealed class ShoppingReservationService
    {
        private readonly ProductDefinitionRegistry _products;
        private readonly ShoppingReservationRegistry _reservations;
        private readonly ShoppingPolicy _policy;
        private readonly ShoppingAvailabilityService _availability;

        public ShoppingReservationService(
            ProductDefinitionRegistry products,
            ShoppingReservationRegistry reservations,
            ShoppingPolicy policy)
        {
            _products = products ?? throw new ArgumentNullException(nameof(products));
            _reservations = reservations ?? throw new ArgumentNullException(nameof(reservations));
            _policy = policy ?? throw new ArgumentNullException(nameof(policy));
            _availability = new ShoppingAvailabilityService(_reservations);
        }

        public ShoppingReservationResult TryReserve(
            ShoppingReservationId reservationId,
            CustomerInstanceId customerId,
            DisplayInstance display,
            ProductDefinitionId productId,
            Quantity quantity)
        {
            if (display == null) throw new ArgumentNullException(nameof(display));
            ShoppingAvailabilitySnapshot snapshot = _availability.GetAvailability(display);

            if (_reservations.Contains(reservationId))
                return ShoppingReservationResult.Failure(
                    ShoppingReservationFailureReason.DuplicateReservationId,
                    snapshot.Available);
            if (quantity.IsZero)
                return ShoppingReservationResult.Failure(
                    ShoppingReservationFailureReason.InvalidQuantity,
                    snapshot.Available);
            if (quantity.Value > _policy.MaxUnitsPerReservation)
                return ShoppingReservationResult.Failure(
                    ShoppingReservationFailureReason.QuantityExceedsPolicy,
                    snapshot.Available);
            if (!display.HasAssignedProduct)
                return ShoppingReservationResult.Failure(
                    ShoppingReservationFailureReason.DisplayHasNoAssignedProduct,
                    snapshot.Available);
            if (display.AssignedProductId != productId)
                return ShoppingReservationResult.Failure(
                    ShoppingReservationFailureReason.ProductMismatch,
                    snapshot.Available);
            if (!_products.Contains(productId))
                return ShoppingReservationResult.Failure(
                    ShoppingReservationFailureReason.ProductDefinitionMissing,
                    snapshot.Available);
            if (quantity > snapshot.Available)
                return ShoppingReservationResult.Failure(
                    ShoppingReservationFailureReason.InsufficientAvailableQuantity,
                    snapshot.Available);

            ShoppingReservation reservation = new ShoppingReservation(
                reservationId,
                customerId,
                display.Id,
                productId,
                quantity);

            if (!_reservations.TryRegister(reservation))
                return ShoppingReservationResult.Failure(
                    ShoppingReservationFailureReason.DuplicateReservationId,
                    snapshot.Available);

            return ShoppingReservationResult.Success(
                reservation,
                snapshot.Available,
                new Quantity(snapshot.Available.Value - quantity.Value));
        }

        public ShoppingReservationResult TryRelease(
            ShoppingReservationId reservationId,
            DisplayInstanceRegistry displays)
        {
            if (displays == null) throw new ArgumentNullException(nameof(displays));
            if (!_reservations.TryGet(reservationId, out ShoppingReservation reservation))
                return ShoppingReservationResult.Failure(
                    ShoppingReservationFailureReason.ReservationNotFound,
                    Quantity.Zero);

            if (!displays.TryGet(reservation.DisplayId, out DisplayInstance display))
                throw new InvalidOperationException("Reservation references a missing display.");

            ShoppingAvailabilitySnapshot before = _availability.GetAvailability(display);
            if (!reservation.TryRelease())
                return ShoppingReservationResult.Failure(
                    ShoppingReservationFailureReason.ReservationNotActive,
                    before.Available);
            ShoppingAvailabilitySnapshot after = _availability.GetAvailability(display);
            return ShoppingReservationResult.Success(
                reservation,
                before.Available,
                after.Available);
        }
    }
}
