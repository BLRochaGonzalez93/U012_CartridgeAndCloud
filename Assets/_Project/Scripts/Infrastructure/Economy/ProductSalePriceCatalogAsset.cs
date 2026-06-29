using System;
using System.Collections.Generic;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.Economy;
using VRMGames.CartridgeAndCloud.Domain.Products;

namespace VRMGames.CartridgeAndCloud.Infrastructure.Economy
{
    [CreateAssetMenu(
        menuName =
            "Cartridge & Cloud/Economy/Product Sale Price Catalog",
        fileName = "CC_ProductSalePriceCatalog_")]
    public sealed class ProductSalePriceCatalogAsset :
        ScriptableObject
    {
        [Serializable]
        private sealed class Entry
        {
            [SerializeField]
            private string _productId;

            [SerializeField, Min(1)]
            private long _unitSalePriceCents = 1;

            public string ProductId => _productId;

            public long UnitSalePriceCents =>
                _unitSalePriceCents;

            public Entry(
                string productId,
                long unitSalePriceCents)
            {
                _productId = productId;
                _unitSalePriceCents =
                    unitSalePriceCents;
            }
        }

        [SerializeField]
        private List<Entry> _entries =
            new List<Entry>();

        public int Count => _entries.Count;

        public ProductSalePriceCatalog BuildCatalog(
            CurrencyCode currency)
        {
            List<ProductSalePrice> prices =
                new List<ProductSalePrice>();

            foreach (Entry entry in _entries)
            {
                if (entry == null)
                {
                    throw new InvalidOperationException(
                        "Sale-price entries cannot contain null.");
                }

                prices.Add(
                    new ProductSalePrice(
                        new ProductDefinitionId(
                            entry.ProductId),
                        new Money(
                            entry.UnitSalePriceCents,
                            currency)));
            }

            return new ProductSalePriceCatalog(
                currency,
                prices);
        }

        public void Configure(
            IReadOnlyList<string> productIds,
            IReadOnlyList<long> unitSalePricesCents)
        {
            if (productIds == null)
                throw new ArgumentNullException(
                    nameof(productIds));
            if (unitSalePricesCents == null)
                throw new ArgumentNullException(
                    nameof(unitSalePricesCents));
            if (productIds.Count !=
                unitSalePricesCents.Count)
            {
                throw new ArgumentException(
                    "Product IDs and prices must have the same count.");
            }

            _entries = new List<Entry>();

            for (int index = 0;
                 index < productIds.Count;
                 index++)
            {
                _entries.Add(
                    new Entry(
                        productIds[index],
                        unitSalePricesCents[index]));
            }
        }
    }
}
