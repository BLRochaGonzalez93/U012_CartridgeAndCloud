using System;
using System.Collections.Generic;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.Customers;

namespace VRMGames.CartridgeAndCloud.Infrastructure.Customers
{
    [CreateAssetMenu(
        menuName = "Cartridge & Cloud/Customers/Customer Profile Catalog",
        fileName = "CC_CustomerProfileCatalog_")]
    public sealed class CustomerProfileCatalogAsset : ScriptableObject
    {
        [SerializeField]
        private List<CustomerProfileAsset> _profiles =
            new List<CustomerProfileAsset>();

        public IReadOnlyList<CustomerProfileAsset> Profiles => _profiles;

        public CustomerProfileRegistry BuildRegistry()
        {
            List<CustomerProfile> profiles =
                new List<CustomerProfile>(_profiles.Count);

            foreach (CustomerProfileAsset profile in _profiles)
            {
                if (profile == null)
                {
                    throw new InvalidOperationException(
                        "Customer profile catalog contains a missing reference.");
                }

                profiles.Add(profile.BuildProfile());
            }

            return new CustomerProfileRegistry(profiles);
        }

        public void Configure(
            IEnumerable<CustomerProfileAsset> profiles)
        {
            if (profiles == null)
            {
                throw new ArgumentNullException(nameof(profiles));
            }

            _profiles = new List<CustomerProfileAsset>(profiles);
        }
    }
}
