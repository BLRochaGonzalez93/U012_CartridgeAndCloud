using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Application.Persistence;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;
using VRMGames.CartridgeAndCloud.Domain.Persistence;

namespace VRMGames.CartridgeAndCloud.Infrastructure.Persistence
{
    public sealed class IntegratedSaveFormatException :
        Exception
    {
        public IntegratedSaveRepositoryStatus Status { get; }

        public IntegratedSaveFormatException(
            IntegratedSaveRepositoryStatus status,
            string message)
            : base(message)
        {
            Status = status;
        }
    }

    public sealed class IntegratedSaveJsonCodec
    {
        public string Encode(
            IntegratedGameStateSnapshot snapshot,
            long generation)
        {
            if (snapshot == null)
            {
                throw new ArgumentNullException(nameof(snapshot));
            }

            if (generation < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(generation));
            }

            SnapshotDto payload =
                SnapshotDto.FromSnapshot(snapshot);
            string payloadJson =
                JsonUtility.ToJson(payload, false);
            string checksum =
                ComputeSha256(payloadJson);

            EnvelopeDto envelope =
                new EnvelopeDto
                {
                    schemaVersion =
                        IntegratedGameStateSnapshot
                            .CurrentSchemaVersion,
                    slot = snapshot.SlotId.Value,
                    generation = generation,
                    updatedUtcTicks =
                        snapshot.UpdatedUtc.Ticks,
                    payloadJson = payloadJson,
                    payloadSha256 = checksum
                };

            return JsonUtility.ToJson(envelope, true);
        }

