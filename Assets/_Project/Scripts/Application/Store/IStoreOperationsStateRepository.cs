using VRMGames.CartridgeAndCloud.Domain.Identifiers;
using VRMGames.CartridgeAndCloud.Domain.Store;

namespace VRMGames.CartridgeAndCloud.Application.Store
{
    public interface IStoreOperationsStateRepository
    {
        StoreOperationsState Load(
            SaveSlotId slotId);

        void Save(
            StoreOperationsState state);

        bool Delete(
            SaveSlotId slotId);
    }
}
