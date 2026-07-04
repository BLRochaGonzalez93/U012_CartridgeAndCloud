using NUnit.Framework;
using System;
using VRMGames.CartridgeAndCloud.Application.Store;
using VRMGames.CartridgeAndCloud.Application.UIUX;
using VRMGames.CartridgeAndCloud.Domain.Economy;
using VRMGames.CartridgeAndCloud.Domain.Products;
using VRMGames.CartridgeAndCloud.Domain.Store;
namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Store
{
    public sealed class StoreContentCatalogTests
    {

        [Test]
        public void Catalog_ExposesFurniture()
        {
            StoreContentCatalog catalog =
                StoreOperationsTestFactory.Catalog();

            Assert.That(
                catalog.Furniture.Count,
                Is.EqualTo(4));
        }

        [Test]
        public void Catalog_ExposesProducts()
        {
            StoreContentCatalog catalog =
                StoreOperationsTestFactory.Catalog();

            Assert.That(
                catalog.Products.Count,
                Is.EqualTo(2));
        }

        [Test]
        public void Catalog_FindsFurnitureById()
        {
            bool found =
                StoreOperationsTestFactory.Catalog()
                    .TryGetFurniture(
                        "central-shelf",
                        out StoreFixtureDefinition
                            definition);

            Assert.That(found, Is.True);
            Assert.That(
                definition.Kind,
                Is.EqualTo(
                    StoreFixtureKind.CentralShelf));
        }

        [Test]
        public void Catalog_FindsProductById()
        {
            bool found =
                StoreOperationsTestFactory.Catalog()
                    .TryGetProduct(
                        "game-neon-drift",
                        out RetailProductDefinition
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
                StoreOperationsTestFactory.Catalog()
                    .TryGetFurniture(
                        "missing",
                        out _),
                Is.False);
        }

        [Test]
        public void Catalog_MissingProductReturnsFalse()
        {
            Assert.That(
                StoreOperationsTestFactory.Catalog()
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
                () => new StoreContentCatalog(
                    null,
                    new RetailProductDefinition[0]));
        }

        [Test]
        public void Catalog_RejectsNullProducts()
        {
            Assert.Throws<
                ArgumentNullException>(
                () => new StoreContentCatalog(
                    new StoreFixtureDefinition[0],
                    null));
        }

        [Test]
        public void Catalog_RejectsDuplicateFurniture()
        {
            StoreFixtureDefinition item =
                StoreOperationsTestFactory.Furniture(
                    "duplicate",
                    StoreFixtureKind.CentralShelf,
                    2,
                    2,
                    1f,
                    5,
                    100,
                    true,
                    true,
                    true);

            Assert.Throws<ArgumentException>(
                () => new StoreContentCatalog(
                    new[] { item, item },
                    new RetailProductDefinition[0]));
        }

        [Test]
        public void Catalog_RejectsDuplicateProducts()
        {
            RetailProductDefinition item =
                StoreOperationsTestFactory.Product(
                    "duplicate",
                    RetailProductKind.Accessory,
                    100,
                    200,
                    1);

            Assert.Throws<ArgumentException>(
                () => new StoreContentCatalog(
                    new StoreFixtureDefinition[0],
                    new[] { item, item }));
        }

        [Test]
        public void Furniture_StoresDefinitionId()
        {
            Assert.That(
                Furniture().DefinitionId,
                Is.EqualTo("fixture"));
        }

        [Test]
        public void Furniture_StoresFootprint()
        {
            StoreFixtureDefinition item =
                Furniture();

            Assert.That(item.WidthCells, Is.EqualTo(2));
            Assert.That(item.DepthCells, Is.EqualTo(3));
        }

        [Test]
        public void Furniture_StoresCapacity()
        {
            Assert.That(
                Furniture().Capacity,
                Is.EqualTo(12));
        }

        [Test]
        public void Furniture_StoresPurchasability()
        {
            Assert.That(
                Furniture().IsPurchasable,
                Is.True);
        }

        [Test]
        public void Furniture_RejectsEmptyId()
        {
            Assert.Throws<ArgumentException>(
                () => new StoreFixtureDefinition(
                    "",
                    "Fixture",
                    StoreFixtureKind.CentralShelf,
                    2,
                    2,
                    1f,
                    1,
                    100,
                    true,
                    true,
                    true,
                    "",
                    ""));
        }

        [Test]
        public void Furniture_RejectsZeroWidth()
        {
            Assert.Throws<
                ArgumentOutOfRangeException>(
                () => CreateFurniture(width: 0));
        }

        [Test]
        public void Furniture_RejectsZeroDepth()
        {
            Assert.Throws<
                ArgumentOutOfRangeException>(
                () => CreateFurniture(depth: 0));
        }

        [Test]
        public void Furniture_RejectsZeroHeight()
        {
            Assert.Throws<
                ArgumentOutOfRangeException>(
                () => CreateFurniture(height: 0f));
        }

        [Test]
        public void Furniture_RejectsNegativeCapacity()
        {
            Assert.Throws<
                ArgumentOutOfRangeException>(
                () => CreateFurniture(capacity: -1));
        }

        [Test]
        public void Furniture_RejectsZeroCost()
        {
            Assert.Throws<
                ArgumentOutOfRangeException>(
                () => CreateFurniture(cost: 0));
        }

        [Test]
        public void Furniture_RejectsUnknownKind()
        {
            Assert.Throws<
                ArgumentOutOfRangeException>(
                () => new StoreFixtureDefinition(
                    "fixture",
                    "Fixture",
                    (StoreFixtureKind)99,
                    1,
                    1,
                    1f,
                    0,
                    100,
                    false,
                    false,
                    false,
                    "",
                    ""));
        }

        [Test]
        public void Product_StoresPrices()
        {
            RetailProductDefinition item =
                Product();

            Assert.That(
                item.WholesalePriceCents,
                Is.EqualTo(100));
            Assert.That(
                item.SalePriceCents,
                Is.EqualTo(200));
        }

        [Test]
        public void Product_StoresUnitsPerCase()
        {
            Assert.That(
                Product().UnitsPerCase,
                Is.EqualTo(6));
        }

        [Test]
        public void Product_RejectsEmptyId()
        {
            Assert.Throws<ArgumentException>(
                () => new RetailProductDefinition(
                    "",
                    "Product",
                    RetailProductKind.Accessory,
                    100,
                    200,
                    1,
                    "",
                    "",
                    "",
                    "",
                    ""));
        }

        [Test]
        public void Product_RejectsZeroWholesale()
        {
            Assert.Throws<
                ArgumentOutOfRangeException>(
                () => CreateProduct(
                    wholesale: 0));
        }

        [Test]
        public void Product_RejectsSaleEqualToWholesale()
        {
            Assert.Throws<
                ArgumentOutOfRangeException>(
                () => CreateProduct(
                    wholesale: 100,
                    sale: 100));
        }

        [Test]
        public void Product_RejectsSaleBelowWholesale()
        {
            Assert.Throws<
                ArgumentOutOfRangeException>(
                () => CreateProduct(
                    wholesale: 100,
                    sale: 99));
        }

        [Test]
        public void Product_RejectsZeroUnitsPerCase()
        {
            Assert.Throws<
                ArgumentOutOfRangeException>(
                () => CreateProduct(units: 0));
        }

        [Test]
        public void Feedback_StoresAnchor()
        {
            GameplayFeedbackEvent feedback =
                new GameplayFeedbackEvent(
                    GameplayFeedbackType.Restocked,
                    "Restocked",
                    "display-a");

            Assert.That(
                feedback.AnchorId,
                Is.EqualTo("display-a"));
        }

        [Test]
        public void Feedback_StoresMoney()
        {
            GameplayFeedbackEvent feedback =
                new GameplayFeedbackEvent(
                    GameplayFeedbackType.Revenue,
                    "Revenue",
                    "cash",
                    2999,
                    "eur");

            Assert.That(
                feedback.HasMoney,
                Is.True);
            Assert.That(
                feedback.CurrencyCode,
                Is.EqualTo("EUR"));
        }

        [Test]
        public void Feedback_RejectsEmptyMessage()
        {
            Assert.Throws<ArgumentException>(
                () => new GameplayFeedbackEvent(
                    GameplayFeedbackType.Revenue,
                    ""));
        }

        [Test]
        public void Feedback_RejectsMoneyWithoutCurrency()
        {
            Assert.Throws<ArgumentException>(
                () => new GameplayFeedbackEvent(
                    GameplayFeedbackType.Revenue,
                    "Revenue",
                    "",
                    100,
                    ""));
        }

        [Test]
        public void OperationResult_SuccessIsSucceeded()
        {
            Assert.That(
                StoreOperationResult
                    .Success("done")
                    .Succeeded,
                Is.True);
        }

        [Test]
        public void OperationResult_FailureIsNotSucceeded()
        {
            Assert.That(
                StoreOperationResult
                    .Failure(
                        StoreOperationStatus.NotFound,
                        "missing")
                    .Succeeded,
                Is.False);
        }

        private static StoreFixtureDefinition
            Furniture()
        {
            return CreateFurniture();
        }

        private static StoreFixtureDefinition
            CreateFurniture(
                int width = 2,
                int depth = 3,
                float height = 1.2f,
                int capacity = 12,
                long cost = 1000)
        {
            return new StoreFixtureDefinition(
                "fixture",
                "Fixture",
                StoreFixtureKind.CentralShelf,
                width,
                depth,
                height,
                capacity,
                cost,
                true,
                true,
                true,
                "material",
                "prefab");
        }

        private static RetailProductDefinition
            Product()
        {
            return CreateProduct();
        }

        private static RetailProductDefinition
            CreateProduct(
                long wholesale = 100,
                long sale = 200,
                int units = 6)
        {
            return new RetailProductDefinition(
                "product",
                "Product",
                RetailProductKind.Accessory,
                wholesale,
                sale,
                units,
                "material",
                "label",
                "icon",
                "cover",
                "prefab");
        }
    }
}
