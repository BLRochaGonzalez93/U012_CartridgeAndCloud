using System;

namespace VRMGames.CartridgeAndCloud.Domain.Store
{
    public sealed class PlacedStoreFixtureRecord
    {
        public string InstanceId { get; }
        public string DefinitionId { get; }
        public int AnchorX { get; }
        public int AnchorZ { get; }
        public int RotationQuarterTurns { get; }
        public string AssignedProductId { get; }
        public int ProductQuantity { get; }

        public PlacedStoreFixtureRecord(
            string instanceId,
            string definitionId,
            int anchorX,
            int anchorZ,
            int rotationQuarterTurns,
            string assignedProductId,
            int productQuantity)
        {
            if (string.IsNullOrWhiteSpace(instanceId))
            {
                throw new ArgumentException(
                    "An instance ID is required.",
                    nameof(instanceId));
            }

            if (string.IsNullOrWhiteSpace(definitionId))
            {
                throw new ArgumentException(
                    "A definition ID is required.",
                    nameof(definitionId));
            }

            if (rotationQuarterTurns < 0 ||
                rotationQuarterTurns > 3)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(rotationQuarterTurns));
            }

            if (productQuantity < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(productQuantity));
            }

            if (productQuantity > 0 &&
                string.IsNullOrWhiteSpace(
                    assignedProductId))
            {
                throw new ArgumentException(
                    "Stocked fixture requires an assigned product.",
                    nameof(assignedProductId));
            }

            InstanceId = instanceId;
            DefinitionId = definitionId;
            AnchorX = anchorX;
            AnchorZ = anchorZ;
            RotationQuarterTurns =
                rotationQuarterTurns;
            AssignedProductId =
                assignedProductId ?? string.Empty;
            ProductQuantity = productQuantity;
        }

        public PlacedStoreFixtureRecord
            WithAssignedProduct(
                string productId,
                int quantity)
        {
            return new PlacedStoreFixtureRecord(
                InstanceId,
                DefinitionId,
                AnchorX,
                AnchorZ,
                RotationQuarterTurns,
                productId,
                quantity);
        }
    }
}
