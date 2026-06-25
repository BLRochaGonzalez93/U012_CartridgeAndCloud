using System;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.Inventory;
using VRMGames.CartridgeAndCloud.Domain.Products;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Inventory
{
    public sealed class InventoryContainerTests
    {
        private static readonly ProductDefinitionId ProductA =
            new ProductDefinitionId("product-a");

        private static readonly ProductDefinitionId ProductB =
            new ProductDefinitionId("product-b");

        [Test]
        public void Constructor_EmptyContainer_HasFullAvailableCapacity()
        {
            InventoryContainer container = CreateContainer(10);

            Assert.That(container.UsedCapacity, Is.Zero);
            Assert.That(container.AvailableCapacity, Is.EqualTo(10));
            Assert.That(container.StackCount, Is.Zero);
        }

        [Test]
        public void Constructor_InitialStacks_ComputesState()
        {
            InventoryContainer container =
                new InventoryContainer(
                    new InventoryContainerId("container-a"),
                    InventoryContainerType.Storage,
                    new InventoryCapacity(10),
                    new[]
                    {
                        new InventoryStack(ProductA, new Quantity(3)),
                        new InventoryStack(ProductB, new Quantity(2))
                    });

            Assert.That(container.UsedCapacity, Is.EqualTo(5));
            Assert.That(container.AvailableCapacity, Is.EqualTo(5));
            Assert.That(container.StackCount, Is.EqualTo(2));
        }

        [Test]
        public void Constructor_DuplicateInitialProduct_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(
                () => new InventoryContainer(
                    new InventoryContainerId("container-a"),
                    InventoryContainerType.Storage,
                    new InventoryCapacity(10),
                    new[]
                    {
                        new InventoryStack(ProductA, new Quantity(3)),
                        new InventoryStack(ProductA, new Quantity(2))
                    }));
        }

        [Test]
        public void Constructor_InitialStacksOverCapacity_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(
                () => new InventoryContainer(
                    new InventoryContainerId("container-a"),
                    InventoryContainerType.Storage,
                    new InventoryCapacity(4),
                    new[]
                    {
                        new InventoryStack(ProductA, new Quantity(5))
                    }));
        }

        [Test]
        public void Constructor_DefaultInitialStack_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(
                () => new InventoryContainer(
                    new InventoryContainerId("container-a"),
                    InventoryContainerType.Storage,
                    new InventoryCapacity(10),
                    new[] { default(InventoryStack) }));
        }

        [Test]
        public void Constructor_UnspecifiedType_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new InventoryContainer(
                    new InventoryContainerId("container-a"),
                    InventoryContainerType.Unspecified,
                    new InventoryCapacity(10)));
        }

        [Test]
        public void TryAdd_NewProduct_CreatesStack()
        {
            InventoryContainer container = CreateContainer(10);

            InventoryMutationResult result =
                container.TryAdd(ProductA, new Quantity(3));

            Assert.That(result.Succeeded, Is.True);
            Assert.That(container.GetQuantity(ProductA).Value, Is.EqualTo(3));
            Assert.That(container.StackCount, Is.EqualTo(1));
            Assert.That(container.UsedCapacity, Is.EqualTo(3));
        }

        [Test]
        public void TryAdd_ExistingProduct_MergesStack()
        {
            InventoryContainer container =
                CreateContainerWith(ProductA, 2, 10);

            InventoryMutationResult result =
                container.TryAdd(ProductA, new Quantity(3));

            Assert.That(result.Succeeded, Is.True);
            Assert.That(container.GetQuantity(ProductA).Value, Is.EqualTo(5));
            Assert.That(container.StackCount, Is.EqualTo(1));
        }

        [Test]
        public void TryAdd_ZeroQuantity_FailsWithoutMutation()
        {
            InventoryContainer container =
                CreateContainerWith(ProductA, 2, 10);

            InventoryMutationResult result =
                container.TryAdd(ProductA, Quantity.Zero);

            Assert.That(result.Succeeded, Is.False);
            Assert.That(
                result.FailureReason,
                Is.EqualTo(
                    InventoryMutationFailureReason.InvalidQuantity));

            Assert.That(container.GetQuantity(ProductA).Value, Is.EqualTo(2));
            Assert.That(container.UsedCapacity, Is.EqualTo(2));
        }

        [Test]
        public void TryAdd_OverCapacity_FailsWithoutMutation()
        {
            InventoryContainer container =
                CreateContainerWith(ProductA, 4, 5);

            InventoryMutationResult result =
                container.TryAdd(ProductB, new Quantity(2));

            Assert.That(result.Succeeded, Is.False);
            Assert.That(
                result.FailureReason,
                Is.EqualTo(
                    InventoryMutationFailureReason.CapacityExceeded));

            Assert.That(container.ContainsProduct(ProductB), Is.False);
            Assert.That(container.UsedCapacity, Is.EqualTo(4));
        }

        [Test]
        public void TryRemove_PartialQuantity_LeavesRemainingStack()
        {
            InventoryContainer container =
                CreateContainerWith(ProductA, 5, 10);

            InventoryMutationResult result =
                container.TryRemove(ProductA, new Quantity(2));

            Assert.That(result.Succeeded, Is.True);
            Assert.That(container.GetQuantity(ProductA).Value, Is.EqualTo(3));
            Assert.That(container.StackCount, Is.EqualTo(1));
            Assert.That(container.UsedCapacity, Is.EqualTo(3));
        }

        [Test]
        public void TryRemove_WholeQuantity_RemovesStack()
        {
            InventoryContainer container =
                CreateContainerWith(ProductA, 5, 10);

            InventoryMutationResult result =
                container.TryRemove(ProductA, new Quantity(5));

            Assert.That(result.Succeeded, Is.True);
            Assert.That(container.ContainsProduct(ProductA), Is.False);
            Assert.That(container.StackCount, Is.Zero);
            Assert.That(container.UsedCapacity, Is.Zero);
        }

        [Test]
        public void TryRemove_MissingProduct_FailsWithoutMutation()
        {
            InventoryContainer container = CreateContainer(10);

            InventoryMutationResult result =
                container.TryRemove(ProductA, new Quantity(1));

            Assert.That(result.Succeeded, Is.False);
            Assert.That(
                result.FailureReason,
                Is.EqualTo(
                    InventoryMutationFailureReason.InsufficientQuantity));

            Assert.That(container.UsedCapacity, Is.Zero);
        }

        [Test]
        public void TryRemove_InsufficientQuantity_FailsWithoutMutation()
        {
            InventoryContainer container =
                CreateContainerWith(ProductA, 2, 10);

            InventoryMutationResult result =
                container.TryRemove(ProductA, new Quantity(3));

            Assert.That(result.Succeeded, Is.False);
            Assert.That(container.GetQuantity(ProductA).Value, Is.EqualTo(2));
            Assert.That(container.UsedCapacity, Is.EqualTo(2));
        }

        [Test]
        public void Stacks_UnorderedInsertion_ReturnsOrdinalSnapshot()
        {
            InventoryContainer container = CreateContainer(10);
            container.TryAdd(ProductB, new Quantity(1));
            container.TryAdd(ProductA, new Quantity(1));

            Assert.That(container.Stacks.Count, Is.EqualTo(2));
            Assert.That(
                container.Stacks[0].ProductId,
                Is.EqualTo(ProductA));

            Assert.That(
                container.Stacks[1].ProductId,
                Is.EqualTo(ProductB));
        }

        [Test]
        public void Stacks_PreviousSnapshot_DoesNotChangeAfterMutation()
        {
            InventoryContainer container =
                CreateContainerWith(ProductA, 2, 10);

            System.Collections.Generic.IReadOnlyList<InventoryStack> snapshot =
                container.Stacks;

            container.TryAdd(ProductA, new Quantity(3));

            Assert.That(snapshot[0].Quantity.Value, Is.EqualTo(2));
            Assert.That(container.Stacks[0].Quantity.Value, Is.EqualTo(5));
        }

        [Test]
        public void ZeroCapacity_TryAdd_FailsWithoutStack()
        {
            InventoryContainer container = CreateContainer(0);

            InventoryMutationResult result =
                container.TryAdd(ProductA, new Quantity(1));

            Assert.That(result.Succeeded, Is.False);
            Assert.That(container.StackCount, Is.Zero);
            Assert.That(container.UsedCapacity, Is.Zero);
        }

        private static InventoryContainer CreateContainer(int capacity)
        {
            return new InventoryContainer(
                new InventoryContainerId("container-a"),
                InventoryContainerType.Storage,
                new InventoryCapacity(capacity));
        }

        private static InventoryContainer CreateContainerWith(
            ProductDefinitionId productId,
            int quantity,
            int capacity)
        {
            return new InventoryContainer(
                new InventoryContainerId("container-a"),
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
