using UnityEngine;
using VRMGames.CartridgeAndCloud.Application.Store;
using VRMGames.CartridgeAndCloud.Domain.Store;
using VRMGames.CartridgeAndCloud.Infrastructure.Store;
using VRMGames.CartridgeAndCloud.Runtime.Development.Blockout;
namespace VRMGames.CartridgeAndCloud.Runtime.Development.Authoring.Store
{
    public sealed class StoreFixturePrefabAuthoring :
        MonoBehaviour
    {
        [SerializeField]
        private string _definitionId;

        [SerializeField, Min(0.01f)]
        private float _cellSize = 0.5f;

        public string DefinitionId =>
            _definitionId;

        private void Awake()
        {
            BuildBlockout();
        }

        [ContextMenu("Build Blockout")]
        public void BuildBlockout()
        {
            if (transform.childCount > 0)
            {
                return;
            }

            StoreRuntimeAssetRegistry registry =
                StoreRuntimeAssetRegistry
                    .FindLoaded();

            if (registry == null ||
                registry.ContentCatalog == null ||
                registry.MaterialPalette == null)
            {
                return;
            }

            StoreContentCatalog catalog =
                registry.ContentCatalog
                    .BuildCatalog();

            if (!catalog.TryGetFurniture(
                    _definitionId,
                    out StoreFixtureDefinition
                        definition))
            {
                return;
            }

            StoreBlockoutVisualFactory
                .BuildFurniture(
                    gameObject,
                    definition,
                    registry.MaterialPalette.Find(
                        definition
                            .MaterialVariantId),
                    _cellSize);
        }

        public void Configure(
            string definitionId,
            float cellSize)
        {
            _definitionId =
                definitionId ?? string.Empty;
            _cellSize =
                Mathf.Max(0.01f, cellSize);
        }
    }
}
