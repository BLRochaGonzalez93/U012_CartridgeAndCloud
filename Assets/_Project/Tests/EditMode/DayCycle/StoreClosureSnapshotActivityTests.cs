using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.Checkout;
using VRMGames.CartridgeAndCloud.Domain.DayCycle;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.DayCycle
{
    public sealed class StoreClosureSnapshotActivityTests
    {
        [Test] public void Snapshot_RejectsNegativeCustomers() =>
            Assert.Throws<System.ArgumentOutOfRangeException>(
                () => new StoreClosureSnapshot(
                    -1,
                    0,
                    CheckoutStationState.Closed,
                    0,
                    0));

        [Test] public void Snapshot_RejectsNegativeQueue() =>
            Assert.Throws<System.ArgumentOutOfRangeException>(
                () => new StoreClosureSnapshot(
                    0,
                    -1,
                    CheckoutStationState.Closed,
                    0,
                    0));

        [Test] public void Snapshot_RejectsNegativeReservations() =>
            Assert.Throws<System.ArgumentOutOfRangeException>(
                () => new StoreClosureSnapshot(
                    0,
                    0,
                    CheckoutStationState.Closed,
                    -1,
                    0));

        [Test] public void Snapshot_RejectsNegativeSessions() =>
            Assert.Throws<System.ArgumentOutOfRangeException>(
                () => new StoreClosureSnapshot(
                    0,
                    0,
                    CheckoutStationState.Closed,
                    0,
                    -1));

        [Test] public void Snapshot_IsReadyWhenAllWorkDrained()
        {
            var snapshot = new StoreClosureSnapshot(
                0,
                0,
                CheckoutStationState.Closed,
                0,
                0);
            Assert.That(
                snapshot.IsReadyToClose,
                Is.True);
        }

        [Test] public void Snapshot_AvailableStationCanBeReady()
        {
            var snapshot = new StoreClosureSnapshot(
                0,
                0,
                CheckoutStationState.Available,
                0,
                0);
            Assert.That(
                snapshot.IsReadyToClose,
                Is.True);
        }

        [Test] public void Snapshot_BusyStationBlocksClose()
        {
            var snapshot = new StoreClosureSnapshot(
                0,
                0,
                CheckoutStationState.Busy,
                0,
                0);
            Assert.That(
                snapshot.IsReadyToClose,
                Is.False);
        }

        [Test] public void Snapshot_ActiveCustomerBlocksClose()
        {
            var snapshot = new StoreClosureSnapshot(
                1,
                0,
                CheckoutStationState.Closed,
                0,
                0);
            Assert.That(
                snapshot.IsReadyToClose,
                Is.False);
        }

        [Test] public void Snapshot_QueueBlocksClose()
        {
            var snapshot = new StoreClosureSnapshot(
                0,
                1,
                CheckoutStationState.Closed,
                0,
                0);
            Assert.That(
                snapshot.IsReadyToClose,
                Is.False);
        }

        [Test] public void Snapshot_ReservationBlocksClose()
        {
            var snapshot = new StoreClosureSnapshot(
                0,
                0,
                CheckoutStationState.Closed,
                1,
                0);
            Assert.That(
                snapshot.IsReadyToClose,
                Is.False);
        }

        [Test] public void Snapshot_PendingSessionBlocksClose()
        {
            var snapshot = new StoreClosureSnapshot(
                0,
                0,
                CheckoutStationState.Closed,
                0,
                1);
            Assert.That(
                snapshot.IsReadyToClose,
                Is.False);
        }

        [Test] public void Tracker_RejectsNegativeIncrement()
        {
            Assert.Throws<System.ArgumentOutOfRangeException>(
                () => new StoreDayActivityTracker()
                    .RecordCustomerArrivals(-1));
        }

        [Test] public void Tracker_AccumulatesArrivals()
        {
            var tracker = new StoreDayActivityTracker();
            tracker.RecordCustomerArrivals(2);
            tracker.RecordCustomerArrivals(3);
            Assert.That(
                tracker.CustomerArrivals,
                Is.EqualTo(5));
        }

        [Test] public void Tracker_AccumulatesClosureActions()
        {
            var tracker = new StoreDayActivityTracker();
            tracker.RecordCustomersDirectedToExit(2);
            tracker.RecordCancelledQueueEntries(1);
            tracker.RecordAbandonedShoppingSessions(1);
            tracker.RecordReleasedReservations(3);
            Assert.That(
                tracker.CustomersDirectedToExit,
                Is.EqualTo(2));
            Assert.That(
                tracker.CancelledQueueEntries,
                Is.EqualTo(1));
            Assert.That(
                tracker.AbandonedShoppingSessions,
                Is.EqualTo(1));
            Assert.That(
                tracker.ReleasedReservations,
                Is.EqualTo(3));
        }

        [Test] public void Tracker_RecordsCompletedCheckouts()
        {
            var tracker = new StoreDayActivityTracker();
            tracker.RecordCompletedCheckouts(2);
            Assert.That(
                tracker.CompletedCheckouts,
                Is.EqualTo(2));
        }

        [Test] public void Summary_CapturesFinalState()
        {
            StoreDay day = DayCycleTestFactory.Day();
            day.TryBeginClosing();
            day.TryCompleteClosing(true);
            var tracker = new StoreDayActivityTracker();
            var summary = tracker.CreateSummary(
                day,
                new StoreClosureSnapshot(
                    0,
                    0,
                    CheckoutStationState.Closed,
                    0,
                    0));
            Assert.That(
                summary.FinalState,
                Is.EqualTo(StoreDayState.Closed));
            Assert.That(
                summary.DayId,
                Is.EqualTo(day.Id));
        }

        [Test] public void Summary_CapturesFinalCounters()
        {
            StoreDay day = DayCycleTestFactory.Day();
            day.TryBeginClosing();
            day.TryCompleteClosing(true);
            var tracker = new StoreDayActivityTracker();
            tracker.RecordCustomerArrivals(4);
            tracker.RecordReleasedReservations(2);
            var summary = tracker.CreateSummary(
                day,
                new StoreClosureSnapshot(
                    0,
                    0,
                    CheckoutStationState.Closed,
                    0,
                    0));
            Assert.That(
                summary.CustomerArrivals,
                Is.EqualTo(4));
            Assert.That(
                summary.ReleasedReservations,
                Is.EqualTo(2));
        }
    }
}
