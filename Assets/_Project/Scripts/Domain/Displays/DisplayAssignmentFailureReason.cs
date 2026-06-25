namespace VRMGames.CartridgeAndCloud.Domain.Displays
{
    public enum DisplayAssignmentFailureReason
    {
        None = 0,
        ProductDefinitionMissing = 1,
        CategoryNotAllowed = 2,
        ProductAlreadyAssigned = 3,
        DifferentProductAlreadyAssigned = 4,
        DisplayContainsStock = 5
    }
}
