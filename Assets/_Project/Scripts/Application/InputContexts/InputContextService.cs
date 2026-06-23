using System;

namespace VRMGames.CartridgeAndCloud.Application.InputContexts
{
    public sealed class InputContextService : IInputContextService
    {
        public InputContextId CurrentContext { get; private set; } =
            InputContextId.None;

        public event Action<InputContextId> ContextChanged;

        public InputContextChangeResult SetContext(InputContextId context)
        {
            if (!Enum.IsDefined(typeof(InputContextId), context))
            {
                return InputContextChangeResult.InvalidContext;
            }

            if (CurrentContext == context)
            {
                return InputContextChangeResult.AlreadyActive;
            }

            CurrentContext = context;
            ContextChanged?.Invoke(context);

            return InputContextChangeResult.Changed;
        }
    }
}
