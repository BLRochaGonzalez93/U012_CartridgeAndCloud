using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.Customers;
using VRMGames.CartridgeAndCloud.Domain.Shopping;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.DayCycle
{
    public sealed class CustomerShoppingSessionRegistryTests
    {
        private static CustomerShoppingSession Session(
            string customer,
            string suffix)
        {
            var customerId =
                new CustomerInstanceId(customer);
            var intent =
                new ShoppingIntent(
                    new ShoppingIntentId(
                        "intent-" + suffix),
                    customerId,
                    new[] { DayCycleTestFactory.Category },
                    1);
            var cart =
                new ShoppingCart(
                    new ShoppingCartId(
                        "cart-" + suffix),
                    customerId,
                    3);
            return new CustomerShoppingSession(
                customerId,
                intent,
                cart);
        }

        [Test] public void Registry_StartsEmpty()
        {
            var registry =
                new CustomerShoppingSessionRegistry();
            Assert.That(registry.Count, Is.EqualTo(0));
            Assert.That(
                registry.PendingCount,
                Is.EqualTo(0));
        }

        [Test] public void Registry_RegistersSession()
        {
            var registry =
                new CustomerShoppingSessionRegistry();
            Assert.That(
                registry.TryRegister(
                    Session("customer-a", "a")),
                Is.True);
            Assert.That(registry.Count, Is.EqualTo(1));
        }

        [Test] public void Registry_RejectsDuplicateCustomer()
        {
            var registry =
                new CustomerShoppingSessionRegistry();
            registry.TryRegister(
                Session("customer-a", "a"));
            Assert.That(
                registry.TryRegister(
                    Session("customer-a", "b")),
                Is.False);
        }

        [Test] public void Registry_ContainsCustomer()
        {
            var registry =
                new CustomerShoppingSessionRegistry();
            registry.TryRegister(
                Session("customer-a", "a"));
            Assert.That(
                registry.Contains(
                    new CustomerInstanceId("customer-a")),
                Is.True);
        }

        [Test] public void Registry_TryGetReturnsSession()
        {
            var registry =
                new CustomerShoppingSessionRegistry();
            CustomerShoppingSession session =
                Session("customer-a", "a");
            registry.TryRegister(session);
            Assert.That(
                registry.TryGet(
                    session.CustomerId,
                    out CustomerShoppingSession found),
                Is.True);
            Assert.That(found, Is.SameAs(session));
        }

        [Test] public void Registry_GetThrowsForMissing()
        {
            Assert.Throws<
                System.Collections.Generic.KeyNotFoundException>(
                () => new CustomerShoppingSessionRegistry()
                    .Get(
                        new CustomerInstanceId("missing")));
        }

        [Test] public void Registry_OrdersByCustomerId()
        {
            var registry =
                new CustomerShoppingSessionRegistry();
            registry.TryRegister(
                Session("z-customer", "z"));
            registry.TryRegister(
                Session("a-customer", "a"));
            Assert.That(
                registry.Sessions[0].CustomerId.Value,
                Is.EqualTo("a-customer"));
        }

        [Test] public void SearchingSession_IsPending()
        {
            var registry =
                new CustomerShoppingSessionRegistry();
            registry.TryRegister(
                Session("customer-a", "a"));
            Assert.That(
                registry.PendingCount,
                Is.EqualTo(1));
        }

        [Test] public void AbandonedSession_IsNotPending()
        {
            var registry =
                new CustomerShoppingSessionRegistry();
            CustomerShoppingSession session =
                Session("customer-a", "a");
            session.TryAbandon();
            registry.TryRegister(session);
            Assert.That(
                registry.PendingCount,
                Is.EqualTo(0));
        }

        [Test] public void CheckedOutSession_IsNotPending()
        {
            DayCycleTestScenario scenario =
                DayCycleTestFactory.Scenario();
            CustomerShoppingSession session =
                scenario.Sessions.Sessions[0];

            foreach (ShoppingCartLine line in session.Cart.Lines)
            {
                session.Cart.TryRemove(line.ReservationId);
            }
            session.TryMarkCheckedOut();

            Assert.That(
                scenario.Sessions.PendingCount,
                Is.EqualTo(0));
        }
    }
}
