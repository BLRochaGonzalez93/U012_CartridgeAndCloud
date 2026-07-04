using System;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.Grid;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode
{
    public sealed class GridModelTests
    {
        [TestCase(0, 2)]
        [TestCase(2, 0)]
        public void Constructor_InvalidDimension_Throws(
            int width,
            int depth)
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new GridSize(
                    width,
                    depth));
        }

        [Test]
        public void GetOriented_QuarterTurnSwapsAxes()
        {
            GridSize size =
                new GridSize(4, 2);

            Assert.That(
                size.GetOriented(
                    GridRotation.Degrees90),
                Is.EqualTo(
                    new GridSize(2, 4)));

            Assert.That(
                size.GetOriented(
                    GridRotation.Degrees270),
                Is.EqualTo(
                    new GridSize(2, 4)));
        }

        [Test]
        public void GetOriented_HalfTurnKeepsAxes()
        {
            GridSize size =
                new GridSize(4, 2);

            Assert.That(
                size.GetOriented(
                    GridRotation.Degrees180),
                Is.EqualTo(size));
        }

        [Test]
        public void RotateClockwise_CyclesQuarterTurns()
        {
            GridRotation rotation =
                GridRotation.Degrees0;

            rotation =
                rotation.RotateClockwise();

            Assert.That(
                rotation,
                Is.EqualTo(
                    GridRotation.Degrees90));

            rotation =
                rotation.RotateClockwise()
                        .RotateClockwise()
                        .RotateClockwise();

            Assert.That(
                rotation,
                Is.EqualTo(
                    GridRotation.Degrees0));
        }

        [Test]
        public void RotateCounterClockwise_WrapsTo270()
        {
            Assert.That(
                GridRotation.Degrees0
                    .RotateCounterClockwise(),
                Is.EqualTo(
                    GridRotation.Degrees270));
        }

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
