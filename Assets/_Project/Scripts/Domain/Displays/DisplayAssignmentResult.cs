using VRMGames.CartridgeAndCloud.Domain.Products;

namespace VRMGames.CartridgeAndCloud.Domain.Displays
{
    public sealed class DisplayAssignmentResult
    {
        public bool Succeeded { get; }

        public DisplayAssignmentFailureReason FailureReason { get; }

        public bool HadAssignmentBefore { get; }

        public bool HasAssignmentAfter { get; }

        public ProductDefinitionId PreviousProductId { get; }

        public ProductDefinitionId CurrentProductId { get; }

        private DisplayAssignmentResult(
            bool succeeded,
            DisplayAssignmentFailureReason failureReason,
            bool hadAssignmentBefore,
            bool hasAssignmentAfter,
            ProductDefinitionId previousProductId,
            ProductDefinitionId currentProductId)
        {
            Succeeded = succeeded;
            FailureReason = failureReason;
            HadAssignmentBefore = hadAssignmentBefore;
            HasAssignmentAfter = hasAssignmentAfter;
            PreviousProductId = previousProductId;
            CurrentProductId = currentProductId;
        }

        public static DisplayAssignmentResult Success(
            ProductDefinitionId productId)
        {
            return new DisplayAssignmentResult(
                true,
                DisplayAssignmentFailureReason.None,
                false,
                true,
                default(ProductDefinitionId),
                productId);
        }

        public static DisplayAssignmentResult Failure(
            DisplayAssignmentFailureReason failureReason,
            bool hasAssignment,
            ProductDefinitionId productId)
        {
            return new DisplayAssignmentResult(
                false,
                failureReason,
                hasAssignment,
                hasAssignment,
                productId,
                productId);
        }
    }
}
