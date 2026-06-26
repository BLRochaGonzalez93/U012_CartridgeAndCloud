using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.Shopping;
using VRMGames.CartridgeAndCloud.Domain.Customers;
using VRMGames.CartridgeAndCloud.Domain.Products;
using VRMGames.CartridgeAndCloud.Domain.Shopping;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Shopping
{
    public sealed class ShoppingValueAndIntentTests
    {
        [Test] public void IntentId_RejectsEmpty() =>
            Assert.Throws<System.ArgumentException>(
                () => new ShoppingIntentId(" "));

        [Test] public void ReservationId_RejectsEmpty() =>
            Assert.Throws<System.ArgumentException>(
                () => new ShoppingReservationId(""));

        [Test] public void CartId_RejectsNull() =>
            Assert.Throws<System.ArgumentException>(
                () => new ShoppingCartId(null));

        [Test] public void IntentId_UsesOrdinalEquality()
        {
            Assert.That(
                new ShoppingIntentId("A") == new ShoppingIntentId("A"),
                Is.True);
        }

        [Test] public void IntentId_IsCaseSensitive()
        {
            Assert.That(
                new ShoppingIntentId("A") == new ShoppingIntentId("a"),
                Is.False);
        }

        [Test] public void ReservationId_HashMatchesEquality()
        {
            var left = new ShoppingReservationId("id");
            var right = new ShoppingReservationId("id");
            Assert.That(left.GetHashCode(), Is.EqualTo(right.GetHashCode()));
        }

        [Test] public void CartId_ToStringReturnsValue()
        {
            Assert.That(new ShoppingCartId("cart").ToString(), Is.EqualTo("cart"));
        }

        [Test] public void Policy_RejectsZeroCartCapacity() =>
            Assert.Throws<System.ArgumentOutOfRangeException>(
                () => new ShoppingPolicy(0, 1, true));

        [Test] public void Policy_RejectsReservationAboveCart() =>
            Assert.Throws<System.ArgumentOutOfRangeException>(
                () => new ShoppingPolicy(1, 2, true));

        [Test] public void Policy_StoresValues()
        {
            var policy = new ShoppingPolicy(3, 1, false);
            Assert.That(policy.MaxCartUnits, Is.EqualTo(3));
            Assert.That(policy.MaxUnitsPerReservation, Is.EqualTo(1));
            Assert.That(policy.AllowFallbackCategories, Is.False);
        }

        [Test] public void Intent_RejectsZeroDesiredUnits() =>
            Assert.Throws<System.ArgumentOutOfRangeException>(
                () => new ShoppingIntent(
                    new ShoppingIntentId("intent"),
                    new CustomerInstanceId("customer"),
                    new[] { new ProductCategoryId("video-game") },
                    0));

        [Test] public void Intent_RejectsDuplicateCategories() =>
            Assert.Throws<System.ArgumentException>(
                () => new ShoppingIntent(
                    new ShoppingIntentId("intent"),
                    new CustomerInstanceId("customer"),
                    new[] {
                        new ProductCategoryId("video-game"),
                        new ProductCategoryId("video-game")
                    },
                    1));

        [Test] public void Intent_ReturnsPreferenceRank()
        {
            var intent = ShoppingTestFactory.IntentWithCategories(
                new CustomerInstanceId("customer"),
                1,
                ShoppingTestFactory.Accessory,
                ShoppingTestFactory.VideoGame);
            bool found = intent.TryGetPreferenceRank(
                ShoppingTestFactory.VideoGame,
                out int rank);
            Assert.That(found, Is.True);
            Assert.That(rank, Is.EqualTo(1));
        }

        [Test] public void Intent_ReturnsMinusOneForUnknownCategory()
        {
            var intent = ShoppingTestFactory.Intent(
                new CustomerInstanceId("customer"));
            bool found = intent.TryGetPreferenceRank(
                new ProductCategoryId("console"),
                out int rank);
            Assert.That(found, Is.False);
            Assert.That(rank, Is.EqualTo(-1));
        }

        [Test] public void IntentFactory_UsesProfilePreferences()
        {
            var profile = ShoppingTestFactory.Profile();
            var intent = new ShoppingIntentFactory().CreateFromProfile(
                new ShoppingIntentId("intent"),
                new CustomerInstanceId("customer"),
                profile,
                1);
            Assert.That(intent.PreferredCategories.Count, Is.EqualTo(1));
            Assert.That(intent.DesiredUnits, Is.EqualTo(1));
        }
    }
}
