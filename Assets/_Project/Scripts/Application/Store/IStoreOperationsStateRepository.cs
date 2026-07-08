using VRMGames.CartridgeAndCloud.Domain.Identifiers;
using VRMGames.CartridgeAndCloud.Domain.Store;

namespace VRMGames.CartridgeAndCloud.Application.Store
{
    public interface IStoreOperationsCheckpoint : System.IDisposable
    {
        void Commit();
    }

    public interface IStoreOperationsStateRepository
    {
        StoreOperationsState Load(
            SaveSlotId slotId,
            bool preferBackup = false);

        void Save(
            StoreOperationsState state);

        IStoreOperationsCheckpoint BeginCheckpoint(
            StoreOperationsState state);

        bool Delete(
            SaveSlotId slotId);
    }
}
