using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.Customers;
using VRMGames.CartridgeAndCloud.Application.Store;
using VRMGames.CartridgeAndCloud.Application.UIUX;
using VRMGames.CartridgeAndCloud.Domain.Checkout;
using VRMGames.CartridgeAndCloud.Domain.Customers;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;
using VRMGames.CartridgeAndCloud.Domain.Persistence;
using VRMGames.CartridgeAndCloud.Domain.Store;
using VRMGames.CartridgeAndCloud.Editor.ProjectOrganization.Validation;
using VRMGames.CartridgeAndCloud.Infrastructure.Persistence;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Store
{
    public sealed class W8GlobalRegressionCharacterizationTests
    {
        private string _directory;
        private StoreContentCatalog _catalog;
        private JsonStoreOperationsStateRepository _operationsRepository;
        private ActiveGameSessionService _active;
        private StoreOperationsTestFactory.FixedClock _clock;
        private StoreOperationsFacade _service;

        [SetUp]
        public void SetUp()
        {
            _directory = StoreOperationsTestFactory.TempDirectory();
            _catalog = StoreOperationsTestFactory.Catalog();
            _operationsRepository =
                new JsonStoreOperationsStateRepository(
                    Path.Combine(_directory, "Operations"));
            _active = StoreOperationsTestFactory.ActiveSession();
            _clock = new StoreOperationsTestFactory.FixedClock(
                StoreOperationsTestFactory.Utc());
            _service = CreateService(_active);
            _service.InitializeForActiveSlot();
        }

        [TearDown]
        public void TearDown()
        {
            StoreOperationsTestFactory.DeleteDirectory(_directory);
        }

        [Test]
        public void CHAR_REG_001_NewGameOrderReceiptDisplaySaleAndCloseComplete()
        {
            PrepareOrderedPlayableStore();
            OpenCurrentDay();

            Assert.That(_service.ProcessNextCustomerPurchase().Succeeded, Is.True);

            _active.Replace(WithDayState(
                _active.Snapshot,
                1,
                "Closed",
                300));
            StoreOperationResult settlement =
                _service.SettleClosingEconomy();

            PlacedStoreFixtureRecord shelf =
                FindFixture("shelf-001");
            StoreManagementDayDetailRecord day =
                _service.State.ManagementHistory.FindDetailedDay(1);

            Assert.That(settlement.Succeeded, Is.True);
            Assert.That(_active.Snapshot.CashCents, Is.EqualTo(13999));
            Assert.That(_active.Snapshot.LedgerEntries.Count, Is.EqualTo(4));
            Assert.That(_service.State.CompletedSales, Is.EqualTo(1));
            Assert.That(shelf.ProductQuantity, Is.EqualTo(4));
            Assert.That(day, Is.Not.Null);
            Assert.That(day.IsClosed, Is.True);
            Assert.That(day.CompletedSales, Is.EqualTo(1));
        }

        [Test]
        public void CHAR_REG_002_SaveLoadPreservesEconomyStockAndHistory()
        {
            PrepareOrderedPlayableStore();
            OpenCurrentDay();
            _service.ProcessNextCustomerPurchase();
            _active.Replace(WithDayState(
                _active.Snapshot,
                1,
                "Closed",
                300));
            _service.SettleClosingEconomy();
            _service.SaveCheckpoint();

            JsonIntegratedSaveRepository integrated =
                new JsonIntegratedSaveRepository(
                    Path.Combine(_directory, "Integrated"));
            Assert.That(integrated.Save(_active.Snapshot).Succeeded, Is.True);
            Assert.That(
                integrated.Load(
                    new SaveSlotId(0),
                    out IntegratedGameStateSnapshot loadedSnapshot).Succeeded,
                Is.True);

            StoreOperationsState loadedOperations =
                _operationsRepository.Load(new SaveSlotId(0));

            Assert.That(loadedSnapshot.CashCents, Is.EqualTo(_active.Snapshot.CashCents));
            Assert.That(
                loadedSnapshot.LedgerEntries.Count,
                Is.EqualTo(_active.Snapshot.LedgerEntries.Count));
            Assert.That(loadedOperations.CompletedSales, Is.EqualTo(1));
            Assert.That(
                loadedOperations.ManagementHistory
                    .FindDetailedDay(1).CompletedSales,
                Is.EqualTo(1));
            Assert.That(
                loadedOperations.Fixtures
                    .Single(fixture => fixture.InstanceId == "shelf-001")
                    .ProductQuantity,
                Is.EqualTo(4));
        }

        [Test]
        public void CHAR_REG_003_RepeatedReceiptDoesNotDuplicateCashStockLedgerOrRun()
        {
            string orderId =
                _service.OrderProduct("game-neon-drift", 1).Detail;

            Assert.That(_service.DispatchAndComplete(orderId).Succeeded, Is.True);
            long cash = _active.Snapshot.CashCents;
            int ledger = _active.Snapshot.LedgerEntries.Count;
            int stock = _service.GetProductWarehouseQuantity("game-neon-drift");
            int runs = _service.State.DeliveryRuns.Count;

            Assert.That(_service.DispatchAndComplete(orderId).Succeeded, Is.False);
            Assert.That(_active.Snapshot.CashCents, Is.EqualTo(cash));
            Assert.That(_active.Snapshot.LedgerEntries.Count, Is.EqualTo(ledger));
            Assert.That(
                _service.GetProductWarehouseQuantity("game-neon-drift"),
                Is.EqualTo(stock));
            Assert.That(_service.State.DeliveryRuns.Count, Is.EqualTo(runs));
        }

        [Test]
        public void CHAR_REG_004_RepeatedLoadDoesNotDuplicateStateOrAdvanceGeneration()
        {
            PrepareOrderedPlayableStore();
            _service.SaveCheckpoint();
            int generation = _service.State.Generation;
            int orderCount = _service.State.Orders.Count;
            int fixtureCount = _service.State.Fixtures.Count;
            int ledgerCount = _active.Snapshot.LedgerEntries.Count;

            StoreOperationsFacade firstReload = CreateService(_active);
            firstReload.InitializeForActiveSlot();
            StoreOperationsFacade secondReload = CreateService(_active);
            secondReload.InitializeForActiveSlot();

            Assert.That(firstReload.State.Generation, Is.EqualTo(generation));
            Assert.That(secondReload.State.Generation, Is.EqualTo(generation));
            Assert.That(firstReload.State.Orders.Count, Is.EqualTo(orderCount));
            Assert.That(secondReload.State.Fixtures.Count, Is.EqualTo(fixtureCount));
            Assert.That(_active.Snapshot.LedgerEntries.Count, Is.EqualTo(ledgerCount));
        }

        [Test]
        public void CHAR_REG_005_DaySevenAppliesOneWeeklyTaxOnly()
        {
            SeedStockedStore(12);
            _active.Replace(WithDayState(
                _active.Snapshot,
                7,
                "Open",
                0));
            _service.SynchronizeManagementDay();

            for (int index = 0; index < 12; index++)
            {
                Assert.That(_service.ProcessNextCustomerPurchase().Succeeded, Is.True);
            }

            _active.Replace(WithDayState(
                _active.Snapshot,
                7,
                "Closed",
                300));

            StoreOperationResult first = _service.SettleClosingEconomy();
            long cashAfterFirst = _active.Snapshot.CashCents;
            int ledgerAfterFirst = _active.Snapshot.LedgerEntries.Count;
            StoreOperationResult second = _service.SettleClosingEconomy();

            Assert.That(first.Succeeded, Is.True);
            Assert.That(second.Succeeded, Is.True);
            Assert.That(_service.State.LastSettledWeek, Is.EqualTo(1));
            Assert.That(_service.State.LastWeeklyGrossResultCents, Is.EqualTo(35988));
            Assert.That(_service.State.LastWeeklyTaxCents, Is.EqualTo(3598));
            Assert.That(_active.Snapshot.CashCents, Is.EqualTo(cashAfterFirst));
            Assert.That(_active.Snapshot.LedgerEntries.Count, Is.EqualTo(ledgerAfterFirst));
            Assert.That(
                _active.Snapshot.LedgerEntries.Count(
                    entry => entry.PostingType == "WeeklyTax"),
                Is.EqualTo(1));
        }

        [Test]
        public void CHAR_REG_006_FourDaysKeepThreeDetailsAndOneSummary()
        {
            StoreManagementHistory history = StoreManagementHistory.Empty();

            for (int day = 1; day <= 4; day++)
            {
                string dayId = $"day-{day:000}";
                string customerId = $"customer-{day:000}";
                history = history
                    .RecordVisit(day, dayId, 100000, customerId)
                    .RecordVisitOutcome(
                        day,
                        dayId,
                        100000,
                        customerId,
                        StoreCustomerVisitOutcome.Purchased)
                    .RecordSale(
                        day,
                        dayId,
                        100000,
                        new StoreSaleDetailRecord(
                            $"transaction-{day:000}",
                            customerId,
                            "game-neon-drift",
                            1,
                            2999))
                    .CloseDay(day, dayId, 100000, 102999, 0);
            }

            Assert.That(
                history.RecentDayDetails.Select(day => day.DayNumber),
                Is.EqualTo(new[] { 2, 3, 4 }));
            Assert.That(history.DailySummaries.Count, Is.EqualTo(1));
            Assert.That(history.FindSummary(1), Is.Not.Null);
            Assert.That(history.FindSummary(1).RevenueCents, Is.EqualTo(2999));
            Assert.That(history.FindSummary(1).CompletedSales, Is.EqualTo(1));
        }

        [Test]
        public void CHAR_REG_007_EightActiveCustomersBlockNinthUntilDespawn()
        {
            ActiveCustomerRegistry registry = new ActiveCustomerRegistry();
            for (int index = 0; index < 8; index++)
            {
                Assert.That(
                    registry.TryRegister(
                        new CustomerInstanceId($"customer-{index:000}")),
                    Is.True);
            }

            StoreCustomerAdmissionPolicy policy =
                new StoreCustomerAdmissionPolicy();
            StoreCustomerAdmissionDecision blocked =
                policy.EvaluateSpawn(
                    AdmissionContext(registry.ActiveCount));

            registry.TrySetState(
                new CustomerInstanceId("customer-000"),
                ActiveCustomerState.Despawned);
            StoreCustomerAdmissionDecision allowed =
                policy.EvaluateSpawn(
                    AdmissionContext(registry.ActiveCount));

            Assert.That(blocked.Allowed, Is.False);
            Assert.That(
                blocked.FailureReason,
                Is.EqualTo(StoreCustomerAdmissionFailureReason.CapacityReached));
            Assert.That(registry.ActiveCount, Is.EqualTo(7));
            Assert.That(allowed.Allowed, Is.True);
        }

        [Test]
        public void CHAR_REG_008_SaleDrainsQueueAndCustomerBeforeClosingCompletes()
        {
            SeedStockedStore(1);
            OpenCurrentDay();

            Assert.That(_service.ProcessNextCustomerPurchase().Succeeded, Is.True);
            Assert.That(_active.Snapshot.QueueEntries, Is.Empty);
            Assert.That(
                _active.Snapshot.Customers.All(
                    customer => customer.State == "Despawned"),
                Is.True);

            StoreCustomerAdmissionDecision closing =
                new StoreCustomerAdmissionPolicy()
                    .EvaluateClosingCompletion(
                        new StoreCustomerAdmissionContext(
                            "Closing",
                            true,
                            false,
                            0,
                            8));

            Assert.That(closing.Allowed, Is.True);
        }

        [Test]
        public void CHAR_REG_009_ProcessAllKeepsIdentityAndIsIdempotent()
        {
            string furnitureOrder =
                _service.OrderFurniture("central-shelf", 1).Detail;
            string productOrder =
                _service.OrderProduct("game-neon-drift", 1).Detail;

            StoreOperationResult first =
                _service.ProcessAllAndComplete();
            StoreOperationResult second =
                _service.ProcessAllAndComplete();

            Assert.That(first.Succeeded, Is.True);
            Assert.That(second.Succeeded, Is.False);
            Assert.That(_service.State.DeliveryRuns.Count, Is.EqualTo(1));
            Assert.That(
                _service.State.DeliveryRuns[0].OrderIds,
                Is.EquivalentTo(new[] { furnitureOrder, productOrder }));
            Assert.That(_service.State.Orders.Select(order => order.OrderId),
                Is.EquivalentTo(new[] { furnitureOrder, productOrder }));
            Assert.That(_active.Snapshot.LedgerEntries.Count, Is.EqualTo(2));
        }

        [Test]
        public void CHAR_REG_010_PlacementRoundTripPreservesAnchorAndRotation()
        {
            string orderId =
                _service.OrderFurniture("central-shelf", 1).Detail;
            _service.DispatchAndComplete(orderId);
            Assert.That(
                _service.ConfirmFurniturePlacement(
                    "central-shelf",
                    "placed-regression-shelf",
                    13,
                    21,
                    3).Succeeded,
                Is.True);
            _service.SaveCheckpoint();

            StoreOperationsState loaded =
                _operationsRepository.Load(new SaveSlotId(0));
            PlacedStoreFixtureRecord fixture =
                loaded.Fixtures.Single(
                    value => value.InstanceId == "placed-regression-shelf");

            Assert.That(fixture.AnchorX, Is.EqualTo(13));
            Assert.That(fixture.AnchorZ, Is.EqualTo(21));
            Assert.That(fixture.RotationQuarterTurns, Is.EqualTo(3));
        }

        [Test]
        public void CHAR_REG_011_NewSessionDoesNotReusePreviousSidecarState()
        {
            PrepareOrderedPlayableStore();
            _service.SaveCheckpoint();

            ActiveGameSessionService nextActive =
                StoreOperationsTestFactory.ActiveSession();
            Assert.That(
                nextActive.Snapshot.SessionId,
                Is.Not.EqualTo(_active.Snapshot.SessionId));

            StoreOperationsFacade nextService =
                CreateService(nextActive);
            nextService.InitializeForActiveSlot();

            Assert.That(nextService.State.Orders, Is.Empty);
            Assert.That(nextService.State.DeliveryRuns, Is.Empty);
            Assert.That(nextService.State.Fixtures, Is.Empty);
            Assert.That(nextService.State.CompletedSales, Is.Zero);
            Assert.That(nextService.State.SessionId,
                Is.EqualTo(nextActive.Snapshot.SessionId.Value));
        }

        [Test]
        public void CHAR_REG_012_ReleaseConfigurationAndPlayerLogAuditAreDeterministic()
        {
            IReadOnlyList<string> validation =
                W8ReleaseValidationTool.ValidateProject();

            Assert.That(validation, Is.Empty,
                string.Join("\n", validation));
            Assert.That(
                W8ReleaseValidationTool.ProductionScenePaths.All(
                    path => path.IndexOf(
                        "TestLab",
                        StringComparison.OrdinalIgnoreCase) < 0),
                Is.True);

            string cleanLog = Path.Combine(_directory, "clean-player.log");
            File.WriteAllText(cleanLog, "Boot complete\nGolden Path PASS\n");
            Assert.That(
                W8ReleaseValidationTool.AuditPlayerLog(cleanLog),
                Is.Empty);

            string invalidLog = Path.Combine(_directory, "invalid-player.log");
            File.WriteAllText(
                invalidLog,
                "NullReferenceException: regression failure\n");
            Assert.That(
                W8ReleaseValidationTool.AuditPlayerLog(invalidLog),
                Has.Count.EqualTo(1));
        }

        private StoreOperationsFacade CreateService(
            ActiveGameSessionService active)
        {
            return new StoreOperationsFacade(
                _catalog,
                _operationsRepository,
                active,
                _clock);
        }

        private void PrepareOrderedPlayableStore()
        {
            string checkoutOrder =
                _service.OrderFurniture("checkout-counter", 1).Detail;
            string shelfOrder =
                _service.OrderFurniture("central-shelf", 1).Detail;
            Assert.That(_service.DispatchAndComplete(checkoutOrder).Succeeded, Is.True);
            Assert.That(_service.DispatchAndComplete(shelfOrder).Succeeded, Is.True);
            Assert.That(
                _service.ConfirmFurniturePlacement(
                    "checkout-counter",
                    "checkout-001",
                    2,
                    2,
                    0).Succeeded,
                Is.True);
            Assert.That(
                _service.ConfirmFurniturePlacement(
                    "central-shelf",
                    "shelf-001",
                    8,
                    8,
                    1).Succeeded,
                Is.True);

            string productOrder =
                _service.OrderProduct("game-neon-drift", 1).Detail;
            Assert.That(_service.DispatchAndComplete(productOrder).Succeeded, Is.True);
            Assert.That(
                _service.AssignProduct(
                    "shelf-001",
                    "game-neon-drift").Succeeded,
                Is.True);
            Assert.That(
                _service.RestockDisplay("shelf-001", 5).Succeeded,
                Is.True);
        }

        private void SeedStockedStore(int quantity)
        {
            Assert.That(
                _service.SeedInitialFixtures(
                    new[]
                    {
                        new PlacedStoreFixtureRecord(
                            "checkout-seeded",
                            "checkout-counter",
                            2,
                            2,
                            0,
                            string.Empty,
                            0),
                        new PlacedStoreFixtureRecord(
                            "shelf-seeded",
                            "central-shelf",
                            8,
                            8,
                            1,
                            "game-neon-drift",
                            quantity)
                    }).Succeeded,
                Is.True);
        }

        private void OpenCurrentDay()
        {
            _active.Replace(WithDayState(
                _active.Snapshot,
                _active.Snapshot.CurrentDay,
                "Open",
                0));
            _service.SynchronizeManagementDay();
        }

        private PlacedStoreFixtureRecord FindFixture(string instanceId)
        {
            return _service.State.Fixtures.Single(
                fixture => fixture.InstanceId == instanceId);
        }

        private static StoreCustomerAdmissionContext AdmissionContext(
            int activeCustomers)
        {
            return new StoreCustomerAdmissionContext(
                "Open",
                true,
                false,
                activeCustomers,
                8);
        }

        private static IntegratedGameStateSnapshot WithDayState(
            IntegratedGameStateSnapshot source,
            int currentDay,
            string state,
            int elapsedSeconds)
        {
            return new IntegratedGameStateSnapshot(
                source.SchemaVersion,
                source.SessionId,
                source.SlotId,
                source.CreatedUtc,
                source.UpdatedUtc.AddSeconds(1),
                currentDay,
                source.CashCents,
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
                    state == "Closed" ? "Closed" : "Available",
                    string.Empty),
                source.Transactions,
                new DayCycleSaveRecord(
                    $"day-{currentDay:000}",
                    state,
                    source.DayCycle.OpenDurationSeconds,
                    elapsedSeconds,
                    source.DayCycle.AutoBeginClosing,
                    source.DayCycle.SimulationSpeedMultiplier),
                source.LedgerEntries);
        }
    }
}
