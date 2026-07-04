using System;
using System.Collections.Generic;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.Store;

namespace VRMGames.CartridgeAndCloud.Infrastructure.Store
{
    [CreateAssetMenu(
        menuName = "Cartridge & Cloud/Store/Store Fixture Catalog",
        fileName = "StoreFixtureCatalog")]
    public sealed class StoreFixtureCatalogAsset : ScriptableObject
    {
        [Serializable]
        public sealed class StoreFixtureEntry
        {
            public string definitionId;
            public string displayName;
            public StoreFixtureKind kind;
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

        [SerializeField]
        private StoreFixtureEntry[] _entries =
            new StoreFixtureEntry[0];

        public IReadOnlyList<StoreFixtureEntry> Entries
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

        public IReadOnlyList<StoreFixtureDefinition> BuildDefinitions()
        {
            EnsureDefaults();

            List<StoreFixtureDefinition> definitions =
                new List<StoreFixtureDefinition>(_entries.Length);

            foreach (StoreFixtureEntry entry in _entries)
            {
                if (entry == null)
                {
                    continue;
                }

                definitions.Add(
                    new StoreFixtureDefinition(
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

            return definitions;
        }

        public void Configure(StoreFixtureEntry[] entries)
        {
            _entries = entries ?? new StoreFixtureEntry[0];
        }

        private void EnsureDefaults()
        {
            if (_entries == null || _entries.Length == 0)
            {
                _entries = CreateDefaultFurniture();
            }
        }

        private static StoreFixtureEntry[]
            CreateDefaultFurniture()
        {
            return new[]
            {
                CreateFurnitureEntry(
                    "checkout-counter",
                    "Checkout Counter",
                    StoreFixtureKind.CheckoutCounter,
                    4,
                    2,
                    1.1f,
                    0,
                    45000,
                    true,
                    true,
                    false,
                    "furniture-checkout",
                    "Furniture/CheckoutCounter"),
                CreateFurnitureEntry(
                    "wall-shelf",
                    "Wall Shelf",
                    StoreFixtureKind.WallShelf,
                    4,
                    1,
                    2.2f,
                    24,
                    18000,
                    true,
                    true,
                    true,
                    "furniture-wall-shelf",
                    "Furniture/WallShelf"),
                CreateFurnitureEntry(
                    "central-shelf",
                    "Central Shelf",
                    StoreFixtureKind.CentralShelf,
                    4,
                    2,
                    1.6f,
                    32,
                    26000,
                    true,
                    true,
                    true,
                    "furniture-central-shelf",
                    "Furniture/CentralShelf"),
                CreateFurnitureEntry(
                    "low-display",
                    "Low Display",
                    StoreFixtureKind.LowDisplay,
                    3,
                    2,
                    0.9f,
                    12,
                    16000,
                    true,
                    true,
                    true,
                    "furniture-low-display",
                    "Furniture/LowDisplay"),
                CreateFurnitureEntry(
                    "featured-display",
                    "Featured Display",
                    StoreFixtureKind.FeaturedDisplay,
                    2,
                    2,
                    1.1f,
                    8,
                    24000,
                    true,
                    true,
                    true,
                    "furniture-featured",
                    "Furniture/FeaturedDisplay"),
                CreateFurnitureEntry(
                    "backroom-storage",
                    "Backroom Storage",
                    StoreFixtureKind.BackroomStorage,
                    5,
                    2,
                    2.4f,
                    80,
                    22000,
                    true,
                    false,
                    false,
                    "furniture-storage",
                    "Furniture/BackroomStorage"),
                CreateFurnitureEntry(
                    "receiving-crate",
                    "Receiving Crate",
                    StoreFixtureKind.ReceivingCrate,
                    2,
                    2,
                    0.8f,
                    24,
                    3500,
                    true,
                    false,
                    false,
                    "furniture-crate",
                    "Furniture/ReceivingCrate"),
                CreateFurnitureEntry(
                    "decoration-plant",
                    "Decorative Plant",
                    StoreFixtureKind.Decoration,
                    1,
                    1,
                    1.2f,
                    0,
                    2500,
                    false,
                    false,
                    false,
                    "decoration",
                    "Furniture/DecorationPlant")
            };
        }

        private static StoreFixtureEntry CreateFurnitureEntry(
            string definitionId,
            string displayName,
            StoreFixtureKind kind,
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
            return new StoreFixtureEntry
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
    }
}
