using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.Checkout;
using VRMGames.CartridgeAndCloud.Domain.Checkout;
using VRMGames.CartridgeAndCloud.Domain.Shopping;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Checkout
{
    public sealed class CheckoutServiceSuccessTests
    {
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

        [Test] public void Checkout_CompletesSingleLine()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario();
            CheckoutResult result = Run(scenario);
            Assert.That(result.Succeeded, Is.True);
            Assert.That(result.ProcessedLines, Is.EqualTo(1));
            Assert.That(result.ProcessedUnits, Is.EqualTo(1));
        }

        [Test] public void Checkout_RemovesPhysicalStock()
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

        [Test] public void Checkout_ConsumesReservations()
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

        [Test] public void Checkout_EmptiesCart()
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

        [Test] public void Checkout_MarksSessionCheckedOut()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario();
            Run(scenario);
            Assert.That(
                scenario.Session.State,
                Is.EqualTo(
                    CustomerShoppingState.CheckedOut));
        }

        [Test] public void Checkout_RemovesQueueFront()
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

        [Test] public void Checkout_ReleasesStation()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario();
            Run(scenario);
            Assert.That(
                scenario.Station.State,
                Is.EqualTo(
                    CheckoutStationState.Available));
        }

        [Test] public void Checkout_RecordsCompletedTransaction()
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

        [Test] public void Checkout_BlocksSecondCheckout()
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

        [Test] public void Checkout_AggregatesMultipleReservations()
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

        [Test] public void ConsumedReservation_CanRollbackInternally()
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

        [Test] public void CheckedOutSession_CannotBeAbandoned()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario();
            Run(scenario);
            Assert.That(
                scenario.Session.TryAbandon(),
                Is.False);
        }

        [Test] public void Checkout_PreservesConservation()
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
    }
}
