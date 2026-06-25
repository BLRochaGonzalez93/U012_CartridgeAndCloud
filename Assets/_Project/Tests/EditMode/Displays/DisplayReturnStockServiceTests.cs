using System;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.Displays;
using VRMGames.CartridgeAndCloud.Domain.Displays;
using VRMGames.CartridgeAndCloud.Domain.Inventory;
using VRMGames.CartridgeAndCloud.Domain.Products;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Displays
{
    public sealed class DisplayReturnStockServiceTests
    {
        [Test]
        public void Return_ValidQuantity_MovesStockToStorage()
        {
            Context context = CreateContext(5);

            DisplayReturnStockResult result =
                context.Service.Return(
                    context.Display,
                    context.Destination,
                    new Quantity(2));

            Assert.That(result.Succeeded, Is.True);
            Assert.That(
                context.Display.Inventory.UsedCapacity,
                Is.EqualTo(3));
            Assert.That(
                context.Destination.UsedCapacity,
                Is.EqualTo(2));
        }

        [Test]
        public void Return_ZeroQuantity_FailsWithoutMutation()
        {
            Context context = CreateContext(5);

            DisplayReturnStockResult result =
                context.Service.Return(
                    context.Display,
                    context.Destination,
                    Quantity.Zero);

            Assert.That(
                result.FailureReason,
                Is.EqualTo(DisplayReturnStockFailureReason
                    .InvalidQuantity));
            Assert.That(
                context.Display.Inventory.UsedCapacity,
                Is.EqualTo(5));
            Assert.That(context.Destination.UsedCapacity, Is.Zero);
        }

        [Test]
        public void Return_GenericDestination_Fails()
        {
            Context context = CreateContext(
                5,
                InventoryContainerType.Generic);

            DisplayReturnStockResult result =
                context.Service.Return(
                    context.Display,
                    context.Destination,
                    new Quantity(1));

            Assert.That(
                result.FailureReason,
                Is.EqualTo(DisplayReturnStockFailureReason
                    .DestinationContainerTypeNotAllowed));
        }

        [Test]
        public void Return_QuantityAboveDisplayStock_Fails()
        {
            Context context = CreateContext(2);

            DisplayReturnStockResult result =
                context.Service.Return(
                    context.Display,
                    context.Destination,
                    new Quantity(3));

            Assert.That(
                result.FailureReason,
                Is.EqualTo(DisplayReturnStockFailureReason
                    .InsufficientDisplayQuantity));
        }

        [Test]
        public void Return_DestinationWithoutCapacity_Fails()
        {
            Context context =
                CreateContext(5, destinationCapacity: 1);

            context.Destination.TryAdd(
                context.ProductId,
                new Quantity(1));

            DisplayReturnStockResult result =
                context.Service.Return(
                    context.Display,
                    context.Destination,
                    new Quantity(1));

            Assert.That(
                result.FailureReason,
                Is.EqualTo(DisplayReturnStockFailureReason
                    .DestinationCapacityExceeded));
        }

        [Test]
        public void ReturnAllAndClear_EmptiesAndClearsAssignment()
        {
            Context context = CreateContext(5);

            DisplayReturnStockResult result =
                context.Service.ReturnAllAndClear(
                    context.Display,
                    context.Destination);

            Assert.That(result.Succeeded, Is.True);
            Assert.That(result.AssignmentCleared, Is.True);
            Assert.That(context.Display.IsEmpty, Is.True);
            Assert.That(
                context.Display.HasAssignedProduct,
                Is.False);
            Assert.That(
                context.Destination.UsedCapacity,
                Is.EqualTo(5));
        }

        [Test]
        public void ReturnAllAndClear_EmptyDisplay_ClearsAssignment()
        {
            Context context = CreateContext(0);

            DisplayReturnStockResult result =
                context.Service.ReturnAllAndClear(
                    context.Display,
                    context.Destination);

            Assert.That(result.Succeeded, Is.True);
            Assert.That(result.ReturnedQuantity, Is.EqualTo(Quantity.Zero));
            Assert.That(result.AssignmentCleared, Is.True);
            Assert.That(
                context.Display.HasAssignedProduct,
                Is.False);
            Assert.That(context.Destination.UsedCapacity, Is.Zero);
        }

        [Test]
        public void Return_PreservesCombinedUnits()
        {
            Context context = CreateContext(5);
            int before =
                context.Display.Inventory.UsedCapacity +
                context.Destination.UsedCapacity;

            context.Service.Return(
                context.Display,
                context.Destination,
                new Quantity(3));

            int after =
                context.Display.Inventory.UsedCapacity +
                context.Destination.UsedCapacity;

            Assert.That(after, Is.EqualTo(before));
        }

        private static Context CreateContext(
            int displayUnits,
            InventoryContainerType destinationType =
                InventoryContainerType.Storage,
            int destinationCapacity = 20)
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
                        new InventoryCapacity(10),
                        5,
                        Array.Empty<ProductCategoryId>(),
                        "placement"));

            display.TryAssignProduct(registry, productId);

            if (displayUnits > 0)
            {
                display.Inventory.TryAdd(
                    productId,
                    new Quantity(displayUnits));
            }

            InventoryContainer destination =
                new InventoryContainer(
                    new InventoryContainerId("destination"),
                    destinationType,
                    new InventoryCapacity(destinationCapacity));

            return new Context(
                new DisplayReturnStockService(registry),
                display,
                destination,
                productId);
        }

        private sealed class Context
        {
            public DisplayReturnStockService Service { get; }
            public DisplayInstance Display { get; }
            public InventoryContainer Destination { get; }
            public ProductDefinitionId ProductId { get; }

            public Context(
                DisplayReturnStockService service,
                DisplayInstance display,
                InventoryContainer destination,
                ProductDefinitionId productId)
            {
                Service = service;
                Display = display;
                Destination = destination;
                ProductId = productId;
            }
        }
    }
}
