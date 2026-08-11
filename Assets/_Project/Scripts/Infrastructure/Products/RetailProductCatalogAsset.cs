using System;
using System.Collections.Generic;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.Products;

namespace VRMGames.CartridgeAndCloud.Infrastructure.Products
{
    [CreateAssetMenu(
        menuName = "Cartridge & Cloud/Products/Retail Product Catalog",
        fileName = "RetailProductCatalog")]
    public sealed class RetailProductCatalogAsset : ScriptableObject
    {
        [Serializable]
        public sealed class RetailProductEntry
        {
            public string productId;
            public string displayName;
            public RetailProductKind kind;
            [Min(1)] public long wholesalePriceCents = 100;
            [Min(2)] public long salePriceCents = 200;
            [Min(1)] public int unitsPerCase = 1;
            public string materialVariantId;
            public string labelId;
            public string iconResourcePath;
            public string coverResourcePath;
            public string prefabResourcePath;
        }

        [SerializeField]
        private RetailProductEntry[] _entries =
            Array.Empty<RetailProductEntry>();

        public IReadOnlyList<RetailProductEntry> Entries =>
            _entries ?? Array.Empty<RetailProductEntry>();

        public IReadOnlyList<RetailProductDefinition> BuildDefinitions()
        {
            RetailProductEntry[] entries =
                _entries ?? Array.Empty<RetailProductEntry>();

            List<RetailProductDefinition> definitions =
                new List<RetailProductDefinition>(entries.Length);

            foreach (RetailProductEntry entry in entries)
            {
                if (entry == null)
                {
                    throw new InvalidOperationException(
                        "Retail product catalog contains a missing entry.");
                }

                definitions.Add(
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

            return definitions;
        }

        public void Configure(RetailProductEntry[] entries)
        {
            _entries = entries ?? Array.Empty<RetailProductEntry>();
        }
    }
}
