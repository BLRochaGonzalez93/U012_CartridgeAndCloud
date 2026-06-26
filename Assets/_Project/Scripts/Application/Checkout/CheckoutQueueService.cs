using System;
using VRMGames.CartridgeAndCloud.Domain.Checkout;
using VRMGames.CartridgeAndCloud.Domain.Shopping;

namespace VRMGames.CartridgeAndCloud.Application.Checkout
{
    public enum CheckoutQueueServiceFailureReason
    {
        None = 0,
        SessionNotReady = 1,
        EmptyCart = 2,
        QueueRejected = 3,
        StationUnavailable = 4,
        StationBusy = 5,
        StationEntryMismatch = 6
    }

    public sealed class CheckoutQueueServiceResult
    {
        public bool Succeeded { get; }

        public CheckoutQueueServiceFailureReason FailureReason { get; }

        public CheckoutQueueFailureReason QueueFailureReason { get; }

        public CheckoutQueueEntry Entry { get; }

        public int Position { get; }

        private CheckoutQueueServiceResult(
            bool succeeded,
            CheckoutQueueServiceFailureReason failureReason,
            CheckoutQueueFailureReason queueFailureReason,
            CheckoutQueueEntry entry,
            int position)
        {
            Succeeded = succeeded;
            FailureReason = failureReason;
            QueueFailureReason = queueFailureReason;
            Entry = entry;
            Position = position;
        }

        public static CheckoutQueueServiceResult Success(
            CheckoutQueueEntry entry,
            int position)
        {
            return new CheckoutQueueServiceResult(
                true,
                CheckoutQueueServiceFailureReason.None,
                CheckoutQueueFailureReason.None,
                entry,
                position);
        }

        public static CheckoutQueueServiceResult Failure(
            CheckoutQueueServiceFailureReason reason,
            CheckoutQueueFailureReason queueReason =
                CheckoutQueueFailureReason.None,
            CheckoutQueueEntry entry = null,
            int position = 0)
        {
            return new CheckoutQueueServiceResult(
                false,
                reason,
                queueReason,
                entry,
                position);
        }
    }

    public sealed class CheckoutQueueService
    {
        private readonly CheckoutPolicy _policy;

        public CheckoutQueueService(CheckoutPolicy policy)
        {
            _policy = policy ??
                throw new ArgumentNullException(nameof(policy));
        }

        public CheckoutQueue CreateQueue()
        {
            return new CheckoutQueue(_policy.MaxQueueLength);
        }

        public CheckoutQueueServiceResult TryEnqueue(
            CheckoutQueue queue,
            CheckoutQueueEntryId entryId,
            CustomerShoppingSession session)
        {
            if (queue == null)
            {
                throw new ArgumentNullException(nameof(queue));
            }

            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            if (session.State !=
                CustomerShoppingState.ReadyForCheckout)
            {
                return CheckoutQueueServiceResult.Failure(
                    CheckoutQueueServiceFailureReason
                        .SessionNotReady);
            }

            if (session.Cart.IsEmpty)
            {
                return CheckoutQueueServiceResult.Failure(
                    CheckoutQueueServiceFailureReason.EmptyCart);
            }

            CheckoutQueueEntry entry =
                new CheckoutQueueEntry(
                    entryId,
                    session.CustomerId,
                    session.Cart.Id);

            CheckoutQueueResult result =
                queue.TryEnqueue(entry);

            if (!result.Succeeded)
            {
                return CheckoutQueueServiceResult.Failure(
                    CheckoutQueueServiceFailureReason
                        .QueueRejected,
                    result.FailureReason,
                    result.Entry,
                    result.Position);
            }

            return CheckoutQueueServiceResult.Success(
                result.Entry,
                result.Position);
        }

        public CheckoutQueueServiceResult TryCallNext(
            CheckoutQueue queue)
        {
            if (queue == null)
            {
                throw new ArgumentNullException(nameof(queue));
            }

            CheckoutQueueResult result =
                queue.TryCallNext();

            return result.Succeeded
                ? CheckoutQueueServiceResult.Success(
                    result.Entry,
                    result.Position)
                : CheckoutQueueServiceResult.Failure(
                    CheckoutQueueServiceFailureReason
                        .QueueRejected,
                    result.FailureReason,
                    result.Entry,
                    result.Position);
        }

        public CheckoutQueueServiceResult TryBeginProcessing(
            CheckoutQueue queue,
            CheckoutStation station,
            CheckoutQueueEntryId entryId)
        {
            if (queue == null)
            {
                throw new ArgumentNullException(nameof(queue));
            }

            if (station == null)
            {
                throw new ArgumentNullException(nameof(station));
            }

            if (!station.IsOpen)
            {
                return CheckoutQueueServiceResult.Failure(
                    CheckoutQueueServiceFailureReason
                        .StationUnavailable);
            }

            if (station.IsBusy)
            {
                return CheckoutQueueServiceResult.Failure(
                    CheckoutQueueServiceFailureReason
                        .StationBusy);
            }

            CheckoutQueueEntry current = queue.CurrentEntry;
            if (current == null ||
                current.Id != entryId ||
                current.State != CheckoutQueueEntryState.Called)
            {
                return CheckoutQueueServiceResult.Failure(
                    CheckoutQueueServiceFailureReason
                        .StationEntryMismatch);
            }

            if (!station.TryBeginProcessing(entryId))
            {
                return CheckoutQueueServiceResult.Failure(
                    CheckoutQueueServiceFailureReason
                        .StationUnavailable);
            }

            CheckoutQueueResult queueResult =
                queue.TryBeginProcessing(entryId);

            if (!queueResult.Succeeded)
            {
                station.TryCompleteProcessing(entryId);
                return CheckoutQueueServiceResult.Failure(
                    CheckoutQueueServiceFailureReason
                        .QueueRejected,
                    queueResult.FailureReason,
                    queueResult.Entry,
                    queueResult.Position);
            }

            return CheckoutQueueServiceResult.Success(
                queueResult.Entry,
                queueResult.Position);
        }

        public CheckoutQueueServiceResult TryCancel(
            CheckoutQueue queue,
            CheckoutQueueEntryId entryId)
        {
            if (queue == null)
            {
                throw new ArgumentNullException(nameof(queue));
            }

            CheckoutQueueResult result =
                queue.TryCancel(entryId);

            return result.Succeeded
                ? CheckoutQueueServiceResult.Success(
                    result.Entry,
                    result.Position)
                : CheckoutQueueServiceResult.Failure(
                    CheckoutQueueServiceFailureReason
                        .QueueRejected,
                    result.FailureReason,
                    result.Entry,
                    result.Position);
        }
    }
}
