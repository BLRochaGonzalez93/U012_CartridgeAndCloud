namespace VRMGames.CartridgeAndCloud.Domain.PlayerAgency
{
    /// <summary>
    /// Stable player tool slots exposed by the radial selector. Tool behaviour
    /// is implemented by the phases that own each mechanic; selection alone
    /// never mutates store state.
    /// </summary>
    public enum PlayerToolId
    {
        Hands = 0,
        Scanner = 1,
        Tablet = 2,
        Broom = 3
    }
}
