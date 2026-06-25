using System;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.Access;
using VRMGames.CartridgeAndCloud.Domain.Grid;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode
{
    public sealed class AccessAnchorTests
    {
        [Test]
        public void AccessAnchorId_RejectsEmptyValue()
        {
            Assert.Throws<ArgumentException>(
                () => new AccessAnchorId(string.Empty));

            Assert.Throws<ArgumentException>(
                () => new AccessAnchorId("   "));
        }

        [Test]
        public void AccessAnchorId_UsesOrdinalEquality()
        {
            AccessAnchorId lower =
                new AccessAnchorId("rear-service");

            AccessAnchorId same =
                new AccessAnchorId("rear-service");

            AccessAnchorId upper =
                new AccessAnchorId("REAR-SERVICE");

            Assert.That(lower, Is.EqualTo(same));
            Assert.That(lower, Is.Not.EqualTo(upper));
        }

        [Test]
        public void AccessAnchor_RejectsDefaultId()
        {
            Assert.Throws<ArgumentException>(
                () => new AccessAnchor(
                    default,
                    new GridCoordinate(2, 3)));
        }

        [Test]
        public void AccessAnchor_StoresIdAndCell()
        {
            AccessAnchorId id =
                new AccessAnchorId("checkout");

            GridCoordinate cell =
                new GridCoordinate(4, 9);

            AccessAnchor anchor =
                new AccessAnchor(id, cell);

            Assert.That(anchor.Id, Is.EqualTo(id));
            Assert.That(anchor.Cell, Is.EqualTo(cell));
        }
    }
}
