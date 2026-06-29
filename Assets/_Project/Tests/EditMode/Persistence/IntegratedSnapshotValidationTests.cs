using System;
using System.Collections.Generic;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.Persistence;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Persistence
{
    public sealed class IntegratedSnapshotValidationTests
    {
        private static IntegratedGameStateSnapshot Build(
            IEnumerable<InventoryContainerSaveRecord> inventories = null,
            IEnumerable<DisplaySaveRecord> displays = null,
            IEnumerable<CustomerSaveRecord> customers = null,
            IEnumerable<ShoppingSessionSaveRecord> sessions = null,
            IEnumerable<ReservationSaveRecord> reservations = null,
            IEnumerable<CheckoutQueueEntrySaveRecord> queue = null,
            CheckoutStationSaveRecord station = null,
            IEnumerable<CheckoutTransactionSaveRecord> transactions = null,
            DayCycleSaveRecord day = null,
            IEnumerable<EconomyLedgerSaveRecord> ledger = null,
            int schemaVersion =
                IntegratedGameStateSnapshot.CurrentSchemaVersion,
            DateTime? createdUtc = null,
            DateTime? updatedUtc = null,
            int currentDay = 3,
            string currency = "EUR")
        {
            IntegratedGameStateSnapshot valid =
                PersistenceTestFactory.ClosedSnapshot();

            return new IntegratedGameStateSnapshot(
                schemaVersion,
                valid.SessionId,
                valid.SlotId,
                createdUtc ?? valid.CreatedUtc,
                updatedUtc ?? valid.UpdatedUtc,
                currentDay,
                valid.CashCents,
                currency,
                inventories ?? valid.Inventories,
                valid.SupplierOrders,
                displays ?? valid.Displays,
                customers ?? valid.Customers,
                sessions ?? valid.ShoppingSessions,
                reservations ?? valid.Reservations,
                queue ?? valid.QueueEntries,
                station ?? valid.CheckoutStation,
                transactions ?? valid.Transactions,
                day ?? valid.DayCycle,
                ledger ?? valid.LedgerEntries);
        }

        [Test] public void ClosedSnapshot_IsValid()
        {
            Assert.That(
                PersistenceTestFactory
                    .ClosedSnapshot()
                    .TotalRecordCount,
                Is.EqualTo(12));
        }

        [Test] public void OpenSnapshot_IsValid()
        {
            IntegratedGameStateSnapshot snapshot =
                PersistenceTestFactory.OpenSnapshot();

            Assert.That(
                snapshot.QueueEntries.Count,
                Is.EqualTo(1));
            Assert.That(
                snapshot.CheckoutStation.State,
                Is.EqualTo("Busy"));
        }

        [Test] public void Snapshot_RejectsUnsupportedSchema()
        {
            Assert.Throws<
                System.ArgumentOutOfRangeException>(
                () => Build(schemaVersion: 99));
        }

        [Test] public void Snapshot_RejectsNonUtcCreatedTime()
        {
            Assert.Throws<System.ArgumentException>(
                () => Build(
                    createdUtc:
                        DateTime.SpecifyKind(
                            PersistenceTestFactory.CreatedUtc,
                            DateTimeKind.Local)));
        }

        [Test] public void Snapshot_RejectsUpdatedBeforeCreated()
        {
            Assert.Throws<System.ArgumentException>(
                () => Build(
                    updatedUtc:
                        PersistenceTestFactory.CreatedUtc
                            .AddSeconds(-1)));
        }

        [Test] public void Snapshot_RejectsZeroCurrentDay()
        {
            Assert.Throws<
                System.ArgumentOutOfRangeException>(
                () => Build(currentDay: 0));
        }

        [Test] public void Snapshot_RejectsInvalidCurrency()
        {
            Assert.Throws<System.ArgumentException>(
                () => Build(currency: "EU"));
        }

        [Test] public void Snapshot_RejectsDuplicateInventory()
        {
            var inventory =
                new InventoryContainerSaveRecord(
                    "inventory",
                    1,
                    new ProductQuantitySaveRecord[0]);

            Assert.Throws<System.ArgumentException>(
                () => Build(
                    inventories:
                        new[] { inventory, inventory }));
        }

        [Test] public void Snapshot_RejectsMissingDisplayInventory()
        {
            Assert.Throws<System.ArgumentException>(
                () => Build(
                    displays:
                        new[]
                        {
                            new DisplaySaveRecord(
                                "display-a",
                                "definition-a",
                                "product-a",
                                "missing")
                        }));
        }

        [Test] public void Snapshot_RejectsDuplicateCustomer()
        {
            var customer =
                new CustomerSaveRecord(
                    "customer-a",
                    "profile-a",
                    "Despawned",
                    0,
                    0);

            Assert.Throws<System.ArgumentException>(
                () => Build(
                    customers:
                        new[] { customer, customer }));
        }

        [Test] public void Snapshot_RejectsSessionMissingCustomer()
        {
            Assert.Throws<System.ArgumentException>(
                () => Build(
                    sessions:
                        new[]
                        {
                            new ShoppingSessionSaveRecord(
                                "missing",
                                "intent",
                                "cart",
                                "CheckedOut",
                                3)
                        },
                    reservations:
                        new ReservationSaveRecord[0],
                    transactions:
                        new CheckoutTransactionSaveRecord[0]));
        }

        [Test] public void Snapshot_RejectsDuplicateCart()
        {
            var customers =
                new[]
                {
                    new CustomerSaveRecord(
                        "customer-a",
                        "profile",
                        "Despawned",
                        0,
                        0),
                    new CustomerSaveRecord(
                        "customer-b",
                        "profile",
                        "Despawned",
                        0,
                        0)
                };

            var sessions =
                new[]
                {
                    new ShoppingSessionSaveRecord(
                        "customer-a",
                        "intent-a",
                        "cart",
                        "CheckedOut",
                        3),
                    new ShoppingSessionSaveRecord(
                        "customer-b",
                        "intent-b",
                        "cart",
                        "CheckedOut",
                        3)
                };

            Assert.Throws<System.ArgumentException>(
                () => Build(
                    customers: customers,
                    sessions: sessions,
                    reservations:
                        new ReservationSaveRecord[0],
                    transactions:
                        new CheckoutTransactionSaveRecord[0]));
        }

        [Test] public void Snapshot_RejectsReservationMissingDisplay()
        {
            Assert.Throws<System.ArgumentException>(
                () => Build(
                    reservations:
                        new[]
                        {
                            new ReservationSaveRecord(
                                "reservation",
                                "customer-a",
                                "cart-a",
                                "missing",
                                "product-a",
                                1,
                                "Consumed")
                        }));
        }

        [Test] public void Snapshot_RejectsReservationCustomerMismatch()
        {
            Assert.Throws<System.ArgumentException>(
                () => Build(
                    reservations:
                        new[]
                        {
                            new ReservationSaveRecord(
                                "reservation",
                                "other-customer",
                                "cart-a",
                                "display-a",
                                "product-a",
                                1,
                                "Consumed")
                        }));
        }

        [Test] public void Snapshot_RejectsDuplicateQueuePosition()
        {
            IntegratedGameStateSnapshot open =
                PersistenceTestFactory.OpenSnapshot();

            Assert.Throws<System.ArgumentException>(
                () => new IntegratedGameStateSnapshot(
                    open.SchemaVersion,
                    open.SessionId,
                    open.SlotId,
                    open.CreatedUtc,
                    open.UpdatedUtc,
                    open.CurrentDay,
                    open.CashCents,
                    open.CurrencyCode,
                    open.Inventories,
                    open.SupplierOrders,
                    open.Displays,
                    open.Customers,
                    open.ShoppingSessions,
                    open.Reservations,
                    new[]
                    {
                        open.QueueEntries[0],
                        new CheckoutQueueEntrySaveRecord(
                            "entry-b",
                            "customer-a",
                            "cart-a",
                            "Waiting",
                            1)
                    },
                    open.CheckoutStation,
                    open.Transactions,
                    open.DayCycle,
                    open.LedgerEntries));
        }

        [Test] public void Snapshot_RejectsNonContiguousQueue()
        {
            IntegratedGameStateSnapshot open =
                PersistenceTestFactory.OpenSnapshot();

            Assert.Throws<System.ArgumentException>(
                () => new IntegratedGameStateSnapshot(
                    open.SchemaVersion,
                    open.SessionId,
                    open.SlotId,
                    open.CreatedUtc,
                    open.UpdatedUtc,
                    open.CurrentDay,
                    open.CashCents,
                    open.CurrencyCode,
                    open.Inventories,
                    open.SupplierOrders,
                    open.Displays,
                    open.Customers,
                    open.ShoppingSessions,
                    open.Reservations,
                    new[]
                    {
                        new CheckoutQueueEntrySaveRecord(
                            "entry-a",
                            "customer-a",
                            "cart-a",
                            "Processing",
                            2)
                    },
                    new CheckoutStationSaveRecord(
                        "station-a",
                        "Busy",
                        "entry-a"),
                    open.Transactions,
                    open.DayCycle,
                    open.LedgerEntries));
        }

        [Test] public void Snapshot_RejectsBusyStationWithoutEntry()
        {
            IntegratedGameStateSnapshot open =
                PersistenceTestFactory.OpenSnapshot();

            Assert.Throws<System.ArgumentException>(
                () => new IntegratedGameStateSnapshot(
                    open.SchemaVersion,
                    open.SessionId,
                    open.SlotId,
                    open.CreatedUtc,
                    open.UpdatedUtc,
                    open.CurrentDay,
                    open.CashCents,
                    open.CurrencyCode,
                    open.Inventories,
                    open.SupplierOrders,
                    open.Displays,
                    open.Customers,
                    open.ShoppingSessions,
                    open.Reservations,
                    open.QueueEntries,
                    new CheckoutStationSaveRecord(
                        "station-a",
                        "Busy",
                        "missing"),
                    open.Transactions,
                    open.DayCycle,
                    open.LedgerEntries));
        }

        [Test] public void Snapshot_RejectsIdleStationWithEntryReference()
        {
            IntegratedGameStateSnapshot open =
                PersistenceTestFactory.OpenSnapshot();

            Assert.Throws<System.ArgumentException>(
                () => new IntegratedGameStateSnapshot(
                    open.SchemaVersion,
                    open.SessionId,
                    open.SlotId,
                    open.CreatedUtc,
                    open.UpdatedUtc,
                    open.CurrentDay,
                    open.CashCents,
                    open.CurrencyCode,
                    open.Inventories,
                    open.SupplierOrders,
                    open.Displays,
                    open.Customers,
                    open.ShoppingSessions,
                    open.Reservations,
                    open.QueueEntries,
                    new CheckoutStationSaveRecord(
                        "station-a",
                        "Available",
                        "entry-a"),
                    open.Transactions,
                    open.DayCycle,
                    open.LedgerEntries));
        }

        [Test] public void Snapshot_RejectsTransactionMissingCart()
        {
            Assert.Throws<System.ArgumentException>(
                () => Build(
                    transactions:
                        new[]
                        {
                            new CheckoutTransactionSaveRecord(
                                "transaction",
                                "customer-a",
                                "missing",
                                "station-a",
                                "Completed",
                                1,
                                1)
                        }));
        }

        [Test] public void ClosedSnapshot_RejectsActiveCustomer()
        {
            Assert.Throws<System.ArgumentException>(
                () => Build(
                    customers:
                        new[]
                        {
                            new CustomerSaveRecord(
                                "customer-a",
                                "profile-a",
                                "Browsing",
                                10,
                                1)
                        }));
        }
    }
}
