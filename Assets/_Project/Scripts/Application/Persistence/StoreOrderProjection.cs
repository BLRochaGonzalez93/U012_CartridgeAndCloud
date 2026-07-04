using System;

using VRMGames.CartridgeAndCloud.Domain.Persistence;
namespace VRMGames.CartridgeAndCloud.Application.Persistence
{
    internal sealed class StoreOrderProjection
    {
        public string OrderId { get; }
        public string ItemId { get; }
        public string State { get; }
        public int OrderedUnits { get; }
        public int ReceivedUnits { get; }
        public long UnitCostCents { get; }

        public StoreOrderProjection(
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
