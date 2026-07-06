using UnityEngine;

namespace VRMGames.CartridgeAndCloud.Runtime.Placement
{
    public sealed class PlacedFixtureVisual :
        MonoBehaviour
    {
        public string DefinitionId {
            get;
            private set;
        }

        public string InstanceId {
            get;
            private set;
        }

        public string AssignedProductId {
            get;
            private set;
        }

        public int ProductQuantity {
            get;
            private set;
        }

        public bool HasStock =>
            ProductQuantity > 0 &&
            !string.IsNullOrWhiteSpace(AssignedProductId);

        public void Configure(
            string definitionId,
            string instanceId)
        {
            DefinitionId =
                definitionId ?? string.Empty;
            InstanceId =
                instanceId ?? string.Empty;
        }

        public void ConfigureStock(
            string assignedProductId,
            int productQuantity)
        {
            AssignedProductId =
                assignedProductId ?? string.Empty;
            ProductQuantity =
                Mathf.Max(0, productQuantity);
        }
    }
}
