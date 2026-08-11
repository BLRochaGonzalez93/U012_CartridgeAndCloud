using System;
using System.Collections.Generic;
using System.Globalization;
using VRMGames.CartridgeAndCloud.Domain.Persistence;
using VRMGames.CartridgeAndCloud.Domain.DayCycle;
using VRMGames.CartridgeAndCloud.Domain.UIUX;
using VRMGames.CartridgeAndCloud.Domain.Store;

using VRMGames.CartridgeAndCloud.Domain.Checkout;
using VRMGames.CartridgeAndCloud.Domain.Economy;
using VRMGames.CartridgeAndCloud.Domain.Inventory;
namespace VRMGames.CartridgeAndCloud.Application.UIUX
{
    public sealed class StoreUiProjectionService
    {
        public StoreHudSnapshot BuildHud(
            IntegratedGameStateSnapshot snapshot,
            DailyAutosaveStatus saveStatus)
        {
            if (snapshot == null)
            {
                throw new ArgumentNullException(
                    nameof(snapshot));
            }

            int activeCustomers = 0;

            foreach (CustomerSaveRecord customer
                     in snapshot.Customers)
            {
                if (!string.Equals(
                        customer.State,
                        "Despawned",
                        StringComparison.Ordinal))
                {
                    activeCustomers++;
                }
            }

            return new StoreHudSnapshot(
                snapshot.CurrentDay,
                snapshot.DayCycle.State,
                snapshot.DayCycle.ElapsedOpenSeconds,
                snapshot.DayCycle.OpenDurationSeconds,
                snapshot.CashCents,
                snapshot.CurrencyCode,
                activeCustomers,
                snapshot.QueueEntries.Count,
                snapshot.CheckoutStation.State,
                saveStatus.ToString());
        }

        public static bool
            RequiresWeeklySummaryAcknowledgement(
                IntegratedGameStateSnapshot snapshot)
        {
            return snapshot != null &&
                snapshot.CurrentDay % 7 == 0 &&
                string.Equals(
                    snapshot.DayCycle.State,
                    "Closed",
                    StringComparison.Ordinal) &&
                StoreTradingHoursPolicy.IsDayComplete(
                    snapshot.DayCycle.ElapsedDaySeconds,
                    snapshot.DayCycle.DayDurationSeconds);
        }

        public WeeklySummarySnapshot BuildWeeklySummary(
            IntegratedGameStateSnapshot snapshot,
            StoreOperationsState operationsState)
        {
            if (snapshot == null)
            {
                throw new ArgumentNullException(
                    nameof(snapshot));
            }

            if (operationsState == null)
            {
                throw new ArgumentNullException(
                    nameof(operationsState));
            }

            if (!RequiresWeeklySummaryAcknowledgement(
                    snapshot))
            {
                throw new InvalidOperationException(
                    "A weekly summary requires a closed seventh day at 24:00.");
            }

            int weekNumber = snapshot.CurrentDay / 7;
            if (operationsState.LastSettledWeek !=
                weekNumber)
            {
                throw new InvalidOperationException(
                    "Weekly economy must be settled before building the summary.");
            }

            int firstDay = checked(
                (weekNumber - 1) * 7 + 1);
            int lastDay = checked(
                weekNumber * 7);
            int historyDays = 0;
            long revenue = 0;
            long supplierCosts = 0;
            int visitors = 0;
            int buyers = 0;
            int abandonedCustomers = 0;
            int completedSales = 0;
            int unitsSold = 0;
            int ordersReceived = 0;

            for (int day = firstDay;
                 day <= lastDay;
                 day++)
            {
                StoreManagementDayDetailRecord detail =
                    operationsState.ManagementHistory
                        .FindDetailedDay(day);

                if (detail != null)
                {
                    historyDays++;
                    revenue = checked(
                        revenue + detail.RevenueCents);
                    supplierCosts = checked(
                        supplierCosts +
                        detail.SupplierCostCents);
                    visitors = checked(
                        visitors + detail.VisitorCount);
                    buyers = checked(
                        buyers + detail.BuyerCount);
                    abandonedCustomers = checked(
                        abandonedCustomers +
                        detail.AbandonedCustomerCount);
                    completedSales = checked(
                        completedSales +
                        detail.CompletedSales);
                    unitsSold = checked(
                        unitsSold + detail.UnitsSold);
                    ordersReceived = checked(
                        ordersReceived +
                        detail.OrdersReceived);
                    continue;
                }

                StoreDailySummaryRecord summary =
                    operationsState.ManagementHistory
                        .FindSummary(day);

                if (summary == null)
                {
                    continue;
                }

                historyDays++;
                revenue = checked(
                    revenue + summary.RevenueCents);
                supplierCosts = checked(
                    supplierCosts +
                    summary.SupplierCostCents);
                visitors = checked(
                    visitors + summary.Visitors);
                buyers = checked(
                    buyers + summary.Buyers);
                abandonedCustomers = checked(
                    abandonedCustomers +
                    summary.AbandonedCustomers);
                completedSales = checked(
                    completedSales +
                    summary.CompletedSales);
                unitsSold = checked(
                    unitsSold + summary.UnitsSold);
                ordersReceived = checked(
                    ordersReceived +
                    summary.OrdersReceived);
            }

            bool completeHistory =
                historyDays == 7 &&
                checked(revenue - supplierCosts) ==
                    operationsState
                        .LastWeeklyGrossResultCents;
            long tax =
                operationsState.LastWeeklyTaxCents;

            return new WeeklySummarySnapshot(
                weekNumber,
                firstDay,
                lastDay,
                revenue,
                supplierCosts,
                operationsState
                    .LastWeeklyGrossResultCents,
                tax,
                checked(snapshot.CashCents + tax),
                snapshot.CashCents,
                visitors,
                buyers,
                abandonedCustomers,
                completedSales,
                unitsSold,
                ordersReceived,
                snapshot.CurrencyCode,
                completeHistory);
        }

