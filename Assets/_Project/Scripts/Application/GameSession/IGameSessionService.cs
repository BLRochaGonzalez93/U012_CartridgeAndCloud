using DomainGameSession = VRMGames.CartridgeAndCloud.Domain.GameSession.GameSession;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;
using VRMGames.CartridgeAndCloud.Domain.GameSession;

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

    public enum GameSessionOperationResult
    {
        Success = 0,
        NoActiveSession = 1,
        SlotEmpty = 2,
        StorageFailure = 3
    }

    public interface IGameSessionConsumer
    {
        void Initialize(IGameSessionService gameSessionService);
    }
}
