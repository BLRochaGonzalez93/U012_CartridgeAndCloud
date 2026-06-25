using System;
using System.Collections.Generic;
using VRMGames.CartridgeAndCloud.Domain.Orders;
using VRMGames.CartridgeAndCloud.Domain.Receiving;

namespace VRMGames.CartridgeAndCloud.Application.Receiving
{
    public sealed class SupplierDeliveryService
    {
        public DeliveryCreationResult CreateDelivery(
            DeliveryId deliveryId,
            PurchaseOrder order)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }

            if (order.Status != PurchaseOrderStatus.Submitted)
            {
                return DeliveryCreationResult.Failure(
                    DeliveryCreationFailureReason.OrderNotSubmitted);
            }

            List<ShipmentBox> boxes =
                new List<ShipmentBox>(order.TotalBoxes);

            int sequence = 1;

            foreach (PurchaseOrderLine line in order.Lines)
            {
                for (int boxIndex = 0;
                     boxIndex < line.BoxCount;
                     boxIndex++)
                {
                    ShipmentBoxId boxId =
                        new ShipmentBoxId(
                            $"{deliveryId.Value}-box-{sequence:000}");

                    boxes.Add(
                        new ShipmentBox(
                            boxId,
                            order.Id,
                            line.ProductId,
                            line.UnitsPerBox));

                    sequence++;
                }
            }

            Delivery delivery =
                new Delivery(
                    deliveryId,
                    order.Id,
                    order.SupplierId,
                    boxes);

            PurchaseOrderTransitionResult transition =
                order.MarkDelivered();

            if (!transition.Succeeded)
            {
                return DeliveryCreationResult.Failure(
                    DeliveryCreationFailureReason
                        .OrderTransitionRejected);
            }

            return DeliveryCreationResult.Success(delivery);
        }
    }
}
