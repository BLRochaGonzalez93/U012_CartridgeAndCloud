using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VRMGames.CartridgeAndCloud.Domain.Employees;

namespace VRMGames.CartridgeAndCloud.Application.Employees
{
    public sealed class EmployeeHiringCatalog
    {
        private readonly ReadOnlyCollection<
            RecruitmentChannelDefinition> _channels;
        private readonly Dictionary<
            string,
            RecruitmentChannelDefinition> _byId;
        private readonly ReadOnlyCollection<string> _candidateNames;
        private readonly ReadOnlyCollection<string> _traits;

        public int MinimumBusinessLevel { get; }
        public int MinimumReputation { get; }
        public int MinimumOperatingDays { get; }
        public int CandidateWindowDays { get; }

        public IReadOnlyList<RecruitmentChannelDefinition>
            Channels => _channels;

        public IReadOnlyList<string> CandidateNames =>
            _candidateNames;

        public IReadOnlyList<string> Traits => _traits;

        public EmployeeHiringCatalog(
            int minimumBusinessLevel,
            int minimumReputation,
            int minimumOperatingDays,
            int candidateWindowDays,
            IEnumerable<RecruitmentChannelDefinition> channels,
            IEnumerable<string> candidateNames,
            IEnumerable<string> traits)
        {
            if (minimumBusinessLevel < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(minimumBusinessLevel));
            }

            if (minimumReputation < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(minimumReputation));
            }

            if (minimumOperatingDays < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(minimumOperatingDays));
            }

            if (candidateWindowDays < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(candidateWindowDays));
            }

            if (channels == null)
            {
                throw new ArgumentNullException(nameof(channels));
            }

            if (candidateNames == null)
            {
                throw new ArgumentNullException(nameof(candidateNames));
            }

            if (traits == null)
            {
                throw new ArgumentNullException(nameof(traits));
            }

            List<RecruitmentChannelDefinition> channelCopy =
                new List<RecruitmentChannelDefinition>();
            _byId = new Dictionary<
                string,
                RecruitmentChannelDefinition>(
                    StringComparer.Ordinal);

            foreach (RecruitmentChannelDefinition channel in channels)
            {
                if (channel == null)
                {
                    throw new ArgumentException(
                        "Recruitment channels cannot contain null.",
                        nameof(channels));
                }

                if (_byId.ContainsKey(channel.ChannelId))
                {
                    throw new ArgumentException(
                        $"Duplicate recruitment channel '{channel.ChannelId}'.",
                        nameof(channels));
                }

                _byId.Add(channel.ChannelId, channel);
                channelCopy.Add(channel);
            }

            if (channelCopy.Count == 0)
            {
                throw new ArgumentException(
                    "At least one recruitment channel is required.",
                    nameof(channels));
            }

            List<string> nameCopy = CopyText(
                candidateNames,
                nameof(candidateNames));
            List<string> traitCopy = CopyText(
                traits,
                nameof(traits));

            MinimumBusinessLevel = minimumBusinessLevel;
            MinimumReputation = minimumReputation;
            MinimumOperatingDays = minimumOperatingDays;
            CandidateWindowDays = candidateWindowDays;
            _channels = new ReadOnlyCollection<RecruitmentChannelDefinition>(
                channelCopy);
            _candidateNames = new ReadOnlyCollection<string>(nameCopy);
            _traits = new ReadOnlyCollection<string>(traitCopy);
        }

        public bool TryGetChannel(
            string channelId,
            out RecruitmentChannelDefinition channel)
        {
            return _byId.TryGetValue(
                channelId ?? string.Empty,
                out channel);
        }

        private static List<string> CopyText(
            IEnumerable<string> source,
            string parameterName)
        {
            List<string> copy = new List<string>();
            HashSet<string> unique = new HashSet<string>(
                StringComparer.Ordinal);

            foreach (string value in source)
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(
                        "Catalog text values cannot be empty.",
                        parameterName);
                }

                string normalized = value.Trim();
                if (!unique.Add(normalized))
                {
                    throw new ArgumentException(
                        $"Duplicate catalog value '{normalized}'.",
                        parameterName);
                }

                copy.Add(normalized);
            }

            if (copy.Count == 0)
            {
                throw new ArgumentException(
                    "At least one catalog value is required.",
                    parameterName);
            }

            return copy;
        }
    }
}
