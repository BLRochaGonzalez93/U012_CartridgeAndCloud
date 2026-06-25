using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.Grid;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode
{
    public sealed class GridFootprintAndBoundsTests
    {
        [Test]
        public void GetOccupiedCells_ZeroRotationUsesBaseSize()
        {
            GridFootprint footprint =
                new GridFootprint(
                    new GridSize(2, 3));

            GridCoordinate[] cells =
                footprint.GetOccupiedCells(
                    new GridCoordinate(10, 20),
                    GridRotation.Degrees0);

            Assert.That(cells, Has.Length.EqualTo(6));
            Assert.That(
                cells,
                Does.Contain(
                    new GridCoordinate(10, 20)));
            Assert.That(
                cells,
                Does.Contain(
                    new GridCoordinate(11, 22)));
        }

        [Test]
        public void GetOccupiedCells_QuarterTurnSwapsExtent()
        {
            GridFootprint footprint =
                new GridFootprint(
                    new GridSize(3, 1));

            GridCoordinate[] cells =
                footprint.GetOccupiedCells(
                    new GridCoordinate(-2, 4),
                    GridRotation.Degrees90);

            CollectionAssert.AreEquivalent(
                new[]
                {
                    new GridCoordinate(-2, 4),
                    new GridCoordinate(-2, 5),
                    new GridCoordinate(-2, 6)
                },
                cells);
        }

        [Test]
        public void ContainsCell_UsesOrientedSize()
        {
            GridFootprint footprint =
                new GridFootprint(
                    new GridSize(3, 1));

            Assert.That(
                footprint.ContainsCell(
                    new GridCoordinate(0, 0),
                    GridRotation.Degrees90,
                    new GridCoordinate(0, 2)),
                Is.True);

            Assert.That(
                footprint.ContainsCell(
                    new GridCoordinate(0, 0),
                    GridRotation.Degrees90,
                    new GridCoordinate(1, 0)),
                Is.False);
        }

        [Test]
        public void Bounds_ContainsFootprintWhenFullyInside()
        {
            GridBounds bounds =
                new GridBounds(
                    new GridCoordinate(0, 0),
                    new GridSize(10, 10));

            GridFootprint footprint =
                new GridFootprint(
                    new GridSize(4, 2));

            Assert.That(
                bounds.ContainsFootprint(
                    new GridCoordinate(6, 8),
                    footprint,
                    GridRotation.Degrees0),
                Is.True);
        }

        [Test]
        public void Bounds_RejectsFootprintOverflow()
        {
            GridBounds bounds =
                new GridBounds(
                    new GridCoordinate(0, 0),
                    new GridSize(10, 10));

            GridFootprint footprint =
                new GridFootprint(
                    new GridSize(4, 2));

            Assert.That(
                bounds.ContainsFootprint(
                    new GridCoordinate(7, 8),
                    footprint,
                    GridRotation.Degrees0),
                Is.False);
        }
    }
}
