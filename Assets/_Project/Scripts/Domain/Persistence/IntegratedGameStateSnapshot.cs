using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;

namespace VRMGames.CartridgeAndCloud.Domain.Persistence
{
    public sealed class IntegratedGameStateSnapshot
    {
        public const int CurrentSchemaVersion = 2;

        private readonly ReadOnlyCollection<
            InventoryContainerSaveRecord> _inventories;
        private readonly ReadOnlyCollection<
            SupplierOrderSaveRecord> _supplierOrders;
        private readonly ReadOnlyCollection<
            DisplaySaveRecord> _displays;
        private readonly ReadOnlyCollection<
            CustomerSaveRecord> _customers;
        private readonly ReadOnlyCollection<
            ShoppingSessionSaveRecord> _shoppingSessions;
        private readonly ReadOnlyCollection<
            ReservationSaveRecord> _reservations;
        private readonly ReadOnlyCollection<
            CheckoutQueueEntrySaveRecord> _queueEntries;
        private readonly ReadOnlyCollection<
            CheckoutTransactionSaveRecord> _transactions;
        private readonly ReadOnlyCollection<
            EconomyLedgerSaveRecord> _ledgerEntries;

        public int SchemaVersion { get; }
        public StableId SessionId { get; }
        public SaveSlotId SlotId { get; }
        public DateTime CreatedUtc { get; }
        public DateTime UpdatedUtc { get; }
        public int CurrentDay { get; }
        public long CashCents { get; }
        public string CurrencyCode { get; }

        public IReadOnlyList<
            InventoryContainerSaveRecord> Inventories =>
                _inventories;

        public IReadOnlyList<
            SupplierOrderSaveRecord> SupplierOrders =>
                _supplierOrders;

        public IReadOnlyList<DisplaySaveRecord> Displays =>
            _displays;

        public IReadOnlyList<CustomerSaveRecord> Customers =>
            _customers;

        public IReadOnlyList<
            ShoppingSessionSaveRecord> ShoppingSessions =>
                _shoppingSessions;

        public IReadOnlyList<
            ReservationSaveRecord> Reservations =>
                _reservations;

        public IReadOnlyList<
            CheckoutQueueEntrySaveRecord> QueueEntries =>
                _queueEntries;

        public CheckoutStationSaveRecord CheckoutStation {
            get;
        }

        public IReadOnlyList<
            CheckoutTransactionSaveRecord> Transactions =>
                _transactions;

        public DayCycleSaveRecord DayCycle { get; }

        public IReadOnlyList<
            EconomyLedgerSaveRecord> LedgerEntries =>
                _ledgerEntries;

        public int TotalRecordCount =>
            _inventories.Count +
            _supplierOrders.Count +
            _displays.Count +
            _customers.Count +
            _shoppingSessions.Count +
            _reservations.Count +
            _queueEntries.Count +
            _transactions.Count +
            _ledgerEntries.Count +
            2;

        public IntegratedGameStateSnapshot(
            int schemaVersion,
            StableId sessionId,
            SaveSlotId slotId,
            DateTime createdUtc,
            DateTime updatedUtc,
            int currentDay,
            long cashCents,
            string currencyCode,
            IEnumerable<InventoryContainerSaveRecord>
                inventories,
            IEnumerable<SupplierOrderSaveRecord>
                supplierOrders,
            IEnumerable<DisplaySaveRecord> displays,
            IEnumerable<CustomerSaveRecord> customers,
            IEnumerable<ShoppingSessionSaveRecord>
                shoppingSessions,
            IEnumerable<ReservationSaveRecord>
                reservations,
            IEnumerable<CheckoutQueueEntrySaveRecord>
                queueEntries,
            CheckoutStationSaveRecord checkoutStation,
            IEnumerable<CheckoutTransactionSaveRecord>
                transactions,
            DayCycleSaveRecord dayCycle,
            IEnumerable<EconomyLedgerSaveRecord>
                ledgerEntries)
        {
            if (schemaVersion != CurrentSchemaVersion)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(schemaVersion),
                    schemaVersion,
                    $"Only schema version " +
                    $"{CurrentSchemaVersion} is supported.");
            }

