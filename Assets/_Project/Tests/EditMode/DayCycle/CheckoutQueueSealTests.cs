using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.Checkout;
using VRMGames.CartridgeAndCloud.Domain.Customers;
using VRMGames.CartridgeAndCloud.Domain.Shopping;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.DayCycle
{
    public sealed class CheckoutQueueSealTests
    {
        private static CheckoutQueueEntry Entry(
            string suffix = "a")
        {
            return new CheckoutQueueEntry(
                new CheckoutQueueEntryId(
                    "entry-" + suffix),
                new CustomerInstanceId(
                    "customer-" + suffix),
                new ShoppingCartId(
                    "cart-" + suffix));
        }

        [Test] public void Queue_StartsAcceptingEntries()
        {
            Assert.That(
                new CheckoutQueue(3).IsAcceptingEntries,
                Is.True);
        }

        [Test] public void Seal_StopsAdmissions()
        {
            var queue = new CheckoutQueue(3);
            Assert.That(queue.TrySeal(), Is.True);
            Assert.That(
                queue.IsAcceptingEntries,
                Is.False);
        }

        [Test] public void Seal_IsIdempotentFalseOnSecondCall()
        {
            var queue = new CheckoutQueue(3);
            queue.TrySeal();
            Assert.That(queue.TrySeal(), Is.False);
        }

        [Test] public void SealedQueue_RejectsNewEntry()
        {
            var queue = new CheckoutQueue(3);
            queue.TrySeal();
            CheckoutQueueResult result =
                queue.TryEnqueue(Entry());
            Assert.That(result.Succeeded, Is.False);
            Assert.That(
                result.FailureReason,
                Is.EqualTo(
                    CheckoutQueueFailureReason.QueueSealed));
        }

        [Test] public void Seal_PreservesExistingEntries()
        {
            var queue = new CheckoutQueue(3);
            queue.TryEnqueue(Entry());
            queue.TrySeal();
            Assert.That(queue.ActiveCount, Is.EqualTo(1));
        }

        [Test] public void SealedQueue_CanCallExistingEntry()
        {
            var queue = new CheckoutQueue(3);
            queue.TryEnqueue(Entry());
            queue.TrySeal();
            Assert.That(
                queue.TryCallNext().Succeeded,
                Is.True);
        }

        [Test] public void SealedQueue_CanCompleteExistingEntry()
        {
            var queue = new CheckoutQueue(3);
            CheckoutQueueEntry entry = Entry();
            queue.TryEnqueue(entry);
            queue.TrySeal();
            queue.TryCallNext();
            queue.TryBeginProcessing(entry.Id);
            Assert.That(
                queue.TryCompleteCurrent(entry.Id)
                    .Succeeded,
                Is.True);
        }

        [Test] public void SealedQueue_CanCancelExistingEntry()
        {
            var queue = new CheckoutQueue(3);
            CheckoutQueueEntry entry = Entry();
            queue.TryEnqueue(entry);
            queue.TrySeal();
            Assert.That(
                queue.TryCancel(entry.Id).Succeeded,
                Is.True);
        }

        [Test] public void QueueService_ReportsSealedQueue()
        {
            DayCycleTestScenario scenario =
                DayCycleTestFactory.Scenario(
                    addReadySession: true,
                    enqueue: false);
            scenario.Queue.TrySeal();

            var result =
                new VRMGames.CartridgeAndCloud.Application.Checkout
                    .CheckoutQueueService(
                        new CheckoutPolicy(8))
                    .TryEnqueue(
                        scenario.Queue,
                        new CheckoutQueueEntryId("entry"),
                        scenario.Sessions.Sessions[0]);

            Assert.That(result.Succeeded, Is.False);
            Assert.That(
                result.QueueFailureReason,
                Is.EqualTo(
                    CheckoutQueueFailureReason.QueueSealed));
        }
    }
}
