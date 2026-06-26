using System;
using VRMGames.CartridgeAndCloud.Application.Shopping;
using VRMGames.CartridgeAndCloud.Domain.Checkout;
using VRMGames.CartridgeAndCloud.Domain.Displays;
using VRMGames.CartridgeAndCloud.Domain.Shopping;

namespace VRMGames.CartridgeAndCloud.Application.Checkout
{
    public enum CheckoutCancellationFailureReason
    {
        None = 0,
        EntryNotFound = 1,
        EntryProcessing = 2,
        OwnershipMismatch = 3,
        ReservationInvalid = 4,
        DisplayMissing = 5,
        QueueCancellationFailed = 6,
        ReleaseFailed = 7,
        SessionAbandonFailed = 8
    }

    public sealed class CheckoutCancellationResult
    {
        public bool Succeeded { get; }
        public CheckoutCancellationFailureReason FailureReason { get; }
        public int ReleasedReservations { get; }

        private CheckoutCancellationResult(
            bool succeeded,
            CheckoutCancellationFailureReason failureReason,
            int releasedReservations)
        {
            Succeeded = succeeded;
            FailureReason = failureReason;
            ReleasedReservations = releasedReservations;
        }

        public static CheckoutCancellationResult Success(
            int releasedReservations)
        {
            return new CheckoutCancellationResult(
                true,
                CheckoutCancellationFailureReason.None,
                releasedReservations);
        }

        public static CheckoutCancellationResult Failure(
            CheckoutCancellationFailureReason reason)
        {
            return new CheckoutCancellationResult(
                false,
                reason,
                0);
        }
    }

    public sealed class CheckoutCancellationService
    {
        private readonly ShoppingCartService _cartService;
        private readonly ShoppingReservationRegistry _reservations;

        public CheckoutCancellationService(
            ShoppingCartService cartService,
            ShoppingReservationRegistry reservations)
        {
            _cartService = cartService ??
                throw new ArgumentNullException(nameof(cartService));
            _reservations = reservations ??
                throw new ArgumentNullException(nameof(reservations));
        }

        public CheckoutCancellationResult TryCancel(
            CheckoutQueue queue,
            CheckoutQueueEntryId entryId,
            CustomerShoppingSession session,
            DisplayInstanceRegistry displays)
        {
            if (queue == null)
                throw new ArgumentNullException(nameof(queue));
            if (session == null)
                throw new ArgumentNullException(nameof(session));
            if (displays == null)
                throw new ArgumentNullException(nameof(displays));

            CheckoutQueueEntry entry = null;
            foreach (CheckoutQueueEntry candidate in queue.ActiveEntries)
            {
                if (candidate.Id == entryId)
                {
                    entry = candidate;
                    break;
                }
            }

            if (entry == null)
            {
                return CheckoutCancellationResult.Failure(
                    CheckoutCancellationFailureReason.EntryNotFound);
            }

            if (entry.State == CheckoutQueueEntryState.Processing)
            {
                return CheckoutCancellationResult.Failure(
                    CheckoutCancellationFailureReason.EntryProcessing);
            }

            if (entry.CustomerId != session.CustomerId ||
                entry.CartId != session.Cart.Id)
            {
                return CheckoutCancellationResult.Failure(
                    CheckoutCancellationFailureReason.OwnershipMismatch);
            }

            int lineCount = session.Cart.LineCount;
            foreach (ShoppingCartLine line in session.Cart.Lines)
            {
                if (!_reservations.TryGet(
                        line.ReservationId,
                        out ShoppingReservation reservation) ||
                    !reservation.IsActive ||
                    reservation.CustomerId != session.CustomerId ||
                    reservation.DisplayId != line.DisplayId ||
                    reservation.ProductId != line.ProductId ||
                    reservation.Quantity != line.Quantity)
                {
                    return CheckoutCancellationResult.Failure(
                        CheckoutCancellationFailureReason.ReservationInvalid);
                }

                if (!displays.Contains(line.DisplayId))
                {
                    return CheckoutCancellationResult.Failure(
                        CheckoutCancellationFailureReason.DisplayMissing);
                }
            }

            CheckoutQueueResult queueResult = queue.TryCancel(entryId);
            if (!queueResult.Succeeded)
            {
                return CheckoutCancellationResult.Failure(
                    CheckoutCancellationFailureReason.QueueCancellationFailed);
            }

            int released = _cartService.Abandon(
                session.Cart,
                displays);

            if (released != lineCount || !session.Cart.IsEmpty)
            {
                return CheckoutCancellationResult.Failure(
                    CheckoutCancellationFailureReason.ReleaseFailed);
            }

            if (!session.TryAbandon())
            {
                return CheckoutCancellationResult.Failure(
                    CheckoutCancellationFailureReason.SessionAbandonFailed);
            }

            return CheckoutCancellationResult.Success(released);
        }
    }
}
