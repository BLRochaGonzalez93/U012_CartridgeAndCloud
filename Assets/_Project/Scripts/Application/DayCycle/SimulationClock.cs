using System;
using VRMGames.CartridgeAndCloud.Domain.DayCycle;

namespace VRMGames.CartridgeAndCloud.Application.DayCycle
{
    public sealed class SimulationClockTickResult
    {
        public int PreviousElapsedSeconds { get; }
        public int CurrentElapsedSeconds { get; }
        public bool WholeSecondChanged =>
            PreviousElapsedSeconds != CurrentElapsedSeconds;
        public bool ReachedEnd { get; }

        public SimulationClockTickResult(
            int previousElapsedSeconds,
            int currentElapsedSeconds,
            bool reachedEnd)
        {
            if (previousElapsedSeconds < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(previousElapsedSeconds));
            }

            if (currentElapsedSeconds <
                previousElapsedSeconds)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(currentElapsedSeconds));
            }

            PreviousElapsedSeconds =
                previousElapsedSeconds;
            CurrentElapsedSeconds =
                currentElapsedSeconds;
            ReachedEnd = reachedEnd;
        }
    }

    public interface ISimulationClock
    {
        int DurationSeconds { get; }
        int ElapsedWholeSeconds { get; }
        double ElapsedSeconds { get; }
        float SelectedSpeedMultiplier { get; }
        bool IsComplete { get; }

        void Synchronize(
            int durationSeconds,
            int elapsedSeconds,
            float selectedSpeedMultiplier);

        void SetSpeed(float multiplier);

        SimulationClockTickResult Tick(
            double unscaledDeltaSeconds,
            bool isPaused);
    }

    public sealed class SimulationClock :
        ISimulationClock
    {
        private double _elapsedSeconds;

        public int DurationSeconds { get; private set; }

        public int ElapsedWholeSeconds =>
            Math.Min(
                DurationSeconds,
                (int)Math.Floor(
                    _elapsedSeconds + 0.0000001d));

        public double ElapsedSeconds =>
            _elapsedSeconds;

        public float SelectedSpeedMultiplier {
            get;
            private set;
        } = SimulationSpeedPolicy.Normal;

        public bool IsComplete =>
            DurationSeconds > 0 &&
            _elapsedSeconds >= DurationSeconds;

        public void Synchronize(
            int durationSeconds,
            int elapsedSeconds,
            float selectedSpeedMultiplier)
        {
            if (durationSeconds < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(durationSeconds));
            }

            if (elapsedSeconds < 0 ||
                elapsedSeconds > durationSeconds)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(elapsedSeconds));
            }

            DurationSeconds = durationSeconds;
            _elapsedSeconds = elapsedSeconds;
            SelectedSpeedMultiplier =
                SimulationSpeedPolicy.RequireSupported(
                    selectedSpeedMultiplier,
                    nameof(selectedSpeedMultiplier));
        }

        public void SetSpeed(float multiplier)
        {
            SelectedSpeedMultiplier =
                SimulationSpeedPolicy.RequireSupported(
                    multiplier,
                    nameof(multiplier));
        }

        public SimulationClockTickResult Tick(
            double unscaledDeltaSeconds,
            bool isPaused)
        {
            if (double.IsNaN(unscaledDeltaSeconds) ||
                double.IsInfinity(unscaledDeltaSeconds) ||
                unscaledDeltaSeconds < 0d)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(unscaledDeltaSeconds));
            }

            if (DurationSeconds < 1)
            {
                throw new InvalidOperationException(
                    "Simulation clock is not synchronized.");
            }

            int previous = ElapsedWholeSeconds;
            bool wasComplete = IsComplete;

            if (!isPaused &&
                !wasComplete &&
                unscaledDeltaSeconds > 0d)
            {
                double scaledDelta =
                    unscaledDeltaSeconds *
                    SelectedSpeedMultiplier;

                _elapsedSeconds = Math.Min(
                    DurationSeconds,
                    _elapsedSeconds + scaledDelta);
            }

            bool reachedEnd =
                !wasComplete && IsComplete;

            return new SimulationClockTickResult(
                previous,
                ElapsedWholeSeconds,
                reachedEnd);
        }
    }
}
