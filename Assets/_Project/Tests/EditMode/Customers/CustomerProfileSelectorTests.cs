using System;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.Customers;
using VRMGames.CartridgeAndCloud.Domain.Customers;
using VRMGames.CartridgeAndCloud.Domain.Products;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Customers
{
    public sealed class CustomerProfileSelectorTests
    {
        [Test] public void Constructor_NullRegistry_Throws() { Assert.Throws<ArgumentNullException>(() => new CustomerProfileSelector(null)); }
        [Test] public void Constructor_EmptyRegistry_Throws() { Assert.Throws<ArgumentException>(() => new CustomerProfileSelector(new CustomerProfileRegistry(Array.Empty<CustomerProfile>()))); }
        [Test] public void Select_NegativeRoll_Throws() { Assert.Throws<ArgumentOutOfRangeException>(() => Selector().SelectByRoll(-1)); }
        [Test] public void Select_RollAtTotalWeight_Throws() { Assert.Throws<ArgumentOutOfRangeException>(() => Selector().SelectByRoll(5)); }
        [Test] public void Select_FirstWeightRange_ReturnsFirstOrdinalProfile() { Assert.That(Selector().SelectByRoll(0).Id.Value, Is.EqualTo("a")); }
        [Test] public void Select_LastRollOfFirstRange_ReturnsFirstProfile() { Assert.That(Selector().SelectByRoll(1).Id.Value, Is.EqualTo("a")); }
        [Test] public void Select_FirstRollOfSecondRange_ReturnsSecondProfile() { Assert.That(Selector().SelectByRoll(2).Id.Value, Is.EqualTo("b")); }
        [Test] public void Select_LastValidRoll_ReturnsSecondProfile() { Assert.That(Selector().SelectByRoll(4).Id.Value, Is.EqualTo("b")); }

        private static CustomerProfileSelector Selector() { return new CustomerProfileSelector(new CustomerProfileRegistry(new[] { Create("b", 3), Create("a", 2) })); }
        private static CustomerProfile Create(string id, int weight) { return new CustomerProfile(new CustomerProfileId(id), "name", Array.Empty<ProductCategoryId>(), weight, 10, 1, 1f); }
    }
}
