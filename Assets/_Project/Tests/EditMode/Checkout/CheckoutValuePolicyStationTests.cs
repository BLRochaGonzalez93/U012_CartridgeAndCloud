using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.Checkout;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Checkout
{
    public sealed class CheckoutValuePolicyStationTests
    {
        [Test] public void QueueEntryId_RejectsEmpty() =>
            Assert.Throws<System.ArgumentException>(
                () => new CheckoutQueueEntryId(" "));

        [Test] public void StationId_RejectsEmpty() =>
            Assert.Throws<System.ArgumentException>(
                () => new CheckoutStationId(""));

        [Test] public void TransactionId_RejectsNull() =>
            Assert.Throws<System.ArgumentException>(
                () => new CheckoutTransactionId(null));

        [Test] public void QueueEntryId_UsesOrdinalEquality()
        {
            Assert.That(
                new CheckoutQueueEntryId("A") ==
                new CheckoutQueueEntryId("A"),
                Is.True);
        }

        [Test] public void QueueEntryId_IsCaseSensitive()
        {
            Assert.That(
                new CheckoutQueueEntryId("A") ==
                new CheckoutQueueEntryId("a"),
                Is.False);
        }

        [Test] public void StationId_ToStringReturnsValue()
        {
            Assert.That(
                new CheckoutStationId("station").ToString(),
                Is.EqualTo("station"));
        }

        [Test] public void TransactionId_HashMatchesEquality()
        {
            var left =
                new CheckoutTransactionId("transaction");
            var right =
                new CheckoutTransactionId("transaction");
            Assert.That(
                left.GetHashCode(),
                Is.EqualTo(right.GetHashCode()));
        }

        [Test] public void Policy_RejectsZeroCapacity() =>
            Assert.Throws<System.ArgumentOutOfRangeException>(
                () => new CheckoutPolicy(0));

        [Test] public void Policy_StoresCapacity()
        {
            Assert.That(
                new CheckoutPolicy(6).MaxQueueLength,
                Is.EqualTo(6));
        }

        [Test] public void Station_StartsClosed()
        {
            var station = new CheckoutStation(
                new CheckoutStationId("station"));
            Assert.That(
                station.State,
                Is.EqualTo(CheckoutStationState.Closed));
        }

        [Test] public void Station_Opens()
        {
            var station = new CheckoutStation(
                new CheckoutStationId("station"));
            Assert.That(station.TryOpen(), Is.True);
            Assert.That(
                station.State,
                Is.EqualTo(CheckoutStationState.Available));
        }

        [Test] public void Station_CannotOpenTwice()
        {
            var station = new CheckoutStation(
                new CheckoutStationId("station"));
            station.TryOpen();
            Assert.That(station.TryOpen(), Is.False);
        }

        [Test] public void Station_BeginsProcessing()
        {
            var station = new CheckoutStation(
                new CheckoutStationId("station"));
            station.TryOpen();
            var entryId =
                new CheckoutQueueEntryId("entry");
            Assert.That(
                station.TryBeginProcessing(entryId),
                Is.True);
            Assert.That(station.IsBusy, Is.True);
            Assert.That(
                station.CurrentEntryId,
                Is.EqualTo(entryId));
        }

        [Test] public void Station_RejectsMismatchedCompletion()
        {
            var station = new CheckoutStation(
                new CheckoutStationId("station"));
            station.TryOpen();
            station.TryBeginProcessing(
                new CheckoutQueueEntryId("entry"));
            Assert.That(
                station.TryCompleteProcessing(
                    new CheckoutQueueEntryId("other")),
                Is.False);
        }

        [Test] public void Station_CompletesAndBecomesAvailable()
        {
            var station = new CheckoutStation(
                new CheckoutStationId("station"));
            var entryId =
                new CheckoutQueueEntryId("entry");
            station.TryOpen();
            station.TryBeginProcessing(entryId);
            Assert.That(
                station.TryCompleteProcessing(entryId),
                Is.True);
            Assert.That(
                station.State,
                Is.EqualTo(CheckoutStationState.Available));
        }

        [Test] public void BusyStation_CannotClose()
        {
            var station = new CheckoutStation(
                new CheckoutStationId("station"));
            station.TryOpen();
            station.TryBeginProcessing(
                new CheckoutQueueEntryId("entry"));
            Assert.That(station.TryClose(), Is.False);
        }
    }
}
