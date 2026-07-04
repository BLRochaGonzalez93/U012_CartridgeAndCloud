using UnityEngine;

namespace VRMGames.CartridgeAndCloud.Presentation.Products
{
    public sealed class ProductVisualMarker :
        MonoBehaviour
    {
        public string ProductId {
            get;
            private set;
        }

        public void Configure(
            string productId)
        {
            ProductId =
                productId ?? string.Empty;
        }
    }
}
