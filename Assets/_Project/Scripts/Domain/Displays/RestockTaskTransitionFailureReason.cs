namespace VRMGames.CartridgeAndCloud.Domain.Displays
{
    public enum RestockTaskTransitionFailureReason
    {
        None = 0,
        TaskNotPending = 1,
        InvalidCompletedQuantity = 2,
        CompletedQuantityExceedsRequested = 3
    }
}
