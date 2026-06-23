using System;

namespace VRMGames.CartridgeAndCloud.Application.GameSession
{
    public interface IUtcClock
    {
        DateTime UtcNow { get; }
    }
}
