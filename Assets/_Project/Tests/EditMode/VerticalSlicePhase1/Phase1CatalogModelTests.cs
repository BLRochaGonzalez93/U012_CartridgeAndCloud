using System;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.VerticalSlicePhase1;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.VerticalSlicePhase1
{
    public sealed class Phase1CatalogModelTests
    {
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
            Phase1FurnitureDefinition item =
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
                () => new Phase1FurnitureDefinition(
                    "",
                    "Fixture",
                    Phase1FurnitureKind.CentralShelf,
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
                () => new Phase1FurnitureDefinition(
                    "fixture",
                    "Fixture",
                    (Phase1FurnitureKind)99,
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
            Phase1ProductDefinition item =
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
                () => new Phase1ProductDefinition(
                    "",
                    "Product",
                    Phase1ProductKind.Accessory,
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
            Phase1FeedbackEvent feedback =
                new Phase1FeedbackEvent(
                    Phase1FeedbackKind.Restocked,
                    "Restocked",
                    "display-a");

            Assert.That(
                feedback.AnchorId,
                Is.EqualTo("display-a"));
        }

        [Test]
        public void Feedback_StoresMoney()
        {
            Phase1FeedbackEvent feedback =
                new Phase1FeedbackEvent(
                    Phase1FeedbackKind.Revenue,
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
                () => new Phase1FeedbackEvent(
                    Phase1FeedbackKind.Revenue,
                    ""));
        }

        [Test]
        public void Feedback_RejectsMoneyWithoutCurrency()
        {
            Assert.Throws<ArgumentException>(
                () => new Phase1FeedbackEvent(
                    Phase1FeedbackKind.Revenue,
                    "Revenue",
                    "",
                    100,
                    ""));
        }

        [Test]
        public void OperationResult_SuccessIsSucceeded()
        {
            Assert.That(
                Phase1OperationResult
                    .Success("done")
                    .Succeeded,
                Is.True);
        }

        [Test]
        public void OperationResult_FailureIsNotSucceeded()
        {
            Assert.That(
                Phase1OperationResult
                    .Failure(
                        Phase1OperationStatus.NotFound,
                        "missing")
                    .Succeeded,
                Is.False);
        }

        private static Phase1FurnitureDefinition
            Furniture()
        {
            return CreateFurniture();
        }

        private static Phase1FurnitureDefinition
            CreateFurniture(
                int width = 2,
                int depth = 3,
                float height = 1.2f,
                int capacity = 12,
                long cost = 1000)
        {
            return new Phase1FurnitureDefinition(
                "fixture",
                "Fixture",
                Phase1FurnitureKind.CentralShelf,
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

        private static Phase1ProductDefinition
            Product()
        {
            return CreateProduct();
        }

        private static Phase1ProductDefinition
            CreateProduct(
                long wholesale = 100,
                long sale = 200,
                int units = 6)
        {
            return new Phase1ProductDefinition(
                "product",
                "Product",
                Phase1ProductKind.Accessory,
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
