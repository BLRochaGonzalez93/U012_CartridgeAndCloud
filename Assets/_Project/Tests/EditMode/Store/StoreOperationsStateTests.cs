using System;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;
using VRMGames.CartridgeAndCloud.Domain.Store;
namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Store
{
    public sealed class StoreOperationsStateTests
    {
        [Test]
        public void Empty_UsesSlotAndSession()
        {
            StoreOperationsState state =
                StoreOperationsState.Empty(
                    new SaveSlotId(1),
                    "session-a");

            Assert.That(
                state.SlotId,
                Is.EqualTo(new SaveSlotId(1)));
            Assert.That(
                state.SessionId,
                Is.EqualTo("session-a"));
        }

        [Test]
        public void Empty_StartsWithSequencesAtOne()
        {
            StoreOperationsState state =
                Empty();

            Assert.That(
                state.NextOrderSequence,
                Is.EqualTo(1));
            Assert.That(
                state.NextFixtureSequence,
                Is.EqualTo(1));
            Assert.That(
                state.NextCustomerSequence,
                Is.EqualTo(1));
        }

        [Test]
        public void Empty_HasNoRecords()
        {
            StoreOperationsState state =
                Empty();

            Assert.That(
                state.Orders.Count,
                Is.EqualTo(0));
            Assert.That(
                state.Fixtures.Count,
                Is.EqualTo(0));
        }

        [Test]
        public void State_RejectsEmptySession()
        {
            Assert.Throws<ArgumentException>(
                () => CreateState(sessionId: ""));
        }

        [Test]
        public void State_RejectsNegativeGeneration()
        {
            Assert.Throws<
                ArgumentOutOfRangeException>(
                () => CreateState(generation: -1));
        }

        [Test]
        public void State_RejectsZeroOrderSequence()
        {
            Assert.Throws<
                ArgumentOutOfRangeException>(
                () => CreateState(
                    nextOrderSequence: 0));
        }

        [Test]
        public void State_RejectsNegativeSales()
        {
            Assert.Throws<
                ArgumentOutOfRangeException>(
                () => CreateState(
                    completedSales: -1));
        }

        [Test]
        public void Order_ReceiveAllChangesState()
        {
            StoreOrderRecord order =
                Order().ReceiveAll();

            Assert.That(
                order.State,
                Is.EqualTo(
                    StoreOrderStatus.Received));
            Assert.That(
                order.ReceivedUnits,
                Is.EqualTo(order.OrderedUnits));
        }

        [Test]
        public void Order_RejectsZeroUnits()
        {
            Assert.Throws<
                ArgumentOutOfRangeException>(
                () => new StoreOrderRecord(
                    "order",
                    "item",
                    true,
                    StoreOrderStatus.Ordered,
                    0,
                    0,
                    100));
        }

        [Test]
        public void Order_RejectsReceivedAboveOrdered()
        {
            Assert.Throws<
                ArgumentOutOfRangeException>(
                () => new StoreOrderRecord(
                    "order",
                    "item",
                    true,
                    StoreOrderStatus.Ordered,
                    1,
                    2,
                    100));
        }

        [Test]
        public void Stock_RejectsNegativeQuantity()
        {
            Assert.Throws<
                ArgumentOutOfRangeException>(
                () => new StoreStockRecord(
                    "item",
                    -1));
        }

        [Test]
        public void Fixture_StoresPlacement()
        {
            PlacedStoreFixtureRecord fixture =
                Fixture();

            Assert.That(
                fixture.AnchorX,
                Is.EqualTo(3));
            Assert.That(
                fixture.AnchorZ,
                Is.EqualTo(4));
            Assert.That(
                fixture.RotationQuarterTurns,
                Is.EqualTo(2));
        }

        [Test]
        public void Fixture_RejectsUnknownRotation()
        {
            Assert.Throws<
                ArgumentOutOfRangeException>(
                () => new PlacedStoreFixtureRecord(
                    "fixture",
                    "definition",
                    0,
                    0,
                    4,
                    "",
                    0));
        }

        [Test]
        public void Fixture_RejectsStockWithoutProduct()
        {
            Assert.Throws<ArgumentException>(
                () => new PlacedStoreFixtureRecord(
                    "fixture",
                    "definition",
                    0,
                    0,
                    0,
                    "",
                    1));
        }

        [Test]
        public void Fixture_WithAssignedProductReturnsCopy()
        {
            PlacedStoreFixtureRecord source =
                Fixture();

            PlacedStoreFixtureRecord result =
                source.WithAssignedProduct(
                    "product",
                    5);

            Assert.That(
                result.AssignedProductId,
                Is.EqualTo("product"));
            Assert.That(
                result.ProductQuantity,
                Is.EqualTo(5));
            Assert.That(
                source.AssignedProductId,
                Is.Empty);
        }

        [Test]
        public void State_RejectsDuplicateOrderIds()
        {
            StoreOrderRecord order = Order();

            Assert.Throws<ArgumentException>(
                () => new StoreOperationsState(
                    new SaveSlotId(0),
                    "session",
                    0,
                    1,
                    1,
                    1,
                    0,
                    0,
                    0,
                    new[] { order, order },
                    new StoreStockRecord[0],
                    new StoreStockRecord[0],
                    new PlacedStoreFixtureRecord[0]));
        }

        [Test]
        public void State_RejectsDuplicateFixtureIds()
        {
            PlacedStoreFixtureRecord fixture =
                Fixture();

            Assert.Throws<ArgumentException>(
                () => new StoreOperationsState(
                    new SaveSlotId(0),
                    "session",
                    0,
                    1,
                    1,
                    1,
                    0,
                    0,
                    0,
                    new StoreOrderRecord[0],
                    new StoreStockRecord[0],
                    new StoreStockRecord[0],
                    new[] { fixture, fixture }));
        }

        private static StoreOperationsState Empty()
        {
            return StoreOperationsState.Empty(
                new SaveSlotId(0),
                "session");
        }

        private static StoreOperationsState CreateState(
            string sessionId = "session",
            int generation = 0,
            int nextOrderSequence = 1,
            int completedSales = 0)
        {
            return new StoreOperationsState(
                new SaveSlotId(0),
                sessionId,
                generation,
                nextOrderSequence,
                1,
                1,
                completedSales,
                0,
                0,
                new StoreOrderRecord[0],
                new StoreStockRecord[0],
                new StoreStockRecord[0],
                new PlacedStoreFixtureRecord[0]);
        }

        private static StoreOrderRecord Order()
        {
            return new StoreOrderRecord(
                "order",
                "item",
                true,
                StoreOrderStatus.Ordered,
                2,
                0,
                100);
        }

        private static PlacedStoreFixtureRecord
            Fixture()
        {
            return new PlacedStoreFixtureRecord(
                "fixture",
                "definition",
                3,
                4,
                2,
                "",
                0);
        }
    }
}
