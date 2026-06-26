using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.Checkout;
using VRMGames.CartridgeAndCloud.Domain.Checkout;
using VRMGames.CartridgeAndCloud.Domain.Customers;
using VRMGames.CartridgeAndCloud.Domain.Displays;
using VRMGames.CartridgeAndCloud.Domain.Inventory;
using VRMGames.CartridgeAndCloud.Domain.Products;
using VRMGames.CartridgeAndCloud.Domain.Shopping;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Checkout
{
    public sealed class CheckoutServiceValidationTests
    {
        private static CheckoutResult Run(
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

        [Test] public void Checkout_RejectsDuplicateTransactionId()
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

        [Test] public void Checkout_RejectsCompletedCart()
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

        [Test] public void Checkout_RequiresProcessingQueueEntry()
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

        [Test] public void Checkout_RequiresBusyStation()
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

        [Test] public void Checkout_RequiresMatchingStationEntry()
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

        [Test] public void Checkout_RequiresReadySession()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario();
            scenario.Session.TryAbandon();

            Assert.That(
                Run(scenario).FailureReason,
                Is.EqualTo(
                    CheckoutFailureReason.SessionNotReady));
        }

        [Test] public void Checkout_RejectsEmptyCart()
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

        [Test] public void Checkout_RejectsOwnershipMismatch()
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

        [Test] public void Checkout_RejectsMissingReservation()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario();

            Assert.That(
                Run(
                    scenario,
                    reservations:
                        new ShoppingReservationRegistry())
                    .FailureReason,
                Is.EqualTo(
                    CheckoutFailureReason.ReservationMissing));
        }

        [Test] public void Checkout_RejectsInactiveReservation()
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

        [Test] public void Checkout_RejectsReservationMismatch()
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
                Run(
                    scenario,
                    reservations: replacement)
                    .FailureReason,
                Is.EqualTo(
                    CheckoutFailureReason
                        .ReservationMismatch));
        }

        [Test] public void Checkout_RejectsMissingDisplay()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario();

            Assert.That(
                Run(
                    scenario,
                    displays:
                        new DisplayInstanceRegistry(
                            new DisplayInstance[0]))
                    .FailureReason,
                Is.EqualTo(
                    CheckoutFailureReason.DisplayMissing));
        }

        [Test] public void Checkout_RejectsDisplayProductMismatch()
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
                Run(
                    scenario,
                    displays:
                        new DisplayInstanceRegistry(
                            new[] { replacement }))
                    .FailureReason,
                Is.EqualTo(
                    CheckoutFailureReason
                        .DisplayProductMismatch));
        }

        [Test] public void Checkout_RejectsInsufficientAggregateStock()
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

        [Test] public void ValidationFailure_DoesNotMutateState()
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
    }
}
