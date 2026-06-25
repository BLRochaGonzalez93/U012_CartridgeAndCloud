using System;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.Inventory;
using VRMGames.CartridgeAndCloud.Domain.Orders;
using VRMGames.CartridgeAndCloud.Domain.Products;
using VRMGames.CartridgeAndCloud.Domain.Suppliers;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Orders
{
    public sealed class PurchaseOrderTests
    {
        private static readonly ProductDefinitionId ProductA =
            new ProductDefinitionId("product-a");

        private static readonly ProductDefinitionId ProductB =
            new ProductDefinitionId("product-b");

        [Test]
        public void RequestLine_ZeroBoxes_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new PurchaseOrderRequestLine(ProductA, 0));
        }

        [Test]
        public void OrderLine_ValidData_ComputesQuantityAndCost()
        {
            PurchaseOrderLine line =
                CreateLine(ProductA, 2, 6, 125);

            Assert.That(line.OrderedQuantity.Value, Is.EqualTo(12));
            Assert.That(line.TotalCostCents, Is.EqualTo(1500));
        }

        [Test]
        public void Constructor_EmptyLines_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(
                () => new PurchaseOrder(
                    new PurchaseOrderId("order-a"),
                    new SupplierId("supplier-a"),
                    Array.Empty<PurchaseOrderLine>()));
        }

        [Test]
        public void Constructor_DuplicateProducts_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(
                () => CreateOrder(
                    CreateLine(ProductA, 1, 6, 100),
                    CreateLine(ProductA, 2, 6, 100)));
        }

        [Test]
        public void Constructor_ValidLines_ComputesTotalsAndSortsLines()
        {
            PurchaseOrder order =
                CreateOrder(
                    CreateLine(ProductB, 1, 2, 500),
                    CreateLine(ProductA, 2, 6, 100));

            Assert.That(
                order.Status,
                Is.EqualTo(PurchaseOrderStatus.Draft));

            Assert.That(order.TotalBoxes, Is.EqualTo(3));
            Assert.That(order.TotalUnits, Is.EqualTo(14));
            Assert.That(order.TotalCostCents, Is.EqualTo(2200));
            Assert.That(order.Lines[0].ProductId, Is.EqualTo(ProductA));
        }

        [Test]
        public void Submit_DraftOrder_TransitionsToSubmitted()
        {
            PurchaseOrder order =
                CreateOrder(CreateLine(ProductA, 1, 6, 100));

            PurchaseOrderTransitionResult result =
                order.Submit();

            Assert.That(result.Succeeded, Is.True);
            Assert.That(
                order.Status,
                Is.EqualTo(PurchaseOrderStatus.Submitted));
        }

        [Test]
        public void Submit_SubmittedOrder_FailsWithoutMutation()
        {
            PurchaseOrder order = CreateSubmittedOrder();

            PurchaseOrderTransitionResult result =
                order.Submit();

            Assert.That(result.Succeeded, Is.False);
            Assert.That(
                order.Status,
                Is.EqualTo(PurchaseOrderStatus.Submitted));
        }

        [Test]
        public void DeliveredThenReceived_TransitionsInOrder()
        {
            PurchaseOrder order = CreateSubmittedOrder();

            Assert.That(order.MarkDelivered().Succeeded, Is.True);
            Assert.That(order.MarkReceived().Succeeded, Is.True);
            Assert.That(
                order.Status,
                Is.EqualTo(PurchaseOrderStatus.Received));
        }

        [Test]
        public void MarkReceived_SubmittedOrder_FailsWithoutMutation()
        {
            PurchaseOrder order = CreateSubmittedOrder();

            PurchaseOrderTransitionResult result =
                order.MarkReceived();

            Assert.That(result.Succeeded, Is.False);
            Assert.That(
                order.Status,
                Is.EqualTo(PurchaseOrderStatus.Submitted));
        }

        [Test]
        public void Cancel_DraftOrSubmitted_SucceedsButDeliveredFails()
        {
            PurchaseOrder draft =
                CreateOrder(CreateLine(ProductA, 1, 6, 100));

            PurchaseOrder submitted = CreateSubmittedOrder();
            PurchaseOrder delivered = CreateSubmittedOrder();
            delivered.MarkDelivered();

            Assert.That(draft.Cancel().Succeeded, Is.True);
            Assert.That(submitted.Cancel().Succeeded, Is.True);
            Assert.That(delivered.Cancel().Succeeded, Is.False);
        }

        private static PurchaseOrderLine CreateLine(
            ProductDefinitionId productId,
            int boxes,
            int unitsPerBox,
            int unitCostCents)
        {
            return new PurchaseOrderLine(
                productId,
                boxes,
                new Quantity(unitsPerBox),
                unitCostCents);
        }

        private static PurchaseOrder CreateOrder(
            params PurchaseOrderLine[] lines)
        {
            return new PurchaseOrder(
                new PurchaseOrderId("order-a"),
                new SupplierId("supplier-a"),
                lines);
        }

        private static PurchaseOrder CreateSubmittedOrder()
        {
            PurchaseOrder order =
                CreateOrder(CreateLine(ProductA, 1, 6, 100));

            order.Submit();
            return order;
        }
    }
}
