namespace VRMGames.CartridgeAndCloud.Application.Customers
{
    public enum CustomerSpawnFailureReason
    {
        None = 0,
        QueueEmpty = 1,
        PopulationLimitReached = 2,
        ProfileMissing = 3,
        DuplicateInstanceId = 4,
        QueueStateChanged = 5
    }
}
