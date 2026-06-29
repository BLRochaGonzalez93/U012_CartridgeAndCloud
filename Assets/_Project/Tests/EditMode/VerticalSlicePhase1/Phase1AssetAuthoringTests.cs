using NUnit.Framework;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Infrastructure.VerticalSlicePhase1;
using VRMGames.CartridgeAndCloud.Domain.VerticalSlicePhase1;
using VRMGames.CartridgeAndCloud.Presentation.Placement;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.VerticalSlicePhase1
{
    public sealed class Phase1AssetAuthoringTests
    {
        private const string Root =
            "Sprint16Phase1/";

        [Test]
        public void ContentCatalog_Loads()
        {
            Assert.That(
                Load<Phase1ContentCatalogAsset>(
                    "CC_S16_P1_ContentCatalog"),
                Is.Not.Null);
        }

        [Test]
        public void ContentCatalog_HasEightFurnitureDefinitions()
        {
            Phase1ContentCatalogAsset asset =
                Load<Phase1ContentCatalogAsset>(
                    "CC_S16_P1_ContentCatalog");

            Assert.That(
                asset.Furniture.Count,
                Is.EqualTo(8));
        }

        [Test]
        public void ContentCatalog_HasSixProducts()
        {
            Phase1ContentCatalogAsset asset =
                Load<Phase1ContentCatalogAsset>(
                    "CC_S16_P1_ContentCatalog");

            Assert.That(
                asset.Products.Count,
                Is.EqualTo(6));
        }

        [Test]
        public void ContentCatalog_CheckoutIsPurchasable()
        {
            var catalog =
                Load<Phase1ContentCatalogAsset>(
                    "CC_S16_P1_ContentCatalog")
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
                    "CC_S16_P1_ContentCatalog")
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
                    "CC_S16_P1_StoreShell");

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
                    "CC_S16_P1_MaterialPalette");

            Assert.That(
                palette.Find("shell-wall"),
                Is.Not.Null);
        }

        [Test]
        public void MaterialPalette_HasPlacementFeedbackMaterials()
        {
            Phase1MaterialPaletteAsset palette =
                Load<Phase1MaterialPaletteAsset>(
                    "CC_S16_P1_MaterialPalette");

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
                    "CC_S16_P1_PresentationCatalog");

            Assert.That(
                asset.Characters.Length,
                Is.EqualTo(3));
        }


        [Test]
        public void PresentationCatalog_HasPlaceholderAnimations()
        {
            Phase1PresentationCatalogAsset asset =
                Load<Phase1PresentationCatalogAsset>(
                    "CC_S16_P1_PresentationCatalog");

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
                    "CC_S16_P1_PresentationCatalog");

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
                    "CC_S16_P1_AudioCatalog");

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
                    "CC_S16_P1_Settings");

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
                Resources.Load<
                    TechnicalPlaceableDefinition>(
                        Root +
                        "PlacementDefinitions/" +
                        "CC_S16_P1_CentralShelf");

            Assert.That(definition, Is.Not.Null);
            Assert.That(
                definition.DefinitionId,
                Is.EqualTo("central-shelf"));
        }

        [Test]
        public void FurniturePrefab_Loads()
        {
            GameObject prefab =
                Resources.Load<GameObject>(
                    Root +
                    "Prefabs/Furniture/" +
                    "CheckoutCounter");

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
                Resources.Load<GameObject>(
                    Root +
                    "Prefabs/Products/" +
                    "NeonDrift");

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
                Resources.Load<GameObject>(
                    Root +
                    "Prefabs/Characters/" +
                    "Customer");

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
                Resources.Load<Texture2D>(
                    Root +
                    "Icons/" +
                    "game-neon-drift_icon");
            Texture2D cover =
                Resources.Load<Texture2D>(
                    Root +
                    "Covers/" +
                    "game-neon-drift_cover");

            Assert.That(icon, Is.Not.Null);
            Assert.That(cover, Is.Not.Null);
        }

        private static T Load<T>(string name)
            where T : Object
        {
            return Resources.Load<T>(
                Root + name);
        }
    }
}
