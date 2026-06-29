using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.Economy;

namespace VRMGames.CartridgeAndCloud.Infrastructure.Economy
{
    [CreateAssetMenu(
        menuName = "Cartridge & Cloud/Economy/Settings",
        fileName = "CC_EconomySettings_")]
    public sealed class EconomySettingsAsset : ScriptableObject
    {
        [SerializeField]
        private string _currencyCode = "EUR";

        public string CurrencyCodeValue => _currencyCode;

        public CurrencyCode BuildCurrency()
        {
            return new CurrencyCode(_currencyCode);
        }

        public EconomyLedger BuildLedger()
        {
            return new EconomyLedger(BuildCurrency());
        }

        public void Configure(string currencyCode)
        {
            _currencyCode = currencyCode;
        }
    }
}
