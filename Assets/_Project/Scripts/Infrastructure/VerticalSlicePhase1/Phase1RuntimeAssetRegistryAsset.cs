using UnityEngine;

namespace VRMGames.CartridgeAndCloud.Infrastructure.VerticalSlicePhase1
{
    [CreateAssetMenu(
        menuName =
            "Cartridge & Cloud/Runtime/Asset Registry",
        fileName =
            "RuntimeAssetRegistry")]
    public sealed class Phase1RuntimeAssetRegistryAsset :
        ScriptableObject
    {
        [SerializeField]
        private Phase1SettingsAsset _settings;

        [SerializeField]
        private Phase1ContentCatalogAsset
            _contentCatalog;

        [SerializeField]
        private Phase1StoreShellAsset _storeShell;

        [SerializeField]
        private Phase1MaterialPaletteAsset
            _materialPalette;

        [SerializeField]
        private Phase1PresentationCatalogAsset
            _presentationCatalog;

        [SerializeField]
        private Phase1AudioCatalogAsset
            _audioCatalog;

        [SerializeField]
        private RepresentativePrefabCatalogAsset
            _representativePrefabs;

        public Phase1SettingsAsset Settings =>
            _settings;

        public Phase1ContentCatalogAsset
            ContentCatalog =>
                _contentCatalog;

        public Phase1StoreShellAsset StoreShell =>
            _storeShell;

        public Phase1MaterialPaletteAsset
            MaterialPalette =>
                _materialPalette;

        public Phase1PresentationCatalogAsset
            PresentationCatalog =>
                _presentationCatalog;

        public Phase1AudioCatalogAsset AudioCatalog =>
            _audioCatalog;

        public RepresentativePrefabCatalogAsset
            RepresentativePrefabs =>
                _representativePrefabs;

        public void Configure(
            Phase1SettingsAsset settings,
            Phase1ContentCatalogAsset contentCatalog,
            Phase1StoreShellAsset storeShell,
            Phase1MaterialPaletteAsset materialPalette,
            Phase1PresentationCatalogAsset
                presentationCatalog,
            Phase1AudioCatalogAsset audioCatalog)
        {
            _settings = settings;
            _contentCatalog = contentCatalog;
            _storeShell = storeShell;
            _materialPalette = materialPalette;
            _presentationCatalog =
                presentationCatalog;
            _audioCatalog = audioCatalog;
        }

        public void ConfigureRepresentativePrefabs(
            RepresentativePrefabCatalogAsset catalog)
        {
            _representativePrefabs = catalog;
        }

        public static Phase1RuntimeAssetRegistryAsset
            FindLoaded()
        {
            Phase1RuntimeAssetRegistryAsset[]
                registries =
                    Resources.FindObjectsOfTypeAll<
                        Phase1RuntimeAssetRegistryAsset>();

            if (registries == null ||
                registries.Length == 0)
            {
                return null;
            }

            return registries[0];
        }
    }
}
