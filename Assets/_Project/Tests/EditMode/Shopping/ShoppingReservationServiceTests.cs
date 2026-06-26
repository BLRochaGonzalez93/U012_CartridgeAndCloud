using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.Shopping;
using VRMGames.CartridgeAndCloud.Domain.Customers;
using VRMGames.CartridgeAndCloud.Domain.Displays;
using VRMGames.CartridgeAndCloud.Domain.Inventory;
using VRMGames.CartridgeAndCloud.Domain.Products;
using VRMGames.CartridgeAndCloud.Domain.Shopping;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Shopping
{
    public sealed class ShoppingReservationServiceTests
    {
        private static ShoppingReservationService Create(
            ProductDefinitionRegistry products,
            ShoppingReservationRegistry reservations,
            int maxPerReservation = 1)
        {
            return new ShoppingReservationService(
                products,
                reservations,
                ShoppingTestFactory.Policy(reservation: maxPerReservation));
        }

        [Test] public void Reserve_SucceedsForAvailableStock()
        {
            var product = ShoppingTestFactory.Product();
            var products = ShoppingTestFactory.Products(product);
            var display = ShoppingTestFactory.Display(products, product);
            var result = Create(products, new ShoppingReservationRegistry())
                .TryReserve(
                    new ShoppingReservationId("r"),
                    new CustomerInstanceId("customer"),
                    display,
                    product.Id,
                    new Quantity(1));
            Assert.That(result.Succeeded, Is.True);
            Assert.That(result.AvailableAfter.Value, Is.EqualTo(2));
        }

        [Test] public void Reserve_DoesNotRemovePhysicalStock()
        {
            var product = ShoppingTestFactory.Product();
            var products = ShoppingTestFactory.Products(product);
            var display = ShoppingTestFactory.Display(products, product);
            Create(products, new ShoppingReservationRegistry())
                .TryReserve(
                    new ShoppingReservationId("r"),
                    new CustomerInstanceId("customer"),
                    display,
                    product.Id,
                    new Quantity(1));
            Assert.That(
                display.Inventory.GetQuantity(product.Id).Value,
                Is.EqualTo(3));
        }

        [Test] public void Reserve_RejectsDuplicateId()
        {
            var product = ShoppingTestFactory.Product();
            var products = ShoppingTestFactory.Products(product);
            var display = ShoppingTestFactory.Display(products, product);
            var registry = new ShoppingReservationRegistry();
            var service = Create(products, registry);
            service.TryReserve(
                new ShoppingReservationId("r"),
                new CustomerInstanceId("customer"),
                display,
                product.Id,
                new Quantity(1));
            Assert.That(
                service.TryReserve(
                    new ShoppingReservationId("r"),
                    new CustomerInstanceId("customer"),
                    display,
                    product.Id,
                    new Quantity(1)).FailureReason,
                Is.EqualTo(ShoppingReservationFailureReason.DuplicateReservationId));
        }

        [Test] public void Reserve_RejectsPolicyOverflow()
        {
            var product = ShoppingTestFactory.Product();
            var products = ShoppingTestFactory.Products(product);
            var display = ShoppingTestFactory.Display(products, product);
            Assert.That(
                Create(products, new ShoppingReservationRegistry())
                    .TryReserve(
                        new ShoppingReservationId("r"),
                        new CustomerInstanceId("customer"),
                        display,
                        product.Id,
                        new Quantity(2)).FailureReason,
                Is.EqualTo(ShoppingReservationFailureReason.QuantityExceedsPolicy));
        }

        [Test] public void Reserve_RejectsProductMismatch()
        {
            var product = ShoppingTestFactory.Product();
            var products = ShoppingTestFactory.Products(product);
            var display = ShoppingTestFactory.Display(products, product);
            Assert.That(
                Create(products, new ShoppingReservationRegistry())
                    .TryReserve(
                        new ShoppingReservationId("r"),
                        new CustomerInstanceId("customer"),
                        display,
                        new ProductDefinitionId("other"),
                        new Quantity(1)).FailureReason,
                Is.EqualTo(ShoppingReservationFailureReason.ProductMismatch));
        }

        [Test] public void Reserve_PreventsOverselling()
        {
            var product = ShoppingTestFactory.Product();
            var products = ShoppingTestFactory.Products(product);
            var display = ShoppingTestFactory.Display(products, product, stock: 1);
            var registry = new ShoppingReservationRegistry();
            var service = Create(products, registry);
            service.TryReserve(
                new ShoppingReservationId("r1"),
                new CustomerInstanceId("customer-1"),
                display,
                product.Id,
                new Quantity(1));
            Assert.That(
                service.TryReserve(
                    new ShoppingReservationId("r2"),
                    new CustomerInstanceId("customer-2"),
                    display,
                    product.Id,
                    new Quantity(1)).FailureReason,
                Is.EqualTo(ShoppingReservationFailureReason.InsufficientAvailableQuantity));
        }

        [Test] public void Release_RestoresAvailability()
        {
            var product = ShoppingTestFactory.Product();
            var products = ShoppingTestFactory.Products(product);
            var display = ShoppingTestFactory.Display(products, product, stock: 1);
            var displays = new DisplayInstanceRegistry(new[] { display });
            var registry = new ShoppingReservationRegistry();
            var service = Create(products, registry);
            service.TryReserve(
                new ShoppingReservationId("r"),
                new CustomerInstanceId("customer"),
                display,
                product.Id,
                new Quantity(1));
            var result = service.TryRelease(
                new ShoppingReservationId("r"),
                displays);
            Assert.That(result.Succeeded, Is.True);
            Assert.That(result.AvailableAfter.Value, Is.EqualTo(1));
        }

        [Test] public void Release_RejectsSecondRelease()
        {
            var product = ShoppingTestFactory.Product();
            var products = ShoppingTestFactory.Products(product);
            var display = ShoppingTestFactory.Display(products, product);
            var displays = new DisplayInstanceRegistry(new[] { display });
            var registry = new ShoppingReservationRegistry();
            var service = Create(products, registry);
            service.TryReserve(
                new ShoppingReservationId("r"),
                new CustomerInstanceId("customer"),
                display,
                product.Id,
                new Quantity(1));
            service.TryRelease(new ShoppingReservationId("r"), displays);
            Assert.That(
                service.TryRelease(
                    new ShoppingReservationId("r"),
                    displays).FailureReason,
                Is.EqualTo(ShoppingReservationFailureReason.ReservationNotActive));
        }

        [Test] public void Release_MissingReservationFails()
        {
            var product = ShoppingTestFactory.Product();
            var products = ShoppingTestFactory.Products(product);
            var displays = new DisplayInstanceRegistry(
                new[] { ShoppingTestFactory.Display(products, product) });
            Assert.That(
                Create(products, new ShoppingReservationRegistry())
                    .TryRelease(
                        new ShoppingReservationId("missing"),
                        displays).FailureReason,
                Is.EqualTo(ShoppingReservationFailureReason.ReservationNotFound));
        }
    }
}
