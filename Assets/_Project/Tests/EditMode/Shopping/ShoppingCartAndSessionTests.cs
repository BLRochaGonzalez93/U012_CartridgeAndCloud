using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.Customers;
using VRMGames.CartridgeAndCloud.Domain.Shopping;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Shopping
{
    public sealed class ShoppingCartAndSessionTests
    {
        private static ShoppingCart CreateCart(string customer = "customer-a", int capacity = 3)
        {
            return new ShoppingCart(
                new ShoppingCartId("cart-a"),
                new CustomerInstanceId(customer),
                capacity);
        }

        [Test] public void Cart_StartsEmpty()
        {
            var cart = CreateCart();
            Assert.That(cart.IsEmpty, Is.True);
            Assert.That(cart.TotalUnits, Is.EqualTo(0));
        }

        [Test] public void Cart_AddsActiveReservation()
        {
            var cart = CreateCart();
            Assert.That(cart.TryAdd(ShoppingTestFactory.Reservation()).Succeeded, Is.True);
            Assert.That(cart.TotalUnits, Is.EqualTo(1));
        }

        [Test] public void Cart_RejectsReleasedReservation()
        {
            var cart = CreateCart();
            var reservation = ShoppingTestFactory.Reservation();
            reservation.TryRelease();
            Assert.That(
                cart.TryAdd(reservation).FailureReason,
                Is.EqualTo(ShoppingCartMutationFailureReason.ReservationNotActive));
        }

        [Test] public void Cart_RejectsDifferentCustomer()
        {
            var cart = CreateCart();
            Assert.That(
                cart.TryAdd(
                    ShoppingTestFactory.Reservation(customer: "customer-b")).FailureReason,
                Is.EqualTo(
                    ShoppingCartMutationFailureReason.ReservationOwnedByDifferentCustomer));
        }

        [Test] public void Cart_RejectsDuplicateReservation()
        {
            var cart = CreateCart();
            var reservation = ShoppingTestFactory.Reservation();
            cart.TryAdd(reservation);
            Assert.That(
                cart.TryAdd(reservation).FailureReason,
                Is.EqualTo(ShoppingCartMutationFailureReason.ReservationAlreadyInCart));
        }

        [Test] public void Cart_RejectsCapacityOverflow()
        {
            var cart = CreateCart(capacity: 1);
            Assert.That(
                cart.TryAdd(ShoppingTestFactory.Reservation(quantity: 2)).FailureReason,
                Is.EqualTo(ShoppingCartMutationFailureReason.CartCapacityExceeded));
        }

        [Test] public void Cart_RemovesReservation()
        {
            var cart = CreateCart();
            var reservation = ShoppingTestFactory.Reservation();
            cart.TryAdd(reservation);
            Assert.That(cart.TryRemove(reservation.Id).Succeeded, Is.True);
            Assert.That(cart.IsEmpty, Is.True);
        }

        [Test] public void Cart_RejectsMissingRemoval()
        {
            var cart = CreateCart();
            Assert.That(
                cart.TryRemove(new ShoppingReservationId("missing")).FailureReason,
                Is.EqualTo(ShoppingCartMutationFailureReason.ReservationNotInCart));
        }

        [Test] public void Cart_LinesAreDeterministic()
        {
            var cart = CreateCart();
            cart.TryAdd(ShoppingTestFactory.Reservation("z"));
            cart.TryAdd(ShoppingTestFactory.Reservation("a"));
            Assert.That(cart.Lines[0].ReservationId.Value, Is.EqualTo("a"));
        }

        [Test] public void Session_StartsSearching()
        {
            var customer = new CustomerInstanceId("customer");
            var session = new CustomerShoppingSession(
                customer,
                ShoppingTestFactory.Intent(customer),
                new ShoppingCart(new ShoppingCartId("cart"), customer, 3));
            Assert.That(session.State, Is.EqualTo(CustomerShoppingState.Searching));
        }

        [Test] public void Session_RequiresMatchingOwnership()
        {
            Assert.Throws<System.ArgumentException>(
                () => new CustomerShoppingSession(
                    new CustomerInstanceId("customer-a"),
                    ShoppingTestFactory.Intent(new CustomerInstanceId("customer-b")),
                    new ShoppingCart(
                        new ShoppingCartId("cart"),
                        new CustomerInstanceId("customer-a"),
                        3)));
        }

        [Test] public void Session_CannotBecomeReadyWithEmptyCart()
        {
            var customer = new CustomerInstanceId("customer");
            var session = new CustomerShoppingSession(
                customer,
                ShoppingTestFactory.Intent(customer),
                new ShoppingCart(new ShoppingCartId("cart"), customer, 3));
            Assert.That(session.TryMarkReadyForCheckout(), Is.False);
        }

        [Test] public void Session_CanBeAbandonedOnce()
        {
            var customer = new CustomerInstanceId("customer");
            var session = new CustomerShoppingSession(
                customer,
                ShoppingTestFactory.Intent(customer),
                new ShoppingCart(new ShoppingCartId("cart"), customer, 3));
            Assert.That(session.TryAbandon(), Is.True);
            Assert.That(session.TryAbandon(), Is.False);
        }
    }
}
