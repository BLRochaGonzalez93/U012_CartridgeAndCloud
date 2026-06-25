namespace VRMGames.CartridgeAndCloud.Domain.Customers
{
    public enum CustomerTransitionFailureReason
    {
        None = 0,
        InvalidState = 1,
        InvalidElapsedSeconds = 2,
        AlreadyDespawned = 3,
        TargetMismatch = 4
    }
}
