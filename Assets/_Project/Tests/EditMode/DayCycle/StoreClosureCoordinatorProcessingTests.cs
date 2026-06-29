using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.DayCycle;
using VRMGames.CartridgeAndCloud.Domain.Checkout;
using VRMGames.CartridgeAndCloud.Domain.Customers;
using VRMGames.CartridgeAndCloud.Domain.DayCycle;
using VRMGames.CartridgeAndCloud.Domain.Shopping;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.DayCycle
{
    public sealed class StoreClosureCoordinatorProcessingTests
    {
        [Test] public void Closing_PreservesProcessingEntry()
        {
            DayCycleTestScenario scenario =
                DayCycleTestFactory.Scenario(
                    processing: true);

            scenario.DayService.RequestClose(
                scenario.Customers,
                scenario.Queue,
                scenario.Station,
                scenario.Sessions,
                scenario.Reservations,
                scenario.Displays);

            Assert.That(
                scenario.Queue.ActiveCount,
                Is.EqualTo(1));
            Assert.That(
                scenario.Queue.CurrentEntry.State,
                Is.EqualTo(
                    CheckoutQueueEntryState.Processing));
        }

        [Test] public void Closing_PreservesBusyStation()
        {
            DayCycleTestScenario scenario =
                DayCycleTestFactory.Scenario(
                    processing: true);

            scenario.DayService.RequestClose(
                scenario.Customers,
                scenario.Queue,
                scenario.Station,
                scenario.Sessions,
                scenario.Reservations,
                scenario.Displays);

            Assert.That(
                scenario.Station.State,
                Is.EqualTo(
                    CheckoutStationState.Busy));
        }

        [Test] public void Closing_PreservesProcessingReservation()
        {
            DayCycleTestScenario scenario =
                DayCycleTestFactory.Scenario(
                    processing: true);

            scenario.DayService.RequestClose(
                scenario.Customers,
                scenario.Queue,
                scenario.Station,
                scenario.Sessions,
                scenario.Reservations,
                scenario.Displays);

            Assert.That(
                scenario.Reservations.ActiveCount,
                Is.EqualTo(1));
        }

        [Test] public void Closing_PreservesProcessingSession()
        {
            DayCycleTestScenario scenario =
                DayCycleTestFactory.Scenario(
                    processing: true);

            scenario.DayService.RequestClose(
                scenario.Customers,
                scenario.Queue,
                scenario.Station,
                scenario.Sessions,
                scenario.Reservations,
                scenario.Displays);

            Assert.That(
                scenario.Sessions.Sessions[0].State,
                Is.EqualTo(
                    CustomerShoppingState.ReadyForCheckout));
        }

        [Test] public void Closing_StillDirectsProcessingCustomerToExit()
        {
            DayCycleTestScenario scenario =
                DayCycleTestFactory.Scenario(
                    processing: true);

            scenario.DayService.RequestClose(
                scenario.Customers,
                scenario.Queue,
                scenario.Station,
                scenario.Sessions,
                scenario.Reservations,
                scenario.Displays);

            Assert.That(
                scenario.Customers.Instances[0].State,
                Is.EqualTo(CustomerState.Leaving));
        }

        [Test] public void ProcessingCheckout_CanCompleteDuringClosing()
        {
            DayCycleTestScenario scenario =
                DayCycleTestFactory.Scenario(
                    processing: true);

            scenario.DayService.RequestClose(
                scenario.Customers,
                scenario.Queue,
                scenario.Station,
                scenario.Sessions,
                scenario.Reservations,
                scenario.Displays);

            var result =
                DayCycleTestFactory
                    .CompleteProcessingCheckout(scenario);

            Assert.That(result.Succeeded, Is.True);
            Assert.That(scenario.Queue.IsEmpty, Is.True);
            Assert.That(
                scenario.Sessions.Sessions[0].State,
                Is.EqualTo(
                    CustomerShoppingState.CheckedOut));
        }

        [Test] public void DayClosesAfterProcessingCheckoutAndDrain()
        {
            DayCycleTestScenario scenario =
                DayCycleTestFactory.Scenario(
                    processing: true);

            scenario.DayService.RequestClose(
                scenario.Customers,
                scenario.Queue,
                scenario.Station,
                scenario.Sessions,
                scenario.Reservations,
                scenario.Displays);
            DayCycleTestFactory.CompleteProcessingCheckout(
                scenario);
            scenario.Activity.RecordCompletedCheckouts(1);
            DayCycleTestFactory.DespawnAll(
                scenario.Customers);

            StoreDayServiceResult result =
                scenario.DayService.ContinueClosing(
                    scenario.Customers,
                    scenario.Queue,
                    scenario.Station,
                    scenario.Sessions,
                    scenario.Reservations,
                    scenario.Displays);

            Assert.That(
                result.ClosureProgress.Completed,
                Is.True);
            Assert.That(
                scenario.Day.State,
                Is.EqualTo(StoreDayState.Closed));
            Assert.That(
                scenario.Station.State,
                Is.EqualTo(
                    CheckoutStationState.Closed));
        }

        [Test] public void ProcessingCheckout_ConsumesReservation()
        {
            DayCycleTestScenario scenario =
                DayCycleTestFactory.Scenario(
                    processing: true);

            scenario.DayService.RequestClose(
                scenario.Customers,
                scenario.Queue,
                scenario.Station,
                scenario.Sessions,
                scenario.Reservations,
                scenario.Displays);
            DayCycleTestFactory.CompleteProcessingCheckout(
                scenario);

            Assert.That(
                scenario.Reservations.ActiveCount,
                Is.EqualTo(0));
            Assert.That(
                scenario.Reservations.Reservations[0]
                    .State,
                Is.EqualTo(
                    ShoppingReservationState.Consumed));
        }

        [Test] public void ProcessingCheckout_ReducesPhysicalStock()
        {
            DayCycleTestScenario scenario =
                DayCycleTestFactory.Scenario(
                    stock: 4,
                    processing: true);

            scenario.DayService.RequestClose(
                scenario.Customers,
                scenario.Queue,
                scenario.Station,
                scenario.Sessions,
                scenario.Reservations,
                scenario.Displays);
            DayCycleTestFactory.CompleteProcessingCheckout(
                scenario);

            Assert.That(
                scenario.Display.Inventory.GetQuantity(
                    scenario.Product.Id).Value,
                Is.EqualTo(3));
        }

        [Test] public void SealedQueue_DoesNotBlockCurrentCheckout()
        {
            DayCycleTestScenario scenario =
                DayCycleTestFactory.Scenario(
                    processing: true);

            scenario.DayService.RequestClose(
                scenario.Customers,
                scenario.Queue,
                scenario.Station,
                scenario.Sessions,
                scenario.Reservations,
                scenario.Displays);

            Assert.That(
                scenario.Queue.IsAcceptingEntries,
                Is.False);
            Assert.That(
                DayCycleTestFactory
                    .CompleteProcessingCheckout(scenario)
                    .Succeeded,
                Is.True);
        }

        [Test] public void Activity_CanRecordCheckoutDuringClosing()
        {
            DayCycleTestScenario scenario =
                DayCycleTestFactory.Scenario(
                    processing: true);

            scenario.DayService.RequestClose(
                scenario.Customers,
                scenario.Queue,
                scenario.Station,
                scenario.Sessions,
                scenario.Reservations,
                scenario.Displays);
            DayCycleTestFactory.CompleteProcessingCheckout(
                scenario);
            scenario.Activity.RecordCompletedCheckouts(1);

            Assert.That(
                scenario.Activity.CompletedCheckouts,
                Is.EqualTo(1));
        }

        [Test] public void ProcessingWork_BlocksReadiness()
        {
            DayCycleTestScenario scenario =
                DayCycleTestFactory.Scenario(
                    processing: true);

            StoreDayServiceResult result =
                scenario.DayService.RequestClose(
                    scenario.Customers,
                    scenario.Queue,
                    scenario.Station,
                    scenario.Sessions,
                    scenario.Reservations,
                    scenario.Displays);

            Assert.That(
                result.ClosureProgress.Snapshot
                    .IsReadyToClose,
                Is.False);
        }
    }
}
