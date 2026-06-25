using UnityEngine;
using VRMGames.CartridgeAndCloud.Application.InputContexts;

namespace VRMGames.CartridgeAndCloud.Infrastructure.InputSystem.Actions
{
    public sealed class InputActionContextRouter :
        MonoBehaviour,
        IInputContextConsumer
    {
        [SerializeField]
        private bool _allowStandaloneGameplay;

        private ProjectInputActions _actions;
        private IInputContextService _inputContextService;

        public ProjectInputActions Actions
        {
            get
            {
                EnsureActions();
                return _actions;
            }
        }

        public InputContextId EffectiveContext { get; private set; } =
            InputContextId.None;

        public bool IsUiMapEnabled =>
            _actions != null &&
            _actions.UI.enabled;

        public bool IsGameplayMapEnabled =>
            _actions != null &&
            _actions.Gameplay.enabled;

        private void Awake()
        {
            EnsureActions();
        }

        private void OnEnable()
        {
            EnsureActions();
            ApplyResolvedContext();
        }

        private void OnDisable()
        {
            _actions?.DisableAll();
        }

        private void OnDestroy()
        {
            UnsubscribeFromContextService();
            _actions?.Dispose();
            _actions = null;
        }

        public void Configure(
            bool allowStandaloneGameplay)
        {
            _allowStandaloneGameplay =
                allowStandaloneGameplay;

            if (isActiveAndEnabled)
            {
                ApplyResolvedContext();
            }
        }

        public void Initialize(
            IInputContextService inputContextService)
        {
            if (ReferenceEquals(
                    _inputContextService,
                    inputContextService))
            {
                ApplyResolvedContext();
                return;
            }

            UnsubscribeFromContextService();

            _inputContextService =
                inputContextService;

            if (_inputContextService != null)
            {
                _inputContextService.ContextChanged +=
                    HandleContextChanged;
            }

            ApplyResolvedContext();
        }

        public void ApplyContext(InputContextId context)
        {
            EnsureActions();
            _actions.DisableAll();

            switch (context)
            {
                case InputContextId.UI:
                    _actions.UI.Enable();
                    EffectiveContext = InputContextId.UI;
                    break;

                case InputContextId.Gameplay:
                    _actions.Gameplay.Enable();
                    EffectiveContext =
                        InputContextId.Gameplay;
                    break;

                default:
                    EffectiveContext =
                        InputContextId.None;
                    break;
            }
        }

        private void ApplyResolvedContext()
        {
            InputContextId context =
                _inputContextService != null
                    ? _inputContextService.CurrentContext
                    : _allowStandaloneGameplay
                        ? InputContextId.Gameplay
                        : InputContextId.None;

            ApplyContext(context);
        }

        private void HandleContextChanged(
            InputContextId context)
        {
            ApplyContext(context);
        }

        private void UnsubscribeFromContextService()
        {
            if (_inputContextService != null)
            {
                _inputContextService.ContextChanged -=
                    HandleContextChanged;
            }

            _inputContextService = null;
        }

        private void EnsureActions()
        {
            if (_actions == null)
            {
                _actions =
                    new ProjectInputActions();
            }
        }
    }
}
