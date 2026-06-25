using System;

namespace VRMGames.CartridgeAndCloud.Application.PlayerMovement
{
    public static class PlanarMovementCalculator
    {
        private const float DistanceEpsilon = 0.0001f;

        public static PlanarMovementStep Calculate(
            float currentX,
            float currentZ,
            float targetX,
            float targetZ,
            float moveSpeed,
            float deltaTime,
            float stoppingDistance)
        {
            ValidateNonNegativeFinite(moveSpeed, nameof(moveSpeed));
            ValidateNonNegativeFinite(deltaTime, nameof(deltaTime));
            ValidateNonNegativeFinite(
                stoppingDistance,
                nameof(stoppingDistance));

            float offsetX = targetX - currentX;
            float offsetZ = targetZ - currentZ;
            float distance = (float)Math.Sqrt(
                offsetX * offsetX +
                offsetZ * offsetZ);

            if (distance <= stoppingDistance + DistanceEpsilon)
            {
                return new PlanarMovementStep(0f, 0f, true);
            }

            if (moveSpeed == 0f || deltaTime == 0f)
            {
                return new PlanarMovementStep(0f, 0f, false);
            }

            float availableDistance = distance - stoppingDistance;
            float requestedDistance = moveSpeed * deltaTime;
            float stepDistance = Math.Min(
                requestedDistance,
                availableDistance);

            float inverseDistance = 1f / distance;
            float deltaX = offsetX * inverseDistance * stepDistance;
            float deltaZ = offsetZ * inverseDistance * stepDistance;
            bool reachesStoppingDistance =
                distance - stepDistance <=
                stoppingDistance + DistanceEpsilon;

            return new PlanarMovementStep(
                deltaX,
                deltaZ,
                reachesStoppingDistance);
        }

        private static void ValidateNonNegativeFinite(
            float value,
            string parameterName)
        {
            if (value < 0f ||
                float.IsNaN(value) ||
                float.IsInfinity(value))
            {
                throw new ArgumentOutOfRangeException(
                    parameterName,
                    value,
                    "Value must be finite and non-negative.");
            }
        }
    }
}
