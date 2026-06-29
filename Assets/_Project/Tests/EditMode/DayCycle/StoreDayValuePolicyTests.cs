using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.DayCycle;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.DayCycle
{
    public sealed class StoreDayValuePolicyTests
    {
        [Test] public void DayId_RejectsEmpty() =>
            Assert.Throws<System.ArgumentException>(
                () => new StoreDayId(" "));

        [Test] public void DayId_RejectsNull() =>
            Assert.Throws<System.ArgumentException>(
                () => new StoreDayId(null));

        [Test] public void DayId_UsesOrdinalEquality()
        {
            Assert.That(
                new StoreDayId("A") ==
                new StoreDayId("A"),
                Is.True);
        }

        [Test] public void DayId_IsCaseSensitive()
        {
            Assert.That(
                new StoreDayId("A") ==
                new StoreDayId("a"),
                Is.False);
        }

        [Test] public void DayId_InequalityWorks()
        {
            Assert.That(
                new StoreDayId("a") !=
                new StoreDayId("b"),
                Is.True);
        }

        [Test] public void DayId_ToStringReturnsValue()
        {
            Assert.That(
                new StoreDayId("day").ToString(),
                Is.EqualTo("day"));
        }

        [Test] public void DayId_HashMatchesEquality()
        {
            var left = new StoreDayId("day");
            var right = new StoreDayId("day");
            Assert.That(
                left.GetHashCode(),
                Is.EqualTo(right.GetHashCode()));
        }

        [Test] public void Policy_RejectsZeroDuration() =>
            Assert.Throws<
                System.ArgumentOutOfRangeException>(
                () => new StoreDayPolicy(0, true));

        [Test] public void Policy_RejectsNegativeDuration() =>
            Assert.Throws<
                System.ArgumentOutOfRangeException>(
                () => new StoreDayPolicy(-1, true));

        [Test] public void Policy_StoresDuration()
        {
            var policy = new StoreDayPolicy(300, true);
            Assert.That(
                policy.OpenDurationSeconds,
                Is.EqualTo(300));
        }

        [Test] public void Policy_StoresAutoCloseTrue()
        {
            var policy = new StoreDayPolicy(300, true);
            Assert.That(
                policy.AutoBeginClosing,
                Is.True);
        }

        [Test] public void Policy_StoresAutoCloseFalse()
        {
            var policy = new StoreDayPolicy(300, false);
            Assert.That(
                policy.AutoBeginClosing,
                Is.False);
        }

        [Test] public void Day_StartsBeforeOpen()
        {
            StoreDay day =
                DayCycleTestFactory.Day(open: false);
            Assert.That(
                day.State,
                Is.EqualTo(StoreDayState.BeforeOpen));
        }

        [Test] public void Day_StartsWithZeroElapsed()
        {
            StoreDay day =
                DayCycleTestFactory.Day(open: false);
            Assert.That(
                day.ElapsedOpenSeconds,
                Is.EqualTo(0));
        }

        [Test] public void Day_RemainingEqualsDurationInitially()
        {
            StoreDay day =
                DayCycleTestFactory.Day(
                    duration: 90,
                    open: false);
            Assert.That(
                day.RemainingOpenSeconds,
                Is.EqualTo(90));
        }

        [Test] public void BeforeOpen_DoesNotAcceptCustomers()
        {
            StoreDay day =
                DayCycleTestFactory.Day(open: false);
            Assert.That(
                day.CanAcceptCustomers,
                Is.False);
        }
    }
}
