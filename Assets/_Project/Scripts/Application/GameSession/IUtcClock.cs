using System;

using VRMGames.CartridgeAndCloud.Domain.GameSession;
namespace VRMGames.CartridgeAndCloud.Application.GameSession
{
    public interface IUtcClock
    {
        DateTime UtcNow { get; }
    }
}
