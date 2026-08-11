using UnityEngine;
using UnityEngine.Serialization;
using VRMGames.CartridgeAndCloud.Infrastructure.Audio;
using VRMGames.CartridgeAndCloud.Infrastructure.Employees;

namespace VRMGames.CartridgeAndCloud.Infrastructure.Store
{
    [CreateAssetMenu(
        menuName = "Cartridge & Cloud/Runtime/Asset Registry",
        fileName = "RuntimeAssetRegistry")]
    public sealed class StoreRuntimeAssetRegistry : ScriptableObject
    {
        [SerializeField]
        private StoreRuntimeSettingsAsset _settings;

        [SerializeField]
        private StoreContentCatalogAsset _contentCatalog;

        [SerializeField]
        [FormerlySerializedAs("_storeShell")]
        private StoreLayoutAsset _storeLayout;

        [SerializeField]
        private StoreMaterialPaletteAsset _materialPalette;

        [SerializeField]
        private StorePresentationCatalogAsset _presentationCatalog;

        [SerializeField]
        private AudioEventCatalogAsset _audioCatalog;

        [SerializeField]
        private EmployeeHiringCatalogAsset _employeeHiringCatalog;

        [SerializeField]
        [FormerlySerializedAs("_representativePrefabs")]
        private StoreVisualPrefabCatalogAsset _visualPrefabs;

        public StoreRuntimeSettingsAsset Settings => _settings;

        public StoreContentCatalogAsset ContentCatalog =>
            _contentCatalog;

        public StoreLayoutAsset StoreLayout => _storeLayout;

        // Compatibility alias retained for existing scenes, tools and tests.
        public StoreLayoutAsset StoreShell => _storeLayout;

        public StoreMaterialPaletteAsset MaterialPalette =>
            _materialPalette;

        public StorePresentationCatalogAsset PresentationCatalog =>
            _presentationCatalog;

        public AudioEventCatalogAsset AudioCatalog => _audioCatalog;

        public EmployeeHiringCatalogAsset EmployeeHiringCatalog =>
            _employeeHiringCatalog;

        public StoreVisualPrefabCatalogAsset VisualPrefabs =>
            _visualPrefabs;

        // Compatibility alias retained while representative naming is retired.
        public StoreVisualPrefabCatalogAsset RepresentativePrefabs =>
            _visualPrefabs;

        public void Configure(
            StoreRuntimeSettingsAsset settings,
            StoreContentCatalogAsset contentCatalog,
            StoreLayoutAsset storeLayout,
            StoreMaterialPaletteAsset materialPalette,
            StorePresentationCatalogAsset presentationCatalog,
            AudioEventCatalogAsset audioCatalog)
        {
            _settings = settings;
            _contentCatalog = contentCatalog;
            _storeLayout = storeLayout;
            _materialPalette = materialPalette;
            _presentationCatalog = presentationCatalog;
            _audioCatalog = audioCatalog;
        }

        public void ConfigureVisualPrefabs(
            StoreVisualPrefabCatalogAsset catalog)
        {
            _visualPrefabs = catalog;
        }

        public void ConfigureRepresentativePrefabs(
            StoreVisualPrefabCatalogAsset catalog)
        {
            ConfigureVisualPrefabs(catalog);
        }

        public static StoreRuntimeAssetRegistry FindLoaded()
        {
            return Resources.Load<StoreRuntimeAssetRegistry>(
                "RuntimeAssetRegistry");
        }
    }
}
