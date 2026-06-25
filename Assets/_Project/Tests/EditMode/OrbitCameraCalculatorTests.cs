using System;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.Camera;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode
{
    public sealed class OrbitCameraCalculatorTests
    {
        [Test]
        public void Constraints_InvalidPitchRange_Throws()
        {
            Assert.Throws<ArgumentException>(
                () => new OrbitCameraConstraints(
                    80f,
                    20f,
                    5f,
                    20f));
        }

        [Test]
        public void Constraints_InvalidDistanceRange_Throws()
        {
            Assert.Throws<ArgumentException>(
                () => new OrbitCameraConstraints(
                    20f,
                    80f,
                    20f,
                    5f));
        }

        [Test]
        public void CreateState_NormalizesAndClamps()
        {
            OrbitCameraConstraints constraints =
                CreateConstraints();

            OrbitCameraState state =
                OrbitCameraCalculator.CreateState(
                    450f,
                    90f,
                    100f,
                    constraints);

            Assert.That(state.YawDegrees, Is.EqualTo(90f));
            Assert.That(state.PitchDegrees, Is.EqualTo(75f));
            Assert.That(state.Distance, Is.EqualTo(24f));
        }

        [Test]
        public void ApplyOrbit_NormalizesYawAndClampsPitch()
        {
            OrbitCameraConstraints constraints =
                CreateConstraints();

            OrbitCameraState initial =
                OrbitCameraCalculator.CreateState(
                    350f,
                    45f,
                    18f,
                    constraints);

            OrbitCameraState result =
                OrbitCameraCalculator.ApplyOrbit(
                    initial,
                    30f,
                    -100f,
                    constraints);

            Assert.That(result.YawDegrees, Is.EqualTo(20f));
            Assert.That(result.PitchDegrees, Is.EqualTo(25f));
            Assert.That(result.Distance, Is.EqualTo(18f));
        }

        [Test]
        public void ApplyZoom_PositiveDeltaMovesCloser()
        {
            OrbitCameraConstraints constraints =
                CreateConstraints();

            OrbitCameraState initial =
                OrbitCameraCalculator.CreateState(
                    0f,
                    45f,
                    18f,
                    constraints);

            OrbitCameraState result =
                OrbitCameraCalculator.ApplyZoom(
                    initial,
                    3f,
                    constraints);

            Assert.That(result.Distance, Is.EqualTo(15f));
        }

        [Test]
        public void ApplyZoom_ClampsBothLimits()
        {
            OrbitCameraConstraints constraints =
                CreateConstraints();

            OrbitCameraState initial =
                OrbitCameraCalculator.CreateState(
                    0f,
                    45f,
                    18f,
                    constraints);

            OrbitCameraState closest =
                OrbitCameraCalculator.ApplyZoom(
                    initial,
                    100f,
                    constraints);

            OrbitCameraState farthest =
                OrbitCameraCalculator.ApplyZoom(
                    initial,
                    -100f,
                    constraints);

            Assert.That(closest.Distance, Is.EqualTo(6f));
            Assert.That(farthest.Distance, Is.EqualTo(24f));
        }

        private static OrbitCameraConstraints CreateConstraints()
        {
            return new OrbitCameraConstraints(
                25f,
                75f,
                6f,
                24f);
        }
    }
}
