using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.DayCycle;

namespace VRMGames.CartridgeAndCloud.Infrastructure.DayCycle
{
    [CreateAssetMenu(
        menuName = "Cartridge & Cloud/Day Cycle/Settings",
        fileName = "CC_StoreDaySettings_")]
    public sealed class StoreDaySettingsAsset : ScriptableObject
    {
        [SerializeField]
        private string _dayId = "technical-day-001";

        [SerializeField, Min(1)]
        private int _openDurationSeconds = 300;

        [SerializeField]
        private bool _autoOpenOnInitialize = true;

        [SerializeField]
        private bool _autoBeginClosing = true;

        public string DayId => _dayId;

        public int OpenDurationSeconds =>
            _openDurationSeconds;

        public bool AutoOpenOnInitialize =>
            _autoOpenOnInitialize;

        public bool AutoBeginClosing =>
            _autoBeginClosing;

        public StoreDay BuildDay()
        {
            return new StoreDay(
                new StoreDayId(_dayId),
                new StoreDayPolicy(
                    _openDurationSeconds,
                    _autoBeginClosing));
        }

        public void Configure(
            string dayId,
            int openDurationSeconds,
            bool autoOpenOnInitialize,
            bool autoBeginClosing)
        {
            _dayId = dayId;
            _openDurationSeconds =
                openDurationSeconds;
            _autoOpenOnInitialize =
                autoOpenOnInitialize;
            _autoBeginClosing =
                autoBeginClosing;
        }
    }
}
