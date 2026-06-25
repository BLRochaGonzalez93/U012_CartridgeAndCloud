using System;

namespace VRMGames.CartridgeAndCloud.Domain.Customers
{
    public sealed class CustomerSpawnPolicy
    {
        public int MaxActiveCustomers { get; }

        public int ArrivalIntervalSeconds { get; }

        public CustomerSpawnPolicy(
            int maxActiveCustomers,
            int arrivalIntervalSeconds)
        {
            if (maxActiveCustomers <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(maxActiveCustomers));
            }

            if (arrivalIntervalSeconds <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(arrivalIntervalSeconds));
            }

            MaxActiveCustomers = maxActiveCustomers;
            ArrivalIntervalSeconds = arrivalIntervalSeconds;
        }
    }
}
