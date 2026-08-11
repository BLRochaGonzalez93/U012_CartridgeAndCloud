using System;
using System.Collections.Generic;
using VRMGames.CartridgeAndCloud.Application.GameSession;
using VRMGames.CartridgeAndCloud.Domain.DayCycle;
using VRMGames.CartridgeAndCloud.Domain.Employees;
using VRMGames.CartridgeAndCloud.Domain.Persistence;

namespace VRMGames.CartridgeAndCloud.Application.Employees
{
    public interface IEmployeeWorkCalendar
    {
        bool IsScheduledWorkDay(EmployeeId employeeId, int day);
    }

    public enum EmployeeScheduleOperationStatus
    {
        Success = 0,
        InvalidState = 1,
        EmployeeNotFound = 2,
        InvalidDay = 3
    }

    public sealed class EmployeeScheduleOperationResult
    {
        public EmployeeScheduleOperationStatus Status { get; }
        public string Detail { get; }
        public bool Succeeded =>
            Status == EmployeeScheduleOperationStatus.Success;

        private EmployeeScheduleOperationResult(
            EmployeeScheduleOperationStatus status,
            string detail)
        {
            Status = status;
            Detail = detail ?? string.Empty;
        }

        public static EmployeeScheduleOperationResult Success(string detail)
        {
            return new EmployeeScheduleOperationResult(
                EmployeeScheduleOperationStatus.Success,
                detail);
        }

        public static EmployeeScheduleOperationResult Failure(
            EmployeeScheduleOperationStatus status,
            string detail)
        {
            if (status == EmployeeScheduleOperationStatus.Success)
            {
                throw new ArgumentOutOfRangeException(nameof(status));
            }

            return new EmployeeScheduleOperationResult(status, detail);
        }
    }

    public sealed class EmployeeScheduleService : IEmployeeWorkCalendar
    {
        private static readonly EmployeeShiftDefinition[] ShiftPresets =
        {
            new EmployeeShiftDefinition(
                "full-day",
                "Full day",
                8 * 60,
                20 * 60,
                14 * 60,
                30),
            new EmployeeShiftDefinition(
                "early",
                "Early",
                8 * 60,
                16 * 60,
                12 * 60,
                30),
            new EmployeeShiftDefinition(
                "late",
                "Late",
                14 * 60,
                22 * 60,
                18 * 60,
                30)
        };

        private readonly IEmployeeRosterSource _roster;
        private readonly IActiveGameSession _activeSession;
        private readonly Dictionary<EmployeeId, ScheduleState> _states =
            new Dictionary<EmployeeId, ScheduleState>();
        private readonly Dictionary<LockedDayKey, EmployeeDaySchedule>
            _lockedDays =
                new Dictionary<LockedDayKey, EmployeeDaySchedule>();

        private string _boundSessionId = string.Empty;

        public event Action StateChanged;

        public IReadOnlyList<EmployeeShiftDefinition> Presets => ShiftPresets;

        public int CurrentDay
        {
            get
            {
                EnsureSession();
                return _activeSession.HasActiveSession
                    ? _activeSession.Snapshot.CurrentDay
                    : 0;
            }
        }

        public int CurrentVirtualMinute
        {
            get
            {
                EnsureSession();
                if (!_activeSession.HasActiveSession)
                {
                    return 0;
                }

                return StoreTradingHoursPolicy.VirtualMinute(
                    _activeSession.Snapshot.DayCycle.ElapsedDaySeconds,
                    _activeSession.Snapshot.DayCycle.DayDurationSeconds);
            }
        }

        public EmployeeScheduleService(
            IEmployeeRosterSource roster,
            IActiveGameSession activeSession)
        {
            _roster = roster ??
                throw new ArgumentNullException(nameof(roster));
            _activeSession = activeSession ??
                throw new ArgumentNullException(nameof(activeSession));

            EnsureSession();
        }

