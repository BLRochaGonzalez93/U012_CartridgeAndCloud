using NUnit.Framework;
using System;
using System.Collections.Generic;
using VRMGames.CartridgeAndCloud.Domain.Inventory;
using VRMGames.CartridgeAndCloud.Domain.Orders;
using VRMGames.CartridgeAndCloud.Domain.Products;
using VRMGames.CartridgeAndCloud.Domain.Receiving;
using VRMGames.CartridgeAndCloud.Domain.Suppliers;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Suppliers
{
    public sealed class SupplierCatalogTests
    {
        private static readonly ProductDefinitionId ProductA =
            new ProductDefinitionId("product-a");

        private static readonly ProductDefinitionId ProductB =
            new ProductDefinitionId("product-b");

        [Test]
        public void SupplierId_Whitespace_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(
                () => new SupplierId(" "));
        }

        [Test]
        public void SupplierId_SameOrdinalValue_AreEqual()
        {
            SupplierId left = new SupplierId("supplier-a");
            SupplierId right = new SupplierId("supplier-a");

            Assert.That(left, Is.EqualTo(right));
            Assert.That(left == right, Is.True);
        }

        [Test]
        public void SupplierCatalogId_DifferentCase_AreNotEqual()
        {
            SupplierCatalogId left =
                new SupplierCatalogId("catalog-a");

            SupplierCatalogId right =
                new SupplierCatalogId("Catalog-A");

            Assert.That(left, Is.Not.EqualTo(right));
        }

        [Test]
        public void PurchaseOrderId_ToString_ReturnsValue()
        {
            PurchaseOrderId id =
                new PurchaseOrderId("order-001");

            Assert.That(id.ToString(), Is.EqualTo("order-001"));
        }

        [Test]
        public void DeliveryId_Empty_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(
                () => new DeliveryId(string.Empty));
        }

        [Test]
        public void ShipmentBoxId_SameValue_HasSameHashCode()
        {
            ShipmentBoxId left =
                new ShipmentBoxId("box-001");

            ShipmentBoxId right =
                new ShipmentBoxId("box-001");

            Assert.That(
                left.GetHashCode(),
                Is.EqualTo(right.GetHashCode()));
        }

        [Test]
        public void SupplierDefinition_EmptyNameKey_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(
                () => new SupplierDefinition(
                    new SupplierId("supplier-a"),
                    string.Empty));
        }

        [Test]
        public void SupplierDefinition_ValidData_CapturesValues()
        {
            SupplierDefinition supplier =
                new SupplierDefinition(
                    new SupplierId("supplier-a"),
                    "suppliers.a.name");

            Assert.That(
                supplier.Id,
                Is.EqualTo(new SupplierId("supplier-a")));

            Assert.That(
                supplier.DisplayNameKey,
                Is.EqualTo("suppliers.a.name"));
        }

        [Test]
        public void Entry_ZeroUnitCost_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new SupplierCatalogEntry(
                    ProductA,
                    0,
                    new Quantity(6),
                    1,
                    5));
        }

        [Test]
        public void Entry_ZeroUnitsPerBox_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new SupplierCatalogEntry(
                    ProductA,
                    100,
                    Quantity.Zero,
                    1,
                    5));
        }

        [Test]
        public void Entry_MaximumBelowMinimum_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new SupplierCatalogEntry(
                    ProductA,
                    100,
                    new Quantity(6),
                    3,
                    2));
        }

        [Test]
        public void Entry_BoxCountWithinLimits_ReturnsOrderedQuantityAndCost()
        {
            SupplierCatalogEntry entry = CreateEntry(ProductA);

            Assert.That(entry.CanOrder(2), Is.True);
            Assert.That(entry.GetOrderedQuantity(2).Value, Is.EqualTo(12));
            Assert.That(entry.GetTotalCostCents(2), Is.EqualTo(1200));
        }

        [Test]
        public void Entry_BoxCountOutsideLimits_ThrowsArgumentOutOfRangeException()
        {
            SupplierCatalogEntry entry = CreateEntry(ProductA);

            Assert.Throws<ArgumentOutOfRangeException>(
                () => entry.GetOrderedQuantity(6));
        }

        [Test]
        public void Catalog_ProductMissingFromRegistry_ThrowsArgumentException()
        {
            ProductDefinitionRegistry registry = CreateRegistry(ProductA);

            Assert.Throws<ArgumentException>(
                () => CreateCatalog(
                    registry,
                    CreateEntry(ProductB)));
        }

        [Test]
        public void Catalog_DuplicateProduct_ThrowsArgumentException()
        {
            ProductDefinitionRegistry registry = CreateRegistry(ProductA);

            Assert.Throws<ArgumentException>(
                () => CreateCatalog(
                    registry,
                    CreateEntry(ProductA),
                    CreateEntry(ProductA)));
        }

        [Test]
        public void Catalog_UnorderedEntries_AreReturnedInOrdinalOrder()
        {
            ProductDefinitionRegistry registry =
                CreateRegistry(ProductA, ProductB);

            SupplierCatalog catalog =
                CreateCatalog(
                    registry,
                    CreateEntry(ProductB),
                    CreateEntry(ProductA));

            Assert.That(catalog.Count, Is.EqualTo(2));
            Assert.That(catalog.Entries[0].ProductId, Is.EqualTo(ProductA));
            Assert.That(catalog.Entries[1].ProductId, Is.EqualTo(ProductB));
        }

        [Test]
        public void TryGetEntry_ExistingProduct_ReturnsEntry()
        {
            SupplierCatalogEntry expected = CreateEntry(ProductA);
            SupplierCatalog catalog =
                CreateCatalog(CreateRegistry(ProductA), expected);

            bool found = catalog.TryGetEntry(
                ProductA,
                out SupplierCatalogEntry actual);

            Assert.That(found, Is.True);
            Assert.That(actual, Is.SameAs(expected));
        }

        [Test]
        public void GetEntry_MissingProduct_ThrowsKeyNotFoundException()
        {
            SupplierCatalog catalog =
                CreateCatalog(
                    CreateRegistry(ProductA),
                    CreateEntry(ProductA));

            Assert.Throws<KeyNotFoundException>(
                () => catalog.GetEntry(ProductB));
        }

        private static SupplierCatalogEntry CreateEntry(
            ProductDefinitionId productId)
        {
            return new SupplierCatalogEntry(
                productId,
                100,
                new Quantity(6),
                1,
                5);
        }

        private static SupplierCatalog CreateCatalog(
            ProductDefinitionRegistry registry,
            params SupplierCatalogEntry[] entries)
        {
            return new SupplierCatalog(
                new SupplierCatalogId("catalog-a"),
                new SupplierDefinition(
                    new SupplierId("supplier-a"),
                    "suppliers.a.name"),
                registry,
                entries);
        }

        private static ProductDefinitionRegistry CreateRegistry(
            params ProductDefinitionId[] productIds)
        {
            List<ProductDefinition> definitions =
                new List<ProductDefinition>();

            foreach (ProductDefinitionId id in productIds)
            {
                definitions.Add(
                    new ProductDefinition(
                        id,
                        $"products.{id.Value}.name",
                        new ProductCategoryId("category"),
                        Array.Empty<ProductTagId>()));
            }

            return new ProductDefinitionRegistry(definitions);
        }
    }
}
