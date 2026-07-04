using NUnit.Framework;
using System;
using VRMGames.CartridgeAndCloud.Application.Customers;
using VRMGames.CartridgeAndCloud.Domain.Customers;
using VRMGames.CartridgeAndCloud.Domain.Products;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Customers
{
    public sealed class CustomerSpawnTests
    {
        [Test] public void NewQueue_IsEmpty() { Assert.That(new CustomerSpawnQueue().Count, Is.Zero); }
        [Test] public void Enqueue_Null_Throws() { Assert.Throws<System.ArgumentNullException>(() => new CustomerSpawnQueue().TryEnqueue(null)); }
        [Test] public void Enqueue_FirstRequest_ReturnsTrue() { Assert.That(new CustomerSpawnQueue().TryEnqueue(Create("r1", "c1")), Is.True); }
        [Test] public void Enqueue_DuplicateRequestId_ReturnsFalse() { CustomerSpawnQueue q = new CustomerSpawnQueue(); q.TryEnqueue(Create("r", "c1")); Assert.That(q.TryEnqueue(Create("r", "c2")), Is.False); }
        [Test] public void Enqueue_DuplicateInstanceId_ReturnsFalse() { CustomerSpawnQueue q = new CustomerSpawnQueue(); q.TryEnqueue(Create("r1", "c")); Assert.That(q.TryEnqueue(Create("r2", "c")), Is.False); }
        [Test] public void Dequeue_UsesFifoOrder() { CustomerSpawnQueue q = new CustomerSpawnQueue(); q.TryEnqueue(Create("r1", "c1")); q.TryEnqueue(Create("r2", "c2")); q.TryDequeue(out CustomerSpawnRequest r); Assert.That(r.RequestId.Value, Is.EqualTo("r1")); }
        [Test] public void Peek_DoesNotRemoveRequest() { CustomerSpawnQueue q = new CustomerSpawnQueue(); q.TryEnqueue(Create("r1", "c1")); q.TryPeek(out CustomerSpawnRequest _); Assert.That(q.Count, Is.EqualTo(1)); }
        [Test] public void Clear_RemovesAllRequests() { CustomerSpawnQueue q = new CustomerSpawnQueue(); q.TryEnqueue(Create("r1", "c1")); q.Clear(); Assert.That(q.Count, Is.Zero); }
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
        private static CustomerSpawnRequest Create(string request, string instance) { return new CustomerSpawnRequest(new CustomerSpawnRequestId(request), new CustomerInstanceId(instance), new CustomerProfileId("profile"), new CustomerNavigationPlan(new[] { new CustomerNavigationTarget(new CustomerNavigationPointId("entry-" + instance), CustomerNavigationTargetType.Entry, 0), new CustomerNavigationTarget(new CustomerNavigationPointId("exit-" + instance), CustomerNavigationTargetType.Exit, 0) })); }
    }
}