        public IntegratedGameStateSnapshot Decode(
            string json,
            SaveSlotId expectedSlot,
            out long generation)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                throw new IntegratedSaveFormatException(
                    IntegratedSaveRepositoryStatus
                        .ValidationFailure,
                    "Save file is empty.");
            }

            EnvelopeDto envelope;

            try
            {
                envelope =
                    JsonUtility.FromJson<EnvelopeDto>(
                        json);
            }
            catch (Exception exception)
            {
                throw new IntegratedSaveFormatException(
                    IntegratedSaveRepositoryStatus
                        .ValidationFailure,
                    "Save envelope could not be deserialized: " +
                    exception.Message);
            }

            if (envelope == null)
            {
                throw new IntegratedSaveFormatException(
                    IntegratedSaveRepositoryStatus
                        .ValidationFailure,
                    "Save envelope is null.");
            }

            if (envelope.schemaVersion !=
                IntegratedGameStateSnapshot
                    .CurrentSchemaVersion)
            {
                throw new IntegratedSaveFormatException(
                    IntegratedSaveRepositoryStatus
                        .UnsupportedSchema,
                    $"Unsupported schema " +
                    $"{envelope.schemaVersion}.");
            }

            if (envelope.slot != expectedSlot.Value)
            {
                throw new IntegratedSaveFormatException(
                    IntegratedSaveRepositoryStatus
                        .ValidationFailure,
                    $"Save slot mismatch. Expected " +
                    $"{expectedSlot.Value}, got " +
                    $"{envelope.slot}.");
            }

            if (envelope.generation < 1)
            {
                throw new IntegratedSaveFormatException(
                    IntegratedSaveRepositoryStatus
                        .ValidationFailure,
                    "Save generation must be positive.");
            }

            if (string.IsNullOrWhiteSpace(
                    envelope.payloadJson) ||
                string.IsNullOrWhiteSpace(
                    envelope.payloadSha256))
            {
                throw new IntegratedSaveFormatException(
                    IntegratedSaveRepositoryStatus
                        .ValidationFailure,
                    "Save payload or checksum is missing.");
            }

            string actualChecksum =
                ComputeSha256(envelope.payloadJson);

            if (!string.Equals(
                    actualChecksum,
                    envelope.payloadSha256,
                    StringComparison.OrdinalIgnoreCase))
            {
                throw new IntegratedSaveFormatException(
                    IntegratedSaveRepositoryStatus
                        .ValidationFailure,
                    "Save payload checksum mismatch.");
            }

            SnapshotDto payload;

            try
            {
                payload =
                    JsonUtility.FromJson<SnapshotDto>(
                        envelope.payloadJson);
            }
            catch (Exception exception)
            {
                throw new IntegratedSaveFormatException(
                    IntegratedSaveRepositoryStatus
                        .ValidationFailure,
                    "Save payload could not be deserialized: " +
                    exception.Message);
            }

            if (payload == null)
            {
                throw new IntegratedSaveFormatException(
                    IntegratedSaveRepositoryStatus
                        .ValidationFailure,
                    "Save payload is null.");
            }

            IntegratedGameStateSnapshot snapshot;

            try
            {
                snapshot = payload.ToSnapshot();
            }
            catch (Exception exception)
            {
                throw new IntegratedSaveFormatException(
                    IntegratedSaveRepositoryStatus
                        .ValidationFailure,
                    "Save payload validation failed: " +
                    exception.Message);
            }

            if (snapshot.SlotId != expectedSlot)
            {
                throw new IntegratedSaveFormatException(
                    IntegratedSaveRepositoryStatus
                        .ValidationFailure,
                    "Payload slot does not match the requested slot.");
            }

            if (snapshot.UpdatedUtc.Ticks !=
                envelope.updatedUtcTicks)
            {
                throw new IntegratedSaveFormatException(
                    IntegratedSaveRepositoryStatus
                        .ValidationFailure,
                    "Envelope timestamp does not match payload.");
            }

            generation = envelope.generation;
            return snapshot;
        }

        public long ReadGeneration(
            string json,
            SaveSlotId expectedSlot)
        {
            Decode(
                json,
                expectedSlot,
                out long generation);
            return generation;
        }

        private static string ComputeSha256(
            string value)
        {
            byte[] bytes =
                Encoding.UTF8.GetBytes(value);

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hash =
                    sha256.ComputeHash(bytes);
                StringBuilder builder =
                    new StringBuilder(
                        hash.Length * 2);

                foreach (byte item in hash)
                {
                    builder.Append(
                        item.ToString("x2"));
                }

                return builder.ToString();
            }
        }

        [Serializable]
        private sealed class EnvelopeDto
        {
            public int schemaVersion;
            public int slot;
            public long generation;
            public long updatedUtcTicks;
            public string payloadJson;
            public string payloadSha256;
        }

        [Serializable]
        private sealed class SnapshotDto
        {
            public int schemaVersion;
            public string sessionId;
            public int slot;
            public long createdUtcTicks;
            public long updatedUtcTicks;
            public int currentDay;
            public long cashCents;
            public string currencyCode;
            public List<InventoryDto> inventories =
                new List<InventoryDto>();
            public List<SupplierOrderDto> supplierOrders =
                new List<SupplierOrderDto>();
            public List<DisplayDto> displays =
                new List<DisplayDto>();
            public List<CustomerDto> customers =
                new List<CustomerDto>();
            public List<ShoppingSessionDto> shoppingSessions =
                new List<ShoppingSessionDto>();
            public List<ReservationDto> reservations =
                new List<ReservationDto>();
            public List<QueueEntryDto> queueEntries =
                new List<QueueEntryDto>();
            public StationDto checkoutStation;
            public List<TransactionDto> transactions =
                new List<TransactionDto>();
            public DayCycleDto dayCycle;
            public List<LedgerEntryDto> ledgerEntries =
                new List<LedgerEntryDto>();

            public static SnapshotDto FromSnapshot(
                IntegratedGameStateSnapshot snapshot)
            {
                SnapshotDto dto =
                    new SnapshotDto
                    {
                        schemaVersion =
                            snapshot.SchemaVersion,
                        sessionId =
                            snapshot.SessionId.Value,
                        slot = snapshot.SlotId.Value,
                        createdUtcTicks =
                            snapshot.CreatedUtc.Ticks,
                        updatedUtcTicks =
                            snapshot.UpdatedUtc.Ticks,
                        currentDay =
                            snapshot.CurrentDay,
                        cashCents =
                            snapshot.CashCents,
                        currencyCode =
                            snapshot.CurrencyCode,
                        checkoutStation =
                            StationDto.FromRecord(
                                snapshot.CheckoutStation),
                        dayCycle =
                            DayCycleDto.FromRecord(
                                snapshot.DayCycle)
                    };

                foreach (InventoryContainerSaveRecord item
                         in snapshot.Inventories)
                {
                    dto.inventories.Add(
                        InventoryDto.FromRecord(item));
                }

                foreach (SupplierOrderSaveRecord item
                         in snapshot.SupplierOrders)
                {
                    dto.supplierOrders.Add(
                        SupplierOrderDto.FromRecord(item));
                }

                foreach (DisplaySaveRecord item
                         in snapshot.Displays)
                {
                    dto.displays.Add(
                        DisplayDto.FromRecord(item));
                }

                foreach (CustomerSaveRecord item
                         in snapshot.Customers)
                {
                    dto.customers.Add(
                        CustomerDto.FromRecord(item));
                }

                foreach (ShoppingSessionSaveRecord item
                         in snapshot.ShoppingSessions)
                {
                    dto.shoppingSessions.Add(
                        ShoppingSessionDto.FromRecord(item));
                }

                foreach (ReservationSaveRecord item
                         in snapshot.Reservations)
                {
                    dto.reservations.Add(
                        ReservationDto.FromRecord(item));
                }

                foreach (CheckoutQueueEntrySaveRecord item
                         in snapshot.QueueEntries)
                {
                    dto.queueEntries.Add(
                        QueueEntryDto.FromRecord(item));
                }

                foreach (CheckoutTransactionSaveRecord item
                         in snapshot.Transactions)
                {
                    dto.transactions.Add(
                        TransactionDto.FromRecord(item));
                }

                foreach (EconomyLedgerSaveRecord item
                         in snapshot.LedgerEntries)
                {
                    dto.ledgerEntries.Add(
                        LedgerEntryDto.FromRecord(item));
                }

                return dto;
            }

            public IntegratedGameStateSnapshot ToSnapshot()
            {
                List<InventoryContainerSaveRecord>
                    inventoryRecords =
                        new List<
                            InventoryContainerSaveRecord>();
                List<SupplierOrderSaveRecord>
                    orderRecords =
                        new List<
                            SupplierOrderSaveRecord>();
                List<DisplaySaveRecord> displayRecords =
                    new List<DisplaySaveRecord>();
                List<CustomerSaveRecord> customerRecords =
                    new List<CustomerSaveRecord>();
                List<ShoppingSessionSaveRecord>
                    sessionRecords =
                        new List<
                            ShoppingSessionSaveRecord>();
                List<ReservationSaveRecord>
                    reservationRecords =
                        new List<ReservationSaveRecord>();
                List<CheckoutQueueEntrySaveRecord>
                    queueRecords =
                        new List<
                            CheckoutQueueEntrySaveRecord>();
                List<CheckoutTransactionSaveRecord>
                    transactionRecords =
                        new List<
                            CheckoutTransactionSaveRecord>();
                List<EconomyLedgerSaveRecord>
                    ledgerRecords =
                        new List<
                            EconomyLedgerSaveRecord>();

                foreach (InventoryDto item
                         in Require(inventories))
                {
                    inventoryRecords.Add(
                        item.ToRecord());
                }

                foreach (SupplierOrderDto item
                         in Require(supplierOrders))
                {
                    orderRecords.Add(
                        item.ToRecord());
                }

                foreach (DisplayDto item
                         in Require(displays))
                {
                    displayRecords.Add(
                        item.ToRecord());
                }

                foreach (CustomerDto item
                         in Require(customers))
                {
                    customerRecords.Add(
                        item.ToRecord());
                }

                foreach (ShoppingSessionDto item
                         in Require(shoppingSessions))
                {
                    sessionRecords.Add(
                        item.ToRecord());
                }

                foreach (ReservationDto item
                         in Require(reservations))
                {
                    reservationRecords.Add(
                        item.ToRecord());
                }

                foreach (QueueEntryDto item
                         in Require(queueEntries))
                {
                    queueRecords.Add(
                        item.ToRecord());
                }

                foreach (TransactionDto item
                         in Require(transactions))
                {
                    transactionRecords.Add(
                        item.ToRecord());
                }

                foreach (LedgerEntryDto item
                         in Require(ledgerEntries))
                {
                    ledgerRecords.Add(
                        item.ToRecord());
                }

                if (checkoutStation == null ||
                    dayCycle == null)
                {
                    throw new InvalidOperationException(
                        "Station and day-cycle records are required.");
                }

                return new IntegratedGameStateSnapshot(
                    schemaVersion,
                    StableId.Parse(sessionId),
                    new SaveSlotId(slot),
                    new DateTime(
                        createdUtcTicks,
                        DateTimeKind.Utc),
                    new DateTime(
                        updatedUtcTicks,
                        DateTimeKind.Utc),
                    currentDay,
                    cashCents,
                    currencyCode,
                    inventoryRecords,
                    orderRecords,
                    displayRecords,
                    customerRecords,
                    sessionRecords,
                    reservationRecords,
                    queueRecords,
                    checkoutStation.ToRecord(),
                    transactionRecords,
                    dayCycle.ToRecord(),
                    ledgerRecords);
            }

            private static List<T> Require<T>(
                List<T> values)
            {
                if (values == null)
                {
                    throw new InvalidOperationException(
                        "Save collection is missing.");
                }

                return values;
            }
        }

        [Serializable]
        private sealed class ProductQuantityDto
        {
            public string productId;
            public int quantity;

            public static ProductQuantityDto FromRecord(
                ProductQuantitySaveRecord record)
            {
                return new ProductQuantityDto
                {
                    productId = record.ProductId,
                    quantity = record.Quantity
                };
            }

            public ProductQuantitySaveRecord ToRecord()
            {
                return new ProductQuantitySaveRecord(
                    productId,
                    quantity);
            }
        }

        [Serializable]
        private sealed class InventoryDto
        {
            public string containerId;
            public int capacity;
            public List<ProductQuantityDto> products =
                new List<ProductQuantityDto>();

            public static InventoryDto FromRecord(
                InventoryContainerSaveRecord record)
            {
                InventoryDto dto =
                    new InventoryDto
                    {
                        containerId = record.ContainerId,
                        capacity = record.Capacity
                    };

                foreach (ProductQuantitySaveRecord product
                         in record.Products)
                {
                    dto.products.Add(
                        ProductQuantityDto.FromRecord(
                            product));
                }

                return dto;
            }

            public InventoryContainerSaveRecord ToRecord()
            {
                List<ProductQuantitySaveRecord> records =
                    new List<ProductQuantitySaveRecord>();

                if (products == null)
                {
                    throw new InvalidOperationException(
                        "Inventory products are missing.");
                }

                foreach (ProductQuantityDto product
                         in products)
                {
                    if (product == null)
                    {
                        throw new InvalidOperationException(
                            "Inventory product is null.");
                    }

                    records.Add(product.ToRecord());
                }

                return new InventoryContainerSaveRecord(
                    containerId,
                    capacity,
                    records);
            }
        }

        [Serializable]
        private sealed class SupplierOrderDto
        {
            public string orderId;
            public string supplierId;
            public string productId;
            public string state;
            public int orderedUnits;
            public int receivedUnits;
            public long unitCostCents;

            public static SupplierOrderDto FromRecord(
                SupplierOrderSaveRecord record)
            {
                return new SupplierOrderDto
                {
                    orderId = record.OrderId,
                    supplierId = record.SupplierId,
                    productId = record.ProductId,
                    state = record.State,
                    orderedUnits = record.OrderedUnits,
                    receivedUnits = record.ReceivedUnits,
                    unitCostCents = record.UnitCostCents
                };
            }

            public SupplierOrderSaveRecord ToRecord()
            {
                return new SupplierOrderSaveRecord(
                    orderId,
                    supplierId,
                    productId,
                    state,
                    orderedUnits,
                    receivedUnits,
                    unitCostCents);
            }
        }

        [Serializable]
        private sealed class DisplayDto
        {
            public string displayId;
            public string definitionId;
            public string assignedProductId;
            public string inventoryContainerId;

            public static DisplayDto FromRecord(
                DisplaySaveRecord record)
            {
                return new DisplayDto
                {
                    displayId = record.DisplayId,
                    definitionId = record.DefinitionId,
                    assignedProductId =
                        record.AssignedProductId,
                    inventoryContainerId =
                        record.InventoryContainerId
                };
            }

            public DisplaySaveRecord ToRecord()
            {
                return new DisplaySaveRecord(
                    displayId,
                    definitionId,
                    assignedProductId,
                    inventoryContainerId);
            }
        }

        [Serializable]
        private sealed class CustomerDto
        {
            public string customerId;
            public string profileId;
            public string state;
            public int remainingPatienceSeconds;
            public int currentNavigationIndex;

            public static CustomerDto FromRecord(
                CustomerSaveRecord record)
            {
                return new CustomerDto
                {
                    customerId = record.CustomerId,
                    profileId = record.ProfileId,
                    state = record.State,
                    remainingPatienceSeconds =
                        record.RemainingPatienceSeconds,
                    currentNavigationIndex =
                        record.CurrentNavigationIndex
                };
            }

            public CustomerSaveRecord ToRecord()
            {
                return new CustomerSaveRecord(
                    customerId,
                    profileId,
                    state,
                    remainingPatienceSeconds,
                    currentNavigationIndex);
            }
        }

        [Serializable]
        private sealed class ShoppingSessionDto
        {
            public string customerId;
            public string intentId;
            public string cartId;
            public string state;
            public int maxCartUnits;

            public static ShoppingSessionDto FromRecord(
                ShoppingSessionSaveRecord record)
            {
                return new ShoppingSessionDto
                {
                    customerId = record.CustomerId,
                    intentId = record.IntentId,
                    cartId = record.CartId,
                    state = record.State,
                    maxCartUnits = record.MaxCartUnits
                };
            }

            public ShoppingSessionSaveRecord ToRecord()
            {
                return new ShoppingSessionSaveRecord(
                    customerId,
                    intentId,
                    cartId,
                    state,
                    maxCartUnits);
            }
        }

        [Serializable]
        private sealed class ReservationDto
        {
            public string reservationId;
            public string customerId;
            public string cartId;
            public string displayId;
            public string productId;
            public int quantity;
            public string state;

            public static ReservationDto FromRecord(
                ReservationSaveRecord record)
            {
                return new ReservationDto
                {
                    reservationId = record.ReservationId,
                    customerId = record.CustomerId,
                    cartId = record.CartId,
                    displayId = record.DisplayId,
                    productId = record.ProductId,
                    quantity = record.Quantity,
                    state = record.State
                };
            }

            public ReservationSaveRecord ToRecord()
            {
                return new ReservationSaveRecord(
                    reservationId,
                    customerId,
                    cartId,
                    displayId,
                    productId,
                    quantity,
                    state);
            }
        }

        [Serializable]
        private sealed class QueueEntryDto
        {
            public string entryId;
            public string customerId;
            public string cartId;
            public string state;
            public int position;

            public static QueueEntryDto FromRecord(
                CheckoutQueueEntrySaveRecord record)
            {
                return new QueueEntryDto
                {
                    entryId = record.EntryId,
                    customerId = record.CustomerId,
                    cartId = record.CartId,
                    state = record.State,
                    position = record.Position
                };
            }

            public CheckoutQueueEntrySaveRecord ToRecord()
            {
                return new CheckoutQueueEntrySaveRecord(
                    entryId,
                    customerId,
                    cartId,
                    state,
                    position);
            }
        }

        [Serializable]
        private sealed class StationDto
        {
            public string stationId;
            public string state;
            public string currentEntryId;

            public static StationDto FromRecord(
                CheckoutStationSaveRecord record)
            {
                return new StationDto
                {
                    stationId = record.StationId,
                    state = record.State,
                    currentEntryId =
                        record.CurrentEntryId
                };
            }

            public CheckoutStationSaveRecord ToRecord()
            {
                return new CheckoutStationSaveRecord(
                    stationId,
                    state,
                    currentEntryId);
            }
        }

        [Serializable]
        private sealed class TransactionDto
        {
            public string transactionId;
            public string customerId;
            public string cartId;
            public string stationId;
            public string state;
            public int lineCount;
            public int unitCount;

            public static TransactionDto FromRecord(
                CheckoutTransactionSaveRecord record)
            {
                return new TransactionDto
                {
                    transactionId =
                        record.TransactionId,
                    customerId = record.CustomerId,
                    cartId = record.CartId,
                    stationId = record.StationId,
                    state = record.State,
                    lineCount = record.LineCount,
                    unitCount = record.UnitCount
                };
            }

            public CheckoutTransactionSaveRecord ToRecord()
            {
                return new CheckoutTransactionSaveRecord(
                    transactionId,
                    customerId,
                    cartId,
                    stationId,
                    state,
                    lineCount,
                    unitCount);
            }
        }

        [Serializable]
        private sealed class DayCycleDto
        {
            public string dayId;
            public string state;
            public int openDurationSeconds;
            public int elapsedOpenSeconds;
            public bool autoBeginClosing;

            public static DayCycleDto FromRecord(
                DayCycleSaveRecord record)
            {
                return new DayCycleDto
                {
                    dayId = record.DayId,
                    state = record.State,
                    openDurationSeconds =
                        record.OpenDurationSeconds,
                    elapsedOpenSeconds =
                        record.ElapsedOpenSeconds,
                    autoBeginClosing =
                        record.AutoBeginClosing
                };
            }

            public DayCycleSaveRecord ToRecord()
            {
                return new DayCycleSaveRecord(
                    dayId,
                    state,
                    openDurationSeconds,
                    elapsedOpenSeconds,
                    autoBeginClosing);
            }
        }

        [Serializable]
        private sealed class LedgerEntryDto
        {
            public string entryId;
            public string postingType;
            public string sourceId;
            public string dayId;
            public long minorUnits;
            public string currencyCode;

            public static LedgerEntryDto FromRecord(
                EconomyLedgerSaveRecord record)
            {
                return new LedgerEntryDto
                {
                    entryId = record.EntryId,
                    postingType = record.PostingType,
                    sourceId = record.SourceId,
                    dayId = record.DayId,
                    minorUnits = record.MinorUnits,
                    currencyCode = record.CurrencyCode
                };
            }

            public EconomyLedgerSaveRecord ToRecord()
            {
                return new EconomyLedgerSaveRecord(
                    entryId,
                    postingType,
                    sourceId,
                    dayId,
                    minorUnits,
                    currencyCode);
            }
        }
    }
}
