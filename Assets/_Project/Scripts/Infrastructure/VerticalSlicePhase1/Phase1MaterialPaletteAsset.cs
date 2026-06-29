using System;
using UnityEngine;

namespace VRMGames.CartridgeAndCloud.Infrastructure.VerticalSlicePhase1
{
    [CreateAssetMenu(
        menuName =
            "Cartridge & Cloud/Sprint 16 Phase 1/Material Palette",
        fileName =
            "CC_S16_P1_MaterialPalette")]
    public sealed class Phase1MaterialPaletteAsset :
        ScriptableObject
    {
        private const string ResourceRoot =
            "Sprint16Phase1/Materials/";

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
                    EntryFor("shell-wall", "CC_S16_P1_ShellWall"),
                    EntryFor("shell-glass", "CC_S16_P1_ShellGlass"),
                    EntryFor("shell-floor", "CC_S16_P1_ShellFloor"),
                    EntryFor("zone-marker", "CC_S16_P1_ZoneMarker"),
                    EntryFor("zone-checkout", "CC_S16_P1_ZoneCheckout"),
                    EntryFor("zone-receiving", "CC_S16_P1_ZoneReceiving"),
                    EntryFor("zone-backroom", "CC_S16_P1_ZoneBackroom"),
                    EntryFor("furniture-checkout", "CC_S16_P1_FurnitureCheckout"),
                    EntryFor("furniture-wall-shelf", "CC_S16_P1_FurnitureWallShelf"),
                    EntryFor("furniture-central-shelf", "CC_S16_P1_FurnitureCentralShelf"),
                    EntryFor("furniture-low-display", "CC_S16_P1_FurnitureLowDisplay"),
                    EntryFor("furniture-featured", "CC_S16_P1_FurnitureFeatured"),
                    EntryFor("furniture-storage", "CC_S16_P1_FurnitureStorage"),
                    EntryFor("furniture-crate", "CC_S16_P1_FurnitureCrate"),
                    EntryFor("decoration", "CC_S16_P1_Decoration"),
                    EntryFor("product-game", "CC_S16_P1_ProductGame"),
                    EntryFor("product-case", "CC_S16_P1_ProductCase"),
                    EntryFor("product-console", "CC_S16_P1_ProductConsole"),
                    EntryFor("product-controller", "CC_S16_P1_ProductController"),
                    EntryFor("product-headset", "CC_S16_P1_ProductHeadset"),
                    EntryFor("product-accessory", "CC_S16_P1_ProductAccessory"),
                    EntryFor("character-employee", "CC_S16_P1_CharacterEmployee"),
                    EntryFor("character-customer", "CC_S16_P1_CharacterCustomer"),
                    EntryFor("character-supplier", "CC_S16_P1_CharacterSupplier"),
                    EntryFor("feedback-valid", "CC_S16_P1_FeedbackValid"),
                    EntryFor("feedback-invalid", "CC_S16_P1_FeedbackInvalid"),
                    EntryFor("feedback-warning", "CC_S16_P1_FeedbackWarning")
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
