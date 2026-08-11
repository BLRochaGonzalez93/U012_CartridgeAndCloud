using System;
using System.Collections.Generic;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Application.Store;
using VRMGames.CartridgeAndCloud.Domain.Products;
using VRMGames.CartridgeAndCloud.Domain.Store;
using VRMGames.CartridgeAndCloud.Infrastructure.Products;

namespace VRMGames.CartridgeAndCloud.Infrastructure.Store
{
    [CreateAssetMenu(
        menuName = "Cartridge & Cloud/Store/Store Content Catalog",
        fileName = "ContentCatalog")]
    public sealed class StoreContentCatalogAsset : ScriptableObject
    {
        [SerializeField]
        private StoreFixtureCatalogAsset _fixtureCatalog;

        [SerializeField]
        private RetailProductCatalogAsset _productCatalog;

        public StoreFixtureCatalogAsset FixtureCatalog =>
            _fixtureCatalog;

        public RetailProductCatalogAsset ProductCatalog =>
            _productCatalog;

        public IReadOnlyList<StoreFixtureCatalogAsset.StoreFixtureEntry>
            Furniture => RequireFixtureCatalog().Entries;

        public IReadOnlyList<RetailProductCatalogAsset.RetailProductEntry>
            Products => RequireProductCatalog().Entries;

        public StoreContentCatalog BuildCatalog()
        {
            StoreFixtureCatalogAsset fixtureCatalog =
                RequireFixtureCatalog();
            RetailProductCatalogAsset productCatalog =
                RequireProductCatalog();

            IReadOnlyList<StoreFixtureDefinition> fixtureDefinitions =
                fixtureCatalog.BuildDefinitions();
            IReadOnlyList<RetailProductDefinition> productDefinitions =
                productCatalog.BuildDefinitions();

            return new StoreContentCatalog(
                fixtureDefinitions,
                productDefinitions);
        }

        public void Configure(
            StoreFixtureCatalogAsset fixtureCatalog,
            RetailProductCatalogAsset productCatalog)
        {
            _fixtureCatalog = fixtureCatalog ??
                throw new ArgumentNullException(nameof(fixtureCatalog));
            _productCatalog = productCatalog ??
                throw new ArgumentNullException(nameof(productCatalog));
        }

        private StoreFixtureCatalogAsset RequireFixtureCatalog()
        {
            if (_fixtureCatalog == null)
            {
                throw new InvalidOperationException(
                    "Store content catalog has no fixture catalog assigned.");
            }

            return _fixtureCatalog;
        }

        private RetailProductCatalogAsset RequireProductCatalog()
        {
            if (_productCatalog == null)
            {
                throw new InvalidOperationException(
                    "Store content catalog has no retail product catalog assigned.");
            }

            return _productCatalog;
        }
    }
}
