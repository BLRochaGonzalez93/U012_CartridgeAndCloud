using UnityEngine;

namespace VRMGames.CartridgeAndCloud.Infrastructure.Store
{
    [CreateAssetMenu(
        menuName =
            "Cartridge & Cloud/Sprint 16 Phase 1/Settings",
        fileName =
            "CC_S16_P1_Settings")]
    public sealed class StoreRuntimeSettingsAsset :
        ScriptableObject
    {
        [SerializeField]
        private string _storeSceneName =
            "StoreInitial";

        [SerializeField]
        private bool _hideOccludingWalls =
            true;

        [SerializeField]
        private bool _showProcedurePanel =
            true;

        [SerializeField, Min(1)]
        private int _vfxPoolSize = 24;

        [SerializeField, Min(1)]
        private int _maximumCustomers = 4;

        public string StoreSceneName =>
            _storeSceneName;

        public bool HideOccludingWalls =>
            _hideOccludingWalls;

        public bool ShowProcedurePanel =>
            _showProcedurePanel;

        public int VfxPoolSize =>
            _vfxPoolSize;

        public int MaximumCustomers =>
            _maximumCustomers;
    }
}
