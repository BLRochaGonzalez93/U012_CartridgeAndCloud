using System;

namespace VRMGames.CartridgeAndCloud.Domain.DayCycle
{
    public readonly struct StoreDayId : IEquatable<StoreDayId>
    {
        public string Value { get; }

        public StoreDayId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "Store day ID cannot be empty.",
                    nameof(value));
            }

            Value = value;
        }

        public bool Equals(StoreDayId other)
        {
            return string.Equals(
                Value,
                other.Value,
                StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is StoreDayId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return StringComparer.Ordinal.GetHashCode(
                Value ?? string.Empty);
        }

        public override string ToString()
        {
            return Value ?? string.Empty;
        }

        public static bool operator ==(
            StoreDayId left,
            StoreDayId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(
            StoreDayId left,
            StoreDayId right)
        {
            return !left.Equals(right);
        }
    }

    public enum StoreDayState
    {
        BeforeOpen = 0,
        Open = 1,
        Closing = 2,
        Closed = 3
    }

    public sealed class StoreDayPolicy
    {
        public int OpenDurationSeconds { get; }

        public bool AutoBeginClosing { get; }

        public StoreDayPolicy(
            int openDurationSeconds,
            bool autoBeginClosing)
        {
            if (openDurationSeconds <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(openDurationSeconds));
            }

            OpenDurationSeconds = openDurationSeconds;
            AutoBeginClosing = autoBeginClosing;
        }
    }

    public enum StoreDayTransitionFailureReason
    {
        None = 0,
        InvalidState = 1,
        InvalidElapsedSeconds = 2,
        ClosingConditionsNotMet = 3
    }

    public sealed class StoreDayTransitionResult
    {
        public bool Succeeded { get; }

        public StoreDayTransitionFailureReason FailureReason { get; }

        public StoreDayState StateBefore { get; }

        public StoreDayState StateAfter { get; }

        public int ElapsedOpenSeconds { get; }

        public bool ClosingStarted { get; }

        private StoreDayTransitionResult(
            bool succeeded,
            StoreDayTransitionFailureReason failureReason,
            StoreDayState stateBefore,
            StoreDayState stateAfter,
            int elapsedOpenSeconds,
            bool closingStarted)
        {
            Succeeded = succeeded;
            FailureReason = failureReason;
            StateBefore = stateBefore;
            StateAfter = stateAfter;
            ElapsedOpenSeconds = elapsedOpenSeconds;
            ClosingStarted = closingStarted;
        }

        public static StoreDayTransitionResult Success(
            StoreDayState before,
            StoreDayState after,
            int elapsedOpenSeconds,
            bool closingStarted)
        {
            return new StoreDayTransitionResult(
                true,
                StoreDayTransitionFailureReason.None,
                before,
                after,
                elapsedOpenSeconds,
                closingStarted);
        }

        public static StoreDayTransitionResult Failure(
            StoreDayTransitionFailureReason reason,
            StoreDayState state,
            int elapsedOpenSeconds)
        {
            return new StoreDayTransitionResult(
                false,
                reason,
                state,
                state,
                elapsedOpenSeconds,
                false);
        }
    }

    public sealed class StoreDay
    {
        public StoreDayId Id { get; }

        public StoreDayPolicy Policy { get; }

        public StoreDayState State { get; private set; }

        public int ElapsedOpenSeconds { get; private set; }

        public int RemainingOpenSeconds =>
            Math.Max(
                0,
                Policy.OpenDurationSeconds -
                ElapsedOpenSeconds);

        public bool CanAcceptCustomers =>
            State == StoreDayState.Open;

        public StoreDay(
            StoreDayId id,
            StoreDayPolicy policy)
        {
            Policy = policy ??
                throw new ArgumentNullException(nameof(policy));

            Id = id;
            State = StoreDayState.BeforeOpen;
            ElapsedOpenSeconds = 0;
        }

        public StoreDayTransitionResult TryOpen()
        {
            StoreDayState before = State;

            if (State != StoreDayState.BeforeOpen)
            {
                return Failure(
                    StoreDayTransitionFailureReason.InvalidState);
            }

            State = StoreDayState.Open;
            return Success(before, false);
        }

        public StoreDayTransitionResult Advance(
            int elapsedSeconds)
        {
            StoreDayState before = State;

            if (elapsedSeconds <= 0)
            {
                return Failure(
                    StoreDayTransitionFailureReason
                        .InvalidElapsedSeconds);
            }

            if (State != StoreDayState.Open)
            {
                return Failure(
                    StoreDayTransitionFailureReason.InvalidState);
            }

            ElapsedOpenSeconds = Math.Min(
                Policy.OpenDurationSeconds,
                checked(
                    ElapsedOpenSeconds +
                    elapsedSeconds));

            bool closingStarted = false;
            if (Policy.AutoBeginClosing &&
                ElapsedOpenSeconds >=
                    Policy.OpenDurationSeconds)
            {
                State = StoreDayState.Closing;
                closingStarted = true;
            }

            return Success(before, closingStarted);
        }

        public StoreDayTransitionResult TryBeginClosing()
        {
            StoreDayState before = State;

            if (State != StoreDayState.Open)
            {
                return Failure(
                    StoreDayTransitionFailureReason.InvalidState);
            }

            State = StoreDayState.Closing;
            return Success(before, true);
        }

        public StoreDayTransitionResult TryCompleteClosing(
            bool conditionsMet)
        {
            StoreDayState before = State;

            if (State != StoreDayState.Closing)
            {
                return Failure(
                    StoreDayTransitionFailureReason.InvalidState);
            }

            if (!conditionsMet)
            {
                return Failure(
                    StoreDayTransitionFailureReason
                        .ClosingConditionsNotMet);
            }

            State = StoreDayState.Closed;
            return Success(before, false);
        }

        private StoreDayTransitionResult Success(
            StoreDayState before,
            bool closingStarted)
        {
            return StoreDayTransitionResult.Success(
                before,
                State,
                ElapsedOpenSeconds,
                closingStarted);
        }

        private StoreDayTransitionResult Failure(
            StoreDayTransitionFailureReason reason)
        {
            return StoreDayTransitionResult.Failure(
                reason,
                State,
                ElapsedOpenSeconds);
        }
    }
}
