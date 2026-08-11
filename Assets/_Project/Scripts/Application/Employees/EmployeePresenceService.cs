using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VRMGames.CartridgeAndCloud.Domain.Employees;

namespace VRMGames.CartridgeAndCloud.Application.Employees
{
    public enum EmployeePresenceStatus
    {
        AwaitingStart = 0,
        OffDuty = 1,
        Present = 2,
        OnBreak = 3,
        Closing = 4
    }

    public sealed class EmployeePresenceEntry
    {
        public HiredEmployee Employee { get; }
        public EmployeePresenceStatus Status { get; }

        public bool IsPhysicallyPresent =>
            Status == EmployeePresenceStatus.Present ||
            Status == EmployeePresenceStatus.OnBreak ||
            Status == EmployeePresenceStatus.Closing;

        public EmployeePresenceEntry(
            HiredEmployee employee,
            EmployeePresenceStatus status)
        {
            Employee = employee ??
                throw new ArgumentNullException(nameof(employee));
            Status = status;
        }
    }

    /// <summary>
    /// Presentation-facing projection of the authoritative employee state.
    /// It deliberately owns no lifecycle or schedule rules of its own.
    /// </summary>
    public sealed class EmployeePresenceService
    {
        private readonly IEmployeeRosterSource _roster;
        private readonly IEmployeeStateSource _states;

        public EmployeePresenceService(
            IEmployeeRosterSource roster,
            IEmployeeStateSource states)
        {
            _roster = roster ??
                throw new ArgumentNullException(nameof(roster));
            _states = states ??
                throw new ArgumentNullException(nameof(states));
        }

        public EmployeePresenceStatus GetStatus(HiredEmployee employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            return ToPresenceStatus(_states.GetState(employee).State);
        }

        public bool IsPresent(EmployeeId employeeId)
        {
            return _states.TryGetState(
                       employeeId,
                       out EmployeeStateSnapshot state) &&
                   state.IsPhysicallyPresent;
        }

        public IReadOnlyList<EmployeePresenceEntry> GetSnapshot()
        {
            List<EmployeePresenceEntry> entries =
                new List<EmployeePresenceEntry>(_roster.Employees.Count);

            foreach (HiredEmployee employee in _roster.Employees)
            {
                entries.Add(
                    new EmployeePresenceEntry(
                        employee,
                        GetStatus(employee)));
            }

            return new ReadOnlyCollection<EmployeePresenceEntry>(entries);
        }

        public int CountPresent()
        {
            int count = 0;
            foreach (HiredEmployee employee in _roster.Employees)
            {
                if (_states.GetState(employee).IsPhysicallyPresent)
                {
                    count++;
                }
            }

            return count;
        }

        private static EmployeePresenceStatus ToPresenceStatus(
            EmployeeOperationalState state)
        {
            switch (state)
            {
                case EmployeeOperationalState.Available:
                    return EmployeePresenceStatus.Present;
                case EmployeeOperationalState.OnBreak:
                    return EmployeePresenceStatus.OnBreak;
                case EmployeeOperationalState.Closing:
                    return EmployeePresenceStatus.Closing;
                case EmployeeOperationalState.OffDuty:
                    return EmployeePresenceStatus.OffDuty;
                default:
                    return EmployeePresenceStatus.AwaitingStart;
            }
        }
    }
}