        public ManagementPanelSnapshot BuildPanel(
            IntegratedGameStateSnapshot snapshot,
            ManagementPanelId panelId,
            StoreOperationsState operationsState = null)
        {
            if (snapshot == null)
            {
                throw new ArgumentNullException(
                    nameof(snapshot));
            }

            switch (panelId)
            {
                case ManagementPanelId.Overview:
                    return Overview(snapshot, operationsState);
                case ManagementPanelId.Inventory:
                    return Inventory(snapshot);
                case ManagementPanelId.Suppliers:
                    return Suppliers(snapshot, operationsState);
                case ManagementPanelId.Displays:
                    return Displays(snapshot);
                case ManagementPanelId.Customers:
                    return Customers(snapshot, operationsState);
                case ManagementPanelId.Shopping:
                    return Shopping(snapshot);
                case ManagementPanelId.Checkout:
                    return Checkout(snapshot);
                case ManagementPanelId.DayCycle:
                    return DayCycle(snapshot);
                case ManagementPanelId.Economy:
                    return Economy(snapshot, operationsState);
                case ManagementPanelId.History:
                    return History(snapshot, operationsState);
                case ManagementPanelId.Weekly:
                    return Weekly(snapshot, operationsState);
                case ManagementPanelId.Help:
                    return Help();
                case ManagementPanelId.Accessibility:
                    return Accessibility();
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(panelId));
            }
        }

        private static ManagementPanelSnapshot Inventory(
            IntegratedGameStateSnapshot snapshot)
        {
            List<ManagementPanelRow> rows =
                new List<ManagementPanelRow>();

            foreach (InventoryContainerSaveRecord
                     inventory in snapshot.Inventories)
            {
                int units = 0;

                foreach (ProductQuantitySaveRecord product
                         in inventory.Products)
                {
                    units += product.Quantity;
                }

                rows.Add(Row(
                    inventory.ContainerId,
                    $"{units}/{inventory.Capacity}",
                    units == 0
                        ? "Empty"
                        : "Stocked"));

                foreach (ProductQuantitySaveRecord product
                         in inventory.Products)
                {
                    rows.Add(Row(
                        "  " + product.ProductId,
                        product.Quantity.ToString(
                            CultureInfo.InvariantCulture),
                        "On hand"));
                }
            }

            return Panel(
                ManagementPanelId.Inventory,
                "Inventory",
                rows);
        }

