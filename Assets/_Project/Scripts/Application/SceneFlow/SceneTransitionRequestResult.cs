namespace VRMGames.CartridgeAndCloud.Application.SceneFlow
{
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
}
