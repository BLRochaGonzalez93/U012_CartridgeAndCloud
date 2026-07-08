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
        public long ReservedCostCents { get; }
        public string DeliveryRunId { get; }

        public long TotalCostCents =>
            checked(UnitCostCents * OrderedUnits);

        public bool HasFundsReservation =>
            ReservedCostCents == TotalCostCents &&
            (State == StoreOrderStatus.Reserved ||
             State == StoreOrderStatus.InTransit);

        public StoreOrderRecord(
            string orderId,
            string itemId,
            bool isFurniture,
            StoreOrderStatus state,
            int orderedUnits,
            int receivedUnits,
            long unitCostCents)
            : this(
                orderId,
                itemId,
                isFurniture,
                state,
                orderedUnits,
                receivedUnits,
                unitCostCents,
                state == StoreOrderStatus.Reserved
                    ? checked(unitCostCents * orderedUnits)
                    : 0,
                string.Empty)
        {
        }

        public StoreOrderRecord(
            string orderId,
            string itemId,
            bool isFurniture,
            StoreOrderStatus state,
            int orderedUnits,
            int receivedUnits,
            long unitCostCents,
            long reservedCostCents,
            string deliveryRunId)
        {
            OrderId = Required(orderId, nameof(orderId));
            ItemId = Required(itemId, nameof(itemId));

            if (orderedUnits < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(orderedUnits));
            }

            if (receivedUnits < 0 || receivedUnits > orderedUnits)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(receivedUnits));
            }

            if (unitCostCents <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(unitCostCents));
            }

            long totalCost = checked(unitCostCents * orderedUnits);
            if (reservedCostCents < 0 || reservedCostCents > totalCost)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(reservedCostCents));
            }

            switch (state)
            {
                case StoreOrderStatus.Reserved:
                    if (receivedUnits != 0 || reservedCostCents != totalCost)
                    {
                        throw new ArgumentException(
                            "An ordered delivery must have a full funds reservation and no received units.");
                    }
                    break;
                case StoreOrderStatus.InTransit:
                    if (receivedUnits != 0 ||
                        reservedCostCents != totalCost ||
                        string.IsNullOrWhiteSpace(deliveryRunId))
                    {
                        throw new ArgumentException(
                            "An in-transit delivery requires a run, a full reservation and no received units.");
                    }
                    break;
                case StoreOrderStatus.Received:
                case StoreOrderStatus.Completed:
                    if (receivedUnits != orderedUnits || reservedCostCents != 0)
                    {
                        throw new ArgumentException(
                            "A received delivery must contain all units and no remaining reservation.");
                    }
                    break;
                case StoreOrderStatus.Cancelled:
                    if (receivedUnits != 0 || reservedCostCents != 0)
                    {
                        throw new ArgumentException(
                            "A cancelled delivery cannot retain units or reserved funds.");
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state));
            }

            IsFurniture = isFurniture;
            State = state;
            OrderedUnits = orderedUnits;
            ReceivedUnits = receivedUnits;
            UnitCostCents = unitCostCents;
            ReservedCostCents = reservedCostCents;
            DeliveryRunId = deliveryRunId ?? string.Empty;
        }

        public StoreOrderRecord BeginTransit(string deliveryRunId)
        {
            if (State != StoreOrderStatus.Reserved)
            {
                throw new InvalidOperationException(
                    "Only reserved deliveries can begin transit.");
            }

            if (string.IsNullOrWhiteSpace(deliveryRunId))
            {
                throw new ArgumentException(
                    "A delivery run ID is required.",
                    nameof(deliveryRunId));
            }

            return new StoreOrderRecord(
                OrderId,
                ItemId,
                IsFurniture,
                StoreOrderStatus.InTransit,
                OrderedUnits,
                0,
                UnitCostCents,
                TotalCostCents,
                deliveryRunId);
        }

        public StoreOrderRecord ReceiveAll()
        {
            return ReceiveAll(DeliveryRunId);
        }

        public StoreOrderRecord ReceiveAll(string deliveryRunId)
        {
            if (State != StoreOrderStatus.InTransit)
            {
                throw new InvalidOperationException(
                    "Only in-transit deliveries can be received.");
            }

            return new StoreOrderRecord(
                OrderId,
                ItemId,
                IsFurniture,
                StoreOrderStatus.Received,
                OrderedUnits,
                OrderedUnits,
                UnitCostCents,
                0,
                string.IsNullOrWhiteSpace(deliveryRunId)
                    ? DeliveryRunId
                    : deliveryRunId);
        }

        public StoreOrderRecord Cancel()
        {
            if (State != StoreOrderStatus.Reserved)
            {
                throw new InvalidOperationException(
                    "Only ordered deliveries can be cancelled.");
            }

            return new StoreOrderRecord(
                OrderId,
                ItemId,
                IsFurniture,
                StoreOrderStatus.Cancelled,
                OrderedUnits,
                0,
                UnitCostCents,
                0,
                string.Empty);
        }

        private static string Required(string value, string parameterName)
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
        Reserved = 0,
        Ordered = Reserved,
        Received = 1,
        Completed = 2,
        Cancelled = 3,
        InTransit = 4
    }
}
