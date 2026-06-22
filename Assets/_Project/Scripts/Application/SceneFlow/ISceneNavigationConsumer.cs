namespace VRMGames.CartridgeAndCloud.Application.SceneFlow
{
    /// <summary>
    /// Implemented by scene components that receive the navigator from the composition root.
    /// </summary>
    public interface ISceneNavigationConsumer
    {
        void Initialize(ISceneNavigator sceneNavigator);
    }
}
