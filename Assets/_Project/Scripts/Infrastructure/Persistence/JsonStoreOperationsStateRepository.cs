using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Application.Store;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;
using VRMGames.CartridgeAndCloud.Domain.Store;
using VRMGames.CartridgeAndCloud.Infrastructure.UIUX;

using VRMGames.CartridgeAndCloud.Domain.Inventory;
namespace VRMGames.CartridgeAndCloud.Infrastructure.Persistence
{
    public sealed class JsonStoreOperationsStateRepository :
        IStoreOperationsStateRepository
    {
        private readonly string _directory;

        public JsonStoreOperationsStateRepository(
            string directory)
        {
            if (string.IsNullOrWhiteSpace(
                    directory))
            {
                throw new ArgumentException(
                    "A state directory is required.",
                    nameof(directory));
            }

            _directory = directory;
        }

        public StoreOperationsState Load(
            SaveSlotId slotId,
            bool preferBackup = false)
        {
            string primary = PrimaryPath(slotId);
            string backup = BackupPath(slotId);
            StoreOperationsState state;

            if (preferBackup &&
                TryLoad(
                    backup,
                    slotId,
                    out state))
            {
                RecoverPrimaryFromBackup(
                    backup,
                    primary);
                return state;
            }

            if (TryLoad(
                    primary,
                    slotId,
                    out state))
            {
                return state;
            }

            if (TryLoad(
                    backup,
                    slotId,
                    out state))
            {
                RecoverPrimaryFromBackup(
                    backup,
                    primary);
                return state;
            }

            string legacy = LegacyPath(slotId);

            if (TryLoad(
                    legacy,
                    slotId,
                    out state))
            {
                Save(state);
                return state;
            }

            return null;
        }

        public void Save(
            StoreOperationsState state)
        {
            if (state == null)
            {
                throw new ArgumentNullException(
                    nameof(state));
            }

            string path =
                PrimaryPath(state.SlotId);

            AtomicJsonFile.Write(
                path,
                JsonUtility.ToJson(
                    ToDto(state),
                    true));
        }

        public IStoreOperationsCheckpoint BeginCheckpoint(
            StoreOperationsState state)
        {
            if (state == null)
            {
                throw new ArgumentNullException(
                    nameof(state));
            }

            string primary = PrimaryPath(state.SlotId);
            string backup = BackupPath(state.SlotId);
            string temporary = primary + ".tmp";

            FileImage previousPrimary =
                FileImage.Capture(primary);
            FileImage previousBackup =
                FileImage.Capture(backup);

            try
            {
                Save(state);

                if (!TryLoad(
                        primary,
                        state.SlotId,
                        out StoreOperationsState validation) ||
                    validation == null ||
                    !string.Equals(
                        validation.SessionId,
                        state.SessionId,
                        StringComparison.Ordinal) ||
                    validation.Generation != state.Generation)
                {
                    throw new InvalidDataException(
                        "Store operations checkpoint validation failed.");
                }

                return new FileCheckpoint(
                    previousPrimary,
                    previousBackup,
                    temporary);
            }
            catch
            {
                previousPrimary.Restore();
                previousBackup.Restore();
                DeleteIfExists(temporary);
                throw;
            }
        }

        public bool Delete(
            SaveSlotId slotId)
        {
            bool deleted = false;

            foreach (string path in new[]
                     {
                         PrimaryPath(slotId),
                         BackupPath(slotId),
                         PrimaryPath(slotId) + ".tmp",
                         PrimaryPath(slotId) + ".recovery",
                         LegacyPath(slotId),
                         LegacyPath(slotId) + ".bak",
                         LegacyPath(slotId) + ".tmp"
                     })
            {
                if (!File.Exists(path))
                {
                    continue;
                }

                File.Delete(path);
                deleted = true;
            }

            return deleted;
        }

        public string PrimaryPath(
            SaveSlotId slotId)
        {
            return Path.Combine(
                _directory,
                $"slot_{slotId.Value}.store-operations.json");
        }

        public string BackupPath(
            SaveSlotId slotId)
        {
            return PrimaryPath(slotId) + ".bak";
        }


        public string LegacyPath(
            SaveSlotId slotId)
        {
            return Path.Combine(
                _directory,
                $"slot_{slotId.Value}.phase1.json");
        }

        private static bool TryLoad(
            string path,
            SaveSlotId expectedSlot,
            out StoreOperationsState state)
        {
            state = null;

            if (!File.Exists(path))
            {
                return false;
            }

            try
            {
                StateDto dto =
                    JsonUtility.FromJson<StateDto>(
                        File.ReadAllText(path));

                if (dto == null ||
                    (dto.schemaVersion != 1 && dto.schemaVersion != 2 && dto.schemaVersion != 3) ||
                    dto.slotValue !=
                        expectedSlot.Value ||
                    string.IsNullOrWhiteSpace(
                        dto.sessionId))
                {
                    return false;
                }

                state = FromDto(dto);
                return true;
            }
            catch
            {
                state = null;
                return false;
            }
        }

        private static StateDto ToDto(
            StoreOperationsState state)
        {
            StateDto dto =
                new StateDto
                {
                    schemaVersion = 3,
                    slotValue = state.SlotId.Value,
                    sessionId = state.SessionId,
                    generation = state.Generation,
                    nextOrderSequence =
                        state.NextOrderSequence,
                    nextDeliveryRunSequence =
                        state.NextDeliveryRunSequence,
                    nextFixtureSequence =
                        state.NextFixtureSequence,
                    nextCustomerSequence =
                        state.NextCustomerSequence,
                    completedSales =
                        state.CompletedSales,
                    lifetimeRevenueCents =
                        state.LifetimeRevenueCents,
                    lifetimeExpenseCents =
                        state.LifetimeExpenseCents,
                    lifetimeTaxCents =
                        state.LifetimeTaxCents,
                    weeklyRevenueCents =
                        state.WeeklyRevenueCents,
                    weeklySupplierCostCents =
                        state.WeeklySupplierCostCents,
                    lastSettledWeek =
                        state.LastSettledWeek,
                    lastWeeklyGrossResultCents =
                        state.LastWeeklyGrossResultCents,
                    lastWeeklyTaxCents =
                        state.LastWeeklyTaxCents,
                    orders = new OrderDto[
                        state.Orders.Count],
                    deliveryRuns = new DeliveryRunDto[
                        state.DeliveryRuns.Count],
                    furnitureWarehouse =
                        new StockDto[
                            state.FurnitureWarehouse
                                .Count],
                    productWarehouse =
                        new StockDto[
                            state.ProductWarehouse
                                .Count],
                    fixtures = new FixtureDto[
                        state.Fixtures.Count],
                    recentDayDetails = new DayDetailDto[
                        state.ManagementHistory.RecentDayDetails.Count],
                    dailySummaries = new DailySummaryDto[
                        state.ManagementHistory.DailySummaries.Count]
                };

            for (int index = 0;
                 index < state.Orders.Count;
                 index++)
            {
                StoreOrderRecord item =
                    state.Orders[index];

                dto.orders[index] =
                    new OrderDto
                    {
                        orderId = item.OrderId,
                        itemId = item.ItemId,
                        isFurniture =
                            item.IsFurniture,
                        state = (int)item.State,
                        orderedUnits =
                            item.OrderedUnits,
                        receivedUnits =
                            item.ReceivedUnits,
                        unitCostCents =
                            item.UnitCostCents,
                        reservedCostCents =
                            item.ReservedCostCents,
                        deliveryRunId =
                            item.DeliveryRunId
                    };
            }

            for (int index = 0;
                 index < state.DeliveryRuns.Count;
                 index++)
            {
                StoreDeliveryRunRecord run =
                    state.DeliveryRuns[index];

                dto.deliveryRuns[index] =
                    new DeliveryRunDto
                    {
                        deliveryRunId =
                            run.DeliveryRunId,
                        orderIds =
                            new List<string>(
                                run.OrderIds).ToArray(),
                        totalCostCents =
                            run.TotalCostCents,
                        status = (int)run.Status
                    };
            }

            CopyStock(
                state.FurnitureWarehouse,
                dto.furnitureWarehouse);
            CopyStock(
                state.ProductWarehouse,
                dto.productWarehouse);

            for (int index = 0;
                 index < state.Fixtures.Count;
                 index++)
            {
                PlacedStoreFixtureRecord item =
                    state.Fixtures[index];

                dto.fixtures[index] =
                    new FixtureDto
                    {
                        instanceId =
                            item.InstanceId,
                        definitionId =
                            item.DefinitionId,
                        anchorX = item.AnchorX,
                        anchorZ = item.AnchorZ,
                        rotationQuarterTurns =
                            item.RotationQuarterTurns,
                        assignedProductId =
                            item.AssignedProductId,
                        productQuantity =
                            item.ProductQuantity
                    };
            }

            for (int index = 0;
                 index < state.ManagementHistory.RecentDayDetails.Count;
                 index++)
            {
                dto.recentDayDetails[index] =
                    ToDto(state.ManagementHistory.RecentDayDetails[index]);
            }

            for (int index = 0;
                 index < state.ManagementHistory.DailySummaries.Count;
                 index++)
            {
                dto.dailySummaries[index] =
                    ToDto(state.ManagementHistory.DailySummaries[index]);
            }

            return dto;
        }

        private static StoreOperationsState FromDto(
            StateDto dto)
        {
            List<StoreOrderRecord> orders =
                new List<StoreOrderRecord>();
            List<StoreDeliveryRunRecord> deliveryRuns =
                new List<StoreDeliveryRunRecord>();
            List<StoreStockRecord> furniture =
                new List<StoreStockRecord>();
            List<StoreStockRecord> products =
                new List<StoreStockRecord>();
            List<PlacedStoreFixtureRecord> fixtures =
                new List<
                    PlacedStoreFixtureRecord>();
            List<StoreManagementDayDetailRecord> details =
                new List<StoreManagementDayDetailRecord>();
            List<StoreDailySummaryRecord> summaries =
                new List<StoreDailySummaryRecord>();

            foreach (OrderDto item in
                     dto.orders ??
                     new OrderDto[0])
            {
                orders.Add(
                    dto.schemaVersion >= 2
                        ? new StoreOrderRecord(
                            item.orderId,
                            item.itemId,
                            item.isFurniture,
                            (StoreOrderStatus)item.state,
                            item.orderedUnits,
                            item.receivedUnits,
                            item.unitCostCents,
                            item.reservedCostCents,
                            item.deliveryRunId)
                        : new StoreOrderRecord(
                            item.orderId,
                            item.itemId,
                            item.isFurniture,
                            (StoreOrderStatus)item.state,
                            item.orderedUnits,
                            item.receivedUnits,
                            item.unitCostCents));
            }

            foreach (DeliveryRunDto item in
                     dto.deliveryRuns ??
                     new DeliveryRunDto[0])
            {
                deliveryRuns.Add(
                    new StoreDeliveryRunRecord(
                        item.deliveryRunId,
                        item.orderIds ??
                            new string[0],
                        item.totalCostCents,
                        (StoreDeliveryRunStatus)
                            item.status));
            }

            ReadStock(
                dto.furnitureWarehouse,
                furniture);
            ReadStock(
                dto.productWarehouse,
                products);

            foreach (FixtureDto item in
                     dto.fixtures ??
                     new FixtureDto[0])
            {
                fixtures.Add(
                    new PlacedStoreFixtureRecord(
                        item.instanceId,
                        item.definitionId,
                        item.anchorX,
                        item.anchorZ,
                        item.rotationQuarterTurns,
                        item.assignedProductId,
                        item.productQuantity));
            }

            if (dto.schemaVersion >= 3)
            {
                foreach (DayDetailDto item in
                         dto.recentDayDetails ??
                         new DayDetailDto[0])
                {
                    details.Add(FromDto(item));
                }

                foreach (DailySummaryDto item in
                         dto.dailySummaries ??
                         new DailySummaryDto[0])
                {
                    summaries.Add(FromDto(item));
                }
            }

            return new StoreOperationsState(
                new SaveSlotId(dto.slotValue),
                dto.sessionId,
                dto.generation,
                dto.nextOrderSequence,
                Math.Max(1,
                    dto.nextDeliveryRunSequence),
                dto.nextFixtureSequence,
                dto.nextCustomerSequence,
                dto.completedSales,
                dto.lifetimeRevenueCents,
                dto.lifetimeExpenseCents,
                dto.lifetimeTaxCents,
                dto.weeklyRevenueCents,
                dto.weeklySupplierCostCents,
                dto.lastSettledWeek,
                dto.lastWeeklyGrossResultCents,
                dto.lastWeeklyTaxCents,
                orders,
                deliveryRuns,
                furniture,
                products,
                fixtures,
                new StoreManagementHistory(
                    details,
                    summaries));
        }

        private static DayDetailDto ToDto(
            StoreManagementDayDetailRecord detail)
        {
            DayDetailDto dto = new DayDetailDto
            {
                dayNumber = detail.DayNumber,
                dayId = detail.DayId,
                isClosed = detail.IsClosed,
                startingCashCents = detail.StartingCashCents,
                endingCashCents = detail.EndingCashCents,
                taxCents = detail.TaxCents,
                visits = new VisitDto[detail.CustomerVisits.Count],
                sales = new SaleDto[detail.Sales.Count],
                receipts = new ReceiptDto[detail.Receipts.Count]
            };

            for (int index = 0; index < detail.CustomerVisits.Count; index++)
            {
                StoreCustomerVisitDetailRecord visit = detail.CustomerVisits[index];
                dto.visits[index] = new VisitDto
                {
                    customerId = visit.CustomerId,
                    outcome = (int)visit.Outcome
                };
            }

            for (int index = 0; index < detail.Sales.Count; index++)
            {
                StoreSaleDetailRecord sale = detail.Sales[index];
                dto.sales[index] = new SaleDto
                {
                    transactionId = sale.TransactionId,
                    customerId = sale.CustomerId,
                    productId = sale.ProductId,
                    units = sale.Units,
                    revenueCents = sale.RevenueCents
                };
            }

            for (int index = 0; index < detail.Receipts.Count; index++)
            {
                StoreReceiptDetailRecord receipt = detail.Receipts[index];
                dto.receipts[index] = new ReceiptDto
                {
                    receiptId = receipt.ReceiptId,
                    deliveryRunId = receipt.DeliveryRunId,
                    orderId = receipt.OrderId,
                    itemId = receipt.ItemId,
                    isFurniture = receipt.IsFurniture,
                    units = receipt.Units,
                    supplierCostCents = receipt.SupplierCostCents
                };
            }

            return dto;
        }

        private static StoreManagementDayDetailRecord FromDto(
            DayDetailDto dto)
        {
            List<StoreCustomerVisitDetailRecord> visits =
                new List<StoreCustomerVisitDetailRecord>();
            List<StoreSaleDetailRecord> sales =
                new List<StoreSaleDetailRecord>();
            List<StoreReceiptDetailRecord> receipts =
                new List<StoreReceiptDetailRecord>();

            foreach (VisitDto visit in dto.visits ?? new VisitDto[0])
            {
                visits.Add(new StoreCustomerVisitDetailRecord(
                    visit.customerId,
                    (StoreCustomerVisitOutcome)visit.outcome));
            }

            foreach (SaleDto sale in dto.sales ?? new SaleDto[0])
            {
                sales.Add(new StoreSaleDetailRecord(
                    sale.transactionId,
                    sale.customerId,
                    sale.productId,
                    sale.units,
                    sale.revenueCents));
            }

            foreach (ReceiptDto receipt in dto.receipts ?? new ReceiptDto[0])
            {
                receipts.Add(new StoreReceiptDetailRecord(
                    receipt.receiptId,
                    receipt.deliveryRunId,
                    receipt.orderId,
                    receipt.itemId,
                    receipt.isFurniture,
                    receipt.units,
                    receipt.supplierCostCents));
            }

            return new StoreManagementDayDetailRecord(
                dto.dayNumber,
                dto.dayId,
                dto.isClosed,
                dto.startingCashCents,
                dto.endingCashCents,
                dto.taxCents,
                visits,
                sales,
                receipts);
        }

        private static DailySummaryDto ToDto(
            StoreDailySummaryRecord summary)
        {
            return new DailySummaryDto
            {
                dayNumber = summary.DayNumber,
                dayId = summary.DayId,
                startingCashCents = summary.StartingCashCents,
                endingCashCents = summary.EndingCashCents,
                visitors = summary.Visitors,
                buyers = summary.Buyers,
                abandonedCustomers = summary.AbandonedCustomers,
                completedSales = summary.CompletedSales,
                unitsSold = summary.UnitsSold,
                ordersReceived = summary.OrdersReceived,
                revenueCents = summary.RevenueCents,
                supplierCostCents = summary.SupplierCostCents,
                taxCents = summary.TaxCents
            };
        }

        private static StoreDailySummaryRecord FromDto(
            DailySummaryDto dto)
        {
            return new StoreDailySummaryRecord(
                dto.dayNumber,
                dto.dayId,
                dto.startingCashCents,
                dto.endingCashCents,
                dto.visitors,
                dto.buyers,
                dto.abandonedCustomers,
                dto.completedSales,
                dto.unitsSold,
                dto.ordersReceived,
                dto.revenueCents,
                dto.supplierCostCents,
                dto.taxCents);
        }

        private static void CopyStock(
            IReadOnlyList<StoreStockRecord> source,
            StockDto[] destination)
        {
            for (int index = 0;
                 index < source.Count;
                 index++)
            {
                destination[index] =
                    new StockDto
                    {
                        itemId =
                            source[index].ItemId,
                        quantity =
                            source[index].Quantity
                    };
            }
        }

        private static void ReadStock(
            StockDto[] source,
            List<StoreStockRecord> destination)
        {
            foreach (StockDto item in
                     source ??
                     new StockDto[0])
            {
                destination.Add(
                    new StoreStockRecord(
                        item.itemId,
                        item.quantity));
            }
        }

        private static void RecoverPrimaryFromBackup(
            string backup,
            string primary)
        {
            string recovery = primary + ".recovery";
            DeleteIfExists(recovery);
            File.Copy(backup, recovery, true);

            using (FileStream stream =
                   new FileStream(
                       recovery,
                       FileMode.Open,
                       FileAccess.ReadWrite,
                       FileShare.None))
            {
                stream.Flush(true);
            }

            if (File.Exists(primary))
            {
                try
                {
                    File.Replace(
                        recovery,
                        primary,
                        null);
                    return;
                }
                catch (PlatformNotSupportedException)
                {
                    File.Delete(primary);
                }
            }

            File.Move(recovery, primary);
        }

        private static void DeleteIfExists(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        private sealed class FileCheckpoint :
            IStoreOperationsCheckpoint
        {
            private readonly FileImage _primary;
            private readonly FileImage _backup;
            private readonly string _temporary;
            private bool _committed;
            private bool _disposed;

            public FileCheckpoint(
                FileImage primary,
                FileImage backup,
                string temporary)
            {
                _primary = primary;
                _backup = backup;
                _temporary = temporary;
            }

            public void Commit()
            {
                if (_disposed)
                {
                    throw new ObjectDisposedException(
                        nameof(FileCheckpoint));
                }

                _committed = true;
            }

            public void Dispose()
            {
                if (_disposed)
                {
                    return;
                }

                _disposed = true;

                if (!_committed)
                {
                    _primary.Restore();
                    _backup.Restore();
                }

                DeleteIfExists(_temporary);
            }
        }

        private sealed class FileImage
        {
            private readonly string _path;
            private readonly bool _existed;
            private readonly byte[] _content;

            private FileImage(
                string path,
                bool existed,
                byte[] content)
            {
                _path = path;
                _existed = existed;
                _content = content;
            }

            public static FileImage Capture(string path)
            {
                bool existed = File.Exists(path);
                return new FileImage(
                    path,
                    existed,
                    existed
                        ? File.ReadAllBytes(path)
                        : null);
            }

            public void Restore()
            {
                if (!_existed)
                {
                    DeleteIfExists(_path);
                    return;
                }

                string directory =
                    Path.GetDirectoryName(_path);
                if (!string.IsNullOrWhiteSpace(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (FileStream stream =
                       new FileStream(
                           _path,
                           FileMode.Create,
                           FileAccess.Write,
                           FileShare.None))
                {
                    stream.Write(
                        _content,
                        0,
                        _content.Length);
                    stream.Flush(true);
                }
            }
        }

        [Serializable]
        private sealed class StateDto
        {
            public int schemaVersion;
            public int slotValue;
            public string sessionId;
            public int generation;
            public int nextOrderSequence;
            public int nextDeliveryRunSequence;
            public int nextFixtureSequence;
            public int nextCustomerSequence;
            public int completedSales;
            public long lifetimeRevenueCents;
            public long lifetimeExpenseCents;
            public long lifetimeTaxCents;
            public long weeklyRevenueCents;
            public long weeklySupplierCostCents;
            public int lastSettledWeek;
            public long lastWeeklyGrossResultCents;
            public long lastWeeklyTaxCents;
            public OrderDto[] orders;
            public DeliveryRunDto[] deliveryRuns;
            public StockDto[] furnitureWarehouse;
            public StockDto[] productWarehouse;
            public FixtureDto[] fixtures;
            public DayDetailDto[] recentDayDetails;
            public DailySummaryDto[] dailySummaries;
        }

        [Serializable]
        private sealed class OrderDto
        {
            public string orderId;
            public string itemId;
            public bool isFurniture;
            public int state;
            public int orderedUnits;
            public int receivedUnits;
            public long unitCostCents;
            public long reservedCostCents;
            public string deliveryRunId;
        }

        [Serializable]
        private sealed class DeliveryRunDto
        {
            public string deliveryRunId;
            public string[] orderIds;
            public long totalCostCents;
            public int status;
        }

        [Serializable]
        private sealed class StockDto
        {
            public string itemId;
            public int quantity;
        }

        [Serializable]
        private sealed class FixtureDto
        {
            public string instanceId;
            public string definitionId;
            public int anchorX;
            public int anchorZ;
            public int rotationQuarterTurns;
            public string assignedProductId;
            public int productQuantity;
        }

        [Serializable]
        private sealed class DayDetailDto
        {
            public int dayNumber;
            public string dayId;
            public bool isClosed;
            public long startingCashCents;
            public long endingCashCents;
            public long taxCents;
            public VisitDto[] visits;
            public SaleDto[] sales;
            public ReceiptDto[] receipts;
        }

        [Serializable]
        private sealed class VisitDto
        {
            public string customerId;
            public int outcome;
        }

        [Serializable]
        private sealed class SaleDto
        {
            public string transactionId;
            public string customerId;
            public string productId;
            public int units;
            public long revenueCents;
        }

        [Serializable]
        private sealed class ReceiptDto
        {
            public string receiptId;
            public string deliveryRunId;
            public string orderId;
            public string itemId;
            public bool isFurniture;
            public int units;
            public long supplierCostCents;
        }

        [Serializable]
        private sealed class DailySummaryDto
        {
            public int dayNumber;
            public string dayId;
            public long startingCashCents;
            public long endingCashCents;
            public int visitors;
            public int buyers;
            public int abandonedCustomers;
            public int completedSales;
            public int unitsSold;
            public int ordersReceived;
            public long revenueCents;
            public long supplierCostCents;
            public long taxCents;
        }
    }
}
