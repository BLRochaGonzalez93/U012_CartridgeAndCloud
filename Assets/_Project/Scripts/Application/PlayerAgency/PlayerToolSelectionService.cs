using System;
using VRMGames.CartridgeAndCloud.Domain.PlayerAgency;

namespace VRMGames.CartridgeAndCloud.Application.PlayerAgency
{
    /// <summary>
    /// Session-scoped authority for the player's currently selected tool.
    /// Tool selection is intentionally not persistent progress.
    /// </summary>
    public sealed class PlayerToolSelectionService
    {
        public PlayerToolId SelectedTool { get; private set; } =
            PlayerToolId.Hands;

        public event Action<PlayerToolId> SelectionChanged;

        public bool Select(PlayerToolId tool)
        {
            if (!Enum.IsDefined(typeof(PlayerToolId), tool) ||
                SelectedTool == tool)
            {
                return false;
            }

            SelectedTool = tool;
            SelectionChanged?.Invoke(tool);
            return true;
        }

        public void Reset()
        {
            Select(PlayerToolId.Hands);
        }
    }
}
