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
            Array.Empty<StoreFixtureEntry>();

        public IReadOnlyList<StoreFixtureEntry> Entries =>
            _entries ?? Array.Empty<StoreFixtureEntry>();

        public IReadOnlyList<StoreFixtureDefinition> BuildDefinitions()
        {
            StoreFixtureEntry[] entries =
                _entries ?? Array.Empty<StoreFixtureEntry>();

            List<StoreFixtureDefinition> definitions =
                new List<StoreFixtureDefinition>(entries.Length);

            foreach (StoreFixtureEntry entry in entries)
            {
                if (entry == null)
                {
                    throw new InvalidOperationException(
                        "Store fixture catalog contains a missing entry.");
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
            _entries = entries ?? Array.Empty<StoreFixtureEntry>();
        }
    }
}
