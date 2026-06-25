using System;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.PlayerMovement;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode
{
    public sealed class PlanarMovementCalculatorTests
    {
        [Test]
        public void Calculate_WhenInsideStoppingDistance_ReturnsReached()
        {
            PlanarMovementStep step =
                PlanarMovementCalculator.Calculate(
                    0f,
                    0f,
                    0.05f,
                    0f,
                    4f,
                    1f,
                    0.1f);

            Assert.That(step.HasMovement, Is.False);
            Assert.That(
                step.IsWithinStoppingDistance,
                Is.True);
        }

        [Test]
        public void Calculate_AlongAxis_ReturnsExpectedDelta()
        {
            PlanarMovementStep step =
                PlanarMovementCalculator.Calculate(
                    0f,
                    0f,
                    10f,
                    0f,
                    4f,
                    0.5f,
                    0f);

            Assert.That(step.DeltaX, Is.EqualTo(2f).Within(0.0001f));
            Assert.That(step.DeltaZ, Is.EqualTo(0f).Within(0.0001f));
        }

        [Test]
        public void Calculate_Diagonal_NormalizesDirection()
        {
            PlanarMovementStep step =
                PlanarMovementCalculator.Calculate(
                    0f,
                    0f,
                    10f,
                    10f,
                    2f,
                    0.5f,
                    0f);

            float magnitude = (float)Math.Sqrt(
                step.DeltaX * step.DeltaX +
                step.DeltaZ * step.DeltaZ);

            Assert.That(magnitude, Is.EqualTo(1f).Within(0.0001f));
            Assert.That(step.DeltaX, Is.EqualTo(step.DeltaZ).Within(0.0001f));
        }

        [Test]
        public void Calculate_WhenStepWouldOvershoot_StopsAtRadius()
        {
            PlanarMovementStep step =
                PlanarMovementCalculator.Calculate(
                    0f,
                    0f,
                    1f,
                    0f,
                    10f,
                    1f,
                    0.2f);

            Assert.That(step.DeltaX, Is.EqualTo(0.8f).Within(0.0001f));
            Assert.That(
                step.IsWithinStoppingDistance,
                Is.True);
        }

        [TestCase(-1f, 0.1f, 0.1f)]
        [TestCase(1f, -0.1f, 0.1f)]
        [TestCase(1f, 0.1f, -0.1f)]
        public void Calculate_InvalidArguments_Throws(
            float speed,
            float deltaTime,
            float stoppingDistance)
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => PlanarMovementCalculator.Calculate(
                    0f,
                    0f,
                    1f,
                    1f,
                    speed,
                    deltaTime,
                    stoppingDistance));
        }
    }
}
