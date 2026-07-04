using System;

namespace VRMGames.CartridgeAndCloud.Application.InputContexts
{
    public interface IInputContextService
    {
        InputContextId CurrentContext { get; }

        event Action<InputContextId> ContextChanged;

        InputContextChangeResult SetContext(InputContextId context);
    }

    public enum InputContextId
    {
        None = 0,
        UI = 1,
        Gameplay = 2
    }

    public enum InputContextChangeResult
    {
        Changed = 0,
        AlreadyActive = 1,
        InvalidContext = 2
    }

    public interface IInputContextConsumer
    {
        void Initialize(IInputContextService inputContextService);
    }
}
