using System;
using VRMGames.CartridgeAndCloud.Application.VerticalSlicePhase1;

namespace VRMGames.CartridgeAndCloud.Infrastructure.VerticalSlicePhase1
{
    public sealed class SystemPhase1UtcClock :
        IPhase1UtcClock
    {
        public DateTime UtcNow =>
            DateTime.UtcNow;
    }
}
