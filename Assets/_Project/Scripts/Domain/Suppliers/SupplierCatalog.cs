using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VRMGames.CartridgeAndCloud.Domain.Products;
using VRMGames.CartridgeAndCloud.Domain.Inventory;

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

    public readonly struct SupplierCatalogId :
        IEquatable<SupplierCatalogId>
    {
        public string Value { get; }

        public SupplierCatalogId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "Supplier catalog ID cannot be empty.",
                    nameof(value));
            }

            Value = value;
        }

        public bool Equals(SupplierCatalogId other)
        {
            return string.Equals(
                Value,
                other.Value,
                StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is SupplierCatalogId other &&
                   Equals(other);
        }

        public override int GetHashCode()
        {
            return StringComparer.Ordinal.GetHashCode(
                Value);
        }

        public override string ToString()
        {
            return Value;
        }

        public static bool operator ==(
            SupplierCatalogId left,
            SupplierCatalogId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(
            SupplierCatalogId left,
            SupplierCatalogId right)
        {
            return !left.Equals(right);
        }
    }

    public sealed class SupplierCatalogEntry
    {
        public ProductDefinitionId ProductId { get; }

        public int UnitCostCents { get; }

        public Quantity UnitsPerBox { get; }

        public int MinimumBoxes { get; }

        public int MaximumBoxes { get; }

        public SupplierCatalogEntry(
            ProductDefinitionId productId,
            int unitCostCents,
            Quantity unitsPerBox,
            int minimumBoxes,
            int maximumBoxes)
        {
            if (string.IsNullOrWhiteSpace(productId.Value))
            {
                throw new ArgumentException(
                    "Product definition ID must be initialized.",
                    nameof(productId));
            }

            if (unitCostCents <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(unitCostCents),
                    "Supplier unit cost must be greater than zero.");
            }

            if (unitsPerBox.IsZero)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(unitsPerBox),
                    "Units per box must be greater than zero.");
            }

            if (minimumBoxes <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(minimumBoxes),
                    "Minimum boxes must be greater than zero.");
            }

            if (maximumBoxes < minimumBoxes)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(maximumBoxes),
                    "Maximum boxes cannot be below the minimum.");
            }

            ProductId = productId;
            UnitCostCents = unitCostCents;
            UnitsPerBox = unitsPerBox;
            MinimumBoxes = minimumBoxes;
            MaximumBoxes = maximumBoxes;
        }

        public bool CanOrder(int boxCount)
        {
            return boxCount >= MinimumBoxes &&
                   boxCount <= MaximumBoxes;
        }

        public Quantity GetOrderedQuantity(int boxCount)
        {
            if (!CanOrder(boxCount))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(boxCount),
                    "Box count is outside supplier limits.");
            }

            return new Quantity(
                checked(UnitsPerBox.Value * boxCount));
        }

        public long GetTotalCostCents(int boxCount)
        {
            Quantity quantity = GetOrderedQuantity(boxCount);

            return checked(
                (long)quantity.Value * UnitCostCents);
        }
    }
}
