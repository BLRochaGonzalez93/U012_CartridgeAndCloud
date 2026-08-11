using System;
using System.Collections.Generic;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Application.Employees;
using VRMGames.CartridgeAndCloud.Domain.Employees;

namespace VRMGames.CartridgeAndCloud.Infrastructure.Employees
{
    [CreateAssetMenu(
        menuName = "Cartridge & Cloud/Employees/Hiring Catalog",
        fileName = "EmployeeHiringCatalog")]
    public sealed class EmployeeHiringCatalogAsset : ScriptableObject
    {
        [Serializable]
        public sealed class RecruitmentChannelEntry
        {
            public string channelId;
            public string displayName;
            [Min(0)] public long postingCostCents;
            [Min(1)] public int candidateCount = 3;
            [Min(0)] public int minimumDelayDays = 1;
            [Min(0)] public int maximumDelayDays = 1;
            public EmployeeSeniority minimumSeniority =
                EmployeeSeniority.Junior;
            public EmployeeSeniority maximumSeniority =
                EmployeeSeniority.Qualified;
        }

        [Header("Unlock policy")]
        [SerializeField]
        [Min(1)]
        private int _minimumBusinessLevel = 2;

        [SerializeField]
        [Min(0)]
        private int _minimumReputation = 50;

        [SerializeField]
        [Min(1)]
        private int _minimumOperatingDays = 7;

        [SerializeField]
        [Min(1)]
        private int _candidateWindowDays = 5;

        [Header("Recruitment channels")]
        [SerializeField]
        private RecruitmentChannelEntry[] _channels =
            Array.Empty<RecruitmentChannelEntry>();

        [Header("Candidate presentation")]
        [SerializeField]
        private string[] _candidateNames = Array.Empty<string>();

        [SerializeField]
        private string[] _traits = Array.Empty<string>();

        public EmployeeHiringCatalog BuildCatalog()
        {
            RecruitmentChannelEntry[] source =
                _channels ?? Array.Empty<RecruitmentChannelEntry>();
            List<RecruitmentChannelDefinition> channels =
                new List<RecruitmentChannelDefinition>(source.Length);

            foreach (RecruitmentChannelEntry entry in source)
            {
                if (entry == null)
                {
                    throw new InvalidOperationException(
                        "Employee hiring catalog contains a missing channel.");
                }

                channels.Add(
                    new RecruitmentChannelDefinition(
                        entry.channelId,
                        entry.displayName,
                        entry.postingCostCents,
                        entry.candidateCount,
                        entry.minimumDelayDays,
                        entry.maximumDelayDays,
                        entry.minimumSeniority,
                        entry.maximumSeniority));
            }

            return new EmployeeHiringCatalog(
                _minimumBusinessLevel,
                _minimumReputation,
                _minimumOperatingDays,
                _candidateWindowDays,
                channels,
                _candidateNames ?? Array.Empty<string>(),
                _traits ?? Array.Empty<string>());
        }
    }
}
