using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.Customers;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Customers
{
    public sealed class CustomerSpawnQueueTests
    {
        [Test] public void NewQueue_IsEmpty() { Assert.That(new CustomerSpawnQueue().Count, Is.Zero); }
        [Test] public void Enqueue_Null_Throws() { Assert.Throws<System.ArgumentNullException>(() => new CustomerSpawnQueue().TryEnqueue(null)); }
        [Test] public void Enqueue_FirstRequest_ReturnsTrue() { Assert.That(new CustomerSpawnQueue().TryEnqueue(Create("r1", "c1")), Is.True); }
        [Test] public void Enqueue_DuplicateRequestId_ReturnsFalse() { CustomerSpawnQueue q = new CustomerSpawnQueue(); q.TryEnqueue(Create("r", "c1")); Assert.That(q.TryEnqueue(Create("r", "c2")), Is.False); }
        [Test] public void Enqueue_DuplicateInstanceId_ReturnsFalse() { CustomerSpawnQueue q = new CustomerSpawnQueue(); q.TryEnqueue(Create("r1", "c")); Assert.That(q.TryEnqueue(Create("r2", "c")), Is.False); }
        [Test] public void Dequeue_UsesFifoOrder() { CustomerSpawnQueue q = new CustomerSpawnQueue(); q.TryEnqueue(Create("r1", "c1")); q.TryEnqueue(Create("r2", "c2")); q.TryDequeue(out CustomerSpawnRequest r); Assert.That(r.RequestId.Value, Is.EqualTo("r1")); }
        [Test] public void Peek_DoesNotRemoveRequest() { CustomerSpawnQueue q = new CustomerSpawnQueue(); q.TryEnqueue(Create("r1", "c1")); q.TryPeek(out CustomerSpawnRequest _); Assert.That(q.Count, Is.EqualTo(1)); }
        [Test] public void Clear_RemovesAllRequests() { CustomerSpawnQueue q = new CustomerSpawnQueue(); q.TryEnqueue(Create("r1", "c1")); q.Clear(); Assert.That(q.Count, Is.Zero); }

        private static CustomerSpawnRequest Create(string request, string instance) { return new CustomerSpawnRequest(new CustomerSpawnRequestId(request), new CustomerInstanceId(instance), new CustomerProfileId("profile"), new CustomerNavigationPlan(new[] { new CustomerNavigationTarget(new CustomerNavigationPointId("entry-" + instance), CustomerNavigationTargetType.Entry, 0), new CustomerNavigationTarget(new CustomerNavigationPointId("exit-" + instance), CustomerNavigationTargetType.Exit, 0) })); }
    }
}
