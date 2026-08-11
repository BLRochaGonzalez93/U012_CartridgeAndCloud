using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VRMGames.CartridgeAndCloud.Application.GameSession;
using VRMGames.CartridgeAndCloud.Application.Persistence;
using VRMGames.CartridgeAndCloud.Domain.Economy;
using VRMGames.CartridgeAndCloud.Domain.Employees;
using VRMGames.CartridgeAndCloud.Domain.Persistence;

namespace VRMGames.CartridgeAndCloud.Application.Employees
{
    public interface IEmployeeRosterSource
    {
        IReadOnlyList<HiredEmployee> Employees { get; }
    }

    public interface IEmployeePayrollStatus
    {
        bool HasOutstandingSalaryObligations { get; }
        long OutstandingSalaryCents { get; }
    }

    public interface IEmployeePayrollStoreAccess
    {
        long AvailableCashCents { get; }
    }

    public interface IEmployeePayrollClosingService :
        IEmployeePayrollStatus
    {
        EmployeePayrollOperationResult SettleDailyPayroll();
    }

    public enum EmployeePayrollOperationStatus
    {
        Paid = 0,
        NoPayrollDue = 1,
        OutstandingRecorded = 2,
        InvalidState = 3,
        InsufficientCash = 4
    }

    public sealed class EmployeePayrollOperationResult
    {
        public EmployeePayrollOperationStatus Status { get; }
        public string Detail { get; }

        public bool Succeeded =>
            Status == EmployeePayrollOperationStatus.Paid ||
            Status == EmployeePayrollOperationStatus.NoPayrollDue ||
            Status == EmployeePayrollOperationStatus.OutstandingRecorded;

        public bool HasOutstandingSalaryObligations =>
            Status == EmployeePayrollOperationStatus.OutstandingRecorded ||
            Status == EmployeePayrollOperationStatus.InsufficientCash;

        private EmployeePayrollOperationResult(
            EmployeePayrollOperationStatus status,
            string detail)
        {
            Status = status;
            Detail = detail ?? string.Empty;
        }

        public static EmployeePayrollOperationResult Create(
            EmployeePayrollOperationStatus status,
            string detail)
        {
            return new EmployeePayrollOperationResult(status, detail);
        }
    }

    public sealed class EmployeePayrollSummary
    {
        public int CurrentDay { get; }
        public int ActiveEmployeeCount { get; }
        public long DailyPayrollCents { get; }
        public long PaidTodayCents { get; }
        public int OutstandingObligationCount { get; }
        public long OutstandingSalaryCents { get; }

        public EmployeePayrollSummary(
            int currentDay,
            int activeEmployeeCount,
            long dailyPayrollCents,
            long paidTodayCents,
            int outstandingObligationCount,
            long outstandingSalaryCents)
        {
            CurrentDay = currentDay;
            ActiveEmployeeCount = activeEmployeeCount;
            DailyPayrollCents = dailyPayrollCents;
            PaidTodayCents = paidTodayCents;
            OutstandingObligationCount = outstandingObligationCount;
            OutstandingSalaryCents = outstandingSalaryCents;
        }
    }

