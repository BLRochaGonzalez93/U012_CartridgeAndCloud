using System;
using System.Collections.Generic;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.Products;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Products
{
    public sealed class ProductDefinitionRegistryTests
    {
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
