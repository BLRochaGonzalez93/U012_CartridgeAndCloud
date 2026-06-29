using System;
using System.IO;
using VRMGames.CartridgeAndCloud.Application.GameSession;
using VRMGames.CartridgeAndCloud.Application.UIUX;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;
using VRMGames.CartridgeAndCloud.Domain.Persistence;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.UIUX
{
    internal static class UIUXTestFactory
    {
        public static SaveSlotId Slot(int value = 0)
        {
            return new SaveSlotId(value);
        }

        public static DateTime Utc(
            int minute = 0)
        {
            return new DateTime(
                2026,
                6,
                29,
                12,
                minute,
                0,
                DateTimeKind.Utc);
        }

        public static IntegratedGameStateSnapshot
            EmptySnapshot(
                SaveSlotId? slot = null,
                string state = "BeforeOpen",
                int elapsed = 0,
                long cash = 100000)
        {
            return new IntegratedGameStateSnapshot(
                IntegratedGameStateSnapshot
                    .CurrentSchemaVersion,
                StableId.Parse(
                    "55555555555555555555555555555555"),
                slot ?? Slot(),
                Utc(),
                Utc(1),
                1,
                cash,
                "EUR",
                new[]
                {
                    new InventoryContainerSaveRecord(
                        "store-inventory",
                        100,
                        new ProductQuantitySaveRecord[0]),
                    new InventoryContainerSaveRecord(
                        "backroom-inventory",
                        200,
                        new ProductQuantitySaveRecord[0])
                },
                new SupplierOrderSaveRecord[0],
                new DisplaySaveRecord[0],
                new CustomerSaveRecord[0],
                new ShoppingSessionSaveRecord[0],
                new ReservationSaveRecord[0],
                new CheckoutQueueEntrySaveRecord[0],
                new CheckoutStationSaveRecord(
                    "station",
                    "Closed",
                    string.Empty),
                new CheckoutTransactionSaveRecord[0],
                new DayCycleSaveRecord(
                    "day-001",
                    state,
                    300,
                    elapsed,
                    true),
                new EconomyLedgerSaveRecord[0]);
        }

        public static IntegratedGameStateSnapshot
            PopulatedSnapshot(
                string state = "Open",
                int elapsed = 100)
        {
            return new IntegratedGameStateSnapshot(
                IntegratedGameStateSnapshot
                    .CurrentSchemaVersion,
                StableId.Parse(
                    "66666666666666666666666666666666"),
                Slot(),
                Utc(),
                Utc(2),
                2,
                250000,
                "EUR",
                new[]
                {
                    new InventoryContainerSaveRecord(
                        "store-inventory",
                        20,
                        new[]
                        {
                            new ProductQuantitySaveRecord(
                                "product-a",
                                7)
                        }),
                    new InventoryContainerSaveRecord(
                        "backroom-inventory",
                        30,
                        new[]
                        {
                            new ProductQuantitySaveRecord(
                                "product-b",
                                9)
                        })
                },
                new[]
                {
                    new SupplierOrderSaveRecord(
                        "order-a",
                        "supplier-a",
                        "product-a",
                        "Received",
                        3,
                        3,
                        1200)
                },
                new[]
                {
                    new DisplaySaveRecord(
                        "display-a",
                        "display-definition",
                        "product-a",
                        "store-inventory")
                },
                new[]
                {
                    new CustomerSaveRecord(
                        "customer-a",
                        "profile-a",
                        "Browsing",
                        20,
                        1)
                },
                new[]
                {
                    new ShoppingSessionSaveRecord(
                        "customer-a",
                        "intent-a",
                        "cart-a",
                        "ReadyForCheckout",
                        3)
                },
                new[]
                {
                    new ReservationSaveRecord(
                        "reservation-a",
                        "customer-a",
                        "cart-a",
                        "display-a",
                        "product-a",
                        1,
                        "Active")
                },
                new[]
                {
                    new CheckoutQueueEntrySaveRecord(
                        "entry-a",
                        "customer-a",
                        "cart-a",
                        "Processing",
                        1)
                },
                new CheckoutStationSaveRecord(
                    "station",
                    "Busy",
                    "entry-a"),
                new CheckoutTransactionSaveRecord[0],
                new DayCycleSaveRecord(
                    "day-002",
                    state,
                    300,
                    elapsed,
                    true),
                new[]
                {
                    new EconomyLedgerSaveRecord(
                        "ledger-revenue",
                        "CheckoutRevenue",
                        "transaction-old",
                        "day-002",
                        2999,
                        "EUR"),
                    new EconomyLedgerSaveRecord(
                        "ledger-cost",
                        "SupplierReceivingCost",
                        "receipt-a",
                        "day-002",
                        1200,
                        "EUR")
                });
        }

        public static IntegratedGameStateSnapshot
            ClosedSnapshot(
                SaveSlotId? slot = null,
                long cash = 100000)
        {
            IntegratedGameStateSnapshot source =
                EmptySnapshot(
                    slot,
                    "Closed",
                    300,
                    cash);

            return source;
        }

        public static string TempDirectory()
        {
            string path =
                Path.Combine(
                    Path.GetTempPath(),
                    "CC_S15_" +
                    Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(path);
            return path;
        }

        public static void DeleteDirectory(
            string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }

        internal sealed class FixedClock :
            IUtcClock
        {
            public DateTime UtcNow { get; set; }

            public FixedClock(DateTime value)
            {
                UtcNow = value;
            }
        }
    }
}
