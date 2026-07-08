using System;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Application.DayCycle;
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
        private ISimulationClock _clock;
        private IPauseService _pauseService;

        public bool IsInitialized => _day != null;

        public StoreDay Day =>
            _day ??
            throw new InvalidOperationException(
                "Store day is not initialized.");

        public ISimulationClock Clock =>
            _clock ??
            throw new InvalidOperationException(
                "Simulation clock is not initialized.");

        public IPauseService PauseService =>
            _pauseService ??
            throw new InvalidOperationException(
                "Pause service is not initialized.");

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
                Tick(Time.unscaledDeltaTime);
            }
        }

        public void Configure(
            StoreDaySettingsAsset settings,
            CustomerTechnicalSpawner customerSpawner,
            bool autoRun)
        {
            Configure(
                settings,
                customerSpawner,
                autoRun,
                new SimulationClock(),
                new PauseService());
        }

        public void Configure(
            StoreDaySettingsAsset settings,
            CustomerTechnicalSpawner customerSpawner,
            bool autoRun,
            ISimulationClock clock,
            IPauseService pauseService)
        {
            _settings = settings;
            _customerSpawner = customerSpawner;
            _autoRun = autoRun;
            _clock = clock ??
                throw new ArgumentNullException(
                    nameof(clock));
            _pauseService = pauseService ??
                throw new ArgumentNullException(
                    nameof(pauseService));
            _day = null;
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

            _clock ??= new SimulationClock();
            _pauseService ??= new PauseService();
            _day = _settings.BuildDay();

            _clock.Synchronize(
                _day.Policy.OpenDurationSeconds,
                _day.ElapsedOpenSeconds,
                SimulationSpeedPolicy.Normal);

            if (_settings.AutoOpenOnInitialize)
            {
                _day.TryOpen();
            }

            ApplySpawnGate();
        }

        public void Tick(float unscaledDeltaTime)
        {
            if (unscaledDeltaTime < 0f ||
                float.IsNaN(unscaledDeltaTime) ||
                float.IsInfinity(unscaledDeltaTime))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(unscaledDeltaTime));
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

            SimulationClockTickResult tick =
                _clock.Tick(
                    unscaledDeltaTime,
                    _pauseService.IsPaused);

            int elapsedDelta =
                tick.CurrentElapsedSeconds -
                _day.ElapsedOpenSeconds;

            if (elapsedDelta > 0)
            {
                _day.Advance(elapsedDelta);
            }

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
