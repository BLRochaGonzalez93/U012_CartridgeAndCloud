using System;

namespace VRMGames.CartridgeAndCloud.Domain.Employees
{
    public enum EmployeeScheduleExceptionKind
    {
        None = 0,
        DayOff = 1,
        ForceWork = 2
    }

    public enum EmployeeWorkAvailability
    {
        DayOff = 0,
        BeforeShift = 1,
        Working = 2,
        OnBreak = 3,
        AfterShift = 4
    }

    public sealed class EmployeeShiftDefinition
    {
        public string ShiftId { get; }
        public string DisplayName { get; }
        public int StartMinute { get; }
        public int EndMinute { get; }
        public int BreakStartMinute { get; }
        public int BreakDurationMinutes { get; }
        public int BreakEndMinute =>
            checked(BreakStartMinute + BreakDurationMinutes);

        public EmployeeShiftDefinition(
            string shiftId,
            string displayName,
            int startMinute,
            int endMinute,
            int breakStartMinute,
            int breakDurationMinutes)
        {
            if (string.IsNullOrWhiteSpace(shiftId))
            {
                throw new ArgumentException(
                    "Shift ID is required.",
                    nameof(shiftId));
            }

            if (string.IsNullOrWhiteSpace(displayName))
            {
                throw new ArgumentException(
                    "Shift display name is required.",
                    nameof(displayName));
            }

            if (startMinute < 0 || startMinute >= 24 * 60)
            {
                throw new ArgumentOutOfRangeException(nameof(startMinute));
            }

            if (endMinute <= startMinute || endMinute > 24 * 60)
            {
                throw new ArgumentOutOfRangeException(nameof(endMinute));
            }

            if (breakDurationMinutes < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(breakDurationMinutes));
            }

            if (breakDurationMinutes == 0)
            {
                if (breakStartMinute != 0)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(breakStartMinute),
                        "A shift without a break must use minute 0 as its break start.");
                }
            }
            else
            {
                int breakEnd = checked(
                    breakStartMinute + breakDurationMinutes);

                if (breakStartMinute < startMinute ||
                    breakEnd > endMinute)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(breakStartMinute),
                        "The scheduled break must be fully contained inside the shift.");
                }
            }

            ShiftId = shiftId.Trim();
            DisplayName = displayName.Trim();
            StartMinute = startMinute;
            EndMinute = endMinute;
            BreakStartMinute = breakStartMinute;
            BreakDurationMinutes = breakDurationMinutes;
        }

        public EmployeeWorkAvailability EvaluateMinute(int virtualMinute)
        {
            if (virtualMinute < 0 || virtualMinute > 24 * 60)
            {
                throw new ArgumentOutOfRangeException(nameof(virtualMinute));
            }

            if (virtualMinute < StartMinute)
            {
                return EmployeeWorkAvailability.BeforeShift;
            }

            if (virtualMinute >= EndMinute)
            {
                return EmployeeWorkAvailability.AfterShift;
            }

            if (BreakDurationMinutes > 0 &&
                virtualMinute >= BreakStartMinute &&
                virtualMinute < BreakEndMinute)
            {
                return EmployeeWorkAvailability.OnBreak;
            }

            return EmployeeWorkAvailability.Working;
        }
    }

    public sealed class EmployeeDaySchedule
    {
        public int Day { get; }
        public int CycleDay { get; }
        public bool IsWorkingDay { get; }
        public EmployeeShiftDefinition Shift { get; }
        public EmployeeScheduleExceptionKind ExceptionKind { get; }

        public EmployeeDaySchedule(
            int day,
            int cycleDay,
            bool isWorkingDay,
            EmployeeShiftDefinition shift,
            EmployeeScheduleExceptionKind exceptionKind)
        {
            if (day < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(day));
            }

            if (cycleDay < 1 || cycleDay > 7)
            {
                throw new ArgumentOutOfRangeException(nameof(cycleDay));
            }

            Day = day;
            CycleDay = cycleDay;
            IsWorkingDay = isWorkingDay;
            Shift = shift ?? throw new ArgumentNullException(nameof(shift));
            ExceptionKind = exceptionKind;
        }

        public EmployeeWorkAvailability EvaluateMinute(int virtualMinute)
        {
            return IsWorkingDay
                ? Shift.EvaluateMinute(virtualMinute)
                : EmployeeWorkAvailability.DayOff;
        }
    }
}
