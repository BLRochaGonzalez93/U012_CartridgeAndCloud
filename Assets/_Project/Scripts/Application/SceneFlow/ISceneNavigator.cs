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

    /// <summary>
    /// Stable identifiers for the approved Sprint 1 scene list.
    /// Values intentionally match the validated build indexes.
    /// </summary>
    public enum SceneId
    {
        Bootstrap = 0,
        MainMenu = 1,
        Store = 2,
        TestLab = 3
    }

    /// <summary>
    /// Immediate result returned when requesting a scene transition.
    /// </summary>
    public enum SceneTransitionRequestResult
    {
        Accepted = 0,
        AlreadyActive = 1,
        TransitionInProgress = 2,
        UnknownScene = 3
    }

    /// <summary>
    /// Implemented by scene components that receive the navigator from the composition root.
    /// </summary>
    public interface ISceneNavigationConsumer
    {
        void Initialize(ISceneNavigator sceneNavigator);
    }
}
