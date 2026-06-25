using System;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.Customers;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Customers
{
    public sealed class CustomerIdentifierTests
    {
        [Test] public void ProfileId_Empty_Throws() { Assert.Throws<ArgumentException>(() => new CustomerProfileId("")); }
        [Test] public void InstanceId_Whitespace_Throws() { Assert.Throws<ArgumentException>(() => new CustomerInstanceId(" ")); }
        [Test] public void RequestId_Null_Throws() { Assert.Throws<ArgumentException>(() => new CustomerSpawnRequestId(null)); }
        [Test] public void NavigationPointId_Valid_PreservesValue() { Assert.That(new CustomerNavigationPointId("entry").Value, Is.EqualTo("entry")); }
        [Test] public void ProfileId_Equality_IsOrdinal() { Assert.That(new CustomerProfileId("A") == new CustomerProfileId("A"), Is.True); }
        [Test] public void ProfileId_Equality_IsCaseSensitive() { Assert.That(new CustomerProfileId("A") == new CustomerProfileId("a"), Is.False); }
        [Test] public void InstanceId_ToString_ReturnsValue() { Assert.That(new CustomerInstanceId("customer-1").ToString(), Is.EqualTo("customer-1")); }
        [Test] public void RequestId_Inequality_Works() { Assert.That(new CustomerSpawnRequestId("a") != new CustomerSpawnRequestId("b"), Is.True); }
    }
}
