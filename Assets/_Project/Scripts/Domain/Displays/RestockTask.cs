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

    public readonly struct RestockTaskId : IEquatable<RestockTaskId>
    {
        public string Value { get; }

        public RestockTaskId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "Restock task ID cannot be empty.",
                    nameof(value));
            }

            Value = value;
        }

        public bool Equals(RestockTaskId other)
        {
            return string.Equals(Value, other.Value, StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is RestockTaskId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value == null
                ? 0
                : StringComparer.Ordinal.GetHashCode(Value);
        }

        public override string ToString()
        {
            return Value ?? string.Empty;
        }

        public static bool operator ==(RestockTaskId left, RestockTaskId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RestockTaskId left, RestockTaskId right)
        {
            return !left.Equals(right);
        }
    }

    public enum RestockTaskStatus
    {
        Pending = 0,
        Completed = 1,
        Cancelled = 2
    }

    public enum RestockTaskTransitionFailureReason
    {
        None = 0,
        TaskNotPending = 1,
        InvalidCompletedQuantity = 2,
        CompletedQuantityExceedsRequested = 3
    }

    public sealed class RestockTaskTransitionResult
    {
        public bool Succeeded { get; }

        public RestockTaskTransitionFailureReason FailureReason { get; }

        public RestockTaskStatus PreviousStatus { get; }

        public RestockTaskStatus CurrentStatus { get; }

        public Quantity CompletedQuantity { get; }

        private RestockTaskTransitionResult(
            bool succeeded,
            RestockTaskTransitionFailureReason failureReason,
            RestockTaskStatus previousStatus,
            RestockTaskStatus currentStatus,
            Quantity completedQuantity)
        {
            Succeeded = succeeded;
            FailureReason = failureReason;
            PreviousStatus = previousStatus;
            CurrentStatus = currentStatus;
            CompletedQuantity = completedQuantity;
        }

        public static RestockTaskTransitionResult Success(
            RestockTaskStatus previousStatus,
            RestockTaskStatus currentStatus,
            Quantity completedQuantity)
        {
            return new RestockTaskTransitionResult(
                true,
                RestockTaskTransitionFailureReason.None,
                previousStatus,
                currentStatus,
                completedQuantity);
        }

        public static RestockTaskTransitionResult Failure(
            RestockTaskTransitionFailureReason failureReason,
            RestockTaskStatus status,
            Quantity completedQuantity)
        {
            return new RestockTaskTransitionResult(
                false,
                failureReason,
                status,
                status,
                completedQuantity);
        }
    }
}
