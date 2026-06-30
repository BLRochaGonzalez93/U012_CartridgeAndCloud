using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.VerticalSlicePhase1;
using VRMGames.CartridgeAndCloud.Infrastructure.VerticalSlicePhase1;
using VRMGames.CartridgeAndCloud.Presentation.Placement;

namespace VRMGames.CartridgeAndCloud.Editor.ProjectOrganization
{
    public static class ProjectAssetOrganizationMigration
    {
        private const string SourceRoot =
            "Assets/_Project/Resources/Sprint16Phase1";

        private const string CatalogRoot =
            "Assets/_Project/Data/Catalogs";
        private const string StoreRoot =
            "Assets/_Project/Data/Store";
        private const string SettingsRoot =
            "Assets/_Project/Settings/Runtime";
        private const string DefinitionRoot =
            "Assets/_Project/Data/Placement/Definitions";

        private const string RuntimeRootScript =
            "Assets/_Project/Scripts/Runtime/" +
            "VerticalSlicePhase1/" +
            "Sprint16Phase1RuntimeRoot.cs";

        private const string ContentCatalogScript =
            "Assets/_Project/Scripts/Infrastructure/" +
            "VerticalSlicePhase1/" +
            "Phase1ContentCatalogAsset.cs";

        private const string MaterialPaletteScript =
            "Assets/_Project/Scripts/Infrastructure/" +
            "VerticalSlicePhase1/" +
            "Phase1MaterialPaletteAsset.cs";

        private const string PresentationCatalogScript =
            "Assets/_Project/Scripts/Infrastructure/" +
            "VerticalSlicePhase1/" +
            "Phase1PresentationCatalogAsset.cs";

        private const string AudioCatalogScript =
            "Assets/_Project/Scripts/Infrastructure/" +
            "VerticalSlicePhase1/" +
            "Phase1AudioCatalogAsset.cs";

        [MenuItem(
            "Tools/Cartridge & Cloud/" +
            "Project Organization/" +
            "Migrate Legacy Resources")]
        public static void Migrate()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                EditorUtility.DisplayDialog(
                    "Project Organization",
                    "Exit Play Mode before running the migration.",
                    "OK");
                return;
            }

            bool proceed =
                EditorUtility.DisplayDialog(
                    "Migrate legacy Resources",
                    "This operation moves and renames project assets " +
                    "while preserving their GUIDs. It also updates " +
                    "catalog data, preloaded assets and runtime loading.\n\n" +
                    "A clean Git commit should exist before continuing.",
                    "Migrate",
                    "Cancel");

            if (!proceed)
            {
                return;
            }

