using System;

namespace VRMGames.CartridgeAndCloud.Application.Camera
{
    public readonly struct OrbitCameraConstraints
    {
        public float MinimumPitchDegrees { get; }
        public float MaximumPitchDegrees { get; }
        public float MinimumDistance { get; }
        public float MaximumDistance { get; }

        public OrbitCameraConstraints(
            float minimumPitchDegrees,
            float maximumPitchDegrees,
            float minimumDistance,
            float maximumDistance)
        {
            ValidateFinite(
                minimumPitchDegrees,
                nameof(minimumPitchDegrees));
            ValidateFinite(
                maximumPitchDegrees,
                nameof(maximumPitchDegrees));
            ValidateFinite(
                minimumDistance,
                nameof(minimumDistance));
            ValidateFinite(
                maximumDistance,
                nameof(maximumDistance));

            if (minimumPitchDegrees > maximumPitchDegrees)
            {
                throw new ArgumentException(
                    "Minimum pitch cannot exceed maximum pitch.");
            }

            if (minimumDistance <= 0f)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(minimumDistance),
                    minimumDistance,
                    "Minimum distance must be greater than zero.");
            }

            if (minimumDistance > maximumDistance)
            {
                throw new ArgumentException(
                    "Minimum distance cannot exceed maximum distance.");
            }

            MinimumPitchDegrees = minimumPitchDegrees;
            MaximumPitchDegrees = maximumPitchDegrees;
            MinimumDistance = minimumDistance;
            MaximumDistance = maximumDistance;
        }

        private static void ValidateFinite(
            float value,
            string parameterName)
        {
            if (float.IsNaN(value) ||
                float.IsInfinity(value))
            {
                throw new ArgumentOutOfRangeException(
                    parameterName,
                    value,
                    "Value must be finite.");
            }
        }
    }
}
