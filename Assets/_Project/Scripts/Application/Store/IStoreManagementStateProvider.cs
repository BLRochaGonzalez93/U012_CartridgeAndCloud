using VRMGames.CartridgeAndCloud.Domain.Store;

namespace VRMGames.CartridgeAndCloud.Application.Store
{
    public interface IStoreManagementStateProvider
    {
        StoreOperationsState ManagementState { get; }
    }
}
