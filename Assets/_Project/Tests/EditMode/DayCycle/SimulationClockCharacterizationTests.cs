using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.DayCycle;
using VRMGames.CartridgeAndCloud.Domain.DayCycle;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.DayCycle
{
    public sealed class SimulationClockCharacterizationTests
    {
        [Test]
        public void CHAR_TIM_001_NormalSpeedCompletes300SecondDay()
        {
            SimulationClock clock = Clock(
                SimulationSpeedPolicy.Normal);

            SimulationClockTickResult beforeEnd =
                clock.Tick(299d, false);
            SimulationClockTickResult end =
                clock.Tick(1d, false);

            Assert.That(
                beforeEnd.CurrentElapsedSeconds,
                Is.EqualTo(299));
            Assert.That(end.ReachedEnd, Is.True);
            Assert.That(
                clock.ElapsedWholeSeconds,
                Is.EqualTo(300));
        }

        [Test]
        public void CHAR_TIM_002_HalfSpeedAdvancesProportionally()
        {
            SimulationClock clock = Clock(
                SimulationSpeedPolicy.Half);

            clock.Tick(20d, false);

            Assert.That(
                clock.ElapsedWholeSeconds,
                Is.EqualTo(10));
        }

        [Test]
        public void CHAR_TIM_003_DoubleSpeedAdvancesProportionally()
        {
            SimulationClock clock = Clock(
                SimulationSpeedPolicy.Double);

            clock.Tick(20d, false);

            Assert.That(
                clock.ElapsedWholeSeconds,
                Is.EqualTo(40));
        }

        [Test]
        public void CHAR_TIM_004_QuadrupleSpeedAdvancesProportionally()
        {
            SimulationClock clock = Clock(
                SimulationSpeedPolicy.Quadruple);

            clock.Tick(20d, false);

            Assert.That(
                clock.ElapsedWholeSeconds,
                Is.EqualTo(80));
        }

        [Test]
        public void CHAR_TIM_005_PauseFreezesAndResumeKeepsSpeed()
        {
            SimulationClock clock = Clock(
                SimulationSpeedPolicy.Quadruple);
            PauseService pause =
                new PauseService();

            pause.RequestPause("test-menu");
            clock.Tick(10d, pause.IsPaused);

            Assert.That(
                clock.ElapsedWholeSeconds,
                Is.Zero);
            Assert.That(
                clock.SelectedSpeedMultiplier,
                Is.EqualTo(
                    SimulationSpeedPolicy.Quadruple));

            pause.ReleasePause("test-menu");
            clock.Tick(1d, pause.IsPaused);

            Assert.That(
                clock.ElapsedWholeSeconds,
                Is.EqualTo(4));
            Assert.That(
                clock.SelectedSpeedMultiplier,
                Is.EqualTo(
                    SimulationSpeedPolicy.Quadruple));
        }

        [Test]
        public void CHAR_TIM_006_FractionalTicksKeepRemainder()
        {
            SimulationClock clock = Clock(
                SimulationSpeedPolicy.Half);

            clock.Tick(1d, false);
            Assert.That(
                clock.ElapsedWholeSeconds,
                Is.Zero);

            clock.Tick(1d, false);
            Assert.That(
                clock.ElapsedWholeSeconds,
                Is.EqualTo(1));
        }

        [Test]
        public void CHAR_TIM_007_EndIsReportedOnlyOnce()
        {
            SimulationClock clock =
                new SimulationClock();
            clock.Synchronize(
                2,
                0,
                SimulationSpeedPolicy.Quadruple);

            SimulationClockTickResult first =
                clock.Tick(1d, false);
            SimulationClockTickResult second =
                clock.Tick(1d, false);

            Assert.That(first.ReachedEnd, Is.True);
            Assert.That(second.ReachedEnd, Is.False);
            Assert.That(
                second.CurrentElapsedSeconds,
                Is.EqualTo(2));
        }

        [Test]
        public void PauseService_NestedOwnersRequireFullRelease()
        {
            PauseService pause =
                new PauseService();

            pause.RequestPause("menu");
            pause.RequestPause("confirmation");
            pause.ReleasePause("menu");

            Assert.That(pause.IsPaused, Is.True);
            Assert.That(
                pause.ActiveRequestCount,
                Is.EqualTo(1));

            pause.ReleasePause("confirmation");
            Assert.That(pause.IsPaused, Is.False);
        }

        private static SimulationClock Clock(
            float speed)
        {
            SimulationClock clock =
                new SimulationClock();
            clock.Synchronize(300, 0, speed);
            return clock;
        }
    }
}
