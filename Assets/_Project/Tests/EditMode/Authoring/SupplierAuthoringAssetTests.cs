using System;
using NUnit.Framework;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.Suppliers;
using VRMGames.CartridgeAndCloud.Infrastructure.Products;
using VRMGames.CartridgeAndCloud.Infrastructure.Suppliers;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Authoring
{
    public sealed class SupplierAuthoringAssetTests
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
                Array.Empty<string>());

            return asset;
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
