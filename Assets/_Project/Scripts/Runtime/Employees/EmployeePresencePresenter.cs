using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using VRMGames.CartridgeAndCloud.Application.Employees;
using VRMGames.CartridgeAndCloud.Domain.Characters;
using VRMGames.CartridgeAndCloud.Domain.Employees;
using VRMGames.CartridgeAndCloud.Infrastructure.Store;
using VRMGames.CartridgeAndCloud.Runtime.Characters;

namespace VRMGames.CartridgeAndCloud.Runtime.Employees
{
    [DisallowMultipleComponent]
    internal sealed class EmployeePresencePresenter : MonoBehaviour
    {
        private readonly Dictionary<EmployeeId, GameObject> _instances =
            new Dictionary<EmployeeId, GameObject>();

        private EmployeeHiringService _hiring;
        private EmployeePresenceService _presence;
        private EmployeeScheduleService _schedules;
        private StorePresentationCatalogAsset _presentationCatalog;
        private Transform _presenceRoot;
        private Transform _anchor;
        private int _lastDay = -1;
        private int _lastVirtualMinute = -1;

        public void Configure(
            EmployeeHiringService hiring,
            EmployeePresenceService presence,
            EmployeeScheduleService schedules,
            StorePresentationCatalogAsset presentationCatalog,
            Transform anchor)
        {
            _hiring = hiring ??
                throw new ArgumentNullException(nameof(hiring));
            _presence = presence ??
                throw new ArgumentNullException(nameof(presence));
            _schedules = schedules ??
                throw new ArgumentNullException(nameof(schedules));
            _presentationCatalog = presentationCatalog ??
                throw new ArgumentNullException(nameof(presentationCatalog));
            _anchor = anchor ??
                throw new ArgumentNullException(nameof(anchor));

            if (_presentationCatalog.FindActorPrefab(
                    "employee-female-01",
                    CharacterRole.Employee) == null ||
                _presentationCatalog.FindActorPrefab(
                    "employee-male-01",
                    CharacterRole.Employee) == null)
            {
                throw new InvalidOperationException(
                    "Employee presence requires both representative employee prefabs.");
            }

            GameObject root = new GameObject("S20_P04_EmployeePresence");
            root.transform.SetParent(transform, false);
            _presenceRoot = root.transform;

            _hiring.StateChanged += HandleHiringStateChanged;
            _schedules.StateChanged += HandleScheduleStateChanged;
            Synchronize();
        }

        private void OnDestroy()
        {
            if (_hiring != null)
            {
                _hiring.StateChanged -= HandleHiringStateChanged;
            }

            if (_schedules != null)
            {
                _schedules.StateChanged -= HandleScheduleStateChanged;
            }
        }


        private void Update()
        {
            if (_schedules == null)
            {
                return;
            }

            int day = _schedules.CurrentDay;
            int minute = _schedules.CurrentVirtualMinute;
            if (day == _lastDay && minute == _lastVirtualMinute)
            {
                return;
            }

            _lastDay = day;
            _lastVirtualMinute = minute;
            Synchronize();
        }

        private void HandleHiringStateChanged()
        {
            Synchronize();
        }

        private void HandleScheduleStateChanged()
        {
            Synchronize();
        }

        private void Synchronize()
        {
            if (_hiring == null ||
                _presence == null ||
                _presenceRoot == null)
            {
                return;
            }

            HashSet<EmployeeId> expected = new HashSet<EmployeeId>();
            int presenceIndex = 0;

            foreach (HiredEmployee employee in _hiring.Employees)
            {
                EmployeePresenceStatus status =
                    _presence.GetStatus(employee);
                if (status != EmployeePresenceStatus.Present &&
                    status != EmployeePresenceStatus.OnBreak &&
                    status != EmployeePresenceStatus.Closing)
                {
                    continue;
                }

                expected.Add(employee.EmployeeId);
                EnsureInstance(employee, presenceIndex);
                presenceIndex++;
            }

            List<EmployeeId> stale = new List<EmployeeId>();
            foreach (KeyValuePair<EmployeeId, GameObject> pair in _instances)
            {
                if (!expected.Contains(pair.Key))
                {
                    stale.Add(pair.Key);
                }
            }

            foreach (EmployeeId employeeId in stale)
            {
                if (_instances.TryGetValue(
                        employeeId,
                        out GameObject instance) &&
                    instance != null)
                {
                    Destroy(instance);
                }

                _instances.Remove(employeeId);
            }
        }

        private void EnsureInstance(
            HiredEmployee employee,
            int presenceIndex)
        {
            Vector3 targetPosition = ResolvePresencePosition(presenceIndex);

            if (_instances.TryGetValue(
                    employee.EmployeeId,
                    out GameObject existing) &&
                existing != null)
            {
                return;
            }

            string stableId = employee.EmployeeId.ToStableId().Value;
            string actorId = SelectActorId(stableId);
            string displayObjectName =
                "Employee_" + (presenceIndex + 1).ToString("00") +
                "_" + stableId.Substring(0, 8);

            GameObject instance = CharacterPrefabFactory.Instantiate(
                _presentationCatalog,
                actorId,
                _presenceRoot,
                displayObjectName,
                stableId,
                CharacterRole.Employee,
                targetPosition);

            PlaceIdle(instance, targetPosition);
            _instances.Add(employee.EmployeeId, instance);
        }

        private Vector3 ResolvePresencePosition(int index)
        {
            const int columns = 3;
            const float horizontalSpacing = 0.9f;
            const float depthSpacing = 0.85f;

            int column = index % columns;
            int row = index / columns;
            float horizontal = (column - 1) * horizontalSpacing;
            float depth = row * depthSpacing;

            return _anchor.position +
                   _anchor.right * horizontal -
                   _anchor.forward * depth;
        }

        private void PlaceIdle(
            GameObject instance,
            Vector3 targetPosition)
        {
            NavMeshAgent agent = instance.GetComponent<NavMeshAgent>();
            if (agent != null &&
                NavMesh.SamplePosition(
                    targetPosition,
                    out NavMeshHit hit,
                    2f,
                    agent.areaMask))
            {
                agent.Warp(hit.position);
            }
            else
            {
                instance.transform.position = targetPosition;
            }

            if (agent != null)
            {
                agent.updateRotation = false;
                if (agent.isOnNavMesh)
                {
                    agent.ResetPath();
                    agent.isStopped = true;
                }
            }

            Vector3 lookTarget = _anchor.position + _anchor.forward * 2f;
            if (StaffOrientationUtility.TryResolveHorizontalRotation(
                    instance.transform.position,
                    lookTarget,
                    out Quaternion rotation))
            {
                instance.transform.rotation = rotation;
            }
        }

        private static string SelectActorId(string stableId)
        {
            if (string.IsNullOrWhiteSpace(stableId))
            {
                throw new ArgumentException(
                    "Employee stable ID is required.",
                    nameof(stableId));
            }

            char last = stableId[stableId.Length - 1];
            int nibble = last >= '0' && last <= '9'
                ? last - '0'
                : 10 + char.ToLowerInvariant(last) - 'a';

            return nibble % 2 == 0
                ? "employee-female-01"
                : "employee-male-01";
        }
    }
}
