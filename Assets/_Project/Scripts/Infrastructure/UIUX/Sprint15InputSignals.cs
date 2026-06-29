using System;

namespace VRMGames.CartridgeAndCloud.Infrastructure.UIUX
{
    public static class Sprint15InputSignals
    {
        public static event Action CancelRequested;

        public static void RaiseCancelRequested()
        {
            CancelRequested?.Invoke();
        }
    }
}
