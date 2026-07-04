using System;
using System.Collections.Generic;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Application.Store;
using VRMGames.CartridgeAndCloud.Infrastructure.Products;
using VRMGames.CartridgeAndCloud.Domain.Store;
using VRMGames.CartridgeAndCloud.Domain.Products;

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

        // Embedded legacy-compatible data. Keeping these field names preserves
        // the existing ContentCatalog.asset without an editor migration.
        [SerializeField]
        private StoreFixtureCatalogAsset.StoreFixtureEntry[] _furniture =
            Array.Empty<StoreFixtureCatalogAsset.StoreFixtureEntry>();

        [SerializeField]
        private RetailProductCatalogAsset.RetailProductEntry[] _products =
            Array.Empty<RetailProductCatalogAsset.RetailProductEntry>();

        public StoreFixtureCatalogAsset FixtureCatalog => _fixtureCatalog;

        public RetailProductCatalogAsset ProductCatalog => _productCatalog;

        public IReadOnlyList<StoreFixtureCatalogAsset.StoreFixtureEntry> Furniture
        {
            get
            {
                if (_fixtureCatalog != null)
                {
                    return _fixtureCatalog.Entries;
                }

                EnsureEmbeddedDefaults();
                return _furniture;
            }
        }

        public IReadOnlyList<RetailProductCatalogAsset.RetailProductEntry> Products
        {
            get
            {
                if (_productCatalog != null)
                {
                    return _productCatalog.Entries;
                }

                EnsureEmbeddedDefaults();
                return _products;
            }
        }

        private void OnEnable()
        {
            EnsureEmbeddedDefaults();
        }

        public StoreContentCatalog BuildCatalog()
        {
            IReadOnlyList<StoreFixtureCatalogAsset.StoreFixtureEntry> furniture =
                Furniture;
            IReadOnlyList<RetailProductCatalogAsset.RetailProductEntry> products =
                Products;

            List<StoreFixtureDefinition> fixtureDefinitions =
                new List<StoreFixtureDefinition>(furniture.Count);
            foreach (StoreFixtureCatalogAsset.StoreFixtureEntry entry in furniture)
            {
                if (entry == null)
                {
                    continue;
                }

                fixtureDefinitions.Add(
                    new StoreFixtureDefinition(
                        entry.definitionId,
                        entry.displayName,
                        entry.kind,
                        entry.widthCells,
                        entry.depthCells,
                        entry.heightMeters,
                        entry.capacity,
                        entry.unitCostCents,
                        entry.isInteractive,
                        entry.isPurchasable,
                        entry.supportsProducts,
                        entry.materialVariantId,
                        entry.prefabResourcePath));
            }

            List<RetailProductDefinition> productDefinitions =
                new List<RetailProductDefinition>(products.Count);
            foreach (RetailProductCatalogAsset.RetailProductEntry entry in products)
            {
                if (entry == null)
                {
                    continue;
                }

                productDefinitions.Add(
                    new RetailProductDefinition(
                        entry.productId,
                        entry.displayName,
                        entry.kind,
                        entry.wholesalePriceCents,
                        entry.salePriceCents,
                        entry.unitsPerCase,
                        entry.materialVariantId,
                        entry.labelId,
                        entry.iconResourcePath,
                        entry.coverResourcePath,
                        entry.prefabResourcePath));
            }

            return new StoreContentCatalog(
                fixtureDefinitions,
                productDefinitions);
        }

        public void Configure(
            StoreFixtureCatalogAsset fixtureCatalog,
            RetailProductCatalogAsset productCatalog)
        {
            _fixtureCatalog = fixtureCatalog;
            _productCatalog = productCatalog;
        }

        public void Configure(
            StoreFixtureCatalogAsset.StoreFixtureEntry[] furniture,
            RetailProductCatalogAsset.RetailProductEntry[] products)
        {
            _fixtureCatalog = null;
            _productCatalog = null;
            _furniture = furniture ??
                Array.Empty<StoreFixtureCatalogAsset.StoreFixtureEntry>();
            _products = products ??
                Array.Empty<RetailProductCatalogAsset.RetailProductEntry>();
            EnsureEmbeddedDefaults();
        }

        private void EnsureEmbeddedDefaults()
        {
            if (_fixtureCatalog == null &&
                (_furniture == null || _furniture.Length == 0))
            {
                StoreFixtureCatalogAsset defaults =
                    CreateInstance<StoreFixtureCatalogAsset>();
                _furniture = Copy(defaults.Entries);
                DestroyImmediate(defaults);
            }

            if (_productCatalog == null &&
                (_products == null || _products.Length == 0))
            {
                RetailProductCatalogAsset defaults =
                    CreateInstance<RetailProductCatalogAsset>();
                _products = Copy(defaults.Entries);
                DestroyImmediate(defaults);
            }
        }

        private static T[] Copy<T>(IReadOnlyList<T> source)
        {
            T[] result = new T[source.Count];
            for (int index = 0; index < source.Count; index++)
            {
                result[index] = source[index];
            }

            return result;
        }
    }
}
