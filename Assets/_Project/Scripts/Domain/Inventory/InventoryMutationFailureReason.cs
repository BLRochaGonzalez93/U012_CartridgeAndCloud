namespace VRMGames.CartridgeAndCloud.Domain.Inventory
{
    public enum InventoryMutationFailureReason
    {
        None = 0,
        InvalidQuantity = 1,
        InsufficientQuantity = 2,
        CapacityExceeded = 3
    }
}