        private static ManagementPanelSnapshot Suppliers(
            IntegratedGameStateSnapshot snapshot,
            StoreOperationsState operationsState)
        {
            List<ManagementPanelRow> rows =
                new List<ManagementPanelRow>();

            if (operationsState != null)
            {
                foreach (StoreOrderRecord order in operationsState.Orders)
                {
                    rows.Add(Row(
                        order.OrderId,
                        FormatMoney(order.TotalCostCents, snapshot.CurrencyCode),
                        order.State + " · " + order.ItemId +
                        (order.ReservedCostCents > 0
                            ? " · reserved " + FormatMoney(
                                order.ReservedCostCents,
                                snapshot.CurrencyCode)
                            : string.Empty) +
                        (!string.IsNullOrWhiteSpace(order.DeliveryRunId)
                            ? " · " + order.DeliveryRunId
                            : string.Empty)));
                }

                foreach (StoreDeliveryRunRecord run in operationsState.DeliveryRuns)
                {
                    rows.Add(Row(
                        run.DeliveryRunId,
                        FormatMoney(run.TotalCostCents, snapshot.CurrencyCode),
                        run.Status + " · " + run.OrderIds.Count + " order(s)"));
                }
            }
            else
            {
                foreach (SupplierOrderSaveRecord order
                         in snapshot.SupplierOrders)
                {
                    rows.Add(Row(
                        order.OrderId,
                        $"{order.ReceivedUnits}/" +
                        $"{order.OrderedUnits}",
                        order.State + " · " +
                        FormatMoney(
                            order.UnitCostCents,
                            snapshot.CurrencyCode) +
                        " each"));
                }
            }

            if (rows.Count == 0)
            {
                rows.Add(Row(
                    "Orders",
                    "0",
                    "No supplier orders"));
            }

            return Panel(
                ManagementPanelId.Suppliers,
                "Suppliers, Orders & Delivery Runs",
                rows);
        }

        private static ManagementPanelSnapshot Displays(
            IntegratedGameStateSnapshot snapshot)
        {
            List<ManagementPanelRow> rows =
                new List<ManagementPanelRow>();

            foreach (DisplaySaveRecord display
                     in snapshot.Displays)
            {
                rows.Add(Row(
                    display.DisplayId,
                    display.HasAssignedProduct
                        ? display.AssignedProductId
                        : "Unassigned",
                    display.InventoryContainerId));
            }

            if (rows.Count == 0)
            {
                rows.Add(Row(
                    "Displays",
                    "0",
                    "No displays registered"));
            }

            return Panel(
                ManagementPanelId.Displays,
                "Displays & Restocking",
                rows);
        }

        private static ManagementPanelSnapshot Customers(
            IntegratedGameStateSnapshot snapshot,
            StoreOperationsState operationsState)
        {
            List<ManagementPanelRow> rows =
                new List<ManagementPanelRow>();

            StoreManagementDayDetailRecord day =
                CurrentDay(operationsState, snapshot.CurrentDay);
            if (day != null)
            {
                rows.Add(Row(
                    "Visitors",
                    day.VisitorCount.ToString(CultureInfo.InvariantCulture),
                    "Admitted today"));
                rows.Add(Row(
                    "Buyers",
                    day.BuyerCount.ToString(CultureInfo.InvariantCulture),
                    "Visits ending in purchase"));
                rows.Add(Row(
                    "Abandoned",
                    day.AbandonedCustomerCount.ToString(CultureInfo.InvariantCulture),
                    "Visits ending without purchase"));
            }

            foreach (CustomerSaveRecord customer
                     in snapshot.Customers)
            {
                rows.Add(Row(
                    customer.CustomerId,
                    customer.State,
                    $"Patience " +
                    $"{customer.RemainingPatienceSeconds}s"));
            }

            if (rows.Count == 0)
            {
                rows.Add(Row(
                    "Customers",
                    "0",
                    "No customer activity"));
            }

            return Panel(
                ManagementPanelId.Customers,
                "Customers",
                rows);
        }

