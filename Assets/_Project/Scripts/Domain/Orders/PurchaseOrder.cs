using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VRMGames.CartridgeAndCloud.Domain.Products;
using VRMGames.CartridgeAndCloud.Domain.Suppliers;
using VRMGames.CartridgeAndCloud.Domain.Inventory;

namespace VRMGames.CartridgeAndCloud.Domain.Orders
{
    public sealed class PurchaseOrder
    {
        private readonly ReadOnlyCollection<PurchaseOrderLine> _lines;

        public PurchaseOrderId Id { get; }

        public SupplierId SupplierId { get; }

        public IReadOnlyList<PurchaseOrderLine> Lines => _lines;

        public PurchaseOrderStatus Status { get; private set; }

        public long TotalCostCents { get; }

        public int TotalBoxes { get; }

        public int TotalUnits { get; }

        public PurchaseOrder(
            PurchaseOrderId id,
            SupplierId supplierId,
            IEnumerable<PurchaseOrderLine> lines)
        {
            if (string.IsNullOrWhiteSpace(id.Value))
            {
                throw new ArgumentException(
                    "Purchase order ID must be initialized.",
                    nameof(id));
            }

            if (string.IsNullOrWhiteSpace(supplierId.Value))
            {
                throw new ArgumentException(
                    "Supplier ID must be initialized.",
                    nameof(supplierId));
            }

            if (lines == null)
            {
                throw new ArgumentNullException(nameof(lines));
            }

            List<PurchaseOrderLine> lineList =
                new List<PurchaseOrderLine>();

            HashSet<ProductDefinitionId> uniqueProducts =
                new HashSet<ProductDefinitionId>();

            long totalCost = 0;
            int totalBoxes = 0;
            int totalUnits = 0;

            foreach (PurchaseOrderLine line in lines)
            {
                if (line == null)
                {
                    throw new ArgumentException(
                        "Purchase order lines cannot contain null.",
                        nameof(lines));
                }

                if (!uniqueProducts.Add(line.ProductId))
                {
                    throw new ArgumentException(
                        $"Purchase order product {line.ProductId} is duplicated.",
                        nameof(lines));
                }

                totalCost = checked(totalCost + line.TotalCostCents);
                totalBoxes = checked(totalBoxes + line.BoxCount);
                totalUnits = checked(
                    totalUnits + line.OrderedQuantity.Value);

                lineList.Add(line);
            }

            if (lineList.Count == 0)
            {
                throw new ArgumentException(
                    "Purchase order must contain at least one line.",
                    nameof(lines));
            }

            lineList.Sort(
                (left, right) =>
                    StringComparer.Ordinal.Compare(
                        left.ProductId.Value,
                        right.ProductId.Value));

            Id = id;
            SupplierId = supplierId;
            _lines =
                new ReadOnlyCollection<PurchaseOrderLine>(lineList);

            Status = PurchaseOrderStatus.Draft;
            TotalCostCents = totalCost;
            TotalBoxes = totalBoxes;
            TotalUnits = totalUnits;
        }

        public PurchaseOrderTransitionResult Submit()
        {
            return Transition(
                PurchaseOrderStatus.Draft,
                PurchaseOrderStatus.Submitted);
        }

        public PurchaseOrderTransitionResult MarkDelivered()
        {
            return Transition(
                PurchaseOrderStatus.Submitted,
                PurchaseOrderStatus.Delivered);
        }

        public PurchaseOrderTransitionResult MarkReceived()
        {
            return Transition(
                PurchaseOrderStatus.Delivered,
                PurchaseOrderStatus.Received);
        }

        public PurchaseOrderTransitionResult Cancel()
        {
            PurchaseOrderStatus previous = Status;

            if (Status != PurchaseOrderStatus.Draft &&
                Status != PurchaseOrderStatus.Submitted)
            {
                return PurchaseOrderTransitionResult.Failure(Status);
            }

            Status = PurchaseOrderStatus.Cancelled;

            return PurchaseOrderTransitionResult.Success(
                previous,
                Status);
        }

        private PurchaseOrderTransitionResult Transition(
            PurchaseOrderStatus requiredStatus,
            PurchaseOrderStatus nextStatus)
        {
            PurchaseOrderStatus previous = Status;

            if (Status != requiredStatus)
            {
                return PurchaseOrderTransitionResult.Failure(Status);
            }

            Status = nextStatus;

            return PurchaseOrderTransitionResult.Success(
                previous,
                Status);
        }
    }

