using UnityEngine;

namespace VRMGames.CartridgeAndCloud.Infrastructure.UIUX
{
    [CreateAssetMenu(
        menuName =
            "Cartridge & Cloud/UI UX/Sprint 15 Settings",
        fileName = "CC_Sprint15Settings")]
    public sealed class Sprint15SettingsAsset :
        ScriptableObject
    {
        [SerializeField]
        private string _mainMenuSceneName =
            "MainMenu";

        [SerializeField]
        private string _storeSceneName =
            "Store";

        [SerializeField]
        private string _currencyCode =
            "EUR";

        [SerializeField]
        private long _initialCashCents =
            100000;

        [SerializeField, Min(1)]
        private int _dayDurationSeconds =
            300;

        [SerializeField]
        private bool _showTutorialOnNewGame =
            true;

        public string MainMenuSceneName =>
            _mainMenuSceneName;

        public string StoreSceneName =>
            _storeSceneName;

        public string CurrencyCode =>
            _currencyCode;

        public long InitialCashCents =>
            _initialCashCents;

        public int DayDurationSeconds =>
            _dayDurationSeconds;

        public bool ShowTutorialOnNewGame =>
            _showTutorialOnNewGame;

        public void Configure(
            string mainMenuSceneName,
            string storeSceneName,
            string currencyCode,
            long initialCashCents,
            int dayDurationSeconds,
            bool showTutorialOnNewGame)
        {
            _mainMenuSceneName =
                mainMenuSceneName;
            _storeSceneName =
                storeSceneName;
            _currencyCode =
                currencyCode;
            _initialCashCents =
                initialCashCents;
            _dayDurationSeconds =
                dayDurationSeconds;
            _showTutorialOnNewGame =
                showTutorialOnNewGame;
        }
    }
}
