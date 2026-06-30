using UnityEngine;
using VRMGames.CartridgeAndCloud.Application.VerticalSlicePhase1;
using VRMGames.CartridgeAndCloud.Domain.VerticalSlicePhase1;

namespace VRMGames.CartridgeAndCloud.Infrastructure.VerticalSlicePhase1
{
    public sealed class Phase1FurniturePrefabAuthoring :
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

            Phase1RuntimeAssetRegistryAsset registry =
                Phase1RuntimeAssetRegistryAsset
                    .FindLoaded();

            if (registry == null ||
                registry.ContentCatalog == null ||
                registry.MaterialPalette == null)
            {
                return;
            }

            Phase1Catalog catalog =
                registry.ContentCatalog
                    .BuildCatalog();

            if (!catalog.TryGetFurniture(
                    _definitionId,
                    out Phase1FurnitureDefinition
                        definition))
            {
                return;
            }

            Phase1BlockoutVisualFactory
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
