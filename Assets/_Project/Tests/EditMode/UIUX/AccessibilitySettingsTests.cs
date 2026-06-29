using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.UIUX;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.UIUX
{
    public sealed class AccessibilitySettingsTests
    {
        [Test] public void Default_UiScaleIs100() =>
            Assert.That(
                UiAccessibilitySettings.Default()
                    .UiScalePercent,
                Is.EqualTo(100));

        [Test] public void Default_TextScaleIs100() =>
            Assert.That(
                UiAccessibilitySettings.Default()
                    .TextScalePercent,
                Is.EqualTo(100));

        [Test] public void Default_TutorialEnabled() =>
            Assert.That(
                UiAccessibilitySettings.Default()
                    .TutorialEnabled,
                Is.True);

        [Test] public void Default_ConfirmationsEnabled() =>
            Assert.That(
                UiAccessibilitySettings.Default()
                    .ConfirmDestructiveActions,
                Is.True);

        [Test] public void Constructor_RejectsLowUiScale() =>
            Assert.Throws<
                System.ArgumentOutOfRangeException>(
                () => Create(uiScale: 79));

        [Test] public void Constructor_RejectsHighUiScale() =>
            Assert.Throws<
                System.ArgumentOutOfRangeException>(
                () => Create(uiScale: 151));

        [Test] public void Constructor_RejectsLowTextScale() =>
            Assert.Throws<
                System.ArgumentOutOfRangeException>(
                () => Create(textScale: 79));

        [Test] public void Constructor_RejectsHighTextScale() =>
            Assert.Throws<
                System.ArgumentOutOfRangeException>(
                () => Create(textScale: 151));

        [Test] public void Constructor_RejectsShortMessage() =>
            Assert.Throws<
                System.ArgumentOutOfRangeException>(
                () => Create(messageSeconds: 0));

        [Test] public void Constructor_RejectsLongMessage() =>
            Assert.Throws<
                System.ArgumentOutOfRangeException>(
                () => Create(messageSeconds: 31));

        [Test] public void WithUiScale_ChangesOnlyUiScale()
        {
            UiAccessibilitySettings source = Create();
            UiAccessibilitySettings result =
                source.WithUiScale(120);

            Assert.That(
                result.UiScalePercent,
                Is.EqualTo(120));
            Assert.That(
                result.TextScalePercent,
                Is.EqualTo(100));
        }

        [Test] public void WithTextScale_ChangesOnlyTextScale()
        {
            UiAccessibilitySettings source = Create();
            UiAccessibilitySettings result =
                source.WithTextScale(130);

            Assert.That(
                result.TextScalePercent,
                Is.EqualTo(130));
            Assert.That(
                result.UiScalePercent,
                Is.EqualTo(100));
        }

        [Test] public void WithReduceMotion_ChangesValue() =>
            Assert.That(
                Create()
                    .WithReduceMotion(true)
                    .ReduceMotion,
                Is.True);

        [Test] public void WithTutorialEnabled_ChangesValue() =>
            Assert.That(
                Create()
                    .WithTutorialEnabled(false)
                    .TutorialEnabled,
                Is.False);

        [Test] public void WithConfirmations_ChangesValue() =>
            Assert.That(
                Create()
                    .WithDestructiveConfirmations(
                        false)
                    .ConfirmDestructiveActions,
                Is.False);

        [Test] public void EqualSettings_AreEqual() =>
            Assert.That(
                Create().Equals(Create()),
                Is.True);

        [Test] public void DifferentSettings_AreNotEqual() =>
            Assert.That(
                Create().Equals(
                    Create(uiScale: 110)),
                Is.False);

        [Test] public void Multipliers_UsePercent()
        {
            UiAccessibilitySettings settings =
                Create(
                    uiScale: 120,
                    textScale: 130);

            Assert.That(
                settings.UiScaleMultiplier,
                Is.EqualTo(1.2f));
            Assert.That(
                settings.TextScaleMultiplier,
                Is.EqualTo(1.3f));
        }

        private static UiAccessibilitySettings Create(
            int uiScale = 100,
            int textScale = 100,
            int messageSeconds = 6)
        {
            return new UiAccessibilitySettings(
                uiScale,
                textScale,
                false,
                messageSeconds,
                true,
                true);
        }
    }
}
