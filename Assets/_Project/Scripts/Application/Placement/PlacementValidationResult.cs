using VRMGames.CartridgeAndCloud.Domain.Placement;

namespace VRMGames.CartridgeAndCloud.Application.Placement
{
    public readonly struct PlacementValidationResult
    {
        public bool IsValid { get; }
        public PlacementFailureReason FailureReason { get; }

        private PlacementValidationResult(
            bool isValid,
            PlacementFailureReason failureReason)
        {
            IsValid = isValid;
            FailureReason = failureReason;
        }

        public static PlacementValidationResult Valid()
        {
            return new PlacementValidationResult(
                true,
                PlacementFailureReason.None);
        }

        public static PlacementValidationResult Invalid(
            PlacementFailureReason failureReason)
        {
            return new PlacementValidationResult(
                false,
                failureReason);
        }
    }
}
