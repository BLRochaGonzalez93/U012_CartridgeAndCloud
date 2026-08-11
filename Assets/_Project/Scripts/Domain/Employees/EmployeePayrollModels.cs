using System;

namespace VRMGames.CartridgeAndCloud.Domain.Employees
{
    public sealed class EmployeeSalaryObligation
    {
        public EmployeeId EmployeeId { get; }
        public string EmployeeName { get; }
        public int DueDay { get; }
        public long AmountCents { get; }

        public string SourceId =>
            EmployeeId.Value +
            ":salary-day:" +
            DueDay.ToString("0000");

        public string LedgerEntryId =>
            "employee-salary-day-" +
            DueDay.ToString("0000") +
            "-" +
            EmployeeId.Value;

        public EmployeeSalaryObligation(
            EmployeeId employeeId,
            string employeeName,
            int dueDay,
            long amountCents)
        {
            if (!employeeId.IsInitialized)
            {
                throw new ArgumentException(
                    "Employee ID must be initialized.",
                    nameof(employeeId));
            }

            if (string.IsNullOrWhiteSpace(employeeName))
            {
                throw new ArgumentException(
                    "Employee name is required.",
                    nameof(employeeName));
            }

            if (dueDay < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(dueDay));
            }

            if (amountCents <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(amountCents));
            }

            EmployeeId = employeeId;
            EmployeeName = employeeName.Trim();
            DueDay = dueDay;
            AmountCents = amountCents;
        }
    }
}
