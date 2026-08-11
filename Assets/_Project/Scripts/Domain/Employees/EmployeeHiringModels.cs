using System;

namespace VRMGames.CartridgeAndCloud.Domain.Employees
{
    public enum EmployeeProfile
    {
        Clerk = 0,
        Restocker = 1,
        OrderPicker = 2,
        Generalist = 3
    }

    public enum EmployeeSeniority
    {
        Junior = 0,
        Qualified = 1,
        Experienced = 2,
        Specialist = 3
    }

    public readonly struct EmployeeSkillSet
    {
        public int Clerk { get; }
        public int Restocking { get; }
        public int OrderPicking { get; }

        public EmployeeSkillSet(
            int clerk,
            int restocking,
            int orderPicking)
        {
            Clerk = RequireSkill(clerk, nameof(clerk));
            Restocking = RequireSkill(
                restocking,
                nameof(restocking));
            OrderPicking = RequireSkill(
                orderPicking,
                nameof(orderPicking));
        }

        private static int RequireSkill(
            int value,
            string parameterName)
        {
            if (value < 1 || value > 5)
            {
                throw new ArgumentOutOfRangeException(
                    parameterName,
                    "Employee skills must be between 1 and 5.");
            }

            return value;
        }
    }

    public sealed class RecruitmentChannelDefinition
    {
        public string ChannelId { get; }
        public string DisplayName { get; }
        public long PostingCostCents { get; }
        public int CandidateCount { get; }
        public int MinimumDelayDays { get; }
        public int MaximumDelayDays { get; }
        public EmployeeSeniority MinimumSeniority { get; }
        public EmployeeSeniority MaximumSeniority { get; }

        public RecruitmentChannelDefinition(
            string channelId,
            string displayName,
            long postingCostCents,
            int candidateCount,
            int minimumDelayDays,
            int maximumDelayDays,
            EmployeeSeniority minimumSeniority,
            EmployeeSeniority maximumSeniority)
        {
            if (string.IsNullOrWhiteSpace(channelId))
            {
                throw new ArgumentException(
                    "Recruitment channel ID is required.",
                    nameof(channelId));
            }

            if (string.IsNullOrWhiteSpace(displayName))
            {
                throw new ArgumentException(
                    "Recruitment channel name is required.",
                    nameof(displayName));
            }

            if (postingCostCents < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(postingCostCents));
            }

            if (candidateCount < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(candidateCount));
            }

