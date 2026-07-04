using UnityEngine;
using VRMGames.CartridgeAndCloud.Application.Store;
using VRMGames.CartridgeAndCloud.Domain.Products;
using VRMGames.CartridgeAndCloud.Infrastructure.Store;
using VRMGames.CartridgeAndCloud.Presentation.Products;
using VRMGames.CartridgeAndCloud.Runtime.Development.Blockout;
namespace VRMGames.CartridgeAndCloud.Runtime.Development.Authoring.Products
{
    public sealed class ProductPrefabAuthoring :
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
            ProductVisualMarker existing =
                GetComponentInChildren<
                    ProductVisualMarker>(true);

            if (existing != null)
            {
                existing.Configure(_productId);
                return;
            }

            if (transform.childCount > 0)
            {
                ProductVisualMarker marker =
                    gameObject.AddComponent<
                        ProductVisualMarker>();

                marker.Configure(_productId);
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

            if (!catalog.TryGetProduct(
                    _productId,
                    out RetailProductDefinition
                        definition))
            {
                return;
            }

            StoreBlockoutVisualFactory
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
            RetailProductKind kind)
        {
            switch (kind)
            {
                case RetailProductKind.Console:
                    return new Vector3(
                        0.42f,
                        0.16f,
                        0.32f);
                case RetailProductKind.Controller:
                    return new Vector3(
                        0.28f,
                        0.12f,
                        0.20f);
                case RetailProductKind.Headset:
                    return new Vector3(
                        0.20f,
                        0.12f,
                        0.20f);
                case RetailProductKind.Accessory:
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
