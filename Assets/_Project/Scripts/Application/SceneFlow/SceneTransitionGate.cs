namespace VRMGames.CartridgeAndCloud.Application.SceneFlow
{
    /// <summary>
    /// Small deterministic gate that prevents concurrent scene transitions.
    /// </summary>
    public sealed class SceneTransitionGate
    {
        public bool IsEntered { get; private set; }

        public bool TryEnter()
        {
            if (IsEntered)
            {
                return false;
            }

            IsEntered = true;
            return true;
        }

        public void Exit()
        {
            IsEntered = false;
        }
    }
}
