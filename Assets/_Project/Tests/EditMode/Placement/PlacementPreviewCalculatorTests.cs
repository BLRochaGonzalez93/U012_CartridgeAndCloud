using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.Placement;
using VRMGames.CartridgeAndCloud.Domain.Grid;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode
{
    public sealed class PlacementPreviewCalculatorTests
    {
        private static GridBounds CreateBounds()
        {
            return new GridBounds(
                new GridCoordinate(0, 0),
                new GridSize(10, 10));
        }

        [Test]
        public void Calculate_InsideBounds_ReturnsValidState()
        {
            PlacementPreviewState state =
                PlacementPreviewCalculator.Calculate(
                    new GridCoordinate(3, 4),
                    new GridSize(4, 2),
                    GridRotation.Degrees0,
                    CreateBounds());

            Assert.That(state.IsWithinBounds, Is.True);
            Assert.That(
                state.OrientedSize,
                Is.EqualTo(new GridSize(4, 2)));
        }

        [Test]
        public void Calculate_Overflow_ReturnsInvalidState()
        {
            PlacementPreviewState state =
                PlacementPreviewCalculator.Calculate(
                    new GridCoordinate(7, 9),
                    new GridSize(4, 2),
                    GridRotation.Degrees0,
                    CreateBounds());

            Assert.That(state.IsWithinBounds, Is.False);
        }

        [Test]
        public void Calculate_QuarterTurnSwapsOrientedSize()
        {
            PlacementPreviewState state =
                PlacementPreviewCalculator.Calculate(
                    new GridCoordinate(0, 0),
                    new GridSize(4, 2),
                    GridRotation.Degrees90,
                    CreateBounds());

            Assert.That(
                state.OrientedSize,
                Is.EqualTo(new GridSize(2, 4)));
        }

        [Test]
        public void Calculate_ReturnsFootprintCenterOffsets()
        {
            PlacementPreviewState state =
                PlacementPreviewCalculator.Calculate(
                    new GridCoordinate(0, 0),
                    new GridSize(4, 2),
                    GridRotation.Degrees0,
                    CreateBounds());

            Assert.That(state.CenterOffsetCellsX, Is.EqualTo(1.5f));
            Assert.That(state.CenterOffsetCellsZ, Is.EqualTo(0.5f));
        }
    }
}
