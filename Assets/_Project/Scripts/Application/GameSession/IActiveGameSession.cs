using System;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;
using VRMGames.CartridgeAndCloud.Domain.Persistence;

namespace VRMGames.CartridgeAndCloud.Application.GameSession
{
    public interface IActiveGameSession
    {
        bool HasActiveSession { get; }

        SaveSlotId ActiveSlotId { get; }

        IntegratedGameStateSnapshot Snapshot { get; }

        event Action<IntegratedGameStateSnapshot> SnapshotChanged;

        void Activate(
            SaveSlotId slotId,
            IntegratedGameStateSnapshot snapshot);

        void Replace(
            IntegratedGameStateSnapshot snapshot);

        void Clear();
    }
}
