using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VRMGames.CartridgeAndCloud.Domain.UIUX
{
    public enum UiLayerId
    {
        Hud = 0,
        ManagementPanel = 1,
        Submenu = 2,
        Tooltip = 3,
        Confirmation = 4,
        PauseMenu = 5
    }

    public enum ManagementPanelId
    {
        None = 0,
        Inventory = 1,
        Suppliers = 2,
        Displays = 3,
        Customers = 4,
        Shopping = 5,
        Checkout = 6,
        DayCycle = 7,
        Economy = 8,
        Help = 9,
        Accessibility = 10
    }

    public sealed class UiNavigationEntry
    {
        public UiLayerId Layer { get; }
        public string FocusTargetId { get; }

        public UiNavigationEntry(
            UiLayerId layer,
            string focusTargetId)
        {
            Layer = layer;

            if (string.IsNullOrWhiteSpace(
                    focusTargetId))
            {
                throw new ArgumentException(
                    "A focus target is required.",
                    nameof(focusTargetId));
            }

            FocusTargetId = focusTargetId;
        }
    }

    public sealed class UiNavigationState
    {
        private readonly List<UiNavigationEntry>
            _entries =
                new List<UiNavigationEntry>();

        public IReadOnlyList<UiNavigationEntry>
            Entries =>
                new ReadOnlyCollection<
                    UiNavigationEntry>(_entries);

        public bool HasOverlay => _entries.Count > 0;

        public UiNavigationEntry Top =>
            _entries.Count == 0
                ? null
                : _entries[_entries.Count - 1];

        public void Push(UiNavigationEntry entry)
        {
            if (entry == null)
            {
                throw new ArgumentNullException(
                    nameof(entry));
            }

            if (_entries.Count > 0 &&
                _entries[_entries.Count - 1].Layer ==
                    entry.Layer &&
                string.Equals(
                    _entries[_entries.Count - 1]
                        .FocusTargetId,
                    entry.FocusTargetId,
                    StringComparison.Ordinal))
            {
                return;
            }

            _entries.Add(entry);
        }

        public UiNavigationEntry Pop()
        {
            if (_entries.Count == 0)
            {
                return null;
            }

            int last = _entries.Count - 1;
            UiNavigationEntry result =
                _entries[last];
            _entries.RemoveAt(last);
            return result;
        }

        public void Clear()
        {
            _entries.Clear();
        }
    }
}
