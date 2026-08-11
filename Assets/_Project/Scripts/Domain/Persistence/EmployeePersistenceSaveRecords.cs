using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VRMGames.CartridgeAndCloud.Domain.Employees;

namespace VRMGames.CartridgeAndCloud.Domain.Persistence
{
    public sealed class EmployeeCandidateSaveRecord
    {
        public string CandidateId { get; }
        public string DisplayName { get; }
        public EmployeeProfile Profile { get; }
        public EmployeeSeniority Seniority { get; }
        public int ClerkSkill { get; }
        public int RestockingSkill { get; }
        public int OrderPickingSkill { get; }
        public string WorkSpeed { get; }
        public string Experience { get; }
        public long RequestedDailySalaryCents { get; }
        public string Trait { get; }
        public int AvailableFromDay { get; }
        public string State { get; }

        public EmployeeCandidateSaveRecord(
            string candidateId,
            string displayName,
            EmployeeProfile profile,
            EmployeeSeniority seniority,
            int clerkSkill,
            int restockingSkill,
            int orderPickingSkill,
            string workSpeed,
            string experience,
            long requestedDailySalaryCents,
            string trait,
            int availableFromDay,
            string state)
        {
            global::VRMGames.CartridgeAndCloud.Domain.Employees.CandidateId parsedId = global::VRMGames.CartridgeAndCloud.Domain.Employees.CandidateId.Parse(
                SaveRecordGuard.Required(candidateId, nameof(candidateId)));

            if (!Enum.IsDefined(typeof(EmployeeProfile), profile))
            {
                throw new ArgumentOutOfRangeException(nameof(profile));
            }

            if (!Enum.IsDefined(typeof(EmployeeSeniority), seniority))
            {
                throw new ArgumentOutOfRangeException(nameof(seniority));
            }

            _ = new EmployeeSkillSet(
                clerkSkill,
                restockingSkill,
                orderPickingSkill);

            if (requestedDailySalaryCents <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(requestedDailySalaryCents));
            }

            if (availableFromDay < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(availableFromDay));
            }

            string normalizedState =
                SaveRecordGuard.Required(state, nameof(state));
            if (!IsCandidateState(normalizedState))
            {
                throw new ArgumentException(
                    "Candidate state is not supported.",
                    nameof(state));
            }