        public EmployeeShiftDefinition GetConfiguredShift(EmployeeId employeeId)
        {
            return GetOrCreateState(employeeId).Shift;
        }

        public bool IsRecurringWorkDay(EmployeeId employeeId, int cycleDay)
        {
            if (cycleDay < 1 || cycleDay > 7)
            {
                throw new ArgumentOutOfRangeException(nameof(cycleDay));
            }

            return GetOrCreateState(employeeId).WorkingDays[cycleDay - 1];
        }

        public EmployeeScheduleExceptionKind GetException(
            EmployeeId employeeId,
            int day)
        {
            if (day < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(day));
            }

            ScheduleState state = GetOrCreateState(employeeId);
            return state.Exceptions.TryGetValue(
                    day,
                    out EmployeeScheduleExceptionKind kind)
                ? kind
                : EmployeeScheduleExceptionKind.None;
        }

        public EmployeeDaySchedule GetCurrentDaySchedule(
            HiredEmployee employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            EnsureEmployeeExists(employee.EmployeeId);
            EnsureSession();

            if (!_activeSession.HasActiveSession)
            {
                throw new InvalidOperationException(
                    "An active session is required to resolve an employee schedule.");
            }

            return GetOrLockDay(
                employee.EmployeeId,
                _activeSession.Snapshot.CurrentDay);
        }

        public EmployeeWorkAvailability EvaluateCurrent(
            HiredEmployee employee)
        {
            EmployeeDaySchedule schedule = GetCurrentDaySchedule(employee);
            return schedule.EvaluateMinute(CurrentVirtualMinute);
        }

        public bool IsScheduledWorkDay(EmployeeId employeeId, int day)
        {
            EnsureEmployeeExists(employeeId);
            EnsureSession();

            if (day < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(day));
            }

            if (_activeSession.HasActiveSession &&
                day == _activeSession.Snapshot.CurrentDay)
            {
                return GetOrLockDay(employeeId, day).IsWorkingDay;
            }

