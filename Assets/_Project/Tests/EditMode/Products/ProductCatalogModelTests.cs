using System;
using System.Collections.Generic;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.Products;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Products
{
    public sealed class ProductCatalogModelTests
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

        [Test]
        public void ProductDefinitionId_Whitespace_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(
                () => new ProductDefinitionId(" "));
        }

        [Test]
        public void ProductDefinitionId_SameOrdinalValue_AreEqual()
        {
            ProductDefinitionId left =
                new ProductDefinitionId("cartridge-basic");

            ProductDefinitionId right =
                new ProductDefinitionId("cartridge-basic");

            Assert.That(left, Is.EqualTo(right));
            Assert.That(left == right, Is.True);
        }

        [Test]
        public void ProductDefinitionId_DifferentCase_AreNotEqual()
        {
            ProductDefinitionId left =
                new ProductDefinitionId("cartridge-basic");

            ProductDefinitionId right =
                new ProductDefinitionId("Cartridge-Basic");

            Assert.That(left, Is.Not.EqualTo(right));
        }

        [Test]
        public void ProductDefinitionId_ToString_ReturnsValue()
        {
            ProductDefinitionId id =
                new ProductDefinitionId("cloud-basic");

            Assert.That(id.ToString(), Is.EqualTo("cloud-basic"));
        }

        [Test]
        public void ProductCategoryId_Empty_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(
                () => new ProductCategoryId(string.Empty));
        }

        [Test]
        public void ProductTagId_Empty_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(
                () => new ProductTagId(string.Empty));
        }

        [Test]
        public void Constructor_NullDefinitions_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => new ProductDefinitionRegistry(null));
        }

        [Test]
        public void Constructor_NullDefinition_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(
                () => new ProductDefinitionRegistry(
                    new ProductDefinition[] { null }));
        }

        [Test]
        public void Constructor_DuplicateIds_ThrowsArgumentException()
        {
            ProductDefinition first = CreateDefinition("product-a");
            ProductDefinition duplicate = CreateDefinition("product-a");

            Assert.Throws<ArgumentException>(
                () => new ProductDefinitionRegistry(
                    new[] { first, duplicate }));
        }

        [Test]
        public void TryGet_ExistingDefinition_ReturnsDefinition()
        {
            ProductDefinition expected = CreateDefinition("product-a");
            ProductDefinitionRegistry registry =
                new ProductDefinitionRegistry(new[] { expected });

            bool found = registry.TryGet(
                expected.Id,
                out ProductDefinition actual);

            Assert.That(found, Is.True);
            Assert.That(actual, Is.SameAs(expected));
        }

        [Test]
        public void TryGet_MissingDefinition_ReturnsFalse()
        {
            ProductDefinitionRegistry registry =
                new ProductDefinitionRegistry(
                    Array.Empty<ProductDefinition>());

            bool found = registry.TryGet(
                new ProductDefinitionId("missing"),
                out ProductDefinition definition);

            Assert.That(found, Is.False);
            Assert.That(definition, Is.Null);
        }

        [Test]
        public void Get_MissingDefinition_ThrowsKeyNotFoundException()
        {
            ProductDefinitionRegistry registry =
                new ProductDefinitionRegistry(
                    Array.Empty<ProductDefinition>());

            Assert.Throws<KeyNotFoundException>(
                () => registry.Get(
                    new ProductDefinitionId("missing")));
        }

        [Test]
        public void Definitions_UnorderedInput_ReturnsOrdinalOrder()
        {
            ProductDefinitionRegistry registry =
                new ProductDefinitionRegistry(
                    new[]
                    {
                        CreateDefinition("product-z"),
                        CreateDefinition("product-a")
                    });

            Assert.That(registry.Count, Is.EqualTo(2));
            Assert.That(
                registry.Definitions[0].Id.Value,
                Is.EqualTo("product-a"));

            Assert.That(
                registry.Definitions[1].Id.Value,
                Is.EqualTo("product-z"));
        }

        private static ProductDefinition CreateDefinition(string id)
        {
            return new ProductDefinition(
                new ProductDefinitionId(id),
                $"products.{id}.name",
                new ProductCategoryId("cartridge"),
                Array.Empty<ProductTagId>());
        }
    }
}
