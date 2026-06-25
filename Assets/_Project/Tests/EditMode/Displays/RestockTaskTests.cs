using System;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.Displays;
using VRMGames.CartridgeAndCloud.Domain.Inventory;
using VRMGames.CartridgeAndCloud.Domain.Products;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Displays
{
    public sealed class RestockTaskTests
    {
        [Test]
        public void Constructor_ZeroRequestedQuantity_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => CreateTask(Quantity.Zero));
        }

        [Test]
        public void NewTask_StartsPendingWithZeroCompletedQuantity()
        {
            RestockTask task = CreateTask(new Quantity(4));

            Assert.That(
                task.Status,
                Is.EqualTo(RestockTaskStatus.Pending));
            Assert.That(
                task.CompletedQuantity,
                Is.EqualTo(Quantity.Zero));
        }

        [Test]
        public void Complete_ValidQuantity_Succeeds()
        {
            RestockTask task = CreateTask(new Quantity(4));

            RestockTaskTransitionResult result =
                task.TryComplete(new Quantity(3));

            Assert.That(result.Succeeded, Is.True);
            Assert.That(
                task.Status,
                Is.EqualTo(RestockTaskStatus.Completed));
            Assert.That(
                task.CompletedQuantity.Value,
                Is.EqualTo(3));
        }

        [Test]
        public void Complete_AboveRequestedQuantity_Fails()
        {
            RestockTask task = CreateTask(new Quantity(4));

            RestockTaskTransitionResult result =
                task.TryComplete(new Quantity(5));

            Assert.That(
                result.FailureReason,
                Is.EqualTo(RestockTaskTransitionFailureReason
                    .CompletedQuantityExceedsRequested));
        }

        [Test]
        public void Cancel_PendingTask_Succeeds()
        {
            RestockTask task = CreateTask(new Quantity(4));

            RestockTaskTransitionResult result = task.TryCancel();

            Assert.That(result.Succeeded, Is.True);
            Assert.That(
                task.Status,
                Is.EqualTo(RestockTaskStatus.Cancelled));
        }

        [Test]
        public void Complete_CancelledTask_FailsWithoutChangingStatus()
        {
            RestockTask task = CreateTask(new Quantity(4));
            task.TryCancel();

            RestockTaskTransitionResult result =
                task.TryComplete(new Quantity(2));

            Assert.That(
                result.FailureReason,
                Is.EqualTo(RestockTaskTransitionFailureReason
                    .TaskNotPending));
            Assert.That(
                task.Status,
                Is.EqualTo(RestockTaskStatus.Cancelled));
        }

        private static RestockTask CreateTask(Quantity requested)
        {
            return new RestockTask(
                new RestockTaskId("task"),
                new InventoryContainerId("storage"),
                new DisplayInstanceId("display"),
                new ProductDefinitionId("game"),
                requested);
        }
    }
}
