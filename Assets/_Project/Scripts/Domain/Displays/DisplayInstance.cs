using System;
using VRMGames.CartridgeAndCloud.Domain.Inventory;
using VRMGames.CartridgeAndCloud.Domain.Products;

namespace VRMGames.CartridgeAndCloud.Domain.Displays
{
    public sealed class DisplayInstance
    {
        private ProductDefinitionId _assignedProductId;

        public DisplayInstanceId Id { get; }

        public DisplayDefinition Definition { get; }

        public InventoryContainer Inventory { get; }

        public bool HasAssignedProduct { get; private set; }

        public ProductDefinitionId AssignedProductId => _assignedProductId;

        public bool IsEmpty => Inventory.UsedCapacity == 0;

        public int VisibleUnitCount
        {
            get
            {
                if (!HasAssignedProduct)
                {
                    return 0;
                }

                int quantity =
                    Inventory.GetQuantity(_assignedProductId).Value;

                return Math.Min(
                    quantity,
                    Definition.VisibleUnitLimit);
            }
        }

        public DisplayInstance(
            DisplayInstanceId id,
            DisplayDefinition definition)
        {
            if (string.IsNullOrWhiteSpace(id.Value))
            {
                throw new ArgumentException(
                    "Display instance ID must be initialized.",
                    nameof(id));
            }

            Definition = definition ??
                throw new ArgumentNullException(nameof(definition));

            Id = id;
            Inventory = new InventoryContainer(
                new InventoryContainerId($"display:{id.Value}"),
                InventoryContainerType.Display,
                definition.Capacity);
        }

        public DisplayAssignmentResult TryAssignProduct(
            ProductDefinitionRegistry productDefinitions,
            ProductDefinitionId productId)
        {
            if (productDefinitions == null)
            {
                throw new ArgumentNullException(
                    nameof(productDefinitions));
            }

            if (!productDefinitions.TryGet(
                    productId,
                    out ProductDefinition product))
            {
                return DisplayAssignmentResult.Failure(
                    DisplayAssignmentFailureReason.ProductDefinitionMissing,
                    HasAssignedProduct,
                    _assignedProductId);
            }

            if (Inventory.UsedCapacity != 0)
            {
                return DisplayAssignmentResult.Failure(
                    DisplayAssignmentFailureReason.DisplayContainsStock,
                    HasAssignedProduct,
                    _assignedProductId);
            }

            if (HasAssignedProduct)
            {
                DisplayAssignmentFailureReason reason =
                    _assignedProductId == productId
                        ? DisplayAssignmentFailureReason.ProductAlreadyAssigned
                        : DisplayAssignmentFailureReason
                            .DifferentProductAlreadyAssigned;

                return DisplayAssignmentResult.Failure(
                    reason,
                    true,
                    _assignedProductId);
            }

            if (!Definition.CanAccept(product))
            {
                return DisplayAssignmentResult.Failure(
                    DisplayAssignmentFailureReason.CategoryNotAllowed,
                    false,
                    default(ProductDefinitionId));
            }

            _assignedProductId = productId;
            HasAssignedProduct = true;

            return DisplayAssignmentResult.Success(productId);
        }

        public DisplayClearAssignmentResult TryClearAssignment()
        {
            if (!HasAssignedProduct)
            {
                return DisplayClearAssignmentResult.Failure(
                    DisplayClearAssignmentFailureReason.NoAssignedProduct,
                    default(ProductDefinitionId),
                    false);
            }

            if (Inventory.UsedCapacity != 0)
            {
                return DisplayClearAssignmentResult.Failure(
                    DisplayClearAssignmentFailureReason.StockRemaining,
                    _assignedProductId,
                    true);
            }

            ProductDefinitionId previous = _assignedProductId;
            _assignedProductId = default(ProductDefinitionId);
            HasAssignedProduct = false;

            return DisplayClearAssignmentResult.Success(previous);
        }
    }
}
