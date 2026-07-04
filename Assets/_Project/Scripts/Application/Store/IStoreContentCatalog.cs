using System.Collections.Generic;
using VRMGames.CartridgeAndCloud.Domain.Store;
using VRMGames.CartridgeAndCloud.Domain.Products;

namespace VRMGames.CartridgeAndCloud.Application.Store
{
    public interface IStoreContentCatalog
    {
        IReadOnlyList<StoreFixtureDefinition>
            Furniture { get; }

        IReadOnlyList<RetailProductDefinition>
            Products { get; }

        bool TryGetFurniture(
            string definitionId,
            out StoreFixtureDefinition definition);

        bool TryGetProduct(
            string productId,
            out RetailProductDefinition definition);
    }
}
