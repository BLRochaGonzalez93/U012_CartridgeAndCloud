using NUnit.Framework;
using System;
using VRMGames.CartridgeAndCloud.Domain.Customers;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Customers
{
    public sealed class CustomerLifecycleTests
    {
        [Test] public void Constructor_ZeroInterval_Throws() { Assert.Throws<System.ArgumentOutOfRangeException>(() => new CustomerArrivalClock(0)); }
        [Test] public void Advance_Negative_Throws() { Assert.Throws<System.ArgumentOutOfRangeException>(() => new CustomerArrivalClock(5).Advance(-1)); }
        [Test] public void Advance_BelowInterval_ReturnsZero() { Assert.That(new CustomerArrivalClock(5).Advance(4), Is.Zero); }
        [Test] public void Advance_ExactInterval_ReturnsOne() { Assert.That(new CustomerArrivalClock(5).Advance(5), Is.EqualTo(1)); }
        [Test] public void Advance_MultipleIntervals_ReturnsDueCount() { Assert.That(new CustomerArrivalClock(5).Advance(12), Is.EqualTo(2)); }
        [Test] public void Advance_PreservesRemainder() { CustomerArrivalClock c = new CustomerArrivalClock(5); c.Advance(7); Assert.That(c.AccumulatedSeconds, Is.EqualTo(2)); }
        [Test] public void Advance_AccumulatesAcrossCalls() { CustomerArrivalClock c = new CustomerArrivalClock(5); c.Advance(3); Assert.That(c.Advance(2), Is.EqualTo(1)); }
        [Test] public void Reset_ClearsAccumulation() { CustomerArrivalClock c = new CustomerArrivalClock(5); c.Advance(3); c.Reset(); Assert.That(c.AccumulatedSeconds, Is.Zero); }
        [Test] public void Target_NegativeDwell_Throws() { Assert.Throws<ArgumentOutOfRangeException>(() => Target("browse", CustomerNavigationTargetType.Browse, -1)); }
        [Test] public void Target_EntryWithDwell_Throws() { Assert.Throws<ArgumentException>(() => Target("entry", CustomerNavigationTargetType.Entry, 1)); }
        [Test] public void Plan_FewerThanTwoTargets_Throws() { Assert.Throws<ArgumentException>(() => new CustomerNavigationPlan(new[] { Target("entry", CustomerNavigationTargetType.Entry, 0) })); }
        [Test] public void Plan_FirstNotEntry_Throws() { Assert.Throws<ArgumentException>(() => new CustomerNavigationPlan(new[] { Target("browse", CustomerNavigationTargetType.Browse, 0), Target("exit", CustomerNavigationTargetType.Exit, 0) })); }
        [Test] public void Plan_LastNotExit_Throws() { Assert.Throws<ArgumentException>(() => new CustomerNavigationPlan(new[] { Target("entry", CustomerNavigationTargetType.Entry, 0), Target("browse", CustomerNavigationTargetType.Browse, 0) })); }
        [Test] public void Plan_IntermediateNotBrowse_Throws() { Assert.Throws<ArgumentException>(() => new CustomerNavigationPlan(new[] { Target("entry", CustomerNavigationTargetType.Entry, 0), Target("entry2", CustomerNavigationTargetType.Entry, 0), Target("exit", CustomerNavigationTargetType.Exit, 0) })); }
        [Test] public void Plan_DuplicatePoint_Throws() { Assert.Throws<ArgumentException>(() => new CustomerNavigationPlan(new[] { Target("same", CustomerNavigationTargetType.Entry, 0), Target("same", CustomerNavigationTargetType.Exit, 0) })); }
        [Test] public void ValidPlan_ExposesEntryAndExit() { CustomerNavigationPlan p = Create(); Assert.That(p.EntryTarget.Type, Is.EqualTo(CustomerNavigationTargetType.Entry)); Assert.That(p.ExitTarget.Type, Is.EqualTo(CustomerNavigationTargetType.Exit)); }
        [Test] public void Constructor_ZeroPatience_Throws() { Assert.Throws<ArgumentOutOfRangeException>(() => new CustomerInstance(new CustomerInstanceId("c"), new CustomerProfileId("p"), Plan(), 0)); }
        [Test] public void Constructor_StartsWaitingAtEntry() { CustomerInstance c = InstanceCreate(); Assert.That(c.State, Is.EqualTo(CustomerState.WaitingToEnter)); Assert.That(c.CurrentTarget.Type, Is.EqualTo(CustomerNavigationTargetType.Entry)); }
        [Test] public void BeginEntering_TransitionsToEntering() { CustomerInstance c = InstanceCreate(); Assert.That(c.BeginEntering().StateAfter, Is.EqualTo(CustomerState.Entering)); }
        [Test] public void ArriveAtEntry_TransitionsToBrowsing() { CustomerInstance c = InstanceCreate(); c.BeginEntering(); c.ArriveAtCurrentTarget(); Assert.That(c.State, Is.EqualTo(CustomerState.Browsing)); }
        [Test] public void ArriveAtFinalBrowse_TransitionsToLeaving() { CustomerInstance c = InstanceCreate(); c.BeginEntering(); c.ArriveAtCurrentTarget(); c.ArriveAtCurrentTarget(); Assert.That(c.State, Is.EqualTo(CustomerState.Leaving)); }
        [Test] public void ArriveAtExit_TransitionsToDespawned() { CustomerInstance c = InstanceCreate(); c.BeginEntering(); c.ArriveAtCurrentTarget(); c.ArriveAtCurrentTarget(); c.ArriveAtCurrentTarget(); Assert.That(c.State, Is.EqualTo(CustomerState.Despawned)); }
        [Test] public void PatienceExpires_TransitionsToLeaving() { CustomerInstance c = InstanceCreate(3); c.BeginEntering(); c.ArriveAtCurrentTarget(); c.AdvancePatience(3); Assert.That(c.State, Is.EqualTo(CustomerState.Leaving)); }
        [Test] public void PatienceOutsideBrowsing_IsRejected() { CustomerInstance c = InstanceCreate(); Assert.That(c.AdvancePatience(1).Succeeded, Is.False); }
        [Test] public void NewRegistry_IsEmpty() { Assert.That(new CustomerInstanceRegistry().Count, Is.Zero); }
        [Test] public void Add_Null_Throws() { Assert.Throws<System.ArgumentNullException>(() => new CustomerInstanceRegistry().Add(null)); }
        [Test] public void Add_FirstInstance_ReturnsTrue() { Assert.That(new CustomerInstanceRegistry().Add(Create("a")), Is.True); }
        [Test] public void Add_DuplicateId_ReturnsFalse() { CustomerInstanceRegistry r = new CustomerInstanceRegistry(); r.Add(Create("a")); Assert.That(r.Add(Create("a")), Is.False); }
        [Test] public void Instances_AreSortedOrdinally() { CustomerInstanceRegistry r = new CustomerInstanceRegistry(); r.Add(Create("z")); r.Add(Create("a")); Assert.That(r.Instances[0].Id.Value, Is.EqualTo("a")); }
        [Test] public void ActiveCount_ExcludesDespawned() { CustomerInstanceRegistry r = new CustomerInstanceRegistry(); CustomerInstance c = Create("a"); r.Add(c); c.BeginEntering(); c.ArriveAtCurrentTarget(); c.ArriveAtCurrentTarget(); Assert.That(r.ActiveCount, Is.EqualTo(1)); c.ArriveAtCurrentTarget(); Assert.That(r.ActiveCount, Is.Zero); }
        [Test] public void Remove_KnownInstance_ReturnsTrue() { CustomerInstanceRegistry r = new CustomerInstanceRegistry(); r.Add(Create("a")); Assert.That(r.Remove(new CustomerInstanceId("a")), Is.True); }
        [Test] public void Get_UnknownInstance_Throws() { Assert.Throws<System.Collections.Generic.KeyNotFoundException>(() => new CustomerInstanceRegistry().Get(new CustomerInstanceId("x"))); }

        private static CustomerInstance Create(string id) { return new CustomerInstance(new CustomerInstanceId(id), new CustomerProfileId("p"), new CustomerNavigationPlan(new[] { new CustomerNavigationTarget(new CustomerNavigationPointId("entry-" + id), CustomerNavigationTargetType.Entry, 0), new CustomerNavigationTarget(new CustomerNavigationPointId("browse-" + id), CustomerNavigationTargetType.Browse, 0), new CustomerNavigationTarget(new CustomerNavigationPointId("exit-" + id), CustomerNavigationTargetType.Exit, 0) }), 10); }
        private static CustomerInstance InstanceCreate(int patience = 10) { return new CustomerInstance(new CustomerInstanceId("customer"), new CustomerProfileId("profile"), Plan(), patience); }
        private static CustomerNavigationPlan Plan() { return new CustomerNavigationPlan(new[] { new CustomerNavigationTarget(new CustomerNavigationPointId("entry"), CustomerNavigationTargetType.Entry, 0), new CustomerNavigationTarget(new CustomerNavigationPointId("browse"), CustomerNavigationTargetType.Browse, 1), new CustomerNavigationTarget(new CustomerNavigationPointId("exit"), CustomerNavigationTargetType.Exit, 0) }); }
        private static CustomerNavigationTarget Target(string id, CustomerNavigationTargetType type, int dwell) { return new CustomerNavigationTarget(new CustomerNavigationPointId(id), type, dwell); }
        private static CustomerNavigationPlan Create() { return new CustomerNavigationPlan(new[] { Target("entry", CustomerNavigationTargetType.Entry, 0), Target("browse", CustomerNavigationTargetType.Browse, 2), Target("exit", CustomerNavigationTargetType.Exit, 0) }); }
    }
}
