using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.Customers;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Customers
{
    public sealed class CustomerInstanceRegistryTests
    {
        [Test] public void NewRegistry_IsEmpty() { Assert.That(new CustomerInstanceRegistry().Count, Is.Zero); }
        [Test] public void Add_Null_Throws() { Assert.Throws<System.ArgumentNullException>(() => new CustomerInstanceRegistry().Add(null)); }
        [Test] public void Add_FirstInstance_ReturnsTrue() { Assert.That(new CustomerInstanceRegistry().Add(Create("a")), Is.True); }
        [Test] public void Add_DuplicateId_ReturnsFalse() { CustomerInstanceRegistry r = new CustomerInstanceRegistry(); r.Add(Create("a")); Assert.That(r.Add(Create("a")), Is.False); }
        [Test] public void Instances_AreSortedOrdinally() { CustomerInstanceRegistry r = new CustomerInstanceRegistry(); r.Add(Create("z")); r.Add(Create("a")); Assert.That(r.Instances[0].Id.Value, Is.EqualTo("a")); }
        [Test] public void ActiveCount_ExcludesDespawned() { CustomerInstanceRegistry r = new CustomerInstanceRegistry(); CustomerInstance c = Create("a"); r.Add(c); c.BeginEntering(); c.ArriveAtCurrentTarget(); c.ArriveAtCurrentTarget(); Assert.That(r.ActiveCount, Is.EqualTo(1)); c.ArriveAtCurrentTarget(); Assert.That(r.ActiveCount, Is.Zero); }
        [Test] public void Remove_KnownInstance_ReturnsTrue() { CustomerInstanceRegistry r = new CustomerInstanceRegistry(); r.Add(Create("a")); Assert.That(r.Remove(new CustomerInstanceId("a")), Is.True); }
        [Test] public void Get_UnknownInstance_Throws() { Assert.Throws<System.Collections.Generic.KeyNotFoundException>(() => new CustomerInstanceRegistry().Get(new CustomerInstanceId("x"))); }

        private static CustomerInstance Create(string id) { return new CustomerInstance(new CustomerInstanceId(id), new CustomerProfileId("p"), new CustomerNavigationPlan(new[] { new CustomerNavigationTarget(new CustomerNavigationPointId("entry-" + id), CustomerNavigationTargetType.Entry, 0), new CustomerNavigationTarget(new CustomerNavigationPointId("browse-" + id), CustomerNavigationTargetType.Browse, 0), new CustomerNavigationTarget(new CustomerNavigationPointId("exit-" + id), CustomerNavigationTargetType.Exit, 0) }), 10); }
    }
}
