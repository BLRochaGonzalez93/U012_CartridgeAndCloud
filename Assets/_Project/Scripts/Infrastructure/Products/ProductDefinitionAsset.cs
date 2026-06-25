using System;
using System.Collections.Generic;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.Products;

namespace VRMGames.CartridgeAndCloud.Infrastructure.Products
{
    [CreateAssetMenu(
        menuName = "Cartridge & Cloud/Products/Product Definition",
        fileName = "CC_Product_")]
    public sealed class ProductDefinitionAsset : ScriptableObject
    {
        [SerializeField]
        private string _productId;

        [SerializeField]
        private string _displayNameKey;

        [SerializeField]
        private string _categoryId;

        [SerializeField]
        private List<string> _tags = new List<string>();

        [SerializeField]
        private Sprite _icon;

        [SerializeField]
        private GameObject _productPrefab;

        public string ProductId => _productId;

        public string DisplayNameKey => _displayNameKey;

        public string CategoryId => _categoryId;

        public IReadOnlyList<string> Tags => _tags;

        public Sprite Icon => _icon;

        public GameObject ProductPrefab => _productPrefab;

        public ProductDefinition BuildDefinition()
        {
            List<ProductTagId> tags =
                new List<ProductTagId>(_tags.Count);

            foreach (string tag in _tags)
            {
                tags.Add(new ProductTagId(tag));
            }

            return new ProductDefinition(
                new ProductDefinitionId(_productId),
                _displayNameKey,
                new ProductCategoryId(_categoryId),
                tags);
        }

        public void Configure(
            string productId,
            string displayNameKey,
            string categoryId,
            IEnumerable<string> tags,
            Sprite icon = null,
            GameObject productPrefab = null)
        {
            if (tags == null)
            {
                throw new ArgumentNullException(nameof(tags));
            }

            _productId = productId;
            _displayNameKey = displayNameKey;
            _categoryId = categoryId;
            _tags = new List<string>(tags);
            _icon = icon;
            _productPrefab = productPrefab;
        }
    }
}
