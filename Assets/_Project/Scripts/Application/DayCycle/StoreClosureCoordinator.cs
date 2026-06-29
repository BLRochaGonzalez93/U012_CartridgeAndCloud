using System;
using System.Collections.Generic;
using VRMGames.CartridgeAndCloud.Application.Checkout;
using VRMGames.CartridgeAndCloud.Application.Shopping;
using VRMGames.CartridgeAndCloud.Domain.Checkout;
using VRMGames.CartridgeAndCloud.Domain.Customers;
using VRMGames.CartridgeAndCloud.Domain.DayCycle;
using VRMGames.CartridgeAndCloud.Domain.Displays;
using VRMGames.CartridgeAndCloud.Domain.Shopping;

namespace VRMGames.CartridgeAndCloud.Application.DayCycle
{
    public enum StoreClosureFailureReason
    {
        None = 0,
        DayNotClosing = 1,
        CustomerDrainFailed = 2,
        QueueCancellationFailed = 3,
        SessionResolutionFailed = 4,
        StationCloseFailed = 5,
        DayCompletionFailed = 6
    }

    public sealed class StoreClosureProgressResult
    {
        public bool Succeeded { get; }

        public bool Completed { get; }

        public StoreClosureFailureReason FailureReason { get; }

        public StoreClosureSnapshot Snapshot { get; }

        public int CustomersDirectedToExit { get; }

        public int CancelledQueueEntries { get; }

        public int AbandonedSessions { get; }

        public int ReleasedReservations { get; }

        private StoreClosureProgressResult(
            bool succeeded,
            bool completed,
            StoreClosureFailureReason failureReason,
            StoreClosureSnapshot snapshot,
            int customersDirectedToExit,
            int cancelledQueueEntries,
            int abandonedSessions,
            int releasedReservations)
        {
            Succeeded = succeeded;
            Completed = completed;
            FailureReason = failureReason;
            Snapshot = snapshot;
            CustomersDirectedToExit =
                customersDirectedToExit;
            CancelledQueueEntries =
                cancelledQueueEntries;
            AbandonedSessions = abandonedSessions;
            ReleasedReservations =
                releasedReservations;
        }

        public static StoreClosureProgressResult Success(
            bool completed,
            StoreClosureSnapshot snapshot,
            int customersDirectedToExit,
            int cancelledQueueEntries,
            int abandonedSessions,
            int releasedReservations)
        {
            return new StoreClosureProgressResult(
                true,
                completed,
                StoreClosureFailureReason.None,
                snapshot,
                customersDirectedToExit,
                cancelledQueueEntries,
                abandonedSessions,
                releasedReservations);
        }

        public static StoreClosureProgressResult Failure(
            StoreClosureFailureReason reason,
            StoreClosureSnapshot snapshot,
            int customersDirectedToExit,
            int cancelledQueueEntries,
            int abandonedSessions,
            int releasedReservations)
        {
            return new StoreClosureProgressResult(
                false,
                false,
                reason,
                snapshot,
                customersDirectedToExit,
                cancelledQueueEntries,
                abandonedSessions,
                releasedReservations);
        }
    }

    public sealed class StoreClosureCoordinator
    {
        private readonly CustomerDrainService _customerDrain;
        private readonly CheckoutCancellationService
            _checkoutCancellation;
        private readonly ShoppingCartService _cartService;
        private readonly StoreClosureSnapshotFactory
            _snapshotFactory;
        private readonly StoreDayActivityTracker _activity;

        public StoreClosureCoordinator(
            CustomerDrainService customerDrain,
            CheckoutCancellationService checkoutCancellation,
            ShoppingCartService cartService,
            StoreClosureSnapshotFactory snapshotFactory,
            StoreDayActivityTracker activity)
        {
            _customerDrain = customerDrain ??
                throw new ArgumentNullException(
                    nameof(customerDrain));
            _checkoutCancellation =
                checkoutCancellation ??
                throw new ArgumentNullException(
                    nameof(checkoutCancellation));
            _cartService = cartService ??
                throw new ArgumentNullException(
                    nameof(cartService));
            _snapshotFactory = snapshotFactory ??
                throw new ArgumentNullException(
                    nameof(snapshotFactory));
            _activity = activity ??
                throw new ArgumentNullException(
                    nameof(activity));
        }

