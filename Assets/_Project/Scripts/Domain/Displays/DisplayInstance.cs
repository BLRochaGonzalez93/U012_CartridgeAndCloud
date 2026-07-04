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

    public readonly struct DisplayInstanceId : IEquatable<DisplayInstanceId>
    {
        public string Value { get; }

        public DisplayInstanceId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "Display instance ID cannot be empty.",
                    nameof(value));
            }

            Value = value;
        }

        public bool Equals(DisplayInstanceId other)
        {
            return string.Equals(Value, other.Value, StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is DisplayInstanceId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value == null
                ? 0
                : StringComparer.Ordinal.GetHashCode(Value);
        }

        public override string ToString()
        {
            return Value ?? string.Empty;
        }

        public static bool operator ==(
            DisplayInstanceId left,
            DisplayInstanceId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(
            DisplayInstanceId left,
            DisplayInstanceId right)
        {
            return !left.Equals(right);
        }
    }

    public enum DisplayAssignmentFailureReason
    {
        None = 0,
        ProductDefinitionMissing = 1,
        CategoryNotAllowed = 2,
        ProductAlreadyAssigned = 3,
        DifferentProductAlreadyAssigned = 4,
        DisplayContainsStock = 5
    }

    public sealed class DisplayAssignmentResult
    {
        public bool Succeeded { get; }

        public DisplayAssignmentFailureReason FailureReason { get; }

        public bool HadAssignmentBefore { get; }

        public bool HasAssignmentAfter { get; }

        public ProductDefinitionId PreviousProductId { get; }

        public ProductDefinitionId CurrentProductId { get; }

        private DisplayAssignmentResult(
            bool succeeded,
            DisplayAssignmentFailureReason failureReason,
            bool hadAssignmentBefore,
            bool hasAssignmentAfter,
            ProductDefinitionId previousProductId,
            ProductDefinitionId currentProductId)
        {
            Succeeded = succeeded;
            FailureReason = failureReason;
            HadAssignmentBefore = hadAssignmentBefore;
            HasAssignmentAfter = hasAssignmentAfter;
            PreviousProductId = previousProductId;
            CurrentProductId = currentProductId;
        }

        public static DisplayAssignmentResult Success(
            ProductDefinitionId productId)
        {
            return new DisplayAssignmentResult(
                true,
                DisplayAssignmentFailureReason.None,
                false,
                true,
                default(ProductDefinitionId),
                productId);
        }

        public static DisplayAssignmentResult Failure(
            DisplayAssignmentFailureReason failureReason,
            bool hasAssignment,
            ProductDefinitionId productId)
        {
            return new DisplayAssignmentResult(
                false,
                failureReason,
                hasAssignment,
                hasAssignment,
                productId,
                productId);
        }
    }

    public enum DisplayClearAssignmentFailureReason
    {
        None = 0,
        NoAssignedProduct = 1,
        StockRemaining = 2
    }

    public sealed class DisplayClearAssignmentResult
    {
        public bool Succeeded { get; }

        public DisplayClearAssignmentFailureReason FailureReason { get; }

        public ProductDefinitionId PreviousProductId { get; }

        public bool HasAssignmentAfter { get; }

        private DisplayClearAssignmentResult(
            bool succeeded,
            DisplayClearAssignmentFailureReason failureReason,
            ProductDefinitionId previousProductId,
            bool hasAssignmentAfter)
        {
            Succeeded = succeeded;
            FailureReason = failureReason;
            PreviousProductId = previousProductId;
            HasAssignmentAfter = hasAssignmentAfter;
        }

        public static DisplayClearAssignmentResult Success(
            ProductDefinitionId previousProductId)
        {
            return new DisplayClearAssignmentResult(
                true,
                DisplayClearAssignmentFailureReason.None,
                previousProductId,
                false);
        }

        public static DisplayClearAssignmentResult Failure(
            DisplayClearAssignmentFailureReason failureReason,
            ProductDefinitionId productId,
            bool hasAssignment)
        {
            return new DisplayClearAssignmentResult(
                false,
                failureReason,
                productId,
                hasAssignment);
        }
    }
}
