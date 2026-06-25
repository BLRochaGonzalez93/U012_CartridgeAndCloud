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
    public sealed class ReceivingServiceTests
    {
        private static readonly ProductDefinitionId ProductA =
            new ProductDefinitionId("product-a");

        private static readonly ProductDefinitionId ProductB =
            new ProductDefinitionId("product-b");

        [Test]
        public void Constructor_NullRegistry_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => new ReceivingService(null));
        }

        [Test]
        public void ReceiveBox_OrderNotDelivered_FailsWithoutInventoryMutation()
        {
            Scenario scenario = CreateScenario();
            scenario.Order = CreateOrder();

            ReceivingResult result =
                scenario.Service.ReceiveBox(
                    scenario.Order,
                    scenario.Delivery,
                    scenario.BoxId,
                    scenario.Destination);

            AssertFailureIsAtomic(
                result,
                ReceivingFailureReason.OrderNotDelivered,
                scenario);
        }

        [Test]
        public void ReceiveBox_OrderMismatch_FailsWithoutInventoryMutation()
        {
            Scenario scenario = CreateScenario();
            scenario.Order =
                CreateDeliveredOrder("other-order");

            ReceivingResult result =
                scenario.Service.ReceiveBox(
                    scenario.Order,
                    scenario.Delivery,
                    scenario.BoxId,
                    scenario.Destination);

            AssertFailureIsAtomic(
                result,
                ReceivingFailureReason.DeliveryOrderMismatch,
                scenario);
        }

        [Test]
        public void ReceiveBox_SupplierMismatch_FailsWithoutInventoryMutation()
        {
            Scenario scenario = CreateScenario();
            scenario.Order =
                CreateDeliveredOrder(
                    "order-a",
                    "other-supplier");

            ReceivingResult result =
                scenario.Service.ReceiveBox(
                    scenario.Order,
                    scenario.Delivery,
                    scenario.BoxId,
                    scenario.Destination);

            AssertFailureIsAtomic(
                result,
                ReceivingFailureReason.DeliverySupplierMismatch,
                scenario);
        }

        [Test]
        public void ReceiveBox_MissingBox_FailsWithoutInventoryMutation()
        {
            Scenario scenario = CreateScenario();

            ReceivingResult result =
                scenario.Service.ReceiveBox(
                    scenario.Order,
                    scenario.Delivery,
                    new ShipmentBoxId("missing-box"),
                    scenario.Destination);

            AssertFailureIsAtomic(
                result,
                ReceivingFailureReason.BoxNotFound,
                scenario);
        }

        [Test]
        public void ReceiveBox_MissingProductDefinition_FailsAtomically()
        {
            Scenario scenario =
                CreateScenario(registryProduct: ProductB);

            ReceivingResult result =
                scenario.Service.ReceiveBox(
                    scenario.Order,
                    scenario.Delivery,
                    scenario.BoxId,
                    scenario.Destination);

            AssertFailureIsAtomic(
                result,
                ReceivingFailureReason.ProductDefinitionMissing,
                scenario);
        }

        [Test]
        public void ReceiveBox_InsufficientCapacity_FailsAtomically()
        {
            Scenario scenario = CreateScenario(capacity: 5);

            ReceivingResult result =
                scenario.Service.ReceiveBox(
                    scenario.Order,
                    scenario.Delivery,
                    scenario.BoxId,
                    scenario.Destination);

            AssertFailureIsAtomic(
                result,
                ReceivingFailureReason.DestinationCapacityExceeded,
                scenario);
        }

        [Test]
        public void ReceiveBox_ValidBox_AddsExactQuantityAndMarksBox()
        {
            Scenario scenario = CreateScenario();

            ReceivingResult result =
                scenario.Service.ReceiveBox(
                    scenario.Order,
                    scenario.Delivery,
                    scenario.BoxId,
                    scenario.Destination);

            Assert.That(result.Succeeded, Is.True);
            Assert.That(result.DestinationBefore.Value, Is.Zero);
            Assert.That(result.DestinationAfter.Value, Is.EqualTo(6));
            Assert.That(scenario.Destination.UsedCapacity, Is.EqualTo(6));
            Assert.That(
                scenario.Delivery.GetBox(
                    scenario.BoxId).IsReceived,
                Is.True);
        }

        [Test]
        public void ReceiveBox_DestinationAlreadyHasProduct_MergesStack()
        {
            Scenario scenario =
                CreateScenario(initialQuantity: 2);

            ReceivingResult result =
                scenario.Service.ReceiveBox(
                    scenario.Order,
                    scenario.Delivery,
                    scenario.BoxId,
                    scenario.Destination);

            Assert.That(result.Succeeded, Is.True);
            Assert.That(result.DestinationAfter.Value, Is.EqualTo(8));
            Assert.That(scenario.Destination.StackCount, Is.EqualTo(1));
        }

        [Test]
        public void ReceiveBox_SameBoxTwice_SecondReceiptFailsWithoutDuplication()
        {
            Scenario scenario = CreateScenario();

            scenario.Service.ReceiveBox(
                scenario.Order,
                scenario.Delivery,
                scenario.BoxId,
                scenario.Destination);

            ReceivingResult second =
                scenario.Service.ReceiveBox(
                    scenario.Order,
                    scenario.Delivery,
                    scenario.BoxId,
                    scenario.Destination);

            Assert.That(second.Succeeded, Is.False);
            Assert.That(
                second.FailureReason,
                Is.EqualTo(
                    ReceivingFailureReason.BoxAlreadyReceived));

            Assert.That(
                scenario.Destination
                    .GetQuantity(ProductA).Value,
                Is.EqualTo(6));
        }

        [Test]
        public void ReceiveBox_FirstOfTwo_LeavesOrderDeliveredAndDeliveryPartial()
        {
            Scenario scenario = CreateScenario(boxCount: 2);

            scenario.Service.ReceiveBox(
                scenario.Order,
                scenario.Delivery,
                scenario.BoxId,
                scenario.Destination);

            Assert.That(
                scenario.Delivery.Status,
                Is.EqualTo(DeliveryStatus.PartiallyReceived));

            Assert.That(
                scenario.Order.Status,
                Is.EqualTo(PurchaseOrderStatus.Delivered));
        }

        [Test]
        public void ReceiveBox_LastOfTwo_CompletesDeliveryAndOrder()
        {
            Scenario scenario = CreateScenario(boxCount: 2);

            scenario.Service.ReceiveBox(
                scenario.Order,
                scenario.Delivery,
                new ShipmentBoxId("delivery-a-box-001"),
                scenario.Destination);

            ReceivingResult result =
                scenario.Service.ReceiveBox(
                    scenario.Order,
                    scenario.Delivery,
                    new ShipmentBoxId("delivery-a-box-002"),
                    scenario.Destination);

            Assert.That(result.Succeeded, Is.True);
            Assert.That(
                scenario.Delivery.Status,
                Is.EqualTo(DeliveryStatus.Received));

            Assert.That(
                scenario.Order.Status,
                Is.EqualTo(PurchaseOrderStatus.Received));

            Assert.That(
                scenario.Destination
                    .GetQuantity(ProductA).Value,
                Is.EqualTo(12));
        }

        private static void AssertFailureIsAtomic(
            ReceivingResult result,
            ReceivingFailureReason expectedReason,
            Scenario scenario)
        {
            Assert.That(result.Succeeded, Is.False);
            Assert.That(
                result.FailureReason,
                Is.EqualTo(expectedReason));

            Assert.That(
                scenario.Destination.UsedCapacity,
                Is.Zero);

            Assert.That(
                scenario.Delivery.ReceivedBoxCount,
                Is.Zero);
        }

        private static Scenario CreateScenario(
            int capacity = 20,
            int initialQuantity = 0,
            int boxCount = 1,
            ProductDefinitionId? registryProduct = null)
        {
            PurchaseOrder order =
                CreateOrder(boxCount: boxCount);

            order.Submit();

            Delivery delivery =
                new SupplierDeliveryService().CreateDelivery(
                    new DeliveryId("delivery-a"),
                    order).Delivery;

            InventoryContainer destination =
                initialQuantity > 0
                    ? new InventoryContainer(
                        new InventoryContainerId("warehouse"),
                        InventoryContainerType.Storage,
                        new InventoryCapacity(capacity),
                        new[]
                        {
                            new InventoryStack(
                                ProductA,
                                new Quantity(initialQuantity))
                        })
                    : new InventoryContainer(
                        new InventoryContainerId("warehouse"),
                        InventoryContainerType.Storage,
                        new InventoryCapacity(capacity));

            ProductDefinitionId includedProduct =
                registryProduct ?? ProductA;

            ProductDefinitionRegistry registry =
                new ProductDefinitionRegistry(
                    new[]
                    {
                        new ProductDefinition(
                            includedProduct,
                            "products.test.name",
                            new ProductCategoryId("category"),
                            Array.Empty<ProductTagId>())
                    });

            return new Scenario
            {
                Order = order,
                Delivery = delivery,
                BoxId =
                    new ShipmentBoxId(
                        "delivery-a-box-001"),
                Destination = destination,
                Service = new ReceivingService(registry)
            };
        }

        private static PurchaseOrder CreateOrder(
            string orderId = "order-a",
            string supplierId = "supplier-a",
            int boxCount = 1)
        {
            return new PurchaseOrder(
                new PurchaseOrderId(orderId),
                new SupplierId(supplierId),
                new[]
                {
                    new PurchaseOrderLine(
                        ProductA,
                        boxCount,
                        new Quantity(6),
                        100)
                });
        }

        private static PurchaseOrder CreateDeliveredOrder(
            string orderId = "order-a",
            string supplierId = "supplier-a")
        {
            PurchaseOrder order =
                CreateOrder(orderId, supplierId);

            order.Submit();
            order.MarkDelivered();
            return order;
        }

        private sealed class Scenario
        {
            public PurchaseOrder Order { get; set; }

            public Delivery Delivery { get; set; }

            public ShipmentBoxId BoxId { get; set; }

            public InventoryContainer Destination { get; set; }

            public ReceivingService Service { get; set; }
        }
    }
}
