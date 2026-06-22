using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.SceneFlow;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode
{
    public sealed class SceneTransitionGateTests
    {
        [Test]
        public void TryEnter_WhenAlreadyEntered_RejectsConcurrentEntry()
        {
            SceneTransitionGate gate = new SceneTransitionGate();

            bool firstEntry = gate.TryEnter();
            bool concurrentEntry = gate.TryEnter();

            Assert.That(firstEntry, Is.True);
            Assert.That(concurrentEntry, Is.False);
            Assert.That(gate.IsEntered, Is.True);

            gate.Exit();

            Assert.That(gate.IsEntered, Is.False);
            Assert.That(gate.TryEnter(), Is.True);
        }
    }
}
