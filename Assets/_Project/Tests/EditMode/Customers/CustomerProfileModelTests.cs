using NUnit.Framework;
using System;
using VRMGames.CartridgeAndCloud.Application.Customers;
using VRMGames.CartridgeAndCloud.Domain.Customers;
using VRMGames.CartridgeAndCloud.Domain.Products;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Customers
{
    public sealed class CustomerProfileModelTests
    {
        [Test] public void ProfileId_Empty_Throws() { Assert.Throws<ArgumentException>(() => new CustomerProfileId("")); }
        [Test] public void InstanceId_Whitespace_Throws() { Assert.Throws<ArgumentException>(() => new CustomerInstanceId(" ")); }
        [Test] public void RequestId_Null_Throws() { Assert.Throws<ArgumentException>(() => new CustomerSpawnRequestId(null)); }
        [Test] public void NavigationPointId_Valid_PreservesValue() { Assert.That(new CustomerNavigationPointId("entry").Value, Is.EqualTo("entry")); }
        [Test] public void ProfileId_Equality_IsOrdinal() { Assert.That(new CustomerProfileId("A") == new CustomerProfileId("A"), Is.True); }
        [Test] public void ProfileId_Equality_IsCaseSensitive() { Assert.That(new CustomerProfileId("A") == new CustomerProfileId("a"), Is.False); }
        [Test] public void InstanceId_ToString_ReturnsValue() { Assert.That(new CustomerInstanceId("customer-1").ToString(), Is.EqualTo("customer-1")); }
        [Test] public void RequestId_Inequality_Works() { Assert.That(new CustomerSpawnRequestId("a") != new CustomerSpawnRequestId("b"), Is.True); }
        [Test] public void Constructor_NullRegistry_Throws() { Assert.Throws<ArgumentNullException>(() => new CustomerProfileSelector(null)); }
        [Test] public void Constructor_EmptyRegistry_Throws() { Assert.Throws<ArgumentException>(() => new CustomerProfileSelector(new CustomerProfileRegistry(Array.Empty<CustomerProfile>()))); }
        [Test] public void Select_NegativeRoll_Throws() { Assert.Throws<ArgumentOutOfRangeException>(() => Selector().SelectByRoll(-1)); }
        [Test] public void Select_RollAtTotalWeight_Throws() { Assert.Throws<ArgumentOutOfRangeException>(() => Selector().SelectByRoll(5)); }
        [Test] public void Select_FirstWeightRange_ReturnsFirstOrdinalProfile() { Assert.That(Selector().SelectByRoll(0).Id.Value, Is.EqualTo("a")); }
        [Test] public void Select_LastRollOfFirstRange_ReturnsFirstProfile() { Assert.That(Selector().SelectByRoll(1).Id.Value, Is.EqualTo("a")); }
        [Test] public void Select_FirstRollOfSecondRange_ReturnsSecondProfile() { Assert.That(Selector().SelectByRoll(2).Id.Value, Is.EqualTo("b")); }
        [Test] public void Select_LastValidRoll_ReturnsSecondProfile() { Assert.That(Selector().SelectByRoll(4).Id.Value, Is.EqualTo("b")); }
        [Test] public void Constructor_Null_Throws() { Assert.Throws<ArgumentNullException>(() => new CustomerProfileRegistry(null)); }
        [Test] public void Constructor_NullEntry_Throws() { Assert.Throws<ArgumentException>(() => new CustomerProfileRegistry(new CustomerProfile[] { null })); }
        [Test] public void Constructor_DuplicateId_Throws() { Assert.Throws<ArgumentException>(() => new CustomerProfileRegistry(new[] { Create("same", 1), Create("same", 2) })); }
        [Test] public void Profiles_AreSortedOrdinally() { CustomerProfileRegistry r = new CustomerProfileRegistry(new[] { Create("z", 1), Create("a", 1) }); Assert.That(r.Profiles[0].Id.Value, Is.EqualTo("a")); }
        [Test] public void TotalSpawnWeight_SumsProfiles() { CustomerProfileRegistry r = new CustomerProfileRegistry(new[] { Create("a", 2), Create("b", 3) }); Assert.That(r.TotalSpawnWeight, Is.EqualTo(5)); }
        [Test] public void Contains_KnownId_ReturnsTrue() { CustomerProfileRegistry r = new CustomerProfileRegistry(new[] { Create("a", 1) }); Assert.That(r.Contains(new CustomerProfileId("a")), Is.True); }
        [Test] public void TryGet_UnknownId_ReturnsFalse() { CustomerProfileRegistry r = new CustomerProfileRegistry(Array.Empty<CustomerProfile>()); Assert.That(r.TryGet(new CustomerProfileId("x"), out CustomerProfile _), Is.False); }
        [Test] public void Get_UnknownId_Throws() { CustomerProfileRegistry r = new CustomerProfileRegistry(Array.Empty<CustomerProfile>()); Assert.Throws<System.Collections.Generic.KeyNotFoundException>(() => r.Get(new CustomerProfileId("x"))); }
        [Test] public void Constructor_EmptyNameKey_Throws() { Assert.Throws<ArgumentException>(() => Create(displayNameKey: "")); }
        [Test] public void Constructor_ZeroWeight_Throws() { Assert.Throws<ArgumentOutOfRangeException>(() => Create(spawnWeight: 0)); }
        [Test] public void Constructor_ZeroPatience_Throws() { Assert.Throws<ArgumentOutOfRangeException>(() => Create(patience: 0)); }
        [Test] public void Constructor_ZeroStops_Throws() { Assert.Throws<ArgumentOutOfRangeException>(() => Create(stops: 0)); }
        [Test] public void Constructor_ZeroSpeed_Throws() { Assert.Throws<ArgumentOutOfRangeException>(() => Create(speed: 0f)); }
        [Test] public void Constructor_DuplicateCategory_Throws() { Assert.Throws<ArgumentException>(() => new CustomerProfile(new CustomerProfileId("p"), "name", new[] { new ProductCategoryId("game"), new ProductCategoryId("game") }, 1, 10, 1, 1f)); }
        [Test] public void Prefers_KnownCategory_ReturnsTrue() { Assert.That(Create().Prefers(new ProductCategoryId("video-game")), Is.True); }
        [Test] public void Prefers_UnknownCategory_ReturnsFalse() { Assert.That(Create().Prefers(new ProductCategoryId("console")), Is.False); }

        private static CustomerProfile Create(string displayNameKey = "customer.name", int spawnWeight = 1, int patience = 30, int stops = 2, float speed = 1.8f)
        {
            return new CustomerProfile(new CustomerProfileId("profile"), displayNameKey, new[] { new ProductCategoryId("video-game") }, spawnWeight, patience, stops, speed);
        }
        private static CustomerProfileSelector Selector() { return new CustomerProfileSelector(new CustomerProfileRegistry(new[] { Create("b", 3), Create("a", 2) })); }
        private static CustomerProfile Create(string id, int weight) { return new CustomerProfile(new CustomerProfileId(id), "name", Array.Empty<ProductCategoryId>(), weight, 10, 1, 1f); }
    }
}
