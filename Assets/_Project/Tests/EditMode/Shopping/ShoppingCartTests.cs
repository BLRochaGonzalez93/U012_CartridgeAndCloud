using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.Shopping;
using VRMGames.CartridgeAndCloud.Domain.Customers;
using VRMGames.CartridgeAndCloud.Domain.Displays;
using VRMGames.CartridgeAndCloud.Domain.Inventory;
using VRMGames.CartridgeAndCloud.Domain.Shopping;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Shopping
{
    public sealed class ShoppingCartTests
    {
        private static ShoppingCart CreateCart(string customer = "customer-a", int capacity = 3)
        {
            return new ShoppingCart(
                new ShoppingCartId("cart-a"),
                new CustomerInstanceId(customer),
                capacity);
        }

        [Test]
        public void CartService_AddsRegisteredReservation()
        {
            var product = ShoppingTestFactory.Product();
            var products = ShoppingTestFactory.Products(product);
            var registry = new ShoppingReservationRegistry();
            var reservation = ShoppingTestFactory.Reservation();
            registry.TryRegister(reservation);
            var reservationService =
                new ShoppingReservationService(
                    products,
                    registry,
                    ShoppingTestFactory.Policy());
            var cartService =
                new ShoppingCartService(registry, reservationService);
            var cart = new ShoppingCart(
                new ShoppingCartId("cart"),
                reservation.CustomerId,
                3);
            Assert.That(
                cartService.TryAddReservation(cart, reservation.Id).Succeeded,
                Is.True);
        }

        [Test]
        public void Abandon_ReleasesAllCartReservations()
        {
            var product = ShoppingTestFactory.Product();
            var products = ShoppingTestFactory.Products(product);
            var display = ShoppingTestFactory.Display(products, product);
            var displays = new DisplayInstanceRegistry(new[] { display });
            var registry = new ShoppingReservationRegistry();
            var reservationService =
                new ShoppingReservationService(
                    products,
                    registry,
                    ShoppingTestFactory.Policy());
            var result = reservationService.TryReserve(
                new ShoppingReservationId("r"),
                new CustomerInstanceId("customer"),
                display,
                product.Id,
                new Quantity(1));
            var cart = new ShoppingCart(
                new ShoppingCartId("cart"),
                result.Reservation.CustomerId,
                3);
            var cartService =
                new ShoppingCartService(registry, reservationService);
            cartService.TryAddReservation(cart, result.Reservation.Id);
            Assert.That(cartService.Abandon(cart, displays), Is.EqualTo(1));
            Assert.That(cart.IsEmpty, Is.True);
            Assert.That(result.Reservation.IsActive, Is.False);
        }

        [Test]
        public void ReleaseLine_RejectsMissingCartLine()
        {
            var product = ShoppingTestFactory.Product();
            var products = ShoppingTestFactory.Products(product);
            var display = ShoppingTestFactory.Display(products, product);
            var displays = new DisplayInstanceRegistry(new[] { display });
            var registry = new ShoppingReservationRegistry();
            var reservationService =
                new ShoppingReservationService(
                    products,
                    registry,
                    ShoppingTestFactory.Policy());
            var cartService =
                new ShoppingCartService(registry, reservationService);
            var cart = new ShoppingCart(
                new ShoppingCartId("cart"),
                new CustomerInstanceId("customer"),
                3);
            Assert.That(
                cartService.TryReleaseLine(
                    cart,
                    new ShoppingReservationId("missing"),
                    displays).FailureReason,
                Is.EqualTo(ShoppingCartMutationFailureReason.ReservationNotInCart));
        }

        [Test]
        public void Consistency_PassesForEmptyCart()
        {
            var product = ShoppingTestFactory.Product();
            var products = ShoppingTestFactory.Products(product);
            var display = ShoppingTestFactory.Display(products, product);
            var registry = new ShoppingReservationRegistry();
            var cart = new ShoppingCart(
                new ShoppingCartId("cart"),
                new CustomerInstanceId("customer"),
                3);
            var report = new ShoppingConsistencyService(registry)
                .Validate(new DisplayInstanceRegistry(new[] { display }), cart);
            Assert.That(report.IsConsistent, Is.True);
        }

        [Test]
        public void Consistency_PassesForBackedCartLine()
        {
            var product = ShoppingTestFactory.Product();
            var products = ShoppingTestFactory.Products(product);
            var display = ShoppingTestFactory.Display(products, product);
            var registry = new ShoppingReservationRegistry();
            var reservation = ShoppingTestFactory.Reservation();
            registry.TryRegister(reservation);
            var cart = new ShoppingCart(
                new ShoppingCartId("cart"),
                reservation.CustomerId,
                3);
            cart.TryAdd(reservation);
            var report = new ShoppingConsistencyService(registry)
                .Validate(new DisplayInstanceRegistry(new[] { display }), cart);
            Assert.That(report.IsConsistent, Is.True);
            Assert.That(report.CheckedCartLines, Is.EqualTo(1));
        }

        [Test]
        public void Consistency_FailsWhenCartReservationReleased()
        {
            var product = ShoppingTestFactory.Product();
            var products = ShoppingTestFactory.Products(product);
            var display = ShoppingTestFactory.Display(products, product);
            var registry = new ShoppingReservationRegistry();
            var reservation = ShoppingTestFactory.Reservation();
            registry.TryRegister(reservation);
            var cart = new ShoppingCart(
                new ShoppingCartId("cart"),
                reservation.CustomerId,
                3);
            cart.TryAdd(reservation);
            reservation.TryRelease();
            var report = new ShoppingConsistencyService(registry)
                .Validate(new DisplayInstanceRegistry(new[] { display }), cart);
            Assert.That(report.IsConsistent, Is.False);
        }

        [Test]
        public void Availability_ThrowsWhenReservationsExceedStock()
        {
            var product = ShoppingTestFactory.Product();
            var products = ShoppingTestFactory.Products(product);
            var display = ShoppingTestFactory.Display(products, product, stock: 1);
            var registry = new ShoppingReservationRegistry();
            registry.TryRegister(ShoppingTestFactory.Reservation("r", quantity: 2));
            Assert.Throws<System.InvalidOperationException>(
                () => new ShoppingAvailabilityService(registry)
                    .GetAvailability(display));
        }

        [Test]
        public void PhysicalConservation_Holds()
        {
            var product = ShoppingTestFactory.Product();
            var products = ShoppingTestFactory.Products(product);
            var display = ShoppingTestFactory.Display(products, product);
            var registry = new ShoppingReservationRegistry();
            registry.TryRegister(ShoppingTestFactory.Reservation());
            var snapshot =
                new ShoppingAvailabilityService(registry)
                    .GetAvailability(display);
            Assert.That(
                snapshot.OnHand.Value,
                Is.EqualTo(snapshot.Available.Value + snapshot.Reserved.Value));
        }

        [Test]
        public void CartLineQuantityMatchesReservation()
        {
            var reservation = ShoppingTestFactory.Reservation(quantity: 2);
            var cart = new ShoppingCart(
                new ShoppingCartId("cart"),
                reservation.CustomerId,
                3);
            cart.TryAdd(reservation);
            Assert.That(cart.Lines[0].Quantity, Is.EqualTo(reservation.Quantity));
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
