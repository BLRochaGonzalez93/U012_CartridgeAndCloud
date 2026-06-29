using System;
using System.Collections.Generic;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.UIUX;
using VRMGames.CartridgeAndCloud.Application.VerticalSlicePhase1;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;
using VRMGames.CartridgeAndCloud.Domain.Persistence;
using VRMGames.CartridgeAndCloud.Domain.VerticalSlicePhase1;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.VerticalSlicePhase1
{
    public sealed class Phase1PlayableProcedureTests
    {
        private Phase1Catalog _catalog;
        private Phase1PlayableProcedure _procedure;
        private IntegratedGameStateSnapshot _snapshot;

        [SetUp]
        public void SetUp()
        {
            _catalog =
                Phase1TestFactory.Catalog();
            _procedure =
                new Phase1PlayableProcedure(
                    _catalog);
            _snapshot =
                Phase1TestFactory
                    .ActiveSession()
                    .Snapshot;
        }

        [Test]
        public void EmptyState_RequestsCheckoutOrder()
        {
            AssertStep(
                State(),
                Phase1ProcedureStep.OrderCheckout);
        }

        [Test]
        public void CheckoutOrdered_RequestsReceive()
        {
            AssertStep(
                State(
                    orders: new[]
                    {
                        FurnitureOrder(
                            "checkout-counter",
                            Phase1OrderState.Ordered)
                    }),
                Phase1ProcedureStep.ReceiveCheckout);
        }

        [Test]
        public void CheckoutReceived_RequestsPlacement()
        {
            AssertStep(
                State(
                    orders: new[]
                    {
                        FurnitureOrder(
                            "checkout-counter",
                            Phase1OrderState.Received)
                    },
                    furniture: new[]
                    {
                        new Phase1StockRecord(
                            "checkout-counter",
                            1)
                    }),
                Phase1ProcedureStep.PlaceCheckout);
        }

        [Test]
        public void CheckoutPlaced_RequestsDisplayOrder()
        {
            AssertStep(
                State(
                    orders: new[]
                    {
                        FurnitureOrder(
                            "checkout-counter",
                            Phase1OrderState.Received)
                    },
                    fixtures: new[]
                    {
                        Fixture(
                            "checkout",
                            "checkout-counter")
                    }),
                Phase1ProcedureStep.OrderDisplay);
        }

        [Test]
        public void WallShelfOrder_CountsAsDisplayOrder()
        {
            AssertStep(
                State(
                    orders: new[]
                    {
                        FurnitureOrder(
                            "checkout-counter",
                            Phase1OrderState.Received),
                        FurnitureOrder(
                            "wall-shelf",
                            Phase1OrderState.Ordered)
                    },
                    fixtures: new[]
                    {
                        Fixture(
                            "checkout",
                            "checkout-counter")
                    }),
                Phase1ProcedureStep.ReceiveDisplay);
        }

        [Test]
        public void DisplayReceived_RequestsPlacement()
        {
            AssertStep(
                State(
                    orders: BaseFurnitureOrders(
                        Phase1OrderState.Received),
                    furniture: new[]
                    {
                        new Phase1StockRecord(
                            "central-shelf",
                            1)
                    },
                    fixtures: new[]
                    {
                        Fixture(
                            "checkout",
                            "checkout-counter")
                    }),
                Phase1ProcedureStep.PlaceDisplay);
        }

        [Test]
        public void DisplayPlaced_RequestsProductOrder()
        {
            AssertStep(
                State(
                    orders: BaseFurnitureOrders(
                        Phase1OrderState.Received),
                    fixtures: BaseFixtures()),
                Phase1ProcedureStep.OrderProduct);
        }

        [Test]
        public void ProductOrdered_RequestsReceive()
        {
            List<Phase1OrderRecord> orders =
                new List<Phase1OrderRecord>(
                    BaseFurnitureOrders(
                        Phase1OrderState.Received));

            orders.Add(
                ProductOrder(
                    Phase1OrderState.Ordered));

            AssertStep(
                State(
                    orders: orders,
                    fixtures: BaseFixtures()),
                Phase1ProcedureStep.ReceiveProduct);
        }

        [Test]
        public void ProductReceived_RequestsAssignment()
        {
            List<Phase1OrderRecord> orders =
                new List<Phase1OrderRecord>(
                    BaseFurnitureOrders(
                        Phase1OrderState.Received));

            orders.Add(
                ProductOrder(
                    Phase1OrderState.Received));

            AssertStep(
                State(
                    orders: orders,
                    products: new[]
                    {
                        new Phase1StockRecord(
                            "game-neon-drift",
                            12)
                    },
                    fixtures: BaseFixtures()),
                Phase1ProcedureStep.AssignProduct);
        }

        [Test]
        public void AssignedDisplay_RequestsRestock()
        {
            AssertStep(
                State(
                    orders: CompleteOrders(),
                    products: new[]
                    {
                        new Phase1StockRecord(
                            "game-neon-drift",
                            12)
                    },
                    fixtures: new[]
                    {
                        Fixture(
                            "checkout",
                            "checkout-counter"),
                        new Phase1PlacedFixtureRecord(
                            "display",
                            "central-shelf",
                            3,
                            3,
                            0,
                            "game-neon-drift",
                            0)
                    }),
                Phase1ProcedureStep.RestockDisplay);
        }

        [Test]
        public void StockedDisplay_RequestsOpenStore()
        {
            AssertStep(
                ReadyState(),
                Phase1ProcedureStep.OpenStore);
        }

        [Test]
        public void OpenStore_RequestsCustomerSale()
        {
            _snapshot =
                Phase1TestFactory.WithDayState(
                    _snapshot,
                    "Open",
                    10,
                    Phase1TestFactory.Utc(1));

            AssertStep(
                ReadyState(),
                Phase1ProcedureStep.ServeCustomer);
        }

        [Test]
        public void CompletedSale_RequestsClose()
        {
            _snapshot =
                Phase1TestFactory.WithDayState(
                    _snapshot,
                    "Open",
                    10,
                    Phase1TestFactory.Utc(1));

            AssertStep(
                ReadyState(completedSales: 1),
                Phase1ProcedureStep.CloseDay);
        }

        [Test]
        public void ClosedDay_RequestsAutosave()
        {
            _snapshot =
                Phase1TestFactory.WithDayState(
                    _snapshot,
                    "Closed",
                    300,
                    Phase1TestFactory.Utc(1));

            AssertStep(
                ReadyState(completedSales: 1),
                Phase1ProcedureStep.Autosave,
                autosaveCompleted: false);
        }

        [Test]
        public void ClosedAndSaved_IsCompleted()
        {
            _snapshot =
                Phase1TestFactory.WithDayState(
                    _snapshot,
                    "Closed",
                    300,
                    Phase1TestFactory.Utc(1));

            AssertStep(
                ReadyState(completedSales: 1),
                Phase1ProcedureStep.Completed,
                autosaveCompleted: true);
        }

        private void AssertStep(
            Phase1StoreState state,
            Phase1ProcedureStep expected,
            bool autosaveCompleted = false)
        {
            Phase1ProcedureStatus status =
                _procedure.Evaluate(
                    state,
                    _snapshot,
                    autosaveCompleted);

            Assert.That(
                status.Step,
                Is.EqualTo(expected));
            Assert.That(
                status.Title,
                Is.Not.Empty);
            Assert.That(
                status.Instruction,
                Is.Not.Empty);
        }

        private static Phase1StoreState ReadyState(
            int completedSales = 0)
        {
            return State(
                orders: CompleteOrders(),
                products: new[]
                {
                    new Phase1StockRecord(
                        "game-neon-drift",
                        7)
                },
                fixtures: new[]
                {
                    Fixture(
                        "checkout",
                        "checkout-counter"),
                    new Phase1PlacedFixtureRecord(
                        "display",
                        "central-shelf",
                        3,
                        3,
                        0,
                        "game-neon-drift",
                        5)
                },
                completedSales: completedSales);
        }

        private static Phase1StoreState State(
            IEnumerable<Phase1OrderRecord> orders = null,
            IEnumerable<Phase1StockRecord> furniture = null,
            IEnumerable<Phase1StockRecord> products = null,
            IEnumerable<Phase1PlacedFixtureRecord> fixtures = null,
            int completedSales = 0)
        {
            return new Phase1StoreState(
                new SaveSlotId(0),
                "session",
                0,
                10,
                10,
                10,
                completedSales,
                0,
                0,
                orders ??
                    new Phase1OrderRecord[0],
                furniture ??
                    new Phase1StockRecord[0],
                products ??
                    new Phase1StockRecord[0],
                fixtures ??
                    new Phase1PlacedFixtureRecord[0]);
        }

        private static Phase1OrderRecord[]
            BaseFurnitureOrders(
                Phase1OrderState displayState)
        {
            return new[]
            {
                FurnitureOrder(
                    "checkout-counter",
                    Phase1OrderState.Received),
                FurnitureOrder(
                    "central-shelf",
                    displayState)
            };
        }

        private static Phase1OrderRecord[]
            CompleteOrders()
        {
            return new[]
            {
                FurnitureOrder(
                    "checkout-counter",
                    Phase1OrderState.Received),
                FurnitureOrder(
                    "central-shelf",
                    Phase1OrderState.Received),
                ProductOrder(
                    Phase1OrderState.Received)
            };
        }

        private static Phase1PlacedFixtureRecord[]
            BaseFixtures()
        {
            return new[]
            {
                Fixture(
                    "checkout",
                    "checkout-counter"),
                Fixture(
                    "display",
                    "central-shelf")
            };
        }

        private static Phase1OrderRecord
            FurnitureOrder(
                string itemId,
                Phase1OrderState state)
        {
            return new Phase1OrderRecord(
                "order-" + itemId,
                itemId,
                true,
                state,
                1,
                state == Phase1OrderState.Ordered
                    ? 0
                    : 1,
                100);
        }

        private static Phase1OrderRecord
            ProductOrder(
                Phase1OrderState state)
        {
            return new Phase1OrderRecord(
                "order-product",
                "game-neon-drift",
                false,
                state,
                1,
                state == Phase1OrderState.Ordered
                    ? 0
                    : 1,
                18000);
        }

        private static Phase1PlacedFixtureRecord
            Fixture(
                string id,
                string definition)
        {
            return new Phase1PlacedFixtureRecord(
                id,
                definition,
                1,
                1,
                0,
                "",
                0);
        }
    }
}
