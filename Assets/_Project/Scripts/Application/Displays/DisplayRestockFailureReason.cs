namespace VRMGames.CartridgeAndCloud.Application.Displays
{
    public enum DisplayRestockFailureReason
    {
        None = 0,
        InvalidQuantity = 1,
        SourceContainerTypeNotAllowed = 2,
        DisplayHasNoAssignedProduct = 3,
        ProductDefinitionMissing = 4,
        SourceProductMissing = 5,
        InsufficientSourceQuantity = 6,
        DisplayAlreadyFull = 7,
        DisplayCapacityExceeded = 8,
        TransferRejected = 9
    }
}
