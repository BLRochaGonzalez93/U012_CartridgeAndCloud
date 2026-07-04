using NUnit.Framework;
using System;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.Products;
using VRMGames.CartridgeAndCloud.Domain.Suppliers;
using VRMGames.CartridgeAndCloud.Infrastructure.Products;
using VRMGames.CartridgeAndCloud.Infrastructure.Suppliers;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Authoring
{
    public sealed class CatalogAuthoringAssetTests
    {
        [Test]
        public void SupplierAsset_ConfigureAndBuild_CreatesDefinition()
        {
            SupplierDefinitionAsset asset =
                CreateSupplier();

            try
            {
                SupplierDefinition definition =
                    asset.BuildDefinition();

                Assert.That(
                    definition.Id.Value,
                    Is.EqualTo("supplier-a"));

                Assert.That(
                    definition.DisplayNameKey,
                    Is.EqualTo("suppliers.a.name"));
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(asset);
            }
        }

        [Test]
        public void SupplierEntry_WithoutProduct_BuildThrowsInvalidOperationException()
        {
            SupplierCatalogEntryAsset entry =
                new SupplierCatalogEntryAsset();

            entry.Configure(
                null,
                100,
                6,
                1,
                5);

            Assert.Throws<InvalidOperationException>(
                () => entry.BuildEntry());
        }

        [Test]
        public void SupplierCatalog_WithoutSupplier_BuildThrowsInvalidOperationException()
        {
            ProductCatalogAsset productCatalog;
            ProductDefinitionAsset product;

            CreateProductCatalog(
                out productCatalog,
                out product);

            SupplierCatalogAsset catalog =
                ScriptableObject.CreateInstance<
                    SupplierCatalogAsset>();

            try
            {
                catalog.Configure(
                    "catalog-a",
                    null,
                    productCatalog,
                    Array.Empty<
                        SupplierCatalogEntryAsset>());

                Assert.Throws<InvalidOperationException>(
                    () => catalog.BuildCatalog());
            }
            finally
            {
                DestroyCatalogAssets(
                    catalog,
                    productCatalog,
                    product,
                    null);
            }
        }

        [Test]
        public void SupplierCatalog_WithoutProductCatalog_BuildThrowsInvalidOperationException()
        {
            SupplierDefinitionAsset supplier =
                CreateSupplier();

            SupplierCatalogAsset catalog =
                ScriptableObject.CreateInstance<
                    SupplierCatalogAsset>();

            try
            {
                catalog.Configure(
                    "catalog-a",
                    supplier,
                    null,
                    Array.Empty<
                        SupplierCatalogEntryAsset>());

                Assert.Throws<InvalidOperationException>(
                    () => catalog.BuildCatalog());
            }
            finally
            {
                DestroyCatalogAssets(
                    catalog,
                    null,
                    null,
                    supplier);
            }
        }

        [Test]
        public void SupplierCatalog_ValidAssets_BuildsDomainCatalog()
        {
            ProductCatalogAsset productCatalog;
            ProductDefinitionAsset product;

            CreateProductCatalog(
                out productCatalog,
                out product);

            SupplierDefinitionAsset supplier =
                CreateSupplier();

            SupplierCatalogEntryAsset entry =
                CreateEntry(product);

            SupplierCatalogAsset catalog =
                CreateCatalog(
                    supplier,
                    productCatalog,
                    entry);

            try
            {
                SupplierCatalog domainCatalog =
                    catalog.BuildCatalog();

                Assert.That(
                    domainCatalog.Count,
                    Is.EqualTo(1));

                Assert.That(
                    domainCatalog.Supplier.Id.Value,
                    Is.EqualTo("supplier-a"));
            }
            finally
            {
                DestroyCatalogAssets(
                    catalog,
                    productCatalog,
                    product,
                    supplier);
            }
        }

        [Test]
        public void SupplierCatalog_ProductOutsideProductCatalog_BuildThrowsArgumentException()
        {
            ProductCatalogAsset productCatalog;
            ProductDefinitionAsset includedProduct;

            CreateProductCatalog(
                out productCatalog,
                out includedProduct);

            ProductDefinitionAsset externalProduct =
                CreateProduct("product-b");

            SupplierDefinitionAsset supplier =
                CreateSupplier();

            SupplierCatalogAsset catalog =
                CreateCatalog(
                    supplier,
                    productCatalog,
                    CreateEntry(externalProduct));

            try
            {
                Assert.Throws<ArgumentException>(
                    () => catalog.BuildCatalog());
            }
            finally
            {
                DestroyCatalogAssets(
                    catalog,
                    productCatalog,
                    includedProduct,
                    supplier);

                UnityEngine.Object.DestroyImmediate(
                    externalProduct);
            }
        }

        private static SupplierDefinitionAsset CreateSupplier()
        {
            SupplierDefinitionAsset asset =
                ScriptableObject.CreateInstance<
                    SupplierDefinitionAsset>();

            asset.Configure(
                "supplier-a",
                "suppliers.a.name");

            return asset;
        }

        [Test]
        public void ProductAsset_ConfigureAndBuild_CreatesDomainDefinition()
        {
            ProductDefinitionAsset asset =
                CreateProduct("product-a");

            try
            {
                ProductDefinition definition =
                    asset.BuildDefinition();

                Assert.That(
                    definition.Id.Value,
                    Is.EqualTo("product-a"));

                Assert.That(
                    definition.CategoryId.Value,
                    Is.EqualTo("game"));

                Assert.That(
                    definition.Tags.Count,
                    Is.EqualTo(2));
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(asset);
            }
        }

        [Test]
        public void ProductAsset_EmptyId_BuildThrowsArgumentException()
        {
            ProductDefinitionAsset asset =
                CreateProduct(string.Empty);

            try
            {
                Assert.Throws<ArgumentException>(
                    () => asset.BuildDefinition());
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(asset);
            }
        }

        [Test]
        public void ProductCatalog_NullReference_BuildThrowsInvalidOperationException()
        {
            ProductCatalogAsset catalog =
                ScriptableObject.CreateInstance<
                    ProductCatalogAsset>();

            try
            {
                catalog.Configure(
                    new ProductDefinitionAsset[] { null });

                Assert.Throws<InvalidOperationException>(
                    () => catalog.BuildRegistry());
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(catalog);
            }
        }

        [Test]
        public void ProductCatalog_DuplicateIds_BuildThrowsArgumentException()
        {
            ProductDefinitionAsset first =
                CreateProduct("product-a");

            ProductDefinitionAsset duplicate =
                CreateProduct("product-a");

            ProductCatalogAsset catalog =
                CreateCatalog(first, duplicate);

            try
            {
                Assert.Throws<ArgumentException>(
                    () => catalog.BuildRegistry());
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(catalog);
                UnityEngine.Object.DestroyImmediate(first);
                UnityEngine.Object.DestroyImmediate(duplicate);
            }
        }

        [Test]
        public void ProductCatalog_ValidAssets_BuildsSearchableRegistry()
        {
            ProductDefinitionAsset first =
                CreateProduct("product-a");

            ProductDefinitionAsset second =
                CreateProduct("product-b");

            ProductCatalogAsset catalog =
                CreateCatalog(first, second);

            try
            {
                ProductDefinitionRegistry registry =
                    catalog.BuildRegistry();

                Assert.That(registry.Count, Is.EqualTo(2));
                Assert.That(
                    registry.Contains(
                        new ProductDefinitionId(
                            "product-b")),
                    Is.True);
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(catalog);
                UnityEngine.Object.DestroyImmediate(first);
                UnityEngine.Object.DestroyImmediate(second);
            }
        }

        [Test]
        public void ProductAsset_VisualReferences_AreOptional()
        {
            ProductDefinitionAsset asset =
                CreateProduct("product-a");

            try
            {
                Assert.That(asset.Icon, Is.Null);
                Assert.That(asset.ProductPrefab, Is.Null);
                Assert.DoesNotThrow(
                    () => asset.BuildDefinition());
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(asset);
            }
        }

        private static ProductDefinitionAsset CreateProduct(
            string id)
        {
            ProductDefinitionAsset asset =
                ScriptableObject.CreateInstance<
                    ProductDefinitionAsset>();

            asset.Configure(
                id,
                "products.test.name",
                "game",
                new[] { "retro", "cartridge" });

            return asset;
        }

        private static ProductCatalogAsset CreateCatalog(
            params ProductDefinitionAsset[] products)
        {
            ProductCatalogAsset catalog =
                ScriptableObject.CreateInstance<
                    ProductCatalogAsset>();

            catalog.Configure(products);
            return catalog;
        }

        private static void CreateProductCatalog(
            out ProductCatalogAsset catalog,
            out ProductDefinitionAsset product)
        {
            product = CreateProduct("product-a");

            catalog =
                ScriptableObject.CreateInstance<
                    ProductCatalogAsset>();

            catalog.Configure(
                new[] { product });
        }

        private static SupplierCatalogEntryAsset CreateEntry(
            ProductDefinitionAsset product)
        {
            SupplierCatalogEntryAsset entry =
                new SupplierCatalogEntryAsset();

            entry.Configure(
                product,
                100,
                6,
                1,
                5);

            return entry;
        }

        private static SupplierCatalogAsset CreateCatalog(
            SupplierDefinitionAsset supplier,
            ProductCatalogAsset productCatalog,
            params SupplierCatalogEntryAsset[] entries)
        {
            SupplierCatalogAsset catalog =
                ScriptableObject.CreateInstance<
                    SupplierCatalogAsset>();

            catalog.Configure(
                "catalog-a",
                supplier,
                productCatalog,
                entries);

            return catalog;
        }

        private static void DestroyCatalogAssets(
            SupplierCatalogAsset catalog,
            ProductCatalogAsset productCatalog,
            ProductDefinitionAsset product,
            SupplierDefinitionAsset supplier)
        {
            if (catalog != null)
            {
                UnityEngine.Object.DestroyImmediate(catalog);
            }

            if (productCatalog != null)
            {
                UnityEngine.Object.DestroyImmediate(
                    productCatalog);
            }

            if (product != null)
            {
                UnityEngine.Object.DestroyImmediate(product);
            }

            if (supplier != null)
            {
                UnityEngine.Object.DestroyImmediate(supplier);
            }
        }
    }
}
