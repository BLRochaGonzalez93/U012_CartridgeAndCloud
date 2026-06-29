using UnityEngine;
using VRMGames.CartridgeAndCloud.Application.VerticalSlicePhase1;
using VRMGames.CartridgeAndCloud.Domain.VerticalSlicePhase1;

namespace VRMGames.CartridgeAndCloud.Infrastructure.VerticalSlicePhase1
{
    public sealed class Phase1ProductPrefabAuthoring :
        MonoBehaviour
    {
        [SerializeField]
        private string _productId;

        public string ProductId =>
            _productId;

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

            if (!catalog.TryGetProduct(
                    _productId,
                    out Phase1ProductDefinition
                        definition))
            {
                return;
            }

            Phase1BlockoutVisualFactory
                .BuildProduct(
                    transform,
                    definition,
                    palette.Find(
                        definition
                            .MaterialVariantId),
                    Vector3.zero,
                    ProductScale(
                        definition.Kind));
        }

        public void Configure(string productId)
        {
            _productId =
                productId ?? string.Empty;
        }

        private static Vector3 ProductScale(
            Phase1ProductKind kind)
        {
            switch (kind)
            {
                case Phase1ProductKind.Console:
                    return new Vector3(
                        0.42f,
                        0.16f,
                        0.32f);
                case Phase1ProductKind.Controller:
                    return new Vector3(
                        0.28f,
                        0.12f,
                        0.20f);
                case Phase1ProductKind.Headset:
                    return new Vector3(
                        0.20f,
                        0.12f,
                        0.20f);
                case Phase1ProductKind.Accessory:
                    return new Vector3(
                        0.18f,
                        0.22f,
                        0.10f);
                default:
                    return new Vector3(
                        0.22f,
                        0.30f,
                        0.05f);
            }
        }
    }
}
