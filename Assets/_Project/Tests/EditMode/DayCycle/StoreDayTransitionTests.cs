using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.DayCycle;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.DayCycle
{
    public sealed class StoreDayTransitionTests
    {
        [Test] public void Open_TransitionsFromBeforeOpen()
        {
            StoreDay day =
                DayCycleTestFactory.Day(open: false);
            StoreDayTransitionResult result =
                day.TryOpen();
            Assert.That(result.Succeeded, Is.True);
            Assert.That(
                day.State,
                Is.EqualTo(StoreDayState.Open));
        }

        [Test] public void Open_EnablesCustomerAdmission()
        {
            StoreDay day =
                DayCycleTestFactory.Day(open: false);
            day.TryOpen();
            Assert.That(
                day.CanAcceptCustomers,
                Is.True);
        }

        [Test] public void Open_CannotRunTwice()
        {
            StoreDay day =
                DayCycleTestFactory.Day(open: true);
            Assert.That(
                day.TryOpen().FailureReason,
                Is.EqualTo(
                    StoreDayTransitionFailureReason
                        .InvalidState));
        }

        [Test] public void Advance_AddsElapsedSeconds()
        {
            StoreDay day =
                DayCycleTestFactory.Day(duration: 60);
            day.Advance(12);
            Assert.That(
                day.ElapsedOpenSeconds,
                Is.EqualTo(12));
        }

        [Test] public void Advance_UpdatesRemainingSeconds()
        {
            StoreDay day =
                DayCycleTestFactory.Day(duration: 60);
            day.Advance(12);
            Assert.That(
                day.RemainingOpenSeconds,
                Is.EqualTo(48));
        }

        [Test] public void Advance_RejectsZero()
        {
            StoreDay day =
                DayCycleTestFactory.Day();
            Assert.That(
                day.Advance(0).FailureReason,
                Is.EqualTo(
                    StoreDayTransitionFailureReason
                        .InvalidElapsedSeconds));
        }

        [Test] public void Advance_RejectsNegative()
        {
            StoreDay day =
                DayCycleTestFactory.Day();
            Assert.That(
                day.Advance(-1).FailureReason,
                Is.EqualTo(
                    StoreDayTransitionFailureReason
                        .InvalidElapsedSeconds));
        }

        [Test] public void Advance_RejectsBeforeOpen()
        {
            StoreDay day =
                DayCycleTestFactory.Day(open: false);
            Assert.That(
                day.Advance(1).FailureReason,
                Is.EqualTo(
                    StoreDayTransitionFailureReason
                        .InvalidState));
        }

        [Test] public void AutoClose_BeginsAtDuration()
        {
            StoreDay day =
                DayCycleTestFactory.Day(
                    duration: 10,
                    autoClose: true);
            StoreDayTransitionResult result =
                day.Advance(10);
            Assert.That(result.Succeeded, Is.True);
            Assert.That(result.ClosingStarted, Is.True);
            Assert.That(
                day.State,
                Is.EqualTo(StoreDayState.Closing));
        }

        [Test] public void AutoClose_ClampsElapsedToDuration()
        {
            StoreDay day =
                DayCycleTestFactory.Day(
                    duration: 10,
                    autoClose: true);
            day.Advance(50);
            Assert.That(
                day.ElapsedOpenSeconds,
                Is.EqualTo(10));
        }

        [Test] public void ManualPolicy_DoesNotAutoClose()
        {
            StoreDay day =
                DayCycleTestFactory.Day(
                    duration: 10,
                    autoClose: false);
            day.Advance(10);
            Assert.That(
                day.State,
                Is.EqualTo(StoreDayState.Open));
        }

        [Test] public void ManualClose_TransitionsFromOpen()
        {
            StoreDay day =
                DayCycleTestFactory.Day();
            StoreDayTransitionResult result =
                day.TryBeginClosing();
            Assert.That(result.Succeeded, Is.True);
            Assert.That(
                day.State,
                Is.EqualTo(StoreDayState.Closing));
        }

        [Test] public void Closing_BlocksCustomerAdmission()
        {
            StoreDay day =
                DayCycleTestFactory.Day();
            day.TryBeginClosing();
            Assert.That(
                day.CanAcceptCustomers,
                Is.False);
        }

        [Test] public void Close_CannotBeginBeforeOpen()
        {
            StoreDay day =
                DayCycleTestFactory.Day(open: false);
            Assert.That(
                day.TryBeginClosing().FailureReason,
                Is.EqualTo(
                    StoreDayTransitionFailureReason
                        .InvalidState));
        }

        [Test] public void CompleteClosing_RequiresConditions()
        {
            StoreDay day =
                DayCycleTestFactory.Day();
            day.TryBeginClosing();
            Assert.That(
                day.TryCompleteClosing(false)
                    .FailureReason,
                Is.EqualTo(
                    StoreDayTransitionFailureReason
                        .ClosingConditionsNotMet));
        }

        [Test] public void CompleteClosing_TransitionsToClosed()
        {
            StoreDay day =
                DayCycleTestFactory.Day();
            day.TryBeginClosing();
            Assert.That(
                day.TryCompleteClosing(true).Succeeded,
                Is.True);
            Assert.That(
                day.State,
                Is.EqualTo(StoreDayState.Closed));
        }

        [Test] public void Closed_DoesNotAcceptCustomers()
        {
            StoreDay day =
                DayCycleTestFactory.Day();
            day.TryBeginClosing();
            day.TryCompleteClosing(true);
            Assert.That(
                day.CanAcceptCustomers,
                Is.False);
        }

        [Test] public void Closed_CannotAdvance()
        {
            StoreDay day =
                DayCycleTestFactory.Day();
            day.TryBeginClosing();
            day.TryCompleteClosing(true);
            Assert.That(
                day.Advance(1).FailureReason,
                Is.EqualTo(
                    StoreDayTransitionFailureReason
                        .InvalidState));
        }

        [Test] public void Closed_CannotCloseAgain()
        {
            StoreDay day =
                DayCycleTestFactory.Day();
            day.TryBeginClosing();
            day.TryCompleteClosing(true);
            Assert.That(
                day.TryCompleteClosing(true)
                    .FailureReason,
                Is.EqualTo(
                    StoreDayTransitionFailureReason
                        .InvalidState));
        }
    }
}
