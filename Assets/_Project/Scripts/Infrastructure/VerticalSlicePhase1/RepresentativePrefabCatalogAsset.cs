using System;
using UnityEngine;

namespace VRMGames.CartridgeAndCloud.Infrastructure.VerticalSlicePhase1
{
    [CreateAssetMenu(
        menuName =
            "Cartridge & Cloud/Presentation/Representative Prefab Catalog",
        fileName =
            "RepresentativePrefabCatalog")]
    public sealed class RepresentativePrefabCatalogAsset :
        ScriptableObject
    {
        [Serializable]
        public sealed class Entry
        {
            public string id;
            public GameObject prefab;
        }

        [SerializeField]
        private Entry[] _architecture =
            Array.Empty<Entry>();

        [SerializeField]
        private Entry[] _furniture =
            Array.Empty<Entry>();

        [SerializeField]
        private Entry[] _products =
            Array.Empty<Entry>();

        [SerializeField]
        private Entry[] _expansions =
            Array.Empty<Entry>();

        public Entry[] Architecture =>
            _architecture;

        public Entry[] Furniture =>
            _furniture;

        public Entry[] Products =>
            _products;

        public Entry[] Expansions =>
            _expansions;

        public void Configure(
            Entry[] architecture,
            Entry[] furniture,
            Entry[] products,
            Entry[] expansions)
        {
            _architecture =
                architecture ??
                Array.Empty<Entry>();
            _furniture =
                furniture ??
                Array.Empty<Entry>();
            _products =
                products ??
                Array.Empty<Entry>();
            _expansions =
                expansions ??
                Array.Empty<Entry>();
        }

        public GameObject FindArchitecture(
            string id)
        {
            return Find(_architecture, id);
        }

        public GameObject FindFurniture(
            string id)
        {
            return Find(_furniture, id);
        }

        public GameObject FindProduct(
            string id)
        {
            return Find(_products, id);
        }

        public GameObject FindExpansion(
            string id)
        {
            return Find(_expansions, id);
        }

        public static RepresentativePrefabCatalogAsset
            FindLoaded()
        {
            Phase1RuntimeAssetRegistryAsset registry =
                Phase1RuntimeAssetRegistryAsset
                    .FindLoaded();

            if (registry != null &&
                registry.RepresentativePrefabs != null)
            {
                return registry
                    .RepresentativePrefabs;
            }

            RepresentativePrefabCatalogAsset[]
                catalogs =
                    Resources.FindObjectsOfTypeAll<
                        RepresentativePrefabCatalogAsset>();

            return catalogs == null ||
                   catalogs.Length == 0
                ? null
                : catalogs[0];
        }

        private static GameObject Find(
            Entry[] entries,
            string id)
        {
            if (entries == null ||
                string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            foreach (Entry entry in entries)
            {
                if (entry != null &&
                    entry.prefab != null &&
                    string.Equals(
                        entry.id,
                        id,
                        StringComparison.Ordinal))
                {
                    return entry.prefab;
                }
            }

            return null;
        }
    }
}
