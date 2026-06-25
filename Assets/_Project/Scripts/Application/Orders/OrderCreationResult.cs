using VRMGames.CartridgeAndCloud.Domain.Orders;
using VRMGames.CartridgeAndCloud.Domain.Products;

namespace VRMGames.CartridgeAndCloud.Application.Orders
{
    public sealed class OrderCreationResult
    {
        public bool Succeeded { get; }

        public OrderCreationFailureReason FailureReason { get; }

        public ProductDefinitionId? FailedProductId { get; }

        public PurchaseOrder Order { get; }

        private OrderCreationResult(
            bool succeeded,
            OrderCreationFailureReason failureReason,
            ProductDefinitionId? failedProductId,
            PurchaseOrder order)
        {
            Succeeded = succeeded;
            FailureReason = failureReason;
            FailedProductId = failedProductId;
            Order = order;
        }

        public static OrderCreationResult Success(PurchaseOrder order)
        {
            return new OrderCreationResult(
                true,
                OrderCreationFailureReason.None,
                null,
                order);
        }

        public static OrderCreationResult Failure(
            OrderCreationFailureReason failureReason,
            ProductDefinitionId? failedProductId = null)
        {
            return new OrderCreationResult(
                false,
                failureReason,
                failedProductId,
                null);
        }
    }
}
