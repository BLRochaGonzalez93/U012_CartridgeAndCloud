using System;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.Displays;
using VRMGames.CartridgeAndCloud.Domain.Displays;
using VRMGames.CartridgeAndCloud.Domain.Inventory;
using VRMGames.CartridgeAndCloud.Domain.Products;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Displays
{
    public sealed class DisplayRestockServiceTests
    {
        [Test]
        public void Restock_ValidQuantity_TransfersUnits()
        {
            Context context = CreateContext(sourceUnits: 10);

            DisplayRestockResult result =
                context.Service.Restock(
                    context.Source,
                    context.Display,
                    new Quantity(4));

            Assert.That(result.Succeeded, Is.True);
            Assert.That(
                context.Source.GetQuantity(context.ProductId).Value,
                Is.EqualTo(6));
            Assert.That(
                context.Display.Inventory
                    .GetQuantity(context.ProductId).Value,
                Is.EqualTo(4));
        }

        [Test]
        public void Restock_ValidQuantity_ConservesCombinedUnits()
        {
            Context context = CreateContext(sourceUnits: 10);
            int before = context.Source.UsedCapacity +
                         context.Display.Inventory.UsedCapacity;

            context.Service.Restock(
                context.Source,
                context.Display,
                new Quantity(4));

            int after = context.Source.UsedCapacity +
                        context.Display.Inventory.UsedCapacity;

            Assert.That(after, Is.EqualTo(before));
        }

        [Test]
        public void Restock_ZeroQuantity_FailsWithoutMutation()
        {
            Context context = CreateContext(sourceUnits: 10);

            DisplayRestockResult result =
                context.Service.Restock(
                    context.Source,
                    context.Display,
                    Quantity.Zero);

            AssertFailureUnchanged(
                context,
                result,
                DisplayRestockFailureReason.InvalidQuantity,
                10,
                0);
        }

        [Test]
        public void Restock_GenericSource_Fails()
        {
            Context context = CreateContext(
                sourceUnits: 10,
                sourceType: InventoryContainerType.Generic);

            DisplayRestockResult result =
                context.Service.Restock(
                    context.Source,
                    context.Display,
                    new Quantity(1));

            Assert.That(
                result.FailureReason,
                Is.EqualTo(DisplayRestockFailureReason
                    .SourceContainerTypeNotAllowed));
        }

        [Test]
        public void Restock_NoAssignment_Fails()
        {
            Context context = CreateContext(
                sourceUnits: 10,
                assignProduct: false);

            DisplayRestockResult result =
                context.Service.Restock(
                    context.Source,
                    context.Display,
                    new Quantity(1));

            Assert.That(
                result.FailureReason,
                Is.EqualTo(DisplayRestockFailureReason
                    .DisplayHasNoAssignedProduct));
        }

        [Test]
        public void Restock_SourceProductMissing_Fails()
        {
            Context context = CreateContext(sourceUnits: 0);

            DisplayRestockResult result =
                context.Service.Restock(
                    context.Source,
                    context.Display,
                    new Quantity(1));

            Assert.That(
                result.FailureReason,
                Is.EqualTo(DisplayRestockFailureReason
                    .SourceProductMissing));
        }

        [Test]
        public void Restock_InsufficientSource_FailsWithoutMutation()
        {
            Context context = CreateContext(sourceUnits: 2);

            DisplayRestockResult result =
                context.Service.Restock(
                    context.Source,
                    context.Display,
                    new Quantity(3));

            AssertFailureUnchanged(
                context,
                result,
                DisplayRestockFailureReason
                    .InsufficientSourceQuantity,
                2,
                0);
        }

        [Test]
        public void Restock_AboveDisplayCapacity_FailsWithoutMutation()
        {
            Context context = CreateContext(sourceUnits: 10);

            DisplayRestockResult result =
                context.Service.Restock(
                    context.Source,
                    context.Display,
                    new Quantity(7));

            AssertFailureUnchanged(
                context,
                result,
                DisplayRestockFailureReason
                    .DisplayCapacityExceeded,
                10,
                0);
        }

        [Test]
        public void Restock_FullDisplay_Fails()
        {
            Context context = CreateContext(sourceUnits: 10);
            context.Display.Inventory.TryAdd(
                context.ProductId,
                new Quantity(6));

            DisplayRestockResult result =
                context.Service.Restock(
                    context.Source,
                    context.Display,
                    new Quantity(1));

            Assert.That(
                result.FailureReason,
                Is.EqualTo(DisplayRestockFailureReason
                    .DisplayAlreadyFull));
        }

        [Test]
        public void RestockToCapacity_UsesMinimumOfStockAndCapacity()
        {
            Context context = CreateContext(sourceUnits: 4);

            DisplayRestockResult result =
                context.Service.RestockToCapacity(
                    context.Source,
                    context.Display);

            Assert.That(
                result.TransferredQuantity.Value,
                Is.EqualTo(4));
            Assert.That(context.Source.UsedCapacity, Is.Zero);
            Assert.That(
                context.Display.Inventory.UsedCapacity,
                Is.EqualTo(4));
        }

        [Test]
        public void RestockToCapacity_PartiallyFilledDisplay_FillsSpace()
        {
            Context context = CreateContext(sourceUnits: 10);
            context.Display.Inventory.TryAdd(
                context.ProductId,
                new Quantity(2));

            DisplayRestockResult result =
                context.Service.RestockToCapacity(
                    context.Source,
                    context.Display);

            Assert.That(
                result.TransferredQuantity.Value,
                Is.EqualTo(4));
        }

        [Test]
        public void Restock_ResultReportsVisibleUnits()
        {
            Context context = CreateContext(sourceUnits: 10);

            DisplayRestockResult result =
                context.Service.Restock(
                    context.Source,
                    context.Display,
                    new Quantity(5));

            Assert.That(result.VisibleUnitsBefore, Is.Zero);
            Assert.That(result.VisibleUnitsAfter, Is.EqualTo(3));
        }

        [Test]
        public void Restock_TransitSource_IsAllowed()
        {
            Context context = CreateContext(
                sourceUnits: 3,
                sourceType: InventoryContainerType.Transit);

            DisplayRestockResult result =
                context.Service.Restock(
                    context.Source,
                    context.Display,
                    new Quantity(2));

            Assert.That(result.Succeeded, Is.True);
        }

        private static Context CreateContext(
            int sourceUnits,
            InventoryContainerType sourceType =
                InventoryContainerType.Storage,
            bool assignProduct = true)
        {
            ProductDefinitionId productId =
                new ProductDefinitionId("game");

            ProductDefinitionRegistry registry =
                new ProductDefinitionRegistry(
                    new[]
                    {
                        new ProductDefinition(
                            productId,
                            "product.game",
                            new ProductCategoryId("video-game"),
                            Array.Empty<ProductTagId>())
                    });

            DisplayInstance display =
                new DisplayInstance(
                    new DisplayInstanceId("display"),
                    new DisplayDefinition(
                        new DisplayDefinitionId("shelf"),
                        "display.shelf",
                        new InventoryCapacity(6),
                        3,
                        new[]
                        {
                            new ProductCategoryId("video-game")
                        },
                        "placement"));

            if (assignProduct)
            {
                display.TryAssignProduct(registry, productId);
            }

            InventoryContainer source =
                new InventoryContainer(
                    new InventoryContainerId("source"),
                    sourceType,
                    new InventoryCapacity(20));

            if (sourceUnits > 0)
            {
                source.TryAdd(
                    productId,
                    new Quantity(sourceUnits));
            }

            return new Context(
                new DisplayRestockService(registry),
                source,
                display,
                productId);
        }

        private static void AssertFailureUnchanged(
            Context context,
            DisplayRestockResult result,
            DisplayRestockFailureReason expectedReason,
            int expectedSource,
            int expectedDisplay)
        {
            Assert.That(result.Succeeded, Is.False);
            Assert.That(
                result.FailureReason,
                Is.EqualTo(expectedReason));
            Assert.That(
                context.Source.GetQuantity(context.ProductId).Value,
                Is.EqualTo(expectedSource));
            Assert.That(
                context.Display.Inventory
                    .GetQuantity(context.ProductId).Value,
                Is.EqualTo(expectedDisplay));
        }

        private sealed class Context
        {
            public DisplayRestockService Service { get; }
            public InventoryContainer Source { get; }
            public DisplayInstance Display { get; }
            public ProductDefinitionId ProductId { get; }

            public Context(
                DisplayRestockService service,
                InventoryContainer source,
                DisplayInstance display,
                ProductDefinitionId productId)
            {
                Service = service;
                Source = source;
                Display = display;
                ProductId = productId;
            }
        }
    }
}
