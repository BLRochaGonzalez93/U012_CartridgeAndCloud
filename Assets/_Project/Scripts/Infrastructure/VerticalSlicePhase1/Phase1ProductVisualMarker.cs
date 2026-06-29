using UnityEngine;

namespace VRMGames.CartridgeAndCloud.Infrastructure.VerticalSlicePhase1
{
    public sealed class Phase1ProductVisualMarker :
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