        public StoreClosureProgressResult Progress(
            StoreDay day,
            CustomerInstanceRegistry customers,
            CheckoutQueue queue,
            CheckoutStation station,
            CustomerShoppingSessionRegistry sessions,
            ShoppingReservationRegistry reservations,
            DisplayInstanceRegistry displays)
        {
            if (day == null)
                throw new ArgumentNullException(nameof(day));
            if (customers == null)
                throw new ArgumentNullException(nameof(customers));
            if (queue == null)
                throw new ArgumentNullException(nameof(queue));
            if (station == null)
                throw new ArgumentNullException(nameof(station));
            if (sessions == null)
                throw new ArgumentNullException(nameof(sessions));
            if (reservations == null)
                throw new ArgumentNullException(nameof(reservations));
            if (displays == null)
                throw new ArgumentNullException(nameof(displays));

            if (day.State != StoreDayState.Closing)
            {
                return StoreClosureProgressResult.Failure(
                    StoreClosureFailureReason.DayNotClosing,
                    _snapshotFactory.Create(
                        customers,
                        queue,
                        station,
                        reservations,
                        sessions),
                    0,
                    0,
                    0,
                    0);
            }

            queue.TrySeal();

            CustomerDrainResult drain =
                _customerDrain.DirectAllToExit(customers);

            int directed = drain.DirectedToExit;
            int cancelledEntries = 0;
            int abandonedSessions = 0;
            int releasedReservations = 0;

            if (drain.FailedTransitions > 0)
            {
                return Failure(
                    StoreClosureFailureReason
                        .CustomerDrainFailed,
                    customers,
                    queue,
                    station,
                    reservations,
                    sessions,
                    directed,
                    cancelledEntries,
                    abandonedSessions,
                    releasedReservations);
            }

            List<CheckoutQueueEntry> queueSnapshot =
                new List<CheckoutQueueEntry>(
                    queue.ActiveEntries);

            foreach (CheckoutQueueEntry entry
                     in queueSnapshot)
            {
                if (entry.State ==
                    CheckoutQueueEntryState.Processing)
                {
                    continue;
                }

                if (!sessions.TryGet(
                        entry.CustomerId,
                        out CustomerShoppingSession session))
                {
                    return Failure(
                        StoreClosureFailureReason
                            .QueueCancellationFailed,
                        customers,
                        queue,
                        station,
                        reservations,
                        sessions,
                        directed,
                        cancelledEntries,
                        abandonedSessions,
                        releasedReservations);
                }

                CheckoutCancellationResult cancellation =
                    _checkoutCancellation.TryCancel(
                        queue,
                        entry.Id,
                        session,
                        displays);

                if (!cancellation.Succeeded)
                {
                    return Failure(
                        StoreClosureFailureReason
                            .QueueCancellationFailed,
                        customers,
                        queue,
                        station,
                        reservations,
                        sessions,
                        directed,
                        cancelledEntries,
                        abandonedSessions,
                        releasedReservations);
                }

                cancelledEntries++;
                abandonedSessions++;
                releasedReservations +=
                    cancellation.ReleasedReservations;
            }

            foreach (CustomerShoppingSession session
                     in sessions.Sessions)
            {
                if (session.State ==
                        CustomerShoppingState.Abandoned ||
                    session.State ==
                        CustomerShoppingState.CheckedOut)
                {
                    continue;
                }

                if (IsProcessingSession(queue, session))
                {
                    continue;
                }

                int expectedLines = session.Cart.LineCount;
                int released = session.Cart.IsEmpty
                    ? 0
                    : _cartService.Abandon(
                        session.Cart,
                        displays);

                if (released != expectedLines ||
                    !session.Cart.IsEmpty ||
                    !session.TryAbandon())
                {
                    return Failure(
                        StoreClosureFailureReason
                            .SessionResolutionFailed,
                        customers,
                        queue,
                        station,
                        reservations,
                        sessions,
                        directed,
                        cancelledEntries,
                        abandonedSessions,
                        releasedReservations);
                }

                abandonedSessions++;
                releasedReservations += released;
            }

            StoreClosureSnapshot snapshot =
                _snapshotFactory.Create(
                    customers,
                    queue,
                    station,
                    reservations,
                    sessions);

            if (queue.IsEmpty &&
                station.State ==
                    CheckoutStationState.Available)
            {
                if (!station.TryClose())
                {
                    return StoreClosureProgressResult.Failure(
                        StoreClosureFailureReason
                            .StationCloseFailed,
                        snapshot,
                        directed,
                        cancelledEntries,
                        abandonedSessions,
                        releasedReservations);
                }

                snapshot = _snapshotFactory.Create(
                    customers,
                    queue,
                    station,
                    reservations,
                    sessions);
            }

            RecordActivity(
                directed,
                cancelledEntries,
                abandonedSessions,
                releasedReservations);

            if (!snapshot.IsReadyToClose)
            {
                return StoreClosureProgressResult.Success(
                    false,
                    snapshot,
                    directed,
                    cancelledEntries,
                    abandonedSessions,
                    releasedReservations);
            }

            StoreDayTransitionResult completion =
                day.TryCompleteClosing(true);

            if (!completion.Succeeded)
            {
                return StoreClosureProgressResult.Failure(
                    StoreClosureFailureReason
                        .DayCompletionFailed,
                    snapshot,
                    directed,
                    cancelledEntries,
                    abandonedSessions,
                    releasedReservations);
            }

            StoreClosureSnapshot finalSnapshot =
                _snapshotFactory.Create(
                    customers,
                    queue,
                    station,
                    reservations,
                    sessions);

            return StoreClosureProgressResult.Success(
                true,
                finalSnapshot,
                directed,
                cancelledEntries,
                abandonedSessions,
                releasedReservations);
        }

