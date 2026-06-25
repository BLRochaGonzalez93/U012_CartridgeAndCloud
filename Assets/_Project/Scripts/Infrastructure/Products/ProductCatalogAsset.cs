using System;
using System.Collections.Generic;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.Products;

namespace VRMGames.CartridgeAndCloud.Infrastructure.Products
{
    [CreateAssetMenu(
        menuName = "Cartridge & Cloud/Products/Product Catalog",
        fileName = "CC_ProductCatalog_")]
    public sealed class ProductCatalogAsset : ScriptableObject
    {
        [SerializeField]
        private List<ProductDefinitionAsset> _products =
            new List<ProductDefinitionAsset>();

        public IReadOnlyList<ProductDefinitionAsset> Products =>
            _products;

        public ProductDefinitionRegistry BuildRegistry()
        {
            List<ProductDefinition> definitions =
                new List<ProductDefinition>(_products.Count);

            foreach (ProductDefinitionAsset product in _products)
            {
                if (product == null)
                {
                    throw new InvalidOperationException(
                        "Product catalog contains a missing asset reference.");
                }

                definitions.Add(product.BuildDefinition());
            }

            return new ProductDefinitionRegistry(definitions);
        }

        public void Configure(
            IEnumerable<ProductDefinitionAsset> products)
        {
            if (products == null)
            {
                throw new ArgumentNullException(nameof(products));
            }

            _products =
                new List<ProductDefinitionAsset>(products);
        }
    }
}
