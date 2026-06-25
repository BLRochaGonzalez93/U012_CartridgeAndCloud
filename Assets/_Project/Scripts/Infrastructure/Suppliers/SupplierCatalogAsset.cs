using System;
using System.Collections.Generic;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.Products;
using VRMGames.CartridgeAndCloud.Domain.Suppliers;
using VRMGames.CartridgeAndCloud.Infrastructure.Products;

namespace VRMGames.CartridgeAndCloud.Infrastructure.Suppliers
{
    [CreateAssetMenu(
        menuName = "Cartridge & Cloud/Suppliers/Supplier Catalog",
        fileName = "CC_SupplierCatalog_")]
    public sealed class SupplierCatalogAsset : ScriptableObject
    {
        [SerializeField]
        private string _catalogId;

        [SerializeField]
        private SupplierDefinitionAsset _supplier;

        [SerializeField]
        private ProductCatalogAsset _productCatalog;

        [SerializeField]
        private List<SupplierCatalogEntryAsset> _entries =
            new List<SupplierCatalogEntryAsset>();

        public string CatalogId => _catalogId;

        public SupplierDefinitionAsset Supplier => _supplier;

        public ProductCatalogAsset ProductCatalog => _productCatalog;

        public IReadOnlyList<SupplierCatalogEntryAsset> Entries =>
            _entries;

        public SupplierCatalog BuildCatalog()
        {
            if (_supplier == null)
            {
                throw new InvalidOperationException(
                    "Supplier catalog has no supplier asset.");
            }

            if (_productCatalog == null)
            {
                throw new InvalidOperationException(
                    "Supplier catalog has no product catalog asset.");
            }

            ProductDefinitionRegistry registry =
                _productCatalog.BuildRegistry();

            List<SupplierCatalogEntry> entries =
                new List<SupplierCatalogEntry>(_entries.Count);

            foreach (SupplierCatalogEntryAsset entry in _entries)
            {
                if (entry == null)
                {
                    throw new InvalidOperationException(
                        "Supplier catalog contains a missing entry.");
                }

                entries.Add(entry.BuildEntry());
            }

            return new SupplierCatalog(
                new SupplierCatalogId(_catalogId),
                _supplier.BuildDefinition(),
                registry,
                entries);
        }

        public void Configure(
            string catalogId,
            SupplierDefinitionAsset supplier,
            ProductCatalogAsset productCatalog,
            IEnumerable<SupplierCatalogEntryAsset> entries)
        {
            if (entries == null)
            {
                throw new ArgumentNullException(nameof(entries));
            }

            _catalogId = catalogId;
            _supplier = supplier;
            _productCatalog = productCatalog;
            _entries =
                new List<SupplierCatalogEntryAsset>(entries);
        }
    }
}
