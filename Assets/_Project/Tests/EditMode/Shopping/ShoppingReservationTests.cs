using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.Shopping;
using VRMGames.CartridgeAndCloud.Domain.Customers;
using VRMGames.CartridgeAndCloud.Domain.Displays;
using VRMGames.CartridgeAndCloud.Domain.Inventory;
using VRMGames.CartridgeAndCloud.Domain.Products;
using VRMGames.CartridgeAndCloud.Domain.Shopping;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Shopping
{
    public sealed class ShoppingReservationTests
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

        [Test]
        public void Reservation_StartsActive()
        {
            var reservation = ShoppingTestFactory.Reservation();
            Assert.That(reservation.IsActive, Is.True);
            Assert.That(reservation.State, Is.EqualTo(ShoppingReservationState.Active));
        }

        [Test]
        public void Reservation_RejectsZeroQuantity() =>
            Assert.Throws<System.ArgumentOutOfRangeException>(
                () => ShoppingTestFactory.Reservation(quantity: 0));

        [Test]
        public void Release_TransitionsActiveReservation()
        {
            var reservation = ShoppingTestFactory.Reservation();
            Assert.That(reservation.TryRelease(), Is.True);
            Assert.That(reservation.State, Is.EqualTo(ShoppingReservationState.Released));
        }

        [Test]
        public void Release_CannotRunTwice()
        {
            var reservation = ShoppingTestFactory.Reservation();
            reservation.TryRelease();
            Assert.That(reservation.TryRelease(), Is.False);
        }

        [Test]
        public void Consume_TransitionsActiveReservation()
        {
            var reservation = ShoppingTestFactory.Reservation();
            Assert.That(reservation.TryConsume(), Is.True);
            Assert.That(reservation.State, Is.EqualTo(ShoppingReservationState.Consumed));
        }

        [Test]
        public void ConsumedReservation_CannotBeReleased()
        {
            var reservation = ShoppingTestFactory.Reservation();
            reservation.TryConsume();
            Assert.That(reservation.TryRelease(), Is.False);
        }

        [Test]
        public void ReleasedReservation_CannotBeConsumed()
        {
            var reservation = ShoppingTestFactory.Reservation();
            reservation.TryRelease();
            Assert.That(reservation.TryConsume(), Is.False);
        }

        [Test]
        public void Reservation_StoresProvenance()
        {
            var reservation = ShoppingTestFactory.Reservation();
            Assert.That(reservation.CustomerId.Value, Is.EqualTo("customer-a"));
            Assert.That(reservation.DisplayId.Value, Is.EqualTo("display-a"));
            Assert.That(reservation.ProductId.Value, Is.EqualTo("product-a"));
        }

        [Test]
        public void Registry_StartsEmpty()
        {
            Assert.That(new ShoppingReservationRegistry().Count, Is.EqualTo(0));
        }

        [Test]
        public void Registry_RegistersReservation()
        {
            var registry = new ShoppingReservationRegistry();
            Assert.That(registry.TryRegister(ShoppingTestFactory.Reservation()), Is.True);
            Assert.That(registry.Count, Is.EqualTo(1));
        }

        [Test]
        public void Registry_RejectsDuplicateId()
        {
            var registry = new ShoppingReservationRegistry();
            registry.TryRegister(ShoppingTestFactory.Reservation());
            Assert.That(registry.TryRegister(ShoppingTestFactory.Reservation()), Is.False);
        }

        [Test]
        public void Registry_SumsActiveReservations()
        {
            var registry = new ShoppingReservationRegistry();
            registry.TryRegister(ShoppingTestFactory.Reservation("r1"));
            registry.TryRegister(ShoppingTestFactory.Reservation("r2"));
            Assert.That(
                registry.GetActiveReservedQuantity(
                    new DisplayInstanceId("display-a"),
                    new ProductDefinitionId("product-a")).Value,
                Is.EqualTo(2));
        }

        [Test]
        public void Registry_ExcludesReleasedReservations()
        {
            var registry = new ShoppingReservationRegistry();
            var reservation = ShoppingTestFactory.Reservation();
            registry.TryRegister(reservation);
            reservation.TryRelease();
            Assert.That(
                registry.GetActiveReservedQuantity(
                    reservation.DisplayId,
                    reservation.ProductId).Value,
                Is.EqualTo(0));
        }

        [Test]
        public void Registry_FiltersByCustomer()
        {
            var registry = new ShoppingReservationRegistry();
            registry.TryRegister(ShoppingTestFactory.Reservation("r1", "customer-a"));
            registry.TryRegister(ShoppingTestFactory.Reservation("r2", "customer-b"));
            Assert.That(
                registry.GetForCustomer(
                    new CustomerInstanceId("customer-a"),
                    true).Count,
                Is.EqualTo(1));
        }

        [Test]
        public void Registry_GetThrowsForMissing()
        {
            Assert.Throws<System.Collections.Generic.KeyNotFoundException>(
                () => new ShoppingReservationRegistry().Get(
                    new ShoppingReservationId("missing")));
        }
    }
}