            if (createdUtc.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException(
                    "CreatedUtc must use UTC.",
                    nameof(createdUtc));
            }

            if (updatedUtc.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException(
                    "UpdatedUtc must use UTC.",
                    nameof(updatedUtc));
            }

            if (updatedUtc < createdUtc)
            {
                throw new ArgumentException(
                    "UpdatedUtc cannot be earlier than CreatedUtc.",
                    nameof(updatedUtc));
            }

            if (currentDay < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(currentDay));
            }

            string normalizedCurrency =
                SaveRecordGuard.Required(
                    currencyCode,
                    nameof(currencyCode))
                .ToUpperInvariant();

            if (normalizedCurrency.Length != 3)
            {
                throw new ArgumentException(
                    "Currency code must contain three characters.",
                    nameof(currencyCode));
            }

            if (checkoutStation == null)
            {
                throw new ArgumentNullException(
                    nameof(checkoutStation));
            }

            if (dayCycle == null)
            {
                throw new ArgumentNullException(
                    nameof(dayCycle));
            }

            SchemaVersion = schemaVersion;
            SessionId = sessionId;
            SlotId = slotId;
            CreatedUtc = createdUtc;
            UpdatedUtc = updatedUtc;
            CurrentDay = currentDay;
            CashCents = cashCents;
            CurrencyCode = normalizedCurrency;
            _inventories =
                SaveRecordGuard.Copy(
                    inventories,
                    nameof(inventories));
            _supplierOrders =
                SaveRecordGuard.Copy(
                    supplierOrders,
                    nameof(supplierOrders));
            _displays =
                SaveRecordGuard.Copy(
                    displays,
                    nameof(displays));
            _customers =
                SaveRecordGuard.Copy(
                    customers,
                    nameof(customers));
            _shoppingSessions =
                SaveRecordGuard.Copy(
                    shoppingSessions,
                    nameof(shoppingSessions));
            _reservations =
                SaveRecordGuard.Copy(
                    reservations,
                    nameof(reservations));
            _queueEntries =
                SaveRecordGuard.Copy(
                    queueEntries,
                    nameof(queueEntries));
            CheckoutStation = checkoutStation;
            _transactions =
                SaveRecordGuard.Copy(
                    transactions,
                    nameof(transactions));
            DayCycle = dayCycle;
            _ledgerEntries =
                SaveRecordGuard.Copy(
                    ledgerEntries,
                    nameof(ledgerEntries));

            Validate();
        }

        private void Validate()
        {
            HashSet<string> inventoryIds =
                UniqueIds(
                    _inventories,
                    item => item.ContainerId,
                    "inventory");

            HashSet<string> displayIds =
                UniqueIds(
                    _displays,
                    item => item.DisplayId,
                    "display");

            HashSet<string> customerIds =
                UniqueIds(
                    _customers,
                    item => item.CustomerId,
                    "customer");

            HashSet<string> orderIds =
                UniqueIds(
                    _supplierOrders,
                    item => item.OrderId,
                    "supplier order");

            if (orderIds.Count != _supplierOrders.Count)
            {
                throw new InvalidOperationException();
            }

            foreach (DisplaySaveRecord display in _displays)
            {
                if (!inventoryIds.Contains(
                        display.InventoryContainerId))
                {
                    throw new ArgumentException(
                        $"Display {display.DisplayId} references " +
                        $"missing inventory " +
                        $"{display.InventoryContainerId}.");
                }
            }

            Dictionary<string, ShoppingSessionSaveRecord>
                sessionsByCustomer =
                    new Dictionary<
                        string,
                        ShoppingSessionSaveRecord>(
                        StringComparer.Ordinal);

            Dictionary<string, ShoppingSessionSaveRecord>
                sessionsByCart =
                    new Dictionary<
                        string,
                        ShoppingSessionSaveRecord>(
                        StringComparer.Ordinal);

            foreach (ShoppingSessionSaveRecord session
                     in _shoppingSessions)
            {
                if (!customerIds.Contains(session.CustomerId))
                {
                    throw new ArgumentException(
                        $"Shopping session references missing " +
                        $"customer {session.CustomerId}.");
                }

                if (sessionsByCustomer.ContainsKey(
                        session.CustomerId))
                {
                    throw new ArgumentException(
                        $"Duplicate shopping customer " +
                        $"{session.CustomerId}.");
                }

                if (sessionsByCart.ContainsKey(session.CartId))
                {
                    throw new ArgumentException(
                        $"Duplicate cart {session.CartId}.");
                }

                sessionsByCustomer.Add(
                    session.CustomerId,
                    session);
                sessionsByCart.Add(
                    session.CartId,
                    session);
            }

            HashSet<string> reservationIds =
                new HashSet<string>(
                    StringComparer.Ordinal);

            foreach (ReservationSaveRecord reservation
                     in _reservations)
            {
                SaveRecordGuard.Unique(
                    reservationIds,
                    reservation.ReservationId,
                    "reservation");

                if (!customerIds.Contains(
                        reservation.CustomerId))
                {
                    throw new ArgumentException(
                        $"Reservation references missing customer " +
                        $"{reservation.CustomerId}.");
                }

                if (!displayIds.Contains(reservation.DisplayId))
                {
                    throw new ArgumentException(
                        $"Reservation references missing display " +
                        $"{reservation.DisplayId}.");
                }

                if (!sessionsByCart.TryGetValue(
                        reservation.CartId,
                        out ShoppingSessionSaveRecord session) ||
                    !string.Equals(
                        session.CustomerId,
                        reservation.CustomerId,
                        StringComparison.Ordinal))
                {
                    throw new ArgumentException(
                        $"Reservation {reservation.ReservationId} " +
                        $"has no matching shopping session.");
                }
            }

            Dictionary<string, CheckoutQueueEntrySaveRecord>
                queueById =
                    new Dictionary<
                        string,
                        CheckoutQueueEntrySaveRecord>(
                        StringComparer.Ordinal);
            HashSet<int> positions = new HashSet<int>();

            foreach (CheckoutQueueEntrySaveRecord entry
                     in _queueEntries)
            {
                if (queueById.ContainsKey(entry.EntryId))
                {
                    throw new ArgumentException(
                        $"Duplicate queue entry {entry.EntryId}.");
                }

                if (!positions.Add(entry.Position))
                {
                    throw new ArgumentException(
                        $"Duplicate queue position {entry.Position}.");
                }

                if (!sessionsByCart.TryGetValue(
                        entry.CartId,
                        out ShoppingSessionSaveRecord session) ||
                    !string.Equals(
                        session.CustomerId,
                        entry.CustomerId,
                        StringComparison.Ordinal))
                {
                    throw new ArgumentException(
                        $"Queue entry {entry.EntryId} " +
                        $"has no matching shopping session.");
                }

                queueById.Add(entry.EntryId, entry);
            }

            for (int expected = 1;
                 expected <= positions.Count;
                 expected++)
            {
                if (!positions.Contains(expected))
                {
                    throw new ArgumentException(
                        "Queue positions must be contiguous and 1-based.");
                }
            }

            bool stationBusy =
                string.Equals(
                    CheckoutStation.State,
                    "Busy",
                    StringComparison.Ordinal);

            if (stationBusy)
            {
                if (string.IsNullOrWhiteSpace(
                        CheckoutStation.CurrentEntryId) ||
                    !queueById.TryGetValue(
                        CheckoutStation.CurrentEntryId,
                        out CheckoutQueueEntrySaveRecord current) ||
                    !string.Equals(
                        current.State,
                        "Processing",
                        StringComparison.Ordinal))
                {
                    throw new ArgumentException(
                        "Busy station must reference a processing queue entry.");
                }
            }
            else if (!string.IsNullOrWhiteSpace(
                         CheckoutStation.CurrentEntryId))
            {
                throw new ArgumentException(
                    "Non-busy station cannot reference a queue entry.");
            }

            HashSet<string> transactionIds =
                new HashSet<string>(
                    StringComparer.Ordinal);

            foreach (CheckoutTransactionSaveRecord transaction
                     in _transactions)
            {
                SaveRecordGuard.Unique(
                    transactionIds,
                    transaction.TransactionId,
                    "checkout transaction");

                if (!customerIds.Contains(transaction.CustomerId))
                {
                    throw new ArgumentException(
                        $"Transaction references missing customer " +
                        $"{transaction.CustomerId}.");
                }

                if (!sessionsByCart.ContainsKey(
                        transaction.CartId))
                {
                    throw new ArgumentException(
                        $"Transaction references missing cart " +
                        $"{transaction.CartId}.");
                }

                if (!string.Equals(
                        transaction.StationId,
                        CheckoutStation.StationId,
                        StringComparison.Ordinal))
                {
                    throw new ArgumentException(
                        "Transaction references another station.");
                }
            }

            HashSet<string> ledgerIds =
                new HashSet<string>(
                    StringComparer.Ordinal);
            HashSet<string> postingKeys =
                new HashSet<string>(
                    StringComparer.Ordinal);

            foreach (EconomyLedgerSaveRecord entry
                     in _ledgerEntries)
            {
                SaveRecordGuard.Unique(
                    ledgerIds,
                    entry.EntryId,
                    "ledger entry");

                string postingKey =
                    entry.PostingType + ":" +
                    entry.SourceId;

                SaveRecordGuard.Unique(
                    postingKeys,
                    postingKey,
                    "ledger posting");

                if (!string.Equals(
                        entry.DayId,
                        DayCycle.DayId,
                        StringComparison.Ordinal))
                {
                    throw new ArgumentException(
                        "Ledger entry references another day.");
                }

                if (!string.Equals(
                        entry.CurrencyCode,
                        CurrencyCode,
                        StringComparison.Ordinal))
                {
                    throw new ArgumentException(
                        "Ledger currency mismatch.");
                }
            }

            ValidateClosedDay();
        }

        private void ValidateClosedDay()
        {
            if (!string.Equals(
                    DayCycle.State,
                    "Closed",
                    StringComparison.Ordinal))
            {
                return;
            }

            foreach (CustomerSaveRecord customer in _customers)
            {
                if (!string.Equals(
                        customer.State,
                        "Despawned",
                        StringComparison.Ordinal))
                {
                    throw new ArgumentException(
                        "Closed day cannot contain active customers.");
                }
            }

            if (_queueEntries.Count > 0)
            {
                throw new ArgumentException(
                    "Closed day cannot contain queue entries.");
            }

            if (string.Equals(
                    CheckoutStation.State,
                    "Busy",
                    StringComparison.Ordinal))
            {
                throw new ArgumentException(
                    "Closed day cannot contain a busy station.");
            }

            foreach (ReservationSaveRecord reservation
                     in _reservations)
            {
                if (string.Equals(
                        reservation.State,
                        "Active",
                        StringComparison.Ordinal))
                {
                    throw new ArgumentException(
                        "Closed day cannot contain active reservations.");
                }
            }

            foreach (ShoppingSessionSaveRecord session
                     in _shoppingSessions)
            {
                if (!string.Equals(
                        session.State,
                        "Abandoned",
                        StringComparison.Ordinal) &&
                    !string.Equals(
                        session.State,
                        "CheckedOut",
                        StringComparison.Ordinal))
                {
                    throw new ArgumentException(
                        "Closed day cannot contain pending shopping sessions.");
                }
            }
        }

        private static HashSet<string> UniqueIds<T>(
            IEnumerable<T> records,
            Func<T, string> getId,
            string label)
        {
            HashSet<string> ids =
                new HashSet<string>(
                    StringComparer.Ordinal);

            foreach (T record in records)
            {
                SaveRecordGuard.Unique(
                    ids,
                    getId(record),
                    label);
            }

            return ids;
        }
    }
}
