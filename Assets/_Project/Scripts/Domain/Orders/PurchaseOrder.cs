using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VRMGames.CartridgeAndCloud.Domain.Products;
using VRMGames.CartridgeAndCloud.Domain.Suppliers;

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
}
