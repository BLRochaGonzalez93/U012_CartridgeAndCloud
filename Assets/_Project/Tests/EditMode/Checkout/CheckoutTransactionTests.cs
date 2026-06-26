using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.Checkout;
using VRMGames.CartridgeAndCloud.Domain.Customers;
using VRMGames.CartridgeAndCloud.Domain.Shopping;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Checkout
{
    public sealed class CheckoutTransactionTests
    {
        private static CheckoutTransaction Create(
            string id = "transaction",
            string cart = "cart")
        {
            return new CheckoutTransaction(
                new CheckoutTransactionId(id),
                new CheckoutStationId("station"),
                new CustomerInstanceId("customer"),
                new ShoppingCartId(cart),
                2,
                3);
        }

        [Test] public void Transaction_StartsPending()
        {
            var transaction = Create();
            Assert.That(
                transaction.State,
                Is.EqualTo(
                    CheckoutTransactionState.Pending));
        }

        [Test] public void Transaction_StoresCounts()
        {
            var transaction = Create();
            Assert.That(
                transaction.LineCount,
                Is.EqualTo(2));
            Assert.That(
                transaction.UnitCount,
                Is.EqualTo(3));
        }

        [Test] public void Transaction_RejectsZeroLines() =>
            Assert.Throws<System.ArgumentOutOfRangeException>(
                () => new CheckoutTransaction(
                    new CheckoutTransactionId("t"),
                    new CheckoutStationId("s"),
                    new CustomerInstanceId("c"),
                    new ShoppingCartId("cart"),
                    0,
                    1));

        [Test] public void Transaction_RejectsZeroUnits() =>
            Assert.Throws<System.ArgumentOutOfRangeException>(
                () => new CheckoutTransaction(
                    new CheckoutTransactionId("t"),
                    new CheckoutStationId("s"),
                    new CustomerInstanceId("c"),
                    new ShoppingCartId("cart"),
                    1,
                    0));

        [Test] public void Transaction_CompletesOnce()
        {
            var transaction = Create();
            Assert.That(
                transaction.TryComplete(),
                Is.True);
            Assert.That(
                transaction.TryComplete(),
                Is.False);
        }

        [Test] public void Transaction_FailsWithCode()
        {
            var transaction = Create();
            Assert.That(
                transaction.TryFail("failure"),
                Is.True);
            Assert.That(
                transaction.State,
                Is.EqualTo(
                    CheckoutTransactionState.Failed));
            Assert.That(
                transaction.FailureCode,
                Is.EqualTo("failure"));
        }

        [Test] public void CompletedTransaction_CannotFail()
        {
            var transaction = Create();
            transaction.TryComplete();
            Assert.That(
                transaction.TryFail("failure"),
                Is.False);
        }

        [Test] public void Registry_RegistersTransaction()
        {
            var registry =
                new CheckoutTransactionRegistry();
            Assert.That(
                registry.TryRegister(Create()),
                Is.True);
            Assert.That(registry.Count, Is.EqualTo(1));
        }

        [Test] public void Registry_RejectsDuplicateId()
        {
            var registry =
                new CheckoutTransactionRegistry();
            registry.TryRegister(Create());
            Assert.That(
                registry.TryRegister(Create()),
                Is.False);
        }

        [Test] public void Registry_RecordsCompletedCart()
        {
            var registry =
                new CheckoutTransactionRegistry();
            var transaction = Create();
            registry.TryRegister(transaction);
            transaction.TryComplete();
            Assert.That(
                registry.TryRecordCompletion(
                    transaction),
                Is.True);
            Assert.That(
                registry.HasCompletedCart(
                    transaction.CartId),
                Is.True);
        }

        [Test] public void Registry_RejectsPendingCompletionRecord()
        {
            var registry =
                new CheckoutTransactionRegistry();
            var transaction = Create();
            registry.TryRegister(transaction);
            Assert.That(
                registry.TryRecordCompletion(
                    transaction),
                Is.False);
        }

        [Test] public void Registry_GetThrowsForMissing()
        {
            Assert.Throws<
                System.Collections.Generic
                    .KeyNotFoundException>(
                () => new CheckoutTransactionRegistry()
                    .Get(
                        new CheckoutTransactionId(
                            "missing")));
        }
    }
}
