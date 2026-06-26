using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.Checkout;
using VRMGames.CartridgeAndCloud.Domain.Customers;
using VRMGames.CartridgeAndCloud.Domain.Shopping;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Checkout
{
    public sealed class CheckoutQueueEntryTests
    {
        private static CheckoutQueueEntry Create()
        {
            return new CheckoutQueueEntry(
                new CheckoutQueueEntryId("entry"),
                new CustomerInstanceId("customer"),
                new ShoppingCartId("cart"));
        }

        [Test] public void Entry_StartsWaiting()
        {
            var entry = Create();
            Assert.That(
                entry.State,
                Is.EqualTo(CheckoutQueueEntryState.Waiting));
            Assert.That(entry.IsActive, Is.True);
        }

        [Test] public void Entry_CanBeCalled()
        {
            var entry = Create();
            Assert.That(entry.TryCall(), Is.True);
            Assert.That(
                entry.State,
                Is.EqualTo(CheckoutQueueEntryState.Called));
        }

        [Test] public void Entry_CannotBeCalledTwice()
        {
            var entry = Create();
            entry.TryCall();
            Assert.That(entry.TryCall(), Is.False);
        }

        [Test] public void CalledEntry_CanBeginProcessing()
        {
            var entry = Create();
            entry.TryCall();
            Assert.That(
                entry.TryBeginProcessing(),
                Is.True);
            Assert.That(
                entry.State,
                Is.EqualTo(
                    CheckoutQueueEntryState.Processing));
        }

        [Test] public void WaitingEntry_CannotBeginProcessing()
        {
            Assert.That(
                Create().TryBeginProcessing(),
                Is.False);
        }

        [Test] public void ProcessingEntry_CanComplete()
        {
            var entry = Create();
            entry.TryCall();
            entry.TryBeginProcessing();
            Assert.That(entry.TryComplete(), Is.True);
            Assert.That(
                entry.State,
                Is.EqualTo(
                    CheckoutQueueEntryState.Completed));
            Assert.That(entry.IsActive, Is.False);
        }

        [Test] public void WaitingEntry_CanCancel()
        {
            var entry = Create();
            Assert.That(entry.TryCancel(), Is.True);
            Assert.That(
                entry.State,
                Is.EqualTo(
                    CheckoutQueueEntryState.Cancelled));
        }

        [Test] public void CalledEntry_CanCancel()
        {
            var entry = Create();
            entry.TryCall();
            Assert.That(entry.TryCancel(), Is.True);
        }

        [Test] public void ProcessingEntry_CannotCancel()
        {
            var entry = Create();
            entry.TryCall();
            entry.TryBeginProcessing();
            Assert.That(entry.TryCancel(), Is.False);
        }

        [Test] public void CompletedEntry_CannotTransitionAgain()
        {
            var entry = Create();
            entry.TryCall();
            entry.TryBeginProcessing();
            entry.TryComplete();
            Assert.That(entry.TryCancel(), Is.False);
            Assert.That(entry.TryComplete(), Is.False);
        }
    }
}
