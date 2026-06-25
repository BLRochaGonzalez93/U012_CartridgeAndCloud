namespace VRMGames.CartridgeAndCloud.Application.Orders
{
    public enum OrderCreationFailureReason
    {
        None = 0,
        EmptyRequest = 1,
        DuplicateProduct = 2,
        ProductNotOffered = 3,
        BoxCountOutsideSupplierLimits = 4
    }
}
