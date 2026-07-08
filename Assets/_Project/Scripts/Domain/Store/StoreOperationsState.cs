using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;

namespace VRMGames.CartridgeAndCloud.Domain.Store
{
    public sealed class StoreOperationsState
    {
        private readonly ReadOnlyCollection<StoreOrderRecord> _orders;
        private readonly ReadOnlyCollection<StoreDeliveryRunRecord> _deliveryRuns;
        private readonly ReadOnlyCollection<StoreStockRecord> _furnitureWarehouse;
        private readonly ReadOnlyCollection<StoreStockRecord> _productWarehouse;
        private readonly ReadOnlyCollection<PlacedStoreFixtureRecord> _fixtures;

        public SaveSlotId SlotId { get; }
        public string SessionId { get; }
        public int Generation { get; }
        public int NextOrderSequence { get; }
        public int NextDeliveryRunSequence { get; }
        public int NextFixtureSequence { get; }
        public int NextCustomerSequence { get; }
        public int CompletedSales { get; }
        public long LifetimeRevenueCents { get; }
        public long LifetimeExpenseCents { get; }
        public long LifetimeTaxCents { get; }
        public long WeeklyRevenueCents { get; }
        public long WeeklySupplierCostCents { get; }
        public int LastSettledWeek { get; }
        public long LastWeeklyGrossResultCents { get; }
        public long LastWeeklyTaxCents { get; }
        public StoreManagementHistory ManagementHistory { get; }

        public IReadOnlyList<StoreOrderRecord> Orders => _orders;
        public IReadOnlyList<StoreDeliveryRunRecord> DeliveryRuns => _deliveryRuns;
        public IReadOnlyList<StoreStockRecord> FurnitureWarehouse => _furnitureWarehouse;
        public IReadOnlyList<StoreStockRecord> ProductWarehouse => _productWarehouse;
        public IReadOnlyList<PlacedStoreFixtureRecord> Fixtures => _fixtures;

        public long ReservedFundsCents
        {
            get
            {
                long total = 0;
                foreach (StoreOrderRecord order in _orders)
                {
                    total = checked(total + order.ReservedCostCents);
                }
                return total;
            }
        }

        public long WeeklyGrossResultCents =>
            checked(WeeklyRevenueCents - WeeklySupplierCostCents);

        public StoreOperationsState(
            SaveSlotId slotId,
            string sessionId,
            int generation,
            int nextOrderSequence,
            int nextFixtureSequence,
            int nextCustomerSequence,
            int completedSales,
            long lifetimeRevenueCents,
            long lifetimeExpenseCents,
            IEnumerable<StoreOrderRecord> orders,
            IEnumerable<StoreStockRecord> furnitureWarehouse,
            IEnumerable<StoreStockRecord> productWarehouse,
            IEnumerable<PlacedStoreFixtureRecord> fixtures)
            : this(
                slotId,
                sessionId,
                generation,
                nextOrderSequence,
                1,
                nextFixtureSequence,
                nextCustomerSequence,
                completedSales,
                lifetimeRevenueCents,
                lifetimeExpenseCents,
                0,
                0,
                0,
                0,
                0,
                0,
                orders,
                new StoreDeliveryRunRecord[0],
                furnitureWarehouse,
                productWarehouse,
                fixtures)
        {
        }

        public StoreOperationsState(
            SaveSlotId slotId,
            string sessionId,
            int generation,
            int nextOrderSequence,
            int nextDeliveryRunSequence,
            int nextFixtureSequence,
            int nextCustomerSequence,
            int completedSales,
            long lifetimeRevenueCents,
            long lifetimeExpenseCents,
            long lifetimeTaxCents,
            long weeklyRevenueCents,
            long weeklySupplierCostCents,
            int lastSettledWeek,
            long lastWeeklyGrossResultCents,
            long lastWeeklyTaxCents,
            IEnumerable<StoreOrderRecord> orders,
            IEnumerable<StoreDeliveryRunRecord> deliveryRuns,
            IEnumerable<StoreStockRecord> furnitureWarehouse,
            IEnumerable<StoreStockRecord> productWarehouse,
            IEnumerable<PlacedStoreFixtureRecord> fixtures)
            : this(
                slotId,
                sessionId,
                generation,
                nextOrderSequence,
                nextDeliveryRunSequence,
                nextFixtureSequence,
                nextCustomerSequence,
                completedSales,
                lifetimeRevenueCents,
                lifetimeExpenseCents,
                lifetimeTaxCents,
                weeklyRevenueCents,
                weeklySupplierCostCents,
                lastSettledWeek,
                lastWeeklyGrossResultCents,
                lastWeeklyTaxCents,
                orders,
                deliveryRuns,
                furnitureWarehouse,
                productWarehouse,
                fixtures,
                StoreManagementHistory.Empty())
        {
        }

