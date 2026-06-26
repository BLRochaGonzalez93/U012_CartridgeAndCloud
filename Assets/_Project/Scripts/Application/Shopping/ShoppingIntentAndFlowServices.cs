using System;
using System.Collections.Generic;
using VRMGames.CartridgeAndCloud.Domain.Customers;
using VRMGames.CartridgeAndCloud.Domain.Displays;
using VRMGames.CartridgeAndCloud.Domain.Inventory;
using VRMGames.CartridgeAndCloud.Domain.Shopping;

namespace VRMGames.CartridgeAndCloud.Application.Shopping
{
    public sealed class ShoppingIntentFactory
    {
        public ShoppingIntent CreateFromProfile(
            ShoppingIntentId intentId,
            CustomerInstanceId customerId,
            CustomerProfile profile,
            int desiredUnits)
        {
            if (profile == null) throw new ArgumentNullException(nameof(profile));
            return new ShoppingIntent(
                intentId,
                customerId,
                profile.PreferredCategoryIds,
                desiredUnits);
        }
    }

    public enum ShoppingFlowFailureReason
    {
        None = 0,
        CustomerNotBrowsing = 1,
        OwnershipMismatch = 2,
        NoCandidate = 3,
        ReservationRejected = 4,
        CartRejected = 5
    }

    public sealed class ShoppingFlowResult
    {
        public bool Succeeded { get; }
        public ShoppingFlowFailureReason FailureReason { get; }
        public ShoppingCandidate Candidate { get; }
        public ShoppingReservation Reservation { get; }

        private ShoppingFlowResult(
            bool succeeded,
            ShoppingFlowFailureReason reason,
            ShoppingCandidate candidate,
            ShoppingReservation reservation)
        {
            Succeeded = succeeded;
            FailureReason = reason;
            Candidate = candidate;
            Reservation = reservation;
        }

        public static ShoppingFlowResult Success(
            ShoppingCandidate candidate,
            ShoppingReservation reservation) =>
            new ShoppingFlowResult(
                true,
                ShoppingFlowFailureReason.None,
                candidate,
                reservation);

        public static ShoppingFlowResult Failure(ShoppingFlowFailureReason reason) =>
            new ShoppingFlowResult(false, reason, null, null);
    }

    public sealed class ShoppingFlowService
    {
        private readonly ShoppingSearchService _search;
        private readonly ShoppingReservationService _reservationService;
        private readonly ShoppingCartService _cartService;
        private readonly ShoppingPolicy _policy;

        public ShoppingFlowService(
            ShoppingSearchService search,
            ShoppingReservationService reservationService,
            ShoppingCartService cartService,
            ShoppingPolicy policy)
        {
            _search = search ?? throw new ArgumentNullException(nameof(search));
            _reservationService = reservationService ??
                throw new ArgumentNullException(nameof(reservationService));
            _cartService = cartService ?? throw new ArgumentNullException(nameof(cartService));
            _policy = policy ?? throw new ArgumentNullException(nameof(policy));
        }

        public ShoppingFlowResult TryReserveBestCandidate(
            CustomerInstance customer,
            CustomerProfile profile,
            CustomerShoppingSession session,
            DisplayInstanceRegistry displays,
            ShoppingReservationId reservationId)
        {
            if (customer == null) throw new ArgumentNullException(nameof(customer));
            if (profile == null) throw new ArgumentNullException(nameof(profile));
            if (session == null) throw new ArgumentNullException(nameof(session));
            if (displays == null) throw new ArgumentNullException(nameof(displays));

            if (customer.State != CustomerState.Browsing)
                return ShoppingFlowResult.Failure(
                    ShoppingFlowFailureReason.CustomerNotBrowsing);

            if (session.CustomerId != customer.Id || profile.Id != customer.ProfileId)
                return ShoppingFlowResult.Failure(
                    ShoppingFlowFailureReason.OwnershipMismatch);

            IReadOnlyList<ShoppingCandidate> candidates =
                _search.FindCandidates(session.Intent, displays);
            if (candidates.Count == 0)
                return ShoppingFlowResult.Failure(
                    ShoppingFlowFailureReason.NoCandidate);

            ShoppingCandidate candidate = candidates[0];
            int remaining = session.Cart.CapacityUnits - session.Cart.TotalUnits;
            int amount = Math.Min(session.Intent.DesiredUnits, candidate.AvailableQuantity.Value);
            amount = Math.Min(amount, _policy.MaxUnitsPerReservation);
            amount = Math.Min(amount, remaining);
            if (amount <= 0)
                return ShoppingFlowResult.Failure(
                    ShoppingFlowFailureReason.CartRejected);

            ShoppingReservationResult reserve = _reservationService.TryReserve(
                reservationId,
                customer.Id,
                displays.Get(candidate.DisplayId),
                candidate.ProductId,
                new Quantity(amount));
            if (!reserve.Succeeded)
                return ShoppingFlowResult.Failure(
                    ShoppingFlowFailureReason.ReservationRejected);

            ShoppingCartMutationResult cart =
                _cartService.TryAddReservation(session.Cart, reservationId);
            if (!cart.Succeeded)
            {
                _reservationService.TryRelease(reservationId, displays);
                return ShoppingFlowResult.Failure(
                    ShoppingFlowFailureReason.CartRejected);
            }

            session.TryMarkHoldingReservations();
            return ShoppingFlowResult.Success(candidate, reserve.Reservation);
        }
    }
}
