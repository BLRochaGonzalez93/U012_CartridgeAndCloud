using VRMGames.CartridgeAndCloud.Application.SceneFlow;
using UnityEngine;

namespace VRMGames.CartridgeAndCloud.Presentation.SceneFlow
{
    /// <summary>
    /// Thin controller for the Sprint 1 technical main menu.
    /// </summary>
    public sealed class MainMenuController : MonoBehaviour, ISceneNavigationConsumer
    {
        private ISceneNavigator _sceneNavigator;

        public void Initialize(ISceneNavigator sceneNavigator)
        {
            _sceneNavigator = sceneNavigator;
        }

        public void EnterStore()
        {
            RequestScene(SceneId.Store);
        }

        public void Quit()
        {
            if (_sceneNavigator == null)
            {
                Debug.LogError("[SceneFlow] MainMenuController has not received an ISceneNavigator.");
                return;
            }

            _sceneNavigator.Quit();
        }

        private void RequestScene(SceneId sceneId)
        {
            if (_sceneNavigator == null)
            {
                Debug.LogError("[SceneFlow] MainMenuController has not received an ISceneNavigator.");
                return;
            }

            SceneTransitionRequestResult result = _sceneNavigator.RequestLoad(sceneId);

            if (result == SceneTransitionRequestResult.UnknownScene)
            {
                Debug.LogError($"[SceneFlow] Scene '{sceneId}' is not registered.");
            }
        }
    }
}
