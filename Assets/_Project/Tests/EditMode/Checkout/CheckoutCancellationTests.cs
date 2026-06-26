using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.Checkout;
using VRMGames.CartridgeAndCloud.Domain.Checkout;
using VRMGames.CartridgeAndCloud.Domain.Shopping;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Checkout
{
    public sealed class CheckoutCancellationTests
    {
        [Test] public void CancelWaitingEntry_ReleasesReservations()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario(
                    reservationCount: 2,
                    processEntry: false);
            var service =
                new CheckoutCancellationService(
                    CheckoutTestFactory.CartService(
                        scenario),
                    scenario.Reservations);

            CheckoutCancellationResult result =
                service.TryCancel(
                    scenario.Queue,
                    scenario.Entry.Id,
                    scenario.Session,
                    scenario.Displays);

            Assert.That(result.Succeeded, Is.True);
            Assert.That(
                result.ReleasedReservations,
                Is.EqualTo(2));
            Assert.That(scenario.Cart.IsEmpty, Is.True);
        }

        [Test] public void CancelWaitingEntry_AbandonsSession()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario(
                    processEntry: false);
            var service =
                new CheckoutCancellationService(
                    CheckoutTestFactory.CartService(
                        scenario),
                    scenario.Reservations);

            service.TryCancel(
                scenario.Queue,
                scenario.Entry.Id,
                scenario.Session,
                scenario.Displays);

            Assert.That(
                scenario.Session.State,
                Is.EqualTo(
                    CustomerShoppingState.Abandoned));
        }

        [Test] public void CancelWaitingEntry_RemovesQueueEntry()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario(
                    processEntry: false);
            var service =
                new CheckoutCancellationService(
                    CheckoutTestFactory.CartService(
                        scenario),
                    scenario.Reservations);

            service.TryCancel(
                scenario.Queue,
                scenario.Entry.Id,
                scenario.Session,
                scenario.Displays);

            Assert.That(scenario.Queue.IsEmpty, Is.True);
            Assert.That(
                scenario.Entry.State,
                Is.EqualTo(
                    CheckoutQueueEntryState.Cancelled));
        }

        [Test] public void CancelCalledEntry_IsAllowed()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario(
                    processEntry: false);
            scenario.QueueService.TryCallNext(
                scenario.Queue);
            var service =
                new CheckoutCancellationService(
                    CheckoutTestFactory.CartService(
                        scenario),
                    scenario.Reservations);

            Assert.That(
                service.TryCancel(
                    scenario.Queue,
                    scenario.Entry.Id,
                    scenario.Session,
                    scenario.Displays).Succeeded,
                Is.True);
        }

        [Test] public void CancelProcessingEntry_IsRejected()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario();
            var service =
                new CheckoutCancellationService(
                    CheckoutTestFactory.CartService(
                        scenario),
                    scenario.Reservations);

            CheckoutCancellationResult result =
                service.TryCancel(
                    scenario.Queue,
                    scenario.Entry.Id,
                    scenario.Session,
                    scenario.Displays);

            Assert.That(result.Succeeded, Is.False);
            Assert.That(
                result.FailureReason,
                Is.EqualTo(
                    CheckoutCancellationFailureReason
                        .EntryProcessing));
            Assert.That(
                scenario.Cart.TotalUnits,
                Is.EqualTo(1));
        }

        [Test] public void CancelMissingEntry_IsRejected()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario(
                    processEntry: false);
            var service =
                new CheckoutCancellationService(
                    CheckoutTestFactory.CartService(
                        scenario),
                    scenario.Reservations);

            Assert.That(
                service.TryCancel(
                    scenario.Queue,
                    new CheckoutQueueEntryId("missing"),
                    scenario.Session,
                    scenario.Displays).FailureReason,
                Is.EqualTo(
                    CheckoutCancellationFailureReason
                        .EntryNotFound));
        }

        [Test] public void Cancellation_RestoresAvailability()
        {
            CheckoutTestScenario scenario =
                CheckoutTestFactory.ReadyScenario(
                    stock: 2,
                    processEntry: false);
            var service =
                new CheckoutCancellationService(
                    CheckoutTestFactory.CartService(
                        scenario),
                    scenario.Reservations);

            service.TryCancel(
                scenario.Queue,
                scenario.Entry.Id,
                scenario.Session,
                scenario.Displays);

            ShoppingReservation reservation =
                scenario.Reservations.GetForCustomer(
                    scenario.Session.CustomerId,
                    activeOnly: false)[0];
            Assert.That(
                reservation.State,
                Is.EqualTo(
                    ShoppingReservationState.Released));
            Assert.That(
                scenario.Display.Inventory.GetQuantity(
                    scenario.Product.Id).Value,
                Is.EqualTo(2));
        }
    }
}
