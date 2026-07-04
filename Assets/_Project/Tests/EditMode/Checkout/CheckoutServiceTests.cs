using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.Checkout;
using VRMGames.CartridgeAndCloud.Domain.Checkout;
using VRMGames.CartridgeAndCloud.Domain.Customers;
using VRMGames.CartridgeAndCloud.Domain.Displays;
using VRMGames.CartridgeAndCloud.Domain.Products;
using VRMGames.CartridgeAndCloud.Domain.Shopping;

using VRMGames.CartridgeAndCloud.Domain.Inventory;
namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Checkout
{
    public sealed class CheckoutServiceTests
    {

        #region Cancellation
        [Test] public void CancelWaitingEntry_ReleasesReservations()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario(
                    reservationCount: 2,
                    processEntry: false);
            var service =
                new CheckoutCancellationService(
                    CheckoutTestFactory.CartService(
                        scenario),
                    scenario.Reservations);

            CheckoutCancellationResult result =
                service.TryCancel(
                    scenario.Queue,
                    scenario.Entry.Id,
                    scenario.Session,
                    scenario.Displays);

            Assert.That(result.Succeeded, Is.True);
            Assert.That(
                result.ReleasedReservations,
                Is.EqualTo(2));
            Assert.That(scenario.Cart.IsEmpty, Is.True);
        }

        [Test] public void CancelWaitingEntry_AbandonsSession()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario(
                    processEntry: false);
            var service =
                new CheckoutCancellationService(
                    CheckoutTestFactory.CartService(
                        scenario),
                    scenario.Reservations);

            service.TryCancel(
                scenario.Queue,
                scenario.Entry.Id,
                scenario.Session,
                scenario.Displays);

            Assert.That(
                scenario.Session.State,
                Is.EqualTo(
                    CustomerShoppingState.Abandoned));
        }

        [Test] public void CancelWaitingEntry_RemovesQueueEntry()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario(
                    processEntry: false);
            var service =
                new CheckoutCancellationService(
                    CheckoutTestFactory.CartService(
                        scenario),
                    scenario.Reservations);

            service.TryCancel(
                scenario.Queue,
                scenario.Entry.Id,
                scenario.Session,
                scenario.Displays);

            Assert.That(scenario.Queue.IsEmpty, Is.True);
            Assert.That(
                scenario.Entry.State,
                Is.EqualTo(
                    CheckoutQueueEntryState.Cancelled));
        }

        [Test] public void CancelCalledEntry_IsAllowed()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario(
                    processEntry: false);
            scenario.QueueService.TryCallNext(
                scenario.Queue);
            var service =
                new CheckoutCancellationService(
                    CheckoutTestFactory.CartService(
                        scenario),
                    scenario.Reservations);

            Assert.That(
                service.TryCancel(
                    scenario.Queue,
                    scenario.Entry.Id,
                    scenario.Session,
                    scenario.Displays).Succeeded,
                Is.True);
        }

        [Test] public void CancelProcessingEntry_IsRejected()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario();
            var service =
                new CheckoutCancellationService(
                    CheckoutTestFactory.CartService(
                        scenario),
                    scenario.Reservations);

            CheckoutCancellationResult result =
                service.TryCancel(
                    scenario.Queue,
                    scenario.Entry.Id,
                    scenario.Session,
                    scenario.Displays);

            Assert.That(result.Succeeded, Is.False);
            Assert.That(
                result.FailureReason,
                Is.EqualTo(
                    CheckoutCancellationFailureReason
                        .EntryProcessing));
            Assert.That(
                scenario.Cart.TotalUnits,
                Is.EqualTo(1));
        }

        [Test] public void CancelMissingEntry_IsRejected()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario(
                    processEntry: false);
            var service =
                new CheckoutCancellationService(
                    CheckoutTestFactory.CartService(
                        scenario),
                    scenario.Reservations);

            Assert.That(
                service.TryCancel(
                    scenario.Queue,
                    new CheckoutQueueEntryId("missing"),
                    scenario.Session,
                    scenario.Displays).FailureReason,
                Is.EqualTo(
                    CheckoutCancellationFailureReason
                        .EntryNotFound));
        }

        [Test] public void Cancellation_RestoresAvailability()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario(
                    stock: 2,
                    processEntry: false);
            var service =
                new CheckoutCancellationService(
                    CheckoutTestFactory.CartService(
                        scenario),
                    scenario.Reservations);

            service.TryCancel(
                scenario.Queue,
                scenario.Entry.Id,
                scenario.Session,
                scenario.Displays);

            ShoppingReservation reservation =
                scenario.Reservations.GetForCustomer(
                    scenario.Session.CustomerId,
                    activeOnly: false)[0];
            Assert.That(
                reservation.State,
                Is.EqualTo(
                    ShoppingReservationState.Released));
            Assert.That(
                scenario.Display.Inventory.GetQuantity(
                    scenario.Product.Id).Value,
                Is.EqualTo(2));
        }

        #endregion

        #region Success
        private static CheckoutResult Run(
            CheckoutTestScenario scenario,
            string transactionId = "transaction")
        {
            return new CheckoutService().TryCheckout(
                new CheckoutTransactionId(transactionId),
                scenario.Queue,
                scenario.Station,
                scenario.Session,
                scenario.Displays,
                scenario.Reservations,
                scenario.Transactions);
        }

        [Test]
        public void Checkout_CompletesSingleLine()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario();
            CheckoutResult result = Run(scenario);
            Assert.That(result.Succeeded, Is.True);
            Assert.That(result.ProcessedLines, Is.EqualTo(1));
            Assert.That(result.ProcessedUnits, Is.EqualTo(1));
        }

        [Test]
        public void Checkout_RemovesPhysicalStock()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario(
                    stock: 3,
                    reservationCount: 2);
            Run(scenario);
            Assert.That(
                scenario.Display.Inventory.GetQuantity(
                    scenario.Product.Id).Value,
                Is.EqualTo(1));
        }

        [Test]
        public void Checkout_ConsumesReservations()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario(
                    reservationCount: 2);
            Run(scenario);

            foreach (ShoppingReservation reservation
                     in scenario.Reservations.GetForCustomer(
                         scenario.Session.CustomerId,
                         activeOnly: false))
            {
                Assert.That(
                    reservation.State,
                    Is.EqualTo(
                        ShoppingReservationState.Consumed));
            }
        }

        [Test]
        public void Checkout_EmptiesCart()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario(
                    reservationCount: 2);
            Run(scenario);
            Assert.That(scenario.Cart.IsEmpty, Is.True);
            Assert.That(
                scenario.Cart.TotalUnits,
                Is.EqualTo(0));
        }

        [Test]
        public void Checkout_MarksSessionCheckedOut()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario();
            Run(scenario);
            Assert.That(
                scenario.Session.State,
                Is.EqualTo(
                    CustomerShoppingState.CheckedOut));
        }

        [Test]
        public void Checkout_RemovesQueueFront()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario();
            Run(scenario);
            Assert.That(scenario.Queue.IsEmpty, Is.True);
            Assert.That(
                scenario.Entry.State,
                Is.EqualTo(
                    CheckoutQueueEntryState.Completed));
        }

        [Test]
        public void Checkout_ReleasesStation()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario();
            Run(scenario);
            Assert.That(
                scenario.Station.State,
                Is.EqualTo(
                    CheckoutStationState.Available));
        }

        [Test]
        public void Checkout_RecordsCompletedTransaction()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario();
            CheckoutResult result = Run(scenario);
            Assert.That(
                result.Transaction.State,
                Is.EqualTo(
                    CheckoutTransactionState.Completed));
            Assert.That(
                scenario.Transactions.HasCompletedCart(
                    scenario.Cart.Id),
                Is.True);
        }

        [Test]
        public void Checkout_BlocksSecondCheckout()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario();
            Run(scenario, "transaction-1");
            CheckoutResult second =
                Run(scenario, "transaction-2");
            Assert.That(second.Succeeded, Is.False);
            Assert.That(
                second.FailureReason,
                Is.EqualTo(
                    CheckoutFailureReason
                        .CartAlreadyCheckedOut));
        }

        [Test]
        public void Checkout_AggregatesMultipleReservations()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario(
                    stock: 5,
                    reservationCount: 3);
            CheckoutResult result = Run(scenario);
            Assert.That(result.Succeeded, Is.True);
            Assert.That(result.ProcessedLines, Is.EqualTo(3));
            Assert.That(result.ProcessedUnits, Is.EqualTo(3));
            Assert.That(
                scenario.Display.Inventory.GetQuantity(
                    scenario.Product.Id).Value,
                Is.EqualTo(2));
        }

        [Test]
        public void ConsumedReservation_CanRollbackInternally()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario();
            ShoppingReservation reservation =
                scenario.Reservations.GetForCustomer(
                    scenario.Session.CustomerId,
                    activeOnly: true)[0];
            reservation.TryConsume();
            Assert.That(
                reservation.TryRollbackConsumption(),
                Is.True);
            Assert.That(reservation.IsActive, Is.True);
        }

        [Test]
        public void CheckedOutSession_CannotBeAbandoned()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario();
            Run(scenario);
            Assert.That(
                scenario.Session.TryAbandon(),
                Is.False);
        }

        [Test]
        public void Checkout_PreservesConservation()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario(
                    stock: 4,
                    reservationCount: 2);
            int before =
                scenario.Display.Inventory.GetQuantity(
                    scenario.Product.Id).Value;
            CheckoutResult result = Run(scenario);
            int after =
                scenario.Display.Inventory.GetQuantity(
                    scenario.Product.Id).Value;

            Assert.That(result.Succeeded, Is.True);
            Assert.That(
                before,
                Is.EqualTo(
                    after + result.ProcessedUnits));
        }

        #endregion

        #region Validation
        private static CheckoutResult RunValidation(
            CheckoutTestScenario scenario,
            string transactionId = "transaction",
            DisplayInstanceRegistry displays = null,
            ShoppingReservationRegistry reservations = null)
        {
            return new CheckoutService().TryCheckout(
                new CheckoutTransactionId(transactionId),
                scenario.Queue,
                scenario.Station,
                scenario.Session,
                displays ?? scenario.Displays,
                reservations ?? scenario.Reservations,
                scenario.Transactions);
        }

        [Test]
        public void Checkout_RejectsDuplicateTransactionId()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario();
            var transaction =
                new CheckoutTransaction(
                    new CheckoutTransactionId("transaction"),
                    scenario.Station.Id,
                    scenario.Session.CustomerId,
                    scenario.Cart.Id,
                    1,
                    1);
            scenario.Transactions.TryRegister(transaction);

            Assert.That(
                Run(scenario).FailureReason,
                Is.EqualTo(
                    CheckoutFailureReason
                        .DuplicateTransactionId));
        }

        [Test]
        public void Checkout_RejectsCompletedCart()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario();
            var previous =
                new CheckoutTransaction(
                    new CheckoutTransactionId("previous"),
                    scenario.Station.Id,
                    scenario.Session.CustomerId,
                    scenario.Cart.Id,
                    1,
                    1);
            scenario.Transactions.TryRegister(previous);
            previous.TryComplete();
            scenario.Transactions.TryRecordCompletion(previous);

            Assert.That(
                Run(scenario).FailureReason,
                Is.EqualTo(
                    CheckoutFailureReason
                        .CartAlreadyCheckedOut));
        }

        [Test]
        public void Checkout_RequiresProcessingQueueEntry()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario(
                    processEntry: false);

            Assert.That(
                Run(scenario).FailureReason,
                Is.EqualTo(
                    CheckoutFailureReason
                        .QueueEntryNotProcessing));
        }

        [Test]
        public void Checkout_RequiresBusyStation()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario();
            scenario.Station.TryCompleteProcessing(
                scenario.Entry.Id);

            Assert.That(
                Run(scenario).FailureReason,
                Is.EqualTo(
                    CheckoutFailureReason
                        .StationNotProcessing));
        }

        [Test]
        public void Checkout_RequiresMatchingStationEntry()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario(
                    processEntry: false);
            scenario.Queue.TryCallNext();
            scenario.Queue.TryBeginProcessing(
                scenario.Entry.Id);
            scenario.Station.TryBeginProcessing(
                new CheckoutQueueEntryId("other"));

            Assert.That(
                Run(scenario).FailureReason,
                Is.EqualTo(
                    CheckoutFailureReason
                        .StationEntryMismatch));
        }

        [Test]
        public void Checkout_RequiresReadySession()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario();
            scenario.Session.TryAbandon();

            Assert.That(
                Run(scenario).FailureReason,
                Is.EqualTo(
                    CheckoutFailureReason.SessionNotReady));
        }

        [Test]
        public void Checkout_RejectsEmptyCart()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario();
            foreach (ShoppingCartLine line
                     in scenario.Cart.Lines)
            {
                scenario.Cart.TryRemove(
                    line.ReservationId);
            }

            Assert.That(
                Run(scenario).FailureReason,
                Is.EqualTo(
                    CheckoutFailureReason.EmptyCart));
        }

        [Test]
        public void Checkout_RejectsOwnershipMismatch()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario(
                    processEntry: false);
            CheckoutQueue queue = new CheckoutQueue(2);
            CheckoutQueueEntry other =
                new CheckoutQueueEntry(
                    new CheckoutQueueEntryId("other-entry"),
                    new CustomerInstanceId("other-customer"),
                    new ShoppingCartId("other-cart"));
            queue.TryEnqueue(other);
            queue.TryCallNext();
            queue.TryBeginProcessing(other.Id);
            CheckoutStation station =
                new CheckoutStation(
                    new CheckoutStationId("station"));
            station.TryOpen();
            station.TryBeginProcessing(other.Id);

            CheckoutResult result =
                new CheckoutService().TryCheckout(
                    new CheckoutTransactionId("transaction"),
                    queue,
                    station,
                    scenario.Session,
                    scenario.Displays,
                    scenario.Reservations,
                    scenario.Transactions);

            Assert.That(
                result.FailureReason,
                Is.EqualTo(
                    CheckoutFailureReason.OwnershipMismatch));
        }

        [Test]
        public void Checkout_RejectsMissingReservation()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario();

            Assert.That(
                RunValidation(
                    scenario,
                    reservations:
                        new ShoppingReservationRegistry())
                    .FailureReason,
                Is.EqualTo(
                    CheckoutFailureReason.ReservationMissing));
        }

        [Test]
        public void Checkout_RejectsInactiveReservation()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario();
            ShoppingReservation reservation =
                scenario.Reservations.GetForCustomer(
                    scenario.Session.CustomerId,
                    activeOnly: true)[0];
            reservation.TryRelease();

            Assert.That(
                Run(scenario).FailureReason,
                Is.EqualTo(
                    CheckoutFailureReason
                        .ReservationNotActive));
        }

        [Test]
        public void Checkout_RejectsReservationMismatch()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario();
            ShoppingCartLine line =
                scenario.Cart.Lines[0];
            ShoppingReservationRegistry replacement =
                new ShoppingReservationRegistry();
            replacement.TryRegister(
                new ShoppingReservation(
                    line.ReservationId,
                    new CustomerInstanceId("other"),
                    line.DisplayId,
                    line.ProductId,
                    line.Quantity));

            Assert.That(
                RunValidation(
                    scenario,
                    reservations: replacement)
                    .FailureReason,
                Is.EqualTo(
                    CheckoutFailureReason
                        .ReservationMismatch));
        }

        [Test]
        public void Checkout_RejectsMissingDisplay()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario();

            Assert.That(
                RunValidation(
                    scenario,
                    displays:
                        new DisplayInstanceRegistry(
                            new DisplayInstance[0]))
                    .FailureReason,
                Is.EqualTo(
                    CheckoutFailureReason.DisplayMissing));
        }

        [Test]
        public void Checkout_RejectsDisplayProductMismatch()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario();
            ProductDefinition otherProduct =
                CheckoutTestFactory.Product("product-b");
            ProductDefinitionRegistry products =
                CheckoutTestFactory.Products(otherProduct);
            DisplayInstance replacement =
                CheckoutTestFactory.Display(
                    products,
                    otherProduct,
                    id: scenario.Display.Id.Value,
                    stock: 3);

            Assert.That(
                RunValidation(
                    scenario,
                    displays:
                        new DisplayInstanceRegistry(
                            new[] { replacement }))
                    .FailureReason,
                Is.EqualTo(
                    CheckoutFailureReason
                        .DisplayProductMismatch));
        }

        [Test]
        public void Checkout_RejectsInsufficientAggregateStock()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario(
                    stock: 1,
                    reservationCount: 2);

            Assert.That(
                Run(scenario).FailureReason,
                Is.EqualTo(
                    CheckoutFailureReason
                        .InsufficientStock));
        }

        [Test]
        public void ValidationFailure_DoesNotMutateState()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario(
                    stock: 1,
                    reservationCount: 2);

            int stockBefore =
                scenario.Display.Inventory.GetQuantity(
                    scenario.Product.Id).Value;
            int cartBefore =
                scenario.Cart.TotalUnits;

            CheckoutResult result = Run(scenario);

            Assert.That(result.Succeeded, Is.False);
            Assert.That(
                scenario.Display.Inventory.GetQuantity(
                    scenario.Product.Id).Value,
                Is.EqualTo(stockBefore));
            Assert.That(
                scenario.Cart.TotalUnits,
                Is.EqualTo(cartBefore));
            Assert.That(
                scenario.Queue.CurrentEntry.State,
                Is.EqualTo(
                    CheckoutQueueEntryState.Processing));
            Assert.That(scenario.Station.IsBusy, Is.True);
            Assert.That(
                scenario.Transactions.Count,
                Is.EqualTo(0));
        }
        #endregion
    }
}
