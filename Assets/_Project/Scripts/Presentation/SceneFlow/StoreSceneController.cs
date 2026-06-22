using VRMGames.CartridgeAndCloud.Application.SceneFlow;
using UnityEngine;

namespace VRMGames.CartridgeAndCloud.Presentation.SceneFlow
{
    /// <summary>
    /// Thin controller for the Sprint 1 Store prototype shell.
    /// </summary>
    public sealed class StoreSceneController : MonoBehaviour, ISceneNavigationConsumer
    {
        private ISceneNavigator _sceneNavigator;

        public void Initialize(ISceneNavigator sceneNavigator)
        {
            _sceneNavigator = sceneNavigator;
        }

        public void ReturnToMainMenu()
        {
            if (_sceneNavigator == null)
            {
                Debug.LogError("[SceneFlow] StoreSceneController has not received an ISceneNavigator.");
                return;
            }

            SceneTransitionRequestResult result =
                _sceneNavigator.RequestLoad(SceneId.MainMenu);

            if (result == SceneTransitionRequestResult.UnknownScene)
            {
                Debug.LogError("[SceneFlow] MainMenu is not registered.");
            }
        }
    }
}
