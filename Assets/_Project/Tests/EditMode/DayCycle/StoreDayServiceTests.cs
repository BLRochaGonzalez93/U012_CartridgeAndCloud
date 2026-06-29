using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.DayCycle;
using VRMGames.CartridgeAndCloud.Domain.DayCycle;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.DayCycle
{
    public sealed class StoreDayServiceTests
    {
        [Test] public void TryOpen_OpensBeforeOpenDay()
        {
            DayCycleTestScenario scenario =
                DayCycleTestFactory.Scenario(
                    addReadySession: false);
            StoreDay day =
                DayCycleTestFactory.Day(open: false);
            StoreDayService service =
                new StoreDayService(
                    day,
                    scenario.Coordinator);

            Assert.That(
                service.TryOpen().Succeeded,
                Is.True);
            Assert.That(
                day.State,
                Is.EqualTo(StoreDayState.Open));
        }

        [Test] public void TryOpen_FailsWhenAlreadyOpen()
        {
            DayCycleTestScenario scenario =
                DayCycleTestFactory.Scenario(
                    addReadySession: false);
            Assert.That(
                scenario.DayService.TryOpen()
                    .TransitionFailureReason,
                Is.EqualTo(
                    StoreDayTransitionFailureReason
                        .InvalidState));
        }

        [Test] public void Advance_BeforeDurationKeepsOpen()
        {
            DayCycleTestScenario scenario =
                DayCycleTestFactory.Scenario(
                    addReadySession: false,
                    duration: 10);

            StoreDayServiceResult result =
                scenario.DayService.Advance(
                    5,
                    scenario.Customers,
                    scenario.Queue,
                    scenario.Station,
                    scenario.Sessions,
                    scenario.Reservations,
                    scenario.Displays);

            Assert.That(result.Succeeded, Is.True);
            Assert.That(
                scenario.Day.State,
                Is.EqualTo(StoreDayState.Open));
            Assert.That(
                result.ClosureProgress,
                Is.Null);
        }

        [Test] public void Advance_AtDurationStartsClosing()
        {
            DayCycleTestScenario scenario =
                DayCycleTestFactory.Scenario(
                    duration: 10);

            StoreDayServiceResult result =
                scenario.DayService.Advance(
                    10,
                    scenario.Customers,
                    scenario.Queue,
                    scenario.Station,
                    scenario.Sessions,
                    scenario.Reservations,
                    scenario.Displays);

            Assert.That(result.Succeeded, Is.True);
            Assert.That(
                scenario.Day.State,
                Is.EqualTo(StoreDayState.Closing));
            Assert.That(
                result.ClosureProgress,
                Is.Not.Null);
        }

        [Test] public void Advance_AutoClosingSealsQueue()
        {
            DayCycleTestScenario scenario =
                DayCycleTestFactory.Scenario(
                    duration: 10);

            scenario.DayService.Advance(
                10,
                scenario.Customers,
                scenario.Queue,
                scenario.Station,
                scenario.Sessions,
                scenario.Reservations,
                scenario.Displays);

            Assert.That(
                scenario.Queue.IsAcceptingEntries,
                Is.False);
        }

        [Test] public void Advance_RejectsInvalidSeconds()
        {
            DayCycleTestScenario scenario =
                DayCycleTestFactory.Scenario(
                    addReadySession: false);

            Assert.That(
                scenario.DayService.Advance(
                    0,
                    scenario.Customers,
                    scenario.Queue,
                    scenario.Station,
                    scenario.Sessions,
                    scenario.Reservations,
                    scenario.Displays)
                    .TransitionFailureReason,
                Is.EqualTo(
                    StoreDayTransitionFailureReason
                        .InvalidElapsedSeconds));
        }

        [Test] public void ContinueClosing_RejectsOpenDay()
        {
            DayCycleTestScenario scenario =
                DayCycleTestFactory.Scenario();

            Assert.That(
                scenario.DayService.ContinueClosing(
                    scenario.Customers,
                    scenario.Queue,
                    scenario.Station,
                    scenario.Sessions,
                    scenario.Reservations,
                    scenario.Displays)
                    .TransitionFailureReason,
                Is.EqualTo(
                    StoreDayTransitionFailureReason
                        .InvalidState));
        }

        [Test] public void RequestClose_RejectsSecondRequest()
        {
            DayCycleTestScenario scenario =
                DayCycleTestFactory.Scenario();

            scenario.DayService.RequestClose(
                scenario.Customers,
                scenario.Queue,
                scenario.Station,
                scenario.Sessions,
                scenario.Reservations,
                scenario.Displays);

            Assert.That(
                scenario.DayService.RequestClose(
                    scenario.Customers,
                    scenario.Queue,
                    scenario.Station,
                    scenario.Sessions,
                    scenario.Reservations,
                    scenario.Displays)
                    .TransitionFailureReason,
                Is.EqualTo(
                    StoreDayTransitionFailureReason
                        .InvalidState));
        }

        [Test] public void ManualPolicy_DoesNotStartClosureOnAdvance()
        {
            DayCycleTestScenario scenario =
                DayCycleTestFactory.Scenario(
                    addReadySession: false,
                    duration: 5,
                    autoClose: false);

            scenario.DayService.Advance(
                5,
                scenario.Customers,
                scenario.Queue,
                scenario.Station,
                scenario.Sessions,
                scenario.Reservations,
                scenario.Displays);

            Assert.That(
                scenario.Day.State,
                Is.EqualTo(StoreDayState.Open));
        }

        [Test] public void SnapshotFactory_ReflectsScenarioCounts()
        {
            DayCycleTestScenario scenario =
                DayCycleTestFactory.Scenario(
                    customerCount: 2);

            var snapshot =
                new StoreClosureSnapshotFactory().Create(
                    scenario.Customers,
                    scenario.Queue,
                    scenario.Station,
                    scenario.Reservations,
                    scenario.Sessions);

            Assert.That(
                snapshot.ActiveCustomers,
                Is.EqualTo(2));
            Assert.That(
                snapshot.ActiveQueueEntries,
                Is.EqualTo(1));
            Assert.That(
                snapshot.ActiveReservations,
                Is.EqualTo(1));
            Assert.That(
                snapshot.PendingShoppingSessions,
                Is.EqualTo(1));
        }

        [Test] public void Coordinator_RejectsOpenDay()
        {
            DayCycleTestScenario scenario =
                DayCycleTestFactory.Scenario();

            StoreClosureProgressResult result =
                scenario.Coordinator.Progress(
                    scenario.Day,
                    scenario.Customers,
                    scenario.Queue,
                    scenario.Station,
                    scenario.Sessions,
                    scenario.Reservations,
                    scenario.Displays);

            Assert.That(result.Succeeded, Is.False);
            Assert.That(
                result.FailureReason,
                Is.EqualTo(
                    StoreClosureFailureReason.DayNotClosing));
        }
    }
}
