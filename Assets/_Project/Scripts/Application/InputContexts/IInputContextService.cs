using System;

namespace VRMGames.CartridgeAndCloud.Application.InputContexts
{
    public interface IInputContextService
    {
        InputContextId CurrentContext { get; }

        event Action<InputContextId> ContextChanged;

        InputContextChangeResult SetContext(InputContextId context);
    }
}
