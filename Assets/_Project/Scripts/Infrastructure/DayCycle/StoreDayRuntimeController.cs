using System;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.DayCycle;
using VRMGames.CartridgeAndCloud.Infrastructure.Customers;

namespace VRMGames.CartridgeAndCloud.Infrastructure.DayCycle
{
    public sealed class StoreDayRuntimeController :
        MonoBehaviour
    {
        [SerializeField]
        private StoreDaySettingsAsset _settings;

        [SerializeField]
        private CustomerTechnicalSpawner _customerSpawner;

        [SerializeField]
        private bool _autoRun = true;

        private StoreDay _day;
        private float _secondAccumulator;

        public bool IsInitialized => _day != null;

        public StoreDay Day =>
            _day ??
            throw new InvalidOperationException(
                "Store day is not initialized.");

        private void Awake()
        {
            if (_settings != null)
            {
                Initialize();
            }
        }

        private void Update()
        {
            if (_autoRun)
            {
                Tick(Time.deltaTime);
            }
        }

        public void Configure(
            StoreDaySettingsAsset settings,
            CustomerTechnicalSpawner customerSpawner,
            bool autoRun)
        {
            _settings = settings;
            _customerSpawner = customerSpawner;
            _autoRun = autoRun;
            _day = null;
            _secondAccumulator = 0f;
        }

        public void Initialize()
        {
            if (IsInitialized)
            {
                return;
            }

            if (_settings == null)
            {
                throw new InvalidOperationException(
                    "Store day settings asset is required.");
            }

            _day = _settings.BuildDay();

            if (_settings.AutoOpenOnInitialize)
            {
                _day.TryOpen();
            }

            ApplySpawnGate();
        }

        public void Tick(float deltaTime)
        {
            if (deltaTime < 0f ||
                float.IsNaN(deltaTime) ||
                float.IsInfinity(deltaTime))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(deltaTime));
            }

            if (!IsInitialized)
            {
                Initialize();
            }

            if (_day.State != StoreDayState.Open)
            {
                ApplySpawnGate();
                return;
            }

            _secondAccumulator += deltaTime;
            int wholeSeconds =
                Mathf.FloorToInt(_secondAccumulator);

            if (wholeSeconds <= 0)
            {
                return;
            }

            _secondAccumulator -= wholeSeconds;
            _day.Advance(wholeSeconds);
            ApplySpawnGate();
        }

        public bool TryOpen()
        {
            if (!IsInitialized)
            {
                Initialize();
            }

            bool succeeded = _day.TryOpen().Succeeded;
            ApplySpawnGate();
            return succeeded;
        }

        public bool TryBeginClosing()
        {
            if (!IsInitialized)
            {
                Initialize();
            }

            bool succeeded =
                _day.TryBeginClosing().Succeeded;
            ApplySpawnGate();
            return succeeded;
        }

        private void ApplySpawnGate()
        {
            if (_customerSpawner != null)
            {
                _customerSpawner.SetSpawningEnabled(
                    _day != null &&
                    _day.CanAcceptCustomers);
            }
        }
    }
}
