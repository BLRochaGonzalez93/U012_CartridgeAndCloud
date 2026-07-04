using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.Shopping;
using VRMGames.CartridgeAndCloud.Domain.Customers;
using VRMGames.CartridgeAndCloud.Domain.Displays;
using VRMGames.CartridgeAndCloud.Domain.Shopping;

using VRMGames.CartridgeAndCloud.Domain.Inventory;
using VRMGames.CartridgeAndCloud.Domain.Products;
namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Shopping
{
    public sealed class ShoppingDiscoveryFlowTests
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
        [Test]
        public void Search_FindsStockedPreferredDisplay()
        {
            var product = ShoppingTestFactory.Product();
            var products = ShoppingTestFactory.Products(product);
            var display = ShoppingTestFactory.Display(products, product);
            var service = new ShoppingSearchService(
                products,
                new ShoppingReservationRegistry(),
                ShoppingTestFactory.Policy());
            var results = service.FindCandidates(
                ShoppingTestFactory.Intent(new CustomerInstanceId("customer")),
                new DisplayInstanceRegistry(new[] { display }));
            Assert.That(results.Count, Is.EqualTo(1));
        }

        [Test]
        public void Search_ExcludesEmptyDisplay()
        {
            var product = ShoppingTestFactory.Product();
            var products = ShoppingTestFactory.Products(product);
            var display = ShoppingTestFactory.Display(products, product, stock: 0);
            var service = new ShoppingSearchService(
                products,
                new ShoppingReservationRegistry(),
                ShoppingTestFactory.Policy());
            Assert.That(
                service.FindCandidates(
                    ShoppingTestFactory.Intent(new CustomerInstanceId("customer")),
                    new DisplayInstanceRegistry(new[] { display })).Count,
                Is.EqualTo(0));
        }

        [Test]
        public void Search_ExcludesFullyReservedDisplay()
        {
            var product = ShoppingTestFactory.Product();
            var products = ShoppingTestFactory.Products(product);
            var display = ShoppingTestFactory.Display(products, product, stock: 1);
            var reservations = new ShoppingReservationRegistry();
            reservations.TryRegister(ShoppingTestFactory.Reservation());
            var service = new ShoppingSearchService(
                products,
                reservations,
                ShoppingTestFactory.Policy());
            Assert.That(
                service.FindCandidates(
                    ShoppingTestFactory.Intent(new CustomerInstanceId("customer")),
                    new DisplayInstanceRegistry(new[] { display })).Count,
                Is.EqualTo(0));
        }

        [Test]
        public void Search_PrefersLowerCategoryRank()
        {
            var game = ShoppingTestFactory.ProductWithCategory(
                "game",
                ShoppingTestFactory.VideoGame);
            var accessory = ShoppingTestFactory.ProductWithCategory(
                "accessory",
                ShoppingTestFactory.Accessory);
            var products = ShoppingTestFactory.Products(game, accessory);
            var displays = new DisplayInstanceRegistry(
                new[] {
                    ShoppingTestFactory.Display(products, game, "display-game"),
                    ShoppingTestFactory.Display(products, accessory, "display-accessory")
                });
            var intent = ShoppingTestFactory.IntentWithCategories(
                new CustomerInstanceId("customer"),
                1,
                ShoppingTestFactory.Accessory,
                ShoppingTestFactory.VideoGame);
            var service = new ShoppingSearchService(
                products,
                new ShoppingReservationRegistry(),
                ShoppingTestFactory.Policy());
            Assert.That(
                service.FindCandidates(intent, displays)[0].ProductId.Value,
                Is.EqualTo("accessory"));
        }

        [Test]
        public void Search_RejectsFallbackWhenDisabled()
        {
            var product = ShoppingTestFactory.ProductWithCategory(
                "accessory",
                ShoppingTestFactory.Accessory);
            var products = ShoppingTestFactory.Products(product);
            var service = new ShoppingSearchService(
                products,
                new ShoppingReservationRegistry(),
                ShoppingTestFactory.Policy(fallback: false));
            Assert.That(
                service.FindCandidates(
                    ShoppingTestFactory.Intent(new CustomerInstanceId("customer")),
                    new DisplayInstanceRegistry(
                        new[] { ShoppingTestFactory.Display(products, product) })).Count,
                Is.EqualTo(0));
        }

        [Test]
        public void Search_AllowsFallbackWhenEnabled()
        {
            var product = ShoppingTestFactory.ProductWithCategory(
                "accessory",
                ShoppingTestFactory.Accessory);
            var products = ShoppingTestFactory.Products(product);
            var service = new ShoppingSearchService(
                products,
                new ShoppingReservationRegistry(),
                ShoppingTestFactory.Policy(fallback: true));
            Assert.That(
                service.FindCandidates(
                    ShoppingTestFactory.Intent(new CustomerInstanceId("customer")),
                    new DisplayInstanceRegistry(
                        new[] { ShoppingTestFactory.Display(products, product) })).Count,
                Is.EqualTo(1));
        }

        [Test]
        public void Search_OrdersSameProductByDisplayId()
        {
            var product = ShoppingTestFactory.Product();
            var products = ShoppingTestFactory.Products(product);
            var displays = new DisplayInstanceRegistry(
                new[] {
                    ShoppingTestFactory.Display(products, product, "z-display"),
                    ShoppingTestFactory.Display(products, product, "a-display")
                });
            var service = new ShoppingSearchService(
                products,
                new ShoppingReservationRegistry(),
                ShoppingTestFactory.Policy());
            Assert.That(
                service.FindCandidates(
                    ShoppingTestFactory.Intent(new CustomerInstanceId("customer")),
                    displays)[0].DisplayId.Value,
                Is.EqualTo("a-display"));
        }

        [Test]
        public void Availability_ReportsOnHandReservedAvailable()
        {
            var product = ShoppingTestFactory.Product();
            var products = ShoppingTestFactory.Products(product);
            var display = ShoppingTestFactory.Display(products, product);
            var reservations = new ShoppingReservationRegistry();
            reservations.TryRegister(ShoppingTestFactory.Reservation());
            var snapshot =
                new ShoppingAvailabilityService(reservations)
                    .GetAvailability(display);
            Assert.That(snapshot.OnHand.Value, Is.EqualTo(3));
            Assert.That(snapshot.Reserved.Value, Is.EqualTo(1));
            Assert.That(snapshot.Available.Value, Is.EqualTo(2));
        }

        [Test]
        public void Availability_EmptyUnassignedDisplayIsZero()
        {
            var definition = new VRMGames.CartridgeAndCloud.Domain.Displays.DisplayDefinition(
                new VRMGames.CartridgeAndCloud.Domain.Displays.DisplayDefinitionId("definition"),
                "display.name",
                new VRMGames.CartridgeAndCloud.Domain.Inventory.InventoryCapacity(3),
                1,
                new VRMGames.CartridgeAndCloud.Domain.Products.ProductCategoryId[0],
                "technical");
            var display = new VRMGames.CartridgeAndCloud.Domain.Displays.DisplayInstance(
                new VRMGames.CartridgeAndCloud.Domain.Displays.DisplayInstanceId("display"),
                definition);
            var snapshot = new ShoppingAvailabilityService(
                new ShoppingReservationRegistry()).GetAvailability(display);
            Assert.That(snapshot.Available.Value, Is.EqualTo(0));
        }
    }
}
