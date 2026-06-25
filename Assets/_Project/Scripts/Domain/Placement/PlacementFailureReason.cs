namespace VRMGames.CartridgeAndCloud.Domain.Placement
{
    public enum PlacementFailureReason
    {
        None = 0,
        OutOfBounds = 1,
        Overlap = 2,
        DuplicateId = 3,
        NotFound = 4
    }
}
