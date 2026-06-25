using System.Linq;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Infrastructure.InputSystem.Actions;

namespace VRMGames.CartridgeAndCloud.InputSystem.Tests.EditMode
{
    public sealed class PlacementModeInputActionsTests
    {
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
