using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VRMGames.CartridgeAndCloud.Domain.Store
{
    public enum StoreCustomerVisitOutcome
    {
        Entered = 0,
        Purchased = 1,
        Abandoned = 2
    }

    public sealed class StoreCustomerVisitDetailRecord
    {
        public string CustomerId { get; }
        public StoreCustomerVisitOutcome Outcome { get; }

        public StoreCustomerVisitDetailRecord(
            string customerId,
            StoreCustomerVisitOutcome outcome)
        {
            CustomerId = Required(customerId, nameof(customerId));
            if (!Enum.IsDefined(typeof(StoreCustomerVisitOutcome), outcome))
            {
                throw new ArgumentOutOfRangeException(nameof(outcome));
            }

            Outcome = outcome;
        }

        public StoreCustomerVisitDetailRecord WithOutcome(
            StoreCustomerVisitOutcome outcome)
        {
            if (Outcome == outcome)
            {
                return this;
            }

            if (Outcome != StoreCustomerVisitOutcome.Entered)
            {
                return this;
            }

            return new StoreCustomerVisitDetailRecord(
                CustomerId,
                outcome);
        }

        private static string Required(
            string value,
            string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "A customer ID is required.",
                    parameterName);
            }

            return value;
        }
    }

    public sealed class StoreSaleDetailRecord
    {
        public string TransactionId { get; }
        public string CustomerId { get; }
        public string ProductId { get; }
        public int Units { get; }
        public long RevenueCents { get; }

        public StoreSaleDetailRecord(
            string transactionId,
            string customerId,
            string productId,
            int units,
            long revenueCents)
        {
            TransactionId = Required(
                transactionId,
                nameof(transactionId));
            CustomerId = Required(
                customerId,
                nameof(customerId));
            ProductId = Required(
                productId,
                nameof(productId));

            if (units < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(units));
            }

            if (revenueCents < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(revenueCents));
            }

            Units = units;
            RevenueCents = revenueCents;
        }

        private static string Required(
            string value,
            string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "A value is required.",
                    parameterName);
            }

            return value;
        }
    }

    public sealed class StoreReceiptDetailRecord
    {
        public string ReceiptId { get; }
        public string DeliveryRunId { get; }
        public string OrderId { get; }
        public string ItemId { get; }
        public bool IsFurniture { get; }
        public int Units { get; }
        public long SupplierCostCents { get; }

        public StoreReceiptDetailRecord(
            string receiptId,
            string deliveryRunId,
            string orderId,
            string itemId,
            bool isFurniture,
            int units,
            long supplierCostCents)
        {
            ReceiptId = Required(
                receiptId,
                nameof(receiptId));
            DeliveryRunId = Required(
                deliveryRunId,
                nameof(deliveryRunId));
            OrderId = Required(
                orderId,
                nameof(orderId));
            ItemId = Required(
                itemId,
                nameof(itemId));

            if (units < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(units));
            }

            if (supplierCostCents < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(supplierCostCents));
            }

            IsFurniture = isFurniture;
            Units = units;
            SupplierCostCents = supplierCostCents;
        }

        private static string Required(
            string value,
            string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "A value is required.",
                    parameterName);
            }

            return value;
        }
    }

    public sealed class StoreManagementDayDetailRecord
    {
        private readonly ReadOnlyCollection<StoreCustomerVisitDetailRecord>
            _customerVisits;
        private readonly ReadOnlyCollection<StoreSaleDetailRecord>
            _sales;
        private readonly ReadOnlyCollection<StoreReceiptDetailRecord>
            _receipts;

        public int DayNumber { get; }
        public string DayId { get; }
        public bool IsClosed { get; }
        public long StartingCashCents { get; }
        public long EndingCashCents { get; }
        public long TaxCents { get; }

        public IReadOnlyList<StoreCustomerVisitDetailRecord>
            CustomerVisits => _customerVisits;
        public IReadOnlyList<StoreSaleDetailRecord> Sales => _sales;
        public IReadOnlyList<StoreReceiptDetailRecord> Receipts => _receipts;

        public int VisitorCount => _customerVisits.Count;
        public int BuyerCount => CountVisitOutcome(
            StoreCustomerVisitOutcome.Purchased);
        public int AbandonedCustomerCount => CountVisitOutcome(
            StoreCustomerVisitOutcome.Abandoned);
        public int CompletedSales => _sales.Count;
        public int UnitsSold => SumSaleUnits();
        public int OrdersReceived => _receipts.Count;
        public long RevenueCents => SumRevenue();
        public long SupplierCostCents => SumSupplierCosts();
        public long GrossResultCents => checked(
            RevenueCents - SupplierCostCents);
        public long NetResultCents => checked(
            GrossResultCents - TaxCents);

        public StoreManagementDayDetailRecord(
            int dayNumber,
            string dayId,
            bool isClosed,
            long startingCashCents,
            long endingCashCents,
            long taxCents,
            IEnumerable<StoreCustomerVisitDetailRecord> customerVisits,
            IEnumerable<StoreSaleDetailRecord> sales,
            IEnumerable<StoreReceiptDetailRecord> receipts)
        {
            if (dayNumber < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(dayNumber));
            }

            if (startingCashCents < 0 || endingCashCents < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(startingCashCents));
            }

            if (taxCents < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(taxCents));
            }

            DayNumber = dayNumber;
            DayId = Required(dayId, nameof(dayId));
            IsClosed = isClosed;
            StartingCashCents = startingCashCents;
            EndingCashCents = endingCashCents;
            TaxCents = taxCents;
            _customerVisits = CopyUnique(
                customerVisits,
                nameof(customerVisits),
                item => item.CustomerId,
                "customer visit");
            _sales = CopyUnique(
                sales,
                nameof(sales),
                item => item.TransactionId,
                "sale");
            _receipts = CopyUnique(
                receipts,
                nameof(receipts),
                item => item.ReceiptId,
                "receipt");
        }

        public static StoreManagementDayDetailRecord Open(
            int dayNumber,
            string dayId,
            long startingCashCents)
        {
            return new StoreManagementDayDetailRecord(
                dayNumber,
                dayId,
                false,
                startingCashCents,
                startingCashCents,
                0,
                new StoreCustomerVisitDetailRecord[0],
                new StoreSaleDetailRecord[0],
                new StoreReceiptDetailRecord[0]);
        }

        public StoreManagementDayDetailRecord AddVisit(
            string customerId)
        {
            if (IsClosed)
            {
                return this;
            }

            foreach (StoreCustomerVisitDetailRecord visit in _customerVisits)
            {
                if (string.Equals(
                        visit.CustomerId,
                        customerId,
                        StringComparison.Ordinal))
                {
                    return this;
                }
            }

            List<StoreCustomerVisitDetailRecord> visits =
                new List<StoreCustomerVisitDetailRecord>(_customerVisits)
                {
                    new StoreCustomerVisitDetailRecord(
                        customerId,
                        StoreCustomerVisitOutcome.Entered)
                };

            return Copy(customerVisits: visits);
        }

        public StoreManagementDayDetailRecord SetVisitOutcome(
            string customerId,
            StoreCustomerVisitOutcome outcome)
        {
            if (IsClosed)
            {
                return this;
            }

            List<StoreCustomerVisitDetailRecord> visits =
                new List<StoreCustomerVisitDetailRecord>(_customerVisits);

            for (int index = 0; index < visits.Count; index++)
            {
                if (!string.Equals(
                        visits[index].CustomerId,
                        customerId,
                        StringComparison.Ordinal))
                {
                    continue;
                }

                StoreCustomerVisitDetailRecord replacement =
                    visits[index].WithOutcome(outcome);
                if (ReferenceEquals(replacement, visits[index]))
                {
                    return this;
                }

                visits[index] = replacement;
                return Copy(customerVisits: visits);
            }

            visits.Add(
                new StoreCustomerVisitDetailRecord(
                    customerId,
                    outcome));
            return Copy(customerVisits: visits);
        }

        public StoreManagementDayDetailRecord AddSale(
            StoreSaleDetailRecord sale)
        {
            if (IsClosed)
            {
                return this;
            }

            if (sale == null)
            {
                throw new ArgumentNullException(nameof(sale));
            }

            foreach (StoreSaleDetailRecord existing in _sales)
            {
                if (string.Equals(
                        existing.TransactionId,
                        sale.TransactionId,
                        StringComparison.Ordinal))
                {
                    return this;
                }
            }

            List<StoreSaleDetailRecord> sales =
                new List<StoreSaleDetailRecord>(_sales)
                {
                    sale
                };
            return Copy(sales: sales);
        }

        public StoreManagementDayDetailRecord AddReceipt(
            StoreReceiptDetailRecord receipt)
        {
            if (IsClosed)
            {
                return this;
            }

            if (receipt == null)
            {
                throw new ArgumentNullException(nameof(receipt));
            }

            foreach (StoreReceiptDetailRecord existing in _receipts)
            {
                if (string.Equals(
                        existing.ReceiptId,
                        receipt.ReceiptId,
                        StringComparison.Ordinal))
                {
                    return this;
                }
            }

            List<StoreReceiptDetailRecord> receipts =
                new List<StoreReceiptDetailRecord>(_receipts)
                {
                    receipt
                };
            return Copy(receipts: receipts);
        }

        public StoreManagementDayDetailRecord Close(
            long endingCashCents,
            long taxCents)
        {
            if (endingCashCents < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(endingCashCents));
            }

            if (taxCents < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(taxCents));
            }

            if (IsClosed &&
                EndingCashCents == endingCashCents &&
                TaxCents == taxCents)
            {
                return this;
            }

            return Copy(
                isClosed: true,
                endingCashCents: endingCashCents,
                taxCents: taxCents);
        }

        public StoreDailySummaryRecord ToSummary()
        {
            return new StoreDailySummaryRecord(
                DayNumber,
                DayId,
                StartingCashCents,
                EndingCashCents,
                VisitorCount,
                BuyerCount,
                AbandonedCustomerCount,
                CompletedSales,
                UnitsSold,
                OrdersReceived,
                RevenueCents,
                SupplierCostCents,
                TaxCents);
        }

        private StoreManagementDayDetailRecord Copy(
            bool? isClosed = null,
            long? endingCashCents = null,
            long? taxCents = null,
            IEnumerable<StoreCustomerVisitDetailRecord> customerVisits = null,
            IEnumerable<StoreSaleDetailRecord> sales = null,
            IEnumerable<StoreReceiptDetailRecord> receipts = null)
        {
            return new StoreManagementDayDetailRecord(
                DayNumber,
                DayId,
                isClosed ?? IsClosed,
                StartingCashCents,
                endingCashCents ?? EndingCashCents,
                taxCents ?? TaxCents,
                customerVisits ?? _customerVisits,
                sales ?? _sales,
                receipts ?? _receipts);
        }

        private int CountVisitOutcome(StoreCustomerVisitOutcome outcome)
        {
            int count = 0;
            foreach (StoreCustomerVisitDetailRecord visit in _customerVisits)
            {
                if (visit.Outcome == outcome)
                {
                    count++;
                }
            }
            return count;
        }

        private int SumSaleUnits()
        {
            int total = 0;
            foreach (StoreSaleDetailRecord sale in _sales)
            {
                total = checked(total + sale.Units);
            }
            return total;
        }

        private long SumRevenue()
        {
            long total = 0;
            foreach (StoreSaleDetailRecord sale in _sales)
            {
                total = checked(total + sale.RevenueCents);
            }
            return total;
        }

        private long SumSupplierCosts()
        {
            long total = 0;
            foreach (StoreReceiptDetailRecord receipt in _receipts)
            {
                total = checked(total + receipt.SupplierCostCents);
            }
            return total;
        }

        private static ReadOnlyCollection<T> CopyUnique<T>(
            IEnumerable<T> source,
            string parameterName,
            Func<T, string> idSelector,
            string label)
            where T : class
        {
            if (source == null)
            {
                throw new ArgumentNullException(parameterName);
            }

            List<T> copy = new List<T>();
            HashSet<string> ids = new HashSet<string>(StringComparer.Ordinal);

            foreach (T item in source)
            {
                if (item == null)
                {
                    throw new ArgumentException(
                        "Collection cannot contain null.",
                        parameterName);
                }

                if (!ids.Add(idSelector(item)))
                {
                    throw new ArgumentException(
                        "Duplicate " + label + " ID.",
                        parameterName);
                }

                copy.Add(item);
            }

            return new ReadOnlyCollection<T>(copy);
        }

        private static string Required(
            string value,
            string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "A day ID is required.",
                    parameterName);
            }

            return value;
        }
    }

    public sealed class StoreDailySummaryRecord
    {
        public int DayNumber { get; }
        public string DayId { get; }
        public long StartingCashCents { get; }
        public long EndingCashCents { get; }
        public int Visitors { get; }
        public int Buyers { get; }
        public int AbandonedCustomers { get; }
        public int CompletedSales { get; }
        public int UnitsSold { get; }
        public int OrdersReceived { get; }
        public long RevenueCents { get; }
        public long SupplierCostCents { get; }
        public long TaxCents { get; }
        public long GrossResultCents => checked(
            RevenueCents - SupplierCostCents);
        public long NetResultCents => checked(
            GrossResultCents - TaxCents);

        public StoreDailySummaryRecord(
            int dayNumber,
            string dayId,
            long startingCashCents,
            long endingCashCents,
            int visitors,
            int buyers,
            int abandonedCustomers,
            int completedSales,
            int unitsSold,
            int ordersReceived,
            long revenueCents,
            long supplierCostCents,
            long taxCents)
        {
            if (dayNumber < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(dayNumber));
            }

            if (startingCashCents < 0 || endingCashCents < 0 ||
                revenueCents < 0 || supplierCostCents < 0 || taxCents < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(startingCashCents));
            }

            if (visitors < 0 || buyers < 0 || abandonedCustomers < 0 ||
                completedSales < 0 || unitsSold < 0 || ordersReceived < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(visitors));
            }

            if (buyers > visitors || abandonedCustomers > visitors ||
                checked(buyers + abandonedCustomers) > visitors)
            {
                throw new ArgumentException(
                    "Resolved customer outcomes cannot exceed visitors.");
            }

            DayNumber = dayNumber;
            DayId = Required(dayId);
            StartingCashCents = startingCashCents;
            EndingCashCents = endingCashCents;
            Visitors = visitors;
            Buyers = buyers;
            AbandonedCustomers = abandonedCustomers;
            CompletedSales = completedSales;
            UnitsSold = unitsSold;
            OrdersReceived = ordersReceived;
            RevenueCents = revenueCents;
            SupplierCostCents = supplierCostCents;
            TaxCents = taxCents;
        }

        private static string Required(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("A day ID is required.", nameof(value));
            }

            return value;
        }
    }

    public sealed class StoreManagementHistory
    {
        public const int DetailedDayCapacity = 3;

        private readonly ReadOnlyCollection<StoreManagementDayDetailRecord>
            _recentDayDetails;
        private readonly ReadOnlyCollection<StoreDailySummaryRecord>
            _dailySummaries;

        public IReadOnlyList<StoreManagementDayDetailRecord>
            RecentDayDetails => _recentDayDetails;
        public IReadOnlyList<StoreDailySummaryRecord>
            DailySummaries => _dailySummaries;

        public StoreManagementHistory(
            IEnumerable<StoreManagementDayDetailRecord> recentDayDetails,
            IEnumerable<StoreDailySummaryRecord> dailySummaries)
        {
            _recentDayDetails = CopyDetails(recentDayDetails);
            _dailySummaries = CopySummaries(dailySummaries);
            Validate();
        }

        public static StoreManagementHistory Empty()
        {
            return new StoreManagementHistory(
                new StoreManagementDayDetailRecord[0],
                new StoreDailySummaryRecord[0]);
        }

        public StoreManagementDayDetailRecord FindDetailedDay(int dayNumber)
        {
            foreach (StoreManagementDayDetailRecord detail in _recentDayDetails)
            {
                if (detail.DayNumber == dayNumber)
                {
                    return detail;
                }
            }

            return null;
        }

        public StoreDailySummaryRecord FindSummary(int dayNumber)
        {
            foreach (StoreDailySummaryRecord summary in _dailySummaries)
            {
                if (summary.DayNumber == dayNumber)
                {
                    return summary;
                }
            }

            return null;
        }

        public StoreManagementHistory EnsureDay(
            int dayNumber,
            string dayId,
            long startingCashCents)
        {
            if (FindDetailedDay(dayNumber) != null ||
                FindSummary(dayNumber) != null)
            {
                return this;
            }

            List<StoreManagementDayDetailRecord> details =
                new List<StoreManagementDayDetailRecord>(_recentDayDetails)
                {
                    StoreManagementDayDetailRecord.Open(
                        dayNumber,
                        dayId,
                        startingCashCents)
                };
            details.Sort((left, right) => left.DayNumber.CompareTo(right.DayNumber));

            List<StoreDailySummaryRecord> summaries =
                new List<StoreDailySummaryRecord>(_dailySummaries);

            while (details.Count > DetailedDayCapacity)
            {
                StoreManagementDayDetailRecord oldest = details[0];
                details.RemoveAt(0);
                UpsertSummary(summaries, oldest.ToSummary());
            }

            summaries.Sort((left, right) => left.DayNumber.CompareTo(right.DayNumber));
            return new StoreManagementHistory(details, summaries);
        }

        public StoreManagementHistory RecordVisit(
            int dayNumber,
            string dayId,
            long startingCashCents,
            string customerId)
        {
            StoreManagementHistory ensured =
                EnsureDay(dayNumber, dayId, startingCashCents);
            return ensured.ReplaceDetail(
                dayNumber,
                detail => detail.AddVisit(customerId));
        }

        public StoreManagementHistory RecordVisitOutcome(
            int dayNumber,
            string dayId,
            long startingCashCents,
            string customerId,
            StoreCustomerVisitOutcome outcome)
        {
            StoreManagementHistory ensured =
                EnsureDay(dayNumber, dayId, startingCashCents);
            return ensured.ReplaceDetail(
                dayNumber,
                detail => detail.SetVisitOutcome(customerId, outcome));
        }

        public StoreManagementHistory RecordSale(
            int dayNumber,
            string dayId,
            long startingCashCents,
            StoreSaleDetailRecord sale)
        {
            StoreManagementHistory ensured =
                EnsureDay(dayNumber, dayId, startingCashCents);
            return ensured.ReplaceDetail(
                dayNumber,
                detail => detail.AddSale(sale));
        }

        public StoreManagementHistory RecordReceipt(
            int dayNumber,
            string dayId,
            long startingCashCents,
            StoreReceiptDetailRecord receipt)
        {
            StoreManagementHistory ensured =
                EnsureDay(dayNumber, dayId, startingCashCents);
            return ensured.ReplaceDetail(
                dayNumber,
                detail => detail.AddReceipt(receipt));
        }

        public StoreManagementHistory CloseDay(
            int dayNumber,
            string dayId,
            long startingCashCents,
            long endingCashCents,
            long taxCents)
        {
            StoreManagementHistory ensured =
                EnsureDay(dayNumber, dayId, startingCashCents);
            return ensured.ReplaceDetail(
                dayNumber,
                detail => detail.Close(endingCashCents, taxCents));
        }

        private StoreManagementHistory ReplaceDetail(
            int dayNumber,
            Func<StoreManagementDayDetailRecord,
                StoreManagementDayDetailRecord> replace)
        {
            List<StoreManagementDayDetailRecord> details =
                new List<StoreManagementDayDetailRecord>(_recentDayDetails);

            for (int index = 0; index < details.Count; index++)
            {
                if (details[index].DayNumber != dayNumber)
                {
                    continue;
                }

                StoreManagementDayDetailRecord replacement =
                    replace(details[index]);
                if (ReferenceEquals(replacement, details[index]))
                {
                    return this;
                }

                details[index] = replacement;
                return new StoreManagementHistory(details, _dailySummaries);
            }

            throw new InvalidOperationException("Detailed day was not created.");
        }

        private void Validate()
        {
            if (_recentDayDetails.Count > DetailedDayCapacity)
            {
                throw new ArgumentException(
                    "Detailed history exceeds the configured window.");
            }

            HashSet<int> dayNumbers = new HashSet<int>();
            int previous = 0;
            foreach (StoreManagementDayDetailRecord detail in _recentDayDetails)
            {
                if (!dayNumbers.Add(detail.DayNumber))
                {
                    throw new ArgumentException("Duplicate detailed day.");
                }

                if (detail.DayNumber <= previous)
                {
                    throw new ArgumentException("Detailed days must be ordered.");
                }
                previous = detail.DayNumber;
            }

            previous = 0;
            foreach (StoreDailySummaryRecord summary in _dailySummaries)
            {
                if (!dayNumbers.Add(summary.DayNumber))
                {
                    throw new ArgumentException("Duplicate history day.");
                }

                if (summary.DayNumber <= previous)
                {
                    throw new ArgumentException("Daily summaries must be ordered.");
                }
                previous = summary.DayNumber;
            }
        }

        private static void UpsertSummary(
            List<StoreDailySummaryRecord> summaries,
            StoreDailySummaryRecord summary)
        {
            for (int index = 0; index < summaries.Count; index++)
            {
                if (summaries[index].DayNumber != summary.DayNumber)
                {
                    continue;
                }

                summaries[index] = summary;
                return;
            }

            summaries.Add(summary);
        }

        private static ReadOnlyCollection<StoreManagementDayDetailRecord>
            CopyDetails(IEnumerable<StoreManagementDayDetailRecord> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            List<StoreManagementDayDetailRecord> copy =
                new List<StoreManagementDayDetailRecord>();
            foreach (StoreManagementDayDetailRecord item in source)
            {
                if (item == null)
                {
                    throw new ArgumentException("History cannot contain null.", nameof(source));
                }
                copy.Add(item);
            }
            return new ReadOnlyCollection<StoreManagementDayDetailRecord>(copy);
        }

        private static ReadOnlyCollection<StoreDailySummaryRecord>
            CopySummaries(IEnumerable<StoreDailySummaryRecord> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            List<StoreDailySummaryRecord> copy =
                new List<StoreDailySummaryRecord>();
            foreach (StoreDailySummaryRecord item in source)
            {
                if (item == null)
                {
                    throw new ArgumentException("History cannot contain null.", nameof(source));
                }
                copy.Add(item);
            }
            return new ReadOnlyCollection<StoreDailySummaryRecord>(copy);
        }
    }
}