        private static ManagementPanelSnapshot Shopping(
            IntegratedGameStateSnapshot snapshot)
        {
            List<ManagementPanelRow> rows =
                new List<ManagementPanelRow>();

            foreach (ShoppingSessionSaveRecord session
                     in snapshot.ShoppingSessions)
            {
                rows.Add(Row(
                    session.CustomerId,
                    session.State,
                    "Cart " + session.CartId));
            }

            foreach (ReservationSaveRecord reservation
                     in snapshot.Reservations)
            {
                rows.Add(Row(
                    "Reservation " +
                    reservation.ReservationId,
                    reservation.Quantity.ToString(
                        CultureInfo.InvariantCulture),
                    reservation.State + " · " +
                    reservation.ProductId));
            }

            if (rows.Count == 0)
            {
                rows.Add(Row(
                    "Shopping",
                    "0",
                    "No sessions or reservations"));
            }

            return Panel(
                ManagementPanelId.Shopping,
                "Shopping & Reservations",
                rows);
        }

        private static ManagementPanelSnapshot Checkout(
            IntegratedGameStateSnapshot snapshot)
        {
            List<ManagementPanelRow> rows =
                new List<ManagementPanelRow>
                {
                    Row(
                        snapshot.CheckoutStation.StationId,
                        snapshot.CheckoutStation.State,
                        string.IsNullOrWhiteSpace(
                            snapshot.CheckoutStation
                                .CurrentEntryId)
                            ? "No current entry"
                            : snapshot.CheckoutStation
                                .CurrentEntryId)
                };

            foreach (CheckoutQueueEntrySaveRecord entry
                     in snapshot.QueueEntries)
            {
                rows.Add(Row(
                    $"#{entry.Position} " +
                    entry.CustomerId,
                    entry.State,
                    entry.EntryId));
            }

            rows.Add(Row(
                "Transactions",
                snapshot.Transactions.Count.ToString(
                    CultureInfo.InvariantCulture),
                "Completed and persisted"));

            return Panel(
                ManagementPanelId.Checkout,
                "Queue & Checkout",
                rows);
        }

        private static ManagementPanelSnapshot DayCycle(
            IntegratedGameStateSnapshot snapshot)
        {
            return Panel(
                ManagementPanelId.DayCycle,
                "Day Cycle",
                new[]
                {
                    Row(
                        "Day",
                        snapshot.CurrentDay.ToString(
                            CultureInfo.InvariantCulture),
                        snapshot.DayCycle.DayId),
                    Row(
                        "State",
                        snapshot.DayCycle.State,
                        snapshot.DayCycle.AutoBeginClosing
                            ? "Automatic closing enabled"
                            : "Manual closing"),
                    Row(
                        "Time",
                        StoreTradingHoursPolicy.FormatTime(
                            snapshot.DayCycle.ElapsedDaySeconds,
                            snapshot.DayCycle.DayDurationSeconds),
                        "24-hour SimulationClock authority · Open 08:00–22:00"),
                    Row(
                        "Speed",
                        SimulationSpeedPolicy.Format(
                            snapshot.DayCycle
                                .SimulationSpeedMultiplier),
                        "Pause is independent from speed")
                });
        }

        private static ManagementPanelSnapshot Overview(
            IntegratedGameStateSnapshot snapshot,
            StoreOperationsState operationsState)
        {
            StoreManagementDayDetailRecord day =
                CurrentDay(operationsState, snapshot.CurrentDay);

            long reserved = operationsState == null
                ? 0
                : operationsState.ReservedFundsCents;
            long available = Math.Max(0, snapshot.CashCents - reserved);

            return Panel(
                ManagementPanelId.Overview,
                "Management Overview",
                new[]
                {
                    Row(
                        "Cash",
                        FormatMoney(snapshot.CashCents, snapshot.CurrencyCode),
                        "Current balance"),
                    Row(
                        "Reserved",
                        FormatMoney(reserved, snapshot.CurrencyCode),
                        "Pending supplier orders"),
                    Row(
                        "Available",
                        FormatMoney(available, snapshot.CurrencyCode),
                        "Cash minus reservations"),
                    Row(
                        "Day revenue",
                        FormatMoney(day == null ? 0 : day.RevenueCents, snapshot.CurrencyCode),
                        "Confirmed checkout revenue"),
                    Row(
                        "Day costs",
                        FormatMoney(day == null ? 0 : day.SupplierCostCents, snapshot.CurrencyCode),
                        "Received supplier costs"),
                    Row(
                        "Day tax",
                        FormatMoney(day == null ? 0 : day.TaxCents, snapshot.CurrencyCode),
                        "Weekly tax posted on this day"),
                    Row(
                        "Day net",
                        FormatMoney(day == null ? 0 : day.NetResultCents, snapshot.CurrencyCode),
                        day != null && day.IsClosed ? "Final" : "In progress"),
                    Row(
                        "Visitors",
                        (day == null ? 0 : day.VisitorCount).ToString(CultureInfo.InvariantCulture),
                        "Customers admitted"),
                    Row(
                        "Buyers / Sales",
                        (day == null ? 0 : day.BuyerCount).ToString(CultureInfo.InvariantCulture) +
                        " / " +
                        (day == null ? 0 : day.CompletedSales).ToString(CultureInfo.InvariantCulture),
                        "Purchased visits and transactions")
                });
        }