    public sealed class EmployeePayrollService :
        IEmployeePayrollClosingService
    {
        private readonly IEmployeeRosterSource _roster;
        private readonly IActiveGameSession _activeSession;
        private readonly ISaveMutationRegistry _mutations;
        private readonly IUtcClock _clock;
        private readonly IEmployeeWorkCalendar _workCalendar;
        private IEmployeePayrollStoreAccess _storeAccess;
        private readonly List<EmployeeSalaryObligation> _outstanding;
        private readonly ReadOnlyCollection<EmployeeSalaryObligation>
            _readOnlyOutstanding;

        private string _boundSessionId = string.Empty;

        public event Action StateChanged;

        public IReadOnlyList<EmployeeSalaryObligation>
            OutstandingObligations => _readOnlyOutstanding;

        public bool HasOutstandingSalaryObligations
        {
            get
            {
                EnsureSession();
                return _outstanding.Count > 0;
            }
        }

        public long OutstandingSalaryCents
        {
            get
            {
                EnsureSession();
                return SumOutstanding();
            }
        }

        public EmployeePayrollService(
            IEmployeeRosterSource roster,
            IActiveGameSession activeSession,
            ISaveMutationRegistry mutations,
            IUtcClock clock,
            IEmployeeWorkCalendar workCalendar,
            IEmployeePayrollStoreAccess storeAccess)
        {
            _roster = roster ??
                throw new ArgumentNullException(nameof(roster));
            _activeSession = activeSession ??
                throw new ArgumentNullException(nameof(activeSession));
            _mutations = mutations ??
                throw new ArgumentNullException(nameof(mutations));
            _clock = clock ??
                throw new ArgumentNullException(nameof(clock));
            _workCalendar = workCalendar ??
                throw new ArgumentNullException(nameof(workCalendar));
            _storeAccess = storeAccess ??
                throw new ArgumentNullException(nameof(storeAccess));

            _outstanding = new List<EmployeeSalaryObligation>();
            _readOnlyOutstanding =
                new ReadOnlyCollection<EmployeeSalaryObligation>(
                    _outstanding);

            EnsureSession();
        }

        public void BindStoreAccess(
            IEmployeePayrollStoreAccess storeAccess)
        {
            _storeAccess = storeAccess ??
                throw new ArgumentNullException(nameof(storeAccess));
            EnsureSession();
        }

        public void DetachStoreAccess()
        {
            _storeAccess = null;
        }

        public EmployeePayrollSummary GetSummary()
        {
            EnsureSession();

            if (!_activeSession.HasActiveSession)
            {
                return new EmployeePayrollSummary(
                    0,
                    0,
                    0,
                    0,
                    _outstanding.Count,
                    SumOutstanding());
            }

            IntegratedGameStateSnapshot snapshot =
                _activeSession.Snapshot;
            int currentDay = snapshot.CurrentDay;
            int activeEmployeeCount = 0;
            long dailyPayrollCents = 0;

            foreach (HiredEmployee employee in _roster.Employees)
            {
                if (employee.StartDay > currentDay ||
                    !_workCalendar.IsScheduledWorkDay(
                        employee.EmployeeId,
                        currentDay))
                {
                    continue;
                }

                activeEmployeeCount++;
                dailyPayrollCents = checked(
                    dailyPayrollCents +
                    employee.ContractedDailySalaryCents);
            }

            long paidTodayCents = SumPaidForDay(
                snapshot,
                currentDay);

            return new EmployeePayrollSummary(
                currentDay,
                activeEmployeeCount,
                dailyPayrollCents,
                paidTodayCents,
                _outstanding.Count,
                SumOutstanding());
        }

        public EmployeePayrollOperationResult SettleDailyPayroll()
        {
            EnsureSession();

            if (!_activeSession.HasActiveSession ||
                _storeAccess == null)
            {
                return EmployeePayrollOperationResult.Create(
                    EmployeePayrollOperationStatus.InvalidState,
                    "Daily payroll requires an active store session.");
            }

            IntegratedGameStateSnapshot snapshot =
                _activeSession.Snapshot;

            if (!string.Equals(
                    snapshot.DayCycle.State,
                    "Closing",
                    StringComparison.Ordinal) &&
                !string.Equals(
                    snapshot.DayCycle.State,
                    "Closed",
                    StringComparison.Ordinal))
            {
                return EmployeePayrollOperationResult.Create(
                    EmployeePayrollOperationStatus.InvalidState,
                    "Daily payroll is settled during store closing.");
            }

            int currentDay = snapshot.CurrentDay;
            bool obligationsAdded = AddDueObligations(
                snapshot,
                currentDay);

            EmployeePayrollOperationResult result =
                TryPayOutstandingInternal(
                    allowOutstandingOnInsufficientCash: true);

            if (obligationsAdded &&
                result.Status == EmployeePayrollOperationStatus.NoPayrollDue)
            {
                StateChanged?.Invoke();
            }

            return result;
        }

        public EmployeePayrollOperationResult TrySettleOutstanding()
        {
            EnsureSession();

            if (!_activeSession.HasActiveSession ||
                _storeAccess == null)
            {
                return EmployeePayrollOperationResult.Create(
                    EmployeePayrollOperationStatus.InvalidState,
                    "Outstanding payroll requires an active store session.");
            }

            return TryPayOutstandingInternal(
                allowOutstandingOnInsufficientCash: false);
        }

        private bool AddDueObligations(
            IntegratedGameStateSnapshot snapshot,
            int currentDay)
        {
            bool changed = false;

            foreach (HiredEmployee employee in _roster.Employees)
            {
                if (employee.StartDay > currentDay ||
                    !_workCalendar.IsScheduledWorkDay(
                        employee.EmployeeId,
                        currentDay))
                {
                    continue;
                }

                EmployeeSalaryObligation obligation =
                    new EmployeeSalaryObligation(
                        employee.EmployeeId,
                        employee.DisplayName,
                        currentDay,
                        employee.ContractedDailySalaryCents);

                if (IsPaid(snapshot, obligation.SourceId) ||
                    ContainsOutstanding(obligation.SourceId))
                {
                    continue;
                }

                _outstanding.Add(obligation);
                changed = true;
            }

            if (changed)
            {
                SortOutstanding();
                StateChanged?.Invoke();
            }

            return changed;
        }

        private EmployeePayrollOperationResult
            TryPayOutstandingInternal(
                bool allowOutstandingOnInsufficientCash)
        {
            if (_outstanding.Count == 0)
            {
                return EmployeePayrollOperationResult.Create(
                    EmployeePayrollOperationStatus.NoPayrollDue,
                    "No employee salaries are outstanding.");
            }

            IntegratedGameStateSnapshot snapshot =
                _activeSession.Snapshot;
            long total = SumOutstanding();
            long availableCashCents =
                _storeAccess == null
                    ? 0
                    : _storeAccess.AvailableCashCents;

            if (availableCashCents < total)
            {
                EmployeePayrollOperationStatus status =
                    allowOutstandingOnInsufficientCash
                        ? EmployeePayrollOperationStatus
                            .OutstandingRecorded
                        : EmployeePayrollOperationStatus
                            .InsufficientCash;

                return EmployeePayrollOperationResult.Create(
                    status,
                    "Payroll requires " + total +
                    " minor units, but unreserved cash is " +
                    availableCashCents +
                    ". Salaries remain outstanding.");
            }

            IntegratedGameStateSnapshot working = snapshot;

            foreach (EmployeeSalaryObligation obligation
                     in _outstanding)
            {
                List<EconomyLedgerSaveRecord> ledger =
                    IntegratedSnapshotStoreOperationsMutator.AppendLedger(
                        working,
                        obligation.LedgerEntryId,
                        EconomyPostingType.EmployeeSalaryCost.ToString(),
                        obligation.SourceId,
                        obligation.AmountCents);

                working = IntegratedSnapshotStoreOperationsMutator.Clone(
                    working,
                    _clock.UtcNow,
                    ledgerEntries: ledger);
            }

            IntegratedGameStateSnapshot nextSnapshot =
                IntegratedSnapshotStoreOperationsMutator.Clone(
                    working,
                    _clock.UtcNow,
                    cashCents: checked(snapshot.CashCents - total));

            using (_mutations.Begin(
                       "employee-payroll:day-" +
                       snapshot.CurrentDay.ToString("0000")))
            {
                _activeSession.Replace(nextSnapshot);
            }

            int paidCount = _outstanding.Count;
            _outstanding.Clear();
            StateChanged?.Invoke();

            return EmployeePayrollOperationResult.Create(
                EmployeePayrollOperationStatus.Paid,
                "Paid " + paidCount +
                " salary obligation(s) for " + total +
                " minor units.");
        }

        private bool ContainsOutstanding(string sourceId)
        {
            foreach (EmployeeSalaryObligation obligation
                     in _outstanding)
            {
                if (string.Equals(
                        obligation.SourceId,
                        sourceId,
                        StringComparison.Ordinal))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsPaid(
            IntegratedGameStateSnapshot snapshot,
            string sourceId)
        {
            foreach (EconomyLedgerSaveRecord entry
                     in snapshot.LedgerEntries)
            {
                if (string.Equals(
                        entry.PostingType,
                        EconomyPostingType.EmployeeSalaryCost.ToString(),
                        StringComparison.Ordinal) &&
                    string.Equals(
                        entry.SourceId,
                        sourceId,
                        StringComparison.Ordinal))
                {
                    return true;
                }
            }

            return false;
        }

        private static long SumPaidForDay(
            IntegratedGameStateSnapshot snapshot,
            int day)
        {
            string dayId = "day-" + day.ToString("000");
            long total = 0;

            foreach (EconomyLedgerSaveRecord entry
                     in snapshot.LedgerEntries)
            {
                if (!string.Equals(
                        entry.DayId,
                        dayId,
                        StringComparison.Ordinal) ||
                    !string.Equals(
                        entry.PostingType,
                        EconomyPostingType.EmployeeSalaryCost.ToString(),
                        StringComparison.Ordinal))
                {
                    continue;
                }

                total = checked(total + entry.MinorUnits);
            }

            return total;
        }

        private long SumOutstanding()
        {
            long total = 0;
            foreach (EmployeeSalaryObligation obligation
                     in _outstanding)
            {
                total = checked(total + obligation.AmountCents);
            }

            return total;
        }

        private void SortOutstanding()
        {
            _outstanding.Sort(
                (left, right) =>
                {
                    int dayComparison =
                        left.DueDay.CompareTo(right.DueDay);
                    if (dayComparison != 0)
                    {
                        return dayComparison;
                    }

                    return string.CompareOrdinal(
                        left.EmployeeId.Value,
                        right.EmployeeId.Value);
                });
        }

        public IReadOnlyList<EmployeeSalaryObligationSaveRecord>
            CaptureOutstandingObligations()
        {
            EnsureSession();
            List<EmployeeSalaryObligationSaveRecord> records =
                new List<EmployeeSalaryObligationSaveRecord>(
                    _outstanding.Count);

            foreach (EmployeeSalaryObligation obligation in _outstanding)
            {
                records.Add(
                    new EmployeeSalaryObligationSaveRecord(
                        obligation.EmployeeId.Value,
                        obligation.EmployeeName,
                        obligation.DueDay,
                        obligation.AmountCents));
            }

            return records;
        }

        public void RestoreOutstandingObligations(
            IReadOnlyList<EmployeeSalaryObligationSaveRecord> records)
        {
            if (records == null)
            {
                throw new ArgumentNullException(nameof(records));
            }

            EnsureSession();
            List<EmployeeSalaryObligation> restored =
                new List<EmployeeSalaryObligation>(records.Count);
            HashSet<string> sourceIds =
                new HashSet<string>(StringComparer.Ordinal);

            foreach (EmployeeSalaryObligationSaveRecord record in records)
            {
                EmployeeId employeeId = EmployeeId.Parse(record.EmployeeId);
                bool employeeExists = false;
                foreach (HiredEmployee employee in _roster.Employees)
                {
                    if (employee.EmployeeId == employeeId)
                    {
                        employeeExists = true;
                        break;
                    }
                }

                if (!employeeExists)
                {
                    throw new InvalidOperationException(
                        "Saved salary obligation references a missing employee.");
                }

                EmployeeSalaryObligation obligation =
                    new EmployeeSalaryObligation(
                        employeeId,
                        record.EmployeeName,
                        record.DueDay,
                        record.AmountCents);

                if (!sourceIds.Add(obligation.SourceId))
                {
                    throw new InvalidOperationException(
                        "Saved salary obligation is duplicated.");
                }

                restored.Add(obligation);
            }

            _outstanding.Clear();
            _outstanding.AddRange(restored);
            SortOutstanding();
        }

        private void EnsureSession()
        {
            if (!_activeSession.HasActiveSession)
            {
                return;
            }

            string sessionId =
                _activeSession.Snapshot.SessionId.Value;
            if (string.Equals(
                    _boundSessionId,
                    sessionId,
                    StringComparison.Ordinal))
            {
                return;
            }

            _boundSessionId = sessionId;
            _outstanding.Clear();
        }
    }
}
