using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VRMGames.CartridgeAndCloud.Domain.Orders;
using VRMGames.CartridgeAndCloud.Domain.Suppliers;

namespace VRMGames.CartridgeAndCloud.Domain.Receiving
{
    public sealed class Delivery
    {
        private readonly Dictionary<ShipmentBoxId, ShipmentBox> _boxesById;

        private readonly ReadOnlyCollection<ShipmentBox> _boxes;

        public DeliveryId Id { get; }

        public PurchaseOrderId OrderId { get; }

        public SupplierId SupplierId { get; }

        public IReadOnlyList<ShipmentBox> Boxes => _boxes;

        public int BoxCount => _boxes.Count;

        public int ReceivedBoxCount { get; private set; }

        public DeliveryStatus Status { get; private set; }

        public Delivery(
            DeliveryId id,
            PurchaseOrderId orderId,
            SupplierId supplierId,
            IEnumerable<ShipmentBox> boxes)
        {
            if (string.IsNullOrWhiteSpace(id.Value))
            {
                throw new ArgumentException(
                    "Delivery ID must be initialized.",
                    nameof(id));
            }

            if (string.IsNullOrWhiteSpace(orderId.Value))
            {
                throw new ArgumentException(
                    "Purchase order ID must be initialized.",
                    nameof(orderId));
            }

            if (string.IsNullOrWhiteSpace(supplierId.Value))
            {
                throw new ArgumentException(
                    "Supplier ID must be initialized.",
                    nameof(supplierId));
            }

            if (boxes == null)
            {
                throw new ArgumentNullException(nameof(boxes));
            }

            List<ShipmentBox> boxList =
                new List<ShipmentBox>();

            _boxesById =
                new Dictionary<ShipmentBoxId, ShipmentBox>();

            foreach (ShipmentBox box in boxes)
            {
                if (box == null)
                {
                    throw new ArgumentException(
                        "Delivery boxes cannot contain null.",
                        nameof(boxes));
                }

                if (box.OrderId != orderId)
                {
                    throw new ArgumentException(
                        "Shipment box belongs to another order.",
                        nameof(boxes));
                }

                if (_boxesById.ContainsKey(box.Id))
                {
                    throw new ArgumentException(
                        $"Shipment box ID {box.Id} is duplicated.",
                        nameof(boxes));
                }

                _boxesById.Add(box.Id, box);
                boxList.Add(box);
            }

            if (boxList.Count == 0)
            {
                throw new ArgumentException(
                    "Delivery must contain at least one box.",
                    nameof(boxes));
            }

            boxList.Sort(
                (left, right) =>
                    StringComparer.Ordinal.Compare(
                        left.Id.Value,
                        right.Id.Value));

            Id = id;
            OrderId = orderId;
            SupplierId = supplierId;
            _boxes = new ReadOnlyCollection<ShipmentBox>(boxList);
            Status = DeliveryStatus.AwaitingReceipt;
        }

        public bool TryGetBox(
            ShipmentBoxId boxId,
            out ShipmentBox box)
        {
            return _boxesById.TryGetValue(boxId, out box);
        }

        public ShipmentBox GetBox(ShipmentBoxId boxId)
        {
            if (!_boxesById.TryGetValue(
                    boxId,
                    out ShipmentBox box))
            {
                throw new KeyNotFoundException(
                    $"Shipment box {boxId} was not found.");
            }

            return box;
        }

        public DeliveryMutationResult TryMarkBoxReceived(
            ShipmentBoxId boxId)
        {
            if (!_boxesById.TryGetValue(
                    boxId,
                    out ShipmentBox box))
            {
                return DeliveryMutationResult.Failure(
                    DeliveryMutationFailureReason.BoxNotFound,
                    Status);
            }

            if (box.IsReceived)
            {
                return DeliveryMutationResult.Failure(
                    DeliveryMutationFailureReason.BoxAlreadyReceived,
                    Status);
            }

            DeliveryStatus previous = Status;
            box.MarkReceived();
            ReceivedBoxCount++;

            Status = ReceivedBoxCount == BoxCount
                ? DeliveryStatus.Received
                : DeliveryStatus.PartiallyReceived;

            return DeliveryMutationResult.Success(
                previous,
                Status);
        }
    }
}
