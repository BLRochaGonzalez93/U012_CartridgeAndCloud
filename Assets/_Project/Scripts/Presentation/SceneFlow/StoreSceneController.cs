using UnityEngine;
using UnityEngine.SceneManagement;
using VRMGames.CartridgeAndCloud.Application.SceneFlow;

namespace VRMGames.CartridgeAndCloud.Presentation.SceneFlow
{
    /// <summary>
    /// Thin controller for the Store scene.
    /// </summary>
    public sealed class StoreSceneController :
        MonoBehaviour,
        ISceneNavigationConsumer
    {
        private const string StandaloneBootstrapSceneName =
            "Bootstrap";

        private ISceneNavigator _sceneNavigator;

        public void Initialize(
            ISceneNavigator sceneNavigator)
        {
            _sceneNavigator =
                sceneNavigator;
        }

        public void ReturnToMainMenu()
        {
            if (_sceneNavigator == null)
            {
                Debug.LogWarning(
                    "[SceneFlow] StoreSceneController has no " +
                    "ISceneNavigator because Store was started directly. " +
                    "Loading Bootstrap as the standalone fallback.");

                SceneManager.LoadScene(
                    StandaloneBootstrapSceneName,
                    LoadSceneMode.Single);

                return;
            }

            SceneTransitionRequestResult result =
                _sceneNavigator.RequestLoad(
                    SceneId.MainMenu);

            if (result ==
                SceneTransitionRequestResult.UnknownScene)
            {
                Debug.LogError(
                    "[SceneFlow] MainMenu is not registered.");
            }
        }
    }
}
