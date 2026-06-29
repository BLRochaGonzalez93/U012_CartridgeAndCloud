using System;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.VerticalSlicePhase1;
using VRMGames.CartridgeAndCloud.Domain.VerticalSlicePhase1;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.VerticalSlicePhase1
{
    public sealed class Phase1CatalogTests
    {
        [Test]
        public void Catalog_ExposesFurniture()
        {
            Phase1Catalog catalog =
                Phase1TestFactory.Catalog();

            Assert.That(
                catalog.Furniture.Count,
                Is.EqualTo(4));
        }

        [Test]
        public void Catalog_ExposesProducts()
        {
            Phase1Catalog catalog =
                Phase1TestFactory.Catalog();

            Assert.That(
                catalog.Products.Count,
                Is.EqualTo(2));
        }

        [Test]
        public void Catalog_FindsFurnitureById()
        {
            bool found =
                Phase1TestFactory.Catalog()
                    .TryGetFurniture(
                        "central-shelf",
                        out Phase1FurnitureDefinition
                            definition);

            Assert.That(found, Is.True);
            Assert.That(
                definition.Kind,
                Is.EqualTo(
                    Phase1FurnitureKind.CentralShelf));
        }

        [Test]
        public void Catalog_FindsProductById()
        {
            bool found =
                Phase1TestFactory.Catalog()
                    .TryGetProduct(
                        "game-neon-drift",
                        out Phase1ProductDefinition
                            definition);

            Assert.That(found, Is.True);
            Assert.That(
                definition.SalePriceCents,
                Is.EqualTo(2999));
        }

        [Test]
        public void Catalog_MissingFurnitureReturnsFalse()
        {
            Assert.That(
                Phase1TestFactory.Catalog()
                    .TryGetFurniture(
                        "missing",
                        out _),
                Is.False);
        }

        [Test]
        public void Catalog_MissingProductReturnsFalse()
        {
            Assert.That(
                Phase1TestFactory.Catalog()
                    .TryGetProduct(
                        "missing",
                        out _),
                Is.False);
        }

        [Test]
        public void Catalog_RejectsNullFurniture()
        {
            Assert.Throws<
                ArgumentNullException>(
                () => new Phase1Catalog(
                    null,
                    new Phase1ProductDefinition[0]));
        }

        [Test]
        public void Catalog_RejectsNullProducts()
        {
            Assert.Throws<
                ArgumentNullException>(
                () => new Phase1Catalog(
                    new Phase1FurnitureDefinition[0],
                    null));
        }

        [Test]
        public void Catalog_RejectsDuplicateFurniture()
        {
            Phase1FurnitureDefinition item =
                Phase1TestFactory.Furniture(
                    "duplicate",
                    Phase1FurnitureKind.CentralShelf,
                    2,
                    2,
                    1f,
                    5,
                    100,
                    true,
                    true,
                    true);

            Assert.Throws<ArgumentException>(
                () => new Phase1Catalog(
                    new[] { item, item },
                    new Phase1ProductDefinition[0]));
        }

        [Test]
        public void Catalog_RejectsDuplicateProducts()
        {
            Phase1ProductDefinition item =
                Phase1TestFactory.Product(
                    "duplicate",
                    Phase1ProductKind.Accessory,
                    100,
                    200,
                    1);

            Assert.Throws<ArgumentException>(
                () => new Phase1Catalog(
                    new Phase1FurnitureDefinition[0],
                    new[] { item, item }));
        }
    }
}