            if (minimumDelayDays < 0 ||
                maximumDelayDays < minimumDelayDays)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(minimumDelayDays));
            }

            if (!Enum.IsDefined(
                    typeof(EmployeeSeniority),
                    minimumSeniority) ||
                !Enum.IsDefined(
                    typeof(EmployeeSeniority),
                    maximumSeniority) ||
                (int)maximumSeniority < (int)minimumSeniority)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(minimumSeniority));
            }

            ChannelId = channelId.Trim();
            DisplayName = displayName.Trim();
            PostingCostCents = postingCostCents;
            CandidateCount = candidateCount;
            MinimumDelayDays = minimumDelayDays;
            MaximumDelayDays = maximumDelayDays;
            MinimumSeniority = minimumSeniority;
            MaximumSeniority = maximumSeniority;
        }
    }

    public sealed class EmployeeCandidate
    {
        public CandidateId CandidateId { get; }
        public string DisplayName { get; }
        public EmployeeProfile Profile { get; }
        public EmployeeSeniority Seniority { get; }
        public EmployeeSkillSet Skills { get; }
        public string WorkSpeed { get; }
        public string Experience { get; }
        public long RequestedDailySalaryCents { get; }
        public string Trait { get; }
        public int AvailableFromDay { get; }

        public EmployeeCandidate(
            CandidateId candidateId,
            string displayName,
            EmployeeProfile profile,
            EmployeeSeniority seniority,
            EmployeeSkillSet skills,
            string workSpeed,
            string experience,
            long requestedDailySalaryCents,
            string trait,
            int availableFromDay)
        {
            if (!candidateId.IsInitialized)
            {
                throw new ArgumentException(
                    "Candidate ID must be initialized.",
                    nameof(candidateId));
            }

            if (string.IsNullOrWhiteSpace(displayName))
            {
                throw new ArgumentException(
                    "Candidate name is required.",
                    nameof(displayName));
            }

            if (!Enum.IsDefined(typeof(EmployeeProfile), profile))
            {
                throw new ArgumentOutOfRangeException(nameof(profile));
            }

            if (!Enum.IsDefined(
                    typeof(EmployeeSeniority),
                    seniority))
            {
                throw new ArgumentOutOfRangeException(nameof(seniority));
            }

            if (string.IsNullOrWhiteSpace(workSpeed))
            {
                throw new ArgumentException(
                    "Candidate work speed is required.",
                    nameof(workSpeed));
            }

            if (string.IsNullOrWhiteSpace(experience))
            {
                throw new ArgumentException(
                    "Candidate experience is required.",
                    nameof(experience));
            }

            if (requestedDailySalaryCents <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(requestedDailySalaryCents));
            }

            if (string.IsNullOrWhiteSpace(trait))
            {
                throw new ArgumentException(
                    "Candidate trait is required.",
                    nameof(trait));
            }

            if (availableFromDay < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(availableFromDay));
            }

            CandidateId = candidateId;
            DisplayName = displayName.Trim();
            Profile = profile;
            Seniority = seniority;
            Skills = skills;
            WorkSpeed = workSpeed.Trim();
            Experience = experience.Trim();
            RequestedDailySalaryCents =
                requestedDailySalaryCents;
            Trait = trait.Trim();
            AvailableFromDay = availableFromDay;
        }
    }

    public sealed class HiredEmployee
    {
        public EmployeeId EmployeeId { get; }
        public CandidateId SourceCandidateId { get; }
        public string DisplayName { get; }
        public EmployeeProfile Profile { get; }
        public EmployeeSeniority Seniority { get; }
        public EmployeeSkillSet Skills { get; }
        public long RequestedDailySalaryCents { get; }
        public long ContractedDailySalaryCents =>
            RequestedDailySalaryCents;
        public string Trait { get; }
        public int HiredDay { get; }
        public int StartDay { get; }

        public HiredEmployee(
            EmployeeId employeeId,
            EmployeeCandidate source,
            int hiredDay,
            int startDay)
        {
            if (!employeeId.IsInitialized)
            {
                throw new ArgumentException(
                    "Employee ID must be initialized.",
                    nameof(employeeId));
            }

            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (hiredDay < 1 || startDay <= hiredDay)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(startDay));
            }

            EmployeeId = employeeId;
            SourceCandidateId = source.CandidateId;
            DisplayName = source.DisplayName;
            Profile = source.Profile;
            Seniority = source.Seniority;
            Skills = source.Skills;
            RequestedDailySalaryCents =
                source.RequestedDailySalaryCents;
            Trait = source.Trait;
            HiredDay = hiredDay;
            StartDay = startDay;
        }

        public HiredEmployee(
            EmployeeId employeeId,
            CandidateId sourceCandidateId,
            string displayName,
            EmployeeProfile profile,
            EmployeeSeniority seniority,
            EmployeeSkillSet skills,
            long requestedDailySalaryCents,
            string trait,
            int hiredDay,
            int startDay)
        {
            if (!employeeId.IsInitialized)
            {
                throw new ArgumentException(
                    "Employee ID must be initialized.",
                    nameof(employeeId));
            }

            if (!sourceCandidateId.IsInitialized)
            {
                throw new ArgumentException(
                    "Source candidate ID must be initialized.",
                    nameof(sourceCandidateId));
            }

            if (string.IsNullOrWhiteSpace(displayName))
            {
                throw new ArgumentException(
                    "Employee name is required.",
                    nameof(displayName));
            }

            if (!Enum.IsDefined(typeof(EmployeeProfile), profile))
            {
                throw new ArgumentOutOfRangeException(nameof(profile));
            }

            if (!Enum.IsDefined(typeof(EmployeeSeniority), seniority))
            {
                throw new ArgumentOutOfRangeException(nameof(seniority));
            }

            if (requestedDailySalaryCents <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(requestedDailySalaryCents));
            }

            if (string.IsNullOrWhiteSpace(trait))
            {
                throw new ArgumentException(
                    "Employee trait is required.",
                    nameof(trait));
            }

            if (hiredDay < 1 || startDay <= hiredDay)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(startDay));
            }

            EmployeeId = employeeId;
            SourceCandidateId = sourceCandidateId;
            DisplayName = displayName.Trim();
            Profile = profile;
            Seniority = seniority;
            Skills = skills;
            RequestedDailySalaryCents = requestedDailySalaryCents;
            Trait = trait.Trim();
            HiredDay = hiredDay;
            StartDay = startDay;
        }
    }
}
