using System;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Application.Customers;
using VRMGames.CartridgeAndCloud.Domain.Customers;

namespace VRMGames.CartridgeAndCloud.Infrastructure.Customers
{
    public sealed class CustomerTechnicalSpawner : MonoBehaviour
    {
        [SerializeField]
        private CustomerProfileCatalogAsset _profileCatalog;

        [SerializeField]
        private CustomerSpawnSettingsAsset _spawnSettings;

        [SerializeField]
        private CustomerSpawnAreaAuthoring _spawnArea;

        [SerializeField]
        private bool _autoRun = true;

        [SerializeField]
        [Min(0)]
        private int _browseDwellSeconds = 2;

        private CustomerProfileRegistry _profiles;
        private CustomerInstanceRegistry _instances;
        private CustomerSpawnQueue _queue;
        private CustomerSpawnPolicy _policy;
        private CustomerArrivalClock _clock;
        private CustomerSpawnService _spawnService;
        private CustomerProfileSelector _selector;
        private float _secondAccumulator;
        private int _sequence;

        public int ActiveCustomerCount =>
            _instances == null ? 0 : _instances.ActiveCount;

        public bool IsInitialized => _spawnService != null;

        public void Initialize()
        {
            if (IsInitialized)
            {
                return;
            }

            if (_profileCatalog == null || _spawnSettings == null ||
                _spawnArea == null)
            {
                throw new InvalidOperationException(
                    "Profile catalog, spawn settings and spawn area are required.");
            }

            _profiles = _profileCatalog.BuildRegistry();
            _instances = new CustomerInstanceRegistry();
            _queue = new CustomerSpawnQueue();
            _policy = _spawnSettings.BuildPolicy();
            _clock = new CustomerArrivalClock(
                _policy.ArrivalIntervalSeconds);
            _spawnService = new CustomerSpawnService(
                _profiles,
                _instances);
            _selector = new CustomerProfileSelector(_profiles);
        }

        public void Tick(float deltaTime)
        {
            if (deltaTime < 0f || float.IsNaN(deltaTime) ||
                float.IsInfinity(deltaTime))
            {
                throw new ArgumentOutOfRangeException(nameof(deltaTime));
            }

            if (!IsInitialized)
            {
                Initialize();
            }

            _secondAccumulator += deltaTime;
            int wholeSeconds = Mathf.FloorToInt(_secondAccumulator);
            if (wholeSeconds <= 0)
            {
                return;
            }

            _secondAccumulator -= wholeSeconds;
            int due = _clock.Advance(wholeSeconds);
            for (int i = 0; i < due; i++)
            {
                QueueNextRequest();
                SpawnQueuedCustomer();
            }
        }

        public void Configure(
            CustomerProfileCatalogAsset profileCatalog,
            CustomerSpawnSettingsAsset spawnSettings,
            CustomerSpawnAreaAuthoring spawnArea,
            bool autoRun,
            int browseDwellSeconds)
        {
            _profileCatalog = profileCatalog;
            _spawnSettings = spawnSettings;
            _spawnArea = spawnArea;
            _autoRun = autoRun;
            _browseDwellSeconds = browseDwellSeconds;
            ClearRuntimeState();
        }

        private void Update()
        {
            if (_autoRun)
            {
                Tick(Time.deltaTime);
            }
        }

        private void QueueNextRequest()
        {
            int roll = _sequence % _profiles.TotalSpawnWeight;
            CustomerProfile profile = _selector.SelectByRoll(roll);
            CustomerNavigationPlan plan =
                _spawnArea.BuildNavigationPlan(
                    profile.BrowseStopCount,
                    _browseDwellSeconds);

            string suffix = _sequence.ToString("D4");
            CustomerSpawnRequest request = new CustomerSpawnRequest(
                new CustomerSpawnRequestId("spawn-request-" + suffix),
                new CustomerInstanceId("customer-" + suffix),
                profile.Id,
                plan);
            _sequence++;
            _queue.TryEnqueue(request);
        }

        private void SpawnQueuedCustomer()
        {
            CustomerSpawnResult result =
                _spawnService.TrySpawnNext(_queue, _policy);
            if (!result.Succeeded)
            {
                return;
            }

            CustomerProfile profile =
                _profiles.Get(result.Customer.ProfileId);
            CustomerProfileAsset authoringProfile =
                FindProfileAsset(profile.Id);
            GameObject instance = CreateTechnicalView(authoringProfile);
            instance.transform.position = _spawnArea.SpawnPoint.position;

            CustomerTechnicalAgentView view =
                instance.GetComponent<CustomerTechnicalAgentView>();
            if (view == null)
            {
                view = instance.AddComponent<CustomerTechnicalAgentView>();
            }

            view.Configure(
                result.Customer,
                _spawnArea.BuildWaypointPositions(
                    profile.BrowseStopCount),
                profile.WalkSpeed,
                0.05f,
                _spawnSettings.DestroyTechnicalViewOnExit);
        }

        private CustomerProfileAsset FindProfileAsset(
            CustomerProfileId id)
        {
            foreach (CustomerProfileAsset profile in _profileCatalog.Profiles)
            {
                if (profile != null &&
                    string.Equals(
                        profile.CustomerProfileId,
                        id.Value,
                        StringComparison.Ordinal))
                {
                    return profile;
                }
            }

            throw new InvalidOperationException(
                $"Profile asset {id} was not found.");
        }

        private static GameObject CreateTechnicalView(
            CustomerProfileAsset profile)
        {
            if (profile.TechnicalPrefab != null)
            {
                return Instantiate(profile.TechnicalPrefab);
            }

            GameObject fallback =
                GameObject.CreatePrimitive(PrimitiveType.Capsule);
            fallback.name =
                "TechnicalCustomer_" + profile.CustomerProfileId;
            return fallback;
        }

        private void ClearRuntimeState()
        {
            _profiles = null;
            _instances = null;
            _queue = null;
            _policy = null;
            _clock = null;
            _spawnService = null;
            _selector = null;
            _secondAccumulator = 0f;
            _sequence = 0;
        }
    }
}
