using VRMGames.CartridgeAndCloud.Domain.Store;

namespace VRMGames.CartridgeAndCloud.Application.Store
{
    /// <summary>
    /// Keeps legacy characterization tests concise while the production API
    /// models dispatch and physical receipt as two distinct operations.
    /// </summary>
    public static class StoreOperationsReceiptTestExtensions
    {
        public static StoreOperationResult DispatchAndComplete(
            this StoreOperationsFacade service,
            string orderId)
        {
            StoreOperationResult dispatch = service.ReceiveOrder(orderId);
            if (!dispatch.Succeeded)
            {
                return dispatch;
            }

            StoreDeliveryRunRecord run =
                service.State.DeliveryRuns[
                    service.State.DeliveryRuns.Count - 1];
            return service.CompleteDeliveryRun(run.DeliveryRunId);
        }

        public static StoreOperationResult ProcessAllAndComplete(
            this StoreOperationsFacade service)
        {
            StoreOperationResult dispatch =
                service.ProcessAllPendingOrders();
            if (!dispatch.Succeeded)
            {
                return dispatch;
            }

            StoreDeliveryRunRecord run =
                service.State.DeliveryRuns[
                    service.State.DeliveryRuns.Count - 1];
            return service.CompleteDeliveryRun(run.DeliveryRunId);
        }
    }
}
