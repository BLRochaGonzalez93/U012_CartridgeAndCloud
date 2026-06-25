using UnityEngine;
using UnityEngine.SceneManagement;
using VRMGames.CartridgeAndCloud.Infrastructure.SceneFlow;

namespace VRMGames.CartridgeAndCloud.Infrastructure.InputSystem.Actions
{
    public static class ProjectInputActionRuntimeBootstrap
    {
        [RuntimeInitializeOnLoadMethod(
            RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Initialize()
        {
            SceneManager.sceneLoaded -=
                HandleSceneLoaded;

            SceneManager.sceneLoaded +=
                HandleSceneLoaded;

            AttachToApplicationRoot();
        }

        private static void HandleSceneLoaded(
            Scene scene,
            LoadSceneMode loadSceneMode)
        {
            AttachToApplicationRoot();
        }

        private static void AttachToApplicationRoot()
        {
            ApplicationRoot root =
                ApplicationRoot.Instance;

            if (root == null ||
                root.InputContextService == null)
            {
                return;
            }

            InputActionContextRouter router =
                root.GetComponent<InputActionContextRouter>();

            if (router == null)
            {
                router =
                    root.gameObject
                        .AddComponent<InputActionContextRouter>();
            }

            router.Configure(
                allowStandaloneGameplay: false);

            router.Initialize(
                root.InputContextService);
        }
    }
}
