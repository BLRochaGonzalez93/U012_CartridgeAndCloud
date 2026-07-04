using System;
using System.IO;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Application.UIUX;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;
using VRMGames.CartridgeAndCloud.Domain.Persistence;
using VRMGames.CartridgeAndCloud.Infrastructure.UIUX;
using VRMGames.CartridgeAndCloud.Application.GameSession;
using VRMGames.CartridgeAndCloud.Application.Store;
using VRMGames.CartridgeAndCloud.Domain.Checkout;
using VRMGames.CartridgeAndCloud.Domain.Economy;
using VRMGames.CartridgeAndCloud.Domain.Store;
using VRMGames.CartridgeAndCloud.Infrastructure.Persistence;
using VRMGames.CartridgeAndCloud.Infrastructure.Store;
using VRMGames.CartridgeAndCloud.Domain.GameSession;
namespace VRMGames.CartridgeAndCloud.Runtime.Development.Scenarios
{
    public sealed class StoreOperationsGoldenPathRunner :
        MonoBehaviour
    {
        [SerializeField]
        private StoreContentCatalogAsset _catalogAsset;

        [SerializeField]
        private bool _runOnStart;

        public bool LastScenarioPassed {
            get;
            private set;
        }

        public bool LastFurnitureOrdered {
            get;
            private set;
        }

        public bool LastFurnitureReceived {
            get;
            private set;
        }

        public bool LastFurniturePlaced {
            get;
            private set;
        }

        public bool LastProductOrdered {
            get;
            private set;
        }

        public bool LastProductReceived {
            get;
            private set;
        }

        public bool LastDisplayRestocked {
            get;
            private set;
        }

        public bool LastCustomerSaleCompleted {
            get;
            private set;
        }

        public bool LastEconomyIntegrated {
            get;
            private set;
        }

        public bool LastStateRoundTripped {
            get;
            private set;
        }

        private void Start()
        {
            if (_runOnStart)
            {
                RunScenario();
            }
        }

        public void Configure(
            StoreContentCatalogAsset catalogAsset,
            bool runOnStart)
        {
            _catalogAsset = catalogAsset;
            _runOnStart = runOnStart;
        }

        [ContextMenu(
            "Run Sprint 16 Phase 1 Scenario")]
        public void RunScenario()
        {
            if (_catalogAsset == null)
            {
                StoreRuntimeAssetRegistry
                    registry =
                        StoreRuntimeAssetRegistry
                            .FindLoaded();

                _catalogAsset =
                    registry == null
                        ? null
                        : registry.ContentCatalog;
            }

            if (_catalogAsset == null)
            {
                throw new InvalidOperationException(
                    "Phase 1 content catalog is required.");
            }

            string directory =
                Path.Combine(
                    UnityEngine.Application
                        .temporaryCachePath,
                    "CC_S16_P1_Technical");

            if (Directory.Exists(directory))
            {
                Directory.Delete(
                    directory,
                    true);
            }

            Directory.CreateDirectory(directory);

            StoreContentCatalog catalog =
                _catalogAsset.BuildCatalog();

            JsonStoreOperationsStateRepository repository =
                new JsonStoreOperationsStateRepository(
                    directory);

            ActiveGameSessionService active =
                new ActiveGameSessionService();

            FixedClock clock =
                new FixedClock(
                    new DateTime(
                        2026,
                        6,
                        29,
                        18,
                        0,
                        0,
                        DateTimeKind.Utc));

            DefaultIntegratedGameStateFactory
                factory =
                    new DefaultIntegratedGameStateFactory(
                        "EUR",
                        100000,
                        300);

            IntegratedGameStateSnapshot initial =
                factory.Create(
                    new SaveSlotId(0),
                    clock.UtcNow);

            active.Activate(
                initial.SlotId,
                initial);

            StoreOperationsFacade service =
                new StoreOperationsFacade(
                    catalog,
                    repository,
                    active,
                    clock);

            service.InitializeForActiveSlot();

            StoreOperationResult checkoutOrder =
                service.OrderFurniture(
                    "checkout-counter",
                    1);

            StoreOperationResult displayOrder =
                service.OrderFurniture(
                    "central-shelf",
                    1);

            LastFurnitureOrdered =
                checkoutOrder.Succeeded &&
                displayOrder.Succeeded &&
                service.State.Orders.Count == 2;

            StoreOperationResult checkoutReceive =
                service.ReceiveOrder(
                    checkoutOrder.Detail);

            StoreOperationResult displayReceive =
                service.ReceiveOrder(
                    displayOrder.Detail);

            LastFurnitureReceived =
                checkoutReceive.Succeeded &&
                displayReceive.Succeeded &&
                service.GetFurnitureWarehouseQuantity(
                    "checkout-counter") == 1 &&
                service.GetFurnitureWarehouseQuantity(
                    "central-shelf") == 1;

            StoreOperationResult checkoutPlace =
                service.ConfirmFurniturePlacement(
                    "checkout-counter",
                    "technical-shelf-0001",
                    2,
                    2,
                    0);

            StoreOperationResult displayPlace =
                service.ConfirmFurniturePlacement(
                    "central-shelf",
                    "technical-shelf-0002",
                    8,
                    8,
                    1);

            LastFurniturePlaced =
                checkoutPlace.Succeeded &&
                displayPlace.Succeeded &&
                service.State.Fixtures.Count == 2;

            StoreOperationResult productOrder =
                service.OrderProduct(
                    "game-neon-drift",
                    1);

            LastProductOrdered =
                productOrder.Succeeded;

            StoreOperationResult productReceive =
                service.ReceiveOrder(
                    productOrder.Detail);

            LastProductReceived =
                productReceive.Succeeded &&
                service.GetProductWarehouseQuantity(
                    "game-neon-drift") == 12;

            StoreOperationResult assign =
                service.AssignProduct(
                    "technical-shelf-0002",
                    "game-neon-drift");

            StoreOperationResult restock =
                service.RestockDisplay(
                    "technical-shelf-0002",
                    5);

            LastDisplayRestocked =
                assign.Succeeded &&
                restock.Succeeded &&
                service.State.Fixtures[1]
                    .ProductQuantity == 5;

            clock.UtcNow =
                clock.UtcNow.AddMinutes(1);

            active.Replace(
                WithDayState(
                    active.Snapshot,
                    "Open",
                    30,
                    clock.UtcNow));

            StoreOperationResult purchase =
                service
                    .ProcessNextCustomerPurchase();

            LastCustomerSaleCompleted =
                purchase.Succeeded &&
                service.State.CompletedSales == 1 &&
                service.State.Fixtures[1]
                    .ProductQuantity == 4;

            LastEconomyIntegrated =
                active.Snapshot.CashCents ==
                    13999 &&
                active.Snapshot.LedgerEntries.Count ==
                    4 &&
                active.Snapshot.Customers.Count == 1 &&
                active.Snapshot
                    .ShoppingSessions.Count == 1 &&
                active.Snapshot
                    .Reservations.Count == 1 &&
                active.Snapshot
                    .Transactions.Count == 1 &&
                active.Snapshot
                    .QueueEntries.Count == 0;

            clock.UtcNow =
                clock.UtcNow.AddMinutes(4);

            active.Replace(
                WithDayState(
                    active.Snapshot,
                    "Closed",
                    300,
                    clock.UtcNow));

            service.SaveCheckpoint();

            StoreOperationsState reloaded =
                repository.Load(
                    new SaveSlotId(0));

            LastStateRoundTripped =
                reloaded != null &&
                reloaded.SessionId ==
                    initial.SessionId.Value &&
                reloaded.CompletedSales == 1 &&
                reloaded.Fixtures.Count == 2 &&
                reloaded.Fixtures[1]
                    .ProductQuantity == 4;

            LastScenarioPassed =
                LastFurnitureOrdered &&
                LastFurnitureReceived &&
                LastFurniturePlaced &&
                LastProductOrdered &&
                LastProductReceived &&
                LastDisplayRestocked &&
                LastCustomerSaleCompleted &&
                LastEconomyIntegrated &&
                LastStateRoundTripped;

            if (LastScenarioPassed)
            {
                Debug.Log(
                    "Sprint 16 Phase 1 playable vertical slice scenario PASS.");
            }
            else
            {
                Debug.LogError(
                    "Sprint 16 Phase 1 playable vertical slice scenario FAILED.");
            }
        }

        private static IntegratedGameStateSnapshot
            WithDayState(
                IntegratedGameStateSnapshot source,
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
                source.CurrentDay,
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
                    source.CheckoutStation
                        .StationId,
                    state == "Closed"
                        ? "Closed"
                        : "Available",
                    string.Empty),
                source.Transactions,
                new DayCycleSaveRecord(
                    source.DayCycle.DayId,
                    state,
                    source.DayCycle
                        .OpenDurationSeconds,
                    elapsedSeconds,
                    source.DayCycle
                        .AutoBeginClosing),
                source.LedgerEntries);
        }

        private sealed class FixedClock :
            IUtcClock
        {
            public DateTime UtcNow {
                get;
                set;
            }

            public FixedClock(DateTime utcNow)
            {
                UtcNow = utcNow;
            }
        }
    }
}
