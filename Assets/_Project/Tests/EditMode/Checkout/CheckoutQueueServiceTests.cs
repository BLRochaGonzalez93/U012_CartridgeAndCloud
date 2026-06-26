using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.Checkout;
using VRMGames.CartridgeAndCloud.Domain.Checkout;
using VRMGames.CartridgeAndCloud.Domain.Customers;
using VRMGames.CartridgeAndCloud.Domain.Shopping;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Checkout
{
    public sealed class CheckoutQueueServiceTests
    {
        [Test] public void Service_CreatesQueueFromPolicy()
        {
            var service =
                new CheckoutQueueService(
                    new CheckoutPolicy(4));
            Assert.That(
                service.CreateQueue().Capacity,
                Is.EqualTo(4));
        }

        [Test] public void Enqueue_RequiresReadySession()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario(
                    processEntry: false);
            scenario.Session.TryAbandon();

            var result = scenario.QueueService.TryEnqueue(
                new CheckoutQueue(6),
                new CheckoutQueueEntryId("other"),
                scenario.Session);

            Assert.That(
                result.FailureReason,
                Is.EqualTo(
                    CheckoutQueueServiceFailureReason
                        .SessionNotReady));
        }

        [Test] public void Enqueue_RequiresNonEmptyCart()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario(
                    processEntry: false);
            foreach (ShoppingCartLine line
                     in scenario.Cart.Lines)
            {
                scenario.Cart.TryRemove(
                    line.ReservationId);
            }

            var result = scenario.QueueService.TryEnqueue(
                new CheckoutQueue(6),
                new CheckoutQueueEntryId("other"),
                scenario.Session);

            Assert.That(
                result.FailureReason,
                Is.EqualTo(
                    CheckoutQueueServiceFailureReason
                        .EmptyCart));
        }

        [Test] public void Enqueue_ReturnsPosition()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario(
                    processEntry: false);
            Assert.That(
                scenario.Queue.GetPosition(
                    scenario.Entry.Id),
                Is.EqualTo(1));
        }

        [Test] public void CallNext_TransitionsFront()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario(
                    processEntry: false);
            var result =
                scenario.QueueService.TryCallNext(
                    scenario.Queue);
            Assert.That(result.Succeeded, Is.True);
            Assert.That(
                result.Entry.State,
                Is.EqualTo(
                    CheckoutQueueEntryState.Called));
        }

        [Test] public void BeginProcessing_RequiresOpenStation()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario(
                    processEntry: false);
            scenario.QueueService.TryCallNext(
                scenario.Queue);
            scenario.Station.TryClose();

            var result =
                scenario.QueueService.TryBeginProcessing(
                    scenario.Queue,
                    scenario.Station,
                    scenario.Entry.Id);

            Assert.That(
                result.FailureReason,
                Is.EqualTo(
                    CheckoutQueueServiceFailureReason
                        .StationUnavailable));
        }

        [Test] public void BeginProcessing_RejectsBusyStation()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario(
                    processEntry: false);
            scenario.QueueService.TryCallNext(
                scenario.Queue);
            scenario.Station.TryBeginProcessing(
                new CheckoutQueueEntryId("other"));

            var result =
                scenario.QueueService.TryBeginProcessing(
                    scenario.Queue,
                    scenario.Station,
                    scenario.Entry.Id);

            Assert.That(
                result.FailureReason,
                Is.EqualTo(
                    CheckoutQueueServiceFailureReason
                        .StationBusy));
        }

        [Test] public void BeginProcessing_AssignsSameEntry()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario(
                    processEntry: false);
            scenario.QueueService.TryCallNext(
                scenario.Queue);

            var result =
                scenario.QueueService.TryBeginProcessing(
                    scenario.Queue,
                    scenario.Station,
                    scenario.Entry.Id);

            Assert.That(result.Succeeded, Is.True);
            Assert.That(
                scenario.Station.CurrentEntryId,
                Is.EqualTo(scenario.Entry.Id));
            Assert.That(
                scenario.Entry.State,
                Is.EqualTo(
                    CheckoutQueueEntryState.Processing));
        }

        [Test] public void Cancel_DelegatesToQueue()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario(
                    processEntry: false);

            var result =
                scenario.QueueService.TryCancel(
                    scenario.Queue,
                    scenario.Entry.Id);

            Assert.That(result.Succeeded, Is.True);
            Assert.That(scenario.Queue.IsEmpty, Is.True);
        }

        [Test] public void CallNext_EmptyQueueFails()
        {
            var service =
                new CheckoutQueueService(
                    new CheckoutPolicy(2));
            var result =
                service.TryCallNext(
                    service.CreateQueue());

            Assert.That(
                result.QueueFailureReason,
                Is.EqualTo(
                    CheckoutQueueFailureReason.QueueEmpty));
        }
    }
}
