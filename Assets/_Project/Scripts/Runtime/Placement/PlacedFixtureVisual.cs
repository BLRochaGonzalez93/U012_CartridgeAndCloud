using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.Store;
using VRMGames.CartridgeAndCloud.Presentation.Interaction;

namespace VRMGames.CartridgeAndCloud.Runtime.Placement
{
    public sealed class PlacedFixtureVisual :
        MonoBehaviour,
        IWorldInteractionTarget
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

        public StoreFixtureKind FixtureKind {
            get;
            private set;
        }

        public bool IsFixtureInteractive {
            get;
            private set;
        }

        public bool HasStock =>
            ProductQuantity > 0 &&
            !string.IsNullOrWhiteSpace(AssignedProductId);

        public string InteractionId =>
            InstanceId ?? string.Empty;

        public WorldInteractionKind InteractionKind =>
            ResolveInteractionKind(FixtureKind);

        public Transform InteractionTransform =>
            transform;

        public float InteractionRange =>
            FixtureKind == StoreFixtureKind.BackroomStorage
                ? 2.2f
                : 1.8f;

        public int InteractionPriority =>
            FixtureKind == StoreFixtureKind.CheckoutCounter
                ? 75
                : 50;

        public bool IsInteractionAvailable =>
            IsFixtureInteractive &&
            !string.IsNullOrWhiteSpace(InstanceId);

        public Vector3 GetInteractionPoint(
            Vector3 actorPosition)
        {
            return WorldInteractionGeometry.ResolveClosestPoint(
                transform,
                actorPosition);
        }

        public void Configure(
            string definitionId,
            string instanceId,
            StoreFixtureKind fixtureKind,
            bool isInteractive)
        {
            DefinitionId =
                definitionId ?? string.Empty;
            InstanceId =
                instanceId ?? string.Empty;
            FixtureKind = fixtureKind;
            IsFixtureInteractive = isInteractive;
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

        private static WorldInteractionKind
            ResolveInteractionKind(
                StoreFixtureKind kind)
        {
            switch (kind)
            {
                case StoreFixtureKind.CheckoutCounter:
                    return WorldInteractionKind.Checkout;

                case StoreFixtureKind.WallShelf:
                case StoreFixtureKind.CentralShelf:
                case StoreFixtureKind.LowDisplay:
                case StoreFixtureKind.FeaturedDisplay:
                    return WorldInteractionKind.Display;

                case StoreFixtureKind.BackroomStorage:
                case StoreFixtureKind.ReceivingCrate:
                    return WorldInteractionKind.Container;

                default:
                    return WorldInteractionKind.Fixture;
            }
        }
    }
}
