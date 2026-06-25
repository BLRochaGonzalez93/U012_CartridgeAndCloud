using VRMGames.CartridgeAndCloud.Domain.Products;

namespace VRMGames.CartridgeAndCloud.Domain.Displays
{
    public sealed class DisplayClearAssignmentResult
    {
        public bool Succeeded { get; }

        public DisplayClearAssignmentFailureReason FailureReason { get; }

        public ProductDefinitionId PreviousProductId { get; }

        public bool HasAssignmentAfter { get; }

        private DisplayClearAssignmentResult(
            bool succeeded,
            DisplayClearAssignmentFailureReason failureReason,
            ProductDefinitionId previousProductId,
            bool hasAssignmentAfter)
        {
            Succeeded = succeeded;
            FailureReason = failureReason;
            PreviousProductId = previousProductId;
            HasAssignmentAfter = hasAssignmentAfter;
        }

        public static DisplayClearAssignmentResult Success(
            ProductDefinitionId previousProductId)
        {
            return new DisplayClearAssignmentResult(
                true,
                DisplayClearAssignmentFailureReason.None,
                previousProductId,
                false);
        }

        public static DisplayClearAssignmentResult Failure(
            DisplayClearAssignmentFailureReason failureReason,
            ProductDefinitionId productId,
            bool hasAssignment)
        {
            return new DisplayClearAssignmentResult(
                false,
                failureReason,
                productId,
                hasAssignment);
        }
    }
}
