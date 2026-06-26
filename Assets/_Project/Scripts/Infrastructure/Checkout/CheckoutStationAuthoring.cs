using System;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.Checkout;

namespace VRMGames.CartridgeAndCloud.Infrastructure.Checkout
{
    public sealed class CheckoutStationAuthoring :
        MonoBehaviour
    {
        [SerializeField]
        private CheckoutSettingsAsset _settings;

        [SerializeField]
        private bool _initializeOnAwake = true;

        private CheckoutStation _station;

        public bool IsInitialized => _station != null;

        public CheckoutStation Station =>
            _station ??
            throw new InvalidOperationException(
                "Checkout station is not initialized.");

        private void Awake()
        {
            if (_initializeOnAwake)
            {
                Initialize();
            }
        }

        public void Configure(
            CheckoutSettingsAsset settings,
            bool initializeOnAwake)
        {
            _settings = settings;
            _initializeOnAwake = initializeOnAwake;
            _station = null;
        }

        public void Initialize()
        {
            if (_station != null)
            {
                return;
            }

            if (_settings == null)
            {
                throw new InvalidOperationException(
                    "Checkout settings asset is required.");
            }

            _station = _settings.BuildStation();
        }
    }
}
