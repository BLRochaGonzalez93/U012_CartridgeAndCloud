namespace VRMGames.CartridgeAndCloud.Domain.Receiving
{
    public readonly struct DeliveryMutationResult
    {
        public bool Succeeded { get; }

        public DeliveryMutationFailureReason FailureReason { get; }

        public DeliveryStatus PreviousStatus { get; }

        public DeliveryStatus CurrentStatus { get; }

        private DeliveryMutationResult(
            bool succeeded,
            DeliveryMutationFailureReason failureReason,
            DeliveryStatus previousStatus,
            DeliveryStatus currentStatus)
        {
            Succeeded = succeeded;
            FailureReason = failureReason;
            PreviousStatus = previousStatus;
            CurrentStatus = currentStatus;
        }

        public static DeliveryMutationResult Success(
            DeliveryStatus previousStatus,
            DeliveryStatus currentStatus)
        {
            return new DeliveryMutationResult(
                true,
                DeliveryMutationFailureReason.None,
                previousStatus,
                currentStatus);
        }

        public static DeliveryMutationResult Failure(
            DeliveryMutationFailureReason failureReason,
            DeliveryStatus currentStatus)
        {
            return new DeliveryMutationResult(
                false,
                failureReason,
                currentStatus,
                currentStatus);
        }
    }
}