            try
            {
                AssetDatabase.DisallowAutoRefresh();

                EnsureDestinationFolders();
                MoveAssets();

                AssetDatabase.AllowAutoRefresh();
                AssetDatabase.Refresh(
                    ImportAssetOptions.ForceSynchronousImport);

                ConfigureContentCatalog();
                ConfigureMaterialPalette();
                ConfigureAudioCatalog();
                ConfigurePresentationCatalog();
                ConfigureRuntimeRegistry();

                DeleteEmptySourceFolder();

                AssetDatabase.SaveAssets();

                PatchRuntimeLoading();
                PatchGenericAssetNames();

                Debug.Log(
                    "[Project Organization] Migration completed. " +
                    "Run EditMode and PlayMode tests before committing.");

                EditorUtility.DisplayDialog(
                    "Project Organization",
                    "Migration completed.\n\n" +
                    "Unity will now recompile the patched runtime.\n\n" +
                    "Next steps:\n" +
                    "1. Wait for compilation.\n" +
                    "2. Run Phase1AssetAuthoringTests.\n" +
                    "3. Run full EditMode and PlayMode.\n" +
                    "4. Inspect GitHub Desktop before committing.",
                    "OK");

                AssetDatabase.Refresh(
                    ImportAssetOptions.ForceSynchronousImport);
            }
            catch (Exception exception)
            {
                AssetDatabase.AllowAutoRefresh();
                AssetDatabase.Refresh();

                Debug.LogException(exception);

                EditorUtility.DisplayDialog(
                    "Migration failed",
                    exception.Message +
                    "\n\nNo commit or push was performed.",
                    "OK");
            }
        }

        [MenuItem(
            "Tools/Cartridge & Cloud/" +
            "Project Organization/" +
            "Validate Organized Assets")]
        public static void ValidateOrganizedAssets()
        {
            List<string> errors =
                new List<string>();

            foreach (MoveEntry entry in Entries())
            {
                if (AssetDatabase.LoadMainAssetAtPath(
                        entry.Destination) == null)
                {
                    errors.Add(
                        "Missing: " +
                        entry.Destination);
                }
            }

            Phase1RuntimeAssetRegistryAsset registry =
                AssetDatabase.LoadAssetAtPath<
                    Phase1RuntimeAssetRegistryAsset>(
                        SettingsRoot +
                        "/RuntimeAssetRegistry.asset");

            if (registry == null ||
                registry.Settings == null ||
                registry.ContentCatalog == null ||
                registry.StoreShell == null ||
                registry.MaterialPalette == null ||
                registry.PresentationCatalog == null ||
                registry.AudioCatalog == null)
            {
                errors.Add(
                    "RuntimeAssetRegistry is missing references.");
            }

            bool legacyFolderExists =
                AssetDatabase.IsValidFolder(
                    SourceRoot);

            if (legacyFolderExists)
            {
                string absoluteSource =
                    AbsolutePath(SourceRoot);

                string[] remainingFiles =
                    Directory.Exists(
                        absoluteSource)
                        ? Directory.GetFiles(
                            absoluteSource,
                            "*",
                            SearchOption.AllDirectories)
                        : Array.Empty<string>();

                if (remainingFiles.Any(
                        path =>
                            !path.EndsWith(
                                ".meta",
                                StringComparison.OrdinalIgnoreCase)))
                {
                    errors.Add(
                        "Legacy Resources folder still contains assets.");
                }
            }

            if (errors.Count == 0)
            {
                Debug.Log(
                    "[Project Organization] Validation PASS.");
                EditorUtility.DisplayDialog(
                    "Project Organization",
                    "Validation PASS.",
                    "OK");
                return;
            }

            string detail =
                string.Join("\n", errors);

            Debug.LogError(
                "[Project Organization] Validation failed:\n" +
                detail);

            EditorUtility.DisplayDialog(
                "Project Organization",
                "Validation failed:\n\n" +
                detail,
                "OK");
        }

        private static void EnsureDestinationFolders()
        {
            HashSet<string> folders =
                new HashSet<string>(
                    StringComparer.Ordinal);

            foreach (MoveEntry entry in Entries())
            {
                string folder =
                    Path.GetDirectoryName(
                        entry.Destination)
                        ?.Replace("\\", "/");

                if (!string.IsNullOrWhiteSpace(folder))
                {
                    folders.Add(folder);
                }
            }

            folders.Add(CatalogRoot);
            folders.Add(StoreRoot);
            folders.Add(SettingsRoot);
            folders.Add(DefinitionRoot);
            folders.Add(
                "Assets/_Project/Art/Materials/Characters");
            folders.Add(
                "Assets/_Project/Art/Materials/Feedback");
            folders.Add(
                "Assets/_Project/Art/Textures/Products/Icons");
            folders.Add(
                "Assets/_Project/Art/Textures/Products/Covers");
            folders.Add(
                "Assets/_Project/Animations/Characters");
            folders.Add(
                "Assets/_Project/Audio/Ambience");
            folders.Add(
                "Assets/_Project/Audio/Music");
            folders.Add(
                "Assets/_Project/Audio/SFX");
            folders.Add(
                "Assets/_Project/Audio/UI");
            folders.Add(
                "Assets/_Project/Prefabs/Characters");

            foreach (string folder in folders
                         .OrderBy(
                             path =>
                                 path.Count(
                                     character =>
                                         character == '/')))
            {
                EnsureFolder(folder);
            }
        }

        private static void EnsureFolder(
            string folderPath)
        {
            if (AssetDatabase.IsValidFolder(
                    folderPath))
            {
                return;
            }

            string normalized =
                folderPath.Replace("\\", "/");
            string[] segments =
                normalized.Split('/');

            if (segments.Length == 0 ||
                !string.Equals(
                    segments[0],
                    "Assets",
                    StringComparison.Ordinal))
            {
                throw new InvalidOperationException(
                    "Folder must be inside Assets: " +
                    folderPath);
            }

            string current = "Assets";

            for (int index = 1;
                 index < segments.Length;
                 index++)
            {
                string next =
                    current + "/" +
                    segments[index];

                if (!AssetDatabase.IsValidFolder(
                        next))
                {
                    AssetDatabase.CreateFolder(
                        current,
                        segments[index]);
                }

                current = next;
            }
        }

        private static void MoveAssets()
        {
            AssetDatabase.StartAssetEditing();

            try
            {
                foreach (MoveEntry entry in Entries())
                {
                    MoveAsset(entry);
                }
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
            }
        }

        private static void MoveAsset(
            MoveEntry entry)
        {
            UnityEngine.Object source =
                AssetDatabase.LoadMainAssetAtPath(
                    entry.Source);
            UnityEngine.Object destination =
                AssetDatabase.LoadMainAssetAtPath(
                    entry.Destination);

            if (source == null)
            {
                if (destination != null)
                {
                    return;
                }

                throw new FileNotFoundException(
                    "Source asset is missing.",
                    entry.Source);
            }

            if (destination != null)
            {
                throw new InvalidOperationException(
                    "Destination already exists: " +
                    entry.Destination);
            }

            string destinationFolder =
                Path.GetDirectoryName(
                    entry.Destination)
                    ?.Replace("\\", "/");

            EnsureFolder(destinationFolder);

            string error =
                AssetDatabase.MoveAsset(
                    entry.Source,
                    entry.Destination);

            if (!string.IsNullOrWhiteSpace(error))
            {
                throw new InvalidOperationException(
                    "Could not move " +
                    entry.Source +
                    " -> " +
                    entry.Destination +
                    ": " +
                    error);
            }
        }

        private static void ConfigureContentCatalog()
        {
            Phase1ContentCatalogAsset asset =
                LoadRequired<
                    Phase1ContentCatalogAsset>(
                        CatalogRoot +
                        "/ContentCatalog.asset");

            Phase1ContentCatalogAsset.FurnitureEntry[]
                furniture =
                {
                    Furniture(
                        "checkout-counter",
                        "Checkout Counter",
                        Phase1FurnitureKind.CheckoutCounter,
                        4, 2, 1.1f, 0, 45000,
                        true, true, false,
                        "furniture-checkout",
                        "Furniture/CheckoutCounter"),
                    Furniture(
                        "wall-shelf",
                        "Wall Shelf",
                        Phase1FurnitureKind.WallShelf,
                        4, 1, 2.2f, 24, 18000,
                        true, true, true,
                        "furniture-wall-shelf",
                        "Furniture/WallShelf"),
                    Furniture(
                        "central-shelf",
                        "Central Shelf",
                        Phase1FurnitureKind.CentralShelf,
                        4, 2, 1.6f, 32, 26000,
                        true, true, true,
                        "furniture-central-shelf",
                        "Furniture/CentralShelf"),
                    Furniture(
                        "low-display",
                        "Low Display",
                        Phase1FurnitureKind.LowDisplay,
                        3, 2, 0.9f, 12, 16000,
                        true, true, true,
                        "furniture-low-display",
                        "Furniture/LowDisplay"),
                    Furniture(
                        "featured-display",
                        "Featured Display",
                        Phase1FurnitureKind.FeaturedDisplay,
                        2, 2, 1.1f, 8, 24000,
                        true, true, true,
                        "furniture-featured",
                        "Furniture/FeaturedDisplay"),
                    Furniture(
                        "backroom-storage",
                        "Backroom Storage",
                        Phase1FurnitureKind.BackroomStorage,
                        5, 2, 2.4f, 80, 22000,
                        true, false, false,
                        "furniture-storage",
                        "Furniture/BackroomStorage"),
                    Furniture(
                        "receiving-crate",
                        "Receiving Crate",
                        Phase1FurnitureKind.ReceivingCrate,
                        2, 2, 0.8f, 24, 3500,
                        true, false, false,
                        "furniture-crate",
                        "Furniture/ReceivingCrate"),
                    Furniture(
                        "decoration-plant",
                        "Decorative Plant",
                        Phase1FurnitureKind.Decoration,
                        1, 1, 1.2f, 0, 2500,
                        false, false, false,
                        "decoration",
                        "Furniture/DecorationPlant")
                };

            Phase1ContentCatalogAsset.ProductEntry[]
                products =
                {
                    Product(
                        "game-neon-drift",
                        "Neon Drift",
                        Phase1ProductKind.PhysicalGame,
                        1500, 2999, 12,
                        "product-game",
                        "label-neon-drift",
                        "NeonDrift"),
                    Product(
                        "case-cloud-runner",
                        "Cloud Runner Case",
                        Phase1ProductKind.GameCase,
                        800, 1499, 16,
                        "product-case",
                        "label-cloud-runner",
                        "CloudRunnerCase"),
                    Product(
                        "console-vertex-one",
                        "Vertex One Console",
                        Phase1ProductKind.Console,
                        18000, 24999, 2,
                        "product-console",
                        "label-vertex-one",
                        "VertexOneConsole"),
                    Product(
                        "controller-orbit-pad",
                        "Orbit Pad Controller",
                        Phase1ProductKind.Controller,
                        3000, 4999, 6,
                        "product-controller",
                        "label-orbit-pad",
                        "OrbitPadController"),
                    Product(
                        "headset-signal-pro",
                        "Signal Pro Headset",
                        Phase1ProductKind.Headset,
                        4500, 6999, 4,
                        "product-headset",
                        "label-signal-pro",
                        "SignalProHeadset"),
                    Product(
                        "accessory-memory-core",
                        "Memory Core Accessory",
                        Phase1ProductKind.Accessory,
                        900, 1999, 10,
                        "product-accessory",
                        "label-memory-core",
                        "MemoryCoreAccessory")
                };

            asset.Configure(
                furniture,
                products);

            EditorUtility.SetDirty(asset);
        }

        private static Phase1ContentCatalogAsset
            .FurnitureEntry Furniture(
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
                string prefabPath)
        {
            return new Phase1ContentCatalogAsset
                .FurnitureEntry
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
                    supportsProducts =
                        supportsProducts,
                    materialVariantId =
                        materialVariantId,
                    prefabResourcePath =
                        prefabPath
                };
        }

        private static Phase1ContentCatalogAsset
            .ProductEntry Product(
                string productId,
                string displayName,
                Phase1ProductKind kind,
                long wholesalePriceCents,
                long salePriceCents,
                int unitsPerCase,
                string materialVariantId,
                string labelId,
                string prefabName)
        {
            string resourceId =
                ProductResourceId(productId);

            return new Phase1ContentCatalogAsset
                .ProductEntry
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
                        "Products/" +
                        prefabName
                };
        }

        private static string ProductResourceId(
            string productId)
        {
            return productId;
        }

        private static void ConfigureMaterialPalette()
        {
            Phase1MaterialPaletteAsset asset =
                LoadRequired<
                    Phase1MaterialPaletteAsset>(
                        CatalogRoot +
                        "/MaterialPalette.asset");

            Phase1MaterialPaletteAsset.Entry[]
                entries =
                {
                    MaterialEntry("shell-wall", "Architecture/ShellWall.mat"),
                    MaterialEntry("shell-glass", "Architecture/ShellGlass.mat"),
                    MaterialEntry("shell-floor", "Architecture/ShellFloor.mat"),
                    MaterialEntry("zone-marker", "Architecture/ZoneMarker.mat"),
                    MaterialEntry("zone-checkout", "Architecture/ZoneCheckout.mat"),
                    MaterialEntry("zone-receiving", "Architecture/ZoneReceiving.mat"),
                    MaterialEntry("zone-backroom", "Architecture/ZoneBackroom.mat"),
                    MaterialEntry("furniture-checkout", "Furniture/FurnitureCheckout.mat"),
                    MaterialEntry("furniture-wall-shelf", "Furniture/FurnitureWallShelf.mat"),
                    MaterialEntry("furniture-central-shelf", "Furniture/FurnitureCentralShelf.mat"),
                    MaterialEntry("furniture-low-display", "Furniture/FurnitureLowDisplay.mat"),
                    MaterialEntry("furniture-featured", "Furniture/FurnitureFeatured.mat"),
                    MaterialEntry("furniture-storage", "Furniture/FurnitureStorage.mat"),
                    MaterialEntry("furniture-crate", "Furniture/FurnitureCrate.mat"),
                    MaterialEntry("decoration", "Furniture/Decoration.mat"),
                    MaterialEntry("product-game", "Products/ProductGame.mat"),
                    MaterialEntry("product-case", "Products/ProductCase.mat"),
                    MaterialEntry("product-console", "Products/ProductConsole.mat"),
                    MaterialEntry("product-controller", "Products/ProductController.mat"),
                    MaterialEntry("product-headset", "Products/ProductHeadset.mat"),
                    MaterialEntry("product-accessory", "Products/ProductAccessory.mat"),
                    MaterialEntry("character-employee", "Characters/CharacterEmployee.mat"),
                    MaterialEntry("character-customer", "Characters/CharacterCustomer.mat"),
                    MaterialEntry("character-supplier", "Characters/CharacterSupplier.mat"),
                    MaterialEntry("feedback-valid", "Feedback/FeedbackValid.mat"),
                    MaterialEntry("feedback-invalid", "Feedback/FeedbackInvalid.mat"),
                    MaterialEntry("feedback-warning", "Feedback/FeedbackWarning.mat")
                };

            asset.Configure(entries);
            EditorUtility.SetDirty(asset);
        }

        private static Phase1MaterialPaletteAsset
            .Entry MaterialEntry(
                string id,
                string relativePath)
        {
            return new Phase1MaterialPaletteAsset
                .Entry
                {
                    id = id,
                    material =
                        LoadRequired<Material>(
                            "Assets/_Project/Art/Materials/" +
                            relativePath)
                };
        }

        private static void ConfigureAudioCatalog()
        {
            Phase1AudioCatalogAsset asset =
                LoadRequired<
                    Phase1AudioCatalogAsset>(
                        CatalogRoot +
                        "/AudioCatalog.asset");

            SerializedObject serialized =
                new SerializedObject(asset);
            SerializedProperty entries =
                serialized.FindProperty("_entries");

            List<AudioData> data =
                AudioEntries();

            entries.arraySize = data.Count;

            for (int index = 0;
                 index < data.Count;
                 index++)
            {
                SerializedProperty element =
                    entries.GetArrayElementAtIndex(
                        index);
                AudioData item = data[index];

                element.FindPropertyRelative(
                    "eventId").stringValue =
                        item.EventId;
                element.FindPropertyRelative(
                    "channel").enumValueIndex =
                        (int)item.Channel;
                element.FindPropertyRelative(
                    "clip").objectReferenceValue =
                        LoadRequired<AudioClip>(
                            item.AssetPath);
                element.FindPropertyRelative(
                    "volume").floatValue =
                        item.Volume;
                element.FindPropertyRelative(
                    "pitch").floatValue = 1f;
                element.FindPropertyRelative(
                    "loop").boolValue =
                        item.Loop;
            }

            serialized.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(asset);
        }

        private static List<AudioData> AudioEntries()
        {
            List<AudioData> entries =
                new List<AudioData>
                {
                    new AudioData(
                        "music.store",
                        Phase1AudioChannel.Music,
                        "Assets/_Project/Audio/Music/StoreMusic.wav",
                        0.45f,
                        true),
                    new AudioData(
                        "ambience.store",
                        Phase1AudioChannel.Ambience,
                        "Assets/_Project/Audio/Ambience/StoreAmbience.wav",
                        0.45f,
                        true)
                };

            AddFeedback(
                entries,
                Phase1FeedbackKind.PlacementValid,
                "Assets/_Project/Audio/SFX/PlacementValid.wav");
            AddFeedback(
                entries,
                Phase1FeedbackKind.PlacementInvalid,
                "Assets/_Project/Audio/SFX/PlacementInvalid.wav");
            AddFeedback(
                entries,
                Phase1FeedbackKind.ObjectSelected,
                "Assets/_Project/Audio/UI/UiConfirm.wav",
                Phase1AudioChannel.Ui);
            AddFeedback(
                entries,
                Phase1FeedbackKind.ObjectHovered,
                "Assets/_Project/Audio/UI/UiConfirm.wav",
                Phase1AudioChannel.Ui);
            AddFeedback(
                entries,
                Phase1FeedbackKind.ProductAssigned,
                "Assets/_Project/Audio/UI/UiConfirm.wav",
                Phase1AudioChannel.Ui);
            AddFeedback(
                entries,
                Phase1FeedbackKind.OutOfStock,
                "Assets/_Project/Audio/UI/UiError.wav");
            AddFeedback(
                entries,
                Phase1FeedbackKind.Reserved,
                "Assets/_Project/Audio/UI/UiConfirm.wav");
            AddFeedback(
                entries,
                Phase1FeedbackKind.Restocked,
                "Assets/_Project/Audio/SFX/OrderReceived.wav");
            AddFeedback(
                entries,
                Phase1FeedbackKind.OrderReceived,
                "Assets/_Project/Audio/SFX/OrderReceived.wav");
            AddFeedback(
                entries,
                Phase1FeedbackKind.CustomerSatisfied,
                "Assets/_Project/Audio/SFX/Checkout.wav");
            AddFeedback(
                entries,
                Phase1FeedbackKind.CustomerFrustrated,
                "Assets/_Project/Audio/UI/UiError.wav");
            AddFeedback(
                entries,
                Phase1FeedbackKind.QueueEntered,
                "Assets/_Project/Audio/SFX/Checkout.wav");
            AddFeedback(
                entries,
                Phase1FeedbackKind.CheckoutCompleted,
                "Assets/_Project/Audio/SFX/Checkout.wav");
            AddFeedback(
                entries,
                Phase1FeedbackKind.Revenue,
                "Assets/_Project/Audio/SFX/Checkout.wav");
            AddFeedback(
                entries,
                Phase1FeedbackKind.Expense,
                "Assets/_Project/Audio/UI/UiConfirm.wav");
            AddFeedback(
                entries,
                Phase1FeedbackKind.ClosingWarning,
                "Assets/_Project/Audio/SFX/DayClosed.wav");
            AddFeedback(
                entries,
                Phase1FeedbackKind.DayClosed,
                "Assets/_Project/Audio/SFX/DayClosed.wav");
            AddFeedback(
                entries,
                Phase1FeedbackKind.AutosaveSucceeded,
                "Assets/_Project/Audio/UI/UiConfirm.wav",
                Phase1AudioChannel.Ui);
            AddFeedback(
                entries,
                Phase1FeedbackKind.AutosaveFailed,
                "Assets/_Project/Audio/UI/UiError.wav",
                Phase1AudioChannel.Ui);
            AddFeedback(
                entries,
                Phase1FeedbackKind.DoorOpened,
                "Assets/_Project/Audio/SFX/Door.wav");
            AddFeedback(
                entries,
                Phase1FeedbackKind.DoorClosed,
                "Assets/_Project/Audio/SFX/Door.wav");

            return entries;
        }

        private static void AddFeedback(
            ICollection<AudioData> entries,
            Phase1FeedbackKind kind,
            string assetPath,
            Phase1AudioChannel channel =
                Phase1AudioChannel.Effects)
        {
            entries.Add(
                new AudioData(
                    "feedback." +
                    kind.ToString()
                        .ToLowerInvariant(),
                    channel,
                    assetPath,
                    0.75f,
                    false));
        }

        private static void ConfigurePresentationCatalog()
        {
            Phase1PresentationCatalogAsset asset =
                LoadRequired<
                    Phase1PresentationCatalogAsset>(
                        CatalogRoot +
                        "/PresentationCatalog.asset");

            SerializedObject serialized =
                new SerializedObject(asset);

            ConfigureCharacters(
                serialized.FindProperty(
                    "_characters"));
            ConfigureAnimations(
                serialized.FindProperty(
                    "_animations"));
            ConfigureFeedback(
                serialized.FindProperty(
                    "_feedback"));

            serialized.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(asset);
        }

        private static void ConfigureCharacters(
            SerializedProperty characters)
        {
            CharacterData[] data =
                {
                    new CharacterData(
                        "employee-main",
                        Phase1CharacterRole.Employee,
                        "Characters/Employee",
                        "character-employee",
                        2f),
                    new CharacterData(
                        "customer-base",
                        Phase1CharacterRole.Customer,
                        "Characters/Customer",
                        "character-customer",
                        2f),
                    new CharacterData(
                        "supplier-base",
                        Phase1CharacterRole.Supplier,
                        "Characters/Supplier",
                        "character-supplier",
                        1.8f)
                };

            characters.arraySize = data.Length;

            for (int index = 0;
                 index < data.Length;
                 index++)
            {
                SerializedProperty element =
                    characters.GetArrayElementAtIndex(
                        index);
                CharacterData item =
                    data[index];

                element.FindPropertyRelative(
                    "id").stringValue =
                        item.Id;
                element.FindPropertyRelative(
                    "role").enumValueIndex =
                        (int)item.Role;
                element.FindPropertyRelative(
                    "prefabResourcePath")
                    .stringValue =
                        item.PrefabPath;
                element.FindPropertyRelative(
                    "materialVariantId")
                    .stringValue =
                        item.MaterialId;
                element.FindPropertyRelative(
                    "moveSpeed").floatValue =
                        item.MoveSpeed;
            }
        }

        private static void ConfigureAnimations(
            SerializedProperty animations)
        {
            AnimationData[] data =
                {
                    new AnimationData("idle", "Idle.anim", true),
                    new AnimationData("walk", "Walk.anim", true),
                    new AnimationData("observe", "Observe.anim", false),
                    new AnimationData("pick-product", "PickProduct.anim", false),
                    new AnimationData("queue-wait", "QueueWait.anim", true),
                    new AnimationData("checkout", "Checkout.anim", false),
                    new AnimationData("satisfied", "Satisfied.anim", false),
                    new AnimationData("frustrated", "Frustrated.anim", false),
                    new AnimationData("move-crate", "MoveCrate.anim", true),
                    new AnimationData("player-place", "PlayerPlace.anim", false),
                    new AnimationData("player-remove", "PlayerRemove.anim", false)
                };

            animations.arraySize = data.Length;

            for (int index = 0;
                 index < data.Length;
                 index++)
            {
                SerializedProperty element =
                    animations.GetArrayElementAtIndex(
                        index);
                AnimationData item =
                    data[index];

                element.FindPropertyRelative(
                    "stateId").stringValue =
                        item.StateId;
                element.FindPropertyRelative(
                    "placeholderClip")
                    .objectReferenceValue =
                        LoadRequired<AnimationClip>(
                            "Assets/_Project/Animations/Characters/" +
                            item.AssetName);
                element.FindPropertyRelative(
                    "loop").boolValue =
                        item.Loop;
            }
        }

        private static void ConfigureFeedback(
            SerializedProperty feedback)
        {
            Phase1FeedbackKind[] kinds =
                (Phase1FeedbackKind[])
                    Enum.GetValues(
                        typeof(Phase1FeedbackKind));

            feedback.arraySize = kinds.Length;

            string[] iconIds =
                {
                    "game-neon-drift",
                    "case-cloud-runner",
                    "console-vertex-one",
                    "controller-orbit-pad",
                    "headset-signal-pro",
                    "accessory-memory-core"
                };

            for (int index = 0;
                 index < kinds.Length;
                 index++)
            {
                Phase1FeedbackKind kind =
                    kinds[index];
                SerializedProperty element =
                    feedback.GetArrayElementAtIndex(
                        index);

                element.FindPropertyRelative(
                    "kind").enumValueIndex =
                        (int)kind;
                element.FindPropertyRelative(
                    "title").stringValue =
                        kind.ToString();
                element.FindPropertyRelative(
                    "placeholderIconPath")
                    .stringValue =
                        "Products/Icons/" +
                        iconIds[index %
                            iconIds.Length] +
                        "_icon";
                element.FindPropertyRelative(
                    "materialVariantId")
                    .stringValue =
                        FeedbackMaterial(kind);
                element.FindPropertyRelative(
                    "showParticles").boolValue =
                        true;
                element.FindPropertyRelative(
                    "showFloatingText").boolValue =
                        true;
                element.FindPropertyRelative(
                    "scalePulse").floatValue =
                        StrongFeedback(kind)
                            ? 1.12f
                            : 1.06f;
            }
        }

        private static string FeedbackMaterial(
            Phase1FeedbackKind kind)
        {
            switch (kind)
            {
                case Phase1FeedbackKind.PlacementInvalid:
                case Phase1FeedbackKind.OutOfStock:
                case Phase1FeedbackKind.CustomerFrustrated:
                case Phase1FeedbackKind.AutosaveFailed:
                    return "feedback-invalid";

                case Phase1FeedbackKind.Expense:
                case Phase1FeedbackKind.ClosingWarning:
                    return "feedback-warning";

                default:
                    return "feedback-valid";
            }
        }

        private static bool StrongFeedback(
            Phase1FeedbackKind kind)
        {
            switch (kind)
            {
                case Phase1FeedbackKind.OrderReceived:
                case Phase1FeedbackKind.CustomerSatisfied:
                case Phase1FeedbackKind.CheckoutCompleted:
                case Phase1FeedbackKind.AutosaveSucceeded:
                    return true;

                default:
                    return false;
            }
        }

        private static void ConfigureRuntimeRegistry()
        {
            Phase1RuntimeAssetRegistryAsset registry =
                AssetDatabase.LoadAssetAtPath<
                    Phase1RuntimeAssetRegistryAsset>(
                        SettingsRoot +
                        "/RuntimeAssetRegistry.asset");

            if (registry == null)
            {
                registry =
                    ScriptableObject.CreateInstance<
                        Phase1RuntimeAssetRegistryAsset>();

                AssetDatabase.CreateAsset(
                    registry,
                    SettingsRoot +
                    "/RuntimeAssetRegistry.asset");
            }

            registry.Configure(
                LoadRequired<Phase1SettingsAsset>(
                    SettingsRoot +
                    "/StoreRuntimeSettings.asset"),
                LoadRequired<Phase1ContentCatalogAsset>(
                    CatalogRoot +
                    "/ContentCatalog.asset"),
                LoadRequired<Phase1StoreShellAsset>(
                    StoreRoot +
                    "/StoreShell.asset"),
                LoadRequired<Phase1MaterialPaletteAsset>(
                    CatalogRoot +
                    "/MaterialPalette.asset"),
                LoadRequired<Phase1PresentationCatalogAsset>(
                    CatalogRoot +
                    "/PresentationCatalog.asset"),
                LoadRequired<Phase1AudioCatalogAsset>(
                    CatalogRoot +
                    "/AudioCatalog.asset"));

            EditorUtility.SetDirty(registry);

            List<UnityEngine.Object> preloaded =
                PlayerSettings.GetPreloadedAssets()
                    .Where(
                        asset =>
                            asset != null &&
                            !(asset is
                                Phase1RuntimeAssetRegistryAsset))
                    .ToList();

            preloaded.Add(registry);

            PlayerSettings.SetPreloadedAssets(
                preloaded.ToArray());
        }

        private static void PatchRuntimeLoading()
        {
            string path =
                AbsolutePath(RuntimeRootScript);
            string content =
                File.ReadAllText(path);

            string constantsPattern =
                @"\s*private const string SettingsPath\s*=" +
                @".*?private const string AudioPath\s*=" +
                @".*?;\s*";

            content =
                Regex.Replace(
                    content,
                    constantsPattern,
                    "\n",
                    RegexOptions.Singleline);

            string loadPattern =
                @"        private void LoadAssets\(\)\s*" +
                @"\{.*?\n        \}\s*" +
                @"\n        private void HandleSceneLoaded";

            string replacement =
                @"        private void LoadAssets()" + "\n" +
                @"        {" + "\n" +
                @"            Phase1RuntimeAssetRegistryAsset" + "\n" +
                @"                registry =" + "\n" +
                @"                    Phase1RuntimeAssetRegistryAsset" + "\n" +
                @"                        .FindLoaded();" + "\n\n" +
                @"            if (registry == null)" + "\n" +
                @"            {" + "\n" +
                @"                Debug.LogError(" + "\n" +
                @"                    ""[Runtime] Preloaded asset registry is missing."");" + "\n" +
                @"                return;" + "\n" +
                @"            }" + "\n\n" +
                @"            _settings = registry.Settings;" + "\n" +
                @"            _contentAsset =" + "\n" +
                @"                registry.ContentCatalog;" + "\n" +
                @"            _shellAsset = registry.StoreShell;" + "\n" +
                @"            _paletteAsset =" + "\n" +
                @"                registry.MaterialPalette;" + "\n" +
                @"            _presentationAsset =" + "\n" +
                @"                registry.PresentationCatalog;" + "\n" +
                @"            _audioAsset = registry.AudioCatalog;" + "\n" +
                @"        }" + "\n\n" +
                @"        private void HandleSceneLoaded";

            string patched =
                Regex.Replace(
                    content,
                    loadPattern,
                    replacement,
                    RegexOptions.Singleline);

            if (string.Equals(
                    patched,
                    content,
                    StringComparison.Ordinal))
            {
                if (!content.Contains(
                        "Phase1RuntimeAssetRegistryAsset"))
                {
                    throw new InvalidOperationException(
                        "Runtime root patch did not match the current source.");
                }
            }

            patched =
                patched.Replace(
                    "[Sprint16 Phase1] Required Resources assets are missing.",
                    "[Runtime] Required project assets are missing.");

            WriteIfChanged(path, patched);
        }

        private static void PatchGenericAssetNames()
        {
            ReplaceInFile(
                ContentCatalogScript,
                new Dictionary<string, string>
                {
                    {
                        "Cartridge & Cloud/Sprint 16 Phase 1/Content Catalog",
                        "Cartridge & Cloud/Runtime/Content Catalog"
                    },
                    {
                        "CC_S16_P1_ContentCatalog",
                        "ContentCatalog"
                    },
                    {
                        "Sprint16Phase1/Prefabs/Furniture/",
                        "Furniture/"
                    },
                    {
                        "Sprint16Phase1/Prefabs/Products/",
                        "Products/"
                    },
                    {
                        "Sprint16Phase1/Icons/",
                        "Products/Icons/"
                    },
                    {
                        "Sprint16Phase1/Covers/",
                        "Products/Covers/"
                    }
                });

            ReplaceInFile(
                MaterialPaletteScript,
                new Dictionary<string, string>
                {
                    {
                        "Cartridge & Cloud/Sprint 16 Phase 1/Material Palette",
                        "Cartridge & Cloud/Runtime/Material Palette"
                    },
                    {
                        "CC_S16_P1_MaterialPalette",
                        "MaterialPalette"
                    },
                    {
                        "Sprint16Phase1/Materials/",
                        "Materials/"
                    },
                    {
                        "CC_S16_P1_",
                        string.Empty
                    }
                });

            ReplaceInFile(
                PresentationCatalogScript,
                new Dictionary<string, string>
                {
                    {
                        "Cartridge & Cloud/Sprint 16 Phase 1/Presentation Catalog",
                        "Cartridge & Cloud/Runtime/Presentation Catalog"
                    },
                    {
                        "CC_S16_P1_PresentationCatalog",
                        "PresentationCatalog"
                    },
                    {
                        "Sprint16Phase1/Animations/",
                        "Animations/"
                    },
                    {
                        "Sprint16Phase1/Prefabs/Characters/",
                        "Characters/"
                    },
                    {
                        "Sprint16Phase1/Icons/",
                        "Products/Icons/"
                    },
                    {
                        "CC_S16_P1_Anim_",
                        string.Empty
                    }
                });

            ReplaceInFile(
                AudioCatalogScript,
                new Dictionary<string, string>
                {
                    {
                        "Cartridge & Cloud/Sprint 16 Phase 1/Audio Catalog",
                        "Cartridge & Cloud/Runtime/Audio Catalog"
                    },
                    {
                        "CC_S16_P1_AudioCatalog",
                        "AudioCatalog"
                    },
                    {
                        "Sprint16Phase1/Audio/",
                        "Audio/"
                    },
                    {
                        "CC_S16_P1_",
                        string.Empty
                    }
                });
        }

        private static void ReplaceInFile(
            string assetPath,
            IReadOnlyDictionary<string, string>
                replacements)
        {
            string path =
                AbsolutePath(assetPath);
            string content =
                File.ReadAllText(path);
            string patched = content;

            foreach (KeyValuePair<string, string>
                     replacement in replacements)
            {
                patched =
                    patched.Replace(
                        replacement.Key,
                        replacement.Value);
            }

            WriteIfChanged(path, patched);
        }

        private static void WriteIfChanged(
            string absolutePath,
            string content)
        {
            string current =
                File.ReadAllText(
                    absolutePath);

            if (string.Equals(
                    current,
                    content,
                    StringComparison.Ordinal))
            {
                return;
            }

            File.WriteAllText(
                absolutePath,
                content);
        }

        private static string AbsolutePath(
            string assetPath)
        {
            string projectRoot =
                Directory.GetParent(
                    Application.dataPath)
                    ?.FullName;

            if (string.IsNullOrWhiteSpace(
                    projectRoot))
            {
                throw new InvalidOperationException(
                    "Project root could not be resolved.");
            }

            return Path.Combine(
                projectRoot,
                assetPath.Replace(
                    '/',
                    Path.DirectorySeparatorChar));
        }

        private static void DeleteEmptySourceFolder()
        {
            if (!AssetDatabase.IsValidFolder(
                    SourceRoot))
            {
                return;
            }

            string absoluteSource =
                AbsolutePath(SourceRoot);

            string[] remainingFiles =
                Directory.Exists(
                    absoluteSource)
                    ? Directory.GetFiles(
                        absoluteSource,
                        "*",
                        SearchOption.AllDirectories)
                    : Array.Empty<string>();

            bool containsAssets =
                remainingFiles.Any(
                    path =>
                        !path.EndsWith(
                            ".meta",
                            StringComparison.OrdinalIgnoreCase));

            if (!containsAssets)
            {
                AssetDatabase.DeleteAsset(
                    SourceRoot);
            }
        }

        private static T LoadRequired<T>(
            string assetPath)
            where T : UnityEngine.Object
        {
            T asset =
                AssetDatabase.LoadAssetAtPath<T>(
                    assetPath);

            if (asset == null)
            {
                throw new FileNotFoundException(
                    "Required asset is missing.",
                    assetPath);
            }

            return asset;
        }

        private static IReadOnlyList<MoveEntry>
            Entries()
        {
            List<MoveEntry> entries =
                new List<MoveEntry>();

            AddAnimations(entries);
            AddAudio(entries);
            AddCatalogs(entries);
            AddTextures(entries);
            AddMaterials(entries);
            AddPlacementDefinitions(entries);
            AddPrefabs(entries);

            return entries;
        }

        private static void AddAnimations(
            ICollection<MoveEntry> entries)
        {
            Add(
                entries,
                "Animations/CC_S16_P1_Anim_Checkout.anim",
                "Animations/Characters/Checkout.anim");
            Add(
                entries,
                "Animations/CC_S16_P1_Anim_Frustrated.anim",
                "Animations/Characters/Frustrated.anim");
            Add(
                entries,
                "Animations/CC_S16_P1_Anim_Idle.anim",
                "Animations/Characters/Idle.anim");
            Add(
                entries,
                "Animations/CC_S16_P1_Anim_MoveCrate.anim",
                "Animations/Characters/MoveCrate.anim");
            Add(
                entries,
                "Animations/CC_S16_P1_Anim_Observe.anim",
                "Animations/Characters/Observe.anim");
            Add(
                entries,
                "Animations/CC_S16_P1_Anim_PickProduct.anim",
                "Animations/Characters/PickProduct.anim");
            Add(
                entries,
                "Animations/CC_S16_P1_Anim_PlayerPlace.anim",
                "Animations/Characters/PlayerPlace.anim");
            Add(
                entries,
                "Animations/CC_S16_P1_Anim_PlayerRemove.anim",
                "Animations/Characters/PlayerRemove.anim");
            Add(
                entries,
                "Animations/CC_S16_P1_Anim_QueueWait.anim",
                "Animations/Characters/QueueWait.anim");
            Add(
                entries,
                "Animations/CC_S16_P1_Anim_Satisfied.anim",
                "Animations/Characters/Satisfied.anim");
            Add(
                entries,
                "Animations/CC_S16_P1_Anim_Walk.anim",
                "Animations/Characters/Walk.anim");
        }

        private static void AddAudio(
            ICollection<MoveEntry> entries)
        {
            Add(
                entries,
                "Audio/CC_S16_P1_AmbienceStore.wav",
                "Audio/Ambience/StoreAmbience.wav");
            Add(
                entries,
                "Audio/CC_S16_P1_MusicStore.wav",
                "Audio/Music/StoreMusic.wav");
            Add(
                entries,
                "Audio/CC_S16_P1_UiConfirm.wav",
                "Audio/UI/UiConfirm.wav");
            Add(
                entries,
                "Audio/CC_S16_P1_UiError.wav",
                "Audio/UI/UiError.wav");
            Add(
                entries,
                "Audio/CC_S16_P1_Checkout.wav",
                "Audio/SFX/Checkout.wav");
            Add(
                entries,
                "Audio/CC_S16_P1_DayClosed.wav",
                "Audio/SFX/DayClosed.wav");
            Add(
                entries,
                "Audio/CC_S16_P1_Door.wav",
                "Audio/SFX/Door.wav");
            Add(
                entries,
                "Audio/CC_S16_P1_OrderReceived.wav",
                "Audio/SFX/OrderReceived.wav");
            Add(
                entries,
                "Audio/CC_S16_P1_PlacementInvalid.wav",
                "Audio/SFX/PlacementInvalid.wav");
            Add(
                entries,
                "Audio/CC_S16_P1_PlacementValid.wav",
                "Audio/SFX/PlacementValid.wav");
        }

        private static void AddCatalogs(
            ICollection<MoveEntry> entries)
        {
            Add(
                entries,
                "CC_S16_P1_AudioCatalog.asset",
                "Data/Catalogs/AudioCatalog.asset");
            Add(
                entries,
                "CC_S16_P1_ContentCatalog.asset",
                "Data/Catalogs/ContentCatalog.asset");
            Add(
                entries,
                "CC_S16_P1_MaterialPalette.asset",
                "Data/Catalogs/MaterialPalette.asset");
            Add(
                entries,
                "CC_S16_P1_PresentationCatalog.asset",
                "Data/Catalogs/PresentationCatalog.asset");
            Add(
                entries,
                "CC_S16_P1_Settings.asset",
                "Settings/Runtime/StoreRuntimeSettings.asset");
            Add(
                entries,
                "CC_S16_P1_StoreShell.asset",
                "Data/Store/StoreShell.asset");
        }

        private static void AddTextures(
            ICollection<MoveEntry> entries)
        {
            string[] ids =
                {
                    "accessory-memory-core",
                    "case-cloud-runner",
                    "console-vertex-one",
                    "controller-orbit-pad",
                    "game-neon-drift",
                    "headset-signal-pro"
                };

            foreach (string id in ids)
            {
                Add(
                    entries,
                    "Icons/" +
                    id +
                    "_icon.png",
                    "Art/Textures/Products/Icons/" +
                    id +
                    "_icon.png");
                Add(
                    entries,
                    "Covers/" +
                    id +
                    "_cover.png",
                    "Art/Textures/Products/Covers/" +
                    id +
                    "_cover.png");
            }
        }

        private static void AddMaterials(
            ICollection<MoveEntry> entries)
        {
            AddMaterial(
                entries,
                "CharacterCustomer",
                "Characters");
            AddMaterial(
                entries,
                "CharacterEmployee",
                "Characters");
            AddMaterial(
                entries,
                "CharacterSupplier",
                "Characters");

            AddMaterial(
                entries,
                "Decoration",
                "Furniture");
            AddMaterial(
                entries,
                "FurnitureCentralShelf",
                "Furniture");
            AddMaterial(
                entries,
                "FurnitureCheckout",
                "Furniture");
            AddMaterial(
                entries,
                "FurnitureCrate",
                "Furniture");
            AddMaterial(
                entries,
                "FurnitureFeatured",
                "Furniture");
            AddMaterial(
                entries,
                "FurnitureLowDisplay",
                "Furniture");
            AddMaterial(
                entries,
                "FurnitureStorage",
                "Furniture");
            AddMaterial(
                entries,
                "FurnitureWallShelf",
                "Furniture");

            AddMaterial(
                entries,
                "ProductAccessory",
                "Products");
            AddMaterial(
                entries,
                "ProductCase",
                "Products");
            AddMaterial(
                entries,
                "ProductConsole",
                "Products");
            AddMaterial(
                entries,
                "ProductController",
                "Products");
            AddMaterial(
                entries,
                "ProductGame",
                "Products");
            AddMaterial(
                entries,
                "ProductHeadset",
                "Products");

            AddMaterial(
                entries,
                "ShellFloor",
                "Architecture");
            AddMaterial(
                entries,
                "ShellGlass",
                "Architecture");
            AddMaterial(
                entries,
                "ShellWall",
                "Architecture");
            AddMaterial(
                entries,
                "ZoneBackroom",
                "Architecture");
            AddMaterial(
                entries,
                "ZoneCheckout",
                "Architecture");
            AddMaterial(
                entries,
                "ZoneMarker",
                "Architecture");
            AddMaterial(
                entries,
                "ZoneReceiving",
                "Architecture");

            AddMaterial(
                entries,
                "FeedbackInvalid",
                "Feedback");
            AddMaterial(
                entries,
                "FeedbackValid",
                "Feedback");
            AddMaterial(
                entries,
                "FeedbackWarning",
                "Feedback");
        }

        private static void AddMaterial(
            ICollection<MoveEntry> entries,
            string name,
            string family)
        {
            Add(
                entries,
                "Materials/CC_S16_P1_" +
                name +
                ".mat",
                "Art/Materials/" +
                family +
                "/" +
                name +
                ".mat");
        }

        private static void AddPlacementDefinitions(
            ICollection<MoveEntry> entries)
        {
            string[] names =
                {
                    "BackroomStorage",
                    "CentralShelf",
                    "CheckoutCounter",
                    "DecorationPlant",
                    "FeaturedDisplay",
                    "LowDisplay",
                    "ReceivingCrate",
                    "WallShelf"
                };

            foreach (string name in names)
            {
                Add(
                    entries,
                    "PlacementDefinitions/" +
                    "CC_S16_P1_" +
                    name +
                    ".asset",
                    "Data/Placement/Definitions/" +
                    name +
                    ".asset");
            }
        }

        private static void AddPrefabs(
            ICollection<MoveEntry> entries)
        {
            string[] characters =
                {
                    "Customer",
                    "Employee",
                    "Supplier"
                };

            foreach (string name in characters)
            {
                Add(
                    entries,
                    "Prefabs/Characters/" +
                    name +
                    ".prefab",
                    "Prefabs/Characters/" +
                    name +
                    ".prefab");
            }

            string[] furniture =
                {
                    "BackroomStorage",
                    "CentralShelf",
                    "CheckoutCounter",
                    "DecorationPlant",
                    "FeaturedDisplay",
                    "LowDisplay",
                    "ReceivingCrate",
                    "WallShelf"
                };

            foreach (string name in furniture)
            {
                Add(
                    entries,
                    "Prefabs/Furniture/" +
                    name +
                    ".prefab",
                    "Prefabs/Furniture/" +
                    name +
                    ".prefab");
            }

            string[] products =
                {
                    "CloudRunnerCase",
                    "MemoryCoreAccessory",
                    "NeonDrift",
                    "OrbitPadController",
                    "SignalProHeadset",
                    "VertexOneConsole"
                };

            foreach (string name in products)
            {
                Add(
                    entries,
                    "Prefabs/Products/" +
                    name +
                    ".prefab",
                    "Prefabs/Products/" +
                    name +
                    ".prefab");
            }
        }

        private static void Add(
            ICollection<MoveEntry> entries,
            string sourceRelative,
            string destinationRelative)
        {
            entries.Add(
                new MoveEntry(
                    SourceRoot +
                    "/" +
                    sourceRelative,
                    "Assets/_Project/" +
                    destinationRelative));
        }

        private readonly struct MoveEntry
        {
            public MoveEntry(
                string source,
                string destination)
            {
                Source = source;
                Destination = destination;
            }

            public string Source { get; }
            public string Destination { get; }
        }

        private readonly struct AudioData
        {
            public AudioData(
                string eventId,
                Phase1AudioChannel channel,
                string assetPath,
                float volume,
                bool loop)
            {
                EventId = eventId;
                Channel = channel;
                AssetPath = assetPath;
                Volume = volume;
                Loop = loop;
            }

            public string EventId { get; }
            public Phase1AudioChannel Channel { get; }
            public string AssetPath { get; }
            public float Volume { get; }
            public bool Loop { get; }
        }

        private readonly struct CharacterData
        {
            public CharacterData(
                string id,
                Phase1CharacterRole role,
                string prefabPath,
                string materialId,
                float moveSpeed)
            {
                Id = id;
                Role = role;
                PrefabPath = prefabPath;
                MaterialId = materialId;
                MoveSpeed = moveSpeed;
            }

            public string Id { get; }
            public Phase1CharacterRole Role { get; }
            public string PrefabPath { get; }
            public string MaterialId { get; }
            public float MoveSpeed { get; }
        }

        private readonly struct AnimationData
        {
            public AnimationData(
                string stateId,
                string assetName,
                bool loop)
            {
                StateId = stateId;
                AssetName = assetName;
                Loop = loop;
            }

            public string StateId { get; }
            public string AssetName { get; }
            public bool Loop { get; }
        }
    }
}
