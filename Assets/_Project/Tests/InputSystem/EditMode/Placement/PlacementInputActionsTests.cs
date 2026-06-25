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
                actions.RotatePlacementCounterClockwise.actionMap,
                Is.SameAs(actions.Gameplay));

            Assert.That(
                actions.RotatePlacementClockwise.actionMap,
                Is.SameAs(actions.Gameplay));
        }

        [Test]
        public void PlacementActions_UseQAndEBindings()
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
    }
}
