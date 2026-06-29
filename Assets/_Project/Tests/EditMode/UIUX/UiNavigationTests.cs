using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.UIUX;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.UIUX
{
    public sealed class UiNavigationTests
    {
        [Test] public void NewState_HasNoOverlay() =>
            Assert.That(
                new UiNavigationState()
                    .HasOverlay,
                Is.False);

        [Test] public void NewState_TopIsNull() =>
            Assert.That(
                new UiNavigationState().Top,
                Is.Null);

        [Test] public void Push_AddsOverlay()
        {
            UiNavigationState state =
                new UiNavigationState();
            state.Push(Entry(
                UiLayerId.ManagementPanel));

            Assert.That(
                state.HasOverlay,
                Is.True);
        }

        [Test] public void Push_SetsTop()
        {
            UiNavigationState state =
                new UiNavigationState();
            UiNavigationEntry entry =
                Entry(UiLayerId.Tooltip);
            state.Push(entry);

            Assert.That(
                state.Top,
                Is.SameAs(entry));
        }

        [Test] public void Push_DuplicateTopIsIgnored()
        {
            UiNavigationState state =
                new UiNavigationState();
            state.Push(Entry(
                UiLayerId.Submenu));
            state.Push(Entry(
                UiLayerId.Submenu));

            Assert.That(
                state.Entries.Count,
                Is.EqualTo(1));
        }

        [Test] public void Push_DifferentFocusIsAdded()
        {
            UiNavigationState state =
                new UiNavigationState();
            state.Push(
                new UiNavigationEntry(
                    UiLayerId.Submenu,
                    "first"));
            state.Push(
                new UiNavigationEntry(
                    UiLayerId.Submenu,
                    "second"));

            Assert.That(
                state.Entries.Count,
                Is.EqualTo(2));
        }

        [Test] public void Pop_ReturnsTop()
        {
            UiNavigationState state =
                new UiNavigationState();
            UiNavigationEntry entry =
                Entry(UiLayerId.Confirmation);
            state.Push(entry);

            Assert.That(
                state.Pop(),
                Is.SameAs(entry));
        }

        [Test] public void Pop_RemovesTop()
        {
            UiNavigationState state =
                new UiNavigationState();
            state.Push(Entry(
                UiLayerId.PauseMenu));
            state.Pop();

            Assert.That(
                state.HasOverlay,
                Is.False);
        }

        [Test] public void Pop_EmptyReturnsNull() =>
            Assert.That(
                new UiNavigationState().Pop(),
                Is.Null);

        [Test] public void Clear_RemovesEveryEntry()
        {
            UiNavigationState state =
                new UiNavigationState();
            state.Push(Entry(
                UiLayerId.ManagementPanel));
            state.Push(Entry(
                UiLayerId.Confirmation));
            state.Clear();

            Assert.That(
                state.Entries.Count,
                Is.EqualTo(0));
        }

        [Test] public void Entry_RejectsEmptyFocus() =>
            Assert.Throws<
                System.ArgumentException>(
                () => new UiNavigationEntry(
                    UiLayerId.Hud,
                    ""));

        [Test] public void Entries_AreOrderedByPush()
        {
            UiNavigationState state =
                new UiNavigationState();
            state.Push(
                new UiNavigationEntry(
                    UiLayerId.ManagementPanel,
                    "panel"));
            state.Push(
                new UiNavigationEntry(
                    UiLayerId.Confirmation,
                    "confirm"));

            Assert.That(
                state.Entries[0]
                    .FocusTargetId,
                Is.EqualTo("panel"));
            Assert.That(
                state.Entries[1]
                    .FocusTargetId,
                Is.EqualTo("confirm"));
        }

        private static UiNavigationEntry Entry(
            UiLayerId layer)
        {
            return new UiNavigationEntry(
                layer,
                "focus");
        }
    }
}