            return BuildDaySchedule(employeeId, day).IsWorkingDay;
        }

        public EmployeeScheduleOperationResult CycleShiftPreset(
            EmployeeId employeeId)
        {
            ScheduleState state;
            if (!TryGetEmployeeState(employeeId, out state))
            {
                return NotFound();
            }

            int currentIndex = Array.IndexOf(ShiftPresets, state.Shift);
            int nextIndex = currentIndex < 0
                ? 0
                : (currentIndex + 1) % ShiftPresets.Length;

            state.Shift = ShiftPresets[nextIndex];
            StateChanged?.Invoke();

            return EmployeeScheduleOperationResult.Success(
                "Schedule updated to " + state.Shift.DisplayName +
                ". The current game day remains locked; changes apply from the next day.");
        }

        public EmployeeScheduleOperationResult ToggleRecurringWorkDay(
            EmployeeId employeeId,
            int cycleDay)
        {
            if (cycleDay < 1 || cycleDay > 7)
            {
                return EmployeeScheduleOperationResult.Failure(
                    EmployeeScheduleOperationStatus.InvalidDay,
                    "Cycle day must be between 1 and 7.");
            }

            ScheduleState state;
            if (!TryGetEmployeeState(employeeId, out state))
            {
                return NotFound();
            }

            int index = cycleDay - 1;
            state.WorkingDays[index] = !state.WorkingDays[index];
            StateChanged?.Invoke();

            return EmployeeScheduleOperationResult.Success(
                "Cycle day " + cycleDay + " is now " +
                (state.WorkingDays[index] ? "WORK" : "OFF") +
                ". The current game day remains locked.");
        }

        public EmployeeScheduleOperationResult CycleNextDayException(
            EmployeeId employeeId)
        {
            EnsureSession();
            if (!_activeSession.HasActiveSession)
            {
                return EmployeeScheduleOperationResult.Failure(
                    EmployeeScheduleOperationStatus.InvalidState,
                    "An active game session is required.");
            }

            ScheduleState state;
            if (!TryGetEmployeeState(employeeId, out state))
            {
                return NotFound();
            }

            int day = checked(_activeSession.Snapshot.CurrentDay + 1);
            EmployeeScheduleExceptionKind current =
                GetException(employeeId, day);
            EmployeeScheduleExceptionKind next;

            switch (current)
            {
                case EmployeeScheduleExceptionKind.None:
                    next = EmployeeScheduleExceptionKind.DayOff;
                    break;
                case EmployeeScheduleExceptionKind.DayOff:
                    next = EmployeeScheduleExceptionKind.ForceWork;
                    break;
                default:
                    next = EmployeeScheduleExceptionKind.None;
                    break;
            }

            if (next == EmployeeScheduleExceptionKind.None)
            {
                state.Exceptions.Remove(day);
            }
            else
            {
                state.Exceptions[day] = next;
            }

            PrunePastExceptions(state);
            StateChanged?.Invoke();

            return EmployeeScheduleOperationResult.Success(
                "Day " + day + " exception: " + next + ".");
        }

        private EmployeeDaySchedule GetOrLockDay(
            EmployeeId employeeId,
            int day)
        {
            LockedDayKey key = new LockedDayKey(employeeId, day);
            if (_lockedDays.TryGetValue(key, out EmployeeDaySchedule locked))
            {
                return locked;
            }

            EmployeeDaySchedule schedule = BuildDaySchedule(employeeId, day);
            _lockedDays.Add(key, schedule);
            return schedule;
        }

        private EmployeeDaySchedule BuildDaySchedule(
            EmployeeId employeeId,
            int day)
        {
            ScheduleState state = GetOrCreateState(employeeId);
            int cycleDay = CycleDay(day);
            bool working = state.WorkingDays[cycleDay - 1];
            EmployeeScheduleExceptionKind exception =
                state.Exceptions.TryGetValue(
                    day,
                    out EmployeeScheduleExceptionKind kind)
                    ? kind
                    : EmployeeScheduleExceptionKind.None;

            if (exception == EmployeeScheduleExceptionKind.DayOff)
            {
                working = false;
            }
            else if (exception == EmployeeScheduleExceptionKind.ForceWork)
            {
                working = true;
            }

            return new EmployeeDaySchedule(
                day,
                cycleDay,
                working,
                state.Shift,
                exception);
        }

        private ScheduleState GetOrCreateState(EmployeeId employeeId)
        {
            EnsureSession();
            EnsureEmployeeExists(employeeId);

            if (_states.TryGetValue(employeeId, out ScheduleState state))
            {
                return state;
            }

            state = new ScheduleState(ShiftPresets[0]);
            _states.Add(employeeId, state);
            return state;
        }

        private bool TryGetEmployeeState(
            EmployeeId employeeId,
            out ScheduleState state)
        {
            EnsureSession();
            state = null;

            if (!ContainsEmployee(employeeId))
            {
                return false;
            }

            state = GetOrCreateState(employeeId);
            return true;
        }

        private void EnsureEmployeeExists(EmployeeId employeeId)
        {
            if (!employeeId.IsInitialized || !ContainsEmployee(employeeId))
            {
                throw new InvalidOperationException(
                    "Employee is not part of the active roster.");
            }
        }

        private bool ContainsEmployee(EmployeeId employeeId)
        {
            if (!employeeId.IsInitialized)
            {
                return false;
            }

            foreach (HiredEmployee employee in _roster.Employees)
            {
                if (employee.EmployeeId == employeeId)
                {
                    return true;
                }
            }

            return false;
        }

        private EmployeeScheduleOperationResult NotFound()
        {
            return EmployeeScheduleOperationResult.Failure(
                EmployeeScheduleOperationStatus.EmployeeNotFound,
                "Employee was not found in the active roster.");
        }

        private void PrunePastExceptions(ScheduleState state)
        {
            if (!_activeSession.HasActiveSession || state.Exceptions.Count == 0)
            {
                return;
            }

            int currentDay = _activeSession.Snapshot.CurrentDay;
            List<int> stale = null;
            foreach (KeyValuePair<int, EmployeeScheduleExceptionKind> pair
                     in state.Exceptions)
            {
                if (pair.Key < currentDay)
                {
                    stale ??= new List<int>();
                    stale.Add(pair.Key);
                }
            }

            if (stale == null)
            {
                return;
            }

            foreach (int day in stale)
            {
                state.Exceptions.Remove(day);
            }
        }

        public IReadOnlyList<EmployeeScheduleSaveRecord>
            CaptureSchedules()
        {
            EnsureSession();
            List<EmployeeScheduleSaveRecord> records =
                new List<EmployeeScheduleSaveRecord>(_roster.Employees.Count);
            int currentDay = _activeSession.HasActiveSession
                ? _activeSession.Snapshot.CurrentDay
                : 0;

            foreach (HiredEmployee employee in _roster.Employees)
            {
                ScheduleState state = GetOrCreateState(employee.EmployeeId);
                List<EmployeeScheduleExceptionSaveRecord> exceptions =
                    new List<EmployeeScheduleExceptionSaveRecord>(
                        state.Exceptions.Count);

                foreach (KeyValuePair<int, EmployeeScheduleExceptionKind> pair
                         in state.Exceptions)
                {
                    exceptions.Add(
                        new EmployeeScheduleExceptionSaveRecord(
                            pair.Key,
                            pair.Value));
                }

                exceptions.Sort((left, right) => left.Day.CompareTo(right.Day));

                int lockedDay = 0;
                bool lockedWorking = false;
                string lockedShiftId = string.Empty;
                EmployeeScheduleExceptionKind lockedException =
                    EmployeeScheduleExceptionKind.None;

                if (currentDay > 0 &&
                    _lockedDays.TryGetValue(
                        new LockedDayKey(employee.EmployeeId, currentDay),
                        out EmployeeDaySchedule locked))
                {
                    lockedDay = currentDay;
                    lockedWorking = locked.IsWorkingDay;
                    lockedShiftId = locked.Shift.ShiftId;
                    lockedException = locked.ExceptionKind;
                }

                records.Add(
                    new EmployeeScheduleSaveRecord(
                        employee.EmployeeId.Value,
                        state.Shift.ShiftId,
                        state.WorkingDays[0],
                        state.WorkingDays[1],
                        state.WorkingDays[2],
                        state.WorkingDays[3],
                        state.WorkingDays[4],
                        state.WorkingDays[5],
                        state.WorkingDays[6],
                        exceptions,
                        lockedDay,
                        lockedWorking,
                        lockedShiftId,
                        lockedException));
            }

            return records;
        }

        public void RestoreSchedules(
            IReadOnlyList<EmployeeScheduleSaveRecord> records)
        {
            if (records == null)
            {
                throw new ArgumentNullException(nameof(records));
            }

            EnsureSession();
            Dictionary<EmployeeId, ScheduleState> restored =
                new Dictionary<EmployeeId, ScheduleState>();
            Dictionary<LockedDayKey, EmployeeDaySchedule> restoredLocks =
                new Dictionary<LockedDayKey, EmployeeDaySchedule>();
            int currentDay = _activeSession.HasActiveSession
                ? _activeSession.Snapshot.CurrentDay
                : 0;

            foreach (EmployeeScheduleSaveRecord record in records)
            {
                EmployeeId employeeId = EmployeeId.Parse(record.EmployeeId);
                EnsureEmployeeExists(employeeId);
                EmployeeShiftDefinition shift = ResolveShift(record.ShiftId);
                ScheduleState state = new ScheduleState(shift);
                bool[] workingDays = record.CopyWorkingDays();
                for (int index = 0; index < workingDays.Length; index++)
                {
                    state.WorkingDays[index] = workingDays[index];
                }

                foreach (EmployeeScheduleExceptionSaveRecord exception
                         in record.Exceptions)
                {
                    state.Exceptions.Add(exception.Day, exception.Kind);
                }

                if (restored.ContainsKey(employeeId))
                {
                    throw new InvalidOperationException(
                        "Saved employee schedule is duplicated.");
                }

                restored.Add(employeeId, state);

                if (record.HasLockedDay && record.LockedDay == currentDay)
                {
                    EmployeeShiftDefinition lockedShift =
                        ResolveShift(record.LockedShiftId);
                    restoredLocks.Add(
                        new LockedDayKey(employeeId, record.LockedDay),
                        new EmployeeDaySchedule(
                            record.LockedDay,
                            CycleDay(record.LockedDay),
                            record.LockedDayWorking,
                            lockedShift,
                            record.LockedExceptionKind));
                }
            }

            foreach (HiredEmployee employee in _roster.Employees)
            {
                if (!restored.ContainsKey(employee.EmployeeId))
                {
                    restored.Add(
                        employee.EmployeeId,
                        new ScheduleState(ShiftPresets[0]));
                }
            }

            _states.Clear();
            foreach (KeyValuePair<EmployeeId, ScheduleState> pair in restored)
            {
                _states.Add(pair.Key, pair.Value);
            }

            _lockedDays.Clear();
            foreach (KeyValuePair<LockedDayKey, EmployeeDaySchedule> pair
                     in restoredLocks)
            {
                _lockedDays.Add(pair.Key, pair.Value);
            }
        }

        private static EmployeeShiftDefinition ResolveShift(string shiftId)
        {
            foreach (EmployeeShiftDefinition shift in ShiftPresets)
            {
                if (string.Equals(
                        shift.ShiftId,
                        shiftId,
                        StringComparison.Ordinal))
                {
                    return shift;
                }
            }

            throw new InvalidOperationException(
                "Saved employee shift '" + shiftId + "' is not available.");
        }

        private void EnsureSession()
        {
            if (!_activeSession.HasActiveSession)
            {
                return;
            }

            string sessionId = _activeSession.Snapshot.SessionId.Value;
            if (string.Equals(
                    _boundSessionId,
                    sessionId,
                    StringComparison.Ordinal))
            {
                return;
            }

            _boundSessionId = sessionId;
            _states.Clear();
            _lockedDays.Clear();
        }

        private static int CycleDay(int day)
        {
            if (day < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(day));
            }

            return ((day - 1) % 7) + 1;
        }

        private sealed class ScheduleState
        {
            public EmployeeShiftDefinition Shift { get; set; }
            public bool[] WorkingDays { get; }
            public Dictionary<int, EmployeeScheduleExceptionKind>
                Exceptions { get; }

            public ScheduleState(EmployeeShiftDefinition shift)
            {
                Shift = shift ?? throw new ArgumentNullException(nameof(shift));
                WorkingDays = new[]
                {
                    true, true, true, true, true, true, true
                };
                Exceptions =
                    new Dictionary<int, EmployeeScheduleExceptionKind>();
            }
        }

        private readonly struct LockedDayKey : IEquatable<LockedDayKey>
        {
            private readonly EmployeeId _employeeId;
            private readonly int _day;

            public LockedDayKey(EmployeeId employeeId, int day)
            {
                _employeeId = employeeId;
                _day = day;
            }

            public bool Equals(LockedDayKey other)
            {
                return _employeeId == other._employeeId && _day == other._day;
            }

            public override bool Equals(object obj)
            {
                return obj is LockedDayKey other && Equals(other);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (_employeeId.GetHashCode() * 397) ^ _day;
                }
            }
        }
    }
}