        private StoreClosureProgressResult Failure(
            StoreClosureFailureReason reason,
            CustomerInstanceRegistry customers,
            CheckoutQueue queue,
            CheckoutStation station,
            ShoppingReservationRegistry reservations,
            CustomerShoppingSessionRegistry sessions,
            int directed,
            int cancelledEntries,
            int abandonedSessions,
            int releasedReservations)
        {
            RecordActivity(
                directed,
                cancelledEntries,
                abandonedSessions,
                releasedReservations);

            return StoreClosureProgressResult.Failure(
                reason,
                _snapshotFactory.Create(
                    customers,
                    queue,
                    station,
                    reservations,
                    sessions),
                directed,
                cancelledEntries,
                abandonedSessions,
                releasedReservations);
        }

        private void RecordActivity(
            int directed,
            int cancelledEntries,
            int abandonedSessions,
            int releasedReservations)
        {
            _activity.RecordCustomersDirectedToExit(
                directed);
            _activity.RecordCancelledQueueEntries(
                cancelledEntries);
            _activity.RecordAbandonedShoppingSessions(
                abandonedSessions);
            _activity.RecordReleasedReservations(
                releasedReservations);
        }

        private static bool IsProcessingSession(
            CheckoutQueue queue,
            CustomerShoppingSession session)
        {
            CheckoutQueueEntry current =
                queue.CurrentEntry;

            return current != null &&
                current.State ==
                    CheckoutQueueEntryState.Processing &&
                current.CustomerId == session.CustomerId &&
                current.CartId == session.Cart.Id;
        }
    }
}
