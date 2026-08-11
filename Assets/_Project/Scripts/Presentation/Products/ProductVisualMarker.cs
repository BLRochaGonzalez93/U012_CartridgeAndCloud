using UnityEngine;
using VRMGames.CartridgeAndCloud.Presentation.Interaction;

namespace VRMGames.CartridgeAndCloud.Presentation.Products
{
    public sealed class ProductVisualMarker :
        MonoBehaviour,
        IWorldInteractionTarget
    {
        public string ProductId {
            get;
            private set;
        }

        public string InteractionId =>
            ProductId ?? string.Empty;

        public WorldInteractionKind InteractionKind =>
            WorldInteractionKind.Product;

        public Transform InteractionTransform =>
            transform;

        public float InteractionRange => 1.5f;

        public int InteractionPriority => 100;

        public bool IsInteractionAvailable =>
            !string.IsNullOrWhiteSpace(ProductId);

        public Vector3 GetInteractionPoint(
            Vector3 actorPosition)
        {
            return WorldInteractionGeometry.ResolveClosestPoint(
                transform,
                actorPosition);
        }

        public void Configure(
            string productId)
        {
            ProductId =
                productId ?? string.Empty;
        }
    }
}
