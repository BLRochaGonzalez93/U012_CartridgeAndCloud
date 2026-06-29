using System;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;
using VRMGames.CartridgeAndCloud.Domain.VerticalSlicePhase1;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.VerticalSlicePhase1
{
    public sealed class Phase1StateTests
    {
        [Test]
        public void Empty_UsesSlotAndSession()
        {
            Phase1StoreState state =
                Phase1StoreState.Empty(
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
            Phase1StoreState state =
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
            Phase1StoreState state =
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
            Phase1OrderRecord order =
                Order().ReceiveAll();

            Assert.That(
                order.State,
                Is.EqualTo(
                    Phase1OrderState.Received));
            Assert.That(
                order.ReceivedUnits,
                Is.EqualTo(order.OrderedUnits));
        }

        [Test]
        public void Order_RejectsZeroUnits()
        {
            Assert.Throws<
                ArgumentOutOfRangeException>(
                () => new Phase1OrderRecord(
                    "order",
                    "item",
                    true,
                    Phase1OrderState.Ordered,
                    0,
                    0,
                    100));
        }

        [Test]
        public void Order_RejectsReceivedAboveOrdered()
        {
            Assert.Throws<
                ArgumentOutOfRangeException>(
                () => new Phase1OrderRecord(
                    "order",
                    "item",
                    true,
                    Phase1OrderState.Ordered,
                    1,
                    2,
                    100));
        }

        [Test]
        public void Stock_RejectsNegativeQuantity()
        {
            Assert.Throws<
                ArgumentOutOfRangeException>(
                () => new Phase1StockRecord(
                    "item",
                    -1));
        }

        [Test]
        public void Fixture_StoresPlacement()
        {
            Phase1PlacedFixtureRecord fixture =
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
                () => new Phase1PlacedFixtureRecord(
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
                () => new Phase1PlacedFixtureRecord(
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
            Phase1PlacedFixtureRecord source =
                Fixture();

            Phase1PlacedFixtureRecord result =
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
            Phase1OrderRecord order = Order();

            Assert.Throws<ArgumentException>(
                () => new Phase1StoreState(
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
                    new Phase1StockRecord[0],
                    new Phase1StockRecord[0],
                    new Phase1PlacedFixtureRecord[0]));
        }

        [Test]
        public void State_RejectsDuplicateFixtureIds()
        {
            Phase1PlacedFixtureRecord fixture =
                Fixture();

            Assert.Throws<ArgumentException>(
                () => new Phase1StoreState(
                    new SaveSlotId(0),
                    "session",
                    0,
                    1,
                    1,
                    1,
                    0,
                    0,
                    0,
                    new Phase1OrderRecord[0],
                    new Phase1StockRecord[0],
                    new Phase1StockRecord[0],
                    new[] { fixture, fixture }));
        }

        private static Phase1StoreState Empty()
        {
            return Phase1StoreState.Empty(
                new SaveSlotId(0),
                "session");
        }

        private static Phase1StoreState CreateState(
            string sessionId = "session",
            int generation = 0,
            int nextOrderSequence = 1,
            int completedSales = 0)
        {
            return new Phase1StoreState(
                new SaveSlotId(0),
                sessionId,
                generation,
                nextOrderSequence,
                1,
                1,
                completedSales,
                0,
                0,
                new Phase1OrderRecord[0],
                new Phase1StockRecord[0],
                new Phase1StockRecord[0],
                new Phase1PlacedFixtureRecord[0]);
        }

        private static Phase1OrderRecord Order()
        {
            return new Phase1OrderRecord(
                "order",
                "item",
                true,
                Phase1OrderState.Ordered,
                2,
                0,
                100);
        }

        private static Phase1PlacedFixtureRecord
            Fixture()
        {
            return new Phase1PlacedFixtureRecord(
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
