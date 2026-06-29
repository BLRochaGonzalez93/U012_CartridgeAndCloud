using System;
using System.Collections.Generic;
using VRMGames.CartridgeAndCloud.Domain.Persistence;

namespace VRMGames.CartridgeAndCloud.Application.VerticalSlicePhase1
{
    internal static class IntegratedSnapshotPhase1Mutator
    {
        public static IntegratedGameStateSnapshot
            Clone(
                IntegratedGameStateSnapshot source,
                DateTime updatedUtc,
                long? cashCents = null,
                IEnumerable<
                    InventoryContainerSaveRecord>
                        inventories = null,
                IEnumerable<
                    SupplierOrderSaveRecord>
                        supplierOrders = null,
                IEnumerable<DisplaySaveRecord>
                    displays = null,
                IEnumerable<CustomerSaveRecord>
                    customers = null,
                IEnumerable<ShoppingSessionSaveRecord>
                    shoppingSessions = null,
                IEnumerable<ReservationSaveRecord>
                    reservations = null,
                IEnumerable<CheckoutQueueEntrySaveRecord>
                    queueEntries = null,
                CheckoutStationSaveRecord
                    checkoutStation = null,
                IEnumerable<CheckoutTransactionSaveRecord>
                    transactions = null,
                IEnumerable<
                    EconomyLedgerSaveRecord>
                        ledgerEntries = null)
        {
            if (source == null)
            {
                throw new ArgumentNullException(
                    nameof(source));
            }

            return new IntegratedGameStateSnapshot(
                source.SchemaVersion,
                source.SessionId,
                source.SlotId,
                source.CreatedUtc,
                updatedUtc,
                source.CurrentDay,
                cashCents ?? source.CashCents,
                source.CurrencyCode,
                inventories ?? source.Inventories,
                supplierOrders ??
                    source.SupplierOrders,
                displays ?? source.Displays,
                customers ?? source.Customers,
                shoppingSessions ??
                    source.ShoppingSessions,
                reservations ?? source.Reservations,
                queueEntries ?? source.QueueEntries,
                checkoutStation ??
                    source.CheckoutStation,
                transactions ?? source.Transactions,
                source.DayCycle,
                ledgerEntries ??
                    source.LedgerEntries);
        }

        public static List<
            SupplierOrderSaveRecord>
            UpsertSupplierOrder(
                IntegratedGameStateSnapshot source,
                Phase1OrderProjection projection)
        {
            List<SupplierOrderSaveRecord> orders =
                new List<SupplierOrderSaveRecord>();

            bool replaced = false;

            foreach (SupplierOrderSaveRecord order
                     in source.SupplierOrders)
            {
                if (string.Equals(
                        order.OrderId,
                        projection.OrderId,
                        StringComparison.Ordinal))
                {
                    orders.Add(
                        projection.ToSaveRecord());
                    replaced = true;
                }
                else
                {
                    orders.Add(order);
                }
            }

            if (!replaced)
            {
                orders.Add(
                    projection.ToSaveRecord());
            }

            return orders;
        }

        public static List<
            EconomyLedgerSaveRecord>
            AppendLedger(
                IntegratedGameStateSnapshot source,
                string entryId,
                string postingType,
                string sourceId,
                long amountCents)
        {
            List<EconomyLedgerSaveRecord> ledger =
                new List<EconomyLedgerSaveRecord>(
                    source.LedgerEntries);

            ledger.Add(
                new EconomyLedgerSaveRecord(
                    entryId,
                    postingType,
                    sourceId,
                    source.DayCycle.DayId,
                    amountCents,
                    source.CurrencyCode));

            return ledger;
        }

        public static List<
            InventoryContainerSaveRecord>
            AddProductToContainer(
                IntegratedGameStateSnapshot source,
                string containerId,
                int capacity,
                string productId,
                int delta)
        {
            List<InventoryContainerSaveRecord>
                inventories =
                    new List<
                        InventoryContainerSaveRecord>();

            bool found = false;

            foreach (InventoryContainerSaveRecord
                     inventory in source.Inventories)
            {
                if (!string.Equals(
                        inventory.ContainerId,
                        containerId,
                        StringComparison.Ordinal))
                {
                    inventories.Add(inventory);
                    continue;
                }

                inventories.Add(
                    ChangeProduct(
                        inventory,
                        productId,
                        delta));
                found = true;
            }

            if (!found)
            {
                if (delta < 0)
                {
                    throw new InvalidOperationException(
                        "Cannot remove stock from a missing container.");
                }

                inventories.Add(
                    new InventoryContainerSaveRecord(
                        containerId,
                        capacity,
                        delta == 0
                            ? new ProductQuantitySaveRecord[0]
                            : new[]
                            {
                                new ProductQuantitySaveRecord(
                                    productId,
                                    delta)
                            }));
            }

            return inventories;
        }

        public static List<
            InventoryContainerSaveRecord>
            TransferProduct(
                IntegratedGameStateSnapshot source,
                string fromContainerId,
                string toContainerId,
                int toCapacity,
                string productId,
                int quantity)
        {
            if (quantity < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(quantity));
            }

            List<InventoryContainerSaveRecord>
                firstPass =
                    AddProductToContainer(
                        source,
                        fromContainerId,
                        0,
                        productId,
                        -quantity);

            IntegratedGameStateSnapshot interim =
                Clone(
                    source,
                    source.UpdatedUtc,
                    inventories: firstPass);

            return AddProductToContainer(
                interim,
                toContainerId,
                toCapacity,
                productId,
                quantity);
        }

