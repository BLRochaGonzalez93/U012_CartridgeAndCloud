using System;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.Grid;
using VRMGames.CartridgeAndCloud.Domain.Grid;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode
{
    public sealed class GridProjectionCalculatorTests
    {
        [Test]
        public void WorldToCell_UsesHalfOpenCellsForPositiveAndNegativeValues()
        {
            Assert.That(
                GridProjectionCalculator.WorldToCell(
                    0.49f,
                    0f),
                Is.EqualTo(
                    new GridCoordinate(0, 0)));

            Assert.That(
                GridProjectionCalculator.WorldToCell(
                    0.5f,
                    -0.01f),
                Is.EqualTo(
                    new GridCoordinate(1, -1)));

            Assert.That(
                GridProjectionCalculator.WorldToCell(
                    -0.5f,
                    -0.5f),
                Is.EqualTo(
                    new GridCoordinate(-1, -1)));
        }

        [Test]
        public void CellToWorldCenter_ReturnsCenterOfHalfMeterCell()
        {
            GridWorldPosition position =
                GridProjectionCalculator
                    .CellToWorldCenter(
                        new GridCoordinate(0, 0));

            Assert.That(
                position.X,
                Is.EqualTo(0.25f)
                  .Within(0.0001f));

            Assert.That(
                position.Z,
                Is.EqualTo(0.25f)
                  .Within(0.0001f));
        }

        [Test]
        public void Projection_WithOriginOffset_RoundTripsCellCenter()
        {
            GridCoordinate expected =
                new GridCoordinate(-3, 6);

            GridWorldPosition center =
                GridProjectionCalculator
                    .CellToWorldCenter(
                        expected,
                        originX: 12f,
                        originZ: -8f);

            GridCoordinate result =
                GridProjectionCalculator
                    .WorldToCell(
                        center.X,
                        center.Z,
                        originX: 12f,
                        originZ: -8f);

            Assert.That(
                result,
                Is.EqualTo(expected));
        }

        [Test]
        public void Projection_InvalidCellSize_Throws()
        {
            Assert.Throws<
                ArgumentOutOfRangeException>(
                () =>
                    GridProjectionCalculator
                        .WorldToCell(
                            0f,
                            0f,
                            cellSize: 0f));
        }
    }
}
