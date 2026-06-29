using VRMGames.CartridgeAndCloud.Application.InputContexts;
using VRMGames.CartridgeAndCloud.Infrastructure.SceneFlow;

namespace VRMGames.CartridgeAndCloud.Infrastructure.UIUX
{
    public sealed class Sprint15InputMapGate
    {
        private IInputContextService _service;
        private InputContextId _previousContext =
            InputContextId.None;

        public bool IsUiExclusive { get; private set; }

        public void EnterUiExclusive()
        {
            if (IsUiExclusive)
            {
                return;
            }

            _service =
                ApplicationRoot.Instance != null
                    ? ApplicationRoot.Instance
                        .InputContextService
                    : null;

            _previousContext =
                _service != null
                    ? _service.CurrentContext
                    : InputContextId.None;

            _service?.SetContext(
                InputContextId.UI);

            IsUiExclusive = true;
        }

        public void ExitUiExclusive()
        {
            if (!IsUiExclusive)
            {
                return;
            }

            _service?.SetContext(
                _previousContext);

            _service = null;
            _previousContext =
                InputContextId.None;
            IsUiExclusive = false;
        }
    }
}
