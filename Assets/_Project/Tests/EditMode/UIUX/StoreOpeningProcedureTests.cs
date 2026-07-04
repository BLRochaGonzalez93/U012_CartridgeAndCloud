using System;
using System.Collections.Generic;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.UIUX;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;
using VRMGames.CartridgeAndCloud.Domain.Persistence;
using VRMGames.CartridgeAndCloud.Application.Store;
using VRMGames.CartridgeAndCloud.Domain.Store;
using VRMGames.CartridgeAndCloud.Tests.EditMode.Store;
namespace VRMGames.CartridgeAndCloud.Tests.EditMode.UIUX
{
    public sealed class StoreOpeningProcedureTests
    {
        private StoreContentCatalog _catalog;
        private StoreOpeningProcedure _procedure;
        private IntegratedGameStateSnapshot _snapshot;

        [SetUp]
        public void SetUp()
        {
            _catalog =
                StoreOperationsTestFactory.Catalog();
            _procedure =
                new StoreOpeningProcedure(
                    _catalog);
            _snapshot =
                StoreOperationsTestFactory
                    .ActiveSession()
                    .Snapshot;
        }

        [Test]
        public void EmptyState_RequestsCheckoutOrder()
        {
            AssertStep(
                State(),
                StoreOpeningProcedureStep.OrderCheckout);
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
                            StoreOrderStatus.Ordered)
                    }),
                StoreOpeningProcedureStep.ReceiveCheckout);
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
                            StoreOrderStatus.Received)
                    },
                    furniture: new[]
                    {
                        new StoreStockRecord(
                            "checkout-counter",
                            1)
                    }),
                StoreOpeningProcedureStep.PlaceCheckout);
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
                            StoreOrderStatus.Received)
                    },
                    fixtures: new[]
                    {
                        Fixture(
                            "checkout",
                            "checkout-counter")
                    }),
                StoreOpeningProcedureStep.OrderDisplay);
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
                            StoreOrderStatus.Received),
                        FurnitureOrder(
                            "wall-shelf",
                            StoreOrderStatus.Ordered)
                    },
                    fixtures: new[]
                    {
                        Fixture(
                            "checkout",
                            "checkout-counter")
                    }),
                StoreOpeningProcedureStep.ReceiveDisplay);
        }

        [Test]
        public void DisplayReceived_RequestsPlacement()
        {
            AssertStep(
                State(
                    orders: BaseFurnitureOrders(
                        StoreOrderStatus.Received),
                    furniture: new[]
                    {
                        new StoreStockRecord(
                            "central-shelf",
                            1)
                    },
                    fixtures: new[]
                    {
                        Fixture(
                            "checkout",
                            "checkout-counter")
                    }),
                StoreOpeningProcedureStep.PlaceDisplay);
        }

        [Test]
        public void DisplayPlaced_RequestsProductOrder()
        {
            AssertStep(
                State(
                    orders: BaseFurnitureOrders(
                        StoreOrderStatus.Received),
                    fixtures: BaseFixtures()),
                StoreOpeningProcedureStep.OrderProduct);
        }

        [Test]
        public void ProductOrdered_RequestsReceive()
        {
            List<StoreOrderRecord> orders =
                new List<StoreOrderRecord>(
                    BaseFurnitureOrders(
                        StoreOrderStatus.Received));

            orders.Add(
                ProductOrder(
                    StoreOrderStatus.Ordered));

            AssertStep(
                State(
                    orders: orders,
                    fixtures: BaseFixtures()),
                StoreOpeningProcedureStep.ReceiveProduct);
        }

        [Test]
        public void ProductReceived_RequestsAssignment()
        {
            List<StoreOrderRecord> orders =
                new List<StoreOrderRecord>(
                    BaseFurnitureOrders(
                        StoreOrderStatus.Received));

            orders.Add(
                ProductOrder(
                    StoreOrderStatus.Received));

            AssertStep(
                State(
                    orders: orders,
                    products: new[]
                    {
                        new StoreStockRecord(
                            "game-neon-drift",
                            12)
                    },
                    fixtures: BaseFixtures()),
                StoreOpeningProcedureStep.AssignProduct);
        }

        [Test]
        public void AssignedDisplay_RequestsRestock()
        {
            AssertStep(
                State(
                    orders: CompleteOrders(),
                    products: new[]
                    {
                        new StoreStockRecord(
                            "game-neon-drift",
                            12)
                    },
                    fixtures: new[]
                    {
                        Fixture(
                            "checkout",
                            "checkout-counter"),
                        new PlacedStoreFixtureRecord(
                            "display",
                            "central-shelf",
                            3,
                            3,
                            0,
                            "game-neon-drift",
                            0)
                    }),
                StoreOpeningProcedureStep.RestockDisplay);
        }

        [Test]
        public void StockedDisplay_RequestsOpenStore()
        {
            AssertStep(
                ReadyState(),
                StoreOpeningProcedureStep.OpenStore);
        }

        [Test]
        public void OpenStore_RequestsCustomerSale()
        {
            _snapshot =
                StoreOperationsTestFactory.WithDayState(
                    _snapshot,
                    "Open",
                    10,
                    StoreOperationsTestFactory.Utc(1));

            AssertStep(
                ReadyState(),
                StoreOpeningProcedureStep.ServeCustomer);
        }

        [Test]
        public void CompletedSale_RequestsClose()
        {
            _snapshot =
                StoreOperationsTestFactory.WithDayState(
                    _snapshot,
                    "Open",
                    10,
                    StoreOperationsTestFactory.Utc(1));

            AssertStep(
                ReadyState(completedSales: 1),
                StoreOpeningProcedureStep.CloseDay);
        }

        [Test]
        public void ClosedDay_RequestsAutosave()
        {
            _snapshot =
                StoreOperationsTestFactory.WithDayState(
                    _snapshot,
                    "Closed",
                    300,
                    StoreOperationsTestFactory.Utc(1));

            AssertStep(
                ReadyState(completedSales: 1),
                StoreOpeningProcedureStep.Autosave,
                autosaveCompleted: false);
        }

        [Test]
        public void ClosedAndSaved_IsCompleted()
        {
            _snapshot =
                StoreOperationsTestFactory.WithDayState(
                    _snapshot,
                    "Closed",
                    300,
                    StoreOperationsTestFactory.Utc(1));

            AssertStep(
                ReadyState(completedSales: 1),
                StoreOpeningProcedureStep.Completed,
                autosaveCompleted: true);
        }

        private void AssertStep(
            StoreOperationsState state,
            StoreOpeningProcedureStep expected,
            bool autosaveCompleted = false)
        {
            StoreOpeningProcedureStatus status =
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

        private static StoreOperationsState ReadyState(
            int completedSales = 0)
        {
            return State(
                orders: CompleteOrders(),
                products: new[]
                {
                    new StoreStockRecord(
                        "game-neon-drift",
                        7)
                },
                fixtures: new[]
                {
                    Fixture(
                        "checkout",
                        "checkout-counter"),
                    new PlacedStoreFixtureRecord(
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

        private static StoreOperationsState State(
            IEnumerable<StoreOrderRecord> orders = null,
            IEnumerable<StoreStockRecord> furniture = null,
            IEnumerable<StoreStockRecord> products = null,
            IEnumerable<PlacedStoreFixtureRecord> fixtures = null,
            int completedSales = 0)
        {
            return new StoreOperationsState(
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
                    new StoreOrderRecord[0],
                furniture ??
                    new StoreStockRecord[0],
                products ??
                    new StoreStockRecord[0],
                fixtures ??
                    new PlacedStoreFixtureRecord[0]);
        }

        private static StoreOrderRecord[]
            BaseFurnitureOrders(
                StoreOrderStatus displayState)
        {
            return new[]
            {
                FurnitureOrder(
                    "checkout-counter",
                    StoreOrderStatus.Received),
                FurnitureOrder(
                    "central-shelf",
                    displayState)
            };
        }

        private static StoreOrderRecord[]
            CompleteOrders()
        {
            return new[]
            {
                FurnitureOrder(
                    "checkout-counter",
                    StoreOrderStatus.Received),
                FurnitureOrder(
                    "central-shelf",
                    StoreOrderStatus.Received),
                ProductOrder(
                    StoreOrderStatus.Received)
            };
        }

        private static PlacedStoreFixtureRecord[]
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

        private static StoreOrderRecord
            FurnitureOrder(
                string itemId,
                StoreOrderStatus state)
        {
            return new StoreOrderRecord(
                "order-" + itemId,
                itemId,
                true,
                state,
                1,
                state == StoreOrderStatus.Ordered
                    ? 0
                    : 1,
                100);
        }

        private static StoreOrderRecord
            ProductOrder(
                StoreOrderStatus state)
        {
            return new StoreOrderRecord(
                "order-product",
                "game-neon-drift",
                false,
                state,
                1,
                state == StoreOrderStatus.Ordered
                    ? 0
                    : 1,
                18000);
        }

        private static PlacedStoreFixtureRecord
            Fixture(
                string id,
                string definition)
        {
            return new PlacedStoreFixtureRecord(
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
