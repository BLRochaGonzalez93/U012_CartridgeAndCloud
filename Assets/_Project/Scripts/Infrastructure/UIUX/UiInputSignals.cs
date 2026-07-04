using System;

namespace VRMGames.CartridgeAndCloud.Infrastructure.UIUX
{
    public static class UiInputSignals
    {
        public static event Action CancelRequested;

        public static void RaiseCancelRequested()
        {
            CancelRequested?.Invoke();
        }
    }
}
