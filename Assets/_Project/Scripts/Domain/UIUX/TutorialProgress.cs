using System;

namespace VRMGames.CartridgeAndCloud.Domain.UIUX
{
    public enum TutorialStepId
    {
        Welcome = 0,
        MovementAndCamera = 1,
        OpenManagement = 2,
        Inventory = 3,
        Suppliers = 4,
        Displays = 5,
        CustomersAndShopping = 6,
        QueueAndCheckout = 7,
        DayCycle = 8,
        DailyResults = 9,
        AutosaveComplete = 10,
        Completed = 11
    }

    public enum TutorialProgressState
    {
        NotStarted = 0,
        Active = 1,
        Completed = 2,
        Skipped = 3
    }

    public sealed class TutorialProgress
    {
        public TutorialProgressState State { get; }

        public TutorialStepId CurrentStep { get; }

        public bool IsTerminal =>
            State == TutorialProgressState.Completed ||
            State == TutorialProgressState.Skipped;

        public TutorialProgress(
            TutorialProgressState state,
            TutorialStepId currentStep)
        {
            if (!Enum.IsDefined(
                    typeof(TutorialProgressState),
                    state))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(state));
            }

            if (!Enum.IsDefined(
                    typeof(TutorialStepId),
                    currentStep))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(currentStep));
            }

            if (state ==
                    TutorialProgressState.Completed &&
                currentStep != TutorialStepId.Completed)
            {
                throw new ArgumentException(
                    "Completed progress must use the " +
                    "Completed step.",
                    nameof(currentStep));
            }

            if (state ==
                    TutorialProgressState.NotStarted &&
                currentStep != TutorialStepId.Welcome)
            {
                throw new ArgumentException(
                    "Not-started progress must use the " +
                    "Welcome step.",
                    nameof(currentStep));
            }

            State = state;
            CurrentStep = currentStep;
        }

        public static TutorialProgress New()
        {
            return new TutorialProgress(
                TutorialProgressState.NotStarted,
                TutorialStepId.Welcome);
        }

        public TutorialProgress Start()
        {
            if (State !=
                TutorialProgressState.NotStarted)
            {
                return this;
            }

            return new TutorialProgress(
                TutorialProgressState.Active,
                TutorialStepId.Welcome);
        }

        public TutorialProgress Advance()
        {
            if (State != TutorialProgressState.Active)
            {
                return this;
            }

            int next = (int)CurrentStep + 1;

            if (next >= (int)TutorialStepId.Completed)
            {
                return Complete();
            }

            return new TutorialProgress(
                TutorialProgressState.Active,
                (TutorialStepId)next);
        }

        public TutorialProgress Complete()
        {
            return new TutorialProgress(
                TutorialProgressState.Completed,
                TutorialStepId.Completed);
        }

        public TutorialProgress Skip()
        {
            return new TutorialProgress(
                TutorialProgressState.Skipped,
                CurrentStep);
        }

        public TutorialProgress Restart()
        {
            return New().Start();
        }
    }

    public sealed class TutorialBubble
    {
        public TutorialStepId Step { get; }
        public string Title { get; }
        public string Body { get; }
        public string ActionHint { get; }
        public string AnchorId { get; }

        public TutorialBubble(
            TutorialStepId step,
            string title,
            string body,
            string actionHint,
            string anchorId)
        {
            Step = step;
            Title = Required(title, nameof(title));
            Body = Required(body, nameof(body));
            ActionHint = actionHint ?? string.Empty;
            AnchorId = Required(
                anchorId,
                nameof(anchorId));
        }

        private static string Required(
            string value,
            string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "Value cannot be empty.",
                    parameterName);
            }

            return value;
        }
    }
}
