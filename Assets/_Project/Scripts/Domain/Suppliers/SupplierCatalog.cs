using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VRMGames.CartridgeAndCloud.Domain.Products;

namespace VRMGames.CartridgeAndCloud.Domain.Suppliers
{
    public sealed class SupplierCatalog
    {
        private readonly Dictionary<
            ProductDefinitionId,
            SupplierCatalogEntry> _entriesByProduct;

        private readonly ReadOnlyCollection<SupplierCatalogEntry> _entries;

        public SupplierCatalogId Id { get; }

        public SupplierDefinition Supplier { get; }

        public IReadOnlyList<SupplierCatalogEntry> Entries =>
            _entries;

        public int Count => _entries.Count;

        public SupplierCatalog(
            SupplierCatalogId id,
            SupplierDefinition supplier,
            ProductDefinitionRegistry productDefinitions,
            IEnumerable<SupplierCatalogEntry> entries)
        {
            if (string.IsNullOrWhiteSpace(id.Value))
            {
                throw new ArgumentException(
                    "Supplier catalog ID must be initialized.",
                    nameof(id));
            }

            Supplier = supplier ??
                throw new ArgumentNullException(nameof(supplier));

            if (productDefinitions == null)
            {
                throw new ArgumentNullException(
                    nameof(productDefinitions));
            }

            if (entries == null)
            {
                throw new ArgumentNullException(nameof(entries));
            }

            Id = id;
            _entriesByProduct =
                new Dictionary<
                    ProductDefinitionId,
                    SupplierCatalogEntry>();

            List<SupplierCatalogEntry> entryList =
                new List<SupplierCatalogEntry>();

            foreach (SupplierCatalogEntry entry in entries)
            {
                if (entry == null)
                {
                    throw new ArgumentException(
                        "Supplier catalog entries cannot contain null.",
                        nameof(entries));
                }

                if (!productDefinitions.Contains(entry.ProductId))
                {
                    throw new ArgumentException(
                        $"Product definition {entry.ProductId} is missing.",
                        nameof(entries));
                }

                if (_entriesByProduct.ContainsKey(entry.ProductId))
                {
                    throw new ArgumentException(
                        $"Supplier product {entry.ProductId} is duplicated.",
                        nameof(entries));
                }

                _entriesByProduct.Add(
                    entry.ProductId,
                    entry);

                entryList.Add(entry);
            }

            entryList.Sort(
                (left, right) =>
                    StringComparer.Ordinal.Compare(
                        left.ProductId.Value,
                        right.ProductId.Value));

            _entries =
                new ReadOnlyCollection<SupplierCatalogEntry>(
                    entryList);
        }

        public bool Contains(ProductDefinitionId productId)
        {
            return _entriesByProduct.ContainsKey(productId);
        }

        public bool TryGetEntry(
            ProductDefinitionId productId,
            out SupplierCatalogEntry entry)
        {
            return _entriesByProduct.TryGetValue(
                productId,
                out entry);
        }

        public SupplierCatalogEntry GetEntry(
            ProductDefinitionId productId)
        {
            if (!_entriesByProduct.TryGetValue(
                    productId,
                    out SupplierCatalogEntry entry))
            {
                throw new KeyNotFoundException(
                    $"Supplier product {productId} was not found.");
            }

            return entry;
        }
    }
}
