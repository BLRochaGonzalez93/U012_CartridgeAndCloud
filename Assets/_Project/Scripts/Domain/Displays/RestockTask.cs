using System;
using VRMGames.CartridgeAndCloud.Domain.Inventory;
using VRMGames.CartridgeAndCloud.Domain.Products;

namespace VRMGames.CartridgeAndCloud.Domain.Displays
{
    public sealed class RestockTask
    {
        public RestockTaskId Id { get; }

        public InventoryContainerId SourceContainerId { get; }

        public DisplayInstanceId DisplayId { get; }

        public ProductDefinitionId ProductId { get; }

        public Quantity RequestedQuantity { get; }

        public Quantity CompletedQuantity { get; private set; }

        public RestockTaskStatus Status { get; private set; }

        public RestockTask(
            RestockTaskId id,
            InventoryContainerId sourceContainerId,
            DisplayInstanceId displayId,
            ProductDefinitionId productId,
            Quantity requestedQuantity)
        {
            if (string.IsNullOrWhiteSpace(id.Value))
            {
                throw new ArgumentException(
                    "Restock task ID must be initialized.",
                    nameof(id));
            }

            if (string.IsNullOrWhiteSpace(sourceContainerId.Value))
            {
                throw new ArgumentException(
                    "Source container ID must be initialized.",
                    nameof(sourceContainerId));
            }

            if (string.IsNullOrWhiteSpace(displayId.Value))
            {
                throw new ArgumentException(
                    "Display instance ID must be initialized.",
                    nameof(displayId));
            }

            if (string.IsNullOrWhiteSpace(productId.Value))
            {
                throw new ArgumentException(
                    "Product definition ID must be initialized.",
                    nameof(productId));
            }

            if (requestedQuantity.IsZero)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(requestedQuantity));
            }

            Id = id;
            SourceContainerId = sourceContainerId;
            DisplayId = displayId;
            ProductId = productId;
            RequestedQuantity = requestedQuantity;
            CompletedQuantity = Quantity.Zero;
            Status = RestockTaskStatus.Pending;
        }

        public RestockTaskTransitionResult TryComplete(
            Quantity completedQuantity)
        {
            if (Status != RestockTaskStatus.Pending)
            {
                return RestockTaskTransitionResult.Failure(
                    RestockTaskTransitionFailureReason.TaskNotPending,
                    Status,
                    CompletedQuantity);
            }

            if (completedQuantity.IsZero)
            {
                return RestockTaskTransitionResult.Failure(
                    RestockTaskTransitionFailureReason
                        .InvalidCompletedQuantity,
                    Status,
                    CompletedQuantity);
            }

            if (completedQuantity > RequestedQuantity)
            {
                return RestockTaskTransitionResult.Failure(
                    RestockTaskTransitionFailureReason
                        .CompletedQuantityExceedsRequested,
                    Status,
                    CompletedQuantity);
            }

            RestockTaskStatus previous = Status;
            CompletedQuantity = completedQuantity;
            Status = RestockTaskStatus.Completed;

            return RestockTaskTransitionResult.Success(
                previous,
                Status,
                CompletedQuantity);
        }

        public RestockTaskTransitionResult TryCancel()
        {
            if (Status != RestockTaskStatus.Pending)
            {
                return RestockTaskTransitionResult.Failure(
                    RestockTaskTransitionFailureReason.TaskNotPending,
                    Status,
                    CompletedQuantity);
            }

            RestockTaskStatus previous = Status;
            Status = RestockTaskStatus.Cancelled;

            return RestockTaskTransitionResult.Success(
                previous,
                Status,
                CompletedQuantity);
        }
    }
}
