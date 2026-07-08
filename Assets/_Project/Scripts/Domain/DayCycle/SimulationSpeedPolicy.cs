using System;
using System.Globalization;

namespace VRMGames.CartridgeAndCloud.Domain.DayCycle
{
    public static class SimulationSpeedPolicy
    {
        public const float Half = 0.5f;
        public const float Normal = 1f;
        public const float Double = 2f;
        public const float Quadruple = 4f;

        private const float ComparisonTolerance = 0.0001f;

        public static bool IsSupported(float multiplier)
        {
            return Is(multiplier, Half) ||
                   Is(multiplier, Normal) ||
                   Is(multiplier, Double) ||
                   Is(multiplier, Quadruple);
        }

        public static float RequireSupported(
            float multiplier,
            string parameterName)
        {
            if (float.IsNaN(multiplier) ||
                float.IsInfinity(multiplier) ||
                !IsSupported(multiplier))
            {
                throw new ArgumentOutOfRangeException(
                    parameterName,
                    multiplier,
                    "Simulation speed must be x0.5, x1, x2 or x4.");
            }

            if (Is(multiplier, Half))
            {
                return Half;
            }

            if (Is(multiplier, Double))
            {
                return Double;
            }

            if (Is(multiplier, Quadruple))
            {
                return Quadruple;
            }

            return Normal;
        }

        public static float NormalizeOrDefault(
            float multiplier)
        {
            return IsSupported(multiplier)
                ? RequireSupported(
                    multiplier,
                    nameof(multiplier))
                : Normal;
        }

        public static string Format(float multiplier)
        {
            float normalized = RequireSupported(
                multiplier,
                nameof(multiplier));

            return "x" + normalized.ToString(
                normalized == Half ? "0.0" : "0",
                CultureInfo.InvariantCulture);
        }

        private static bool Is(
            float left,
            float right)
        {
            return Math.Abs(left - right) <=
                   ComparisonTolerance;
        }
    }
}
