using System;

namespace VRMGames.CartridgeAndCloud.Application.PlayerMovement
{
    public readonly struct PlayerSprintDecision
    {
        public bool Requested { get; }
        public bool Allowed { get; }
        public float SpeedMultiplier { get; }

        public PlayerSprintDecision(
            bool requested,
            bool allowed,
            float speedMultiplier)
        {
            Requested = requested;
            Allowed = allowed;
            SpeedMultiplier = speedMultiplier;
        }
    }

    /// <summary>
    /// Pure policy for resolving sprint without coupling input, Unity movement
    /// or physical load storage. Carrying any load blocks sprint.
    /// </summary>
    public static class PlayerSprintPolicy
    {
        public static PlayerSprintDecision Evaluate(
            bool sprintRequested,
            bool sprintBlockedByLoad,
            float sprintMultiplier)
        {
            if (float.IsNaN(sprintMultiplier) ||
                float.IsInfinity(sprintMultiplier) ||
                sprintMultiplier < 1f)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(sprintMultiplier));
            }

            bool allowed =
                sprintRequested && !sprintBlockedByLoad;

            return new PlayerSprintDecision(
                sprintRequested,
                allowed,
                allowed ? sprintMultiplier : 1f);
        }
    }
}
