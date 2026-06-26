using System;
using System.Collections.Generic;
using VRMGames.CartridgeAndCloud.Domain.Displays;
using VRMGames.CartridgeAndCloud.Domain.Shopping;

namespace VRMGames.CartridgeAndCloud.Application.Shopping
{
    public sealed class ShoppingCartService
    {
        private readonly ShoppingReservationRegistry _reservations;
        private readonly ShoppingReservationService _reservationService;

        public ShoppingCartService(
            ShoppingReservationRegistry reservations,
            ShoppingReservationService reservationService)
        {
            _reservations = reservations ?? throw new ArgumentNullException(nameof(reservations));
            _reservationService = reservationService ??
                throw new ArgumentNullException(nameof(reservationService));
        }

        public ShoppingCartMutationResult TryAddReservation(
            ShoppingCart cart,
            ShoppingReservationId reservationId)
        {
            if (cart == null) throw new ArgumentNullException(nameof(cart));
            return cart.TryAdd(_reservations.Get(reservationId));
        }

        public ShoppingCartMutationResult TryReleaseLine(
            ShoppingCart cart,
            ShoppingReservationId reservationId,
            DisplayInstanceRegistry displays)
        {
            if (cart == null) throw new ArgumentNullException(nameof(cart));
            if (!cart.ContainsReservation(reservationId))
                return ShoppingCartMutationResult.Failure(
                    ShoppingCartMutationFailureReason.ReservationNotInCart,
                    cart.TotalUnits);

            ShoppingReservationResult release =
                _reservationService.TryRelease(reservationId, displays);
            if (!release.Succeeded)
                return ShoppingCartMutationResult.Failure(
                    ShoppingCartMutationFailureReason.ReservationNotActive,
                    cart.TotalUnits);

            return cart.TryRemove(reservationId);
        }

        public int Abandon(ShoppingCart cart, DisplayInstanceRegistry displays)
        {
            if (cart == null) throw new ArgumentNullException(nameof(cart));
            List<ShoppingReservationId> ids = new List<ShoppingReservationId>();
            foreach (ShoppingCartLine line in cart.Lines)
                ids.Add(line.ReservationId);

            int released = 0;
            foreach (ShoppingReservationId id in ids)
            {
                if (TryReleaseLine(cart, id, displays).Succeeded)
                    released++;
            }
            return released;
        }
    }

    public sealed class ShoppingConsistencyReport
    {
        public bool IsConsistent { get; }
        public int CheckedDisplays { get; }
        public int CheckedCartLines { get; }
        public string Error { get; }

        public ShoppingConsistencyReport(
            bool consistent,
            int checkedDisplays,
            int checkedCartLines,
            string error)
        {
            IsConsistent = consistent;
            CheckedDisplays = checkedDisplays;
            CheckedCartLines = checkedCartLines;
            Error = error ?? string.Empty;
        }
    }

    public sealed class ShoppingConsistencyService
    {
        private readonly ShoppingReservationRegistry _reservations;
        private readonly ShoppingAvailabilityService _availability;

        public ShoppingConsistencyService(ShoppingReservationRegistry reservations)
        {
            _reservations = reservations ?? throw new ArgumentNullException(nameof(reservations));
            _availability = new ShoppingAvailabilityService(_reservations);
        }

        public ShoppingConsistencyReport Validate(
            DisplayInstanceRegistry displays,
            ShoppingCart cart)
        {
            if (displays == null) throw new ArgumentNullException(nameof(displays));
            if (cart == null) throw new ArgumentNullException(nameof(cart));

            int checkedDisplays = 0;
            foreach (DisplayInstance display in displays.Instances)
            {
                checkedDisplays++;
                try
                {
                    _availability.GetAvailability(display);
                }
                catch (InvalidOperationException exception)
                {
                    return new ShoppingConsistencyReport(
                        false,
                        checkedDisplays,
                        0,
                        exception.Message);
                }
            }

            int checkedLines = 0;
            HashSet<ShoppingReservationId> unique = new HashSet<ShoppingReservationId>();
            foreach (ShoppingCartLine line in cart.Lines)
            {
                checkedLines++;
                if (!unique.Add(line.ReservationId))
                    return new ShoppingConsistencyReport(
                        false, checkedDisplays, checkedLines, "Duplicate cart reservation.");

                if (!_reservations.TryGet(line.ReservationId, out ShoppingReservation reservation) ||
                    !reservation.IsActive ||
                    reservation.CustomerId != cart.CustomerId ||
                    reservation.DisplayId != line.DisplayId ||
                    reservation.ProductId != line.ProductId ||
                    reservation.Quantity != line.Quantity)
                {
                    return new ShoppingConsistencyReport(
                        false,
                        checkedDisplays,
                        checkedLines,
                        "Cart line lacks a matching active reservation.");
                }
            }

            return new ShoppingConsistencyReport(
                true,
                checkedDisplays,
                checkedLines,
                string.Empty);
        }
    }
}
