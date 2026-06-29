using System;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;
using VRMGames.CartridgeAndCloud.Domain.Persistence;

namespace VRMGames.CartridgeAndCloud.Application.UIUX
{
    public sealed class ActiveGameSessionService
    {
        public bool HasActiveSession =>
            Snapshot != null;

        public SaveSlotId ActiveSlotId { get; private set; }

        public IntegratedGameStateSnapshot Snapshot {
            get;
            private set;
        }

        public event Action<
            IntegratedGameStateSnapshot> SnapshotChanged;

        public void Activate(
            SaveSlotId slotId,
            IntegratedGameStateSnapshot snapshot)
        {
            if (snapshot == null)
            {
                throw new ArgumentNullException(
                    nameof(snapshot));
            }

            if (snapshot.SlotId != slotId)
            {
                throw new ArgumentException(
                    "Snapshot slot mismatch.",
                    nameof(snapshot));
            }

            ActiveSlotId = slotId;
            Snapshot = snapshot;
            SnapshotChanged?.Invoke(Snapshot);
        }

        public void Replace(
            IntegratedGameStateSnapshot snapshot)
        {
            if (!HasActiveSession)
            {
                throw new InvalidOperationException(
                    "No active session exists.");
            }

            Activate(ActiveSlotId, snapshot);
        }

        public void Clear()
        {
            Snapshot = null;
        }
    }
}
