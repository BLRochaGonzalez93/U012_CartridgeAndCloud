using System;
using System.Collections.Generic;
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

        [SerializeField]
        private bool _spawningEnabled = true;

        private CustomerProfileRegistry _profiles;
        private CustomerInstanceRegistry _instances;
        private CustomerSpawnQueue _queue;
        private CustomerSpawnPolicy _policy;
        private CustomerArrivalClock _clock;
        private CustomerSpawnService _spawnService;
        private CustomerProfileSelector _selector;
        private float _secondAccumulator;
        private int _sequence;
        private readonly Dictionary<string, Vector3[]>
            _pendingWaypointPositions =
                new Dictionary<string, Vector3[]>();

        public int ActiveCustomerCount =>
            _instances == null ? 0 : _instances.ActiveCount;

        public bool IsInitialized => _spawnService != null;

        public bool SpawningEnabled => _spawningEnabled;

        public void Initialize()
        {
            if (IsInitialized)
                return;

            if (_profileCatalog == null ||
                _spawnSettings == null ||
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
            if (deltaTime < 0f ||
                float.IsNaN(deltaTime) ||
                float.IsInfinity(deltaTime))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(deltaTime));
            }

            if (!IsInitialized)
                Initialize();

            if (!_spawningEnabled)
                return;

            _secondAccumulator += deltaTime;
            int wholeSeconds =
                Mathf.FloorToInt(_secondAccumulator);
            if (wholeSeconds <= 0)
                return;

            _secondAccumulator -= wholeSeconds;
            int due = _clock.Advance(wholeSeconds);

            for (int index = 0; index < due; index++)
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
            _spawningEnabled = true;
            ClearRuntimeState();
        }

        public void SetSpawningEnabled(bool enabled)
        {
            _spawningEnabled = enabled;

            if (!enabled)
            {
                _secondAccumulator = 0f;
            }
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
            int roll =
                _sequence % _profiles.TotalSpawnWeight;
            CustomerProfile profile =
                _selector.SelectByRoll(roll);

            List<Transform> browseTargets =
                ResolveBrowseTargets(
                    profile.BrowseStopCount);

            CustomerNavigationPlan plan =
                _spawnArea.BuildNavigationPlan(
                    browseTargets,
                    _browseDwellSeconds);

            string suffix = _sequence.ToString("D4");
            string customerId =
                "customer-" + suffix;
            CustomerSpawnRequest request =
                new CustomerSpawnRequest(
                    new CustomerSpawnRequestId(
                        "spawn-request-" + suffix),
                    new CustomerInstanceId(
                        customerId),
                    profile.Id,
                    plan);

            _pendingWaypointPositions[customerId] =
                _spawnArea.BuildWaypointPositions(
                    browseTargets);

            _sequence++;
            _queue.TryEnqueue(request);
        }

        private List<Transform> ResolveBrowseTargets(
            int requestedBrowseStops)
        {
            List<Transform> available =
                new List<Transform>();

            CustomerBrowseFixtureAuthoring[] fixtures =
                UnityEngine.Object.FindObjectsByType<
                    CustomerBrowseFixtureAuthoring>(
                        FindObjectsInactive.Exclude,
                        FindObjectsSortMode.None);

            foreach (CustomerBrowseFixtureAuthoring fixture
                     in fixtures)
            {
                if (fixture == null ||
                    !fixture.HasStock)
                {
                    continue;
                }

                foreach (Transform point
                         in fixture.Points)
                {
                    if (point != null)
                    {
                        available.Add(point);
                    }
                }
            }

            if (available.Count == 0)
            {
                return _spawnArea.GetFallbackBrowsePoints(
                    requestedBrowseStops);
            }

            available.Sort(
                (left, right) =>
                    string.CompareOrdinal(
                        left.name,
                        right.name));

            int count = Mathf.Min(
                requestedBrowseStops,
                available.Count);
            List<Transform> selected =
                new List<Transform>(count);

            int startIndex =
                available.Count > 0
                    ? _sequence % available.Count
                    : 0;

            for (int index = 0;
                 index < count;
                 index++)
            {
                selected.Add(
                    available[
                        (startIndex + index) %
                        available.Count]);
            }

            return selected;
        }

        private void SpawnQueuedCustomer()
        {
            CustomerSpawnResult result =
                _spawnService.TrySpawnNext(
                    _queue,
                    _policy);

            if (!result.Succeeded)
                return;

            CustomerProfile profile =
                _profiles.Get(
                    result.Customer.ProfileId);
            CustomerProfileAsset authoringProfile =
                FindProfileAsset(profile.Id);
            GameObject instance =
                CreateTechnicalView(authoringProfile);
            instance.transform.position =
                _spawnArea.GetRandomSpawnPosition();

            CustomerTechnicalAgentView view =
                instance.GetComponent<
                    CustomerTechnicalAgentView>();

            if (view == null)
            {
                view = instance.AddComponent<
                    CustomerTechnicalAgentView>();
            }

            Vector3[] waypoints;
            if (!_pendingWaypointPositions.TryGetValue(
                    result.Customer.Id.Value,
                    out waypoints))
            {
                List<Transform> fallbackTargets =
                    _spawnArea.GetFallbackBrowsePoints(
                        Mathf.Max(
                            0,
                            result.Customer.NavigationPlan.Count - 2));
                waypoints =
                    _spawnArea.BuildWaypointPositions(
                        fallbackTargets);
            }

            _pendingWaypointPositions.Remove(
                result.Customer.Id.Value);

            view.Configure(
                result.Customer,
                waypoints,
                profile.WalkSpeed,
                0.05f,
                _spawnSettings
                    .DestroyTechnicalViewOnExit);
        }

        private CustomerProfileAsset FindProfileAsset(
            CustomerProfileId id)
        {
            foreach (CustomerProfileAsset profile
                     in _profileCatalog.Profiles)
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
            if (profile.TechnicalPrefab == null)
            {
                throw new InvalidOperationException(
                    $"Customer profile '{profile.CustomerProfileId}' has no authored technical prefab.");
            }

            return Instantiate(profile.TechnicalPrefab);
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
            _pendingWaypointPositions.Clear();
        }
    }
}
