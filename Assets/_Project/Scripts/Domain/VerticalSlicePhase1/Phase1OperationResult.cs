namespace VRMGames.CartridgeAndCloud.Domain.VerticalSlicePhase1
{
    public enum Phase1OperationStatus
    {
        Success = 0,
        NotFound = 1,
        InvalidState = 2,
        InsufficientCash = 3,
        InsufficientStock = 4,
        CapacityExceeded = 5,
        PlacementRequired = 6,
        CheckoutRequired = 7,
        StoreMustBeOpen = 8,
        StoreMustBeClosed = 9,
        Duplicate = 10,
        PersistenceFailure = 11
    }

    public sealed class Phase1OperationResult
    {
        public Phase1OperationStatus Status { get; }
        public string Detail { get; }

        public bool Succeeded =>
            Status == Phase1OperationStatus.Success;

        public Phase1OperationResult(
            Phase1OperationStatus status,
            string detail)
        {
            Status = status;
            Detail = detail ?? string.Empty;
        }

        public static Phase1OperationResult Success(
            string detail)
        {
            return new Phase1OperationResult(
                Phase1OperationStatus.Success,
                detail);
        }

        public static Phase1OperationResult Failure(
            Phase1OperationStatus status,
            string detail)
        {
            return new Phase1OperationResult(
                status,
                detail);
        }
    }
}
