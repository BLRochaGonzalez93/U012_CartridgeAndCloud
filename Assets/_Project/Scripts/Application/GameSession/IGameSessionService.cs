using DomainGameSession = VRMGames.CartridgeAndCloud.Domain.GameSession.GameSession;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;

namespace VRMGames.CartridgeAndCloud.Application.GameSession
{
    public interface IGameSessionService
    {
        bool HasActiveSession { get; }
        DomainGameSession Current { get; }

        void StartNew(SaveSlotId slotId);
        GameSessionOperationResult SaveCurrent();
        GameSessionOperationResult Load(SaveSlotId slotId);
        GameSessionOperationResult Delete(SaveSlotId slotId);
        bool SlotExists(SaveSlotId slotId);
    }
}
