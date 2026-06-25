using System;

namespace VRMGames.CartridgeAndCloud.Application.Camera
{
    public static class OrbitCameraCalculator
    {
        public static OrbitCameraState CreateState(
            float yawDegrees,
            float pitchDegrees,
            float distance,
            OrbitCameraConstraints constraints)
        {
            ValidateFinite(yawDegrees, nameof(yawDegrees));
            ValidateFinite(pitchDegrees, nameof(pitchDegrees));
            ValidateFinite(distance, nameof(distance));

            return new OrbitCameraState(
                NormalizeYaw(yawDegrees),
                Clamp(
                    pitchDegrees,
                    constraints.MinimumPitchDegrees,
                    constraints.MaximumPitchDegrees),
                Clamp(
                    distance,
                    constraints.MinimumDistance,
                    constraints.MaximumDistance));
        }

        public static OrbitCameraState ApplyOrbit(
            OrbitCameraState state,
            float yawDeltaDegrees,
            float pitchDeltaDegrees,
            OrbitCameraConstraints constraints)
        {
            ValidateFinite(
                yawDeltaDegrees,
                nameof(yawDeltaDegrees));
            ValidateFinite(
                pitchDeltaDegrees,
                nameof(pitchDeltaDegrees));

            return CreateState(
                state.YawDegrees + yawDeltaDegrees,
                state.PitchDegrees + pitchDeltaDegrees,
                state.Distance,
                constraints);
        }

        public static OrbitCameraState ApplyZoom(
            OrbitCameraState state,
            float zoomDelta,
            OrbitCameraConstraints constraints)
        {
            ValidateFinite(zoomDelta, nameof(zoomDelta));

            return CreateState(
                state.YawDegrees,
                state.PitchDegrees,
                state.Distance - zoomDelta,
                constraints);
        }

        public static float NormalizeYaw(float yawDegrees)
        {
            ValidateFinite(yawDegrees, nameof(yawDegrees));

            float normalized = yawDegrees % 360f;

            if (normalized < 0f)
            {
                normalized += 360f;
            }

            return normalized;
        }

        private static float Clamp(
            float value,
            float minimum,
            float maximum)
        {
            return Math.Min(
                Math.Max(value, minimum),
                maximum);
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
