using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VRMGames.CartridgeAndCloud.Domain.Customers
{
    public sealed class CustomerProfileRegistry
    {
        private readonly Dictionary<CustomerProfileId, CustomerProfile>
            _profilesById;
        private readonly ReadOnlyCollection<CustomerProfile> _profiles;

        public IReadOnlyList<CustomerProfile> Profiles => _profiles;

        public int Count => _profiles.Count;

        public int TotalSpawnWeight { get; }

        public CustomerProfileRegistry(
            IEnumerable<CustomerProfile> profiles)
        {
            if (profiles == null)
            {
                throw new ArgumentNullException(nameof(profiles));
            }

            _profilesById =
                new Dictionary<CustomerProfileId, CustomerProfile>();
            List<CustomerProfile> ordered =
                new List<CustomerProfile>();
            int totalWeight = 0;

            foreach (CustomerProfile profile in profiles)
            {
                if (profile == null)
                {
                    throw new ArgumentException(
                        "Customer profiles cannot contain null.",
                        nameof(profiles));
                }

                if (_profilesById.ContainsKey(profile.Id))
                {
                    throw new ArgumentException(
                        $"Customer profile ID {profile.Id} is duplicated.",
                        nameof(profiles));
                }

                _profilesById.Add(profile.Id, profile);
                ordered.Add(profile);
                checked
                {
                    totalWeight += profile.SpawnWeight;
                }
            }

            ordered.Sort(
                (left, right) => StringComparer.Ordinal.Compare(
                    left.Id.Value,
                    right.Id.Value));

            _profiles =
                new ReadOnlyCollection<CustomerProfile>(ordered);
            TotalSpawnWeight = totalWeight;
        }

        public bool Contains(CustomerProfileId id)
        {
            return _profilesById.ContainsKey(id);
        }

        public bool TryGet(
            CustomerProfileId id,
            out CustomerProfile profile)
        {
            return _profilesById.TryGetValue(id, out profile);
        }

        public CustomerProfile Get(CustomerProfileId id)
        {
            if (!_profilesById.TryGetValue(id, out CustomerProfile profile))
            {
                throw new KeyNotFoundException(
                    $"Customer profile {id} was not found.");
            }

            return profile;
        }
    }
}