    public readonly struct PurchaseOrderId :
        IEquatable<PurchaseOrderId>
    {
        public string Value { get; }

        public PurchaseOrderId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "Purchase order ID cannot be empty.",
                    nameof(value));
            }

            Value = value;
        }

        public bool Equals(PurchaseOrderId other)
        {
            return string.Equals(
                Value,
                other.Value,
                StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is PurchaseOrderId other &&
                   Equals(other);
        }

        public override int GetHashCode()
        {
            return StringComparer.Ordinal.GetHashCode(
                Value);
        }

        public override string ToString()
        {
            return Value;
        }

        public static bool operator ==(
            PurchaseOrderId left,
            PurchaseOrderId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(
            PurchaseOrderId left,
            PurchaseOrderId right)
        {
            return !left.Equals(right);
        }
    }

    public sealed class PurchaseOrderLine
    {
        public ProductDefinitionId ProductId { get; }

        public int BoxCount { get; }

        public Quantity UnitsPerBox { get; }

        public Quantity OrderedQuantity { get; }

        public int UnitCostCents { get; }

        public long TotalCostCents { get; }

        public PurchaseOrderLine(
            ProductDefinitionId productId,
            int boxCount,
            Quantity unitsPerBox,
            int unitCostCents)
        {
            if (string.IsNullOrWhiteSpace(productId.Value))
            {
                throw new ArgumentException(
                    "Product definition ID must be initialized.",
                    nameof(productId));
            }

            if (boxCount <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(boxCount));
            }

            if (unitsPerBox.IsZero)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(unitsPerBox));
            }

            if (unitCostCents <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(unitCostCents));
            }

            ProductId = productId;
            BoxCount = boxCount;
            UnitsPerBox = unitsPerBox;
            UnitCostCents = unitCostCents;
            OrderedQuantity =
                new Quantity(
                    checked(boxCount * unitsPerBox.Value));

            TotalCostCents =
                checked(
                    (long)OrderedQuantity.Value * unitCostCents);
        }
    }

    public readonly struct PurchaseOrderRequestLine
    {
        public ProductDefinitionId ProductId { get; }

        public int BoxCount { get; }

        public PurchaseOrderRequestLine(
            ProductDefinitionId productId,
            int boxCount)
        {
            if (string.IsNullOrWhiteSpace(productId.Value))
            {
                throw new ArgumentException(
                    "Product definition ID must be initialized.",
                    nameof(productId));
            }

            if (boxCount <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(boxCount),
                    "Requested box count must be greater than zero.");
            }

            ProductId = productId;
            BoxCount = boxCount;
        }
    }

    public enum PurchaseOrderStatus
    {
        Draft = 0,
        Submitted = 1,
        Delivered = 2,
        Received = 3,
        Cancelled = 4
    }

    public enum PurchaseOrderTransitionFailureReason
    {
        None = 0,
        InvalidCurrentStatus = 1
    }

    public readonly struct PurchaseOrderTransitionResult
    {
        public bool Succeeded { get; }

        public PurchaseOrderTransitionFailureReason FailureReason
        {
            get;
        }

        public PurchaseOrderStatus PreviousStatus { get; }

        public PurchaseOrderStatus CurrentStatus { get; }

        private PurchaseOrderTransitionResult(
            bool succeeded,
            PurchaseOrderTransitionFailureReason failureReason,
            PurchaseOrderStatus previousStatus,
            PurchaseOrderStatus currentStatus)
        {
            Succeeded = succeeded;
            FailureReason = failureReason;
            PreviousStatus = previousStatus;
            CurrentStatus = currentStatus;
        }

        public static PurchaseOrderTransitionResult Success(
            PurchaseOrderStatus previousStatus,
            PurchaseOrderStatus currentStatus)
        {
            return new PurchaseOrderTransitionResult(
                true,
                PurchaseOrderTransitionFailureReason.None,
                previousStatus,
                currentStatus);
        }

        public static PurchaseOrderTransitionResult Failure(
            PurchaseOrderStatus currentStatus)
        {
            return new PurchaseOrderTransitionResult(
                false,
                PurchaseOrderTransitionFailureReason.InvalidCurrentStatus,
                currentStatus,
                currentStatus);
        }
    }
}
