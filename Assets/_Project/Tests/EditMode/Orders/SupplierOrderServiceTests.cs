using System;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.Orders;
using VRMGames.CartridgeAndCloud.Domain.Inventory;
using VRMGames.CartridgeAndCloud.Domain.Orders;
using VRMGames.CartridgeAndCloud.Domain.Products;
using VRMGames.CartridgeAndCloud.Domain.Suppliers;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Orders
{
    public sealed class SupplierOrderServiceTests
    {
        private static readonly ProductDefinitionId ProductA =
            new ProductDefinitionId("product-a");

        private static readonly ProductDefinitionId ProductB =
            new ProductDefinitionId("product-b");

        [Test]
        public void CreateDraft_NullCatalog_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => new SupplierOrderService().CreateDraft(
                    new PurchaseOrderId("order-a"),
                    null,
                    Array.Empty<PurchaseOrderRequestLine>()));
        }

        [Test]
        public void CreateDraft_NullRequest_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => new SupplierOrderService().CreateDraft(
                    new PurchaseOrderId("order-a"),
                    CreateCatalog(),
                    null));
        }

        [Test]
        public void CreateDraft_EmptyRequest_ReturnsTypedFailure()
        {
            OrderCreationResult result =
                CreateService().CreateDraft(
                    new PurchaseOrderId("order-a"),
                    CreateCatalog(),
                    Array.Empty<PurchaseOrderRequestLine>());

            Assert.That(result.Succeeded, Is.False);
            Assert.That(
                result.FailureReason,
                Is.EqualTo(OrderCreationFailureReason.EmptyRequest));
        }

        [Test]
        public void CreateDraft_DuplicateProduct_ReturnsTypedFailure()
        {
            OrderCreationResult result =
                CreateService().CreateDraft(
                    new PurchaseOrderId("order-a"),
                    CreateCatalog(),
                    new[]
                    {
                        new PurchaseOrderRequestLine(ProductA, 1),
                        new PurchaseOrderRequestLine(ProductA, 2)
                    });

            Assert.That(
                result.FailureReason,
                Is.EqualTo(OrderCreationFailureReason.DuplicateProduct));
        }

        [Test]
        public void CreateDraft_ProductNotOffered_ReturnsTypedFailure()
        {
            OrderCreationResult result =
                CreateService().CreateDraft(
                    new PurchaseOrderId("order-a"),
                    CreateCatalog(),
                    new[]
                    {
                        new PurchaseOrderRequestLine(ProductB, 1)
                    });

            Assert.That(
                result.FailureReason,
                Is.EqualTo(OrderCreationFailureReason.ProductNotOffered));

            Assert.That(
                result.FailedProductId,
                Is.EqualTo(ProductB));
        }

        [Test]
        public void CreateDraft_TooFewBoxes_ReturnsTypedFailure()
        {
            SupplierCatalog catalog =
                CreateCatalog(minimumBoxes: 2);

            OrderCreationResult result =
                CreateService().CreateDraft(
                    new PurchaseOrderId("order-a"),
                    catalog,
                    new[]
                    {
                        new PurchaseOrderRequestLine(ProductA, 1)
                    });

            Assert.That(
                result.FailureReason,
                Is.EqualTo(
                    OrderCreationFailureReason
                        .BoxCountOutsideSupplierLimits));
        }

        [Test]
        public void CreateDraft_TooManyBoxes_ReturnsTypedFailure()
        {
            SupplierCatalog catalog =
                CreateCatalog(maximumBoxes: 2);

            OrderCreationResult result =
                CreateService().CreateDraft(
                    new PurchaseOrderId("order-a"),
                    catalog,
                    new[]
                    {
                        new PurchaseOrderRequestLine(ProductA, 3)
                    });

            Assert.That(result.Succeeded, Is.False);
        }

        [Test]
        public void CreateDraft_ValidRequest_CreatesDraftWithSupplier()
        {
            SupplierCatalog catalog = CreateCatalog();

            OrderCreationResult result =
                CreateService().CreateDraft(
                    new PurchaseOrderId("order-a"),
                    catalog,
                    new[]
                    {
                        new PurchaseOrderRequestLine(ProductA, 2)
                    });

            Assert.That(result.Succeeded, Is.True);
            Assert.That(
                result.Order.Status,
                Is.EqualTo(PurchaseOrderStatus.Draft));

            Assert.That(
                result.Order.SupplierId,
                Is.EqualTo(catalog.Supplier.Id));
        }

        [Test]
        public void CreateDraft_ValidRequest_SnapshotsCatalogTerms()
        {
            OrderCreationResult result =
                CreateService().CreateDraft(
                    new PurchaseOrderId("order-a"),
                    CreateCatalog(),
                    new[]
                    {
                        new PurchaseOrderRequestLine(ProductA, 2)
                    });

            PurchaseOrderLine line = result.Order.Lines[0];

            Assert.That(line.BoxCount, Is.EqualTo(2));
            Assert.That(line.UnitsPerBox.Value, Is.EqualTo(6));
            Assert.That(line.UnitCostCents, Is.EqualTo(100));
            Assert.That(line.OrderedQuantity.Value, Is.EqualTo(12));
        }

        [Test]
        public void CreateDraft_ValidMultipleLines_AggregatesTotals()
        {
            SupplierCatalog catalog =
                CreateCatalogWithTwoProducts();

            OrderCreationResult result =
                CreateService().CreateDraft(
                    new PurchaseOrderId("order-a"),
                    catalog,
                    new[]
                    {
                        new PurchaseOrderRequestLine(ProductA, 2),
                        new PurchaseOrderRequestLine(ProductB, 1)
                    });

            Assert.That(result.Order.TotalBoxes, Is.EqualTo(3));
            Assert.That(result.Order.TotalUnits, Is.EqualTo(14));
            Assert.That(result.Order.TotalCostCents, Is.EqualTo(2200));
        }

        private static SupplierOrderService CreateService()
        {
            return new SupplierOrderService();
        }

        private static SupplierCatalog CreateCatalog(
            int minimumBoxes = 1,
            int maximumBoxes = 5)
        {
            ProductDefinitionRegistry registry =
                CreateRegistry(ProductA);

            return new SupplierCatalog(
                new SupplierCatalogId("catalog-a"),
                CreateSupplier(),
                registry,
                new[]
                {
                    new SupplierCatalogEntry(
                        ProductA,
                        100,
                        new Quantity(6),
                        minimumBoxes,
                        maximumBoxes)
                });
        }

        private static SupplierCatalog CreateCatalogWithTwoProducts()
        {
            ProductDefinitionRegistry registry =
                CreateRegistry(ProductA, ProductB);

            return new SupplierCatalog(
                new SupplierCatalogId("catalog-a"),
                CreateSupplier(),
                registry,
                new[]
                {
                    new SupplierCatalogEntry(
                        ProductA,
                        100,
                        new Quantity(6),
                        1,
                        5),
                    new SupplierCatalogEntry(
                        ProductB,
                        500,
                        new Quantity(2),
                        1,
                        5)
                });
        }

        private static SupplierDefinition CreateSupplier()
        {
            return new SupplierDefinition(
                new SupplierId("supplier-a"),
                "suppliers.a.name");
        }

        private static ProductDefinitionRegistry CreateRegistry(
            params ProductDefinitionId[] ids)
        {
            ProductDefinition[] definitions =
                new ProductDefinition[ids.Length];

            for (int index = 0; index < ids.Length; index++)
            {
                definitions[index] =
                    new ProductDefinition(
                        ids[index],
                        $"products.{ids[index].Value}.name",
                        new ProductCategoryId("category"),
                        Array.Empty<ProductTagId>());
            }

            return new ProductDefinitionRegistry(definitions);
        }
    }
}
