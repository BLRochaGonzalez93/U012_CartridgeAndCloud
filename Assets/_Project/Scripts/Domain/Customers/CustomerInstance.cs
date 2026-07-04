using System;

namespace VRMGames.CartridgeAndCloud.Domain.Customers
{
    public sealed class CustomerInstance
    {
        public CustomerInstanceId Id { get; }

        public CustomerProfileId ProfileId { get; }

        public CustomerNavigationPlan NavigationPlan { get; }

        public CustomerState State { get; private set; }

        public int CurrentTargetIndex { get; private set; }

        public int RemainingPatienceSeconds { get; private set; }

        public bool IsActive => State != CustomerState.Despawned;

        public CustomerNavigationTarget CurrentTarget =>
            NavigationPlan.Targets[CurrentTargetIndex];

        public CustomerInstance(
            CustomerInstanceId id,
            CustomerProfileId profileId,
            CustomerNavigationPlan navigationPlan,
            int patienceSeconds)
        {
            if (string.IsNullOrWhiteSpace(id.Value))
            {
                throw new ArgumentException(
                    "Customer instance ID must be initialized.",
                    nameof(id));
            }

            if (string.IsNullOrWhiteSpace(profileId.Value))
            {
                throw new ArgumentException(
                    "Customer profile ID must be initialized.",
                    nameof(profileId));
            }

            NavigationPlan = navigationPlan ??
                throw new ArgumentNullException(nameof(navigationPlan));

            if (patienceSeconds <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(patienceSeconds));
            }

            Id = id;
            ProfileId = profileId;
            RemainingPatienceSeconds = patienceSeconds;
            State = CustomerState.WaitingToEnter;
            CurrentTargetIndex = 0;
        }

        public CustomerTransitionResult BeginEntering()
        {
            CustomerState before = State;

            if (State != CustomerState.WaitingToEnter)
            {
                return Failure(CustomerTransitionFailureReason.InvalidState);
            }

            State = CustomerState.Entering;
            CurrentTargetIndex = 0;
            return Success(before);
        }

        public CustomerTransitionResult ArriveAtCurrentTarget()
        {
            CustomerState before = State;

            if (State == CustomerState.Despawned)
            {
                return Failure(
                    CustomerTransitionFailureReason.AlreadyDespawned);
            }

            CustomerNavigationTarget target = CurrentTarget;

            if (State == CustomerState.Entering &&
                target.Type == CustomerNavigationTargetType.Entry)
            {
                CurrentTargetIndex++;
                State = CurrentTarget.Type ==
                    CustomerNavigationTargetType.Exit
                        ? CustomerState.Leaving
                        : CustomerState.Browsing;
                return Success(before);
            }

            if (State == CustomerState.Browsing &&
                target.Type == CustomerNavigationTargetType.Browse)
            {
                CurrentTargetIndex++;
                if (CurrentTarget.Type == CustomerNavigationTargetType.Exit)
                {
                    State = CustomerState.Leaving;
                }

                return Success(before);
            }

            if (State == CustomerState.Leaving &&
                target.Type == CustomerNavigationTargetType.Exit)
            {
                State = CustomerState.Despawned;
                return Success(before);
            }

            return Failure(CustomerTransitionFailureReason.TargetMismatch);
        }

        public CustomerTransitionResult BeginLeaving()
        {
            CustomerState before = State;

            if (State == CustomerState.Despawned)
            {
                return Failure(
                    CustomerTransitionFailureReason.AlreadyDespawned);
            }

            if (State != CustomerState.Entering &&
                State != CustomerState.Browsing)
            {
                return Failure(CustomerTransitionFailureReason.InvalidState);
            }

            State = CustomerState.Leaving;
            CurrentTargetIndex = NavigationPlan.Count - 1;
            return Success(before);
        }

        public CustomerTransitionResult AdvancePatience(int elapsedSeconds)
        {
            CustomerState before = State;

            if (elapsedSeconds <= 0)
            {
                return Failure(
                    CustomerTransitionFailureReason.InvalidElapsedSeconds);
            }

            if (State == CustomerState.Despawned)
            {
                return Failure(
                    CustomerTransitionFailureReason.AlreadyDespawned);
            }

            if (State != CustomerState.Browsing)
            {
                return Failure(CustomerTransitionFailureReason.InvalidState);
            }

            RemainingPatienceSeconds = Math.Max(
                0,
                RemainingPatienceSeconds - elapsedSeconds);

            if (RemainingPatienceSeconds == 0)
            {
                State = CustomerState.Leaving;
                CurrentTargetIndex = NavigationPlan.Count - 1;
            }

            return Success(before);
        }

        private CustomerTransitionResult Success(CustomerState before)
        {
            return CustomerTransitionResult.Success(
                before,
                State,
                RemainingPatienceSeconds);
        }

        private CustomerTransitionResult Failure(
            CustomerTransitionFailureReason reason)
        {
            return CustomerTransitionResult.Failure(
                reason,
                State,
                RemainingPatienceSeconds);
        }
    }

    public readonly struct CustomerInstanceId : IEquatable<CustomerInstanceId>
    {
        public string Value { get; }

        public CustomerInstanceId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "Customer instance ID cannot be empty.",
                    nameof(value));
            }

            Value = value;
        }

        public bool Equals(CustomerInstanceId other)
        {
            return string.Equals(
                Value,
                other.Value,
                StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is CustomerInstanceId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value == null
                ? 0
                : StringComparer.Ordinal.GetHashCode(Value);
        }

        public override string ToString()
        {
            return Value ?? string.Empty;
        }

        public static bool operator ==(CustomerInstanceId left, CustomerInstanceId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CustomerInstanceId left, CustomerInstanceId right)
        {
            return !left.Equals(right);
        }
    }

    public enum CustomerState
    {
        WaitingToEnter = 0,
        Entering = 1,
        Browsing = 2,
        Leaving = 3,
        Despawned = 4
    }

    public enum CustomerTransitionFailureReason
    {
        None = 0,
        InvalidState = 1,
        InvalidElapsedSeconds = 2,
        AlreadyDespawned = 3,
        TargetMismatch = 4
    }

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
