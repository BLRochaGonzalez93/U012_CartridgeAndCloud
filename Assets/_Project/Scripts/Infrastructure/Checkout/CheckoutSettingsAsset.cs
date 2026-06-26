using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.Checkout;

namespace VRMGames.CartridgeAndCloud.Infrastructure.Checkout
{
    [CreateAssetMenu(
        menuName = "Cartridge & Cloud/Checkout/Settings",
        fileName = "CC_CheckoutSettings_")]
    public sealed class CheckoutSettingsAsset : ScriptableObject
    {
        [SerializeField, Min(1)]
        private int _maxQueueLength = 6;

        [SerializeField]
        private string _stationId =
            "checkout-station-technical";

        [SerializeField]
        private bool _openStationOnInitialize = true;

        public int MaxQueueLength => _maxQueueLength;

        public string StationId => _stationId;

        public bool OpenStationOnInitialize =>
            _openStationOnInitialize;

        public CheckoutPolicy BuildPolicy()
        {
            return new CheckoutPolicy(_maxQueueLength);
        }

        public CheckoutStation BuildStation()
        {
            CheckoutStation station =
                new CheckoutStation(
                    new CheckoutStationId(_stationId));

            if (_openStationOnInitialize)
            {
                station.TryOpen();
            }

            return station;
        }

        public void Configure(
            int maxQueueLength,
            string stationId,
            bool openStationOnInitialize)
        {
            _maxQueueLength = maxQueueLength;
            _stationId = stationId;
            _openStationOnInitialize =
                openStationOnInitialize;
        }
    }
}
