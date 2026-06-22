namespace VRMGames.CartridgeAndCloud.Application.SceneFlow
{
    /// <summary>
    /// Application-facing contract for scene navigation and application exit.
    /// </summary>
    public interface ISceneNavigator
    {
        SceneId ActiveScene { get; }

        bool IsTransitioning { get; }

        SceneTransitionRequestResult RequestLoad(SceneId sceneId);

        void Quit();
    }
}
