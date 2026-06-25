using System;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.Customers;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Customers
{
    public sealed class CustomerInstanceTests
    {
        [Test] public void Constructor_ZeroPatience_Throws() { Assert.Throws<ArgumentOutOfRangeException>(() => new CustomerInstance(new CustomerInstanceId("c"), new CustomerProfileId("p"), Plan(), 0)); }
        [Test] public void Constructor_StartsWaitingAtEntry() { CustomerInstance c = Create(); Assert.That(c.State, Is.EqualTo(CustomerState.WaitingToEnter)); Assert.That(c.CurrentTarget.Type, Is.EqualTo(CustomerNavigationTargetType.Entry)); }
        [Test] public void BeginEntering_TransitionsToEntering() { CustomerInstance c = Create(); Assert.That(c.BeginEntering().StateAfter, Is.EqualTo(CustomerState.Entering)); }
        [Test] public void ArriveAtEntry_TransitionsToBrowsing() { CustomerInstance c = Create(); c.BeginEntering(); c.ArriveAtCurrentTarget(); Assert.That(c.State, Is.EqualTo(CustomerState.Browsing)); }
        [Test] public void ArriveAtFinalBrowse_TransitionsToLeaving() { CustomerInstance c = Create(); c.BeginEntering(); c.ArriveAtCurrentTarget(); c.ArriveAtCurrentTarget(); Assert.That(c.State, Is.EqualTo(CustomerState.Leaving)); }
        [Test] public void ArriveAtExit_TransitionsToDespawned() { CustomerInstance c = Create(); c.BeginEntering(); c.ArriveAtCurrentTarget(); c.ArriveAtCurrentTarget(); c.ArriveAtCurrentTarget(); Assert.That(c.State, Is.EqualTo(CustomerState.Despawned)); }
        [Test] public void PatienceExpires_TransitionsToLeaving() { CustomerInstance c = Create(3); c.BeginEntering(); c.ArriveAtCurrentTarget(); c.AdvancePatience(3); Assert.That(c.State, Is.EqualTo(CustomerState.Leaving)); }
        [Test] public void PatienceOutsideBrowsing_IsRejected() { CustomerInstance c = Create(); Assert.That(c.AdvancePatience(1).Succeeded, Is.False); }

        private static CustomerInstance Create(int patience = 10) { return new CustomerInstance(new CustomerInstanceId("customer"), new CustomerProfileId("profile"), Plan(), patience); }
        private static CustomerNavigationPlan Plan() { return new CustomerNavigationPlan(new[] { new CustomerNavigationTarget(new CustomerNavigationPointId("entry"), CustomerNavigationTargetType.Entry, 0), new CustomerNavigationTarget(new CustomerNavigationPointId("browse"), CustomerNavigationTargetType.Browse, 1), new CustomerNavigationTarget(new CustomerNavigationPointId("exit"), CustomerNavigationTargetType.Exit, 0) }); }
    }
}
