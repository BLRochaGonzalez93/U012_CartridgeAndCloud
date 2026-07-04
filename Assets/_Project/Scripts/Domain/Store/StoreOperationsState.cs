using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;

namespace VRMGames.CartridgeAndCloud.Domain.Store
{
    public sealed class StoreOperationsState
    {
        private readonly ReadOnlyCollection<
            StoreOrderRecord> _orders;
        private readonly ReadOnlyCollection<
            StoreStockRecord> _furnitureWarehouse;
        private readonly ReadOnlyCollection<
            StoreStockRecord> _productWarehouse;
        private readonly ReadOnlyCollection<
            PlacedStoreFixtureRecord> _fixtures;

        public SaveSlotId SlotId { get; }
        public string SessionId { get; }
        public int Generation { get; }
        public int NextOrderSequence { get; }
        public int NextFixtureSequence { get; }
        public int NextCustomerSequence { get; }
        public int CompletedSales { get; }
        public long LifetimeRevenueCents { get; }
        public long LifetimeExpenseCents { get; }

        public IReadOnlyList<StoreOrderRecord>
            Orders => _orders;

        public IReadOnlyList<StoreStockRecord>
            FurnitureWarehouse =>
                _furnitureWarehouse;

        public IReadOnlyList<StoreStockRecord>
            ProductWarehouse =>
                _productWarehouse;

        public IReadOnlyList<
            PlacedStoreFixtureRecord> Fixtures =>
                _fixtures;

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
            IEnumerable<StoreStockRecord>
                furnitureWarehouse,
            IEnumerable<StoreStockRecord>
                productWarehouse,
            IEnumerable<PlacedStoreFixtureRecord>
                fixtures)
        {
            if (generation < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(generation));
            }

            if (nextOrderSequence < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(nextOrderSequence));
            }

            if (nextFixtureSequence < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(nextFixtureSequence));
            }

            if (nextCustomerSequence < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(nextCustomerSequence));
            }

            if (completedSales < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(completedSales));
            }

            if (lifetimeRevenueCents < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(lifetimeRevenueCents));
            }

            if (lifetimeExpenseCents < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(lifetimeExpenseCents));
            }

            if (string.IsNullOrWhiteSpace(sessionId))
            {
                throw new ArgumentException(
                    "A session ID is required.",
                    nameof(sessionId));
            }

            SlotId = slotId;
            SessionId = sessionId;
            Generation = generation;
            NextOrderSequence =
                nextOrderSequence;
            NextFixtureSequence =
                nextFixtureSequence;
            NextCustomerSequence =
                nextCustomerSequence;
            CompletedSales = completedSales;
            LifetimeRevenueCents =
                lifetimeRevenueCents;
            LifetimeExpenseCents =
                lifetimeExpenseCents;

            _orders = Copy(
                orders,
                nameof(orders));
            _furnitureWarehouse = Copy(
                furnitureWarehouse,
                nameof(furnitureWarehouse));
            _productWarehouse = Copy(
                productWarehouse,
                nameof(productWarehouse));
            _fixtures = Copy(
                fixtures,
                nameof(fixtures));

            ValidateUniqueIds();
        }

        public static StoreOperationsState Empty(
            SaveSlotId slotId,
            string sessionId)
        {
            return new StoreOperationsState(
                slotId,
                sessionId,
                0,
                1,
                1,
                1,
                0,
                0,
                0,
                new StoreOrderRecord[0],
                new StoreStockRecord[0],
                new StoreStockRecord[0],
                new PlacedStoreFixtureRecord[0]);
        }

        private void ValidateUniqueIds()
        {
            HashSet<string> orderIds =
                new HashSet<string>(
                    StringComparer.Ordinal);
            HashSet<string> fixtureIds =
                new HashSet<string>(
                    StringComparer.Ordinal);

            foreach (StoreOrderRecord order
                     in _orders)
            {
                if (!orderIds.Add(order.OrderId))
                {
                    throw new ArgumentException(
                        "Duplicate order ID.");
                }
            }

            foreach (PlacedStoreFixtureRecord fixture
                     in _fixtures)
            {
                if (!fixtureIds.Add(
                        fixture.InstanceId))
                {
                    throw new ArgumentException(
                        "Duplicate fixture ID.");
                }
            }
        }

        private static ReadOnlyCollection<T> Copy<T>(
            IEnumerable<T> source,
            string parameterName)
        {
            if (source == null)
            {
                throw new ArgumentNullException(
                    parameterName);
            }

            List<T> copy = new List<T>();

            foreach (T item in source)
            {
                if (item == null)
                {
                    throw new ArgumentException(
                        "Collection cannot contain null.",
                        parameterName);
                }

                copy.Add(item);
            }

            return new ReadOnlyCollection<T>(
                copy);
        }
    }
}
