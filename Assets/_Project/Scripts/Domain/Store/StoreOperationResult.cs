namespace VRMGames.CartridgeAndCloud.Domain.Store
{
    public sealed class StoreOperationResult
    {
        public StoreOperationStatus Status { get; }
        public string Detail { get; }

        public bool Succeeded =>
            Status == StoreOperationStatus.Success;

        public StoreOperationResult(
            StoreOperationStatus status,
            string detail)
        {
            Status = status;
            Detail = detail ?? string.Empty;
        }

        public static StoreOperationResult Success(
            string detail)
        {
            return new StoreOperationResult(
                StoreOperationStatus.Success,
                detail);
        }

        public static StoreOperationResult Failure(
            StoreOperationStatus status,
            string detail)
        {
            return new StoreOperationResult(
                status,
                detail);
        }
    }

    public enum StoreOperationStatus
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
}
