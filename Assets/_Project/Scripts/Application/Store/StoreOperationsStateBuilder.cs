using System;
using System.Collections.Generic;
using VRMGames.CartridgeAndCloud.Domain.Store;

namespace VRMGames.CartridgeAndCloud.Application.Store
{
    internal sealed class StoreOperationsStateBuilder
    {
        private readonly StoreOperationsState _source;

        public List<StoreOrderRecord> Orders { get; }
        public List<StoreDeliveryRunRecord> DeliveryRuns { get; }
        public List<StoreStockRecord> FurnitureWarehouse { get; }
        public List<StoreStockRecord> ProductWarehouse { get; }
        public List<PlacedStoreFixtureRecord> Fixtures { get; }

        public int Generation { get; set; }
        public int NextOrderSequence { get; set; }
        public int NextDeliveryRunSequence { get; set; }
        public int NextFixtureSequence { get; set; }
        public int NextCustomerSequence { get; set; }
        public int CompletedSales { get; set; }
        public long LifetimeRevenueCents { get; set; }
        public long LifetimeExpenseCents { get; set; }
        public long LifetimeTaxCents { get; set; }
        public long WeeklyRevenueCents { get; set; }
        public long WeeklySupplierCostCents { get; set; }
        public int LastSettledWeek { get; set; }
        public long LastWeeklyGrossResultCents { get; set; }
        public long LastWeeklyTaxCents { get; set; }
        public StoreManagementHistory ManagementHistory { get; set; }

        public StoreOperationsStateBuilder(StoreOperationsState source)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            Orders = new List<StoreOrderRecord>(source.Orders);
            DeliveryRuns = new List<StoreDeliveryRunRecord>(source.DeliveryRuns);
            FurnitureWarehouse = new List<StoreStockRecord>(source.FurnitureWarehouse);
            ProductWarehouse = new List<StoreStockRecord>(source.ProductWarehouse);
            Fixtures = new List<PlacedStoreFixtureRecord>(source.Fixtures);
            Generation = source.Generation;
            NextOrderSequence = source.NextOrderSequence;
            NextDeliveryRunSequence = source.NextDeliveryRunSequence;
            NextFixtureSequence = source.NextFixtureSequence;
            NextCustomerSequence = source.NextCustomerSequence;
            CompletedSales = source.CompletedSales;
            LifetimeRevenueCents = source.LifetimeRevenueCents;
            LifetimeExpenseCents = source.LifetimeExpenseCents;
            LifetimeTaxCents = source.LifetimeTaxCents;
            WeeklyRevenueCents = source.WeeklyRevenueCents;
            WeeklySupplierCostCents = source.WeeklySupplierCostCents;
            LastSettledWeek = source.LastSettledWeek;
            LastWeeklyGrossResultCents = source.LastWeeklyGrossResultCents;
            LastWeeklyTaxCents = source.LastWeeklyTaxCents;
            ManagementHistory = source.ManagementHistory;
        }

        public int GetStock(List<StoreStockRecord> stock, string itemId)
        {
            int index = FindStockIndex(stock, itemId);
            return index < 0 ? 0 : stock[index].Quantity;
        }

        public void AddStock(List<StoreStockRecord> stock, string itemId, int delta)
        {
            int current = GetStock(stock, itemId);
            int next = checked(current + delta);
            if (next < 0) throw new InvalidOperationException("Stock cannot become negative.");
            int index = FindStockIndex(stock, itemId);
            if (next == 0)
            {
                if (index >= 0) stock.RemoveAt(index);
                return;
            }
            StoreStockRecord replacement = new StoreStockRecord(itemId, next);
            if (index >= 0) stock[index] = replacement;
            else stock.Add(replacement);
        }

        public StoreOperationsState Build(bool incrementGeneration = true)
        {
            return new StoreOperationsState(
                _source.SlotId,
                _source.SessionId,
                incrementGeneration ? checked(Generation + 1) : Generation,
                NextOrderSequence,
                NextDeliveryRunSequence,
                NextFixtureSequence,
                NextCustomerSequence,
                CompletedSales,
                LifetimeRevenueCents,
                LifetimeExpenseCents,
                LifetimeTaxCents,
                WeeklyRevenueCents,
                WeeklySupplierCostCents,
                LastSettledWeek,
                LastWeeklyGrossResultCents,
                LastWeeklyTaxCents,
                Orders,
                DeliveryRuns,
                FurnitureWarehouse,
                ProductWarehouse,
                Fixtures,
                ManagementHistory);
        }

        private static int FindStockIndex(List<StoreStockRecord> stock, string itemId)
        {
            for (int index = 0; index < stock.Count; index++)
            {
                if (string.Equals(stock[index].ItemId, itemId, StringComparison.Ordinal)) return index;
            }
            return -1;
        }
    }
}
