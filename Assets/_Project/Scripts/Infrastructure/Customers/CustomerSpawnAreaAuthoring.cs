using System;
using System.Collections.Generic;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.Customers;

namespace VRMGames.CartridgeAndCloud.Infrastructure.Customers
{
    public sealed class CustomerSpawnAreaAuthoring : MonoBehaviour
    {
        [SerializeField]
        private Transform _spawnPoint;

        [SerializeField]
        private Transform _entryPoint;

        [SerializeField]
        private List<Transform> _browsePoints =
            new List<Transform>();

        [SerializeField]
        private Transform _exitPoint;

        public Transform SpawnPoint => _spawnPoint;
        public Transform EntryPoint => _entryPoint;
        public IReadOnlyList<Transform> BrowsePoints => _browsePoints;
        public Transform ExitPoint => _exitPoint;


        public Vector3 GetRandomSpawnPosition()
        {
            if (_spawnPoint == null)
            {
                throw new InvalidOperationException(
                    "A spawn point is required.");
            }

            Vector3 center = _spawnPoint.position;
            Vector3 right = _spawnPoint.right;
            Vector3 forward = _spawnPoint.forward;
            float halfWidth = 5f;
            float halfDepth = 2f;

            return center +
                right * UnityEngine.Random.Range(-halfWidth, halfWidth) +
                forward * UnityEngine.Random.Range(-halfDepth, halfDepth);
        }

        public CustomerNavigationPlan BuildNavigationPlan(
            int requestedBrowseStops,
            int dwellSecondsPerBrowsePoint)
        {
            if (_entryPoint == null || _exitPoint == null)
            {
                throw new InvalidOperationException(
                    "Entry and exit points are required.");
            }

            if (requestedBrowseStops <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(requestedBrowseStops));
            }

            if (dwellSecondsPerBrowsePoint < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(dwellSecondsPerBrowsePoint));
            }

            List<CustomerNavigationTarget> targets =
                new List<CustomerNavigationTarget>();
            targets.Add(new CustomerNavigationTarget(
                new CustomerNavigationPointId(_entryPoint.name),
                CustomerNavigationTargetType.Entry,
                0));

            int count = Math.Min(
                requestedBrowseStops,
                _browsePoints.Count);
            for (int i = 0; i < count; i++)
            {
                Transform point = _browsePoints[i];
                if (point == null)
                {
                    throw new InvalidOperationException(
                        "Browse points cannot contain missing references.");
                }

                targets.Add(new CustomerNavigationTarget(
                    new CustomerNavigationPointId(point.name),
                    CustomerNavigationTargetType.Browse,
                    dwellSecondsPerBrowsePoint));
            }

            targets.Add(new CustomerNavigationTarget(
                new CustomerNavigationPointId(_exitPoint.name),
                CustomerNavigationTargetType.Exit,
                0));

            return new CustomerNavigationPlan(targets);
        }

        public Vector3[] BuildWaypointPositions(int requestedBrowseStops)
        {
            if (_spawnPoint == null || _entryPoint == null ||
                _exitPoint == null)
            {
                throw new InvalidOperationException(
                    "Spawn, entry and exit points are required.");
            }

            int count = Math.Min(
                requestedBrowseStops,
                _browsePoints.Count);
            Vector3[] positions = new Vector3[count + 2];
            positions[0] = _entryPoint.position;
            for (int i = 0; i < count; i++)
            {
                if (_browsePoints[i] == null)
                {
                    throw new InvalidOperationException(
                        "Browse points cannot contain missing references.");
                }

                positions[i + 1] = _browsePoints[i].position;
            }

            positions[positions.Length - 1] = _exitPoint.position;
            return positions;
        }


        public CustomerNavigationPlan BuildNavigationPlan(
            IReadOnlyList<Transform> browseTargets,
            int dwellSecondsPerBrowsePoint)
        {
            if (_entryPoint == null || _exitPoint == null)
            {
                throw new InvalidOperationException(
                    "Entry and exit points are required.");
            }

            if (browseTargets == null)
            {
                throw new ArgumentNullException(
                    nameof(browseTargets));
            }

            if (dwellSecondsPerBrowsePoint < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(dwellSecondsPerBrowsePoint));
            }

            List<CustomerNavigationTarget> targets =
                new List<CustomerNavigationTarget>
                {
                    new CustomerNavigationTarget(
                        new CustomerNavigationPointId(
                            _entryPoint.name),
                        CustomerNavigationTargetType.Entry,
                        0)
                };

            foreach (Transform point in browseTargets)
            {
                if (point == null)
                {
                    continue;
                }

                targets.Add(
                    new CustomerNavigationTarget(
                        new CustomerNavigationPointId(
                            point.name),
                        CustomerNavigationTargetType.Browse,
                        dwellSecondsPerBrowsePoint));
            }

            targets.Add(
                new CustomerNavigationTarget(
                    new CustomerNavigationPointId(
                        _exitPoint.name),
                    CustomerNavigationTargetType.Exit,
                    0));

            return new CustomerNavigationPlan(targets);
        }

        public Vector3[] BuildWaypointPositions(
            IReadOnlyList<Transform> browseTargets)
        {
            if (_spawnPoint == null || _entryPoint == null ||
                _exitPoint == null)
            {
                throw new InvalidOperationException(
                    "Spawn, entry and exit points are required.");
            }

            if (browseTargets == null)
            {
                throw new ArgumentNullException(
                    nameof(browseTargets));
            }

            Vector3[] positions =
                new Vector3[browseTargets.Count + 2];
            positions[0] = _entryPoint.position;

            for (int index = 0;
                 index < browseTargets.Count;
                 index++)
            {
                Transform point = browseTargets[index];
                if (point == null)
                {
                    throw new InvalidOperationException(
                        "Browse targets cannot contain missing references.");
                }

                positions[index + 1] = point.position;
            }

            positions[positions.Length - 1] =
                _exitPoint.position;
            return positions;
        }

        public List<Transform> GetFallbackBrowsePoints(
            int requestedBrowseStops)
        {
            int count = Math.Min(
                requestedBrowseStops,
                _browsePoints.Count);
            List<Transform> result =
                new List<Transform>(count);

            for (int index = 0;
                 index < count;
                 index++)
            {
                Transform point = _browsePoints[index];
                if (point != null)
                {
                    result.Add(point);
                }
            }

            return result;
        }

        public void Configure(
            Transform spawnPoint,
            Transform entryPoint,
            IEnumerable<Transform> browsePoints,
            Transform exitPoint)
        {
            if (browsePoints == null)
            {
                throw new ArgumentNullException(nameof(browsePoints));
            }

            _spawnPoint = spawnPoint;
            _entryPoint = entryPoint;
            _browsePoints = new List<Transform>(browsePoints);
            _exitPoint = exitPoint;
        }
    }
}
