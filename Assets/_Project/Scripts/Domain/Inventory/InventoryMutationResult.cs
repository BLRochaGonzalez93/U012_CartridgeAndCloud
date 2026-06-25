namespace VRMGames.CartridgeAndCloud.Domain.Inventory
{
    public sealed class InventoryMutationResult
    {
        public bool Succeeded { get; }

        public InventoryMutationFailureReason FailureReason { get; }

        public Quantity PreviousProductQuantity { get; }

        public Quantity CurrentProductQuantity { get; }

        public int PreviousUsedCapacity { get; }

        public int CurrentUsedCapacity { get; }

        private InventoryMutationResult(
            bool succeeded,
            InventoryMutationFailureReason failureReason,
            Quantity previousProductQuantity,
            Quantity currentProductQuantity,
            int previousUsedCapacity,
            int currentUsedCapacity)
        {
            Succeeded = succeeded;
            FailureReason = failureReason;
            PreviousProductQuantity = previousProductQuantity;
            CurrentProductQuantity = currentProductQuantity;
            PreviousUsedCapacity = previousUsedCapacity;
            CurrentUsedCapacity = currentUsedCapacity;
        }

        public static InventoryMutationResult Success(
            Quantity previousProductQuantity,
            Quantity currentProductQuantity,
            int previousUsedCapacity,
            int currentUsedCapacity)
        {
            return new InventoryMutationResult(
                true,
                InventoryMutationFailureReason.None,
                previousProductQuantity,
                currentProductQuantity,
                previousUsedCapacity,
                currentUsedCapacity);
        }

        public static InventoryMutationResult Failure(
            InventoryMutationFailureReason failureReason,
            Quantity productQuantity,
            int usedCapacity)
        {
            return new InventoryMutationResult(
                false,
                failureReason,
                productQuantity,
                productQuantity,
                usedCapacity,
                usedCapacity);
        }
    }
}
