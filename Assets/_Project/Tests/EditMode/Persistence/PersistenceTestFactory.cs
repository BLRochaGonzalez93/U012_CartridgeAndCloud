using System;
using System.IO;
using VRMGames.CartridgeAndCloud.Application.GameSession;
using VRMGames.CartridgeAndCloud.Application.Persistence;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;
using VRMGames.CartridgeAndCloud.Domain.Persistence;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Persistence
{
    internal static class PersistenceTestFactory
    {
        public static SaveSlotId Slot(int value = 0)
        {
            return new SaveSlotId(value);
        }

        public static StableId SessionId =>
            StableId.Parse(
                "33333333333333333333333333333333");

        public static DateTime CreatedUtc =>
            new DateTime(
                2026,
                6,
                29,
                8,
                0,
                0,
                DateTimeKind.Utc);

        public static DateTime UpdatedUtc =>
            CreatedUtc.AddMinutes(10);

        public static string CreateTempDirectory()
        {
            string path = Path.Combine(
                Path.GetTempPath(),
                "CC_S14_" +
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

        public static IntegratedGameStateSnapshot
            ClosedSnapshot(
                SaveSlotId? slot = null,
                long cashCents = 1000,
                DateTime? updatedUtc = null)
        {
            return new IntegratedGameStateSnapshot(
                IntegratedGameStateSnapshot
                    .CurrentSchemaVersion,
                SessionId,
                slot ?? Slot(),
                CreatedUtc,
                updatedUtc ?? UpdatedUtc,
                3,
                cashCents,
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
                                5)
                        }),
                    new InventoryContainerSaveRecord(
                        "backroom-inventory",
                        30,
                        new ProductQuantitySaveRecord[0])
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
                        "definition-a",
                        "product-a",
                        "store-inventory")
                },
                new[]
                {
                    new CustomerSaveRecord(
                        "customer-a",
                        "profile-a",
                        "Despawned",
                        0,
                        2)
                },
                new[]
                {
                    new ShoppingSessionSaveRecord(
                        "customer-a",
                        "intent-a",
                        "cart-a",
                        "CheckedOut",
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
                        "Consumed")
                },
                new CheckoutQueueEntrySaveRecord[0],
                new CheckoutStationSaveRecord(
                    "station-a",
                    "Closed",
                    string.Empty),
                new[]
                {
                    new CheckoutTransactionSaveRecord(
                        "transaction-a",
                        "customer-a",
                        "cart-a",
                        "station-a",
                        "Completed",
                        1,
                        1)
                },
                new DayCycleSaveRecord(
                    "day-003",
                    "Closed",
                    300,
                    300,
                    true),
                new[]
                {
                    new EconomyLedgerSaveRecord(
                        "ledger-revenue-a",
                        "CheckoutRevenue",
                        "transaction-a",
                        "day-003",
                        2999,
                        "EUR"),
                    new EconomyLedgerSaveRecord(
                        "ledger-cost-a",
                        "SupplierReceivingCost",
                        "receipt-a",
                        "day-003",
                        1200,
                        "EUR")
                });
        }

        public static IntegratedGameStateSnapshot
            OpenSnapshot(
                SaveSlotId? slot = null,
                long cashCents = 1000)
        {
            return new IntegratedGameStateSnapshot(
                IntegratedGameStateSnapshot
                    .CurrentSchemaVersion,
                SessionId,
                slot ?? Slot(),
                CreatedUtc,
                UpdatedUtc,
                2,
                cashCents,
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
                                4)
                        })
                },
                new SupplierOrderSaveRecord[0],
                new[]
                {
                    new DisplaySaveRecord(
                        "display-a",
                        "definition-a",
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
                    "station-a",
                    "Busy",
                    "entry-a"),
                new CheckoutTransactionSaveRecord[0],
                new DayCycleSaveRecord(
                    "day-002",
                    "Open",
                    300,
                    100,
                    true),
                new EconomyLedgerSaveRecord[0]);
        }

        internal sealed class FixedClock :
            IUtcClock
        {
            public DateTime UtcNow { get; set; }

            public FixedClock(DateTime utcNow)
            {
                UtcNow = utcNow;
            }
        }

        internal sealed class CaptureSource :
            IIntegratedGameStateCaptureSource
        {
            private readonly Func<
                SaveSlotId,
                DateTime,
                IntegratedGameStateSnapshot> _capture;

            public CaptureSource(
                Func<
                    SaveSlotId,
                    DateTime,
                    IntegratedGameStateSnapshot> capture)
            {
                _capture = capture;
            }

            public IntegratedGameStateSnapshot Capture(
                SaveSlotId slotId,
                DateTime updatedUtc)
            {
                return _capture(slotId, updatedUtc);
            }
        }
    }
}
