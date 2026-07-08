using System;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.Store;
using VRMGames.CartridgeAndCloud.Application.UIUX;
using VRMGames.CartridgeAndCloud.Domain.Checkout;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;
using VRMGames.CartridgeAndCloud.Domain.Persistence;
using VRMGames.CartridgeAndCloud.Domain.Store;
using VRMGames.CartridgeAndCloud.Infrastructure.Persistence;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Store
{
    public sealed class W3OrderEconomyCharacterizationTests
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
            _repository = new JsonStoreOperationsStateRepository(_directory);
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
        public void CHAR_ODR_001_RequestReservesFundsWithoutPaying()
        {
            long cashBefore = _active.Snapshot.CashCents;

            StoreOperationResult result =
                _service.OrderFurniture("central-shelf", 1);

            Assert.That(result.Succeeded, Is.True);
            Assert.That(_active.Snapshot.CashCents, Is.EqualTo(cashBefore));
            Assert.That(_service.ReservedFundsCents, Is.EqualTo(26000));
            Assert.That(_service.AvailableCashCents, Is.EqualTo(cashBefore - 26000));
            Assert.That(_service.State.Orders[0].HasFundsReservation, Is.True);
        }

        [Test]
        public void CHAR_ODR_002_ReservedFundsCannotBeSpentTwice()
        {
            Assert.That(
                _service.OrderFurniture("checkout-counter", 2).Succeeded,
                Is.True);

            StoreOperationResult result =
                _service.OrderFurniture("central-shelf", 1);

            Assert.That(result.Status, Is.EqualTo(StoreOperationStatus.InsufficientCash));
            Assert.That(_service.State.Orders.Count, Is.EqualTo(1));
            Assert.That(_service.ReservedFundsCents, Is.EqualTo(90000));
            Assert.That(_service.AvailableCashCents, Is.EqualTo(10000));
        }

        [Test]
        public void CHAR_ODR_003_PhysicalReceiptConsumesReservationCashLedgerAndStockOnce()
        {
            string orderId = _service.OrderFurniture("central-shelf", 1).Detail;
            long cashBefore = _active.Snapshot.CashCents;

            StoreOperationResult dispatch = _service.ReceiveOrder(orderId);
            string runId = _service.State.DeliveryRuns[0].DeliveryRunId;

            Assert.That(dispatch.Succeeded, Is.True);
            Assert.That(_service.State.Orders[0].State,
                Is.EqualTo(StoreOrderStatus.InTransit));
            Assert.That(_active.Snapshot.CashCents, Is.EqualTo(cashBefore));
            Assert.That(_service.ReservedFundsCents, Is.EqualTo(26000));
            Assert.That(_service.GetFurnitureWarehouseQuantity("central-shelf"), Is.EqualTo(0));
            Assert.That(_active.Snapshot.LedgerEntries.Count, Is.EqualTo(0));

            StoreOperationResult receipt =
                _service.CompleteDeliveryRun(runId);

            Assert.That(receipt.Succeeded, Is.True);
            Assert.That(_active.Snapshot.CashCents, Is.EqualTo(74000));
            Assert.That(_service.ReservedFundsCents, Is.EqualTo(0));
            Assert.That(_service.GetFurnitureWarehouseQuantity("central-shelf"), Is.EqualTo(1));
            Assert.That(_active.Snapshot.LedgerEntries.Count, Is.EqualTo(1));
            Assert.That(_service.State.WeeklySupplierCostCents, Is.EqualTo(26000));
            Assert.That(_service.State.DeliveryRuns[0].Status,
                Is.EqualTo(StoreDeliveryRunStatus.Received));
        }

        [Test]
        public void CHAR_ODR_004_RepeatedPhysicalReceiptDoesNotDuplicateMutations()
        {
            string orderId = _service.OrderFurniture("central-shelf", 1).Detail;
            _service.ReceiveOrder(orderId);
            string runId = _service.State.DeliveryRuns[0].DeliveryRunId;
            _service.CompleteDeliveryRun(runId);

            StoreOperationResult second = _service.CompleteDeliveryRun(runId);

            Assert.That(second.Succeeded, Is.True);
            Assert.That(_active.Snapshot.CashCents, Is.EqualTo(74000));
            Assert.That(_service.GetFurnitureWarehouseQuantity("central-shelf"), Is.EqualTo(1));
            Assert.That(_active.Snapshot.LedgerEntries.Count, Is.EqualTo(1));
            Assert.That(_service.State.DeliveryRuns.Count, Is.EqualTo(1));
        }

        [Test]
        public void CHAR_ODR_005_CancellationReleasesReservation()
        {
            string orderId = _service.OrderFurniture("central-shelf", 1).Detail;

            StoreOperationResult result = _service.CancelOrder(orderId);

            Assert.That(result.Succeeded, Is.True);
            Assert.That(_service.ReservedFundsCents, Is.EqualTo(0));
            Assert.That(_service.AvailableCashCents, Is.EqualTo(100000));
            Assert.That(_service.State.Orders[0].State, Is.EqualTo(StoreOrderStatus.Cancelled));
            Assert.That(_active.Snapshot.CashCents, Is.EqualTo(100000));
        }

        [Test]
        public void CHAR_ODR_006_ProcessAllFailureIsAtomic()
        {
            _service.OrderFurniture("central-shelf", 1);
            _service.OrderProduct("game-neon-drift", 1);
            _active.Replace(CloneSnapshot(_active.Snapshot, 30000, 1, "Open"));

            StoreOperationResult result = _service.ProcessAllPendingOrders();

            Assert.That(result.Status, Is.EqualTo(StoreOperationStatus.InsufficientCash));
            Assert.That(_service.State.DeliveryRuns.Count, Is.EqualTo(0));
            Assert.That(_service.State.Orders[0].State, Is.EqualTo(StoreOrderStatus.Ordered));
            Assert.That(_service.State.Orders[1].State, Is.EqualTo(StoreOrderStatus.Ordered));
            Assert.That(_service.GetFurnitureWarehouseQuantity("central-shelf"), Is.EqualTo(0));
            Assert.That(_service.GetProductWarehouseQuantity("game-neon-drift"), Is.EqualTo(0));
            Assert.That(_active.Snapshot.LedgerEntries.Count, Is.EqualTo(0));
        }

        [Test]
        public void CHAR_ODR_007_ProcessAllCreatesOneInTransitRunAndKeepsOrdersSeparate()
        {
            _service.OrderFurniture("central-shelf", 1);
            _service.OrderProduct("game-neon-drift", 1);
            long cashBefore = _active.Snapshot.CashCents;

            StoreOperationResult result = _service.ProcessAllPendingOrders();

            Assert.That(result.Succeeded, Is.True);
            Assert.That(_service.State.DeliveryRuns.Count, Is.EqualTo(1));
            Assert.That(_service.State.DeliveryRuns[0].Status,
                Is.EqualTo(StoreDeliveryRunStatus.InTransit));
            Assert.That(_service.State.DeliveryRuns[0].OrderIds.Count, Is.EqualTo(2));
            Assert.That(_service.State.Orders.Count, Is.EqualTo(2));
            Assert.That(_service.State.Orders[0].State,
                Is.EqualTo(StoreOrderStatus.InTransit));
            Assert.That(_service.State.Orders[1].State,
                Is.EqualTo(StoreOrderStatus.InTransit));
            Assert.That(_active.Snapshot.LedgerEntries.Count, Is.EqualTo(0));
            Assert.That(_active.Snapshot.CashCents, Is.EqualTo(cashBefore));
            Assert.That(_service.GetFurnitureWarehouseQuantity("central-shelf"), Is.EqualTo(0));
            Assert.That(_service.GetProductWarehouseQuantity("game-neon-drift"), Is.EqualTo(0));

            Assert.That(
                _service.CompleteDeliveryRun(
                    _service.State.DeliveryRuns[0].DeliveryRunId).Succeeded,
                Is.True);
            Assert.That(_active.Snapshot.LedgerEntries.Count, Is.EqualTo(2));
            Assert.That(_active.Snapshot.CashCents, Is.EqualTo(56000));
        }

        [Test]
        public void CHAR_ODR_008_SaveLoadPreservesInTransitRunAndReceiptIdempotency()
        {
            _service.OrderFurniture("central-shelf", 1);
            _service.ReceiveOrder(_service.State.Orders[0].OrderId);
            _service.OrderProduct("game-neon-drift", 1);
            _service.SaveCheckpoint();

            StoreOperationsState loaded = _repository.Load(new SaveSlotId(0));

            Assert.That(loaded, Is.Not.Null);
            Assert.That(loaded.DeliveryRuns.Count, Is.EqualTo(1));
            Assert.That(loaded.DeliveryRuns[0].Status,
                Is.EqualTo(StoreDeliveryRunStatus.InTransit));
            Assert.That(loaded.Orders.Count, Is.EqualTo(2));
            Assert.That(loaded.Orders[0].State,
                Is.EqualTo(StoreOrderStatus.InTransit));
            Assert.That(loaded.Orders[1].HasFundsReservation, Is.True);
            Assert.That(loaded.ReservedFundsCents, Is.EqualTo(44000));
            Assert.That(loaded.WeeklySupplierCostCents, Is.EqualTo(0));

            StoreOperationsFacade reloaded = new StoreOperationsFacade(
                _catalog,
                _repository,
                _active,
                _clock);
            reloaded.InitializeForActiveSlot();

            string runId = loaded.DeliveryRuns[0].DeliveryRunId;
            Assert.That(reloaded.CompleteDeliveryRun(runId).Succeeded, Is.True);
            Assert.That(reloaded.CompleteDeliveryRun(runId).Succeeded, Is.True);
            Assert.That(_active.Snapshot.CashCents, Is.EqualTo(74000));
            Assert.That(_active.Snapshot.LedgerEntries.Count, Is.EqualTo(1));
        }

        [TestCase(10000, 2000, 800)]
        [TestCase(1000, 2000, 0)]
        public void CHAR_ODR_009_WeeklyTaxUsesTenPercentOfPositiveGross(
            long weeklyRevenue,
            long weeklySupplierCost,
            long expectedTax)
        {
            PrepareClosingWeek(weeklyRevenue, weeklySupplierCost);

            StoreOperationResult result = _service.SettleClosingEconomy();

            Assert.That(result.Succeeded, Is.True);
            Assert.That(_service.State.LastSettledWeek, Is.EqualTo(1));
            Assert.That(_service.State.LastWeeklyGrossResultCents,
                Is.EqualTo(weeklyRevenue - weeklySupplierCost));
            Assert.That(_service.State.LastWeeklyTaxCents, Is.EqualTo(expectedTax));
            Assert.That(_active.Snapshot.CashCents, Is.EqualTo(100000 - expectedTax));
            Assert.That(_service.State.WeeklyRevenueCents, Is.EqualTo(0));
            Assert.That(_service.State.WeeklySupplierCostCents, Is.EqualTo(0));
        }

        [Test]
        public void CHAR_ODR_010_WeeklySettlementIsAppliedExactlyOnce()
        {
            PrepareClosingWeek(10000, 2000);
            _service.SettleClosingEconomy();
            _service.SaveCheckpoint();
            long cashAfterFirst = _active.Snapshot.CashCents;
            int ledgerAfterFirst = _active.Snapshot.LedgerEntries.Count;

            StoreOperationsFacade reloaded = new StoreOperationsFacade(
                _catalog,
                _repository,
                _active,
                _clock);
            reloaded.InitializeForActiveSlot();
            StoreOperationResult second = reloaded.SettleClosingEconomy();

            Assert.That(second.Succeeded, Is.True);
            Assert.That(_active.Snapshot.CashCents, Is.EqualTo(cashAfterFirst));
            Assert.That(_active.Snapshot.LedgerEntries.Count, Is.EqualTo(ledgerAfterFirst));
            Assert.That(reloaded.State.LifetimeTaxCents, Is.EqualTo(800));
        }

        private void PrepareClosingWeek(long weeklyRevenue, long weeklySupplierCost)
        {
            _active.Replace(CloneSnapshot(_active.Snapshot, 100000, 7, "Closing"));

            StoreOperationsState state = new StoreOperationsState(
                _active.ActiveSlotId,
                _active.Snapshot.SessionId.Value,
                0,
                1,
                1,
                1,
                1,
                0,
                weeklyRevenue,
                weeklySupplierCost,
                0,
                weeklyRevenue,
                weeklySupplierCost,
                0,
                0,
                0,
                new StoreOrderRecord[0],
                new StoreDeliveryRunRecord[0],
                new StoreStockRecord[0],
                new StoreStockRecord[0],
                new PlacedStoreFixtureRecord[0]);

            _repository.Save(state);
            _service.InitializeForActiveSlot();
        }

        private IntegratedGameStateSnapshot CloneSnapshot(
            IntegratedGameStateSnapshot source,
            long cashCents,
            int currentDay,
            string dayState)
        {
            return new IntegratedGameStateSnapshot(
                source.SchemaVersion,
                source.SessionId,
                source.SlotId,
                source.CreatedUtc,
                _clock.UtcNow.AddMinutes(currentDay),
                currentDay,
                cashCents,
                source.CurrencyCode,
                source.Inventories,
                source.SupplierOrders,
                source.Displays,
                source.Customers,
                source.ShoppingSessions,
                source.Reservations,
                source.QueueEntries,
                new CheckoutStationSaveRecord(
                    source.CheckoutStation.StationId,
                    dayState == "Closed" ? "Closed" : "Available",
                    string.Empty),
                source.Transactions,
                new DayCycleSaveRecord(
                    "day-" + currentDay.ToString("000"),
                    dayState,
                    source.DayCycle.OpenDurationSeconds,
                    dayState == "BeforeOpen" ? 0 : source.DayCycle.OpenDurationSeconds,
                    source.DayCycle.AutoBeginClosing,
                    source.DayCycle.SimulationSpeedMultiplier),
                source.LedgerEntries);
        }
    }
}
