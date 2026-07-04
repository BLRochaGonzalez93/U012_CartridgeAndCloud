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
            SaveSlotId slotId)
        {
            string primary = PrimaryPath(slotId);
            string backup = BackupPath(slotId);

            if (TryLoad(
                    primary,
                    slotId,
                    out StoreOperationsState state))
            {
                return state;
            }

            if (TryLoad(
                    backup,
                    slotId,
                    out state))
            {
                AtomicJsonFile.Write(
                    primary,
                    File.ReadAllText(backup));
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

        public bool Delete(
            SaveSlotId slotId)
        {
            bool deleted = false;

            foreach (string path in new[]
                     {
                         PrimaryPath(slotId),
                         BackupPath(slotId),
                         PrimaryPath(slotId) + ".tmp",
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
                    dto.schemaVersion != 1 ||
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
                    schemaVersion = 1,
                    slotValue = state.SlotId.Value,
                    sessionId = state.SessionId,
                    generation = state.Generation,
                    nextOrderSequence =
                        state.NextOrderSequence,
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
                    orders = new OrderDto[
                        state.Orders.Count],
                    furnitureWarehouse =
                        new StockDto[
                            state.FurnitureWarehouse
                                .Count],
                    productWarehouse =
                        new StockDto[
                            state.ProductWarehouse
                                .Count],
                    fixtures = new FixtureDto[
                        state.Fixtures.Count]
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
                            item.UnitCostCents
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

            return dto;
        }

        private static StoreOperationsState FromDto(
            StateDto dto)
        {
            List<StoreOrderRecord> orders =
                new List<StoreOrderRecord>();
            List<StoreStockRecord> furniture =
                new List<StoreStockRecord>();
            List<StoreStockRecord> products =
                new List<StoreStockRecord>();
            List<PlacedStoreFixtureRecord> fixtures =
                new List<
                    PlacedStoreFixtureRecord>();

            foreach (OrderDto item in
                     dto.orders ??
                     new OrderDto[0])
            {
                orders.Add(
                    new StoreOrderRecord(
                        item.orderId,
                        item.itemId,
                        item.isFurniture,
                        (StoreOrderStatus)item.state,
                        item.orderedUnits,
                        item.receivedUnits,
                        item.unitCostCents));
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

            return new StoreOperationsState(
                new SaveSlotId(dto.slotValue),
                dto.sessionId,
                dto.generation,
                dto.nextOrderSequence,
                dto.nextFixtureSequence,
                dto.nextCustomerSequence,
                dto.completedSales,
                dto.lifetimeRevenueCents,
                dto.lifetimeExpenseCents,
                orders,
                furniture,
                products,
                fixtures);
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

        [Serializable]
        private sealed class StateDto
        {
            public int schemaVersion;
            public int slotValue;
            public string sessionId;
            public int generation;
            public int nextOrderSequence;
            public int nextFixtureSequence;
            public int nextCustomerSequence;
            public int completedSales;
            public long lifetimeRevenueCents;
            public long lifetimeExpenseCents;
            public OrderDto[] orders;
            public StockDto[] furnitureWarehouse;
            public StockDto[] productWarehouse;
            public FixtureDto[] fixtures;
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
    }
}
