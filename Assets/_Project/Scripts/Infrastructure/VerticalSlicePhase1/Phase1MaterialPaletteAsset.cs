using System;
using UnityEngine;

namespace VRMGames.CartridgeAndCloud.Infrastructure.VerticalSlicePhase1
{
    [CreateAssetMenu(
        menuName =
            "Cartridge & Cloud/Runtime/Material Palette",
        fileName =
            "MaterialPalette")]
    public sealed class Phase1MaterialPaletteAsset :
        ScriptableObject
    {
        private const string ResourceRoot =
            "Materials/";

        [Serializable]
        public sealed class Entry
        {
            public string id;
            public Material material;
        }

        [SerializeField]
        private Entry[] _entries =
            new Entry[0];

        private void OnEnable()
        {
            EnsureDefaults();
        }

        public Material Find(string id)
        {
            EnsureDefaults();

            foreach (Entry entry in _entries)
            {
                if (entry != null &&
                    string.Equals(
                        entry.id,
                        id,
                        StringComparison.Ordinal))
                {
                    return entry.material;
                }
            }

            return null;
        }

        public void Configure(Entry[] entries)
        {
            _entries = entries ??
                new Entry[0];
        }

        private void EnsureDefaults()
        {
            if (_entries != null &&
                _entries.Length > 0)
            {
                return;
            }

            _entries =
                new[]
                {
                    EntryFor("shell-wall", "ShellWall"),
                    EntryFor("shell-glass", "ShellGlass"),
                    EntryFor("shell-floor", "ShellFloor"),
                    EntryFor("zone-marker", "ZoneMarker"),
                    EntryFor("zone-checkout", "ZoneCheckout"),
                    EntryFor("zone-receiving", "ZoneReceiving"),
                    EntryFor("zone-backroom", "ZoneBackroom"),
                    EntryFor("furniture-checkout", "FurnitureCheckout"),
                    EntryFor("furniture-wall-shelf", "FurnitureWallShelf"),
                    EntryFor("furniture-central-shelf", "FurnitureCentralShelf"),
                    EntryFor("furniture-low-display", "FurnitureLowDisplay"),
                    EntryFor("furniture-featured", "FurnitureFeatured"),
                    EntryFor("furniture-storage", "FurnitureStorage"),
                    EntryFor("furniture-crate", "FurnitureCrate"),
                    EntryFor("decoration", "Decoration"),
                    EntryFor("product-game", "ProductGame"),
                    EntryFor("product-case", "ProductCase"),
                    EntryFor("product-console", "ProductConsole"),
                    EntryFor("product-controller", "ProductController"),
                    EntryFor("product-headset", "ProductHeadset"),
                    EntryFor("product-accessory", "ProductAccessory"),
                    EntryFor("character-employee", "CharacterEmployee"),
                    EntryFor("character-customer", "CharacterCustomer"),
                    EntryFor("character-supplier", "CharacterSupplier"),
                    EntryFor("feedback-valid", "FeedbackValid"),
                    EntryFor("feedback-invalid", "FeedbackInvalid"),
                    EntryFor("feedback-warning", "FeedbackWarning")
                };
        }

        private static Entry EntryFor(
            string id,
            string resourceName)
        {
            return new Entry
            {
                id = id,
                material =
                    Resources.Load<Material>(
                        ResourceRoot +
                        resourceName)
            };
        }
    }
}
