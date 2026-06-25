using System;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.Placement;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode
{
    public sealed class PlacementInstanceIdTests
    {
        [Test]
        public void Constructor_StoresNonEmptyValue()
        {
            PlacementInstanceId id =
                new PlacementInstanceId(
                    "technical-shelf-0001");

            Assert.That(
                id.Value,
                Is.EqualTo(
                    "technical-shelf-0001"));
        }

        [Test]
        public void Constructor_EmptyValue_Throws()
        {
            Assert.Throws<
                ArgumentException>(
                    () => new PlacementInstanceId(" "));
        }

        [Test]
        public void Equality_IsOrdinalAndCaseSensitive()
        {
            PlacementInstanceId lower =
                new PlacementInstanceId("shelf");

            PlacementInstanceId same =
                new PlacementInstanceId("shelf");

            PlacementInstanceId upper =
                new PlacementInstanceId("SHELF");

            Assert.That(lower, Is.EqualTo(same));
            Assert.That(lower, Is.Not.EqualTo(upper));
        }
    }
}
