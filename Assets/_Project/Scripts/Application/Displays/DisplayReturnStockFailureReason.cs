namespace VRMGames.CartridgeAndCloud.Application.Displays
{
    public enum DisplayReturnStockFailureReason
    {
        None = 0,
        InvalidQuantity = 1,
        DestinationContainerTypeNotAllowed = 2,
        DisplayHasNoAssignedProduct = 3,
        DisplayStockMissing = 4,
        InsufficientDisplayQuantity = 5,
        DestinationCapacityExceeded = 6,
        TransferRejected = 7,
        ClearAssignmentRejected = 8
    }
}
