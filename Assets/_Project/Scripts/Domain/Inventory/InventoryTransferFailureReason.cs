namespace VRMGames.CartridgeAndCloud.Domain.Inventory
{
    public enum InventoryTransferFailureReason
    {
        None = 0,
        InvalidQuantity = 1,
        SameContainer = 2,
        ProductDefinitionMissing = 3,
        SourceProductMissing = 4,
        InsufficientSourceQuantity = 5,
        DestinationCapacityExceeded = 6
    }
}
