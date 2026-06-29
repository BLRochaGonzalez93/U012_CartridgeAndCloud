using System;
using System.Collections.Generic;
using VRMGames.CartridgeAndCloud.Domain.VerticalSlicePhase1;

namespace VRMGames.CartridgeAndCloud.Application.VerticalSlicePhase1
{
    internal sealed class Phase1StateBuilder
    {
        private readonly Phase1StoreState _source;

        public List<Phase1OrderRecord> Orders { get; }
        public List<Phase1StockRecord>
            FurnitureWarehouse { get; }
        public List<Phase1StockRecord>
            ProductWarehouse { get; }
        public List<Phase1PlacedFixtureRecord>
            Fixtures { get; }

        public int Generation { get; set; }
        public int NextOrderSequence { get; set; }
        public int NextFixtureSequence { get; set; }
        public int NextCustomerSequence { get; set; }
        public int CompletedSales { get; set; }
        public long LifetimeRevenueCents { get; set; }
        public long LifetimeExpenseCents { get; set; }

        public Phase1StateBuilder(
            Phase1StoreState source)
        {
            _source = source ??
                throw new ArgumentNullException(
                    nameof(source));

            Orders =
                new List<Phase1OrderRecord>(
                    source.Orders);
            FurnitureWarehouse =
                new List<Phase1StockRecord>(
                    source.FurnitureWarehouse);
            ProductWarehouse =
                new List<Phase1StockRecord>(
                    source.ProductWarehouse);
            Fixtures =
                new List<
                    Phase1PlacedFixtureRecord>(
                        source.Fixtures);

            Generation = source.Generation;
            NextOrderSequence =
                source.NextOrderSequence;
            NextFixtureSequence =
                source.NextFixtureSequence;
            NextCustomerSequence =
                source.NextCustomerSequence;
            CompletedSales = source.CompletedSales;
            LifetimeRevenueCents =
                source.LifetimeRevenueCents;
            LifetimeExpenseCents =
                source.LifetimeExpenseCents;
        }

        public int GetStock(
            List<Phase1StockRecord> stock,
            string itemId)
        {
            int index = FindStockIndex(
                stock,
                itemId);

            return index < 0
                ? 0
                : stock[index].Quantity;
        }

        public void AddStock(
            List<Phase1StockRecord> stock,
            string itemId,
            int delta)
        {
            int current = GetStock(
                stock,
                itemId);
            int next = checked(current + delta);

            if (next < 0)
            {
                throw new InvalidOperationException(
                    "Stock cannot become negative.");
            }

            int index = FindStockIndex(
                stock,
                itemId);

            if (next == 0)
            {
                if (index >= 0)
                {
                    stock.RemoveAt(index);
                }

                return;
            }

            Phase1StockRecord replacement =
                new Phase1StockRecord(
                    itemId,
                    next);

            if (index >= 0)
            {
                stock[index] = replacement;
            }
            else
            {
                stock.Add(replacement);
            }
        }

        public Phase1StoreState Build(
            bool incrementGeneration = true)
        {
            return new Phase1StoreState(
                _source.SlotId,
                _source.SessionId,
                incrementGeneration
                    ? checked(Generation + 1)
                    : Generation,
                NextOrderSequence,
                NextFixtureSequence,
                NextCustomerSequence,
                CompletedSales,
                LifetimeRevenueCents,
                LifetimeExpenseCents,
                Orders,
                FurnitureWarehouse,
                ProductWarehouse,
                Fixtures);
        }

        private static int FindStockIndex(
            List<Phase1StockRecord> stock,
            string itemId)
        {
            for (int index = 0;
                 index < stock.Count;
                 index++)
            {
                if (string.Equals(
                        stock[index].ItemId,
                        itemId,
                        StringComparison.Ordinal))
                {
                    return index;
                }
            }

            return -1;
        }
    }
}
