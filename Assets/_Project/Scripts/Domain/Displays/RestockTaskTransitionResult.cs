using VRMGames.CartridgeAndCloud.Domain.Inventory;

namespace VRMGames.CartridgeAndCloud.Domain.Displays
{
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
