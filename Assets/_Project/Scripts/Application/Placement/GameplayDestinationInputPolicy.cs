namespace VRMGames.CartridgeAndCloud.Application.Placement
{
    public static class GameplayDestinationInputPolicy
    {
        public static bool ShouldSetDestination(
            bool destinationPressed,
            bool placementModeActive)
        {
            return destinationPressed &&
                   !placementModeActive;
        }
    }
}
