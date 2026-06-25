using System;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.Receiving;
using VRMGames.CartridgeAndCloud.Domain.Inventory;
using VRMGames.CartridgeAndCloud.Domain.Orders;
using VRMGames.CartridgeAndCloud.Domain.Products;
using VRMGames.CartridgeAndCloud.Domain.Receiving;
using VRMGames.CartridgeAndCloud.Domain.Suppliers;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Receiving
{
    public sealed class DeliveryTests
    {
        private static readonly ProductDefinitionId ProductA =
            new ProductDefinitionId("product-a");

        [Test]
        public void ShipmentBox_ZeroQuantity_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new ShipmentBox(
                    new ShipmentBoxId("box-a"),
                    new PurchaseOrderId("order-a"),
                    ProductA,
                    Quantity.Zero));
        }

        [Test]
        public void Delivery_EmptyBoxes_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(
                () => new Delivery(
                    new DeliveryId("delivery-a"),
                    new PurchaseOrderId("order-a"),
                    new SupplierId("supplier-a"),
                    Array.Empty<ShipmentBox>()));
        }

        [Test]
        public void Delivery_BoxFromAnotherOrder_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(
                () => new Delivery(
                    new DeliveryId("delivery-a"),
                    new PurchaseOrderId("order-a"),
                    new SupplierId("supplier-a"),
                    new[] { CreateBox("box-a", "other-order") }));
        }

        [Test]
        public void Delivery_DuplicateBoxId_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(
                () => new Delivery(
                    new DeliveryId("delivery-a"),
                    new PurchaseOrderId("order-a"),
                    new SupplierId("supplier-a"),
                    new[]
                    {
                        CreateBox("box-a"),
                        CreateBox("box-a")
                    }));
        }

        [Test]
        public void TryMarkBoxReceived_FirstBox_ChangesToPartial()
        {
            Delivery delivery = CreateDelivery(2);

            DeliveryMutationResult result =
                delivery.TryMarkBoxReceived(
                    new ShipmentBoxId("box-001"));

            Assert.That(result.Succeeded, Is.True);
            Assert.That(delivery.ReceivedBoxCount, Is.EqualTo(1));
            Assert.That(
                delivery.Status,
                Is.EqualTo(DeliveryStatus.PartiallyReceived));
        }

        [Test]
        public void TryMarkBoxReceived_LastBox_ChangesToReceived()
        {
            Delivery delivery = CreateDelivery(1);

            delivery.TryMarkBoxReceived(
                new ShipmentBoxId("box-001"));

            Assert.That(
                delivery.Status,
                Is.EqualTo(DeliveryStatus.Received));
        }

        [Test]
        public void TryMarkBoxReceived_Twice_FailsWithoutCountChange()
        {
            Delivery delivery = CreateDelivery(1);
            ShipmentBoxId boxId =
                new ShipmentBoxId("box-001");

            delivery.TryMarkBoxReceived(boxId);

            DeliveryMutationResult result =
                delivery.TryMarkBoxReceived(boxId);

            Assert.That(result.Succeeded, Is.False);
            Assert.That(delivery.ReceivedBoxCount, Is.EqualTo(1));
        }

        [Test]
        public void CreateDelivery_DraftOrder_ReturnsTypedFailure()
        {
            PurchaseOrder order = CreateOrder(boxCount: 1);

            DeliveryCreationResult result =
                new SupplierDeliveryService().CreateDelivery(
                    new DeliveryId("delivery-a"),
                    order);

            Assert.That(result.Succeeded, Is.False);
            Assert.That(
                order.Status,
                Is.EqualTo(PurchaseOrderStatus.Draft));
        }

        [Test]
        public void CreateDelivery_SubmittedOrder_CreatesOneBoxPerCase()
        {
            PurchaseOrder order = CreateOrder(boxCount: 3);
            order.Submit();

            DeliveryCreationResult result =
                new SupplierDeliveryService().CreateDelivery(
                    new DeliveryId("delivery-a"),
                    order);

            Assert.That(result.Succeeded, Is.True);
            Assert.That(result.Delivery.BoxCount, Is.EqualTo(3));
            Assert.That(
                order.Status,
                Is.EqualTo(PurchaseOrderStatus.Delivered));
        }

        [Test]
        public void CreateDelivery_BoxIdsAreDeterministicAndOrdered()
        {
            PurchaseOrder order = CreateOrder(boxCount: 2);
            order.Submit();

            Delivery delivery =
                new SupplierDeliveryService().CreateDelivery(
                    new DeliveryId("delivery-a"),
                    order).Delivery;

            Assert.That(
                delivery.Boxes[0].Id.Value,
                Is.EqualTo("delivery-a-box-001"));

            Assert.That(
                delivery.Boxes[1].Id.Value,
                Is.EqualTo("delivery-a-box-002"));
        }

        private static ShipmentBox CreateBox(
            string boxId,
            string orderId = "order-a")
        {
            return new ShipmentBox(
                new ShipmentBoxId(boxId),
                new PurchaseOrderId(orderId),
                ProductA,
                new Quantity(6));
        }

        private static Delivery CreateDelivery(int boxCount)
        {
            ShipmentBox[] boxes = new ShipmentBox[boxCount];

            for (int index = 0; index < boxCount; index++)
            {
                boxes[index] =
                    CreateBox($"box-{index + 1:000}");
            }

            return new Delivery(
                new DeliveryId("delivery-a"),
                new PurchaseOrderId("order-a"),
                new SupplierId("supplier-a"),
                boxes);
        }

        private static PurchaseOrder CreateOrder(int boxCount)
        {
            return new PurchaseOrder(
                new PurchaseOrderId("order-a"),
                new SupplierId("supplier-a"),
                new[]
                {
                    new PurchaseOrderLine(
                        ProductA,
                        boxCount,
                        new Quantity(6),
                        100)
                });
        }
    }
}
