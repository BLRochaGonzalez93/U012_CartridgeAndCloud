using System.IO;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.UIUX;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;
using VRMGames.CartridgeAndCloud.Domain.Store;
using VRMGames.CartridgeAndCloud.Domain.UIUX;
using VRMGames.CartridgeAndCloud.Infrastructure.Persistence;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Store
{
    public sealed class W6ManagementCharacterizationTests
    {
        [Test]
        public void CHAR_MGT_001_CurrentDayKeepsDetailedRecords()
        {
            StoreManagementHistory history = DayOne();
            StoreManagementDayDetailRecord day =
                history.FindDetailedDay(1);

            Assert.That(day, Is.Not.Null);
            Assert.That(day.CustomerVisits.Count, Is.EqualTo(1));
            Assert.That(day.Sales.Count, Is.EqualTo(1));
            Assert.That(day.Receipts.Count, Is.EqualTo(1));
            Assert.That(day.Sales[0].ProductId, Is.EqualTo("game-neon-drift"));
        }

        [Test]
        public void CHAR_MGT_002_CurrentAndTwoPreviousDaysRemainDetailed()
        {
            StoreManagementHistory history = DayOne()
                .EnsureDay(2, "day-002", 102000)
                .EnsureDay(3, "day-003", 103000);

            Assert.That(history.RecentDayDetails.Count, Is.EqualTo(3));
            Assert.That(history.FindDetailedDay(1), Is.Not.Null);
            Assert.That(history.FindDetailedDay(2), Is.Not.Null);
            Assert.That(history.FindDetailedDay(3), Is.Not.Null);
            Assert.That(history.DailySummaries.Count, Is.EqualTo(0));
        }

        [Test]
        public void CHAR_MGT_003_FourthDayCompactsOldestToDailySummary()
        {
            StoreManagementHistory history = DayOne()
                .EnsureDay(2, "day-002", 102000)
                .EnsureDay(3, "day-003", 103000)
                .EnsureDay(4, "day-004", 104000);

            Assert.That(history.RecentDayDetails.Count, Is.EqualTo(3));
            Assert.That(history.FindDetailedDay(1), Is.Null);
            Assert.That(history.FindSummary(1), Is.Not.Null);
            Assert.That(history.FindSummary(1).RevenueCents, Is.EqualTo(2999));
        }

        [Test]
        public void CHAR_MGT_004_CompactionDoesNotDuplicateOrLoseMetrics()
        {
            StoreManagementHistory history = DayOne()
                .EnsureDay(2, "day-002", 102000)
                .EnsureDay(3, "day-003", 103000)
                .EnsureDay(4, "day-004", 104000);

            StoreManagementHistory repeated = history
                .EnsureDay(4, "day-004", 104000);

            Assert.That(repeated.DailySummaries.Count, Is.EqualTo(1));
            Assert.That(repeated.FindSummary(1).CompletedSales, Is.EqualTo(1));
            Assert.That(repeated.FindSummary(1).OrdersReceived, Is.EqualTo(1));
            Assert.That(repeated.FindSummary(1).NetResultCents, Is.EqualTo(1299));
        }

        [Test]
        public void CHAR_MGT_005_DailyResultMatchesRecordedEconomicDetail()
        {
            StoreManagementDayDetailRecord day =
                DayOne().FindDetailedDay(1);

            Assert.That(day.RevenueCents, Is.EqualTo(2999));
            Assert.That(day.SupplierCostCents, Is.EqualTo(1500));
            Assert.That(day.TaxCents, Is.EqualTo(200));
            Assert.That(day.GrossResultCents, Is.EqualTo(1499));
            Assert.That(day.NetResultCents, Is.EqualTo(1299));
        }

        [Test]
        public void CHAR_MGT_006_SaveLoadPreservesDetailAndSummaries()
        {
            string directory = StoreOperationsTestFactory.TempDirectory();
            try
            {
                JsonStoreOperationsStateRepository repository =
                    new JsonStoreOperationsStateRepository(directory);
                StoreManagementHistory history = DayOne()
                    .EnsureDay(2, "day-002", 102000)
                    .EnsureDay(3, "day-003", 103000)
                    .EnsureDay(4, "day-004", 104000);

                repository.Save(State(history));
                StoreOperationsState loaded =
                    repository.Load(new SaveSlotId(0));

                Assert.That(loaded.ManagementHistory.RecentDayDetails.Count, Is.EqualTo(3));
                Assert.That(loaded.ManagementHistory.DailySummaries.Count, Is.EqualTo(1));
                Assert.That(loaded.ManagementHistory.FindSummary(1).NetResultCents, Is.EqualTo(1299));
            }
            finally
            {
                StoreOperationsTestFactory.DeleteDirectory(directory);
            }
        }

        [Test]
        public void CHAR_MGT_007_RepeatedCloseDoesNotDuplicateSummary()
        {
            StoreManagementHistory history = DayOne();
            StoreManagementHistory repeated = history.CloseDay(
                1,
                "day-001",
                100000,
                101299,
                200);

            Assert.That(object.ReferenceEquals(history, repeated), Is.True);
            Assert.That(repeated.RecentDayDetails.Count, Is.EqualTo(1));
            Assert.That(repeated.DailySummaries.Count, Is.EqualTo(0));
        }

        [Test]
        public void CHAR_MGT_008_SchemaTwoMigratesWithoutInventedHistory()
        {
            string directory = StoreOperationsTestFactory.TempDirectory();
            try
            {
                JsonStoreOperationsStateRepository repository =
                    new JsonStoreOperationsStateRepository(directory);
                string json =
                    "{\n" +
                    "  \"schemaVersion\": 2,\n" +
                    "  \"slotValue\": 0,\n" +
                    "  \"sessionId\": \"legacy-session\",\n" +
                    "  \"generation\": 0,\n" +
                    "  \"nextOrderSequence\": 1,\n" +
                    "  \"nextDeliveryRunSequence\": 1,\n" +
                    "  \"nextFixtureSequence\": 1,\n" +
                    "  \"nextCustomerSequence\": 1,\n" +
                    "  \"orders\": [],\n" +
                    "  \"deliveryRuns\": [],\n" +
                    "  \"furnitureWarehouse\": [],\n" +
                    "  \"productWarehouse\": [],\n" +
                    "  \"fixtures\": []\n" +
                    "}";
                Directory.CreateDirectory(directory);
                File.WriteAllText(
                    repository.PrimaryPath(new SaveSlotId(0)),
                    json);

                StoreOperationsState loaded =
                    repository.Load(new SaveSlotId(0));

                Assert.That(loaded, Is.Not.Null);
                Assert.That(loaded.ManagementHistory.RecentDayDetails.Count, Is.EqualTo(0));
                Assert.That(loaded.ManagementHistory.DailySummaries.Count, Is.EqualTo(0));
                Assert.That(loaded.LifetimeRevenueCents, Is.EqualTo(0));
            }
            finally
            {
                StoreOperationsTestFactory.DeleteDirectory(directory);
            }
        }

        [Test]
        public void HistoryProjection_UsesAuthoritativeManagementState()
        {
            StoreOperationsState state = State(DayOne());
            var snapshot = StoreOperationsTestFactory.ActiveSession().Snapshot;

            ManagementPanelSnapshot panel =
                new StoreUiProjectionService().BuildPanel(
                    snapshot,
                    ManagementPanelId.History,
                    state);

            Assert.That(panel.Rows.Count, Is.GreaterThan(1));
            Assert.That(panel.Rows[0].Label, Is.EqualTo("Day 1"));
        }

        private static StoreManagementHistory DayOne()
        {
            return StoreManagementHistory.Empty()
                .EnsureDay(1, "day-001", 100000)
                .RecordVisit(1, "day-001", 100000, "customer-1")
                .RecordVisitOutcome(
                    1,
                    "day-001",
                    100000,
                    "customer-1",
                    StoreCustomerVisitOutcome.Purchased)
                .RecordSale(
                    1,
                    "day-001",
                    100000,
                    new StoreSaleDetailRecord(
                        "transaction-1",
                        "customer-1",
                        "game-neon-drift",
                        1,
                        2999))
                .RecordReceipt(
                    1,
                    "day-001",
                    100000,
                    new StoreReceiptDetailRecord(
                        "receipt-1",
                        "run-1",
                        "order-1",
                        "game-neon-drift",
                        false,
                        12,
                        1500))
                .CloseDay(
                    1,
                    "day-001",
                    100000,
                    101299,
                    200);
        }

        private static StoreOperationsState State(
            StoreManagementHistory history)
        {
            return new StoreOperationsState(
                new SaveSlotId(0),
                "session-a",
                1,
                1,
                1,
                1,
                1,
                1,
                2999,
                1700,
                200,
                0,
                0,
                1,
                1499,
                200,
                new StoreOrderRecord[0],
                new StoreDeliveryRunRecord[0],
                new StoreStockRecord[0],
                new StoreStockRecord[0],
                new PlacedStoreFixtureRecord[0],
                history);
        }
    }
}
