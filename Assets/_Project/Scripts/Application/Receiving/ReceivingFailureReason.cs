namespace VRMGames.CartridgeAndCloud.Application.Receiving
{
    public enum ReceivingFailureReason
    {
        None = 0,
        OrderNotDelivered = 1,
        DeliveryOrderMismatch = 2,
        DeliverySupplierMismatch = 3,
        BoxNotFound = 4,
        BoxAlreadyReceived = 5,
        ProductDefinitionMissing = 6,
        DestinationCapacityExceeded = 7,
        InventoryMutationRejected = 8,
        DeliveryMutationRejected = 9
    }
}
