using UnityEngine;

namespace VRMGames.CartridgeAndCloud.Infrastructure.VerticalSlicePhase1
{
    [CreateAssetMenu(
        menuName =
            "Cartridge & Cloud/Sprint 16 Phase 1/Settings",
        fileName =
            "CC_S16_P1_Settings")]
    public sealed class Phase1SettingsAsset :
        ScriptableObject
    {
        [SerializeField]
        private string _storeSceneName =
            "Store";

        [SerializeField]
        private bool _buildBlockoutOnLoad =
            true;

        [SerializeField]
        private bool _hideOccludingWalls =
            true;

        [SerializeField]
        private bool _showProcedurePanel =
            true;

        [SerializeField, Min(1)]
        private int _vfxPoolSize = 24;

        [SerializeField, Min(1)]
        private int _maximumBlockoutCustomers = 4;

        public string StoreSceneName =>
            _storeSceneName;

        public bool BuildBlockoutOnLoad =>
            _buildBlockoutOnLoad;

        public bool HideOccludingWalls =>
            _hideOccludingWalls;

        public bool ShowProcedurePanel =>
            _showProcedurePanel;

        public int VfxPoolSize =>
            _vfxPoolSize;

        public int MaximumBlockoutCustomers =>
            _maximumBlockoutCustomers;
    }
}
