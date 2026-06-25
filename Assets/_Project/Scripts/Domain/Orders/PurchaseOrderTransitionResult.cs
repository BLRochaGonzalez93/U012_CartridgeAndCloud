namespace VRMGames.CartridgeAndCloud.Domain.Orders
{
    public readonly struct PurchaseOrderTransitionResult
    {
        public bool Succeeded { get; }

        public PurchaseOrderTransitionFailureReason FailureReason
        {
            get;
        }

        public PurchaseOrderStatus PreviousStatus { get; }

        public PurchaseOrderStatus CurrentStatus { get; }

        private PurchaseOrderTransitionResult(
            bool succeeded,
            PurchaseOrderTransitionFailureReason failureReason,
            PurchaseOrderStatus previousStatus,
            PurchaseOrderStatus currentStatus)
        {
            Succeeded = succeeded;
            FailureReason = failureReason;
            PreviousStatus = previousStatus;
            CurrentStatus = currentStatus;
        }

        public static PurchaseOrderTransitionResult Success(
            PurchaseOrderStatus previousStatus,
            PurchaseOrderStatus currentStatus)
        {
            return new PurchaseOrderTransitionResult(
                true,
                PurchaseOrderTransitionFailureReason.None,
                previousStatus,
                currentStatus);
        }

        public static PurchaseOrderTransitionResult Failure(
            PurchaseOrderStatus currentStatus)
        {
            return new PurchaseOrderTransitionResult(
                false,
                PurchaseOrderTransitionFailureReason.InvalidCurrentStatus,
                currentStatus,
                currentStatus);
        }
    }
}
