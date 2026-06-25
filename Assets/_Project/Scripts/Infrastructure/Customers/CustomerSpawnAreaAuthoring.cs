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
