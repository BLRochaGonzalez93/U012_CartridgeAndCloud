using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.Customers;

namespace VRMGames.CartridgeAndCloud.Infrastructure.Customers
{
    [CreateAssetMenu(
        menuName = "Cartridge & Cloud/Customers/Spawn Settings",
        fileName = "CC_CustomerSpawnSettings_")]
    public sealed class CustomerSpawnSettingsAsset : ScriptableObject
    {
        [SerializeField]
        [Min(1)]
        private int _maxActiveCustomers = 6;

        [SerializeField]
        [Min(1)]
        private int _arrivalIntervalSeconds = 8;

        [SerializeField]
        private bool _destroyTechnicalViewOnExit = true;

        public int MaxActiveCustomers => _maxActiveCustomers;
        public int ArrivalIntervalSeconds => _arrivalIntervalSeconds;
        public bool DestroyTechnicalViewOnExit =>
            _destroyTechnicalViewOnExit;

        public CustomerSpawnPolicy BuildPolicy()
        {
            return new CustomerSpawnPolicy(
                _maxActiveCustomers,
                _arrivalIntervalSeconds);
        }

        public void Configure(
            int maxActiveCustomers,
            int arrivalIntervalSeconds,
            bool destroyTechnicalViewOnExit)
        {
            _maxActiveCustomers = maxActiveCustomers;
            _arrivalIntervalSeconds = arrivalIntervalSeconds;
            _destroyTechnicalViewOnExit = destroyTechnicalViewOnExit;
        }
    }
}