            CandidateId = parsedId.Value;
            DisplayName = SaveRecordGuard.Required(
                displayName,
                nameof(displayName));
            Profile = profile;
            Seniority = seniority;
            ClerkSkill = clerkSkill;
            RestockingSkill = restockingSkill;
            OrderPickingSkill = orderPickingSkill;
            WorkSpeed = SaveRecordGuard.Required(
                workSpeed,
                nameof(workSpeed));
            Experience = SaveRecordGuard.Required(
                experience,
                nameof(experience));
            RequestedDailySalaryCents = requestedDailySalaryCents;
            Trait = SaveRecordGuard.Required(trait, nameof(trait));
            AvailableFromDay = availableFromDay;
            State = normalizedState;
        }

        private static bool IsCandidateState(string state)
        {
            return string.Equals(state, "Available", StringComparison.Ordinal) ||
                   string.Equals(state, "Hired", StringComparison.Ordinal) ||
                   string.Equals(state, "Discarded", StringComparison.Ordinal);
        }
    }

    public sealed class RecruitmentPostingSaveRecord
    {
        private readonly ReadOnlyCollection<EmployeeCandidateSaveRecord>
            _candidates;

        public string PostingId { get; }
        public string ChannelId { get; }
        public int PublishedDay { get; }
        public int ReadyDay { get; }
        public int ExpiryDayExclusive { get; }
        public long PaidCostCents { get; }
        public bool UsedFirstPublicationBenefit { get; }
        public IReadOnlyList<EmployeeCandidateSaveRecord> Candidates =>
            _candidates;

        public RecruitmentPostingSaveRecord(
            string postingId,
            string channelId,
            int publishedDay,
            int readyDay,
            int expiryDayExclusive,
            long paidCostCents,
            bool usedFirstPublicationBenefit,
            IEnumerable<EmployeeCandidateSaveRecord> candidates)
        {
            if (publishedDay < 1 ||
                readyDay < publishedDay ||
                expiryDayExclusive <= readyDay)
            {
                throw new ArgumentOutOfRangeException(nameof(readyDay));
            }

            if (paidCostCents < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(paidCostCents));
            }

            PostingId = SaveRecordGuard.Required(
                postingId,
                nameof(postingId));
            ChannelId = SaveRecordGuard.Required(
                channelId,
                nameof(channelId));
            PublishedDay = publishedDay;
            ReadyDay = readyDay;
            ExpiryDayExclusive = expiryDayExclusive;
            PaidCostCents = paidCostCents;
            UsedFirstPublicationBenefit = usedFirstPublicationBenefit;
            _candidates = SaveRecordGuard.Copy(
                candidates,
                nameof(candidates));

            HashSet<string> ids = new HashSet<string>(StringComparer.Ordinal);
            foreach (EmployeeCandidateSaveRecord candidate in _candidates)
            {
                SaveRecordGuard.Unique(
                    ids,
                    candidate.CandidateId,
                    "employee candidate");
            }
        }
    }

    public sealed class HiredEmployeeSaveRecord
    {
        public string EmployeeId { get; }
        public string SourceCandidateId { get; }
        public string DisplayName { get; }
        public EmployeeProfile Profile { get; }
        public EmployeeSeniority Seniority { get; }
        public int ClerkSkill { get; }
        public int RestockingSkill { get; }
        public int OrderPickingSkill { get; }
        public long ContractedDailySalaryCents { get; }
        public string Trait { get; }
        public int HiredDay { get; }
        public int StartDay { get; }

        public HiredEmployeeSaveRecord(
            string employeeId,
            string sourceCandidateId,
            string displayName,
            EmployeeProfile profile,
            EmployeeSeniority seniority,
            int clerkSkill,
            int restockingSkill,
            int orderPickingSkill,
            long contractedDailySalaryCents,
            string trait,
            int hiredDay,
            int startDay)
        {
            global::VRMGames.CartridgeAndCloud.Domain.Employees.EmployeeId parsedEmployeeId =
                global::VRMGames.CartridgeAndCloud.Domain.Employees.EmployeeId.Parse(
                    SaveRecordGuard.Required(employeeId, nameof(employeeId)));
            CandidateId parsedCandidateId =
                global::VRMGames.CartridgeAndCloud.Domain.Employees.CandidateId.Parse(
                    SaveRecordGuard.Required(
                        sourceCandidateId,
                        nameof(sourceCandidateId)));

            if (!Enum.IsDefined(typeof(EmployeeProfile), profile))
            {
                throw new ArgumentOutOfRangeException(nameof(profile));
            }

            if (!Enum.IsDefined(typeof(EmployeeSeniority), seniority))
            {
                throw new ArgumentOutOfRangeException(nameof(seniority));
            }

            _ = new EmployeeSkillSet(
                clerkSkill,
                restockingSkill,
                orderPickingSkill);

            if (contractedDailySalaryCents <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(contractedDailySalaryCents));
            }

            if (hiredDay < 1 || startDay <= hiredDay)
            {
                throw new ArgumentOutOfRangeException(nameof(startDay));
            }

            EmployeeId = parsedEmployeeId.Value;
            SourceCandidateId = parsedCandidateId.Value;
            DisplayName = SaveRecordGuard.Required(
                displayName,
                nameof(displayName));
            Profile = profile;
            Seniority = seniority;
            ClerkSkill = clerkSkill;
            RestockingSkill = restockingSkill;
            OrderPickingSkill = orderPickingSkill;
            ContractedDailySalaryCents = contractedDailySalaryCents;
            Trait = SaveRecordGuard.Required(trait, nameof(trait));
            HiredDay = hiredDay;
            StartDay = startDay;
        }
    }

    public sealed class EmployeeScheduleExceptionSaveRecord
    {
        public int Day { get; }
        public EmployeeScheduleExceptionKind Kind { get; }

        public EmployeeScheduleExceptionSaveRecord(
            int day,
            EmployeeScheduleExceptionKind kind)
        {
            if (day < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(day));
            }

            if (!Enum.IsDefined(
                    typeof(EmployeeScheduleExceptionKind),
                    kind) ||
                kind == EmployeeScheduleExceptionKind.None)
            {
                throw new ArgumentOutOfRangeException(nameof(kind));
            }

            Day = day;
            Kind = kind;
        }
    }

    public sealed class EmployeeScheduleSaveRecord
    {
        private readonly ReadOnlyCollection<EmployeeScheduleExceptionSaveRecord>
            _exceptions;

        public string EmployeeId { get; }
        public string ShiftId { get; }
        public bool Day1Working { get; }
        public bool Day2Working { get; }
        public bool Day3Working { get; }
        public bool Day4Working { get; }
        public bool Day5Working { get; }
        public bool Day6Working { get; }
        public bool Day7Working { get; }
        public IReadOnlyList<EmployeeScheduleExceptionSaveRecord> Exceptions =>
            _exceptions;
        public int LockedDay { get; }
        public bool LockedDayWorking { get; }
        public string LockedShiftId { get; }
        public EmployeeScheduleExceptionKind LockedExceptionKind { get; }

        public bool HasLockedDay => LockedDay > 0;

        public EmployeeScheduleSaveRecord(
            string employeeId,
            string shiftId,
            bool day1Working,
            bool day2Working,
            bool day3Working,
            bool day4Working,
            bool day5Working,
            bool day6Working,
            bool day7Working,
            IEnumerable<EmployeeScheduleExceptionSaveRecord> exceptions,
            int lockedDay,
            bool lockedDayWorking,
            string lockedShiftId,
            EmployeeScheduleExceptionKind lockedExceptionKind)
        {
            global::VRMGames.CartridgeAndCloud.Domain.Employees.EmployeeId parsedId =
                global::VRMGames.CartridgeAndCloud.Domain.Employees.EmployeeId.Parse(
                    SaveRecordGuard.Required(employeeId, nameof(employeeId)));

            if (lockedDay < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(lockedDay));
            }

            if (lockedDay == 0)
            {
                if (!string.IsNullOrEmpty(lockedShiftId) ||
                    lockedExceptionKind != EmployeeScheduleExceptionKind.None)
                {
                    throw new ArgumentException(
                        "Unlocked schedules cannot contain locked-day data.");
                }
            }
            else
            {
                SaveRecordGuard.Required(
                    lockedShiftId,
                    nameof(lockedShiftId));
                if (!Enum.IsDefined(
                        typeof(EmployeeScheduleExceptionKind),
                        lockedExceptionKind))
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(lockedExceptionKind));
                }
            }

            EmployeeId = parsedId.Value;
            ShiftId = SaveRecordGuard.Required(shiftId, nameof(shiftId));
            Day1Working = day1Working;
            Day2Working = day2Working;
            Day3Working = day3Working;
            Day4Working = day4Working;
            Day5Working = day5Working;
            Day6Working = day6Working;
            Day7Working = day7Working;
            _exceptions = SaveRecordGuard.Copy(
                exceptions,
                nameof(exceptions));
            LockedDay = lockedDay;
            LockedDayWorking = lockedDayWorking;
            LockedShiftId = lockedShiftId ?? string.Empty;
            LockedExceptionKind = lockedExceptionKind;

            HashSet<int> days = new HashSet<int>();
            foreach (EmployeeScheduleExceptionSaveRecord item in _exceptions)
            {
                if (!days.Add(item.Day))
                {
                    throw new ArgumentException(
                        "Employee schedule contains duplicate exception days.",
                        nameof(exceptions));
                }
            }
        }

        public bool[] CopyWorkingDays()
        {
            return new[]
            {
                Day1Working,
                Day2Working,
                Day3Working,
                Day4Working,
                Day5Working,
                Day6Working,
                Day7Working
            };
        }
    }

    public sealed class EmployeeSalaryObligationSaveRecord
    {
        public string EmployeeId { get; }
        public string EmployeeName { get; }
        public int DueDay { get; }
        public long AmountCents { get; }

        public EmployeeSalaryObligationSaveRecord(
            string employeeId,
            string employeeName,
            int dueDay,
            long amountCents)
        {
            global::VRMGames.CartridgeAndCloud.Domain.Employees.EmployeeId parsedId =
                global::VRMGames.CartridgeAndCloud.Domain.Employees.EmployeeId.Parse(
                    SaveRecordGuard.Required(employeeId, nameof(employeeId)));

            if (dueDay < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(dueDay));
            }

            if (amountCents <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amountCents));
            }

            EmployeeId = parsedId.Value;
            EmployeeName = SaveRecordGuard.Required(
                employeeName,
                nameof(employeeName));
            DueDay = dueDay;
            AmountCents = amountCents;
        }
    }

    public sealed class EmployeeSystemSaveRecord
    {
        private readonly ReadOnlyCollection<RecruitmentPostingSaveRecord>
            _postings;
        private readonly ReadOnlyCollection<HiredEmployeeSaveRecord>
            _employees;
        private readonly ReadOnlyCollection<EmployeeScheduleSaveRecord>
            _schedules;
        private readonly ReadOnlyCollection<EmployeeSalaryObligationSaveRecord>
            _outstandingSalaries;

        public int PostingSequence { get; }
        public bool HasPublishedAnyPosting { get; }
        public IReadOnlyList<RecruitmentPostingSaveRecord> Postings => _postings;
        public IReadOnlyList<HiredEmployeeSaveRecord> Employees => _employees;
        public IReadOnlyList<EmployeeScheduleSaveRecord> Schedules => _schedules;
        public IReadOnlyList<EmployeeSalaryObligationSaveRecord>
            OutstandingSalaries => _outstandingSalaries;

        public int TotalRecordCount =>
            _postings.Count +
            _employees.Count +
            _schedules.Count +
            _outstandingSalaries.Count;

        public EmployeeSystemSaveRecord(
            int postingSequence,
            bool hasPublishedAnyPosting,
            IEnumerable<RecruitmentPostingSaveRecord> postings,
            IEnumerable<HiredEmployeeSaveRecord> employees,
            IEnumerable<EmployeeScheduleSaveRecord> schedules,
            IEnumerable<EmployeeSalaryObligationSaveRecord> outstandingSalaries)
        {
            if (postingSequence < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(postingSequence));
            }

            PostingSequence = postingSequence;
            HasPublishedAnyPosting = hasPublishedAnyPosting;
            _postings = SaveRecordGuard.Copy(postings, nameof(postings));
            _employees = SaveRecordGuard.Copy(employees, nameof(employees));
            _schedules = SaveRecordGuard.Copy(schedules, nameof(schedules));
            _outstandingSalaries = SaveRecordGuard.Copy(
                outstandingSalaries,
                nameof(outstandingSalaries));

            Validate();
        }

        public static EmployeeSystemSaveRecord Empty()
        {
            return new EmployeeSystemSaveRecord(
                0,
                false,
                Array.Empty<RecruitmentPostingSaveRecord>(),
                Array.Empty<HiredEmployeeSaveRecord>(),
                Array.Empty<EmployeeScheduleSaveRecord>(),
                Array.Empty<EmployeeSalaryObligationSaveRecord>());
        }

        private void Validate()
        {
            HashSet<string> employeeIds =
                new HashSet<string>(StringComparer.Ordinal);
            foreach (HiredEmployeeSaveRecord employee in _employees)
            {
                SaveRecordGuard.Unique(
                    employeeIds,
                    employee.EmployeeId,
                    "employee");
            }

            HashSet<string> postingIds =
                new HashSet<string>(StringComparer.Ordinal);
            HashSet<string> candidateIds =
                new HashSet<string>(StringComparer.Ordinal);
            foreach (RecruitmentPostingSaveRecord posting in _postings)
            {
                SaveRecordGuard.Unique(
                    postingIds,
                    posting.PostingId,
                    "recruitment posting");
                foreach (EmployeeCandidateSaveRecord candidate
                         in posting.Candidates)
                {
                    SaveRecordGuard.Unique(
                        candidateIds,
                        candidate.CandidateId,
                        "employee candidate");
                }
            }

            HashSet<string> scheduleIds =
                new HashSet<string>(StringComparer.Ordinal);
            foreach (EmployeeScheduleSaveRecord schedule in _schedules)
            {
                SaveRecordGuard.Unique(
                    scheduleIds,
                    schedule.EmployeeId,
                    "employee schedule");
                if (!employeeIds.Contains(schedule.EmployeeId))
                {
                    throw new ArgumentException(
                        "Employee schedule references a missing employee.");
                }
            }

            if (scheduleIds.Count != employeeIds.Count)
            {
                throw new ArgumentException(
                    "Every persisted employee must have exactly one schedule.");
            }

            HashSet<string> salaryKeys =
                new HashSet<string>(StringComparer.Ordinal);
            foreach (EmployeeSalaryObligationSaveRecord obligation
                     in _outstandingSalaries)
            {
                if (!employeeIds.Contains(obligation.EmployeeId))
                {
                    throw new ArgumentException(
                        "Salary obligation references a missing employee.");
                }

                string key = obligation.EmployeeId + ":" +
                    obligation.DueDay.ToString();
                if (!salaryKeys.Add(key))
                {
                    throw new ArgumentException(
                        "Salary obligation is duplicated.");
                }
            }

            if (PostingSequence != _postings.Count)
            {
                throw new ArgumentException(
                    "Recruitment posting sequence does not match persisted postings.");
            }

            if (HasPublishedAnyPosting != (PostingSequence > 0))
            {
                throw new ArgumentException(
                    "Recruitment first-publication state is inconsistent.");
            }
        }
    }
}
