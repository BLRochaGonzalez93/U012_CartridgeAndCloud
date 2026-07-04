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
            new RetailProductEntry[0];

        public IReadOnlyList<RetailProductEntry> Entries
        {
            get
            {
                EnsureDefaults();
                return _entries;
            }
        }

        private void OnEnable()
        {
            EnsureDefaults();
        }

        public IReadOnlyList<RetailProductDefinition> BuildDefinitions()
        {
            EnsureDefaults();

            List<RetailProductDefinition> definitions =
                new List<RetailProductDefinition>(_entries.Length);

            foreach (RetailProductEntry entry in _entries)
            {
                if (entry == null)
                {
                    continue;
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
            _entries = entries ?? new RetailProductEntry[0];
        }

        private void EnsureDefaults()
        {
            if (_entries == null || _entries.Length == 0)
            {
                _entries = CreateDefaultProducts();
            }
        }

        private static RetailProductEntry[]
            CreateDefaultProducts()
        {
            return new[]
            {
                CreateProductEntry(
                    "game-neon-drift",
                    "Neon Drift",
                    RetailProductKind.PhysicalGame,
                    1500,
                    2999,
                    12,
                    "product-game",
                    "label-neon-drift",
                    "game-neon-drift"),
                CreateProductEntry(
                    "case-cloud-runner",
                    "Cloud Runner Case",
                    RetailProductKind.GameCase,
                    800,
                    1499,
                    16,
                    "product-case",
                    "label-cloud-runner",
                    "case-cloud-runner"),
                CreateProductEntry(
                    "console-vertex-one",
                    "Vertex One Console",
                    RetailProductKind.Console,
                    18000,
                    24999,
                    2,
                    "product-console",
                    "label-vertex-one",
                    "console-vertex-one"),
                CreateProductEntry(
                    "controller-orbit-pad",
                    "Orbit Pad Controller",
                    RetailProductKind.Controller,
                    3000,
                    4999,
                    6,
                    "product-controller",
                    "label-orbit-pad",
                    "controller-orbit-pad"),
                CreateProductEntry(
                    "headset-signal-pro",
                    "Signal Pro Headset",
                    RetailProductKind.Headset,
                    4500,
                    6999,
                    4,
                    "product-headset",
                    "label-signal-pro",
                    "headset-signal-pro"),
                CreateProductEntry(
                    "accessory-memory-core",
                    "Memory Core Accessory",
                    RetailProductKind.Accessory,
                    900,
                    1999,
                    10,
                    "product-accessory",
                    "label-memory-core",
                    "accessory-memory-core")
            };
        }

        private static RetailProductEntry CreateProductEntry(
            string productId,
            string displayName,
            RetailProductKind kind,
            long wholesalePriceCents,
            long salePriceCents,
            int unitsPerCase,
            string materialVariantId,
            string labelId,
            string resourceId)
        {
            return new RetailProductEntry
            {
                productId = productId,
                displayName = displayName,
                kind = kind,
                wholesalePriceCents =
                    wholesalePriceCents,
                salePriceCents =
                    salePriceCents,
                unitsPerCase = unitsPerCase,
                materialVariantId =
                    materialVariantId,
                labelId = labelId,
                iconResourcePath =
                    "Products/Icons/" +
                    resourceId +
                    "_icon",
                coverResourcePath =
                    "Products/Covers/" +
                    resourceId +
                    "_cover",
                prefabResourcePath =
                    ProductPrefabPath(
                        productId)
            };
        }

        private static string ProductPrefabPath(
            string productId)
        {
            switch (productId)
            {
                case "game-neon-drift":
                    return "Products/NeonDrift";
                case "case-cloud-runner":
                    return "Products/CloudRunnerCase";
                case "console-vertex-one":
                    return "Products/VertexOneConsole";
                case "controller-orbit-pad":
                    return "Products/OrbitPadController";
                case "headset-signal-pro":
                    return "Products/SignalProHeadset";
                default:
                    return "Products/MemoryCoreAccessory";
            }
        }
    }
}
