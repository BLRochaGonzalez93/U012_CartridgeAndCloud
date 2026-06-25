using System;
using System.Collections.Generic;
using VRMGames.CartridgeAndCloud.Domain.Orders;
using VRMGames.CartridgeAndCloud.Domain.Products;
using VRMGames.CartridgeAndCloud.Domain.Suppliers;

namespace VRMGames.CartridgeAndCloud.Application.Orders
{
    public sealed class SupplierOrderService
    {
        public OrderCreationResult CreateDraft(
            PurchaseOrderId orderId,
            SupplierCatalog catalog,
            IEnumerable<PurchaseOrderRequestLine> requestedLines)
        {
            if (catalog == null)
            {
                throw new ArgumentNullException(nameof(catalog));
            }

            if (requestedLines == null)
            {
                throw new ArgumentNullException(nameof(requestedLines));
            }

            List<PurchaseOrderRequestLine> requests =
                new List<PurchaseOrderRequestLine>(requestedLines);

            if (requests.Count == 0)
            {
                return OrderCreationResult.Failure(
                    OrderCreationFailureReason.EmptyRequest);
            }

            HashSet<ProductDefinitionId> uniqueProducts =
                new HashSet<ProductDefinitionId>();

            List<PurchaseOrderLine> orderLines =
                new List<PurchaseOrderLine>();

            foreach (PurchaseOrderRequestLine request in requests)
            {
                if (!uniqueProducts.Add(request.ProductId))
                {
                    return OrderCreationResult.Failure(
                        OrderCreationFailureReason.DuplicateProduct,
                        request.ProductId);
                }

                if (!catalog.TryGetEntry(
                        request.ProductId,
                        out SupplierCatalogEntry entry))
                {
                    return OrderCreationResult.Failure(
                        OrderCreationFailureReason.ProductNotOffered,
                        request.ProductId);
                }

                if (!entry.CanOrder(request.BoxCount))
                {
                    return OrderCreationResult.Failure(
                        OrderCreationFailureReason
                            .BoxCountOutsideSupplierLimits,
                        request.ProductId);
                }

                orderLines.Add(
                    new PurchaseOrderLine(
                        request.ProductId,
                        request.BoxCount,
                        entry.UnitsPerBox,
                        entry.UnitCostCents));
            }

            PurchaseOrder order =
                new PurchaseOrder(
                    orderId,
                    catalog.Supplier.Id,
                    orderLines);

            return OrderCreationResult.Success(order);
        }
    }
}
