using UnityEngine;

namespace VRMGames.CartridgeAndCloud.Presentation.Products
{
    /// <summary>
    /// Product identity contract. Product visuals are never generated at runtime.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ProductVisualMarker))]
    public sealed class ProductPrefabAuthoring : MonoBehaviour
    {
        [SerializeField]
        private string _productId;

        public string ProductId => _productId;

        public bool TryValidate(out string report)
        {
            if (string.IsNullOrWhiteSpace(_productId))
            {
                report = "Product ID is missing.";
                return false;
            }

            if (GetComponent<ProductVisualMarker>() == null)
            {
                report = $"{name} is missing ProductVisualMarker.";
                return false;
            }

            report = "Product prefab contract is valid.";
            return true;
        }

        public void Configure(string productId)
        {
            _productId = productId?.Trim() ?? string.Empty;
        }
    }
}
