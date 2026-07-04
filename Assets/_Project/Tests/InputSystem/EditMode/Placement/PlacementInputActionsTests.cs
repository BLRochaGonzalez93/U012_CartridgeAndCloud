using System.Linq;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Infrastructure.InputSystem.Actions;

namespace VRMGames.CartridgeAndCloud.InputSystem.Tests.EditMode
{
    public sealed class PlacementInputActionsTests
    {
        [Test]
        public void PlacementActions_AreInGameplayMap()
        {
            using ProjectInputActions actions =
                new ProjectInputActions();

            Assert.That(
                actions.TogglePlacementMode.actionMap,
                Is.SameAs(actions.Gameplay));

            Assert.That(
                actions.RotatePlacementCounterClockwise.actionMap,
                Is.SameAs(actions.Gameplay));

            Assert.That(
                actions.RotatePlacementClockwise.actionMap,
                Is.SameAs(actions.Gameplay));

            Assert.That(
                actions.CancelPlacement.actionMap,
                Is.SameAs(actions.Gameplay));

            Assert.That(
                actions.RemovePlacement.actionMap,
                Is.SameAs(actions.Gameplay));
        }

        [Test]
        public void PlacementRotationActions_UseQAndEBindings()
        {
            using ProjectInputActions actions =
                new ProjectInputActions();

            Assert.That(
                actions.RotatePlacementCounterClockwise
                    .bindings.Single().path,
                Is.EqualTo("<Keyboard>/q"));

            Assert.That(
                actions.RotatePlacementClockwise
                    .bindings.Single().path,
                Is.EqualTo("<Keyboard>/e"));
        }

        [Test]
        public void TogglePlacementMode_UsesB()
        {
            using ProjectInputActions actions =
                new ProjectInputActions();

            Assert.That(
                actions.TogglePlacementMode
                    .bindings.Single().path,
                Is.EqualTo("<Keyboard>/b"));
        }

        [Test]
        public void CancelPlacement_UsesEscape()
        {
            using ProjectInputActions actions =
                new ProjectInputActions();

            Assert.That(
                actions.CancelPlacement
                    .bindings.Single().path,
                Is.EqualTo("<Keyboard>/escape"));
        }

        [Test]
        public void RemovePlacement_UsesDeleteAndBackspace()
        {
            using ProjectInputActions actions =
                new ProjectInputActions();

            string[] paths =
                actions.RemovePlacement
                    .bindings
                    .Select(binding => binding.path)
                    .ToArray();

            CollectionAssert.AreEquivalent(
                new[]
                {
                    "<Keyboard>/delete",
                    "<Keyboard>/backspace"
                },
                paths);
        }
    }
}