        public static List<DisplaySaveRecord>
            UpsertDisplay(
                IntegratedGameStateSnapshot source,
                string displayId,
                string definitionId,
                string productId,
                string inventoryContainerId)
        {
            List<DisplaySaveRecord> displays =
                new List<DisplaySaveRecord>();

            bool replaced = false;

            foreach (DisplaySaveRecord display
                     in source.Displays)
            {
                if (string.Equals(
                        display.DisplayId,
                        displayId,
                        StringComparison.Ordinal))
                {
                    displays.Add(
                        new DisplaySaveRecord(
                            displayId,
                            definitionId,
                            productId,
                            inventoryContainerId));
                    replaced = true;
                }
                else
                {
                    displays.Add(display);
                }
            }

            if (!replaced)
            {
                displays.Add(
                    new DisplaySaveRecord(
                        displayId,
                        definitionId,
                        productId,
                        inventoryContainerId));
            }

            return displays;
        }


        public static List<
            InventoryContainerSaveRecord>
            RemoveContainer(
                IEnumerable<
                    InventoryContainerSaveRecord>
                        inventories,
                string containerId)
        {
            if (inventories == null)
            {
                throw new ArgumentNullException(
                    nameof(inventories));
            }

            List<InventoryContainerSaveRecord>
                result =
                    new List<
                        InventoryContainerSaveRecord>();

            foreach (InventoryContainerSaveRecord
                     inventory in inventories)
            {
                if (!string.Equals(
                        inventory.ContainerId,
                        containerId,
                        StringComparison.Ordinal))
                {
                    result.Add(inventory);
                }
            }

            return result;
        }

        public static List<DisplaySaveRecord>
            RemoveDisplay(
                IEnumerable<DisplaySaveRecord>
                    displays,
                string displayId)
        {
            if (displays == null)
            {
                throw new ArgumentNullException(
                    nameof(displays));
            }

            List<DisplaySaveRecord> result =
                new List<DisplaySaveRecord>();

            foreach (DisplaySaveRecord display
                     in displays)
            {
                if (!string.Equals(
                        display.DisplayId,
                        displayId,
                        StringComparison.Ordinal))
                {
                    result.Add(display);
                }
            }

            return result;
        }


        public static List<ReservationSaveRecord>
            RemoveReservationsForDisplay(
                IEnumerable<ReservationSaveRecord>
                    reservations,
                string displayId)
        {
            if (reservations == null)
            {
                throw new ArgumentNullException(
                    nameof(reservations));
            }

            List<ReservationSaveRecord> result =
                new List<ReservationSaveRecord>();

            foreach (ReservationSaveRecord reservation
                     in reservations)
            {
                if (!string.Equals(
                        reservation.DisplayId,
                        displayId,
                        StringComparison.Ordinal))
                {
                    result.Add(reservation);
                }
            }

            return result;
        }

        public static int GetProductQuantity(
            IntegratedGameStateSnapshot source,
            string containerId,
            string productId)
        {
            foreach (InventoryContainerSaveRecord
                     inventory in source.Inventories)
            {
                if (!string.Equals(
                        inventory.ContainerId,
                        containerId,
                        StringComparison.Ordinal))
                {
                    continue;
                }

                foreach (ProductQuantitySaveRecord
                         product in inventory.Products)
                {
                    if (string.Equals(
                            product.ProductId,
                            productId,
                            StringComparison.Ordinal))
                    {
                        return product.Quantity;
                    }
                }
            }

            return 0;
        }

        private static InventoryContainerSaveRecord
            ChangeProduct(
                InventoryContainerSaveRecord source,
                string productId,
                int delta)
        {
            List<ProductQuantitySaveRecord> products =
                new List<ProductQuantitySaveRecord>();

            bool found = false;

            foreach (ProductQuantitySaveRecord product
                     in source.Products)
            {
                if (!string.Equals(
                        product.ProductId,
                        productId,
                        StringComparison.Ordinal))
                {
                    products.Add(product);
                    continue;
                }

                int next = checked(
                    product.Quantity + delta);

                if (next < 0)
                {
                    throw new InvalidOperationException(
                        "Insufficient inventory stock.");
                }

                if (next > 0)
                {
                    products.Add(
                        new ProductQuantitySaveRecord(
                            productId,
                            next));
                }

                found = true;
            }

            if (!found)
            {
                if (delta < 0)
                {
                    throw new InvalidOperationException(
                        "Insufficient inventory stock.");
                }

                if (delta > 0)
                {
                    products.Add(
                        new ProductQuantitySaveRecord(
                            productId,
                            delta));
                }
            }

            return new InventoryContainerSaveRecord(
                source.ContainerId,
                source.Capacity,
                products);
        }
    }

    internal sealed class Phase1OrderProjection
    {
        public string OrderId { get; }
        public string ItemId { get; }
        public string State { get; }
        public int OrderedUnits { get; }
        public int ReceivedUnits { get; }
        public long UnitCostCents { get; }

        public Phase1OrderProjection(
            string orderId,
            string itemId,
            string state,
            int orderedUnits,
            int receivedUnits,
            long unitCostCents)
        {
            OrderId = orderId;
            ItemId = itemId;
            State = state;
            OrderedUnits = orderedUnits;
            ReceivedUnits = receivedUnits;
            UnitCostCents = unitCostCents;
        }

        public SupplierOrderSaveRecord
            ToSaveRecord()
        {
            return new SupplierOrderSaveRecord(
                OrderId,
                "s16-phase1-supplier",
                ItemId,
                State,
                OrderedUnits,
                ReceivedUnits,
                UnitCostCents);
        }
    }
}