        public StoreOperationsState(
            SaveSlotId slotId,
            string sessionId,
            int generation,
            int nextOrderSequence,
            int nextDeliveryRunSequence,
            int nextFixtureSequence,
            int nextCustomerSequence,
            int completedSales,
            long lifetimeRevenueCents,
            long lifetimeExpenseCents,
            long lifetimeTaxCents,
            long weeklyRevenueCents,
            long weeklySupplierCostCents,
            int lastSettledWeek,
            long lastWeeklyGrossResultCents,
            long lastWeeklyTaxCents,
            IEnumerable<StoreOrderRecord> orders,
            IEnumerable<StoreDeliveryRunRecord> deliveryRuns,
            IEnumerable<StoreStockRecord> furnitureWarehouse,
            IEnumerable<StoreStockRecord> productWarehouse,
            IEnumerable<PlacedStoreFixtureRecord> fixtures,
            StoreManagementHistory managementHistory)
        {
            if (generation < 0) throw new ArgumentOutOfRangeException(nameof(generation));
            if (nextOrderSequence < 1) throw new ArgumentOutOfRangeException(nameof(nextOrderSequence));
            if (nextDeliveryRunSequence < 1) throw new ArgumentOutOfRangeException(nameof(nextDeliveryRunSequence));
            if (nextFixtureSequence < 1) throw new ArgumentOutOfRangeException(nameof(nextFixtureSequence));
            if (nextCustomerSequence < 1) throw new ArgumentOutOfRangeException(nameof(nextCustomerSequence));
            if (completedSales < 0) throw new ArgumentOutOfRangeException(nameof(completedSales));
            if (lifetimeRevenueCents < 0) throw new ArgumentOutOfRangeException(nameof(lifetimeRevenueCents));
            if (lifetimeExpenseCents < 0) throw new ArgumentOutOfRangeException(nameof(lifetimeExpenseCents));
            if (lifetimeTaxCents < 0) throw new ArgumentOutOfRangeException(nameof(lifetimeTaxCents));
            if (weeklyRevenueCents < 0) throw new ArgumentOutOfRangeException(nameof(weeklyRevenueCents));
            if (weeklySupplierCostCents < 0) throw new ArgumentOutOfRangeException(nameof(weeklySupplierCostCents));
            if (lastSettledWeek < 0) throw new ArgumentOutOfRangeException(nameof(lastSettledWeek));
            if (lastWeeklyTaxCents < 0) throw new ArgumentOutOfRangeException(nameof(lastWeeklyTaxCents));
            if (string.IsNullOrWhiteSpace(sessionId)) throw new ArgumentException("A session ID is required.", nameof(sessionId));

            SlotId = slotId;
            SessionId = sessionId;
            Generation = generation;
            NextOrderSequence = nextOrderSequence;
            NextDeliveryRunSequence = nextDeliveryRunSequence;
            NextFixtureSequence = nextFixtureSequence;
            NextCustomerSequence = nextCustomerSequence;
            CompletedSales = completedSales;
            LifetimeRevenueCents = lifetimeRevenueCents;
            LifetimeExpenseCents = lifetimeExpenseCents;
            LifetimeTaxCents = lifetimeTaxCents;
            WeeklyRevenueCents = weeklyRevenueCents;
            WeeklySupplierCostCents = weeklySupplierCostCents;
            LastSettledWeek = lastSettledWeek;
            LastWeeklyGrossResultCents = lastWeeklyGrossResultCents;
            LastWeeklyTaxCents = lastWeeklyTaxCents;
            ManagementHistory = managementHistory ??
                throw new ArgumentNullException(nameof(managementHistory));

            _orders = Copy(orders, nameof(orders));
            _deliveryRuns = Copy(deliveryRuns, nameof(deliveryRuns));
            _furnitureWarehouse = Copy(furnitureWarehouse, nameof(furnitureWarehouse));
            _productWarehouse = Copy(productWarehouse, nameof(productWarehouse));
            _fixtures = Copy(fixtures, nameof(fixtures));
            ValidateUniqueIds();
        }

        public static StoreOperationsState Empty(SaveSlotId slotId, string sessionId)
        {
            return new StoreOperationsState(
                slotId,
                sessionId,
                0,
                1,
                1,
                1,
                1,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                new StoreOrderRecord[0],
                new StoreDeliveryRunRecord[0],
                new StoreStockRecord[0],
                new StoreStockRecord[0],
                new PlacedStoreFixtureRecord[0]);
        }

        private void ValidateUniqueIds()
        {
            HashSet<string> orderIds = new HashSet<string>(StringComparer.Ordinal);
            HashSet<string> runIds = new HashSet<string>(StringComparer.Ordinal);
            HashSet<string> fixtureIds = new HashSet<string>(StringComparer.Ordinal);

            foreach (StoreOrderRecord order in _orders)
            {
                if (!orderIds.Add(order.OrderId)) throw new ArgumentException("Duplicate order ID.");
            }

            foreach (StoreDeliveryRunRecord run in _deliveryRuns)
            {
                if (!runIds.Add(run.DeliveryRunId)) throw new ArgumentException("Duplicate delivery run ID.");
                foreach (string orderId in run.OrderIds)
                {
                    if (!orderIds.Contains(orderId))
                    {
                        throw new ArgumentException("Delivery run references a missing order.");
                    }
                }
            }

            foreach (PlacedStoreFixtureRecord fixture in _fixtures)
            {
                if (!fixtureIds.Add(fixture.InstanceId)) throw new ArgumentException("Duplicate fixture ID.");
            }
        }

        private static ReadOnlyCollection<T> Copy<T>(IEnumerable<T> source, string parameterName)
        {
            if (source == null) throw new ArgumentNullException(parameterName);
            List<T> copy = new List<T>();
            foreach (T item in source)
            {
                if (item == null) throw new ArgumentException("Collection cannot contain null.", parameterName);
                copy.Add(item);
            }
            return new ReadOnlyCollection<T>(copy);
        }
    }
}
