using System;
using System.Collections.Generic;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.Customers;
using VRMGames.CartridgeAndCloud.Domain.Products;

namespace VRMGames.CartridgeAndCloud.Infrastructure.Customers
{
    [CreateAssetMenu(
        menuName = "Cartridge & Cloud/Customers/Customer Profile",
        fileName = "CC_CustomerProfile_")]
    public sealed class CustomerProfileAsset : ScriptableObject
    {
        [SerializeField]
        private string _customerProfileId;

        [SerializeField]
        private string _displayNameKey;

        [SerializeField]
        private List<string> _preferredCategoryIds =
            new List<string>();

        [SerializeField]
        [Min(1)]
        private int _spawnWeight = 1;

        [SerializeField]
        [Min(1)]
        private int _patienceSeconds = 30;

        [SerializeField]
        [Min(1)]
        private int _browseStopCount = 2;

        [SerializeField]
        [Min(0.1f)]
        private float _walkSpeed = 1.8f;

        [SerializeField]
        private GameObject _technicalPrefab;

        public string CustomerProfileId => _customerProfileId;
        public string DisplayNameKey => _displayNameKey;
        public IReadOnlyList<string> PreferredCategoryIds =>
            _preferredCategoryIds;
        public int SpawnWeight => _spawnWeight;
        public int PatienceSeconds => _patienceSeconds;
        public int BrowseStopCount => _browseStopCount;
        public float WalkSpeed => _walkSpeed;
        public GameObject TechnicalPrefab => _technicalPrefab;

        public CustomerProfile BuildProfile()
        {
            List<ProductCategoryId> categories =
                new List<ProductCategoryId>(
                    _preferredCategoryIds.Count);

            foreach (string categoryId in _preferredCategoryIds)
            {
                categories.Add(new ProductCategoryId(categoryId));
            }

            return new CustomerProfile(
                new CustomerProfileId(_customerProfileId),
                _displayNameKey,
                categories,
                _spawnWeight,
                _patienceSeconds,
                _browseStopCount,
                _walkSpeed);
        }

        public void Configure(
            string customerProfileId,
            string displayNameKey,
            IEnumerable<string> preferredCategoryIds,
            int spawnWeight,
            int patienceSeconds,
            int browseStopCount,
            float walkSpeed,
            GameObject technicalPrefab = null)
        {
            if (preferredCategoryIds == null)
            {
                throw new ArgumentNullException(
                    nameof(preferredCategoryIds));
            }

            _customerProfileId = customerProfileId;
            _displayNameKey = displayNameKey;
            _preferredCategoryIds =
                new List<string>(preferredCategoryIds);
            _spawnWeight = spawnWeight;
            _patienceSeconds = patienceSeconds;
            _browseStopCount = browseStopCount;
            _walkSpeed = walkSpeed;
            _technicalPrefab = technicalPrefab;
        }
    }
}
