using VRMGames.CartridgeAndCloud.Domain.Access;

namespace VRMGames.CartridgeAndCloud.Application.Access
{
    public sealed class AccessValidationResult
    {
        public bool IsValid { get; }

        public AccessValidationFailureReason FailureReason
        {
            get;
        }

        public AccessAnchorId? FailedAnchorId { get; }

        public int ReachableCellCount { get; }

        private AccessValidationResult(
            bool isValid,
            AccessValidationFailureReason failureReason,
            AccessAnchorId? failedAnchorId,
            int reachableCellCount)
        {
            IsValid = isValid;
            FailureReason = failureReason;
            FailedAnchorId = failedAnchorId;
            ReachableCellCount = reachableCellCount;
        }

        public static AccessValidationResult Valid(
            int reachableCellCount)
        {
            return new AccessValidationResult(
                true,
                AccessValidationFailureReason.None,
                null,
                reachableCellCount);
        }

        public static AccessValidationResult Invalid(
            AccessValidationFailureReason failureReason,
            AccessAnchorId? failedAnchorId = null,
            int reachableCellCount = 0)
        {
            return new AccessValidationResult(
                false,
                failureReason,
                failedAnchorId,
                reachableCellCount);
        }
    }
}
