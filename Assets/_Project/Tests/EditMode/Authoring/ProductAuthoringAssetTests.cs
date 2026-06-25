using System;
using NUnit.Framework;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.Products;
using VRMGames.CartridgeAndCloud.Infrastructure.Products;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Authoring
{
    public sealed class ProductAuthoringAssetTests
    {
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
    }
}
