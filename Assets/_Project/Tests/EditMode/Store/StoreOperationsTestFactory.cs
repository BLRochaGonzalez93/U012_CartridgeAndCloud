using System;
using System.IO;
using VRMGames.CartridgeAndCloud.Application.UIUX;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;
using VRMGames.CartridgeAndCloud.Domain.Persistence;
using VRMGames.CartridgeAndCloud.Application.GameSession;
using VRMGames.CartridgeAndCloud.Application.Store;
using VRMGames.CartridgeAndCloud.Domain.Checkout;
using VRMGames.CartridgeAndCloud.Domain.Economy;
using VRMGames.CartridgeAndCloud.Domain.Products;
using VRMGames.CartridgeAndCloud.Domain.Store;
using VRMGames.CartridgeAndCloud.Domain.GameSession;
namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Store
{
    internal static class StoreOperationsTestFactory
    {
        public static StoreContentCatalog Catalog()
        {
            return new StoreContentCatalog(
                new[]
                {
                    Furniture(
                        "checkout-counter",
                        StoreFixtureKind.CheckoutCounter,
                        4,
                        2,
                        1.1f,
                        0,
                        45000,
                        true,
                        true,
                        false),
                    Furniture(
                        "central-shelf",
                        StoreFixtureKind.CentralShelf,
                        4,
                        2,
                        1.6f,
                        32,
                        26000,
                        true,
                        true,
                        true),
                    Furniture(
                        "wall-shelf",
                        StoreFixtureKind.WallShelf,
                        4,
                        1,
                        2.2f,
                        24,
                        18000,
                        true,
                        true,
                        true),
                    Furniture(
                        "backroom-storage",
                        StoreFixtureKind.BackroomStorage,
                        5,
                        2,
                        2.4f,
                        80,
                        22000,
                        true,
                        false,
                        false)
                },
                new[]
                {
                    Product(
                        "game-neon-drift",
                        RetailProductKind.PhysicalGame,
                        1500,
                        2999,
                        12),
                    Product(
                        "console-vertex-one",
                        RetailProductKind.Console,
                        18000,
                        24999,
                        2)
                });
        }

        public static StoreFixtureDefinition
            Furniture(
                string id,
                StoreFixtureKind kind,
                int width,
                int depth,
                float height,
                int capacity,
                long cost,
                bool interactive,
                bool purchasable,
                bool supportsProducts)
        {
            return new StoreFixtureDefinition(
                id,
                id,
                kind,
                width,
                depth,
                height,
                capacity,
                cost,
                interactive,
                purchasable,
                supportsProducts,
                "material-" + id,
                "prefab/" + id);
        }

        public static RetailProductDefinition
            Product(
                string id,
                RetailProductKind kind,
                long wholesale,
                long sale,
                int unitsPerCase)
        {
            return new RetailProductDefinition(
                id,
                id,
                kind,
                wholesale,
                sale,
                unitsPerCase,
                "material-" + id,
                "label-" + id,
                "icon/" + id,
                "cover/" + id,
                "prefab/" + id);
        }

        public static DateTime Utc(
            int minute = 0)
        {
            return new DateTime(
                2026,
                6,
                29,
                20,
                minute,
                0,
                DateTimeKind.Utc);
        }

        public static ActiveGameSessionService
            ActiveSession(
                long initialCashCents = 100000)
        {
            ActiveGameSessionService active =
                new ActiveGameSessionService();

            IntegratedGameStateSnapshot snapshot =
                new DefaultIntegratedGameStateFactory(
                    "EUR",
                    initialCashCents,
                    300)
                    .Create(
                        new SaveSlotId(0),
                        Utc());

            active.Activate(
                snapshot.SlotId,
                snapshot);

            return active;
        }

        public static IntegratedGameStateSnapshot
            WithDayState(
                IntegratedGameStateSnapshot source,
                string state,
                int elapsed,
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
                    source.CheckoutStation.StationId,
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
                    elapsed,
                    source.DayCycle
                        .AutoBeginClosing),
                source.LedgerEntries);
        }

        public static string TempDirectory()
        {
            string path =
                Path.Combine(
                    Path.GetTempPath(),
                    "CC_S16_P1_" +
                    Guid.NewGuid().ToString("N"));

            Directory.CreateDirectory(path);
            return path;
        }

        public static void DeleteDirectory(
            string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(
                    path,
                    true);
            }
        }

        internal sealed class FixedClock :
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
