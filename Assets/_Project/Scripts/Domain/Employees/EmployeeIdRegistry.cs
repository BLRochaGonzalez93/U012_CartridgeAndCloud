using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VRMGames.CartridgeAndCloud.Domain.Employees
{
    public sealed class EmployeeIdRegistry
    {
        private readonly HashSet<EmployeeId> _ids;
        private readonly List<EmployeeId> _orderedIds;
        private ReadOnlyCollection<EmployeeId> _readOnlyIds;

        public IReadOnlyList<EmployeeId> Ids =>
            _readOnlyIds ??
            (_readOnlyIds =
                new ReadOnlyCollection<EmployeeId>(_orderedIds));

        public int Count => _orderedIds.Count;

        public EmployeeIdRegistry()
            : this(Array.Empty<EmployeeId>())
        {
        }

        public EmployeeIdRegistry(
            IEnumerable<EmployeeId> existingIds)
        {
            if (existingIds == null)
            {
                throw new ArgumentNullException(nameof(existingIds));
            }

            _ids = new HashSet<EmployeeId>();
            _orderedIds = new List<EmployeeId>();

            foreach (EmployeeId employeeId in existingIds)
            {
                ValidateInitialized(employeeId, nameof(existingIds));

                if (!_ids.Add(employeeId))
                {
                    throw new ArgumentException(
                        $"Employee ID '{employeeId}' is duplicated.",
                        nameof(existingIds));
                }

                _orderedIds.Add(employeeId);
            }

            SortIds();
        }

        public EmployeeId Allocate()
        {
            EmployeeId employeeId;

            do
            {
                employeeId = EmployeeId.New();
            }
            while (_ids.Contains(employeeId));

            _ids.Add(employeeId);
            _orderedIds.Add(employeeId);
            SortIds();
            return employeeId;
        }

        public bool TryRegister(EmployeeId employeeId)
        {
            if (!employeeId.IsInitialized ||
                !_ids.Add(employeeId))
            {
                return false;
            }

            _orderedIds.Add(employeeId);
            SortIds();
            return true;
        }

        public bool Contains(EmployeeId employeeId)
        {
            return employeeId.IsInitialized &&
                _ids.Contains(employeeId);
        }

        public bool TryResolve(
            EmployeeId employeeId,
            out EmployeeId resolvedId)
        {
            if (Contains(employeeId))
            {
                resolvedId = employeeId;
                return true;
            }

            resolvedId = default;
            return false;
        }

        public bool TryResolve(
            string serializedValue,
            out EmployeeId resolvedId)
        {
            if (!EmployeeId.TryParse(
                    serializedValue,
                    out EmployeeId parsedId))
            {
                resolvedId = default;
                return false;
            }

            return TryResolve(parsedId, out resolvedId);
        }

        private static void ValidateInitialized(
            EmployeeId employeeId,
            string parameterName)
        {
            if (!employeeId.IsInitialized)
            {
                throw new ArgumentException(
                    "Employee IDs must be initialized.",
                    parameterName);
            }
        }

        private void SortIds()
        {
            _orderedIds.Sort(
                (left, right) =>
                    StringComparer.Ordinal.Compare(
                        left.Value,
                        right.Value));
            _readOnlyIds = null;
        }
    }
}
