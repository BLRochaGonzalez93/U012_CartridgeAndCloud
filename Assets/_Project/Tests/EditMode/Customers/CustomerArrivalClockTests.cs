using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.Customers;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Customers
{
    public sealed class CustomerArrivalClockTests
    {
        [Test] public void Constructor_ZeroInterval_Throws() { Assert.Throws<System.ArgumentOutOfRangeException>(() => new CustomerArrivalClock(0)); }
        [Test] public void Advance_Negative_Throws() { Assert.Throws<System.ArgumentOutOfRangeException>(() => new CustomerArrivalClock(5).Advance(-1)); }
        [Test] public void Advance_BelowInterval_ReturnsZero() { Assert.That(new CustomerArrivalClock(5).Advance(4), Is.Zero); }
        [Test] public void Advance_ExactInterval_ReturnsOne() { Assert.That(new CustomerArrivalClock(5).Advance(5), Is.EqualTo(1)); }
        [Test] public void Advance_MultipleIntervals_ReturnsDueCount() { Assert.That(new CustomerArrivalClock(5).Advance(12), Is.EqualTo(2)); }
        [Test] public void Advance_PreservesRemainder() { CustomerArrivalClock c = new CustomerArrivalClock(5); c.Advance(7); Assert.That(c.AccumulatedSeconds, Is.EqualTo(2)); }
        [Test] public void Advance_AccumulatesAcrossCalls() { CustomerArrivalClock c = new CustomerArrivalClock(5); c.Advance(3); Assert.That(c.Advance(2), Is.EqualTo(1)); }
        [Test] public void Reset_ClearsAccumulation() { CustomerArrivalClock c = new CustomerArrivalClock(5); c.Advance(3); c.Reset(); Assert.That(c.AccumulatedSeconds, Is.Zero); }
    }
}
