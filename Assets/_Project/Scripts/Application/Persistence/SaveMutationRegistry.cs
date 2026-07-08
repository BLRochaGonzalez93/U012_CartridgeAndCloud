using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VRMGames.CartridgeAndCloud.Application.Persistence
{
    public interface ISaveMutationRegistry
    {
        int ActiveMutationCount { get; }

        bool HasPendingMutations { get; }

        IReadOnlyList<string> ActiveMutationIds {
            get;
        }

        IDisposable Begin(string mutationId);

        string DescribePendingMutations();
    }

    public sealed class SaveMutationRegistry :
        ISaveMutationRegistry
    {
        private readonly Dictionary<string, int>
            _active = new Dictionary<string, int>(
                StringComparer.Ordinal);

        public int ActiveMutationCount {
            get;
            private set;
        }

        public bool HasPendingMutations =>
            ActiveMutationCount > 0;

        public IReadOnlyList<string> ActiveMutationIds
        {
            get
            {
                List<string> ids =
                    new List<string>(_active.Keys);
                ids.Sort(StringComparer.Ordinal);
                return new ReadOnlyCollection<string>(ids);
            }
        }

        public event Action Changed;

        public IDisposable Begin(string mutationId)
        {
            string normalized =
                RequireMutationId(mutationId);

            if (_active.TryGetValue(
                    normalized,
                    out int count))
            {
                _active[normalized] = checked(count + 1);
            }
            else
            {
                _active.Add(normalized, 1);
            }

            ActiveMutationCount = checked(
                ActiveMutationCount + 1);
            Changed?.Invoke();

            return new Scope(this, normalized);
        }

        public string DescribePendingMutations()
        {
            if (!HasPendingMutations)
            {
                return string.Empty;
            }

            return "Pending operation: " +
                   string.Join(
                       ", ",
                       ActiveMutationIds) +
                   ".";
        }

        private void End(string mutationId)
        {
            if (!_active.TryGetValue(
                    mutationId,
                    out int count))
            {
                return;
            }

            if (count <= 1)
            {
                _active.Remove(mutationId);
            }
            else
            {
                _active[mutationId] = count - 1;
            }

            ActiveMutationCount = Math.Max(
                0,
                ActiveMutationCount - 1);
            Changed?.Invoke();
        }

        private static string RequireMutationId(
            string mutationId)
        {
            if (string.IsNullOrWhiteSpace(mutationId))
            {
                throw new ArgumentException(
                    "A mutation ID is required.",
                    nameof(mutationId));
            }

            return mutationId.Trim();
        }

        private sealed class Scope : IDisposable
        {
            private SaveMutationRegistry _owner;
            private readonly string _mutationId;

            public Scope(
                SaveMutationRegistry owner,
                string mutationId)
            {
                _owner = owner;
                _mutationId = mutationId;
            }

            public void Dispose()
            {
                SaveMutationRegistry owner = _owner;
                if (owner == null)
                {
                    return;
                }

                _owner = null;
                owner.End(_mutationId);
            }
        }
    }
}
