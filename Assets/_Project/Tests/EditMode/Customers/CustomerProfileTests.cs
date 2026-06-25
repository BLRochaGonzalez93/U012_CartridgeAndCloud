using System;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.Customers;
using VRMGames.CartridgeAndCloud.Domain.Products;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Customers
{
    public sealed class CustomerProfileTests
    {
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
    }
}
