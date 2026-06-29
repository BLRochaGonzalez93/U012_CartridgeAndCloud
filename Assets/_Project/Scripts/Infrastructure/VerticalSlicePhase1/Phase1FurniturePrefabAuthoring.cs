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
            Phase1ContentCatalogAsset content =
                Resources.Load<
                    Phase1ContentCatalogAsset>(
                        "Sprint16Phase1/" +
                        "CC_S16_P1_ContentCatalog");

            Phase1MaterialPaletteAsset palette =
                Resources.Load<
                    Phase1MaterialPaletteAsset>(
                        "Sprint16Phase1/" +
                        "CC_S16_P1_MaterialPalette");

            if (content == null ||
                palette == null)
            {
                return;
            }

            Phase1Catalog catalog =
                content.BuildCatalog();

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
                    palette.Find(
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
