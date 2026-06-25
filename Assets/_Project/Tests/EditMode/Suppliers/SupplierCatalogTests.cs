using System;
using System.Collections.Generic;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.Inventory;
using VRMGames.CartridgeAndCloud.Domain.Products;
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
