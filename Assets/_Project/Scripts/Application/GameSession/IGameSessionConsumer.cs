namespace VRMGames.CartridgeAndCloud.Application.GameSession
{
    public interface IGameSessionConsumer
    {
        void Initialize(IGameSessionService gameSessionService);
    }
}
