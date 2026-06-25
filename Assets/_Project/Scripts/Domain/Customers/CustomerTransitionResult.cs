namespace VRMGames.CartridgeAndCloud.Domain.Customers
{
    public sealed class CustomerTransitionResult
    {
        public bool Succeeded { get; }

        public CustomerTransitionFailureReason FailureReason { get; }

        public CustomerState StateBefore { get; }

        public CustomerState StateAfter { get; }

        public int RemainingPatienceSeconds { get; }

        private CustomerTransitionResult(
            bool succeeded,
            CustomerTransitionFailureReason failureReason,
            CustomerState stateBefore,
            CustomerState stateAfter,
            int remainingPatienceSeconds)
        {
            Succeeded = succeeded;
            FailureReason = failureReason;
            StateBefore = stateBefore;
            StateAfter = stateAfter;
            RemainingPatienceSeconds = remainingPatienceSeconds;
        }

        public static CustomerTransitionResult Success(
            CustomerState stateBefore,
            CustomerState stateAfter,
            int remainingPatienceSeconds)
        {
            return new CustomerTransitionResult(
                true,
                CustomerTransitionFailureReason.None,
                stateBefore,
                stateAfter,
                remainingPatienceSeconds);
        }

        public static CustomerTransitionResult Failure(
            CustomerTransitionFailureReason reason,
            CustomerState state,
            int remainingPatienceSeconds)
        {
            return new CustomerTransitionResult(
                false,
                reason,
                state,
                state,
                remainingPatienceSeconds);
        }
    }
}
