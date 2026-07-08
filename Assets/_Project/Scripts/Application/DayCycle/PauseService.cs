using System;
using System.Collections.Generic;

namespace VRMGames.CartridgeAndCloud.Application.DayCycle
{
    public interface IPauseService
    {
        bool IsPaused { get; }
        int ActiveRequestCount { get; }

        event Action<bool> Changed;

        bool RequestPause(string ownerId);
        bool ReleasePause(string ownerId);
        void Clear();
    }

    public sealed class PauseService : IPauseService
    {
        private readonly HashSet<string>
            _owners = new HashSet<string>(
                StringComparer.Ordinal);

        public bool IsPaused =>
            _owners.Count > 0;

        public int ActiveRequestCount =>
            _owners.Count;

        public event Action<bool> Changed;

        public bool RequestPause(string ownerId)
        {
            string normalized = RequireOwner(ownerId);
            bool wasPaused = IsPaused;
            bool added = _owners.Add(normalized);

            if (added && wasPaused != IsPaused)
            {
                Changed?.Invoke(IsPaused);
            }

            return added;
        }

        public bool ReleasePause(string ownerId)
        {
            string normalized = RequireOwner(ownerId);
            bool wasPaused = IsPaused;
            bool removed = _owners.Remove(normalized);

            if (removed && wasPaused != IsPaused)
            {
                Changed?.Invoke(IsPaused);
            }

            return removed;
        }

        public void Clear()
        {
            if (_owners.Count == 0)
            {
                return;
            }

            _owners.Clear();
            Changed?.Invoke(false);
        }

        private static string RequireOwner(
            string ownerId)
        {
            if (string.IsNullOrWhiteSpace(ownerId))
            {
                throw new ArgumentException(
                    "Pause owner ID cannot be empty.",
                    nameof(ownerId));
            }

            return ownerId.Trim();
        }
    }
}
