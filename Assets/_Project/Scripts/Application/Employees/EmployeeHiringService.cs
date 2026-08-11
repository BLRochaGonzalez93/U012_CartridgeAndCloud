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
    public interface IEmployeeHiringStoreAccess
    {
        long AvailableCashCents { get; }

        bool HasValidWorkArea { get; }
    }

    public enum RecruitmentPostingState
    {
        Pending = 0,
        Available = 1,
        Closed = 2,
        Expired = 3
    }

    public enum RecruitmentCandidateState
    {
        Available = 0,
        Hired = 1,
        Discarded = 2
    }

    public sealed class RecruitmentCandidateEntry
    {
        public EmployeeCandidate Candidate { get; }

        public RecruitmentCandidateState State { get; private set; }

        public RecruitmentCandidateEntry(EmployeeCandidate candidate)
        {
            Candidate = candidate ??
                throw new ArgumentNullException(nameof(candidate));
            State = RecruitmentCandidateState.Available;
        }

        internal void MarkHired()
        {
            State = RecruitmentCandidateState.Hired;
        }

        internal void MarkDiscarded()
        {
            State = RecruitmentCandidateState.Discarded;
        }
    }

    public sealed class RecruitmentPosting
    {
        private readonly ReadOnlyCollection<RecruitmentCandidateEntry>
            _candidates;

        public string PostingId { get; }
        public RecruitmentChannelDefinition Channel { get; }
        public int PublishedDay { get; }
        public int ReadyDay { get; }
        public int ExpiryDayExclusive { get; }
        public long PaidCostCents { get; }
        public bool UsedFirstPublicationBenefit { get; }

        public IReadOnlyList<RecruitmentCandidateEntry> Candidates =>
            _candidates;

        internal RecruitmentPosting(
            string postingId,
            RecruitmentChannelDefinition channel,
            int publishedDay,
            int readyDay,
            int expiryDayExclusive,
            long paidCostCents,
            bool usedFirstPublicationBenefit,
            IEnumerable<RecruitmentCandidateEntry> candidates)
        {
            if (string.IsNullOrWhiteSpace(postingId))
            {
                throw new ArgumentException(
                    "Posting ID is required.",
                    nameof(postingId));
            }

            if (channel == null)
            {
                throw new ArgumentNullException(nameof(channel));
            }

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

            if (candidates == null)
            {
                throw new ArgumentNullException(nameof(candidates));
            }

            List<RecruitmentCandidateEntry> candidateCopy =
                new List<RecruitmentCandidateEntry>(candidates);
            if (candidateCopy.Count != channel.CandidateCount)
            {
                throw new ArgumentException(
                    "Candidate count must match the recruitment channel.",
                    nameof(candidates));
            }

            PostingId = postingId.Trim();
            Channel = channel;
            PublishedDay = publishedDay;
            ReadyDay = readyDay;
            ExpiryDayExclusive = expiryDayExclusive;
            PaidCostCents = paidCostCents;
            UsedFirstPublicationBenefit = usedFirstPublicationBenefit;
            _candidates = new ReadOnlyCollection<RecruitmentCandidateEntry>(
                candidateCopy);
        }

        public RecruitmentPostingState GetState(int currentDay)
        {
            if (currentDay < ReadyDay)
            {
                return RecruitmentPostingState.Pending;
            }

            if (currentDay >= ExpiryDayExclusive)
            {
                return RecruitmentPostingState.Expired;
            }

            foreach (RecruitmentCandidateEntry candidate in _candidates)
            {
                if (candidate.State == RecruitmentCandidateState.Available)
                {
                    return RecruitmentPostingState.Available;
                }
            }

            return RecruitmentPostingState.Closed;
        }
    }

    public sealed class EmployeeHiringEligibility
    {
        public bool HasActiveSession { get; }
        public bool MeetsOperatingDayRequirement { get; }
        public bool HasValidWorkArea { get; }
        public bool HasNoOutstandingSalaryObligations { get; }
        public bool ProgressionGatesDeferred { get; }
        public int CurrentDay { get; }
        public int MinimumOperatingDays { get; }
        public int MinimumBusinessLevel { get; }
        public int MinimumReputation { get; }

        public bool CanRecruit =>
            HasActiveSession &&
            MeetsOperatingDayRequirement &&
            HasValidWorkArea &&
            HasNoOutstandingSalaryObligations;

        public EmployeeHiringEligibility(
            bool hasActiveSession,
            bool meetsOperatingDayRequirement,
            bool hasValidWorkArea,
            bool hasNoOutstandingSalaryObligations,
            int currentDay,
            int minimumOperatingDays,
            int minimumBusinessLevel,
            int minimumReputation)
        {
            HasActiveSession = hasActiveSession;
            MeetsOperatingDayRequirement =
                meetsOperatingDayRequirement;
            HasValidWorkArea = hasValidWorkArea;
            HasNoOutstandingSalaryObligations =
                hasNoOutstandingSalaryObligations;
            ProgressionGatesDeferred = true;
            CurrentDay = currentDay;
            MinimumOperatingDays = minimumOperatingDays;
            MinimumBusinessLevel = minimumBusinessLevel;
            MinimumReputation = minimumReputation;
        }
    }

    public enum EmployeeHiringOperationStatus
    {
        Success = 0,
        InvalidState = 1,
        NotFound = 2,
        RequirementsNotMet = 3,
        InsufficientCash = 4,
        Duplicate = 5,
        CandidateUnavailable = 6
    }

    public sealed class EmployeeHiringOperationResult
    {
        public bool Succeeded =>
            Status == EmployeeHiringOperationStatus.Success;

        public EmployeeHiringOperationStatus Status { get; }
        public string Detail { get; }

        private EmployeeHiringOperationResult(
            EmployeeHiringOperationStatus status,
            string detail)
        {
            Status = status;
            Detail = detail ?? string.Empty;
        }

        public static EmployeeHiringOperationResult Success(string detail)
        {
            return new EmployeeHiringOperationResult(
                EmployeeHiringOperationStatus.Success,
                detail);
        }

        public static EmployeeHiringOperationResult Failure(
            EmployeeHiringOperationStatus status,
            string detail)
        {
            if (status == EmployeeHiringOperationStatus.Success)
            {
                throw new ArgumentOutOfRangeException(nameof(status));
            }

            return new EmployeeHiringOperationResult(status, detail);
        }
    }

    public sealed class EmployeeHiringService :
        IEmployeeRosterSource
    {
        private readonly EmployeeHiringCatalog _catalog;
        private readonly IActiveGameSession _activeSession;
        private readonly ISaveMutationRegistry _mutations;
        private readonly IUtcClock _clock;
        private EmployeeIdRegistry _employeeIds;
        private readonly List<RecruitmentPosting> _postings;
        private readonly List<HiredEmployee> _employees;
        private readonly ReadOnlyCollection<RecruitmentPosting>
            _readOnlyPostings;
        private readonly ReadOnlyCollection<HiredEmployee>
            _readOnlyEmployees;

        private IEmployeeHiringStoreAccess _storeAccess;
        private IEmployeePayrollStatus _payrollStatus;
        private string _boundSessionId = string.Empty;
        private int _postingSequence;
        private bool _hasPublishedAnyPosting;

        public event Action StateChanged;
        public event Action PersistenceChanged;

        public EmployeeHiringCatalog Catalog => _catalog;

        public IReadOnlyList<RecruitmentPosting> Postings =>
            _readOnlyPostings;

        public IReadOnlyList<HiredEmployee> Employees =>
            _readOnlyEmployees;

        public EmployeeHiringService(
            EmployeeHiringCatalog catalog,
            IActiveGameSession activeSession,
            ISaveMutationRegistry mutations,
            IUtcClock clock,
            IEmployeeHiringStoreAccess storeAccess)
        {
            _catalog = catalog ??
                throw new ArgumentNullException(nameof(catalog));
            _activeSession = activeSession ??
                throw new ArgumentNullException(nameof(activeSession));
            _mutations = mutations ??
                throw new ArgumentNullException(nameof(mutations));
            _clock = clock ??
                throw new ArgumentNullException(nameof(clock));
            _storeAccess = storeAccess ??
                throw new ArgumentNullException(nameof(storeAccess));

            _employeeIds = new EmployeeIdRegistry();
            _postings = new List<RecruitmentPosting>();
            _employees = new List<HiredEmployee>();
            _readOnlyPostings =
                new ReadOnlyCollection<RecruitmentPosting>(_postings);
            _readOnlyEmployees =
                new ReadOnlyCollection<HiredEmployee>(_employees);

            EnsureSession();
        }

        public void BindStoreAccess(IEmployeeHiringStoreAccess storeAccess)
        {
            _storeAccess = storeAccess ??
                throw new ArgumentNullException(nameof(storeAccess));
            EnsureSession();
        }

        public void DetachStoreAccess()
        {
            _storeAccess = null;
        }

        public void BindPayrollStatus(
            IEmployeePayrollStatus payrollStatus)
        {
            _payrollStatus = payrollStatus ??
                throw new ArgumentNullException(nameof(payrollStatus));
        }

        public EmployeeHiringEligibility EvaluateEligibility()
        {
            EnsureSession();

            bool hasSession = _activeSession.HasActiveSession;
            int currentDay = hasSession
                ? _activeSession.Snapshot.CurrentDay
                : 0;

            return new EmployeeHiringEligibility(
                hasSession,
                hasSession &&
                    currentDay >= _catalog.MinimumOperatingDays,
                _storeAccess != null &&
                    _storeAccess.HasValidWorkArea,
                _payrollStatus == null ||
                    !_payrollStatus.HasOutstandingSalaryObligations,
                currentDay,
                _catalog.MinimumOperatingDays,
                _catalog.MinimumBusinessLevel,
                _catalog.MinimumReputation);
        }

        public long GetEffectivePostingCost(
            RecruitmentChannelDefinition channel)
        {
            if (channel == null)
            {
                throw new ArgumentNullException(nameof(channel));
            }

            return _hasPublishedAnyPosting
                ? channel.PostingCostCents
                : 0L;
        }

        public EmployeeHiringOperationResult Publish(
            string channelId)
        {
            EnsureSession();

            if (!_activeSession.HasActiveSession ||
                _storeAccess == null)
            {
                return Failure(
                    EmployeeHiringOperationStatus.InvalidState,
                    "Hiring is unavailable without an active store session.");
            }

            if (!_catalog.TryGetChannel(
                    channelId,
                    out RecruitmentChannelDefinition channel))
            {
                return Failure(
                    EmployeeHiringOperationStatus.NotFound,
                    "Recruitment channel was not found.");
            }

            EmployeeHiringEligibility eligibility =
                EvaluateEligibility();
            if (!eligibility.CanRecruit)
            {
                return Failure(
                    EmployeeHiringOperationStatus.RequirementsNotMet,
                    DescribeRequirements(eligibility));
            }

            int currentDay = _activeSession.Snapshot.CurrentDay;
            foreach (RecruitmentPosting posting in _postings)
            {
                RecruitmentPostingState state =
                    posting.GetState(currentDay);
                if (string.Equals(
                        posting.Channel.ChannelId,
                        channel.ChannelId,
                        StringComparison.Ordinal) &&
                    (state == RecruitmentPostingState.Pending ||
                     state == RecruitmentPostingState.Available))
                {
                    return Failure(
                        EmployeeHiringOperationStatus.Duplicate,
                        "This recruitment channel already has an active candidate group.");
                }
            }

            long cost = GetEffectivePostingCost(channel);
            if (_storeAccess.AvailableCashCents < cost)
            {
                return Failure(
                    EmployeeHiringOperationStatus.InsufficientCash,
                    "Available cash does not cover the recruitment posting.");
            }

            int nextSequence = checked(_postingSequence + 1);
            string postingId =
                "recruitment-" +
                _boundSessionId.Substring(0, 8) + "-" +
                nextSequence.ToString("0000");

            int delayDays = SelectDelayDays(channel, postingId);
            int readyDay = checked(currentDay + delayDays);
            int expiryDayExclusive = checked(
                readyDay + _catalog.CandidateWindowDays);
            bool firstPublication = !_hasPublishedAnyPosting;

            List<RecruitmentCandidateEntry> candidates =
                GenerateCandidates(
                    channel,
                    postingId,
                    readyDay,
                    firstPublication);

            RecruitmentPosting nextPosting =
                new RecruitmentPosting(
                    postingId,
                    channel,
                    currentDay,
                    readyDay,
                    expiryDayExclusive,
                    cost,
                    firstPublication,
                    candidates);

            if (cost > 0)
            {
                IntegratedGameStateSnapshot snapshot =
                    _activeSession.Snapshot;
                List<EconomyLedgerSaveRecord> ledger =
                    IntegratedSnapshotStoreOperationsMutator.AppendLedger(
                        snapshot,
                        "employee-recruitment-cost-" + postingId,
                        EconomyPostingType.EmployeeRecruitmentCost.ToString(),
                        postingId,
                        cost);
                IntegratedGameStateSnapshot nextSnapshot =
                    IntegratedSnapshotStoreOperationsMutator.Clone(
                        snapshot,
                        _clock.UtcNow,
                        cashCents: checked(snapshot.CashCents - cost),
                        ledgerEntries: ledger);

                using (_mutations.Begin(
                           "employee-recruitment:" + postingId))
                {
                    _activeSession.Replace(nextSnapshot);
                }
            }

            _postings.Add(nextPosting);
            _postingSequence = nextSequence;
            _hasPublishedAnyPosting = true;
            PersistenceChanged?.Invoke();
            StateChanged?.Invoke();

            string costDetail = cost == 0
                ? "The first publication was free."
                : "Recruitment posting paid from available cash.";

            return EmployeeHiringOperationResult.Success(
                channel.DisplayName +
                " published. Candidates arrive on day " +
                readyDay + ". " + costDetail);
        }

        public EmployeeHiringOperationResult Hire(CandidateId candidateId)
        {
            EnsureSession();

            if (!_activeSession.HasActiveSession)
            {
                return Failure(
                    EmployeeHiringOperationStatus.InvalidState,
                    "No active game session exists.");
            }

            if (_payrollStatus != null &&
                _payrollStatus.HasOutstandingSalaryObligations)
            {
                return Failure(
                    EmployeeHiringOperationStatus.RequirementsNotMet,
                    "Outstanding employee salaries must be paid before hiring.");
            }

            if (!TryFindCandidate(
                    candidateId,
                    out RecruitmentPosting posting,
                    out RecruitmentCandidateEntry entry))
            {
                return Failure(
                    EmployeeHiringOperationStatus.NotFound,
                    "Candidate was not found.");
            }

            int currentDay = _activeSession.Snapshot.CurrentDay;
            if (posting.GetState(currentDay) !=
                    RecruitmentPostingState.Available ||
                entry.State != RecruitmentCandidateState.Available)
            {
                return Failure(
                    EmployeeHiringOperationStatus.CandidateUnavailable,
                    "Candidate is no longer available for hiring.");
            }

            int startDay = checked(currentDay + 1);
            HiredEmployee employee = new HiredEmployee(
                _employeeIds.Allocate(),
                entry.Candidate,
                currentDay,
                startDay);

            _employees.Add(employee);
            entry.MarkHired();
            PersistenceChanged?.Invoke();
            StateChanged?.Invoke();

            return EmployeeHiringOperationResult.Success(
                employee.DisplayName +
                " hired. Start: day " +
                employee.StartDay + " at 08:00.");
        }

        public EmployeeHiringOperationResult Discard(
            CandidateId candidateId)
        {
            EnsureSession();

            if (!TryFindCandidate(
                    candidateId,
                    out RecruitmentPosting posting,
                    out RecruitmentCandidateEntry entry))
            {
                return Failure(
                    EmployeeHiringOperationStatus.NotFound,
                    "Candidate was not found.");
            }

            int currentDay = _activeSession.HasActiveSession
                ? _activeSession.Snapshot.CurrentDay
                : 0;

            if (posting.GetState(currentDay) !=
                    RecruitmentPostingState.Available ||
                entry.State != RecruitmentCandidateState.Available)
            {
                return Failure(
                    EmployeeHiringOperationStatus.CandidateUnavailable,
                    "Candidate is no longer available.");
            }

            entry.MarkDiscarded();
            PersistenceChanged?.Invoke();
            StateChanged?.Invoke();

            return EmployeeHiringOperationResult.Success(
                entry.Candidate.DisplayName + " discarded.");
        }

        public void RefreshForCurrentDay()
        {
            EnsureSession();
            StateChanged?.Invoke();
        }

        public int PostingSequence
        {
            get
            {
                EnsureSession();
                return _postingSequence;
            }
        }

        public bool HasPublishedAnyPosting
        {
            get
            {
                EnsureSession();
                return _hasPublishedAnyPosting;
            }
        }

        public IReadOnlyList<RecruitmentPostingSaveRecord>
            CapturePostings()
        {
            EnsureSession();
            List<RecruitmentPostingSaveRecord> records =
                new List<RecruitmentPostingSaveRecord>(_postings.Count);

            foreach (RecruitmentPosting posting in _postings)
            {
                List<EmployeeCandidateSaveRecord> candidates =
                    new List<EmployeeCandidateSaveRecord>(
                        posting.Candidates.Count);

                foreach (RecruitmentCandidateEntry entry
                         in posting.Candidates)
                {
                    EmployeeCandidate candidate = entry.Candidate;
                    candidates.Add(
                        new EmployeeCandidateSaveRecord(
                            candidate.CandidateId.Value,
                            candidate.DisplayName,
                            candidate.Profile,
                            candidate.Seniority,
                            candidate.Skills.Clerk,
                            candidate.Skills.Restocking,
                            candidate.Skills.OrderPicking,
                            candidate.WorkSpeed,
                            candidate.Experience,
                            candidate.RequestedDailySalaryCents,
                            candidate.Trait,
                            candidate.AvailableFromDay,
                            entry.State.ToString()));
                }

                records.Add(
                    new RecruitmentPostingSaveRecord(
                        posting.PostingId,
                        posting.Channel.ChannelId,
                        posting.PublishedDay,
                        posting.ReadyDay,
                        posting.ExpiryDayExclusive,
                        posting.PaidCostCents,
                        posting.UsedFirstPublicationBenefit,
                        candidates));
            }

            return records;
        }

        public IReadOnlyList<HiredEmployeeSaveRecord>
            CaptureEmployees()
        {
            EnsureSession();
            List<HiredEmployeeSaveRecord> records =
                new List<HiredEmployeeSaveRecord>(_employees.Count);

            foreach (HiredEmployee employee in _employees)
            {
                records.Add(
                    new HiredEmployeeSaveRecord(
                        employee.EmployeeId.Value,
                        employee.SourceCandidateId.Value,
                        employee.DisplayName,
                        employee.Profile,
                        employee.Seniority,
                        employee.Skills.Clerk,
                        employee.Skills.Restocking,
                        employee.Skills.OrderPicking,
                        employee.ContractedDailySalaryCents,
                        employee.Trait,
                        employee.HiredDay,
                        employee.StartDay));
            }

            return records;
        }

        public void Restore(EmployeeSystemSaveRecord state)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            EnsureSession();

            List<HiredEmployee> employees =
                new List<HiredEmployee>(state.Employees.Count);
            List<EmployeeId> employeeIds =
                new List<EmployeeId>(state.Employees.Count);

            foreach (HiredEmployeeSaveRecord record in state.Employees)
            {
                EmployeeId employeeId = EmployeeId.Parse(record.EmployeeId);
                employees.Add(
                    new HiredEmployee(
                        employeeId,
                        CandidateId.Parse(record.SourceCandidateId),
                        record.DisplayName,
                        record.Profile,
                        record.Seniority,
                        new EmployeeSkillSet(
                            record.ClerkSkill,
                            record.RestockingSkill,
                            record.OrderPickingSkill),
                        record.ContractedDailySalaryCents,
                        record.Trait,
                        record.HiredDay,
                        record.StartDay));
                employeeIds.Add(employeeId);
            }

            List<RecruitmentPosting> postings =
                new List<RecruitmentPosting>(state.Postings.Count);

            foreach (RecruitmentPostingSaveRecord record in state.Postings)
            {
                if (!_catalog.TryGetChannel(
                        record.ChannelId,
                        out RecruitmentChannelDefinition channel))
                {
                    throw new InvalidOperationException(
                        "Saved recruitment channel '" +
                        record.ChannelId + "' is not available.");
                }

                List<RecruitmentCandidateEntry> candidates =
                    new List<RecruitmentCandidateEntry>(
                        record.Candidates.Count);

                foreach (EmployeeCandidateSaveRecord candidateRecord
                         in record.Candidates)
                {
                    EmployeeCandidate candidate =
                        new EmployeeCandidate(
                            CandidateId.Parse(candidateRecord.CandidateId),
                            candidateRecord.DisplayName,
                            candidateRecord.Profile,
                            candidateRecord.Seniority,
                            new EmployeeSkillSet(
                                candidateRecord.ClerkSkill,
                                candidateRecord.RestockingSkill,
                                candidateRecord.OrderPickingSkill),
                            candidateRecord.WorkSpeed,
                            candidateRecord.Experience,
                            candidateRecord.RequestedDailySalaryCents,
                            candidateRecord.Trait,
                            candidateRecord.AvailableFromDay);
                    RecruitmentCandidateEntry entry =
                        new RecruitmentCandidateEntry(candidate);

                    if (string.Equals(
                            candidateRecord.State,
                            RecruitmentCandidateState.Hired.ToString(),
                            StringComparison.Ordinal))
                    {
                        entry.MarkHired();
                    }
                    else if (string.Equals(
                                 candidateRecord.State,
                                 RecruitmentCandidateState.Discarded.ToString(),
                                 StringComparison.Ordinal))
                    {
                        entry.MarkDiscarded();
                    }
                    else if (!string.Equals(
                                 candidateRecord.State,
                                 RecruitmentCandidateState.Available.ToString(),
                                 StringComparison.Ordinal))
                    {
                        throw new InvalidOperationException(
                            "Saved candidate state is not supported.");
                    }

                    candidates.Add(entry);
                }

                postings.Add(
                    new RecruitmentPosting(
                        record.PostingId,
                        channel,
                        record.PublishedDay,
                        record.ReadyDay,
                        record.ExpiryDayExclusive,
                        record.PaidCostCents,
                        record.UsedFirstPublicationBenefit,
                        candidates));
            }

            if (!state.HasPublishedAnyPosting && postings.Count > 0)
            {
                throw new InvalidOperationException(
                    "Saved recruitment state contains postings but the first-publication flag is unset.");
            }

            EmployeeIdRegistry restoredIds =
                new EmployeeIdRegistry(employeeIds);

            _postings.Clear();
            _postings.AddRange(postings);
            _employees.Clear();
            _employees.AddRange(employees);
            _employeeIds = restoredIds;
            _postingSequence = state.PostingSequence;
            _hasPublishedAnyPosting = state.HasPublishedAnyPosting;
        }

        private bool TryFindCandidate(
            CandidateId candidateId,
            out RecruitmentPosting posting,
            out RecruitmentCandidateEntry entry)
        {
            foreach (RecruitmentPosting currentPosting in _postings)
            {
                foreach (RecruitmentCandidateEntry currentEntry
                         in currentPosting.Candidates)
                {
                    if (currentEntry.Candidate.CandidateId != candidateId)
                    {
                        continue;
                    }

                    posting = currentPosting;
                    entry = currentEntry;
                    return true;
                }
            }

            posting = null;
            entry = null;
            return false;
        }

        private List<RecruitmentCandidateEntry> GenerateCandidates(
            RecruitmentChannelDefinition channel,
            string postingId,
            int readyDay,
            bool firstPublication)
        {
            List<RecruitmentCandidateEntry> candidates =
                new List<RecruitmentCandidateEntry>(
                    channel.CandidateCount);

            StableRandom random = new StableRandom(
                _boundSessionId + ":" + postingId);
            int nameOffset = random.Next(
                0,
                _catalog.CandidateNames.Count);

            for (int index = 0;
                 index < channel.CandidateCount;
                 index++)
            {
                bool guaranteedGeneralist =
                    firstPublication && index == 0;

                EmployeeProfile profile = guaranteedGeneralist
                    ? EmployeeProfile.Generalist
                    : (EmployeeProfile)random.Next(0, 4);

                EmployeeSeniority seniority =
                    guaranteedGeneralist
                        ? EmployeeSeniority.Junior
                        : SelectSeniority(channel, random);

                EmployeeSkillSet skills =
                    CreateSkills(profile, seniority, random);
                long salary = CreateSalary(
                    seniority,
                    guaranteedGeneralist,
                    random);
                string name = _catalog.CandidateNames[
                    (nameOffset + index) %
                    _catalog.CandidateNames.Count];
                string trait = _catalog.Traits[
                    random.Next(0, _catalog.Traits.Count)];

                EmployeeCandidate candidate =
                    new EmployeeCandidate(
                        CandidateId.New(),
                        name,
                        profile,
                        seniority,
                        skills,
                        WorkSpeedLabel(skills),
                        ExperienceLabel(seniority),
                        salary,
                        trait,
                        readyDay);

                candidates.Add(
                    new RecruitmentCandidateEntry(candidate));
            }

            return candidates;
        }

        private static EmployeeSeniority SelectSeniority(
            RecruitmentChannelDefinition channel,
            StableRandom random)
        {
            int minimum = (int)channel.MinimumSeniority;
            int maximum = (int)channel.MaximumSeniority;
            return (EmployeeSeniority)random.Next(
                minimum,
                maximum + 1);
        }

        private static EmployeeSkillSet CreateSkills(
            EmployeeProfile profile,
            EmployeeSeniority seniority,
            StableRandom random)
        {
            SkillRange principal;
            SkillRange secondary;

            switch (seniority)
            {
                case EmployeeSeniority.Junior:
                    principal = new SkillRange(1, 3);
                    secondary = new SkillRange(1, 2);
                    break;
                case EmployeeSeniority.Qualified:
                    principal = new SkillRange(3, 4);
                    secondary = new SkillRange(1, 3);
                    break;
                case EmployeeSeniority.Experienced:
                    principal = new SkillRange(4, 5);
                    secondary = new SkillRange(2, 4);
                    break;
                case EmployeeSeniority.Specialist:
                    principal = new SkillRange(5, 5);
                    secondary = new SkillRange(3, 5);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(seniority));
            }

            if (profile == EmployeeProfile.Generalist)
            {
                int clerk = random.Next(
                    secondary.Minimum,
                    principal.Maximum + 1);
                int restocking = random.Next(
                    secondary.Minimum,
                    principal.Maximum + 1);
                int picking = random.Next(
                    secondary.Minimum,
                    principal.Maximum + 1);
                return new EmployeeSkillSet(
                    clerk,
                    restocking,
                    picking);
            }

            int clerkSkill = RandomSkill(
                profile == EmployeeProfile.Clerk
                    ? principal
                    : secondary,
                random);
            int restockingSkill = RandomSkill(
                profile == EmployeeProfile.Restocker
                    ? principal
                    : secondary,
                random);
            int pickingSkill = RandomSkill(
                profile == EmployeeProfile.OrderPicker
                    ? principal
                    : secondary,
                random);

            return new EmployeeSkillSet(
                clerkSkill,
                restockingSkill,
                pickingSkill);
        }

        private static int RandomSkill(
            SkillRange range,
            StableRandom random)
        {
            return random.Next(
                range.Minimum,
                range.Maximum + 1);
        }

        private static long CreateSalary(
            EmployeeSeniority seniority,
            bool guaranteedGeneralist,
            StableRandom random)
        {
            int minimum;
            int maximum;

            switch (seniority)
            {
                case EmployeeSeniority.Junior:
                    minimum = guaranteedGeneralist ? 10000 : 9000;
                    maximum = 12500;
                    break;
                case EmployeeSeniority.Qualified:
                    minimum = 12500;
                    maximum = 17000;
                    break;
                case EmployeeSeniority.Experienced:
                    minimum = 17000;
                    maximum = 23000;
                    break;
                case EmployeeSeniority.Specialist:
                    minimum = 23000;
                    maximum = 32000;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(seniority));
            }

            int steppedMinimum = minimum / 500;
            int steppedMaximum = maximum / 500;
            return random.Next(
                steppedMinimum,
                steppedMaximum + 1) * 500L;
        }

        private static string WorkSpeedLabel(EmployeeSkillSet skills)
        {
            int total = checked(
                skills.Clerk +
                skills.Restocking +
                skills.OrderPicking);

            if (total >= 12)
            {
                return "Very fast";
            }

            if (total >= 9)
            {
                return "Fast";
            }

            if (total >= 6)
            {
                return "Steady";
            }

            return "Learning";
        }

        private static string ExperienceLabel(
            EmployeeSeniority seniority)
        {
            switch (seniority)
            {
                case EmployeeSeniority.Junior:
                    return "Entry level";
                case EmployeeSeniority.Qualified:
                    return "Qualified";
                case EmployeeSeniority.Experienced:
                    return "Experienced";
                case EmployeeSeniority.Specialist:
                    return "Specialist";
                default:
                    throw new ArgumentOutOfRangeException(nameof(seniority));
            }
        }

        private static int SelectDelayDays(
            RecruitmentChannelDefinition channel,
            string postingId)
        {
            if (channel.MinimumDelayDays == channel.MaximumDelayDays)
            {
                return channel.MinimumDelayDays;
            }

            StableRandom random = new StableRandom(postingId + ":delay");
            return random.Next(
                channel.MinimumDelayDays,
                channel.MaximumDelayDays + 1);
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
            _postings.Clear();
            _employees.Clear();
            _employeeIds = new EmployeeIdRegistry();
            _postingSequence = 0;
            _hasPublishedAnyPosting = false;
        }

        private static string DescribeRequirements(
            EmployeeHiringEligibility eligibility)
        {
            if (!eligibility.HasActiveSession)
            {
                return "An active game session is required.";
            }

            if (!eligibility.MeetsOperatingDayRequirement)
            {
                return "Hiring unlocks on operating day " +
                       eligibility.MinimumOperatingDays + ".";
            }

            if (!eligibility.HasValidWorkArea)
            {
                return "A valid checkout/work area is required before recruiting.";
            }

            if (!eligibility.HasNoOutstandingSalaryObligations)
            {
                return "Outstanding employee salaries must be paid before recruiting.";
            }

            return "Hiring requirements are not met.";
        }

        private static EmployeeHiringOperationResult Failure(
            EmployeeHiringOperationStatus status,
            string detail)
        {
            return EmployeeHiringOperationResult.Failure(status, detail);
        }

        private readonly struct SkillRange
        {
            public int Minimum { get; }
            public int Maximum { get; }

            public SkillRange(int minimum, int maximum)
            {
                Minimum = minimum;
                Maximum = maximum;
            }
        }

        private sealed class StableRandom
        {
            private uint _state;

            public StableRandom(string seed)
            {
                if (string.IsNullOrEmpty(seed))
                {
                    throw new ArgumentException(
                        "A deterministic seed is required.",
                        nameof(seed));
                }

                uint hash = 2166136261u;
                for (int index = 0; index < seed.Length; index++)
                {
                    hash ^= seed[index];
                    hash *= 16777619u;
                }

                _state = hash == 0 ? 0x9E3779B9u : hash;
            }

            public int Next(int minimumInclusive, int maximumExclusive)
            {
                if (maximumExclusive <= minimumInclusive)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(maximumExclusive));
                }

                _state ^= _state << 13;
                _state ^= _state >> 17;
                _state ^= _state << 5;

                uint range = (uint)(
                    maximumExclusive - minimumInclusive);
                return minimumInclusive +
                    (int)(_state % range);
            }
        }
    }
}
