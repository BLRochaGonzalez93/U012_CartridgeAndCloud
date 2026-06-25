using System;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.Displays;
using VRMGames.CartridgeAndCloud.Domain.Inventory;
using VRMGames.CartridgeAndCloud.Domain.Products;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Displays
{
    public sealed class DisplayInstanceTests
    {
        [Test]
        public void Constructor_CreatesDisplayInventoryWithDefinitionCapacity()
        {
            DisplayInstance display = CreateDisplay();

            Assert.That(
                display.Inventory.Type,
                Is.EqualTo(InventoryContainerType.Display));
            Assert.That(
                display.Inventory.Capacity.Units,
                Is.EqualTo(6));
            Assert.That(display.IsEmpty, Is.True);
        }

        [Test]
        public void AssignProduct_ValidProduct_Succeeds()
        {
            DisplayInstance display = CreateDisplay();

            DisplayAssignmentResult result =
                display.TryAssignProduct(
                    CreateRegistry(),
                    new ProductDefinitionId("game"));

            Assert.That(result.Succeeded, Is.True);
            Assert.That(display.HasAssignedProduct, Is.True);
            Assert.That(
                display.AssignedProductId.Value,
                Is.EqualTo("game"));
        }

        [Test]
        public void AssignProduct_MissingDefinition_FailsWithoutMutation()
        {
            DisplayInstance display = CreateDisplay();

            DisplayAssignmentResult result =
                display.TryAssignProduct(
                    CreateRegistry(),
                    new ProductDefinitionId("missing"));

            Assert.That(result.Succeeded, Is.False);
            Assert.That(
                result.FailureReason,
                Is.EqualTo(DisplayAssignmentFailureReason
                    .ProductDefinitionMissing));
            Assert.That(display.HasAssignedProduct, Is.False);
        }

        [Test]
        public void AssignProduct_DisallowedCategory_Fails()
        {
            DisplayInstance display = CreateDisplay();

            DisplayAssignmentResult result =
                display.TryAssignProduct(
                    CreateRegistry(),
                    new ProductDefinitionId("console"));

            Assert.That(
                result.FailureReason,
                Is.EqualTo(DisplayAssignmentFailureReason
                    .CategoryNotAllowed));
        }

        [Test]
        public void AssignProduct_SameProductTwice_Fails()
        {
            DisplayInstance display = CreateAssignedDisplay();

            DisplayAssignmentResult result =
                display.TryAssignProduct(
                    CreateRegistry(),
                    new ProductDefinitionId("game"));

            Assert.That(
                result.FailureReason,
                Is.EqualTo(DisplayAssignmentFailureReason
                    .ProductAlreadyAssigned));
        }

        [Test]
        public void AssignProduct_DifferentProductWhileAssigned_Fails()
        {
            DisplayInstance display = CreateAssignedDisplay();

            DisplayAssignmentResult result =
                display.TryAssignProduct(
                    CreateRegistry(),
                    new ProductDefinitionId("game-two"));

            Assert.That(
                result.FailureReason,
                Is.EqualTo(DisplayAssignmentFailureReason
                    .DifferentProductAlreadyAssigned));
        }

        [Test]
        public void ClearAssignment_EmptyAssignedDisplay_Succeeds()
        {
            DisplayInstance display = CreateAssignedDisplay();

            DisplayClearAssignmentResult result =
                display.TryClearAssignment();

            Assert.That(result.Succeeded, Is.True);
            Assert.That(display.HasAssignedProduct, Is.False);
        }

        [Test]
        public void ClearAssignment_NoAssignment_Fails()
        {
            DisplayClearAssignmentResult result =
                CreateDisplay().TryClearAssignment();

            Assert.That(
                result.FailureReason,
                Is.EqualTo(DisplayClearAssignmentFailureReason
                    .NoAssignedProduct));
        }

        [Test]
        public void ClearAssignment_WithStock_FailsAndPreservesAssignment()
        {
            DisplayInstance display = CreateAssignedDisplay();
            display.Inventory.TryAdd(
                display.AssignedProductId,
                new Quantity(2));

            DisplayClearAssignmentResult result =
                display.TryClearAssignment();

            Assert.That(
                result.FailureReason,
                Is.EqualTo(DisplayClearAssignmentFailureReason
                    .StockRemaining));
            Assert.That(display.HasAssignedProduct, Is.True);
        }

        [Test]
        public void VisibleUnitCount_UsesConfiguredLimit()
        {
            DisplayInstance display = CreateAssignedDisplay();
            display.Inventory.TryAdd(
                display.AssignedProductId,
                new Quantity(5));

            Assert.That(display.VisibleUnitCount, Is.EqualTo(3));
        }

        [Test]
        public void VisibleUnitCount_BelowLimit_ReturnsActualQuantity()
        {
            DisplayInstance display = CreateAssignedDisplay();
            display.Inventory.TryAdd(
                display.AssignedProductId,
                new Quantity(2));

            Assert.That(display.VisibleUnitCount, Is.EqualTo(2));
        }

        [Test]
        public void VisibleUnitCount_NoAssignment_ReturnsZero()
        {
            Assert.That(CreateDisplay().VisibleUnitCount, Is.Zero);
        }

        [Test]
        public void InventoryId_IsDerivedFromDisplayId()
        {
            Assert.That(
                CreateDisplay().Inventory.Id.Value,
                Is.EqualTo("display:display-instance"));
        }

        private static DisplayInstance CreateDisplay()
        {
            return new DisplayInstance(
                new DisplayInstanceId("display-instance"),
                new DisplayDefinition(
                    new DisplayDefinitionId("shelf"),
                    "display.shelf",
                    new InventoryCapacity(6),
                    3,
                    new[]
                    {
                        new ProductCategoryId("video-game")
                    },
                    "technical-shelf"));
        }

        private static DisplayInstance CreateAssignedDisplay()
        {
            DisplayInstance display = CreateDisplay();
            display.TryAssignProduct(
                CreateRegistry(),
                new ProductDefinitionId("game"));
            return display;
        }

        private static ProductDefinitionRegistry CreateRegistry()
        {
            return new ProductDefinitionRegistry(
                new[]
                {
                    CreateProduct("game", "video-game"),
                    CreateProduct("game-two", "video-game"),
                    CreateProduct("console", "console")
                });
        }

        private static ProductDefinition CreateProduct(
            string id,
            string category)
        {
            return new ProductDefinition(
                new ProductDefinitionId(id),
                $"product.{id}",
                new ProductCategoryId(category),
                Array.Empty<ProductTagId>());
        }
    }
}
