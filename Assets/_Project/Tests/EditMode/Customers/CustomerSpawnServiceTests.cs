using System;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.Customers;
using VRMGames.CartridgeAndCloud.Domain.Customers;
using VRMGames.CartridgeAndCloud.Domain.Products;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Customers
{
    public sealed class CustomerSpawnServiceTests
    {
        [Test] public void EmptyQueue_ReturnsQueueEmpty() { Context c = CreateContext(); Assert.That(c.Service.TrySpawnNext(c.Queue, c.Policy).FailureReason, Is.EqualTo(CustomerSpawnFailureReason.QueueEmpty)); }
        [Test] public void SuccessfulSpawn_DequeuesRequest() { Context c = CreateContext(); c.Queue.TryEnqueue(Request("r", "c", "profile")); Assert.That(c.Service.TrySpawnNext(c.Queue, c.Policy).Succeeded, Is.True); Assert.That(c.Queue.Count, Is.Zero); }
        [Test] public void SuccessfulSpawn_RegistersEnteringCustomer() { Context c = CreateContext(); c.Queue.TryEnqueue(Request("r", "c", "profile")); CustomerSpawnResult r = c.Service.TrySpawnNext(c.Queue, c.Policy); Assert.That(r.Customer.State, Is.EqualTo(CustomerState.Entering)); Assert.That(c.Instances.Contains(new CustomerInstanceId("c")), Is.True); }
        [Test] public void PopulationCap_PreservesQueue() { Context c = CreateContext(1); c.Queue.TryEnqueue(Request("r1", "c1", "profile")); c.Service.TrySpawnNext(c.Queue, c.Policy); c.Queue.TryEnqueue(Request("r2", "c2", "profile")); Assert.That(c.Service.TrySpawnNext(c.Queue, c.Policy).FailureReason, Is.EqualTo(CustomerSpawnFailureReason.PopulationLimitReached)); Assert.That(c.Queue.Count, Is.EqualTo(1)); }
        [Test] public void MissingProfile_PreservesQueue() { Context c = CreateContext(); c.Queue.TryEnqueue(Request("r", "c", "missing")); Assert.That(c.Service.TrySpawnNext(c.Queue, c.Policy).FailureReason, Is.EqualTo(CustomerSpawnFailureReason.ProfileMissing)); Assert.That(c.Queue.Count, Is.EqualTo(1)); }
        [Test] public void DuplicateInstance_PreservesQueue() { Context c = CreateContext(); c.Instances.Add(new CustomerInstance(new CustomerInstanceId("c"), new CustomerProfileId("profile"), Plan("existing"), 10)); c.Queue.TryEnqueue(Request("r", "c", "profile")); Assert.That(c.Service.TrySpawnNext(c.Queue, c.Policy).FailureReason, Is.EqualTo(CustomerSpawnFailureReason.DuplicateInstanceId)); Assert.That(c.Queue.Count, Is.EqualTo(1)); }
        [Test] public void Success_IncrementsActiveCount() { Context c = CreateContext(); c.Queue.TryEnqueue(Request("r", "c", "profile")); CustomerSpawnResult r = c.Service.TrySpawnNext(c.Queue, c.Policy); Assert.That(r.ActiveCountBefore, Is.Zero); Assert.That(r.ActiveCountAfter, Is.EqualTo(1)); }
        [Test] public void Spawn_UsesProfilePatience() { Context c = CreateContext(); c.Queue.TryEnqueue(Request("r", "c", "profile")); CustomerSpawnResult r = c.Service.TrySpawnNext(c.Queue, c.Policy); Assert.That(r.Customer.RemainingPatienceSeconds, Is.EqualTo(25)); }

        private static Context CreateContext(int cap = 4) { CustomerProfileRegistry profiles = new CustomerProfileRegistry(new[] { new CustomerProfile(new CustomerProfileId("profile"), "name", Array.Empty<ProductCategoryId>(), 1, 25, 1, 1f) }); CustomerInstanceRegistry instances = new CustomerInstanceRegistry(); return new Context { Instances = instances, Queue = new CustomerSpawnQueue(), Policy = new CustomerSpawnPolicy(cap, 5), Service = new CustomerSpawnService(profiles, instances) }; }
        private static CustomerSpawnRequest Request(string r, string c, string p) { return new CustomerSpawnRequest(new CustomerSpawnRequestId(r), new CustomerInstanceId(c), new CustomerProfileId(p), Plan(c)); }
        private static CustomerNavigationPlan Plan(string suffix) { return new CustomerNavigationPlan(new[] { new CustomerNavigationTarget(new CustomerNavigationPointId("entry-" + suffix), CustomerNavigationTargetType.Entry, 0), new CustomerNavigationTarget(new CustomerNavigationPointId("exit-" + suffix), CustomerNavigationTargetType.Exit, 0) }); }
        private sealed class Context { public CustomerInstanceRegistry Instances; public CustomerSpawnQueue Queue; public CustomerSpawnPolicy Policy; public CustomerSpawnService Service; }
    }
}
