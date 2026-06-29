using System;
using System.Collections.Generic;
using System.Globalization;
using VRMGames.CartridgeAndCloud.Domain.Persistence;
using VRMGames.CartridgeAndCloud.Domain.UIUX;

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

        public ManagementPanelSnapshot BuildPanel(
            IntegratedGameStateSnapshot snapshot,
            ManagementPanelId panelId)
        {
            if (snapshot == null)
            {
                throw new ArgumentNullException(
                    nameof(snapshot));
            }

            switch (panelId)
            {
                case ManagementPanelId.Inventory:
                    return Inventory(snapshot);
                case ManagementPanelId.Suppliers:
                    return Suppliers(snapshot);
                case ManagementPanelId.Displays:
                    return Displays(snapshot);
                case ManagementPanelId.Customers:
                    return Customers(snapshot);
                case ManagementPanelId.Shopping:
                    return Shopping(snapshot);
                case ManagementPanelId.Checkout:
                    return Checkout(snapshot);
                case ManagementPanelId.DayCycle:
                    return DayCycle(snapshot);
                case ManagementPanelId.Economy:
                    return Economy(snapshot);
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
            IntegratedGameStateSnapshot snapshot)
        {
            List<ManagementPanelRow> rows =
                new List<ManagementPanelRow>();

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

            if (rows.Count == 0)
            {
                rows.Add(Row(
                    "Orders",
                    "0",
                    "No supplier orders"));
            }

            return Panel(
                ManagementPanelId.Suppliers,
                "Suppliers & Orders",
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
            IntegratedGameStateSnapshot snapshot)
        {
            List<ManagementPanelRow> rows =
                new List<ManagementPanelRow>();

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
                    "No active customers"));
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
                        $"{snapshot.DayCycle.ElapsedOpenSeconds}/" +
                        $"{snapshot.DayCycle.OpenDurationSeconds}s",
                        "Logical integer seconds")
                });
        }

        private static ManagementPanelSnapshot Economy(
            IntegratedGameStateSnapshot snapshot)
        {
            long revenue = 0;
            long costs = 0;

            foreach (EconomyLedgerSaveRecord entry
                     in snapshot.LedgerEntries)
            {
                if (string.Equals(
                        entry.PostingType,
                        "CheckoutRevenue",
                        StringComparison.Ordinal))
                {
                    revenue += entry.MinorUnits;
                }
                else if (string.Equals(
                             entry.PostingType,
                             "SupplierReceivingCost",
                             StringComparison.Ordinal))
                {
                    costs += entry.MinorUnits;
                }
            }

            return Panel(
                ManagementPanelId.Economy,
                "Economy & Daily Results",
                new[]
                {
                    Row(
                        "Cash",
                        FormatMoney(
                            snapshot.CashCents,
                            snapshot.CurrencyCode),
                        "Current session cash"),
                    Row(
                        "Revenue",
                        FormatMoney(
                            revenue,
                            snapshot.CurrencyCode),
                        "Checkout revenue"),
                    Row(
                        "Supplier costs",
                        FormatMoney(
                            costs,
                            snapshot.CurrencyCode),
                        "Received costs"),
                    Row(
                        "Gross result",
                        FormatMoney(
                            revenue - costs,
                            snapshot.CurrencyCode),
                        "Revenue minus received costs")
                });
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
