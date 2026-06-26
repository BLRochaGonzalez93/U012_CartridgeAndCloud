using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.Shopping;
using VRMGames.CartridgeAndCloud.Domain.Customers;
using VRMGames.CartridgeAndCloud.Domain.Displays;
using VRMGames.CartridgeAndCloud.Domain.Shopping;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Shopping
{
    public sealed class ShoppingFlowTests
    {
        private static CustomerInstance CreateCustomer(
            string customerId = "customer-a",
            string profileId = "profile-a",
            bool browsing = true)
        {
            CustomerNavigationPlan plan = new CustomerNavigationPlan(
                new[] {
                    new CustomerNavigationTarget(
                        new CustomerNavigationPointId("entry"),
                        CustomerNavigationTargetType.Entry,
                        0),
                    new CustomerNavigationTarget(
                        new CustomerNavigationPointId("browse"),
                        CustomerNavigationTargetType.Browse,
                        1),
                    new CustomerNavigationTarget(
                        new CustomerNavigationPointId("exit"),
                        CustomerNavigationTargetType.Exit,
                        0)
                });
            CustomerInstance customer = new CustomerInstance(
                new CustomerInstanceId(customerId),
                new CustomerProfileId(profileId),
                plan,
                30);
            if (browsing)
            {
                customer.BeginEntering();
                customer.ArriveAtCurrentTarget();
            }
            return customer;
        }

        private static ShoppingFlowService CreateFlow(
            VRMGames.CartridgeAndCloud.Domain.Products.ProductDefinitionRegistry products,
            ShoppingReservationRegistry reservations,
            ShoppingPolicy policy)
        {
            ShoppingReservationService reservationService =
                new ShoppingReservationService(products, reservations, policy);
            ShoppingCartService cartService =
                new ShoppingCartService(reservations, reservationService);
            return new ShoppingFlowService(
                new ShoppingSearchService(products, reservations, policy),
                reservationService,
                cartService,
                policy);
        }

        [Test] public void Flow_ReservesBestCandidateAndAddsCartLine()
        {
            var product = ShoppingTestFactory.Product();
            var products = ShoppingTestFactory.Products(product);
            var display = ShoppingTestFactory.Display(products, product);
            var displays = new DisplayInstanceRegistry(new[] { display });
            var customer = CreateCustomer();
            var profile = ShoppingTestFactory.Profile();
            var intent = ShoppingTestFactory.Intent(customer.Id);
            var cart = new ShoppingCart(
                new ShoppingCartId("cart"),
                customer.Id,
                3);
            var session = new CustomerShoppingSession(customer.Id, intent, cart);
            var reservations = new ShoppingReservationRegistry();

            ShoppingFlowResult result = CreateFlow(
                products,
                reservations,
                ShoppingTestFactory.Policy())
                .TryReserveBestCandidate(
                    customer,
                    profile,
                    session,
                    displays,
                    new ShoppingReservationId("r"));

            Assert.That(result.Succeeded, Is.True);
            Assert.That(cart.TotalUnits, Is.EqualTo(1));
            Assert.That(session.State, Is.EqualTo(CustomerShoppingState.HoldingReservations));
            Assert.That(reservations.Count, Is.EqualTo(1));
        }

        [Test] public void Flow_RejectsCustomerWhoIsNotBrowsing()
        {
            var product = ShoppingTestFactory.Product();
            var products = ShoppingTestFactory.Products(product);
            var displays = new DisplayInstanceRegistry(
                new[] { ShoppingTestFactory.Display(products, product) });
            var customer = CreateCustomer(browsing: false);
            var profile = ShoppingTestFactory.Profile();
            var session = new CustomerShoppingSession(
                customer.Id,
                ShoppingTestFactory.Intent(customer.Id),
                new ShoppingCart(new ShoppingCartId("cart"), customer.Id, 3));
            var reservations = new ShoppingReservationRegistry();

            var result = CreateFlow(
                products,
                reservations,
                ShoppingTestFactory.Policy())
                .TryReserveBestCandidate(
                    customer,
                    profile,
                    session,
                    displays,
                    new ShoppingReservationId("r"));

            Assert.That(
                result.FailureReason,
                Is.EqualTo(ShoppingFlowFailureReason.CustomerNotBrowsing));
            Assert.That(reservations.Count, Is.EqualTo(0));
        }

        [Test] public void Flow_RejectsProfileOwnershipMismatch()
        {
            var product = ShoppingTestFactory.Product();
            var products = ShoppingTestFactory.Products(product);
            var displays = new DisplayInstanceRegistry(
                new[] { ShoppingTestFactory.Display(products, product) });
            var customer = CreateCustomer();
            var wrongProfile = ShoppingTestFactory.Profile("other-profile");
            var session = new CustomerShoppingSession(
                customer.Id,
                ShoppingTestFactory.Intent(customer.Id),
                new ShoppingCart(new ShoppingCartId("cart"), customer.Id, 3));

            var result = CreateFlow(
                products,
                new ShoppingReservationRegistry(),
                ShoppingTestFactory.Policy())
                .TryReserveBestCandidate(
                    customer,
                    wrongProfile,
                    session,
                    displays,
                    new ShoppingReservationId("r"));

            Assert.That(
                result.FailureReason,
                Is.EqualTo(ShoppingFlowFailureReason.OwnershipMismatch));
        }

        [Test] public void Flow_ReportsNoCandidateForEmptyDisplay()
        {
            var product = ShoppingTestFactory.Product();
            var products = ShoppingTestFactory.Products(product);
            var displays = new DisplayInstanceRegistry(
                new[] { ShoppingTestFactory.Display(products, product, stock: 0) });
            var customer = CreateCustomer();
            var profile = ShoppingTestFactory.Profile();
            var session = new CustomerShoppingSession(
                customer.Id,
                ShoppingTestFactory.Intent(customer.Id),
                new ShoppingCart(new ShoppingCartId("cart"), customer.Id, 3));

            var result = CreateFlow(
                products,
                new ShoppingReservationRegistry(),
                ShoppingTestFactory.Policy())
                .TryReserveBestCandidate(
                    customer,
                    profile,
                    session,
                    displays,
                    new ShoppingReservationId("r"));

            Assert.That(
                result.FailureReason,
                Is.EqualTo(ShoppingFlowFailureReason.NoCandidate));
        }

        [Test] public void Flow_RejectsWhenCartHasNoRemainingCapacity()
        {
            var product = ShoppingTestFactory.Product();
            var products = ShoppingTestFactory.Products(product);
            var display = ShoppingTestFactory.Display(products, product);
            var displays = new DisplayInstanceRegistry(new[] { display });
            var customer = CreateCustomer();
            var profile = ShoppingTestFactory.Profile();
            var reservations = new ShoppingReservationRegistry();
            var existing = ShoppingTestFactory.Reservation(
                "existing",
                customer.Id.Value,
                display.Id.Value,
                product.Id.Value,
                1);
            reservations.TryRegister(existing);
            var cart = new ShoppingCart(
                new ShoppingCartId("cart"),
                customer.Id,
                1);
            cart.TryAdd(existing);
            var session = new CustomerShoppingSession(
                customer.Id,
                ShoppingTestFactory.Intent(customer.Id),
                cart);

            var result = CreateFlow(
                products,
                reservations,
                ShoppingTestFactory.Policy(cart: 1, reservation: 1))
                .TryReserveBestCandidate(
                    customer,
                    profile,
                    session,
                    displays,
                    new ShoppingReservationId("new"));

            Assert.That(
                result.FailureReason,
                Is.EqualTo(ShoppingFlowFailureReason.CartRejected));
            Assert.That(reservations.Count, Is.EqualTo(1));
        }
    }
}
