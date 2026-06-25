using System;
using System.Collections.Generic;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.Products;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Products
{
    public sealed class ProductDefinitionTests
    {
        [Test]
        public void Constructor_ValidData_CreatesImmutableDefinition()
        {
            List<ProductTagId> tags =
                new List<ProductTagId>
                {
                    new ProductTagId("retro"),
                    new ProductTagId("portable")
                };

            ProductDefinition definition =
                new ProductDefinition(
                    new ProductDefinitionId("product-a"),
                    "products.product_a.name",
                    new ProductCategoryId("cartridge"),
                    tags);

            tags.Clear();

            Assert.That(
                definition.Id,
                Is.EqualTo(new ProductDefinitionId("product-a")));

            Assert.That(
                definition.DisplayNameKey,
                Is.EqualTo("products.product_a.name"));

            Assert.That(
                definition.CategoryId,
                Is.EqualTo(new ProductCategoryId("cartridge")));

            Assert.That(definition.Tags.Count, Is.EqualTo(2));
        }

        [Test]
        public void Constructor_EmptyDisplayNameKey_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(
                () => new ProductDefinition(
                    new ProductDefinitionId("product-a"),
                    string.Empty,
                    new ProductCategoryId("cartridge"),
                    Array.Empty<ProductTagId>()));
        }

        [Test]
        public void Constructor_NullTags_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => new ProductDefinition(
                    new ProductDefinitionId("product-a"),
                    "products.product_a.name",
                    new ProductCategoryId("cartridge"),
                    null));
        }

        [Test]
        public void Constructor_DuplicateTags_ThrowsArgumentException()
        {
            ProductTagId duplicate =
                new ProductTagId("retro");

            Assert.Throws<ArgumentException>(
                () => new ProductDefinition(
                    new ProductDefinitionId("product-a"),
                    "products.product_a.name",
                    new ProductCategoryId("cartridge"),
                    new[] { duplicate, duplicate }));
        }

        [Test]
        public void Constructor_DefaultTag_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(
                () => new ProductDefinition(
                    new ProductDefinitionId("product-a"),
                    "products.product_a.name",
                    new ProductCategoryId("cartridge"),
                    new[] { default(ProductTagId) }));
        }
    }
}
