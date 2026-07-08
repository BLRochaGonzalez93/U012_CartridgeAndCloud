using System;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.Store;
using VRMGames.CartridgeAndCloud.Application.UIUX;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;
using VRMGames.CartridgeAndCloud.Domain.Persistence;
using VRMGames.CartridgeAndCloud.Domain.Store;
using VRMGames.CartridgeAndCloud.Infrastructure.Persistence;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Store
{
    public sealed class W4DisplayCharacterizationTests
    {
        private string _directory;
        private StoreContentCatalog _catalog;
        private JsonStoreOperationsStateRepository _repository;
        private ActiveGameSessionService _active;
        private StoreOperationsTestFactory.FixedClock _clock;
        private StoreOperationsFacade _service;

        [SetUp]
        public void SetUp()
        {
            _directory = StoreOperationsTestFactory.TempDirectory();
            _catalog = StoreOperationsTestFactory.Catalog();
            _repository =
                new JsonStoreOperationsStateRepository(_directory);
            _active = StoreOperationsTestFactory.ActiveSession();
            _clock = new StoreOperationsTestFactory.FixedClock(
                StoreOperationsTestFactory.Utc());
            _service = new StoreOperationsFacade(
                _catalog,
                _repository,
                _active,
                _clock);
            _service.InitializeForActiveSlot();
        }

        [TearDown]
        public void TearDown()
        {
            StoreOperationsTestFactory.DeleteDirectory(_directory);
        }

        [Test]
        public void CHAR_DSP_001_DisplayRemainsSingleProductUntilExplicitClear()
        {
            PlaceDisplay();

            Assert.That(
                _service.AssignProduct(
                    "fixture-1",
                    "game-neon-drift").Succeeded,
                Is.True);

            StoreOperationResult replacement =
                _service.AssignProduct(
                    "fixture-1",
                    "console-vertex-one");

            Assert.That(replacement.Succeeded, Is.False);
            Assert.That(
                _service.State.Fixtures[0].AssignedProductId,
                Is.EqualTo("game-neon-drift"));

            Assert.That(
                _service.ClearDisplayAssignment(
                    "fixture-1").Succeeded,
                Is.True);
            Assert.That(
                _service.AssignProduct(
                    "fixture-1",
                    "console-vertex-one").Succeeded,
                Is.True);
            Assert.That(
                _service.State.Fixtures[0].AssignedProductId,
                Is.EqualTo("console-vertex-one"));
        }

        [Test]
        public void CHAR_DSP_002_RestockUsesMinimumOfRequestStockAndCapacity()
        {
            PrepareAssignedDisplay();
            ReceiveProduct("game-neon-drift", 1);

            StoreOperationResult result =
                _service.RestockDisplay(
                    "fixture-1",
                    99);

            Assert.That(result.Succeeded, Is.True);
            Assert.That(
                _service.State.Fixtures[0].ProductQuantity,
                Is.EqualTo(12));
            Assert.That(
                _service.GetProductWarehouseQuantity(
                    "game-neon-drift"),
                Is.Zero);
            Assert.That(
                DisplayQuantity(
                    "fixture-1",
                    "game-neon-drift"),
                Is.EqualTo(12));
        }

        [Test]
        public void CHAR_DSP_003_ReturnQuantityConservesCombinedStock()
        {
            PrepareStockedDisplay(5);
            int before = CombinedProductUnits();

            StoreOperationResult result =
                _service.ReturnDisplayStock(
                    "fixture-1",
                    2);

            Assert.That(result.Succeeded, Is.True);
            Assert.That(
                _service.State.Fixtures[0].ProductQuantity,
                Is.EqualTo(3));
            Assert.That(
                _service.GetProductWarehouseQuantity(
                    "game-neon-drift"),
                Is.EqualTo(9));
            Assert.That(CombinedProductUnits(), Is.EqualTo(before));
        }

        [Test]
        public void CHAR_DSP_004_ReturnAllPreservesAssignmentAndIsIdempotent()
        {
            PrepareStockedDisplay(5);

            StoreOperationResult first =
                _service.ReturnAllDisplayStock(
                    "fixture-1");
            StoreOperationResult second =
                _service.ReturnAllDisplayStock(
                    "fixture-1");

            Assert.That(first.Succeeded, Is.True);
            Assert.That(second.Succeeded, Is.True);
            Assert.That(
                _service.State.Fixtures[0].ProductQuantity,
                Is.Zero);
            Assert.That(
                _service.State.Fixtures[0].AssignedProductId,
                Is.EqualTo("game-neon-drift"));
            Assert.That(
                _service.GetProductWarehouseQuantity(
                    "game-neon-drift"),
                Is.EqualTo(12));
        }

        [Test]
        public void CHAR_DSP_005_ClearAndReturnAllRespectStockAndReservations()
        {
            PrepareStockedDisplay(5);

            Assert.That(
                _service.ClearDisplayAssignment(
                    "fixture-1").Succeeded,
                Is.False);

            IntegratedGameStateSnapshot withoutReservation =
                _active.Snapshot;
            _active.Replace(
                WithActiveReservation(
                    withoutReservation));

            Assert.That(
                _service.ReturnAllAndClearDisplay(
                    "fixture-1").Succeeded,
                Is.False);
            Assert.That(
                _service.State.Fixtures[0].ProductQuantity,
                Is.EqualTo(5));

            _active.Replace(withoutReservation);

            Assert.That(
                _service.ReturnAllAndClearDisplay(
                    "fixture-1").Succeeded,
                Is.True);
            Assert.That(
                _service.State.Fixtures[0].ProductQuantity,
                Is.Zero);
            Assert.That(
                _service.State.Fixtures[0].AssignedProductId,
                Is.Empty);
            Assert.That(
                _service.GetProductWarehouseQuantity(
                    "game-neon-drift"),
                Is.EqualTo(12));
        }

        [Test]
        public void ActiveReservationBlocksDisplayRemoval()
        {
            PrepareStockedDisplay(5);

            IntegratedGameStateSnapshot withoutReservation =
                _active.Snapshot;
            _active.Replace(
                WithActiveReservation(
                    withoutReservation));

            StoreOperationResult result =
                _service.RemoveFurniturePlacement(
                    "fixture-1");

            Assert.That(result.Succeeded, Is.False);
            Assert.That(
                _service.State.Fixtures.Count,
                Is.EqualTo(1));
            Assert.That(
                _service.State.Fixtures[0].ProductQuantity,
                Is.EqualTo(5));
            Assert.That(CombinedProductUnits(), Is.EqualTo(12));
        }

        [Test]
        public void CHAR_DSP_006_SaveLoadPreservesAssignedEmptyDisplay()
        {
            PrepareStockedDisplay(5);
            _service.ReturnAllDisplayStock("fixture-1");
            _service.SaveCheckpoint();

            StoreOperationsState loaded =
                _repository.Load(new SaveSlotId(0));

            Assert.That(loaded, Is.Not.Null);
            Assert.That(loaded.Fixtures.Count, Is.EqualTo(1));
            Assert.That(
                loaded.Fixtures[0].AssignedProductId,
                Is.EqualTo("game-neon-drift"));
            Assert.That(
                loaded.Fixtures[0].ProductQuantity,
                Is.Zero);
            Assert.That(
                DisplayQuantity(
                    "fixture-1",
                    "game-neon-drift"),
                Is.Zero);
            Assert.That(
                _active.Snapshot.Displays[0].AssignedProductId,
                Is.EqualTo("game-neon-drift"));
        }

        private void PlaceDisplay()
        {
            ReceiveFurniture("central-shelf", 1);
            Assert.That(
                _service.ConfirmFurniturePlacement(
                    "central-shelf",
                    "fixture-1",
                    2,
                    2,
                    0).Succeeded,
                Is.True);
        }

        private void PrepareAssignedDisplay()
        {
            PlaceDisplay();
            Assert.That(
                _service.AssignProduct(
                    "fixture-1",
                    "game-neon-drift").Succeeded,
                Is.True);
        }

        private void PrepareStockedDisplay(int quantity)
        {
            PrepareAssignedDisplay();
            ReceiveProduct("game-neon-drift", 1);
            Assert.That(
                _service.RestockDisplay(
                    "fixture-1",
                    quantity).Succeeded,
                Is.True);
        }

        private void ReceiveFurniture(
            string definitionId,
            int quantity)
        {
            string orderId =
                _service.OrderFurniture(
                    definitionId,
                    quantity).Detail;
            Assert.That(
                _service.DispatchAndComplete(orderId).Succeeded,
                Is.True);
        }

        private void ReceiveProduct(
            string productId,
            int cases)
        {
            string orderId =
                _service.OrderProduct(
                    productId,
                    cases).Detail;
            Assert.That(
                _service.DispatchAndComplete(orderId).Succeeded,
                Is.True);
        }

        private int CombinedProductUnits()
        {
            return _service.GetProductWarehouseQuantity(
                       "game-neon-drift") +
                   _service.State.Fixtures[0]
                       .ProductQuantity;
        }

        private int DisplayQuantity(
            string displayId,
            string productId)
        {
            string containerId =
                "phase1-display-" + displayId;

            foreach (InventoryContainerSaveRecord inventory
                     in _active.Snapshot.Inventories)
            {
                if (!string.Equals(
                        inventory.ContainerId,
                        containerId,
                        StringComparison.Ordinal))
                {
                    continue;
                }

                foreach (ProductQuantitySaveRecord product
                         in inventory.Products)
                {
                    if (string.Equals(
                            product.ProductId,
                            productId,
                            StringComparison.Ordinal))
                    {
                        return product.Quantity;
                    }
                }
            }

            return 0;
        }

        private IntegratedGameStateSnapshot
            WithActiveReservation(
                IntegratedGameStateSnapshot source)
        {
            return new IntegratedGameStateSnapshot(
                source.SchemaVersion,
                source.SessionId,
                source.SlotId,
                source.CreatedUtc,
                _clock.UtcNow.AddMinutes(1),
                source.CurrentDay,
                source.CashCents,
                source.CurrencyCode,
                source.Inventories,
                source.SupplierOrders,
                source.Displays,
                new[]
                {
                    new CustomerSaveRecord(
                        "customer-active",
                        "profile-active",
                        "Browsing",
                        30,
                        1)
                },
                new[]
                {
                    new ShoppingSessionSaveRecord(
                        "customer-active",
                        "intent-active",
                        "cart-active",
                        "Shopping",
                        1)
                },
                new[]
                {
                    new ReservationSaveRecord(
                        "reservation-active",
                        "customer-active",
                        "cart-active",
                        "fixture-1",
                        "game-neon-drift",
                        1,
                        "Active")
                },
                source.QueueEntries,
                source.CheckoutStation,
                source.Transactions,
                source.DayCycle,
                source.LedgerEntries);
        }
    }
}
