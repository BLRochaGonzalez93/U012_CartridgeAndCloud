using VRMGames.CartridgeAndCloud.Domain.GameSession;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;

namespace VRMGames.CartridgeAndCloud.Application.GameSession
{
    public interface ISaveGameRepository
    {
        bool Exists(SaveSlotId slotId);
        void Save(GameSessionSnapshot snapshot);
        bool TryLoad(SaveSlotId slotId, out GameSessionSnapshot snapshot);
        bool Delete(SaveSlotId slotId);
    }
}
