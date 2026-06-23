using System;
using VRMGames.CartridgeAndCloud.Application.GameSession;

namespace VRMGames.CartridgeAndCloud.Infrastructure.GameSession
{
    public sealed class SystemUtcClock : IUtcClock
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
