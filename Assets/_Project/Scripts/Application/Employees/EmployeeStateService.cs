using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VRMGames.CartridgeAndCloud.Application.GameSession;
using VRMGames.CartridgeAndCloud.Domain.DayCycle;
using VRMGames.CartridgeAndCloud.Domain.Employees;

namespace VRMGames.CartridgeAndCloud.Application.Employees
{
    public interface IEmployeeStateSource
    {
        EmployeeStateSnapshot GetState(HiredEmployee employee);

        bool TryGetState(
            EmployeeId employeeId,
            out EmployeeStateSnapshot state);
    }

    /// <summary>
    /// Single application authority for the current operational state of an
    /// employee. It projects contractual start, work calendar and store-day
    /// state without owning tasks, fatigue, satisfaction or persistence.
    /// </summary>
    public sealed class EmployeeStateService : IEmployeeStateSource
    {
        private readonly IEmployeeRosterSource _roster;
        private readonly IActiveGameSession _activeSession;
        private readonly EmployeeScheduleService _schedules;

        public EmployeeStateService(
            IEmployeeRosterSource roster,
            IActiveGameSession activeSession,
            EmployeeScheduleService schedules)
        {
            _roster = roster ??
                throw new ArgumentNullException(nameof(roster));
            _activeSession = activeSession ??
                throw new ArgumentNullException(nameof(activeSession));
            _schedules = schedules ??
                throw new ArgumentNullException(nameof(schedules));
        }

        public EmployeeStateSnapshot GetState(HiredEmployee employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            if (!_activeSession.HasActiveSession)
            {
                return Create(
                    employee,
                    EmployeeOperationalState.AwaitingStart,
                    EmployeeWorkAvailability.DayOff,
                    0,
                    0);
            }

            int day = _activeSession.Snapshot.CurrentDay;
            int minute = _schedules.CurrentVirtualMinute;

            if (day < employee.StartDay)
            {
                return Create(
                    employee,
                    EmployeeOperationalState.AwaitingStart,
                    EmployeeWorkAvailability.DayOff,
                    day,
                    minute);
            }

            EmployeeWorkAvailability availability =
                _schedules.EvaluateCurrent(employee);
            StoreDayState dayState = ResolveStoreDayState();

            if (dayState == StoreDayState.Closed)
            {
                return Create(
                    employee,
                    EmployeeOperationalState.OffDuty,
                    availability,
                    day,
                    minute);
            }

            if (dayState == StoreDayState.Closing &&
                (availability == EmployeeWorkAvailability.Working ||
                 availability == EmployeeWorkAvailability.OnBreak))
            {
                return Create(
                    employee,
                    EmployeeOperationalState.Closing,
                    availability,
                    day,
                    minute);
            }

            switch (availability)
            {
                case EmployeeWorkAvailability.Working:
                    return Create(
                        employee,
                        EmployeeOperationalState.Available,
                        availability,
                        day,
                        minute);
                case EmployeeWorkAvailability.OnBreak:
                    return Create(
                        employee,
                        EmployeeOperationalState.OnBreak,
                        availability,
                        day,
                        minute);
                default:
                    return Create(
                        employee,
                        EmployeeOperationalState.OffDuty,
                        availability,
                        day,
                        minute);
            }
        }

        public bool TryGetState(
            EmployeeId employeeId,
            out EmployeeStateSnapshot state)
        {
            if (!employeeId.IsInitialized)
            {
                state = default;
                return false;
            }

            foreach (HiredEmployee employee in _roster.Employees)
            {
                if (employee.EmployeeId != employeeId)
                {
                    continue;
                }

                state = GetState(employee);
                return true;
            }

            state = default;
            return false;
        }

        public IReadOnlyList<EmployeeStateSnapshot> GetSnapshot()
        {
            List<EmployeeStateSnapshot> states =
                new List<EmployeeStateSnapshot>(_roster.Employees.Count);

            foreach (HiredEmployee employee in _roster.Employees)
            {
                states.Add(GetState(employee));
            }

            return new ReadOnlyCollection<EmployeeStateSnapshot>(states);
        }

        public int CountAvailable()
        {
            int count = 0;
            foreach (HiredEmployee employee in _roster.Employees)
            {
                if (GetState(employee).CanAcceptTasks)
                {
                    count++;
                }
            }

            return count;
        }

        private StoreDayState ResolveStoreDayState()
        {
            string raw = _activeSession.Snapshot.DayCycle.State;
            if (!Enum.TryParse(raw, false, out StoreDayState state) ||
                !Enum.IsDefined(typeof(StoreDayState), state))
            {
                throw new InvalidOperationException(
                    "Unsupported store day state '" + raw + "'.");
            }

            return state;
        }

        private static EmployeeStateSnapshot Create(
            HiredEmployee employee,
            EmployeeOperationalState state,
            EmployeeWorkAvailability availability,
            int day,
            int minute)
        {
            return new EmployeeStateSnapshot(
                employee.EmployeeId,
                state,
                availability,
                day,
                minute);
        }
    }
}
