using System;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.Customers;
using VRMGames.CartridgeAndCloud.Domain.Products;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Customers
{
    public sealed class CustomerProfileRegistryTests
    {
        [Test] public void Constructor_Null_Throws() { Assert.Throws<ArgumentNullException>(() => new CustomerProfileRegistry(null)); }
        [Test] public void Constructor_NullEntry_Throws() { Assert.Throws<ArgumentException>(() => new CustomerProfileRegistry(new CustomerProfile[] { null })); }
        [Test] public void Constructor_DuplicateId_Throws() { Assert.Throws<ArgumentException>(() => new CustomerProfileRegistry(new[] { Create("same", 1), Create("same", 2) })); }
        [Test] public void Profiles_AreSortedOrdinally() { CustomerProfileRegistry r = new CustomerProfileRegistry(new[] { Create("z", 1), Create("a", 1) }); Assert.That(r.Profiles[0].Id.Value, Is.EqualTo("a")); }
        [Test] public void TotalSpawnWeight_SumsProfiles() { CustomerProfileRegistry r = new CustomerProfileRegistry(new[] { Create("a", 2), Create("b", 3) }); Assert.That(r.TotalSpawnWeight, Is.EqualTo(5)); }
        [Test] public void Contains_KnownId_ReturnsTrue() { CustomerProfileRegistry r = new CustomerProfileRegistry(new[] { Create("a", 1) }); Assert.That(r.Contains(new CustomerProfileId("a")), Is.True); }
        [Test] public void TryGet_UnknownId_ReturnsFalse() { CustomerProfileRegistry r = new CustomerProfileRegistry(Array.Empty<CustomerProfile>()); Assert.That(r.TryGet(new CustomerProfileId("x"), out CustomerProfile _), Is.False); }
        [Test] public void Get_UnknownId_Throws() { CustomerProfileRegistry r = new CustomerProfileRegistry(Array.Empty<CustomerProfile>()); Assert.Throws<System.Collections.Generic.KeyNotFoundException>(() => r.Get(new CustomerProfileId("x"))); }

        private static CustomerProfile Create(string id, int weight) { return new CustomerProfile(new CustomerProfileId(id), "name", Array.Empty<ProductCategoryId>(), weight, 20, 1, 1f); }
    }
}
