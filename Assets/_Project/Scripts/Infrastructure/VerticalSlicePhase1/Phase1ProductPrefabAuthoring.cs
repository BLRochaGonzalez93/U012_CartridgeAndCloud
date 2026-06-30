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
            Phase1ProductVisualMarker existing =
                GetComponentInChildren<
                    Phase1ProductVisualMarker>(true);

            if (existing != null)
            {
                existing.Configure(_productId);
                return;
            }

            if (transform.childCount > 0)
            {
                Phase1ProductVisualMarker marker =
                    gameObject.AddComponent<
                        Phase1ProductVisualMarker>();

                marker.Configure(_productId);
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
                    registry.MaterialPalette.Find(
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
