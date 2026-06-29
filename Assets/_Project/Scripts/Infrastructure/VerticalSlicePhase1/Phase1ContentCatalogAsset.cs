using System;
using System.Collections.Generic;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Application.VerticalSlicePhase1;
using VRMGames.CartridgeAndCloud.Domain.VerticalSlicePhase1;

namespace VRMGames.CartridgeAndCloud.Infrastructure.VerticalSlicePhase1
{
    [CreateAssetMenu(
        menuName =
            "Cartridge & Cloud/Sprint 16 Phase 1/Content Catalog",
        fileName =
            "CC_S16_P1_ContentCatalog")]
    public sealed class Phase1ContentCatalogAsset :
        ScriptableObject
    {
        [Serializable]
        public sealed class FurnitureEntry
        {
            public string definitionId;
            public string displayName;
            public Phase1FurnitureKind kind;
            [Min(1)] public int widthCells = 1;
            [Min(1)] public int depthCells = 1;
            [Min(0.05f)] public float heightMeters = 1f;
            [Min(0)] public int capacity;
            [Min(1)] public long unitCostCents = 100;
            public bool isInteractive = true;
            public bool isPurchasable = true;
            public bool supportsProducts;
            public string materialVariantId;
            public string prefabResourcePath;
        }

        [Serializable]
        public sealed class ProductEntry
        {
            public string productId;
            public string displayName;
            public Phase1ProductKind kind;
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
        private FurnitureEntry[] _furniture =
            new FurnitureEntry[0];

        [SerializeField]
        private ProductEntry[] _products =
            new ProductEntry[0];

        public IReadOnlyList<FurnitureEntry>
            Furniture
        {
            get
            {
                EnsureDefaults();
                return _furniture;
            }
        }

        public IReadOnlyList<ProductEntry>
            Products
        {
            get
            {
                EnsureDefaults();
                return _products;
            }
        }

        private void OnEnable()
        {
            EnsureDefaults();
        }

        public Phase1Catalog BuildCatalog()
        {
            EnsureDefaults();

            List<Phase1FurnitureDefinition>
                furniture =
                    new List<
                        Phase1FurnitureDefinition>();

            List<Phase1ProductDefinition>
                products =
                    new List<
                        Phase1ProductDefinition>();

            foreach (FurnitureEntry entry
                     in _furniture)
            {
                if (entry == null)
                {
                    continue;
                }

                furniture.Add(
                    new Phase1FurnitureDefinition(
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

            foreach (ProductEntry entry
                     in _products)
            {
                if (entry == null)
                {
                    continue;
                }

                products.Add(
                    new Phase1ProductDefinition(
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

            return new Phase1Catalog(
                furniture,
                products);
        }

        public void Configure(
            FurnitureEntry[] furniture,
            ProductEntry[] products)
        {
            _furniture =
                furniture ??
                new FurnitureEntry[0];
            _products =
                products ??
                new ProductEntry[0];
        }

        private void EnsureDefaults()
        {
            if (_furniture == null ||
                _furniture.Length == 0)
            {
                _furniture =
                    CreateDefaultFurniture();
            }

            if (_products == null ||
                _products.Length == 0)
            {
                _products =
                    CreateDefaultProducts();
            }
        }

        private static FurnitureEntry[]
            CreateDefaultFurniture()
        {
            return new[]
            {
                CreateFurnitureEntry(
                    "checkout-counter",
                    "Checkout Counter",
                    Phase1FurnitureKind.CheckoutCounter,
                    4,
                    2,
                    1.1f,
                    0,
                    45000,
                    true,
                    true,
                    false,
                    "furniture-checkout",
                    "Sprint16Phase1/Prefabs/Furniture/CheckoutCounter"),
                CreateFurnitureEntry(
                    "wall-shelf",
                    "Wall Shelf",
                    Phase1FurnitureKind.WallShelf,
                    4,
                    1,
                    2.2f,
                    24,
                    18000,
                    true,
                    true,
                    true,
                    "furniture-wall-shelf",
                    "Sprint16Phase1/Prefabs/Furniture/WallShelf"),
                CreateFurnitureEntry(
                    "central-shelf",
                    "Central Shelf",
                    Phase1FurnitureKind.CentralShelf,
                    4,
                    2,
                    1.6f,
                    32,
                    26000,
                    true,
                    true,
                    true,
                    "furniture-central-shelf",
                    "Sprint16Phase1/Prefabs/Furniture/CentralShelf"),
                CreateFurnitureEntry(
                    "low-display",
                    "Low Display",
                    Phase1FurnitureKind.LowDisplay,
                    3,
                    2,
                    0.9f,
                    12,
                    16000,
                    true,
                    true,
                    true,
                    "furniture-low-display",
                    "Sprint16Phase1/Prefabs/Furniture/LowDisplay"),
                CreateFurnitureEntry(
                    "featured-display",
                    "Featured Display",
                    Phase1FurnitureKind.FeaturedDisplay,
                    2,
                    2,
                    1.1f,
                    8,
                    24000,
                    true,
                    true,
                    true,
                    "furniture-featured",
                    "Sprint16Phase1/Prefabs/Furniture/FeaturedDisplay"),
                CreateFurnitureEntry(
                    "backroom-storage",
                    "Backroom Storage",
                    Phase1FurnitureKind.BackroomStorage,
                    5,
                    2,
                    2.4f,
                    80,
                    22000,
                    true,
                    false,
                    false,
                    "furniture-storage",
                    "Sprint16Phase1/Prefabs/Furniture/BackroomStorage"),
                CreateFurnitureEntry(
                    "receiving-crate",
                    "Receiving Crate",
                    Phase1FurnitureKind.ReceivingCrate,
                    2,
                    2,
                    0.8f,
                    24,
                    3500,
                    true,
                    false,
                    false,
                    "furniture-crate",
                    "Sprint16Phase1/Prefabs/Furniture/ReceivingCrate"),
                CreateFurnitureEntry(
                    "decoration-plant",
                    "Decorative Plant",
                    Phase1FurnitureKind.Decoration,
                    1,
                    1,
                    1.2f,
                    0,
                    2500,
                    false,
                    false,
                    false,
                    "decoration",
                    "Sprint16Phase1/Prefabs/Furniture/DecorationPlant")
            };
        }

        private static ProductEntry[]
            CreateDefaultProducts()
        {
            return new[]
            {
                CreateProductEntry(
                    "game-neon-drift",
                    "Neon Drift",
                    Phase1ProductKind.PhysicalGame,
                    1500,
                    2999,
                    12,
                    "product-game",
                    "label-neon-drift",
                    "game-neon-drift"),
                CreateProductEntry(
                    "case-cloud-runner",
                    "Cloud Runner Case",
                    Phase1ProductKind.GameCase,
                    800,
                    1499,
                    16,
                    "product-case",
                    "label-cloud-runner",
                    "case-cloud-runner"),
                CreateProductEntry(
                    "console-vertex-one",
                    "Vertex One Console",
                    Phase1ProductKind.Console,
                    18000,
                    24999,
                    2,
                    "product-console",
                    "label-vertex-one",
                    "console-vertex-one"),
                CreateProductEntry(
                    "controller-orbit-pad",
                    "Orbit Pad Controller",
                    Phase1ProductKind.Controller,
                    3000,
                    4999,
                    6,
                    "product-controller",
                    "label-orbit-pad",
                    "controller-orbit-pad"),
                CreateProductEntry(
                    "headset-signal-pro",
                    "Signal Pro Headset",
                    Phase1ProductKind.Headset,
                    4500,
                    6999,
                    4,
                    "product-headset",
                    "label-signal-pro",
                    "headset-signal-pro"),
                CreateProductEntry(
                    "accessory-memory-core",
                    "Memory Core Accessory",
                    Phase1ProductKind.Accessory,
                    900,
                    1999,
                    10,
                    "product-accessory",
                    "label-memory-core",
                    "accessory-memory-core")
            };
        }

        private static FurnitureEntry CreateFurnitureEntry(
            string definitionId,
            string displayName,
            Phase1FurnitureKind kind,
            int widthCells,
            int depthCells,
            float heightMeters,
            int capacity,
            long unitCostCents,
            bool isInteractive,
            bool isPurchasable,
            bool supportsProducts,
            string materialVariantId,
            string prefabResourcePath)
        {
            return new FurnitureEntry
            {
                definitionId = definitionId,
                displayName = displayName,
                kind = kind,
                widthCells = widthCells,
                depthCells = depthCells,
                heightMeters = heightMeters,
                capacity = capacity,
                unitCostCents = unitCostCents,
                isInteractive = isInteractive,
                isPurchasable = isPurchasable,
                supportsProducts = supportsProducts,
                materialVariantId =
                    materialVariantId,
                prefabResourcePath =
                    prefabResourcePath
            };
        }

        private static ProductEntry CreateProductEntry(
            string productId,
            string displayName,
            Phase1ProductKind kind,
            long wholesalePriceCents,
            long salePriceCents,
            int unitsPerCase,
            string materialVariantId,
            string labelId,
            string resourceId)
        {
            return new ProductEntry
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
                    "Sprint16Phase1/Icons/" +
                    resourceId +
                    "_icon",
                coverResourcePath =
                    "Sprint16Phase1/Covers/" +
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
                    return "Sprint16Phase1/Prefabs/Products/NeonDrift";
                case "case-cloud-runner":
                    return "Sprint16Phase1/Prefabs/Products/CloudRunnerCase";
                case "console-vertex-one":
                    return "Sprint16Phase1/Prefabs/Products/VertexOneConsole";
                case "controller-orbit-pad":
                    return "Sprint16Phase1/Prefabs/Products/OrbitPadController";
                case "headset-signal-pro":
                    return "Sprint16Phase1/Prefabs/Products/SignalProHeadset";
                default:
                    return "Sprint16Phase1/Prefabs/Products/MemoryCoreAccessory";
            }
        }
    }
}
