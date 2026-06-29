using System;

namespace VRMGames.CartridgeAndCloud.Domain.UIUX
{
    public sealed class UiAccessibilitySettings :
        IEquatable<UiAccessibilitySettings>
    {
        public const int MinimumScalePercent = 80;
        public const int MaximumScalePercent = 150;
        public const int MinimumMessageSeconds = 1;
        public const int MaximumMessageSeconds = 30;

        public int UiScalePercent { get; }
        public int TextScalePercent { get; }
        public bool ReduceMotion { get; }
        public int MessageDurationSeconds { get; }
        public bool TutorialEnabled { get; }
        public bool ConfirmDestructiveActions { get; }

        public float UiScaleMultiplier =>
            UiScalePercent / 100f;

        public float TextScaleMultiplier =>
            TextScalePercent / 100f;

        public UiAccessibilitySettings(
            int uiScalePercent,
            int textScalePercent,
            bool reduceMotion,
            int messageDurationSeconds,
            bool tutorialEnabled,
            bool confirmDestructiveActions)
        {
            UiScalePercent = ValidateScale(
                uiScalePercent,
                nameof(uiScalePercent));
            TextScalePercent = ValidateScale(
                textScalePercent,
                nameof(textScalePercent));

            if (messageDurationSeconds <
                    MinimumMessageSeconds ||
                messageDurationSeconds >
                    MaximumMessageSeconds)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(messageDurationSeconds),
                    messageDurationSeconds,
                    $"Message duration must be between " +
                    $"{MinimumMessageSeconds} and " +
                    $"{MaximumMessageSeconds} seconds.");
            }

            ReduceMotion = reduceMotion;
            MessageDurationSeconds =
                messageDurationSeconds;
            TutorialEnabled = tutorialEnabled;
            ConfirmDestructiveActions =
                confirmDestructiveActions;
        }

        public static UiAccessibilitySettings Default()
        {
            return new UiAccessibilitySettings(
                100,
                100,
                reduceMotion: false,
                messageDurationSeconds: 6,
                tutorialEnabled: true,
                confirmDestructiveActions: true);
        }

        public UiAccessibilitySettings WithUiScale(
            int value)
        {
            return new UiAccessibilitySettings(
                value,
                TextScalePercent,
                ReduceMotion,
                MessageDurationSeconds,
                TutorialEnabled,
                ConfirmDestructiveActions);
        }

        public UiAccessibilitySettings WithTextScale(
            int value)
        {
            return new UiAccessibilitySettings(
                UiScalePercent,
                value,
                ReduceMotion,
                MessageDurationSeconds,
                TutorialEnabled,
                ConfirmDestructiveActions);
        }

        public UiAccessibilitySettings WithReduceMotion(
            bool value)
        {
            return new UiAccessibilitySettings(
                UiScalePercent,
                TextScalePercent,
                value,
                MessageDurationSeconds,
                TutorialEnabled,
                ConfirmDestructiveActions);
        }

        public UiAccessibilitySettings WithTutorialEnabled(
            bool value)
        {
            return new UiAccessibilitySettings(
                UiScalePercent,
                TextScalePercent,
                ReduceMotion,
                MessageDurationSeconds,
                value,
                ConfirmDestructiveActions);
        }

        public UiAccessibilitySettings
            WithDestructiveConfirmations(bool value)
        {
            return new UiAccessibilitySettings(
                UiScalePercent,
                TextScalePercent,
                ReduceMotion,
                MessageDurationSeconds,
                TutorialEnabled,
                value);
        }

        public bool Equals(
            UiAccessibilitySettings other)
        {
            return other != null &&
                UiScalePercent == other.UiScalePercent &&
                TextScalePercent == other.TextScalePercent &&
                ReduceMotion == other.ReduceMotion &&
                MessageDurationSeconds ==
                    other.MessageDurationSeconds &&
                TutorialEnabled == other.TutorialEnabled &&
                ConfirmDestructiveActions ==
                    other.ConfirmDestructiveActions;
        }

        public override bool Equals(object obj)
        {
            return Equals(
                obj as UiAccessibilitySettings);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = UiScalePercent;
                hash = (hash * 397) ^
                    TextScalePercent;
                hash = (hash * 397) ^
                    ReduceMotion.GetHashCode();
                hash = (hash * 397) ^
                    MessageDurationSeconds;
                hash = (hash * 397) ^
                    TutorialEnabled.GetHashCode();
                hash = (hash * 397) ^
                    ConfirmDestructiveActions
                        .GetHashCode();
                return hash;
            }
        }

        private static int ValidateScale(
            int value,
            string parameterName)
        {
            if (value < MinimumScalePercent ||
                value > MaximumScalePercent)
            {
                throw new ArgumentOutOfRangeException(
                    parameterName,
                    value,
                    $"Scale must be between " +
                    $"{MinimumScalePercent}% and " +
                    $"{MaximumScalePercent}%.");
            }

            return value;
        }
    }
}
