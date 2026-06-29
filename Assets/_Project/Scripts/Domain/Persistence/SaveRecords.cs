using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VRMGames.CartridgeAndCloud.Domain.Persistence
{
    public sealed class ProductQuantitySaveRecord
    {
        public string ProductId { get; }
        public int Quantity { get; }

        public ProductQuantitySaveRecord(
            string productId,
            int quantity)
        {
            ProductId = SaveRecordGuard.Required(
                productId,
                nameof(productId));
            Quantity = SaveRecordGuard.NonNegative(
                quantity,
                nameof(quantity));
        }
    }

    public sealed class InventoryContainerSaveRecord
    {
        private readonly ReadOnlyCollection<
            ProductQuantitySaveRecord> _products;

        public string ContainerId { get; }
        public int Capacity { get; }

        public IReadOnlyList<ProductQuantitySaveRecord>
            Products => _products;

        public InventoryContainerSaveRecord(
            string containerId,
            int capacity,
            IEnumerable<ProductQuantitySaveRecord> products)
        {
            ContainerId = SaveRecordGuard.Required(
                containerId,
                nameof(containerId));
            Capacity = SaveRecordGuard.NonNegative(
                capacity,
                nameof(capacity));
            _products = SaveRecordGuard.Copy(
                products,
                nameof(products));

            int total = 0;
            HashSet<string> ids =
                new HashSet<string>(
                    StringComparer.Ordinal);

            foreach (ProductQuantitySaveRecord product
                     in _products)
            {
                SaveRecordGuard.Unique(
                    ids,
                    product.ProductId,
                    "inventory product");
                total = checked(total + product.Quantity);
            }

            if (total > Capacity)
            {
                throw new ArgumentException(
                    "Inventory quantity exceeds capacity.",
                    nameof(products));
            }
        }
    }

    public sealed class SupplierOrderSaveRecord
    {
        public string OrderId { get; }
        public string SupplierId { get; }
        public string ProductId { get; }
        public string State { get; }
        public int OrderedUnits { get; }
        public int ReceivedUnits { get; }
        public long UnitCostCents { get; }

        public SupplierOrderSaveRecord(
            string orderId,
            string supplierId,
            string productId,
            string state,
            int orderedUnits,
            int receivedUnits,
            long unitCostCents)
        {
            OrderId = SaveRecordGuard.Required(
                orderId,
                nameof(orderId));
            SupplierId = SaveRecordGuard.Required(
                supplierId,
                nameof(supplierId));
            ProductId = SaveRecordGuard.Required(
                productId,
                nameof(productId));
            State = SaveRecordGuard.Required(
                state,
                nameof(state));
            OrderedUnits = SaveRecordGuard.Positive(
                orderedUnits,
                nameof(orderedUnits));
            ReceivedUnits = SaveRecordGuard.NonNegative(
                receivedUnits,
                nameof(receivedUnits));

            if (ReceivedUnits > OrderedUnits)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(receivedUnits));
            }

            if (unitCostCents <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(unitCostCents));
            }

            UnitCostCents = unitCostCents;
        }
    }

    public sealed class DisplaySaveRecord
    {
        public string DisplayId { get; }
        public string DefinitionId { get; }
        public string AssignedProductId { get; }
        public string InventoryContainerId { get; }

        public bool HasAssignedProduct =>
            !string.IsNullOrWhiteSpace(
                AssignedProductId);

        public DisplaySaveRecord(
            string displayId,
            string definitionId,
            string assignedProductId,
            string inventoryContainerId)
        {
            DisplayId = SaveRecordGuard.Required(
                displayId,
                nameof(displayId));
            DefinitionId = SaveRecordGuard.Required(
                definitionId,
                nameof(definitionId));
            AssignedProductId =
                assignedProductId ?? string.Empty;
            InventoryContainerId =
                SaveRecordGuard.Required(
                    inventoryContainerId,
                    nameof(inventoryContainerId));
        }
    }

    public sealed class CustomerSaveRecord
    {
        public string CustomerId { get; }
        public string ProfileId { get; }
        public string State { get; }
        public int RemainingPatienceSeconds { get; }
        public int CurrentNavigationIndex { get; }

        public CustomerSaveRecord(
            string customerId,
            string profileId,
            string state,
            int remainingPatienceSeconds,
            int currentNavigationIndex)
        {
            CustomerId = SaveRecordGuard.Required(
                customerId,
                nameof(customerId));
            ProfileId = SaveRecordGuard.Required(
                profileId,
                nameof(profileId));
            State = SaveRecordGuard.Required(
                state,
                nameof(state));
            RemainingPatienceSeconds =
                SaveRecordGuard.NonNegative(
                    remainingPatienceSeconds,
                    nameof(remainingPatienceSeconds));
            CurrentNavigationIndex =
                SaveRecordGuard.NonNegative(
                    currentNavigationIndex,
                    nameof(currentNavigationIndex));
        }
    }

    public sealed class ShoppingSessionSaveRecord
    {
        public string CustomerId { get; }
        public string IntentId { get; }
        public string CartId { get; }
        public string State { get; }
        public int MaxCartUnits { get; }

        public ShoppingSessionSaveRecord(
            string customerId,
            string intentId,
            string cartId,
            string state,
            int maxCartUnits)
        {
            CustomerId = SaveRecordGuard.Required(
                customerId,
                nameof(customerId));
            IntentId = SaveRecordGuard.Required(
                intentId,
                nameof(intentId));
            CartId = SaveRecordGuard.Required(
                cartId,
                nameof(cartId));
            State = SaveRecordGuard.Required(
                state,
                nameof(state));
            MaxCartUnits = SaveRecordGuard.Positive(
                maxCartUnits,
                nameof(maxCartUnits));
        }
    }

    public sealed class ReservationSaveRecord
    {
        public string ReservationId { get; }
        public string CustomerId { get; }
        public string CartId { get; }
        public string DisplayId { get; }
        public string ProductId { get; }
        public int Quantity { get; }
        public string State { get; }

        public ReservationSaveRecord(
            string reservationId,
            string customerId,
            string cartId,
            string displayId,
            string productId,
            int quantity,
            string state)
        {
            ReservationId = SaveRecordGuard.Required(
                reservationId,
                nameof(reservationId));
            CustomerId = SaveRecordGuard.Required(
                customerId,
                nameof(customerId));
            CartId = SaveRecordGuard.Required(
                cartId,
                nameof(cartId));
            DisplayId = SaveRecordGuard.Required(
                displayId,
                nameof(displayId));
            ProductId = SaveRecordGuard.Required(
                productId,
                nameof(productId));
            Quantity = SaveRecordGuard.Positive(
                quantity,
                nameof(quantity));
            State = SaveRecordGuard.Required(
                state,
                nameof(state));
        }
    }

    public sealed class CheckoutQueueEntrySaveRecord
    {
        public string EntryId { get; }
        public string CustomerId { get; }
        public string CartId { get; }
        public string State { get; }
        public int Position { get; }

        public CheckoutQueueEntrySaveRecord(
            string entryId,
            string customerId,
            string cartId,
            string state,
            int position)
        {
            EntryId = SaveRecordGuard.Required(
                entryId,
                nameof(entryId));
            CustomerId = SaveRecordGuard.Required(
                customerId,
                nameof(customerId));
            CartId = SaveRecordGuard.Required(
                cartId,
                nameof(cartId));
            State = SaveRecordGuard.Required(
                state,
                nameof(state));
            Position = SaveRecordGuard.Positive(
                position,
                nameof(position));
        }
    }

    public sealed class CheckoutStationSaveRecord
    {
        public string StationId { get; }
        public string State { get; }
        public string CurrentEntryId { get; }

        public CheckoutStationSaveRecord(
            string stationId,
            string state,
            string currentEntryId)
        {
            StationId = SaveRecordGuard.Required(
                stationId,
                nameof(stationId));
            State = SaveRecordGuard.Required(
                state,
                nameof(state));
            CurrentEntryId =
                currentEntryId ?? string.Empty;
        }
    }

    public sealed class CheckoutTransactionSaveRecord
    {
        public string TransactionId { get; }
        public string CustomerId { get; }
        public string CartId { get; }
        public string StationId { get; }
        public string State { get; }
        public int LineCount { get; }
        public int UnitCount { get; }

        public CheckoutTransactionSaveRecord(
            string transactionId,
            string customerId,
            string cartId,
            string stationId,
            string state,
            int lineCount,
            int unitCount)
        {
            TransactionId = SaveRecordGuard.Required(
                transactionId,
                nameof(transactionId));
            CustomerId = SaveRecordGuard.Required(
                customerId,
                nameof(customerId));
            CartId = SaveRecordGuard.Required(
                cartId,
                nameof(cartId));
            StationId = SaveRecordGuard.Required(
                stationId,
                nameof(stationId));
            State = SaveRecordGuard.Required(
                state,
                nameof(state));
            LineCount = SaveRecordGuard.Positive(
                lineCount,
                nameof(lineCount));
            UnitCount = SaveRecordGuard.Positive(
                unitCount,
                nameof(unitCount));
        }
    }

    public sealed class DayCycleSaveRecord
    {
        public string DayId { get; }
        public string State { get; }
        public int OpenDurationSeconds { get; }
        public int ElapsedOpenSeconds { get; }
        public bool AutoBeginClosing { get; }

        public DayCycleSaveRecord(
            string dayId,
            string state,
            int openDurationSeconds,
            int elapsedOpenSeconds,
            bool autoBeginClosing)
        {
            DayId = SaveRecordGuard.Required(
                dayId,
                nameof(dayId));
            State = SaveRecordGuard.Required(
                state,
                nameof(state));
            OpenDurationSeconds =
                SaveRecordGuard.Positive(
                    openDurationSeconds,
                    nameof(openDurationSeconds));
            ElapsedOpenSeconds =
                SaveRecordGuard.NonNegative(
                    elapsedOpenSeconds,
                    nameof(elapsedOpenSeconds));

            if (ElapsedOpenSeconds >
                OpenDurationSeconds)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(elapsedOpenSeconds));
            }

            AutoBeginClosing = autoBeginClosing;
        }
    }

    public sealed class EconomyLedgerSaveRecord
    {
        public string EntryId { get; }
        public string PostingType { get; }
        public string SourceId { get; }
        public string DayId { get; }
        public long MinorUnits { get; }
        public string CurrencyCode { get; }

        public EconomyLedgerSaveRecord(
            string entryId,
            string postingType,
            string sourceId,
            string dayId,
            long minorUnits,
            string currencyCode)
        {
            EntryId = SaveRecordGuard.Required(
                entryId,
                nameof(entryId));
            PostingType = SaveRecordGuard.Required(
                postingType,
                nameof(postingType));
            SourceId = SaveRecordGuard.Required(
                sourceId,
                nameof(sourceId));
            DayId = SaveRecordGuard.Required(
                dayId,
                nameof(dayId));

            if (minorUnits <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(minorUnits));
            }

            MinorUnits = minorUnits;
            CurrencyCode = SaveRecordGuard.Required(
                currencyCode,
                nameof(currencyCode))
                .ToUpperInvariant();

            if (CurrencyCode.Length != 3)
            {
                throw new ArgumentException(
                    "Currency code must contain three characters.",
                    nameof(currencyCode));
            }
        }
    }

    internal static class SaveRecordGuard
    {
        public static string Required(
            string value,
            string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "Value cannot be empty.",
                    parameterName);
            }

            return value;
        }

        public static int NonNegative(
            int value,
            string parameterName)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(
                    parameterName);
            }

            return value;
        }

        public static int Positive(
            int value,
            string parameterName)
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    parameterName);
            }

            return value;
        }

        public static ReadOnlyCollection<T> Copy<T>(
            IEnumerable<T> values,
            string parameterName)
            where T : class
        {
            if (values == null)
            {
                throw new ArgumentNullException(
                    parameterName);
            }

            List<T> copy = new List<T>();

            foreach (T value in values)
            {
                if (value == null)
                {
                    throw new ArgumentException(
                        "Collection cannot contain null.",
                        parameterName);
                }

                copy.Add(value);
            }

            return new ReadOnlyCollection<T>(copy);
        }

        public static void Unique(
            HashSet<string> ids,
            string id,
            string label)
        {
            if (!ids.Add(id))
            {
                throw new ArgumentException(
                    $"Duplicate {label} ID: {id}.");
            }
        }
    }
}