        private static ManagementPanelSnapshot Economy(
            IntegratedGameStateSnapshot snapshot,
            StoreOperationsState operationsState)
        {
            StoreManagementDayDetailRecord day =
                CurrentDay(operationsState, snapshot.CurrentDay);

            long revenue = day == null ? 0 : day.RevenueCents;
            long costs = day == null ? 0 : day.SupplierCostCents;
            long tax = day == null ? 0 : day.TaxCents;
            long salaries = SumLedgerForDay(
                snapshot.LedgerEntries,
                snapshot.DayCycle.DayId,
                EconomyPostingType.EmployeeSalaryCost);

            if (day == null)
            {
                foreach (EconomyLedgerSaveRecord entry
                         in snapshot.LedgerEntries)
                {
                    if (!string.Equals(
                            entry.DayId,
                            snapshot.DayCycle.DayId,
                            StringComparison.Ordinal))
                    {
                        continue;
                    }

                    if (string.Equals(
                            entry.PostingType,
                            "CheckoutRevenue",
                            StringComparison.Ordinal))
                    {
                        revenue = checked(revenue + entry.MinorUnits);
                    }
                    else if (string.Equals(
                                 entry.PostingType,
                                 "SupplierReceivingCost",
                                 StringComparison.Ordinal))
                    {
                        costs = checked(costs + entry.MinorUnits);
                    }
                    else if (string.Equals(
                                 entry.PostingType,
                                 "WeeklyTax",
                                 StringComparison.Ordinal))
                    {
                        tax = checked(tax + entry.MinorUnits);
                    }
                }
            }

            return Panel(
                ManagementPanelId.Economy,
                "Economy & Daily Results",
                new[]
                {
                    Row(
                        "Cash",
                        FormatMoney(snapshot.CashCents, snapshot.CurrencyCode),
                        "Current session cash"),
                    Row(
                        "Revenue",
                        FormatMoney(revenue, snapshot.CurrencyCode),
                        "Checkout revenue"),
                    Row(
                        "Supplier costs",
                        FormatMoney(costs, snapshot.CurrencyCode),
                        "Received costs"),
                    Row(
                        "Employee salaries",
                        FormatMoney(salaries, snapshot.CurrencyCode),
                        "Salary payments recognized today"),
                    Row(
                        "Gross result",
                        FormatMoney(revenue - costs, snapshot.CurrencyCode),
                        "Revenue minus received costs"),
                    Row(
                        "Operating result",
                        FormatMoney(
                            revenue - costs - salaries,
                            snapshot.CurrencyCode),
                        "Gross result minus employee salaries"),
                    Row(
                        "Tax",
                        FormatMoney(tax, snapshot.CurrencyCode),
                        "Weekly tax recognized once"),
                    Row(
                        "Net result",
                        FormatMoney(
                            revenue - costs - salaries - tax,
                            snapshot.CurrencyCode),
                        day != null && day.IsClosed ? "Final" : "In progress")
                });
        }

