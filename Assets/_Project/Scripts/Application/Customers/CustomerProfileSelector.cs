using System;
using VRMGames.CartridgeAndCloud.Domain.Customers;

namespace VRMGames.CartridgeAndCloud.Application.Customers
{
    public sealed class CustomerProfileSelector
    {
        private readonly CustomerProfileRegistry _profiles;

        public CustomerProfileSelector(CustomerProfileRegistry profiles)
        {
            _profiles = profiles ??
                throw new ArgumentNullException(nameof(profiles));

            if (_profiles.Count == 0)
            {
                throw new ArgumentException(
                    "At least one customer profile is required.",
                    nameof(profiles));
            }
        }

        public CustomerProfile SelectByRoll(int roll)
        {
            if (roll < 0 || roll >= _profiles.TotalSpawnWeight)
            {
                throw new ArgumentOutOfRangeException(nameof(roll));
            }

            int cursor = 0;
            foreach (CustomerProfile profile in _profiles.Profiles)
            {
                cursor += profile.SpawnWeight;
                if (roll < cursor)
                {
                    return profile;
                }
            }

            throw new InvalidOperationException(
                "Profile selection could not resolve the supplied roll.");
        }
    }
}
