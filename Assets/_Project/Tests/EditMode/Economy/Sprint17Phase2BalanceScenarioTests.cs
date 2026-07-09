using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using VRMGames.CartridgeAndCloud.Application.UIUX;
using VRMGames.CartridgeAndCloud.Application.Store;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;
using VRMGames.CartridgeAndCloud.Domain.Persistence;
using VRMGames.CartridgeAndCloud.Domain.Products;
using VRMGames.CartridgeAndCloud.Domain.Store;
using VRMGames.CartridgeAndCloud.Infrastructure.Persistence;
using VRMGames.CartridgeAndCloud.Infrastructure.Store;
using VRMGames.CartridgeAndCloud.Infrastructure.UIUX;
using VRMGames.CartridgeAndCloud.Tests.EditMode.Store;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Economy
{
    public sealed class Sprint17Phase2BalanceScenarioTests
    {
        private const string ContentCatalogPath =
            "Assets/_Project/Data/Catalogs/ContentCatalog.asset";

        private const string RuntimeSettingsPath =
            "Assets/_Project/Resources/CC_Sprint15Settings.asset";

        private const string CheckoutId = "checkout-counter";
        private const string DisplayId = "low-display";
        private const string CheckoutInstanceId =
            "s17-balance-checkout";
        private const string DisplayInstanceId =
            "s17-balance-display";

        private string _directory;
        private StoreContentCatalog _catalog;
        private UIRuntimeSettingsAsset _settings;
        private JsonStoreOperationsStateRepository
            _operationsRepository;
        private ActiveGameSessionService _active;
        private StoreOperationsTestFactory.FixedClock _clock;
        private StoreOperationsFacade _service;

        [SetUp]
        public void SetUp()
        {
            StoreContentCatalogAsset catalogAsset =
                AssetDatabase.LoadAssetAtPath<
                    StoreContentCatalogAsset>(ContentCatalogPath);
            _settings =
                AssetDatabase.LoadAssetAtPath<
                    UIRuntimeSettingsAsset>(RuntimeSettingsPath);

            Assert.That(catalogAsset, Is.Not.Null);
            Assert.That(_settings, Is.Not.Null);

            _directory =
                StoreOperationsTestFactory.TempDirectory();
            _catalog = catalogAsset.BuildCatalog();
            _operationsRepository =
                new JsonStoreOperationsStateRepository(
                    Path.Combine(_directory, "Operations"));
            _active =
                StoreOperationsTestFactory.ActiveSession(
                    _settings.InitialCashCents);
            _clock =
                new StoreOperationsTestFactory.FixedClock(
                    StoreOperationsTestFactory.Utc());
            _service = CreateService(_active);
            _service.InitializeForActiveSlot();
        }

        [TearDown]
        public void TearDown()
        {
            StoreOperationsTestFactory.DeleteDirectory(
                _directory);
        }

        [Test]
        public void S17_BAL_001_MinimumOpeningPackagesRetainTenPercentLiquidity()
        {
            StoreFixtureDefinition checkout =
                RequireFurniture(CheckoutId);
            long cheapestDisplayCost =
                _catalog.Furniture
                    .Where(item =>
                        item.IsPurchasable &&
                        item.SupportsProducts)
                    .Min(item => item.UnitCostCents);
            long minimumReserve =
                _settings.InitialCashCents / 10;

            Assert.That(
                _settings.InitialCashCents,
                Is.EqualTo(110000),
                "The approved Phase 2 tuning sets new-game cash to 1,100 EUR.");

            foreach (RetailProductDefinition product
                     in _catalog.Products)
            {
                long caseCost = checked(
                    product.WholesalePriceCents *
                    product.UnitsPerCase);
                long openingCost = checked(
                    checkout.UnitCostCents +
                    cheapestDisplayCost +
                    caseCost);
                long remainingCash = checked(
                    _settings.InitialCashCents -
                    openingCost);

                Assert.That(
                    remainingCash,
                    Is.GreaterThanOrEqualTo(
                        minimumReserve),
                    product.ProductId +
                    " must retain at least 10% of initial cash after the minimum opening package.");
            }
        }

        [Test]
        public void S17_BAL_002_PositiveScenarioSettlesTaxOnceAfterSaveLoad()
        {
            const string productId =
                "console-vertex-one";

            BeginScenarioDay(7);
            RetailProductDefinition product =
                PreparePlayableStore(productId);
            OpenDay(7);
            SellCases(product, 5);
            CloseDay(7);

            StoreOperationResult firstSettlement =
                _service.SettleClosingEconomy();

            Assert.That(firstSettlement.Succeeded, Is.True);
            Assert.That(
                _service.State.LastWeeklyGrossResultCents,
                Is.EqualTo(8990));
            Assert.That(
                _service.State.LastWeeklyTaxCents,
                Is.EqualTo(899));
            Assert.That(
                _active.Snapshot.CashCents,
                Is.EqualTo(118091));
            Assert.That(
                _active.Snapshot.LedgerEntries.Count(
                    entry =>
                        entry.PostingType ==
                        "WeeklyTax"),
                Is.EqualTo(1));

            var weeklySummary =
                new StoreUiProjectionService()
                    .BuildWeeklySummary(
                        _active.Snapshot,
                        _service.State);

            Assert.That(
                weeklySummary.GrossResultCents,
                Is.EqualTo(8990));
            Assert.That(
                weeklySummary.TaxCents,
                Is.EqualTo(899));
            Assert.That(
                weeklySummary.CashBeforeTaxCents,
                Is.EqualTo(118990));
            Assert.That(
                weeklySummary.CashAfterTaxCents,
                Is.EqualTo(118091));

            ReloadFromDisk();
            long cashAfterLoad =
                _active.Snapshot.CashCents;
            int ledgerAfterLoad =
                _active.Snapshot.LedgerEntries.Count;

            StoreOperationResult secondSettlement =
                _service.SettleClosingEconomy();

            Assert.That(secondSettlement.Succeeded, Is.True);
            Assert.That(
                _active.Snapshot.CashCents,
                Is.EqualTo(cashAfterLoad));
            Assert.That(
                _active.Snapshot.LedgerEntries.Count,
                Is.EqualTo(ledgerAfterLoad));
            Assert.That(
                _active.Snapshot.LedgerEntries.Count(
                    entry =>
                        entry.PostingType ==
                        "WeeklyTax"),
                Is.EqualTo(1));
        }

        [Test]
        public void S17_BAL_003_NearZeroScenarioRemainsPlayableWithoutTax()
        {
            const string productId =
                "console-vertex-one";

            BeginScenarioDay(7);
            RetailProductDefinition product =
                PreparePlayableStore(productId);
            OpenDay(7);
            SellCases(product, 4);
            CloseDay(7);

            StoreOperationResult settlement =
                _service.SettleClosingEconomy();

            Assert.That(settlement.Succeeded, Is.True);
            Assert.That(
                Math.Abs(
                    _service.State
                        .LastWeeklyGrossResultCents),
                Is.LessThanOrEqualTo(
                    _settings.InitialCashCents / 20),
                "Near-zero is defined as no more than 5% of initial cash away from zero.");
            Assert.That(
                _service.State.LastWeeklyGrossResultCents,
                Is.EqualTo(-5008));
            Assert.That(
                _service.State.LastWeeklyTaxCents,
                Is.Zero);
            Assert.That(
                _active.Snapshot.CashCents,
                Is.EqualTo(104992));
            Assert.That(
                _service.AvailableCashCents,
                Is.GreaterThan(0));
        }

        [Test]
        public void S17_BAL_004_NegativeScenarioPreservesCashAndRecoverableStock()
        {
            const string productId =
                "game-neon-drift";

            BeginScenarioDay(7);
            RetailProductDefinition product =
                PreparePlayableStore(productId);
            OpenDay(7);
            ReceiveAndRestockCase(product);
            SellUnits(4);
            CloseDay(7);

            StoreOperationResult settlement =
                _service.SettleClosingEconomy();
            PlacedStoreFixtureRecord display =
                _service.State.Fixtures.Single(
                    item =>
                        item.InstanceId ==
                        DisplayInstanceId);

            Assert.That(settlement.Succeeded, Is.True);
            Assert.That(
                _service.State.LastWeeklyGrossResultCents,
                Is.EqualTo(-67004));
            Assert.That(
                _service.State.LastWeeklyTaxCents,
                Is.Zero);
            Assert.That(
                _active.Snapshot.CashCents,
                Is.EqualTo(42996));
            Assert.That(
                _service.AvailableCashCents,
                Is.GreaterThan(0));
            Assert.That(
                display.ProductQuantity,
                Is.EqualTo(8),
                "The negative scenario must retain sellable stock and therefore remain recoverable.");
        }

        [Test]
        public void S17_BAL_005_SaveLoadPreservesUnsettledWeeklyEconomy()
        {
            const string productId =
                "console-vertex-one";

            BeginScenarioDay(7);
            RetailProductDefinition product =
                PreparePlayableStore(productId);
            OpenDay(7);
            SellCases(product, 2);

            long cashBeforeSave =
                _active.Snapshot.CashCents;
            long revenueBeforeSave =
                _service.State.WeeklyRevenueCents;
            long costsBeforeSave =
                _service.State.WeeklySupplierCostCents;
            int salesBeforeSave =
                _service.State.CompletedSales;
            int ledgerBeforeSave =
                _active.Snapshot.LedgerEntries.Count;

            ReloadFromDisk();

            Assert.That(
                _active.Snapshot.CashCents,
                Is.EqualTo(cashBeforeSave));
            Assert.That(
                _service.State.WeeklyRevenueCents,
                Is.EqualTo(revenueBeforeSave));
            Assert.That(
                _service.State.WeeklySupplierCostCents,
                Is.EqualTo(costsBeforeSave));
            Assert.That(
                _service.State.CompletedSales,
                Is.EqualTo(salesBeforeSave));
            Assert.That(
                _active.Snapshot.LedgerEntries.Count,
                Is.EqualTo(ledgerBeforeSave));
            Assert.That(
                _service.State.LastSettledWeek,
                Is.Zero);
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

        private RetailProductDefinition PreparePlayableStore(
            string productId)
        {
            ReceiveAndPlaceFurniture(
                CheckoutId,
                CheckoutInstanceId,
                2,
                2,
                0);
            ReceiveAndPlaceFurniture(
                DisplayId,
                DisplayInstanceId,
                8,
                8,
                0);

            RetailProductDefinition product =
                RequireProduct(productId);

            Assert.That(
                _service.AssignProduct(
                    DisplayInstanceId,
                    productId).Succeeded,
                Is.True);

            return product;
        }

        private void ReceiveAndPlaceFurniture(
            string definitionId,
            string instanceId,
            int anchorX,
            int anchorZ,
            int rotationQuarterTurns)
        {
            StoreOperationResult order =
                _service.OrderFurniture(
                    definitionId,
                    1);
            Assert.That(order.Succeeded, Is.True, order.Detail);
            Assert.That(
                _service.DispatchAndComplete(
                    order.Detail).Succeeded,
                Is.True);
            Assert.That(
                _service.ConfirmFurniturePlacement(
                    definitionId,
                    instanceId,
                    anchorX,
                    anchorZ,
                    rotationQuarterTurns).Succeeded,
                Is.True);
        }

        private void SellCases(
            RetailProductDefinition product,
            int caseCount)
        {
            for (int index = 0;
                 index < caseCount;
                 index++)
            {
                ReceiveAndRestockCase(product);
                SellUnits(product.UnitsPerCase);
            }
        }

        private void ReceiveAndRestockCase(
            RetailProductDefinition product)
        {
            StoreOperationResult order =
                _service.OrderProduct(
                    product.ProductId,
                    1);
            Assert.That(order.Succeeded, Is.True, order.Detail);
            Assert.That(
                _service.DispatchAndComplete(
                    order.Detail).Succeeded,
                Is.True);
            Assert.That(
                _service.RestockDisplay(
                    DisplayInstanceId,
                    product.UnitsPerCase).Succeeded,
                Is.True);
        }

        private void SellUnits(int quantity)
        {
            for (int index = 0;
                 index < quantity;
                 index++)
            {
                StoreOperationResult sale =
                    _service.ProcessNextCustomerPurchase();
                Assert.That(
                    sale.Succeeded,
                    Is.True,
                    sale.Detail);
            }
        }

        private void BeginScenarioDay(int day)
        {
            Assert.That(
                _active.Snapshot.LedgerEntries,
                Is.Empty,
                "Select the scenario day before creating economic postings.");

            _clock.UtcNow =
                _active.Snapshot.UpdatedUtc.AddMinutes(1);
            _active.Replace(
                WithDayState(
                    _active.Snapshot,
                    day,
                    "BeforeOpen",
                    0,
                    _clock.UtcNow));
            _service.SynchronizeManagementDay();
        }

        private void OpenDay(int day)
        {
            _clock.UtcNow =
                _active.Snapshot.UpdatedUtc.AddMinutes(1);
            _active.Replace(
                WithDayState(
                    _active.Snapshot,
                    day,
                    "Open",
                    0,
                    _clock.UtcNow));
            _service.SynchronizeManagementDay();
        }

        private void CloseDay(int day)
        {
            _clock.UtcNow =
                _active.Snapshot.UpdatedUtc.AddMinutes(1);
            _active.Replace(
                WithDayState(
                    _active.Snapshot,
                    day,
                    "Closed",
                    300,
                    _clock.UtcNow));
        }

        private void ReloadFromDisk()
        {
            _service.SaveCheckpoint();

            JsonIntegratedSaveRepository integrated =
                new JsonIntegratedSaveRepository(
                    Path.Combine(_directory, "Integrated"));

            Assert.That(
                integrated.Save(
                    _active.Snapshot).Succeeded,
                Is.True);
            Assert.That(
                integrated.Load(
                    _active.ActiveSlotId,
                    out IntegratedGameStateSnapshot
                        loadedSnapshot).Succeeded,
                Is.True);

            ActiveGameSessionService loadedActive =
                new ActiveGameSessionService();
            loadedActive.Activate(
                loadedSnapshot.SlotId,
                loadedSnapshot);

            _clock.UtcNow =
                loadedSnapshot.UpdatedUtc.AddMinutes(1);
            _active = loadedActive;
            _service = CreateService(_active);
            _service.InitializeForActiveSlot();
        }

        private StoreFixtureDefinition RequireFurniture(
            string definitionId)
        {
            Assert.That(
                _catalog.TryGetFurniture(
                    definitionId,
                    out StoreFixtureDefinition
                        definition),
                Is.True);
            return definition;
        }

        private RetailProductDefinition RequireProduct(
            string productId)
        {
            Assert.That(
                _catalog.TryGetProduct(
                    productId,
                    out RetailProductDefinition
                        product),
                Is.True);
            return product;
        }

        private static IntegratedGameStateSnapshot WithDayState(
            IntegratedGameStateSnapshot source,
            int currentDay,
            string state,
            int elapsedSeconds,
            DateTime updatedUtc)
        {
            return new IntegratedGameStateSnapshot(
                source.SchemaVersion,
                source.SessionId,
                source.SlotId,
                source.CreatedUtc,
                updatedUtc,
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
                    state == "Open"
                        ? "Available"
                        : "Closed",
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
