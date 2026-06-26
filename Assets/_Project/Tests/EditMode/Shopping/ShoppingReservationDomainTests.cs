using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.Customers;
using VRMGames.CartridgeAndCloud.Domain.Displays;
using VRMGames.CartridgeAndCloud.Domain.Products;
using VRMGames.CartridgeAndCloud.Domain.Shopping;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Shopping
{
    public sealed class ShoppingReservationDomainTests
    {
        [Test] public void Reservation_StartsActive()
        {
            var reservation = ShoppingTestFactory.Reservation();
            Assert.That(reservation.IsActive, Is.True);
            Assert.That(reservation.State, Is.EqualTo(ShoppingReservationState.Active));
        }

        [Test] public void Reservation_RejectsZeroQuantity() =>
            Assert.Throws<System.ArgumentOutOfRangeException>(
                () => ShoppingTestFactory.Reservation(quantity: 0));

        [Test] public void Release_TransitionsActiveReservation()
        {
            var reservation = ShoppingTestFactory.Reservation();
            Assert.That(reservation.TryRelease(), Is.True);
            Assert.That(reservation.State, Is.EqualTo(ShoppingReservationState.Released));
        }

        [Test] public void Release_CannotRunTwice()
        {
            var reservation = ShoppingTestFactory.Reservation();
            reservation.TryRelease();
            Assert.That(reservation.TryRelease(), Is.False);
        }

        [Test] public void Consume_TransitionsActiveReservation()
        {
            var reservation = ShoppingTestFactory.Reservation();
            Assert.That(reservation.TryConsume(), Is.True);
            Assert.That(reservation.State, Is.EqualTo(ShoppingReservationState.Consumed));
        }

        [Test] public void ConsumedReservation_CannotBeReleased()
        {
            var reservation = ShoppingTestFactory.Reservation();
            reservation.TryConsume();
            Assert.That(reservation.TryRelease(), Is.False);
        }

        [Test] public void ReleasedReservation_CannotBeConsumed()
        {
            var reservation = ShoppingTestFactory.Reservation();
            reservation.TryRelease();
            Assert.That(reservation.TryConsume(), Is.False);
        }

        [Test] public void Reservation_StoresProvenance()
        {
            var reservation = ShoppingTestFactory.Reservation();
            Assert.That(reservation.CustomerId.Value, Is.EqualTo("customer-a"));
            Assert.That(reservation.DisplayId.Value, Is.EqualTo("display-a"));
            Assert.That(reservation.ProductId.Value, Is.EqualTo("product-a"));
        }

        [Test] public void Registry_StartsEmpty()
        {
            Assert.That(new ShoppingReservationRegistry().Count, Is.EqualTo(0));
        }

        [Test] public void Registry_RegistersReservation()
        {
            var registry = new ShoppingReservationRegistry();
            Assert.That(registry.TryRegister(ShoppingTestFactory.Reservation()), Is.True);
            Assert.That(registry.Count, Is.EqualTo(1));
        }

        [Test] public void Registry_RejectsDuplicateId()
        {
            var registry = new ShoppingReservationRegistry();
            registry.TryRegister(ShoppingTestFactory.Reservation());
            Assert.That(registry.TryRegister(ShoppingTestFactory.Reservation()), Is.False);
        }

        [Test] public void Registry_SumsActiveReservations()
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

        [Test] public void Registry_ExcludesReleasedReservations()
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

        [Test] public void Registry_FiltersByCustomer()
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

        [Test] public void Registry_GetThrowsForMissing()
        {
            Assert.Throws<System.Collections.Generic.KeyNotFoundException>(
                () => new ShoppingReservationRegistry().Get(
                    new ShoppingReservationId("missing")));
        }
    }
}
