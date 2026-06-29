using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.Shopping;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.DayCycle
{
    public sealed class ShoppingReservationRegistryDayCycleTests
    {
        [Test] public void ActiveCount_StartsZero()
        {
            Assert.That(
                new ShoppingReservationRegistry()
                    .ActiveCount,
                Is.EqualTo(0));
        }

        [Test] public void ActiveCount_IncludesActiveReservation()
        {
            DayCycleTestScenario scenario =
                DayCycleTestFactory.Scenario();
            Assert.That(
                scenario.Reservations.ActiveCount,
                Is.EqualTo(1));
        }

        [Test] public void ActiveCount_ExcludesReleased()
        {
            DayCycleTestScenario scenario =
                DayCycleTestFactory.Scenario();
            ShoppingReservation reservation =
                scenario.Reservations.Reservations[0];
            reservation.TryRelease();
            Assert.That(
                scenario.Reservations.ActiveCount,
                Is.EqualTo(0));
        }

        [Test] public void ActiveCount_ExcludesConsumed()
        {
            DayCycleTestScenario scenario =
                DayCycleTestFactory.Scenario();
            ShoppingReservation reservation =
                scenario.Reservations.Reservations[0];
            reservation.TryConsume();
            Assert.That(
                scenario.Reservations.ActiveCount,
                Is.EqualTo(0));
        }

        [Test] public void Snapshot_ContainsRegisteredReservation()
        {
            DayCycleTestScenario scenario =
                DayCycleTestFactory.Scenario();
            Assert.That(
                scenario.Reservations.Reservations.Count,
                Is.EqualTo(1));
        }

        [Test] public void Snapshot_IsOrderedById()
        {
            DayCycleTestScenario scenario =
                DayCycleTestFactory.Scenario(
                    addReadySession: false);
            var customer =
                scenario.Customers.Instances[0];

            var z = new ShoppingReservation(
                new ShoppingReservationId("z"),
                customer.Id,
                scenario.Display.Id,
                scenario.Product.Id,
                new VRMGames.CartridgeAndCloud.Domain.Inventory
                    .Quantity(1));
            var a = new ShoppingReservation(
                new ShoppingReservationId("a"),
                customer.Id,
                scenario.Display.Id,
                scenario.Product.Id,
                new VRMGames.CartridgeAndCloud.Domain.Inventory
                    .Quantity(1));
            scenario.Reservations.TryRegister(z);
            scenario.Reservations.TryRegister(a);

            Assert.That(
                scenario.Reservations.Reservations[0]
                    .Id.Value,
                Is.EqualTo("a"));
        }

        [Test] public void Snapshot_PreservesStateChanges()
        {
            DayCycleTestScenario scenario =
                DayCycleTestFactory.Scenario();
            ShoppingReservation reservation =
                scenario.Reservations.Reservations[0];
            reservation.TryRelease();
            Assert.That(
                scenario.Reservations.Reservations[0]
                    .State,
                Is.EqualTo(
                    ShoppingReservationState.Released));
        }

        [Test] public void ActiveCount_TracksMixedStates()
        {
            DayCycleTestScenario scenario =
                DayCycleTestFactory.Scenario(
                    addReadySession: false);
            var customer =
                scenario.Customers.Instances[0];

            for (int index = 0; index < 3; index++)
            {
                scenario.Reservations.TryRegister(
                    new ShoppingReservation(
                        new ShoppingReservationId(
                            "r-" + index),
                        customer.Id,
                        scenario.Display.Id,
                        scenario.Product.Id,
                        new VRMGames.CartridgeAndCloud.Domain.Inventory
                            .Quantity(1)));
            }

            scenario.Reservations.Reservations[0]
                .TryRelease();
            scenario.Reservations.Reservations[1]
                .TryConsume();

            Assert.That(
                scenario.Reservations.ActiveCount,
                Is.EqualTo(1));
        }
    }
}
