using System;

namespace VRMGames.CartridgeAndCloud.Domain.Customers
{
    public sealed class CustomerNavigationTarget
    {
        public CustomerNavigationPointId PointId { get; }

        public CustomerNavigationTargetType Type { get; }

        public int DwellSeconds { get; }

        public CustomerNavigationTarget(
            CustomerNavigationPointId pointId,
            CustomerNavigationTargetType type,
            int dwellSeconds)
        {
            if (string.IsNullOrWhiteSpace(pointId.Value))
            {
                throw new ArgumentException(
                    "Navigation point ID must be initialized.",
                    nameof(pointId));
            }

            if (dwellSeconds < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(dwellSeconds));
            }

            if (type != CustomerNavigationTargetType.Browse &&
                dwellSeconds != 0)
            {
                throw new ArgumentException(
                    "Entry and exit targets cannot define dwell time.",
                    nameof(dwellSeconds));
            }

            PointId = pointId;
            Type = type;
            DwellSeconds = dwellSeconds;
        }
    }
}
