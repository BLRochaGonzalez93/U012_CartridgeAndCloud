using UnityEngine;
using UnityEngine.InputSystem;
using VRMGames.CartridgeAndCloud.Infrastructure.InputSystem.Actions;
using VRMGames.CartridgeAndCloud.Infrastructure.UIUX;

namespace VRMGames.CartridgeAndCloud.Infrastructure.InputSystem.UIUX
{
    [DefaultExecutionOrder(-9999)]
    public sealed class UiInputSystemBridge :
        MonoBehaviour
    {
        private static UiInputSystemBridge
            _instance;

        private ProjectInputActions _actions;

        [RuntimeInitializeOnLoadMethod(
            RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Install()
        {
            if (_instance != null)
            {
                return;
            }

            GameObject gameObject =
                new GameObject(
                    "UiInputSystemBridge");
            gameObject.AddComponent<
                UiInputSystemBridge>();
        }

        private void Awake()
        {
            if (_instance != null &&
                _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
            EnsureActions();
        }

        private void OnEnable()
        {
            EnsureActions();
            _actions.UiCancel.performed +=
                HandleCancel;
            _actions.UI.Enable();
        }

        private void OnDisable()
        {
            if (_actions == null)
            {
                return;
            }

            _actions.UiCancel.performed -=
                HandleCancel;
            _actions.UI.Disable();
        }

        private void OnDestroy()
        {
            if (_instance != this)
            {
                return;
            }

            _actions?.Dispose();
            _actions = null;
            _instance = null;
        }

        private void HandleCancel(
            InputAction.CallbackContext context)
        {
            UiInputSignals
                .RaiseCancelRequested();
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
