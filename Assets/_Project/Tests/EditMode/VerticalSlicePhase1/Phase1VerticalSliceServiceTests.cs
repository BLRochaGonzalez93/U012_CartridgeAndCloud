using System;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.UIUX;
using VRMGames.CartridgeAndCloud.Application.VerticalSlicePhase1;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;
using VRMGames.CartridgeAndCloud.Domain.Persistence;
using VRMGames.CartridgeAndCloud.Domain.VerticalSlicePhase1;
using VRMGames.CartridgeAndCloud.Infrastructure.VerticalSlicePhase1;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.VerticalSlicePhase1
{
    public sealed class Phase1VerticalSliceServiceTests
    {
        private string _directory;
        private Phase1Catalog _catalog;
        private JsonPhase1StateRepository
            _repository;
        private ActiveGameSessionService _active;
        private Phase1TestFactory.FixedClock
            _clock;
        private Phase1VerticalSliceService
            _service;

        [SetUp]
        public void SetUp()
        {
            _directory =
                Phase1TestFactory.TempDirectory();
            _catalog =
                Phase1TestFactory.Catalog();
            _repository =
                new JsonPhase1StateRepository(
                    _directory);
            _active =
                Phase1TestFactory.ActiveSession();
            _clock =
                new Phase1TestFactory.FixedClock(
                    Phase1TestFactory.Utc());

            _service =
                new Phase1VerticalSliceService(
                    _catalog,
                    _repository,
                    _active,
                    _clock);

            _service.InitializeForActiveSlot();
        }

        [TearDown]
        public void TearDown()
        {
            Phase1TestFactory.DeleteDirectory(
                _directory);
        }

        [Test]
        public void Initialize_UsesActiveSlot()
        {
            Assert.That(
                _service.State.SlotId,
                Is.EqualTo(new SaveSlotId(0)));
        }

        [Test]
        public void Initialize_BindsCurrentSession()
        {
            Assert.That(
                _service.State.SessionId,
                Is.EqualTo(
                    _active.Snapshot
                        .SessionId.Value));
        }

        [Test]
        public void Initialize_DifferentSessionResetsSidecar()
        {
            _service.OrderFurniture(
                "central-shelf",
                1);
            _service.SaveCheckpoint();

            IntegratedGameStateSnapshot replacement =
                new DefaultIntegratedGameStateFactory(
                    "EUR",
                    100000,
                    300)
                    .Create(
                        new SaveSlotId(0),
                        Phase1TestFactory.Utc(1));

            _active.Activate(
                replacement.SlotId,
                replacement);

            _service.InitializeForActiveSlot();

            Assert.That(
                _service.State.Orders.Count,
                Is.EqualTo(0));
            Assert.That(
                _service.State.SessionId,
                Is.EqualTo(
                    replacement.SessionId.Value));
        }

        [Test]
        public void OrderFurniture_CreatesOrder()
        {
            Phase1OperationResult result =
                _service.OrderFurniture(
                    "central-shelf",
                    1);

            Assert.That(result.Succeeded, Is.True);
            Assert.That(
                _service.State.Orders.Count,
                Is.EqualTo(1));
        }

        [Test]
        public void OrderFurniture_ProjectsSupplierOrder()
        {
            _service.OrderFurniture(
                "central-shelf",
                1);

            Assert.That(
                _active.Snapshot
                    .SupplierOrders.Count,
                Is.EqualTo(1));
            Assert.That(
                _active.Snapshot
                    .SupplierOrders[0]
                    .ProductId,
                Is.EqualTo("central-shelf"));
        }

        [Test]
        public void OrderFurniture_NonPurchasableFails()
        {
            Phase1OperationResult result =
                _service.OrderFurniture(
                    "backroom-storage",
                    1);

            Assert.That(
                result.Status,
                Is.EqualTo(
                    Phase1OperationStatus
                        .InvalidState));
        }

        [Test]
        public void OrderFurniture_MissingFails()
        {
            Assert.That(
                _service.OrderFurniture(
                    "missing",
                    1).Status,
                Is.EqualTo(
                    Phase1OperationStatus.NotFound));
        }

        [Test]
        public void OrderFurniture_ZeroQuantityFails()
        {
            Assert.That(
                _service.OrderFurniture(
                    "central-shelf",
                    0).Status,
                Is.EqualTo(
                    Phase1OperationStatus
                        .InvalidState));
        }

        [Test]
        public void ReceiveFurniture_DeductsCash()
        {
            string orderId =
                _service.OrderFurniture(
                    "central-shelf",
                    1).Detail;

            _service.ReceiveOrder(orderId);

            Assert.That(
                _active.Snapshot.CashCents,
                Is.EqualTo(74000));
        }

        [Test]
        public void ReceiveFurniture_AddsExpenseLedger()
        {
            string orderId =
                _service.OrderFurniture(
                    "central-shelf",
                    1).Detail;

            _service.ReceiveOrder(orderId);

            Assert.That(
                _active.Snapshot
                    .LedgerEntries.Count,
                Is.EqualTo(1));
            Assert.That(
                _active.Snapshot
                    .LedgerEntries[0]
                    .PostingType,
                Is.EqualTo(
                    "SupplierReceivingCost"));
        }

        [Test]
        public void ReceiveFurniture_AddsWarehouseStock()
        {
            string orderId =
                _service.OrderFurniture(
                    "central-shelf",
                    2).Detail;

            _service.ReceiveOrder(orderId);

            Assert.That(
                _service
                    .GetFurnitureWarehouseQuantity(
                        "central-shelf"),
                Is.EqualTo(2));
        }

        [Test]
        public void ReceiveOrder_TwiceFails()
        {
            string orderId =
                _service.OrderFurniture(
                    "central-shelf",
                    1).Detail;

            _service.ReceiveOrder(orderId);

            Assert.That(
                _service.ReceiveOrder(orderId)
                    .Status,
                Is.EqualTo(
                    Phase1OperationStatus
                        .InvalidState));
        }

        [Test]
        public void ReceiveOrder_InsufficientCashFails()
        {
            ActiveGameSessionService poor =
                Phase1TestFactory
                    .ActiveSession(100);

            Phase1VerticalSliceService service =
                new Phase1VerticalSliceService(
                    _catalog,
                    _repository,
                    poor,
                    _clock);
            service.InitializeForActiveSlot();

            string orderId =
                service.OrderFurniture(
                    "central-shelf",
                    1).Detail;

            Assert.That(
                service.ReceiveOrder(orderId)
                    .Status,
                Is.EqualTo(
                    Phase1OperationStatus
                        .InsufficientCash));
            Assert.That(
                poor.Snapshot.CashCents,
                Is.EqualTo(100));
        }

        [Test]
        public void PlaceFurniture_RequiresStock()
        {
            Assert.That(
                _service
                    .ConfirmFurniturePlacement(
                        "central-shelf",
                        "fixture-1",
                        1,
                        1,
                        0)
                    .Status,
                Is.EqualTo(
                    Phase1OperationStatus
                        .InsufficientStock));
        }

        [Test]
        public void PlaceFurniture_ConsumesWarehouseStock()
        {
            ReceiveFurniture(
                "central-shelf",
                1);

            _service.ConfirmFurniturePlacement(
                "central-shelf",
                "fixture-1",
                1,
                1,
                0);

            Assert.That(
                _service
                    .GetFurnitureWarehouseQuantity(
                        "central-shelf"),
                Is.EqualTo(0));
        }

        [Test]
        public void PlaceDisplay_CreatesDisplayRecord()
        {
            ReceiveFurniture(
                "central-shelf",
                1);

            _service.ConfirmFurniturePlacement(
                "central-shelf",
                "fixture-1",
                1,
                1,
                0);

            Assert.That(
                _active.Snapshot.Displays.Count,
                Is.EqualTo(1));
            Assert.That(
                _active.Snapshot
                    .Displays[0].DisplayId,
                Is.EqualTo("fixture-1"));
        }

        [Test]
        public void PlaceDisplay_CreatesInventoryContainer()
        {
            ReceiveFurniture(
                "central-shelf",
                1);

            _service.ConfirmFurniturePlacement(
                "central-shelf",
                "fixture-1",
                1,
                1,
                0);

            Assert.That(
                HasContainer(
                    "phase1-display-fixture-1"),
                Is.True);
        }

        [Test]
        public void PlaceFurniture_DuplicateInstanceFails()
        {
            ReceiveFurniture(
                "central-shelf",
                2);

            _service.ConfirmFurniturePlacement(
                "central-shelf",
                "fixture-1",
                1,
                1,
                0);

            Assert.That(
                _service
                    .ConfirmFurniturePlacement(
                        "central-shelf",
                        "fixture-1",
                        4,
                        4,
                        0)
                    .Status,
                Is.EqualTo(
                    Phase1OperationStatus.Duplicate));
        }

        [Test]
        public void AssignProduct_Succeeds()
        {
            PlaceDisplay();

            Phase1OperationResult result =
                _service.AssignProduct(
                    "fixture-1",
                    "game-neon-drift");

            Assert.That(result.Succeeded, Is.True);
            Assert.That(
                _service.State.Fixtures[0]
                    .AssignedProductId,
                Is.EqualTo("game-neon-drift"));
        }

        [Test]
        public void AssignProduct_MissingFixtureFails()
        {
            Assert.That(
                _service.AssignProduct(
                    "missing",
                    "game-neon-drift").Status,
                Is.EqualTo(
                    Phase1OperationStatus.NotFound));
        }

        [Test]
        public void AssignProduct_CheckoutFails()
        {
            ReceiveFurniture(
                "checkout-counter",
                1);
            _service.ConfirmFurniturePlacement(
                "checkout-counter",
                "checkout",
                1,
                1,
                0);

            Assert.That(
                _service.AssignProduct(
                    "checkout",
                    "game-neon-drift").Status,
                Is.EqualTo(
                    Phase1OperationStatus
                        .InvalidState));
        }

        [Test]
        public void OrderProduct_CreatesCaseOrder()
        {
            Phase1OperationResult result =
                _service.OrderProduct(
                    "game-neon-drift",
                    1);

            Assert.That(result.Succeeded, Is.True);
            Assert.That(
                _service.State.Orders[0]
                    .UnitCostCents,
                Is.EqualTo(18000));
        }

        [Test]
        public void ReceiveProduct_AddsCaseUnits()
        {
            string orderId =
                _service.OrderProduct(
                    "game-neon-drift",
                    1).Detail;

            _service.ReceiveOrder(orderId);

            Assert.That(
                _service
                    .GetProductWarehouseQuantity(
                        "game-neon-drift"),
                Is.EqualTo(12));
            Assert.That(
                ProductQuantity(
                    "backroom-inventory",
                    "game-neon-drift"),
                Is.EqualTo(12));
        }

        [Test]
        public void Restock_RequiresAssignment()
        {
            PlaceDisplay();
            ReceiveProduct();

            Assert.That(
                _service.RestockDisplay(
                    "fixture-1",
                    1).Status,
                Is.EqualTo(
                    Phase1OperationStatus
                        .InvalidState));
        }

        [Test]
        public void Restock_TransfersStock()
        {
            PrepareStockedDisplay(5);

            Assert.That(
                _service.State.Fixtures[0]
                    .ProductQuantity,
                Is.EqualTo(5));
            Assert.That(
                _service
                    .GetProductWarehouseQuantity(
                        "game-neon-drift"),
                Is.EqualTo(7));
            Assert.That(
                ProductQuantity(
                    "phase1-display-fixture-1",
                    "game-neon-drift"),
                Is.EqualTo(5));
        }

        [Test]
        public void Restock_RejectsCapacityOverflow()
        {
            PlaceDisplay();
            ReceiveProduct();
            _service.AssignProduct(
                "fixture-1",
                "game-neon-drift");

            Assert.That(
                _service.RestockDisplay(
                    "fixture-1",
                    33).Status,
                Is.EqualTo(
                    Phase1OperationStatus
                        .CapacityExceeded));
        }

        [Test]
        public void Purchase_RequiresOpenStore()
        {
            PrepareCheckoutAndStockedDisplay();

            Assert.That(
                _service
                    .ProcessNextCustomerPurchase()
                    .Status,
                Is.EqualTo(
                    Phase1OperationStatus
                        .StoreMustBeOpen));
        }

        [Test]
        public void Purchase_RequiresCheckout()
        {
            PrepareStockedDisplay(5);
            OpenStore();

            Assert.That(
                _service
                    .ProcessNextCustomerPurchase()
                    .Status,
                Is.EqualTo(
                    Phase1OperationStatus
                        .CheckoutRequired));
        }

        [Test]
        public void Purchase_CompletesSale()
        {
            PrepareCheckoutAndStockedDisplay();
            OpenStore();

            Phase1OperationResult result =
                _service
                    .ProcessNextCustomerPurchase();

            Assert.That(result.Succeeded, Is.True);
            Assert.That(
                _service.State.CompletedSales,
                Is.EqualTo(1));
        }

        [Test]
        public void Purchase_IncreasesCash()
        {
            PrepareCheckoutAndStockedDisplay();
            OpenStore();

            long before =
                _active.Snapshot.CashCents;

            _service
                .ProcessNextCustomerPurchase();

            Assert.That(
                _active.Snapshot.CashCents,
                Is.EqualTo(before + 2999));
        }

        [Test]
        public void Purchase_ReducesDisplayStock()
        {
            PrepareCheckoutAndStockedDisplay();
            OpenStore();

            _service
                .ProcessNextCustomerPurchase();

            Assert.That(
                _service.State.Fixtures[1]
                    .ProductQuantity,
                Is.EqualTo(4));
        }

        [Test]
        public void Purchase_AppendsRevenueLedger()
        {
            PrepareCheckoutAndStockedDisplay();
            OpenStore();

            int before =
                _active.Snapshot
                    .LedgerEntries.Count;

            _service
                .ProcessNextCustomerPurchase();

            Assert.That(
                _active.Snapshot
                    .LedgerEntries.Count,
                Is.EqualTo(before + 1));
            Assert.That(
                _active.Snapshot
                    .LedgerEntries[
                        _active.Snapshot
                            .LedgerEntries.Count - 1]
                    .PostingType,
                Is.EqualTo("CheckoutRevenue"));
        }


        [Test]
        public void Purchase_PersistsCustomerAndCart()
        {
            PrepareCheckoutAndStockedDisplay();
            OpenStore();

            _service
                .ProcessNextCustomerPurchase();

            Assert.That(
                _active.Snapshot.Customers.Count,
                Is.EqualTo(1));
            Assert.That(
                _active.Snapshot.Customers[0]
                    .State,
                Is.EqualTo("Despawned"));
            Assert.That(
                _active.Snapshot
                    .ShoppingSessions.Count,
                Is.EqualTo(1));
            Assert.That(
                _active.Snapshot
                    .ShoppingSessions[0].State,
                Is.EqualTo("CheckedOut"));
        }

        [Test]
        public void Purchase_PersistsConsumedReservation()
        {
            PrepareCheckoutAndStockedDisplay();
            OpenStore();

            _service
                .ProcessNextCustomerPurchase();

            Assert.That(
                _active.Snapshot
                    .Reservations.Count,
                Is.EqualTo(1));
            Assert.That(
                _active.Snapshot
                    .Reservations[0].State,
                Is.EqualTo("Consumed"));
        }

        [Test]
        public void Purchase_PersistsCompletedTransaction()
        {
            PrepareCheckoutAndStockedDisplay();
            OpenStore();

            _service
                .ProcessNextCustomerPurchase();

            Assert.That(
                _active.Snapshot
                    .Transactions.Count,
                Is.EqualTo(1));
            Assert.That(
                _active.Snapshot
                    .Transactions[0].State,
                Is.EqualTo("Completed"));
            Assert.That(
                _active.Snapshot
                    .QueueEntries.Count,
                Is.EqualTo(0));
        }

        [Test]
        public void Purchase_EmptyDisplayRaisesFrustratedFeedback()
        {
            ReceiveFurniture(
                "checkout-counter",
                1);
            _service.ConfirmFurniturePlacement(
                "checkout-counter",
                "checkout",
                1,
                1,
                0);
            OpenStore();

            Phase1FeedbackKind? kind = null;
            _service.FeedbackRaised +=
                feedback => kind = feedback.Kind;

            _service
                .ProcessNextCustomerPurchase();

            Assert.That(
                kind,
                Is.EqualTo(
                    Phase1FeedbackKind
                        .CustomerFrustrated));
        }

        [Test]
        public void RemoveDisplay_ReturnsFurnitureAndProducts()
        {
            PrepareStockedDisplay(5);

            Phase1OperationResult result =
                _service
                    .RemoveFurniturePlacement(
                        "fixture-1");

            Assert.That(result.Succeeded, Is.True);
            Assert.That(
                _service.State.Fixtures.Count,
                Is.EqualTo(0));
            Assert.That(
                _service
                    .GetFurnitureWarehouseQuantity(
                        "central-shelf"),
                Is.EqualTo(1));
            Assert.That(
                _service
                    .GetProductWarehouseQuantity(
                        "game-neon-drift"),
                Is.EqualTo(12));
        }

        [Test]
        public void RemoveDisplay_CleansIntegratedDisplay()
        {
            PrepareStockedDisplay(5);

            _service
                .RemoveFurniturePlacement(
                    "fixture-1");

            Assert.That(
                _active.Snapshot.Displays.Count,
                Is.EqualTo(0));
            Assert.That(
                HasContainer(
                    "phase1-display-fixture-1"),
                Is.False);
        }


        [Test]
        public void RemoveDisplay_AfterSaleRemovesHistoricalReservation()
        {
            PrepareCheckoutAndStockedDisplay();
            OpenStore();
            _service
                .ProcessNextCustomerPurchase();

            Phase1OperationResult result =
                _service
                    .RemoveFurniturePlacement(
                        "fixture-1");

            Assert.That(result.Succeeded, Is.True);
            Assert.That(
                _active.Snapshot
                    .Reservations.Count,
                Is.EqualTo(0));
            Assert.That(
                _active.Snapshot
                    .Transactions.Count,
                Is.EqualTo(1));
        }

        [Test]
        public void SaveCheckpoint_RoundTripsState()
        {
            _service.OrderFurniture(
                "central-shelf",
                1);
            _service.SaveCheckpoint();

            Phase1StoreState loaded =
                _repository.Load(
                    new SaveSlotId(0));

            Assert.That(loaded, Is.Not.Null);
            Assert.That(
                loaded.Orders.Count,
                Is.EqualTo(1));
        }

        private void ReceiveFurniture(
            string definitionId,
            int quantity)
        {
            string orderId =
                _service.OrderFurniture(
                    definitionId,
                    quantity).Detail;
            _service.ReceiveOrder(orderId);
        }

        private void ReceiveProduct()
        {
            string orderId =
                _service.OrderProduct(
                    "game-neon-drift",
                    1).Detail;
            _service.ReceiveOrder(orderId);
        }

        private void PlaceDisplay()
        {
            ReceiveFurniture(
                "central-shelf",
                1);
            _service.ConfirmFurniturePlacement(
                "central-shelf",
                "fixture-1",
                2,
                2,
                0);
        }

        private void PrepareStockedDisplay(
            int quantity)
        {
            PlaceDisplay();
            ReceiveProduct();
            _service.AssignProduct(
                "fixture-1",
                "game-neon-drift");
            _service.RestockDisplay(
                "fixture-1",
                quantity);
        }

        private void PrepareCheckoutAndStockedDisplay()
        {
            ReceiveFurniture(
                "checkout-counter",
                1);
            _service.ConfirmFurniturePlacement(
                "checkout-counter",
                "checkout",
                1,
                1,
                0);
            PrepareStockedDisplay(5);
        }

        private void OpenStore()
        {
            _clock.UtcNow =
                _clock.UtcNow.AddMinutes(1);

            _active.Replace(
                Phase1TestFactory.WithDayState(
                    _active.Snapshot,
                    "Open",
                    10,
                    _clock.UtcNow));
        }

        private bool HasContainer(
            string containerId)
        {
            foreach (InventoryContainerSaveRecord
                     inventory in
                     _active.Snapshot.Inventories)
            {
                if (inventory.ContainerId ==
                    containerId)
                {
                    return true;
                }
            }

            return false;
        }

        private int ProductQuantity(
            string containerId,
            string productId)
        {
            foreach (InventoryContainerSaveRecord
                     inventory in
                     _active.Snapshot.Inventories)
            {
                if (inventory.ContainerId !=
                    containerId)
                {
                    continue;
                }

                foreach (ProductQuantitySaveRecord
                         product in inventory.Products)
                {
                    if (product.ProductId ==
                        productId)
                    {
                        return product.Quantity;
                    }
                }
            }

            return 0;
        }
    }
}
