namespace VRMGames.CartridgeAndCloud.Application.Access
{
    public enum AccessValidationFailureReason
    {
        None = 0,
        BlockedCellOutsideBounds = 1,
        ReservedEntranceBlocked = 2,
        NoOpenEntrance = 3,
        EntranceWidthInsufficient = 4,
        RequiredAnchorBlocked = 5,
        RequiredAnchorUnreachable = 6
    }
}
