using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.Shopping;

namespace VRMGames.CartridgeAndCloud.Infrastructure.Shopping
{
    [CreateAssetMenu(
        menuName = "Cartridge & Cloud/Shopping/Settings",
        fileName = "CC_ShoppingSettings_")]
    public sealed class ShoppingSettingsAsset : ScriptableObject
    {
        [SerializeField, Min(1)]
        private int _maxCartUnits = 3;

        [SerializeField, Min(1)]
        private int _maxUnitsPerReservation = 1;

        [SerializeField]
        private bool _allowFallbackCategories = true;

        public int MaxCartUnits => _maxCartUnits;
        public int MaxUnitsPerReservation => _maxUnitsPerReservation;
        public bool AllowFallbackCategories => _allowFallbackCategories;

        public ShoppingPolicy BuildPolicy()
        {
            return new ShoppingPolicy(
                _maxCartUnits,
                _maxUnitsPerReservation,
                _allowFallbackCategories);
        }

        public void Configure(
            int maxCartUnits,
            int maxUnitsPerReservation,
            bool allowFallbackCategories)
        {
            _maxCartUnits = maxCartUnits;
            _maxUnitsPerReservation = maxUnitsPerReservation;
            _allowFallbackCategories = allowFallbackCategories;
        }
    }
}
