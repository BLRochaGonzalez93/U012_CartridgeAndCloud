using System;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.Grid;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode
{
    public sealed class GridSizeAndRotationTests
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
    }
}
