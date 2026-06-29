using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.UIUX;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.UIUX
{
    public sealed class TutorialProgressTests
    {
        [Test] public void New_IsNotStarted() =>
            Assert.That(
                TutorialProgress.New().State,
                Is.EqualTo(
                    TutorialProgressState
                        .NotStarted));

        [Test] public void New_StartsAtWelcome() =>
            Assert.That(
                TutorialProgress.New()
                    .CurrentStep,
                Is.EqualTo(
                    TutorialStepId.Welcome));

        [Test] public void Start_MakesProgressActive() =>
            Assert.That(
                TutorialProgress.New()
                    .Start().State,
                Is.EqualTo(
                    TutorialProgressState.Active));

        [Test] public void Start_IsIdempotentWhenActive()
        {
            TutorialProgress active =
                TutorialProgress.New().Start();

            Assert.That(
                active.Start(),
                Is.SameAs(active));
        }

        [Test] public void Advance_MovesOneStep() =>
            Assert.That(
                TutorialProgress.New()
                    .Start()
                    .Advance()
                    .CurrentStep,
                Is.EqualTo(
                    TutorialStepId
                        .MovementAndCamera));

        [Test] public void Advance_DoesNothingWhenNotStarted()
        {
            TutorialProgress progress =
                TutorialProgress.New();

            Assert.That(
                progress.Advance(),
                Is.SameAs(progress));
        }

        [Test] public void Advance_DoesNothingWhenSkipped()
        {
            TutorialProgress progress =
                TutorialProgress.New().Skip();

            Assert.That(
                progress.Advance(),
                Is.SameAs(progress));
        }

        [Test] public void Complete_UsesCompletedStep()
        {
            TutorialProgress progress =
                TutorialProgress.New()
                    .Start()
                    .Complete();

            Assert.That(
                progress.State,
                Is.EqualTo(
                    TutorialProgressState.Completed));
            Assert.That(
                progress.CurrentStep,
                Is.EqualTo(
                    TutorialStepId.Completed));
        }

        [Test] public void Skip_IsTerminal() =>
            Assert.That(
                TutorialProgress.New()
                    .Start()
                    .Skip()
                    .IsTerminal,
                Is.True);

        [Test] public void Completed_IsTerminal() =>
            Assert.That(
                TutorialProgress.New()
                    .Complete()
                    .IsTerminal,
                Is.True);

        [Test] public void Active_IsNotTerminal() =>
            Assert.That(
                TutorialProgress.New()
                    .Start()
                    .IsTerminal,
                Is.False);

        [Test] public void Restart_ReturnsActiveWelcome()
        {
            TutorialProgress progress =
                TutorialProgress.New()
                    .Start()
                    .Skip()
                    .Restart();

            Assert.That(
                progress.State,
                Is.EqualTo(
                    TutorialProgressState.Active));
            Assert.That(
                progress.CurrentStep,
                Is.EqualTo(
                    TutorialStepId.Welcome));
        }

        [Test] public void Constructor_RejectsUnknownState() =>
            Assert.Throws<
                System.ArgumentOutOfRangeException>(
                () => new TutorialProgress(
                    (TutorialProgressState)99,
                    TutorialStepId.Welcome));

        [Test] public void Constructor_RejectsUnknownStep() =>
            Assert.Throws<
                System.ArgumentOutOfRangeException>(
                () => new TutorialProgress(
                    TutorialProgressState.Active,
                    (TutorialStepId)99));

        [Test] public void NotStarted_RequiresWelcome() =>
            Assert.Throws<
                System.ArgumentException>(
                () => new TutorialProgress(
                    TutorialProgressState
                        .NotStarted,
                    TutorialStepId.Inventory));

        [Test] public void Completed_RequiresCompletedStep() =>
            Assert.Throws<
                System.ArgumentException>(
                () => new TutorialProgress(
                    TutorialProgressState
                        .Completed,
                    TutorialStepId.Inventory));

        [Test] public void Bubble_RejectsEmptyTitle() =>
            Assert.Throws<
                System.ArgumentException>(
                () => new TutorialBubble(
                    TutorialStepId.Welcome,
                    "",
                    "Body",
                    "Action",
                    "Anchor"));

        [Test] public void Bubble_StoresAnchor()
        {
            TutorialBubble bubble =
                new TutorialBubble(
                    TutorialStepId.Inventory,
                    "Inventory",
                    "Body",
                    "Action",
                    "panel-inventory");

            Assert.That(
                bubble.AnchorId,
                Is.EqualTo(
                    "panel-inventory"));
        }
    }
}
