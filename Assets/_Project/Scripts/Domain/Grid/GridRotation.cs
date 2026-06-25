using System;

namespace VRMGames.CartridgeAndCloud.Domain.Grid
{
    public enum GridRotation
    {
        Degrees0 = 0,
        Degrees90 = 1,
        Degrees180 = 2,
        Degrees270 = 3
    }

    public static class GridRotationExtensions
    {
        public static GridRotation RotateClockwise(
            this GridRotation rotation)
        {
            Validate(rotation);

            return (GridRotation)(
                ((int)rotation + 1) % 4);
        }

        public static GridRotation RotateCounterClockwise(
            this GridRotation rotation)
        {
            Validate(rotation);

            return (GridRotation)(
                ((int)rotation + 3) % 4);
        }

        public static int ToDegrees(
            this GridRotation rotation)
        {
            Validate(rotation);
            return (int)rotation * 90;
        }

        public static void Validate(
            GridRotation rotation)
        {
            if (rotation < GridRotation.Degrees0 ||
                rotation > GridRotation.Degrees270)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(rotation),
                    rotation,
                    "Grid rotation must be a 90-degree quarter turn.");
            }
        }
    }
}
