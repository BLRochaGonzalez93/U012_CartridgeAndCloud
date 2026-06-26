using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.Shopping;
using VRMGames.CartridgeAndCloud.Domain.Customers;
using VRMGames.CartridgeAndCloud.Domain.Displays;
using VRMGames.CartridgeAndCloud.Domain.Shopping;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Shopping
{
    public sealed class ShoppingSearchTests
    {
        [Test] public void Search_FindsStockedPreferredDisplay()
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

        [Test] public void Search_ExcludesEmptyDisplay()
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

        [Test] public void Search_ExcludesFullyReservedDisplay()
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

        [Test] public void Search_PrefersLowerCategoryRank()
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

        [Test] public void Search_RejectsFallbackWhenDisabled()
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

        [Test] public void Search_AllowsFallbackWhenEnabled()
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

        [Test] public void Search_OrdersSameProductByDisplayId()
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

        [Test] public void Availability_ReportsOnHandReservedAvailable()
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

        [Test] public void Availability_EmptyUnassignedDisplayIsZero()
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
