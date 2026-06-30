using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.VerticalSlicePhase1;
using VRMGames.CartridgeAndCloud.Infrastructure.VerticalSlicePhase1;
using VRMGames.CartridgeAndCloud.Presentation.Placement;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.VerticalSlicePhase1
{
    public sealed class Phase1AssetAuthoringTests
    {
        private const string CatalogRoot =
            "Assets/_Project/Data/Catalogs/";
        private const string StoreRoot =
            "Assets/_Project/Data/Store/";
        private const string SettingsRoot =
            "Assets/_Project/Settings/Runtime/";
        private const string DefinitionRoot =
            "Assets/_Project/Data/Placement/Definitions/";
        private const string PrefabRoot =
            "Assets/_Project/Prefabs/";
        private const string ProductTextureRoot =
            "Assets/_Project/Art/Textures/Products/";

        [Test]
        public void ContentCatalog_Loads()
        {
            Assert.That(
                Load<Phase1ContentCatalogAsset>(
                    CatalogRoot +
                    "ContentCatalog.asset"),
                Is.Not.Null);

            Phase1RuntimeAssetRegistryAsset registry =
                Load<Phase1RuntimeAssetRegistryAsset>(
                    SettingsRoot +
                    "RuntimeAssetRegistry.asset");

            Assert.That(registry, Is.Not.Null);
            Assert.That(registry.Settings, Is.Not.Null);
            Assert.That(
                registry.ContentCatalog,
                Is.Not.Null);
            Assert.That(registry.StoreShell, Is.Not.Null);
            Assert.That(
                registry.MaterialPalette,
                Is.Not.Null);
            Assert.That(
                registry.PresentationCatalog,
                Is.Not.Null);
            Assert.That(registry.AudioCatalog, Is.Not.Null);
        }

        [Test]
        public void ContentCatalog_HasEightFurnitureDefinitions()
        {
            Phase1ContentCatalogAsset asset =
                Load<Phase1ContentCatalogAsset>(
                    CatalogRoot +
                    "ContentCatalog.asset");

            Assert.That(
                asset.Furniture.Count,
                Is.EqualTo(8));
        }

        [Test]
        public void ContentCatalog_HasSixProducts()
        {
            Phase1ContentCatalogAsset asset =
                Load<Phase1ContentCatalogAsset>(
                    CatalogRoot +
                    "ContentCatalog.asset");

            Assert.That(
                asset.Products.Count,
                Is.EqualTo(6));
        }

        [Test]
        public void ContentCatalog_CheckoutIsPurchasable()
        {
            var catalog =
                Load<Phase1ContentCatalogAsset>(
                    CatalogRoot +
                    "ContentCatalog.asset")
                    .BuildCatalog();

            Assert.That(
                catalog.TryGetFurniture(
                    "checkout-counter",
                    out var definition),
                Is.True);
            Assert.That(
                definition.IsPurchasable,
                Is.True);
        }

        [Test]
        public void ContentCatalog_BackroomStorageIsNotPurchasable()
        {
            var catalog =
                Load<Phase1ContentCatalogAsset>(
                    CatalogRoot +
                    "ContentCatalog.asset")
                    .BuildCatalog();

            catalog.TryGetFurniture(
                "backroom-storage",
                out var definition);

            Assert.That(
                definition.IsPurchasable,
                Is.False);
        }

        [Test]
        public void StoreShell_UsesTwentyByThirtyCells()
        {
            Phase1StoreShellAsset shell =
                Load<Phase1StoreShellAsset>(
                    StoreRoot +
                    "StoreShell.asset");

            Assert.That(
                shell.WidthCells,
                Is.EqualTo(20));
            Assert.That(
                shell.DepthCells,
                Is.EqualTo(30));
            Assert.That(
                shell.CellSize,
                Is.EqualTo(0.5f));
        }

        [Test]
        public void MaterialPalette_HasShellWall()
        {
            Phase1MaterialPaletteAsset palette =
                Load<Phase1MaterialPaletteAsset>(
                    CatalogRoot +
                    "MaterialPalette.asset");

            Assert.That(
                palette.Find("shell-wall"),
                Is.Not.Null);
        }

        [Test]
        public void MaterialPalette_HasPlacementFeedbackMaterials()
        {
            Phase1MaterialPaletteAsset palette =
                Load<Phase1MaterialPaletteAsset>(
                    CatalogRoot +
                    "MaterialPalette.asset");

            Assert.That(
                palette.Find("feedback-valid"),
                Is.Not.Null);
            Assert.That(
                palette.Find("feedback-invalid"),
                Is.Not.Null);
        }

        [Test]
        public void PresentationCatalog_HasCharacters()
        {
            Phase1PresentationCatalogAsset asset =
                Load<Phase1PresentationCatalogAsset>(
                    CatalogRoot +
                    "PresentationCatalog.asset");

            Assert.That(
                asset.Characters.Length,
                Is.EqualTo(3));
        }

        [Test]
        public void PresentationCatalog_HasPlaceholderAnimations()
        {
            Phase1PresentationCatalogAsset asset =
                Load<Phase1PresentationCatalogAsset>(
                    CatalogRoot +
                    "PresentationCatalog.asset");

            Assert.That(
                asset.Animations.Length,
                Is.EqualTo(11));

            foreach (Phase1PresentationCatalogAsset
                     .AnimationEntry animation
                     in asset.Animations)
            {
                Assert.That(
                    animation.placeholderClip,
                    Is.Not.Null);
            }
        }

        [Test]
        public void PresentationCatalog_HasRestockFeedback()
        {
            Phase1PresentationCatalogAsset asset =
                Load<Phase1PresentationCatalogAsset>(
                    CatalogRoot +
                    "PresentationCatalog.asset");

            Assert.That(
                asset.FindFeedback(
                    Phase1FeedbackKind.Restocked),
                Is.Not.Null);
        }

        [Test]
        public void AudioCatalog_HasStoreMusic()
        {
            Phase1AudioCatalogAsset asset =
                Load<Phase1AudioCatalogAsset>(
                    CatalogRoot +
                    "AudioCatalog.asset");

            Assert.That(
                asset.Find("music.store"),
                Is.Not.Null);
            Assert.That(
                asset.Find("music.store").clip,
                Is.Not.Null);
        }

        [Test]
        public void Settings_EnableBlockout()
        {
            Phase1SettingsAsset settings =
                Load<Phase1SettingsAsset>(
                    SettingsRoot +
                    "StoreRuntimeSettings.asset");

            Assert.That(
                settings.BuildBlockoutOnLoad,
                Is.True);
            Assert.That(
                settings.HideOccludingWalls,
                Is.True);
        }

        [Test]
        public void PlacementDefinitions_Load()
        {
            TechnicalPlaceableDefinition definition =
                Load<TechnicalPlaceableDefinition>(
                    DefinitionRoot +
                    "CentralShelf.asset");

            Assert.That(definition, Is.Not.Null);
            Assert.That(
                definition.DefinitionId,
                Is.EqualTo("central-shelf"));
        }

        [Test]
        public void FurniturePrefab_Loads()
        {
            GameObject prefab =
                Load<GameObject>(
                    PrefabRoot +
                    "Furniture/CheckoutCounter.prefab");

            Assert.That(prefab, Is.Not.Null);
            Assert.That(
                prefab.GetComponent<
                    Phase1FurniturePrefabAuthoring>(),
                Is.Not.Null);
        }

        [Test]
        public void ProductPrefab_Loads()
        {
            GameObject prefab =
                Load<GameObject>(
                    PrefabRoot +
                    "Products/NeonDrift.prefab");

            Assert.That(prefab, Is.Not.Null);
            Assert.That(
                prefab.GetComponent<
                    Phase1ProductPrefabAuthoring>(),
                Is.Not.Null);
        }

        [Test]
        public void CharacterPrefab_Loads()
        {
            GameObject prefab =
                Load<GameObject>(
                    PrefabRoot +
                    "Characters/Customer.prefab");

            Assert.That(prefab, Is.Not.Null);
            Assert.That(
                prefab.GetComponent<
                    Phase1CharacterPrefabAuthoring>(),
                Is.Not.Null);
        }

        [Test]
        public void ProductIconAndCover_Load()
        {
            Texture2D icon =
                Load<Texture2D>(
                    ProductTextureRoot +
                    "Icons/game-neon-drift_icon.png");
            Texture2D cover =
                Load<Texture2D>(
                    ProductTextureRoot +
                    "Covers/game-neon-drift_cover.png");

            Assert.That(icon, Is.Not.Null);
            Assert.That(cover, Is.Not.Null);
        }


        private static T Load<T>(string path)
            where T : Object
        {
            return AssetDatabase.LoadAssetAtPath<T>(
                path);
        }
    }
}