        private static ManagementPanelSnapshot History(
            IntegratedGameStateSnapshot snapshot,
            StoreOperationsState operationsState)
        {
            List<ManagementPanelRow> rows =
                new List<ManagementPanelRow>();

            if (operationsState != null)
            {
                for (int index =
                         operationsState.ManagementHistory.RecentDayDetails.Count - 1;
                     index >= 0;
                     index--)
                {
                    StoreManagementDayDetailRecord day =
                        operationsState.ManagementHistory.RecentDayDetails[index];
                    rows.Add(Row(
                        "Day " + day.DayNumber.ToString(CultureInfo.InvariantCulture),
                        FormatMoney(day.NetResultCents, snapshot.CurrencyCode),
                        "Detailed · " + day.CompletedSales + " sales · " +
                        day.VisitorCount + " visitors · " +
                        (day.IsClosed ? "Closed" : "Current")));

                    foreach (StoreSaleDetailRecord sale in day.Sales)
                    {
                        rows.Add(Row(
                            "  Sale " + sale.TransactionId,
                            FormatMoney(sale.RevenueCents, snapshot.CurrencyCode),
                            sale.ProductId + " · " + sale.Units + " unit(s)"));
                    }

                    foreach (StoreReceiptDetailRecord receipt in day.Receipts)
                    {
                        rows.Add(Row(
                            "  Receipt " + receipt.OrderId,
                            FormatMoney(receipt.SupplierCostCents, snapshot.CurrencyCode),
                            receipt.DeliveryRunId + " · " + receipt.ItemId));
                    }
                }

                for (int index =
                         operationsState.ManagementHistory.DailySummaries.Count - 1;
                     index >= 0;
                     index--)
                {
                    StoreDailySummaryRecord day =
                        operationsState.ManagementHistory.DailySummaries[index];
                    rows.Add(Row(
                        "Day " + day.DayNumber.ToString(CultureInfo.InvariantCulture),
                        FormatMoney(day.NetResultCents, snapshot.CurrencyCode),
                        "Summary · " + day.CompletedSales + " sales · " +
                        day.Visitors + " visitors"));
                }
            }

            if (rows.Count == 0)
            {
                rows.Add(Row(
                    "History",
                    "0 days",
                    "No management history recorded"));
            }

            return Panel(
                ManagementPanelId.History,
                "Recent Detail & Daily Summaries",
                rows);
        }

        private static ManagementPanelSnapshot Weekly(
            IntegratedGameStateSnapshot snapshot,
            StoreOperationsState operationsState)
        {
            long revenue = operationsState == null
                ? 0
                : operationsState.WeeklyRevenueCents;
            long costs = operationsState == null
                ? 0
                : operationsState.WeeklySupplierCostCents;
            int currentWeek = ((snapshot.CurrentDay - 1) / 7) + 1;
            long salaries = SumLedgerForWeek(
                snapshot.LedgerEntries,
                currentWeek,
                EconomyPostingType.EmployeeSalaryCost);
            long gross = checked(revenue - costs - salaries);

            return Panel(
                ManagementPanelId.Weekly,
                "Weekly Economy",
                new[]
                {
                    Row(
                        "Current revenue",
                        FormatMoney(revenue, snapshot.CurrencyCode),
                        "Accumulated this week"),
                    Row(
                        "Current supplier costs",
                        FormatMoney(costs, snapshot.CurrencyCode),
                        "Recognized this week"),
                    Row(
                        "Current salaries",
                        FormatMoney(salaries, snapshot.CurrencyCode),
                        "Paid employee salaries this week"),
                    Row(
                        "Current gross",
                        FormatMoney(gross, snapshot.CurrencyCode),
                        "Tax base candidate"),
                    Row(
                        "Last settled week",
                        (operationsState == null ? 0 : operationsState.LastSettledWeek)
                            .ToString(CultureInfo.InvariantCulture),
                        "Applied exactly once"),
                    Row(
                        "Last gross",
                        FormatMoney(
                            operationsState == null ? 0 : operationsState.LastWeeklyGrossResultCents,
                            snapshot.CurrencyCode),
                        "Gross result at settlement"),
                    Row(
                        "Last tax",
                        FormatMoney(
                            operationsState == null ? 0 : operationsState.LastWeeklyTaxCents,
                            snapshot.CurrencyCode),
                        "10% of positive gross result")
                });
        }

        private static long SumLedgerForDay(
            IReadOnlyList<EconomyLedgerSaveRecord> entries,
            string dayId,
            EconomyPostingType postingType)
        {
            long total = 0;
            string postingTypeName = postingType.ToString();

            foreach (EconomyLedgerSaveRecord entry in entries)
            {
                if (!string.Equals(
                        entry.DayId,
                        dayId,
                        StringComparison.Ordinal) ||
                    !string.Equals(
                        entry.PostingType,
                        postingTypeName,
                        StringComparison.Ordinal))
                {
                    continue;
                }

                total = checked(total + entry.MinorUnits);
            }

            return total;
        }

