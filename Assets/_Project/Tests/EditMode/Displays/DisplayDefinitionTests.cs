using System;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.Displays;
using VRMGames.CartridgeAndCloud.Domain.Inventory;
using VRMGames.CartridgeAndCloud.Domain.Products;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Displays
{
    public sealed class DisplayDefinitionTests
    {
        [Test]
        public void Constructor_ZeroCapacity_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => CreateDefinition(0, 1));
        }

        [Test]
        public void Constructor_VisibleLimitAboveCapacity_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => CreateDefinition(4, 5));
        }

        [Test]
        public void Constructor_DuplicateCategory_Throws()
        {
            Assert.Throws<ArgumentException>(
                () => new DisplayDefinition(
                    new DisplayDefinitionId("display"),
                    "display.name",
                    new InventoryCapacity(4),
                    2,
                    new[]
                    {
                        new ProductCategoryId("game"),
                        new ProductCategoryId("game")
                    },
                    "placement"));
        }

        [Test]
        public void CanAccept_EmptyCategoryList_AcceptsAnyProduct()
        {
            DisplayDefinition definition =
                CreateDefinition(4, 2);

            Assert.That(
                definition.CanAccept(CreateProduct("console")),
                Is.True);
        }

        [Test]
        public void CanAccept_AllowedCategory_ReturnsTrue()
        {
            DisplayDefinition definition =
                CreateDefinition(
                    4,
                    2,
                    new ProductCategoryId("video-game"));

            Assert.That(
                definition.CanAccept(CreateProduct("video-game")),
                Is.True);
        }

        [Test]
        public void CanAccept_DisallowedCategory_ReturnsFalse()
        {
            DisplayDefinition definition =
                CreateDefinition(
                    4,
                    2,
                    new ProductCategoryId("accessory"));

            Assert.That(
                definition.CanAccept(CreateProduct("console")),
                Is.False);
        }

        [Test]
        public void Registry_SortsDefinitionsByOrdinalId()
        {
            DisplayDefinitionRegistry registry =
                new DisplayDefinitionRegistry(
                    new[]
                    {
                        CreateDefinition(4, 2, id: "z"),
                        CreateDefinition(4, 2, id: "a")
                    });

            Assert.That(
                registry.Definitions[0].Id.Value,
                Is.EqualTo("a"));
        }

        [Test]
        public void Registry_DuplicateId_Throws()
        {
            Assert.Throws<ArgumentException>(
                () => new DisplayDefinitionRegistry(
                    new[]
                    {
                        CreateDefinition(4, 2, id: "same"),
                        CreateDefinition(4, 2, id: "same")
                    }));
        }

        private static DisplayDefinition CreateDefinition(
            int capacity,
            int visible,
            ProductCategoryId? category = null,
            string id = "display")
        {
            ProductCategoryId[] categories = category.HasValue
                ? new[] { category.Value }
                : Array.Empty<ProductCategoryId>();

            return new DisplayDefinition(
                new DisplayDefinitionId(id),
                "display.name",
                new InventoryCapacity(capacity),
                visible,
                categories,
                "placement");
        }

        private static ProductDefinition CreateProduct(string category)
        {
            return new ProductDefinition(
                new ProductDefinitionId("product"),
                "product.name",
                new ProductCategoryId(category),
                Array.Empty<ProductTagId>());
        }
    }
}
