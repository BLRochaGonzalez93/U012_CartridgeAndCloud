using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.Grid;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode
{
    public sealed class GridCoordinateTests
    {
        [Test]
        public void Constructor_StoresBothAxes()
        {
            GridCoordinate coordinate =
                new GridCoordinate(4, -3);

            Assert.That(coordinate.X, Is.EqualTo(4));
            Assert.That(coordinate.Z, Is.EqualTo(-3));
        }

        [Test]
        public void Offset_ReturnsTranslatedCoordinate()
        {
            GridCoordinate coordinate =
                new GridCoordinate(2, 5);

            GridCoordinate result =
                coordinate.Offset(-3, 4);

            Assert.That(
                result,
                Is.EqualTo(
                    new GridCoordinate(-1, 9)));
        }

        [Test]
        public void Equality_UsesBothAxes()
        {
            GridCoordinate coordinate =
                new GridCoordinate(3, 7);

            Assert.That(
                coordinate ==
                new GridCoordinate(3, 7),
                Is.True);

            Assert.That(
                coordinate !=
                new GridCoordinate(3, 8),
                Is.True);
        }
    }
}