        private static long SumLedgerForWeek(
            IReadOnlyList<EconomyLedgerSaveRecord> entries,
            int weekNumber,
            EconomyPostingType postingType)
        {
            if (weekNumber < 1)
            {
                return 0;
            }

            int firstDay = checked((weekNumber - 1) * 7 + 1);
            int lastDay = checked(weekNumber * 7);
            long total = 0;
            string postingTypeName = postingType.ToString();

            foreach (EconomyLedgerSaveRecord entry in entries)
            {
                if (!string.Equals(
                        entry.PostingType,
                        postingTypeName,
                        StringComparison.Ordinal) ||
                    !TryParseDayNumber(entry.DayId, out int dayNumber) ||
                    dayNumber < firstDay ||
                    dayNumber > lastDay)
                {
                    continue;
                }

                total = checked(total + entry.MinorUnits);
            }

            return total;
        }

        private static bool TryParseDayNumber(
            string dayId,
            out int dayNumber)
        {
            dayNumber = 0;
            const string prefix = "day-";

            return !string.IsNullOrWhiteSpace(dayId) &&
                dayId.StartsWith(prefix, StringComparison.Ordinal) &&
                int.TryParse(
                    dayId.Substring(prefix.Length),
                    NumberStyles.None,
                    CultureInfo.InvariantCulture,
                    out dayNumber) &&
                dayNumber > 0;
        }

        private static StoreManagementDayDetailRecord CurrentDay(
            StoreOperationsState operationsState,
            int dayNumber)
        {
            return operationsState == null
                ? null
                : operationsState.ManagementHistory.FindDetailedDay(dayNumber);
        }

        private static ManagementPanelSnapshot Help()
        {
            return Panel(
                ManagementPanelId.Help,
                "Help",
                new[]
                {
                    Row(
                        "Navigation",
                        "Tab / Shift+Tab",
                        "Move focus"),
                    Row(
                        "Confirm",
                        "Enter / Submit",
                        "Activate focused control"),
                    Row(
                        "Back",
                        "Escape / Cancel",
                        "Close the top layer"),
                    Row(
                        "Management",
                        "HUD buttons",
                        "Open system panels"),
                    Row(
                        "Saving",
                        "Automatic",
                        "Runs once after a valid Closed day")
                });
        }

        private static ManagementPanelSnapshot
            Accessibility()
        {
            return Panel(
                ManagementPanelId.Accessibility,
                "Accessibility",
                new[]
                {
                    Row(
                        "UI scale",
                        "80%–150%",
                        "Global preference"),
                    Row(
                        "Text scale",
                        "80%–150%",
                        "Global preference"),
                    Row(
                        "Reduced motion",
                        "On / Off",
                        "Disables non-essential transitions"),
                    Row(
                        "Tutorial",
                        "Show / Hide / Restart",
                        "Progress is saved per slot"),
                    Row(
                        "Confirmations",
                        "On / Off",
                        "Destructive actions")
                });
        }

        private static ManagementPanelSnapshot Panel(
            ManagementPanelId id,
            string title,
            IEnumerable<ManagementPanelRow> rows)
        {
            return new ManagementPanelSnapshot(
                id,
                title,
                rows);
        }

        private static ManagementPanelRow Row(
            string label,
            string value,
            string status)
        {
            return new ManagementPanelRow(
                label,
                value,
                status);
        }

        public static string FormatMoney(
            long minorUnits,
            string currencyCode)
        {
            bool negative = minorUnits < 0;
            ulong absolute = negative
                ? (ulong)(-(minorUnits + 1)) + 1UL
                : (ulong)minorUnits;
            ulong major = absolute / 100UL;
            ulong minor = absolute % 100UL;

            return (negative ? "-" : string.Empty) +
                major.ToString(
                    CultureInfo.InvariantCulture) +
                "." +
                minor.ToString("00",
                    CultureInfo.InvariantCulture) +
                " " +
                currencyCode;
        }
    }
}
