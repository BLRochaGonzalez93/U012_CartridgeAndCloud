using System;
using VRMGames.CartridgeAndCloud.Domain.Persistence;

namespace VRMGames.CartridgeAndCloud.Application.Persistence
{
    public sealed class InMemoryRestoreTarget :
        IIntegratedGameStateRestoreTarget
    {
        private readonly Func<
            IntegratedGameStateSnapshot,
            bool> _validation;

        public IntegratedGameStateSnapshot Current {
            get;
            private set;
        }

        public int RestoreCount { get; private set; }

        public InMemoryRestoreTarget(
            Func<IntegratedGameStateSnapshot, bool>
                validation = null)
        {
            _validation = validation;
        }

        public bool CanRestore(
            IntegratedGameStateSnapshot snapshot,
            out string reason)
        {
            if (snapshot == null)
            {
                reason = "Snapshot is null.";
                return false;
            }

            if (_validation != null &&
                !_validation(snapshot))
            {
                reason = "Restore target rejected the snapshot.";
                return false;
            }

            reason = string.Empty;
            return true;
        }

        public void Restore(
            IntegratedGameStateSnapshot snapshot)
        {
            if (snapshot == null)
            {
                throw new ArgumentNullException(nameof(snapshot));
            }

            Current = snapshot;
            RestoreCount++;
        }
    }
}
