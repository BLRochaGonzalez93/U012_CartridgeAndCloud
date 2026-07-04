using System;

namespace VRMGames.CartridgeAndCloud.Domain.Store
{
    public sealed class StoreOrderRecord
    {
        public string OrderId { get; }
        public string ItemId { get; }
        public bool IsFurniture { get; }
        public StoreOrderStatus State { get; }
        public int OrderedUnits { get; }
        public int ReceivedUnits { get; }
        public long UnitCostCents { get; }

        public StoreOrderRecord(
            string orderId,
            string itemId,
            bool isFurniture,
            StoreOrderStatus state,
            int orderedUnits,
            int receivedUnits,
            long unitCostCents)
        {
            OrderId = Required(
                orderId,
                nameof(orderId));
            ItemId = Required(
                itemId,
                nameof(itemId));

            if (orderedUnits < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(orderedUnits));
            }

            if (receivedUnits < 0 ||
                receivedUnits > orderedUnits)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(receivedUnits));
            }

            if (unitCostCents <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(unitCostCents));
            }

            IsFurniture = isFurniture;
            State = state;
            OrderedUnits = orderedUnits;
            ReceivedUnits = receivedUnits;
            UnitCostCents = unitCostCents;
        }

        public StoreOrderRecord ReceiveAll()
        {
            return new StoreOrderRecord(
                OrderId,
                ItemId,
                IsFurniture,
                StoreOrderStatus.Received,
                OrderedUnits,
                OrderedUnits,
                UnitCostCents);
        }

        private static string Required(
            string value,
            string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "Value cannot be empty.",
                    parameterName);
            }

            return value;
        }
    }

    public enum StoreOrderStatus
    {
        Ordered = 0,
        Received = 1,
        Completed = 2
    }
}
