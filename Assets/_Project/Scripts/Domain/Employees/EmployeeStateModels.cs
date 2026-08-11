using System;

namespace VRMGames.CartridgeAndCloud.Domain.Employees
{
    public enum EmployeeOperationalState
    {
        AwaitingStart = 0,
        OffDuty = 1,
        Available = 2,
        OnBreak = 3,
        Closing = 4
    }

    public readonly struct EmployeeStateSnapshot
    {
        public EmployeeId EmployeeId { get; }
        public EmployeeOperationalState State { get; }
        public EmployeeWorkAvailability ScheduleAvailability { get; }
        public int Day { get; }
        public int VirtualMinute { get; }

        public bool IsPhysicallyPresent =>
            State == EmployeeOperationalState.Available ||
            State == EmployeeOperationalState.OnBreak ||
            State == EmployeeOperationalState.Closing;

        public bool CanAcceptTasks =>
            State == EmployeeOperationalState.Available;

        public bool IsPaidWorkingTime =>
            State == EmployeeOperationalState.Available ||
            State == EmployeeOperationalState.OnBreak ||
            State == EmployeeOperationalState.Closing;

        public EmployeeStateSnapshot(
            EmployeeId employeeId,
            EmployeeOperationalState state,
            EmployeeWorkAvailability scheduleAvailability,
            int day,
            int virtualMinute)
        {
            if (!employeeId.IsInitialized)
            {
                throw new ArgumentException(
                    "Employee ID must be initialized.",
                    nameof(employeeId));
            }

            if (!Enum.IsDefined(typeof(EmployeeOperationalState), state))
            {
                throw new ArgumentOutOfRangeException(nameof(state));
            }

            if (!Enum.IsDefined(
                    typeof(EmployeeWorkAvailability),
                    scheduleAvailability))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(scheduleAvailability));
            }

            if (day < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(day));
            }

            if (virtualMinute < 0 || virtualMinute > 24 * 60)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(virtualMinute));
            }

            EmployeeId = employeeId;
            State = state;
            ScheduleAvailability = scheduleAvailability;
            Day = day;
            VirtualMinute = virtualMinute;
        }
    }
}
