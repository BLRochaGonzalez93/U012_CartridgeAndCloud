using System;
using System.Collections.Generic;

namespace VRMGames.CartridgeAndCloud.Runtime.Characters
{
    /// <summary>
    /// Owns delivery presentation identity independently from scene timing.
    /// An order can have at most one active or completed visual presentation.
    /// </summary>
    public sealed class DeliveryPresentationRegistry
    {
        private readonly HashSet<string> _active =
            new HashSet<string>(StringComparer.Ordinal);
        private readonly HashSet<string> _completed =
            new HashSet<string>(StringComparer.Ordinal);

        public int ActiveCount => _active.Count;

        public bool TryBegin(string deliveryId)
        {
            string normalized = Normalize(deliveryId);
            return !_completed.Contains(normalized) && _active.Add(normalized);
        }

        public void Complete(string deliveryId)
        {
            string normalized = Normalize(deliveryId);
            _active.Remove(normalized);
            _completed.Add(normalized);
        }

        public void Cancel(string deliveryId)
        {
            _active.Remove(Normalize(deliveryId));
        }

        public bool HasSeen(string deliveryId)
        {
            string normalized = Normalize(deliveryId);
            return _active.Contains(normalized) || _completed.Contains(normalized);
        }

        private static string Normalize(string deliveryId)
        {
            if (string.IsNullOrWhiteSpace(deliveryId))
            {
                throw new ArgumentException(
                    "A delivery correlation ID is required.",
                    nameof(deliveryId));
            }

            return deliveryId.Trim();
        }
    }
}
