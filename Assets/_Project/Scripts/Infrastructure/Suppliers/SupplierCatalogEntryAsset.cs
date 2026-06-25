using System;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.Inventory;
using VRMGames.CartridgeAndCloud.Domain.Suppliers;
using VRMGames.CartridgeAndCloud.Infrastructure.Products;

namespace VRMGames.CartridgeAndCloud.Infrastructure.Suppliers
{
    [Serializable]
    public sealed class SupplierCatalogEntryAsset
    {
        [SerializeField]
        private ProductDefinitionAsset _product;

        [SerializeField]
        private int _unitCostCents = 100;

        [SerializeField]
        private int _unitsPerBox = 1;

        [SerializeField]
        private int _minimumBoxes = 1;

        [SerializeField]
        private int _maximumBoxes = 10;

        public ProductDefinitionAsset Product => _product;

        public int UnitCostCents => _unitCostCents;

        public int UnitsPerBox => _unitsPerBox;

        public int MinimumBoxes => _minimumBoxes;

        public int MaximumBoxes => _maximumBoxes;

        public SupplierCatalogEntry BuildEntry()
        {
            if (_product == null)
            {
                throw new InvalidOperationException(
                    "Supplier entry has no product asset.");
            }

            return new SupplierCatalogEntry(
                _product.BuildDefinition().Id,
                _unitCostCents,
                new Quantity(_unitsPerBox),
                _minimumBoxes,
                _maximumBoxes);
        }

        public void Configure(
            ProductDefinitionAsset product,
            int unitCostCents,
            int unitsPerBox,
            int minimumBoxes,
            int maximumBoxes)
        {
            _product = product;
            _unitCostCents = unitCostCents;
            _unitsPerBox = unitsPerBox;
            _minimumBoxes = minimumBoxes;
            _maximumBoxes = maximumBoxes;
        }
    }
}
