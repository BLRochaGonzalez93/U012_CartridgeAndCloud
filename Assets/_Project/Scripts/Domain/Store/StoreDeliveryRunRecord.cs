using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VRMGames.CartridgeAndCloud.Domain.Store
{
    public sealed class StoreDeliveryRunRecord
    {
        private readonly ReadOnlyCollection<string> _orderIds;

        public string DeliveryRunId { get; }
        public IReadOnlyList<string> OrderIds => _orderIds;
        public long TotalCostCents { get; }
        public StoreDeliveryRunStatus Status { get; }

        public StoreDeliveryRunRecord(
            string deliveryRunId,
            IEnumerable<string> orderIds,
            long totalCostCents,
            StoreDeliveryRunStatus status)
        {
            if (string.IsNullOrWhiteSpace(deliveryRunId))
            {
                throw new ArgumentException(
                    "Delivery run ID cannot be empty.",
                    nameof(deliveryRunId));
            }

            if (orderIds == null)
            {
                throw new ArgumentNullException(nameof(orderIds));
            }

            if (totalCostCents <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(totalCostCents));
            }

            List<string> copy = new List<string>();
            HashSet<string> unique = new HashSet<string>(
                StringComparer.Ordinal);

            foreach (string orderId in orderIds)
            {
                if (string.IsNullOrWhiteSpace(orderId))
                {
                    throw new ArgumentException(
                        "Delivery run order IDs cannot be empty.",
                        nameof(orderIds));
                }

                if (!unique.Add(orderId))
                {
                    throw new ArgumentException(
                        "Delivery run order IDs must be unique.",
                        nameof(orderIds));
                }

                copy.Add(orderId);
            }

            if (copy.Count == 0)
            {
                throw new ArgumentException(
                    "A delivery run must contain at least one order.",
                    nameof(orderIds));
            }

            DeliveryRunId = deliveryRunId;
            _orderIds = new ReadOnlyCollection<string>(copy);
            TotalCostCents = totalCostCents;
            Status = status;
        }

        public StoreDeliveryRunRecord MarkReceived()
        {
            if (Status == StoreDeliveryRunStatus.Received)
            {
                return this;
            }

            return new StoreDeliveryRunRecord(
                DeliveryRunId,
                OrderIds,
                TotalCostCents,
                StoreDeliveryRunStatus.Received);
        }
    }

    public enum StoreDeliveryRunStatus
    {
        Received = 0,
        InTransit = 1
    }
}
