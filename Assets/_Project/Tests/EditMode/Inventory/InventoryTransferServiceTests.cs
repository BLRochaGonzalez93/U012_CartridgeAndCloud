using System;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.Inventory;
using VRMGames.CartridgeAndCloud.Domain.Products;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Inventory
{
    public sealed class InventoryTransferServiceTests
    {
        private static readonly ProductDefinitionId ProductA =
            new ProductDefinitionId("product-a");

        private static readonly ProductDefinitionId ProductB =
            new ProductDefinitionId("product-b");

        [Test]
        public void Transfer_ValidQuantity_MovesAndConservesUnits()
        {
            InventoryContainer source = CreateContainer(
                "source",
                10,
                ProductA,
                6);

            InventoryContainer destination = CreateContainer(
                "destination",
                10);

            InventoryTransferResult result =
                CreateService(ProductA).Transfer(
                    source,
                    destination,
                    ProductA,
                    new Quantity(4));

            Assert.That(result.Succeeded, Is.True);
            Assert.That(source.GetQuantity(ProductA).Value, Is.EqualTo(2));
            Assert.That(destination.GetQuantity(ProductA).Value, Is.EqualTo(4));
            Assert.That(result.TotalUnitsBefore, Is.EqualTo(6));
            Assert.That(result.TotalUnitsAfter, Is.EqualTo(6));
        }

        [Test]
        public void Transfer_DestinationHasProduct_MergesDestinationStack()
        {
            InventoryContainer source = CreateContainer(
                "source",
                10,
                ProductA,
                5);

            InventoryContainer destination = CreateContainer(
                "destination",
                10,
                ProductA,
                2);

            InventoryTransferResult result =
                CreateService(ProductA).Transfer(
                    source,
                    destination,
                    ProductA,
                    new Quantity(3));

            Assert.That(result.Succeeded, Is.True);
            Assert.That(destination.GetQuantity(ProductA).Value, Is.EqualTo(5));
            Assert.That(destination.StackCount, Is.EqualTo(1));
        }

        [Test]
        public void Transfer_WholeSourceQuantity_RemovesSourceStack()
        {
            InventoryContainer source = CreateContainer(
                "source",
                10,
                ProductA,
                5);

            InventoryContainer destination = CreateContainer(
                "destination",
                10);

            InventoryTransferResult result =
                CreateService(ProductA).Transfer(
                    source,
                    destination,
                    ProductA,
                    new Quantity(5));

            Assert.That(result.Succeeded, Is.True);
            Assert.That(source.ContainsProduct(ProductA), Is.False);
            Assert.That(source.StackCount, Is.Zero);
        }

        [Test]
        public void Transfer_ZeroQuantity_FailsAtomically()
        {
            InventoryContainer source = CreateContainer(
                "source",
                10,
                ProductA,
                5);

            InventoryContainer destination = CreateContainer(
                "destination",
                10);

            InventoryTransferResult result =
                CreateService(ProductA).Transfer(
                    source,
                    destination,
                    ProductA,
                    Quantity.Zero);

            AssertFailureAndState(
                result,
                InventoryTransferFailureReason.InvalidQuantity,
                source,
                destination,
                sourceQuantity: 5,
                destinationQuantity: 0);
        }

        [Test]
        public void Transfer_UndefinedProduct_FailsAtomically()
        {
            InventoryContainer source = CreateContainer(
                "source",
                10,
                ProductB,
                5);

            InventoryContainer destination = CreateContainer(
                "destination",
                10);

            InventoryTransferResult result =
                CreateService(ProductA).Transfer(
                    source,
                    destination,
                    ProductB,
                    new Quantity(1));

            AssertFailureAndState(
                result,
                InventoryTransferFailureReason.ProductDefinitionMissing,
                source,
                destination,
                sourceQuantity: 5,
                destinationQuantity: 0,
                productId: ProductB);
        }

        [Test]
        public void Transfer_SourceMissingProduct_FailsAtomically()
        {
            InventoryContainer source = CreateContainer("source", 10);
            InventoryContainer destination = CreateContainer("destination", 10);

            InventoryTransferResult result =
                CreateService(ProductA).Transfer(
                    source,
                    destination,
                    ProductA,
                    new Quantity(1));

            AssertFailureAndState(
                result,
                InventoryTransferFailureReason.SourceProductMissing,
                source,
                destination,
                sourceQuantity: 0,
                destinationQuantity: 0);
        }

        [Test]
        public void Transfer_InsufficientSourceQuantity_FailsAtomically()
        {
            InventoryContainer source = CreateContainer(
                "source",
                10,
                ProductA,
                2);

            InventoryContainer destination = CreateContainer(
                "destination",
                10);

            InventoryTransferResult result =
                CreateService(ProductA).Transfer(
                    source,
                    destination,
                    ProductA,
                    new Quantity(3));

            AssertFailureAndState(
                result,
                InventoryTransferFailureReason.InsufficientSourceQuantity,
                source,
                destination,
                sourceQuantity: 2,
                destinationQuantity: 0);
        }

        [Test]
        public void Transfer_DestinationCapacityExceeded_FailsAtomically()
        {
            InventoryContainer source = CreateContainer(
                "source",
                10,
                ProductA,
                5);

            InventoryContainer destination = CreateContainer(
                "destination",
                3,
                ProductB,
                2);

            InventoryTransferResult result =
                CreateService(ProductA, ProductB).Transfer(
                    source,
                    destination,
                    ProductA,
                    new Quantity(2));

            AssertFailureAndState(
                result,
                InventoryTransferFailureReason.DestinationCapacityExceeded,
                source,
                destination,
                sourceQuantity: 5,
                destinationQuantity: 0);

            Assert.That(destination.GetQuantity(ProductB).Value, Is.EqualTo(2));
        }

        [Test]
        public void Transfer_SameReference_FailsAtomically()
        {
            InventoryContainer container = CreateContainer(
                "container",
                10,
                ProductA,
                5);

            InventoryTransferResult result =
                CreateService(ProductA).Transfer(
                    container,
                    container,
                    ProductA,
                    new Quantity(1));

            Assert.That(result.Succeeded, Is.False);
            Assert.That(
                result.FailureReason,
                Is.EqualTo(InventoryTransferFailureReason.SameContainer));

            Assert.That(container.GetQuantity(ProductA).Value, Is.EqualTo(5));
            Assert.That(container.UsedCapacity, Is.EqualTo(5));
        }

        [Test]
        public void Transfer_SameLogicalId_FailsAtomically()
        {
            InventoryContainer source = CreateContainer(
                "same-id",
                10,
                ProductA,
                5);

            InventoryContainer destination = CreateContainer(
                "same-id",
                10);

            InventoryTransferResult result =
                CreateService(ProductA).Transfer(
                    source,
                    destination,
                    ProductA,
                    new Quantity(1));

            AssertFailureAndState(
                result,
                InventoryTransferFailureReason.SameContainer,
                source,
                destination,
                sourceQuantity: 5,
                destinationQuantity: 0);
        }

        [Test]
        public void Transfer_NullSource_ThrowsArgumentNullException()
        {
            InventoryContainer destination = CreateContainer("destination", 10);

            Assert.Throws<ArgumentNullException>(
                () => CreateService(ProductA).Transfer(
                    null,
                    destination,
                    ProductA,
                    new Quantity(1)));
        }

        [Test]
        public void Transfer_NullDestination_ThrowsArgumentNullException()
        {
            InventoryContainer source = CreateContainer("source", 10);

            Assert.Throws<ArgumentNullException>(
                () => CreateService(ProductA).Transfer(
                    source,
                    null,
                    ProductA,
                    new Quantity(1)));
        }

        [Test]
        public void Transfer_DefaultProductId_ThrowsArgumentException()
        {
            InventoryContainer source = CreateContainer("source", 10);
            InventoryContainer destination = CreateContainer("destination", 10);

            Assert.Throws<ArgumentException>(
                () => CreateService(ProductA).Transfer(
                    source,
                    destination,
                    default(ProductDefinitionId),
                    new Quantity(1)));
        }

        [Test]
        public void Transfer_ValidQuantity_ReportsBeforeAndAfterState()
        {
            InventoryContainer source = CreateContainer(
                "source",
                10,
                ProductA,
                6);

            InventoryContainer destination = CreateContainer(
                "destination",
                10,
                ProductA,
                1);

            InventoryTransferResult result =
                CreateService(ProductA).Transfer(
                    source,
                    destination,
                    ProductA,
                    new Quantity(2));

            Assert.That(result.SourceQuantityBefore.Value, Is.EqualTo(6));
            Assert.That(result.SourceQuantityAfter.Value, Is.EqualTo(4));
            Assert.That(result.DestinationQuantityBefore.Value, Is.EqualTo(1));
            Assert.That(result.DestinationQuantityAfter.Value, Is.EqualTo(3));
        }

        [Test]
        public void Transfer_SequentialTransfers_PreserveCombinedUnits()
        {
            InventoryContainer first = CreateContainer(
                "first",
                20,
                ProductA,
                10);

            InventoryContainer second = CreateContainer("second", 20);
            InventoryContainer third = CreateContainer("third", 20);
            InventoryTransferService service = CreateService(ProductA);

            InventoryTransferResult firstResult =
                service.Transfer(
                    first,
                    second,
                    ProductA,
                    new Quantity(6));

            InventoryTransferResult secondResult =
                service.Transfer(
                    second,
                    third,
                    ProductA,
                    new Quantity(4));

            int total =
                first.UsedCapacity +
                second.UsedCapacity +
                third.UsedCapacity;

            Assert.That(firstResult.Succeeded, Is.True);
            Assert.That(secondResult.Succeeded, Is.True);
            Assert.That(total, Is.EqualTo(10));
        }

        private static void AssertFailureAndState(
            InventoryTransferResult result,
            InventoryTransferFailureReason expectedReason,
            InventoryContainer source,
            InventoryContainer destination,
            int sourceQuantity,
            int destinationQuantity,
            ProductDefinitionId? productId = null)
        {
            ProductDefinitionId actualProductId =
                productId ?? ProductA;

            Assert.That(result.Succeeded, Is.False);
            Assert.That(result.FailureReason, Is.EqualTo(expectedReason));
            Assert.That(
                source.GetQuantity(actualProductId).Value,
                Is.EqualTo(sourceQuantity));

            Assert.That(
                destination.GetQuantity(actualProductId).Value,
                Is.EqualTo(destinationQuantity));

            Assert.That(result.TotalUnitsAfter, Is.EqualTo(result.TotalUnitsBefore));
        }

        private static InventoryTransferService CreateService(
            params ProductDefinitionId[] productIds)
        {
            ProductDefinition[] definitions =
                new ProductDefinition[productIds.Length];

            for (int index = 0; index < productIds.Length; index++)
            {
                ProductDefinitionId productId = productIds[index];

                definitions[index] =
                    new ProductDefinition(
                        productId,
                        $"products.{productId.Value}.name",
                        new ProductCategoryId("cartridge"),
                        Array.Empty<ProductTagId>());
            }

            return new InventoryTransferService(
                new ProductDefinitionRegistry(definitions));
        }

        private static InventoryContainer CreateContainer(
            string id,
            int capacity)
        {
            return new InventoryContainer(
                new InventoryContainerId(id),
                InventoryContainerType.Storage,
                new InventoryCapacity(capacity));
        }

        private static InventoryContainer CreateContainer(
            string id,
            int capacity,
            ProductDefinitionId productId,
            int quantity)
        {
            return new InventoryContainer(
                new InventoryContainerId(id),
                InventoryContainerType.Storage,
                new InventoryCapacity(capacity),
                new[]
                {
                    new InventoryStack(
                        productId,
                        new Quantity(quantity))
                });
        }
    }
}
