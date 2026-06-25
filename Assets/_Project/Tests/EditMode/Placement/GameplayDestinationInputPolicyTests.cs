using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.Placement;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode
{
    public sealed class GameplayDestinationInputPolicyTests
    {
        [Test]
        public void ShouldSetDestination_NormalGameplayClick_ReturnsTrue()
        {
            Assert.That(
                GameplayDestinationInputPolicy
                    .ShouldSetDestination(
                        destinationPressed: true,
                        placementModeActive: false),
                Is.True);
        }

        [Test]
        public void ShouldSetDestination_PlacementMode_ReturnsFalse()
        {
            Assert.That(
                GameplayDestinationInputPolicy
                    .ShouldSetDestination(
                        destinationPressed: true,
                        placementModeActive: true),
                Is.False);
        }
    }
}
