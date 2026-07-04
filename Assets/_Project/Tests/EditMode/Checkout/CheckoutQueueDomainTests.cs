using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.Checkout;
using VRMGames.CartridgeAndCloud.Domain.Customers;
using VRMGames.CartridgeAndCloud.Domain.Shopping;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Checkout
{
    public sealed class CheckoutQueueDomainTests
    {
        private static CheckoutQueueEntry Entry(
            string id,
            string customer,
            string cart)
        {
            return new CheckoutQueueEntry(
                new CheckoutQueueEntryId(id),
                new CustomerInstanceId(customer),
                new ShoppingCartId(cart));
        }

        private static CheckoutQueueEntry Create()
        {
            return new CheckoutQueueEntry(
                new CheckoutQueueEntryId("entry"),
                new CustomerInstanceId("customer"),
                new ShoppingCartId("cart"));
        }

        [Test]
        public void Entry_StartsWaiting()
        {
            var entry = Create();
            Assert.That(
                entry.State,
                Is.EqualTo(CheckoutQueueEntryState.Waiting));
            Assert.That(entry.IsActive, Is.True);
        }

        [Test]
        public void Entry_CanBeCalled()
        {
            var entry = Create();
            Assert.That(entry.TryCall(), Is.True);
            Assert.That(
                entry.State,
                Is.EqualTo(CheckoutQueueEntryState.Called));
        }

        [Test]
        public void Entry_CannotBeCalledTwice()
        {
            var entry = Create();
            entry.TryCall();
            Assert.That(entry.TryCall(), Is.False);
        }

        [Test]
        public void CalledEntry_CanBeginProcessing()
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

        [Test]
        public void WaitingEntry_CannotBeginProcessing()
        {
            Assert.That(
                Create().TryBeginProcessing(),
                Is.False);
        }

        [Test]
        public void ProcessingEntry_CanComplete()
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

        [Test]
        public void WaitingEntry_CanCancel()
        {
            var entry = Create();
            Assert.That(entry.TryCancel(), Is.True);
            Assert.That(
                entry.State,
                Is.EqualTo(
                    CheckoutQueueEntryState.Cancelled));
        }

        [Test]
        public void CalledEntry_CanCancel()
        {
            var entry = Create();
            entry.TryCall();
            Assert.That(entry.TryCancel(), Is.True);
        }

        [Test]
        public void ProcessingEntry_CannotCancel()
        {
            var entry = Create();
            entry.TryCall();
            entry.TryBeginProcessing();
            Assert.That(entry.TryCancel(), Is.False);
        }

        [Test]
        public void CompletedEntry_CannotTransitionAgain()
        {
            var entry = Create();
            entry.TryCall();
            entry.TryBeginProcessing();
            entry.TryComplete();
            Assert.That(entry.TryCancel(), Is.False);
            Assert.That(entry.TryComplete(), Is.False);
        }

        [Test] public void Queue_RejectsZeroCapacity() =>
            Assert.Throws<System.ArgumentOutOfRangeException>(
                () => new CheckoutQueue(0));

        [Test] public void Queue_StartsEmpty()
        {
            var queue = new CheckoutQueue(3);
            Assert.That(queue.IsEmpty, Is.True);
            Assert.That(queue.ActiveCount, Is.EqualTo(0));
        }

        [Test] public void Queue_EnqueuesAtPositionOne()
        {
            var queue = new CheckoutQueue(3);
            var result = queue.TryEnqueue(
                Entry("e1", "c1", "cart1"));
            Assert.That(result.Succeeded, Is.True);
            Assert.That(result.Position, Is.EqualTo(1));
        }

        [Test] public void Queue_PreservesFifoPositions()
        {
            var queue = new CheckoutQueue(3);
            var first = Entry("e1", "c1", "cart1");
            var second = Entry("e2", "c2", "cart2");
            queue.TryEnqueue(first);
            queue.TryEnqueue(second);
            Assert.That(
                queue.GetPosition(first.Id),
                Is.EqualTo(1));
            Assert.That(
                queue.GetPosition(second.Id),
                Is.EqualTo(2));
        }

        [Test] public void Queue_RejectsWhenFull()
        {
            var queue = new CheckoutQueue(1);
            queue.TryEnqueue(
                Entry("e1", "c1", "cart1"));
            var result = queue.TryEnqueue(
                Entry("e2", "c2", "cart2"));
            Assert.That(
                result.FailureReason,
                Is.EqualTo(
                    CheckoutQueueFailureReason.QueueFull));
        }

        [Test] public void Queue_RejectsDuplicateEntryId()
        {
            var queue = new CheckoutQueue(3);
            queue.TryEnqueue(
                Entry("e1", "c1", "cart1"));
            var result = queue.TryEnqueue(
                Entry("e1", "c2", "cart2"));
            Assert.That(
                result.FailureReason,
                Is.EqualTo(
                    CheckoutQueueFailureReason
                        .DuplicateEntryId));
        }

        [Test] public void Queue_RejectsDuplicateCustomer()
        {
            var queue = new CheckoutQueue(3);
            queue.TryEnqueue(
                Entry("e1", "c1", "cart1"));
            var result = queue.TryEnqueue(
                Entry("e2", "c1", "cart2"));
            Assert.That(
                result.FailureReason,
                Is.EqualTo(
                    CheckoutQueueFailureReason
                        .CustomerAlreadyQueued));
        }

        [Test] public void Queue_RejectsDuplicateCart()
        {
            var queue = new CheckoutQueue(3);
            queue.TryEnqueue(
                Entry("e1", "c1", "cart1"));
            var result = queue.TryEnqueue(
                Entry("e2", "c2", "cart1"));
            Assert.That(
                result.FailureReason,
                Is.EqualTo(
                    CheckoutQueueFailureReason
                        .CartAlreadyQueued));
        }

        [Test] public void Queue_CallsFirstEntry()
        {
            var queue = new CheckoutQueue(3);
            var first = Entry("e1", "c1", "cart1");
            queue.TryEnqueue(first);
            queue.TryEnqueue(
                Entry("e2", "c2", "cart2"));
            var result = queue.TryCallNext();
            Assert.That(result.Entry, Is.SameAs(first));
            Assert.That(
                first.State,
                Is.EqualTo(
                    CheckoutQueueEntryState.Called));
        }

        [Test] public void Queue_CannotCallWhileFrontBusy()
        {
            var queue = new CheckoutQueue(3);
            queue.TryEnqueue(
                Entry("e1", "c1", "cart1"));
            queue.TryCallNext();
            Assert.That(
                queue.TryCallNext().FailureReason,
                Is.EqualTo(
                    CheckoutQueueFailureReason
                        .CurrentEntryBusy));
        }

        [Test] public void Queue_BeginsOnlyFrontEntry()
        {
            var queue = new CheckoutQueue(3);
            var first = Entry("e1", "c1", "cart1");
            var second = Entry("e2", "c2", "cart2");
            queue.TryEnqueue(first);
            queue.TryEnqueue(second);
            queue.TryCallNext();
            Assert.That(
                queue.TryBeginProcessing(second.Id)
                    .FailureReason,
                Is.EqualTo(
                    CheckoutQueueFailureReason
                        .EntryNotAtFront));
        }

        [Test] public void Queue_CompletesAndRemovesFront()
        {
            var queue = new CheckoutQueue(3);
            var first = Entry("e1", "c1", "cart1");
            var second = Entry("e2", "c2", "cart2");
            queue.TryEnqueue(first);
            queue.TryEnqueue(second);
            queue.TryCallNext();
            queue.TryBeginProcessing(first.Id);
            var result =
                queue.TryCompleteCurrent(first.Id);
            Assert.That(result.Succeeded, Is.True);
            Assert.That(queue.ActiveCount, Is.EqualTo(1));
            Assert.That(
                queue.CurrentEntry,
                Is.SameAs(second));
        }

        [Test] public void Queue_CancelUpdatesFollowingPositions()
        {
            var queue = new CheckoutQueue(3);
            var first = Entry("e1", "c1", "cart1");
            var second = Entry("e2", "c2", "cart2");
            queue.TryEnqueue(first);
            queue.TryEnqueue(second);
            queue.TryCancel(first.Id);
            Assert.That(
                queue.GetPosition(second.Id),
                Is.EqualTo(1));
        }

        [Test] public void Queue_CannotCancelProcessingEntry()
        {
            var queue = new CheckoutQueue(3);
            var first = Entry("e1", "c1", "cart1");
            queue.TryEnqueue(first);
            queue.TryCallNext();
            queue.TryBeginProcessing(first.Id);
            Assert.That(
                queue.TryCancel(first.Id).FailureReason,
                Is.EqualTo(
                    CheckoutQueueFailureReason
                        .InvalidEntryState));
        }

        [Test] public void Queue_ReturnsZeroForMissingPosition()
        {
            var queue = new CheckoutQueue(3);
            Assert.That(
                queue.GetPosition(
                    new CheckoutQueueEntryId("missing")),
                Is.EqualTo(0));
        }

        [Test] public void Queue_SnapshotUsesFifoOrder()
        {
            var queue = new CheckoutQueue(3);
            queue.TryEnqueue(
                Entry("e1", "c1", "cart1"));
            queue.TryEnqueue(
                Entry("e2", "c2", "cart2"));
            Assert.That(
                queue.ActiveEntries[0].Id.Value,
                Is.EqualTo("e1"));
            Assert.That(
                queue.ActiveEntries[1].Id.Value,
                Is.EqualTo("e2"));
        }
    }
}
