using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.DayCycle;
using VRMGames.CartridgeAndCloud.Domain.Checkout;
using VRMGames.CartridgeAndCloud.Domain.Customers;
using VRMGames.CartridgeAndCloud.Domain.DayCycle;
using VRMGames.CartridgeAndCloud.Domain.Shopping;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.DayCycle
{
    public sealed class StoreClosureCoordinatorTests
    {
        [Test] public void RequestClose_TransitionsDayToClosing()
        {
            DayCycleTestScenario scenario =
                DayCycleTestFactory.Scenario();

            StoreDayServiceResult result =
                scenario.DayService.RequestClose(
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
        }

        [Test] public void RequestClose_SealsQueue()
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
                scenario.Queue.IsAcceptingEntries,
                Is.False);
        }

        [Test] public void RequestClose_DirectsCustomerToExit()
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
                scenario.Customers.Instances[0].State,
                Is.EqualTo(CustomerState.Leaving));
        }

        [Test] public void RequestClose_CancelsWaitingQueueEntry()
        {
            DayCycleTestScenario scenario =
                DayCycleTestFactory.Scenario();

            CheckoutQueueEntry entry =
                scenario.Queue.CurrentEntry;

            scenario.DayService.RequestClose(
                scenario.Customers,
                scenario.Queue,
                scenario.Station,
                scenario.Sessions,
                scenario.Reservations,
                scenario.Displays);

            Assert.That(scenario.Queue.IsEmpty, Is.True);
            Assert.That(
                entry.State,
                Is.EqualTo(
                    CheckoutQueueEntryState.Cancelled));
        }

        [Test] public void RequestClose_ReleasesReservation()
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
                scenario.Reservations.ActiveCount,
                Is.EqualTo(0));
            Assert.That(
                scenario.Reservations.Reservations[0]
                    .State,
                Is.EqualTo(
                    ShoppingReservationState.Released));
        }

        [Test] public void RequestClose_EmptiesCancelledCart()
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
                scenario.Sessions.Sessions[0]
                    .Cart.IsEmpty,
                Is.True);
        }

        [Test] public void RequestClose_AbandonsCancelledSession()
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
                scenario.Sessions.Sessions[0].State,
                Is.EqualTo(
                    CustomerShoppingState.Abandoned));
        }

        [Test] public void RequestClose_ClosesIdleStation()
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
                scenario.Station.State,
                Is.EqualTo(
                    CheckoutStationState.Closed));
        }

        [Test] public void Day_RemainsClosingWhileCustomerActive()
        {
            DayCycleTestScenario scenario =
                DayCycleTestFactory.Scenario();

            StoreDayServiceResult result =
                scenario.DayService.RequestClose(
                    scenario.Customers,
                    scenario.Queue,
                    scenario.Station,
                    scenario.Sessions,
                    scenario.Reservations,
                    scenario.Displays);

            Assert.That(
                result.ClosureProgress.Completed,
                Is.False);
            Assert.That(
                scenario.Day.State,
                Is.EqualTo(StoreDayState.Closing));
        }

        [Test] public void ContinueClosing_ClosesAfterCustomerDespawn()
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
        }

        [Test] public void ClosedDay_HasReadySnapshot()
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
                result.ClosureProgress.Snapshot
                    .IsReadyToClose,
                Is.True);
        }

        [Test] public void Closure_RecordsDirectedCustomer()
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
                scenario.Activity
                    .CustomersDirectedToExit,
                Is.EqualTo(1));
        }

        [Test] public void Closure_RecordsCancelledEntry()
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
                scenario.Activity
                    .CancelledQueueEntries,
                Is.EqualTo(1));
        }

        [Test] public void Closure_RecordsReleasedReservation()
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
                scenario.Activity.ReleasedReservations,
                Is.EqualTo(1));
        }

        [Test] public void Closure_BlocksNewQueueEntry()
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

            CheckoutQueueResult blocked =
                scenario.Queue.TryEnqueue(
                    new CheckoutQueueEntry(
                        new CheckoutQueueEntryId("new"),
                        new CustomerInstanceId("new-customer"),
                        new ShoppingCartId("new-cart")));

            Assert.That(
                blocked.FailureReason,
                Is.EqualTo(
                    CheckoutQueueFailureReason.QueueSealed));
        }

        [Test] public void EmptyStore_ClosesInSingleProgress()
        {
            DayCycleTestScenario scenario =
                DayCycleTestFactory.Scenario(
                    customerCount: 0,
                    addReadySession: false);

            StoreDayServiceResult result =
                scenario.DayService.RequestClose(
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
        }

        [Test] public void UnqueuedSession_IsResolved()
        {
            DayCycleTestScenario scenario =
                DayCycleTestFactory.Scenario(
                    enqueue: false);

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
                    CustomerShoppingState.Abandoned));
            Assert.That(
                scenario.Reservations.ActiveCount,
                Is.EqualTo(0));
        }

        [Test]
        public void Closing_PreservesProcessingEntry()
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

        [Test]
        public void Closing_PreservesBusyStation()
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

        [Test]
        public void Closing_PreservesProcessingReservation()
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

        [Test]
        public void Closing_PreservesProcessingSession()
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

        [Test]
        public void Closing_StillDirectsProcessingCustomerToExit()
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

        [Test]
        public void ProcessingCheckout_CanCompleteDuringClosing()
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

        [Test]
        public void DayClosesAfterProcessingCheckoutAndDrain()
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

        [Test]
        public void ProcessingCheckout_ConsumesReservation()
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

        [Test]
        public void ProcessingCheckout_ReducesPhysicalStock()
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

        [Test]
        public void SealedQueue_DoesNotBlockCurrentCheckout()
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

        [Test]
        public void Activity_CanRecordCheckoutDuringClosing()
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

        [Test]
        public void ProcessingWork_BlocksReadiness()
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
