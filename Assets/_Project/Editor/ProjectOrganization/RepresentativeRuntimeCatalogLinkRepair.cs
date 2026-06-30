using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Infrastructure.VerticalSlicePhase1;

namespace VRMGames.CartridgeAndCloud.Editor.ProjectOrganization
{
    public static class
        RepresentativeRuntimeCatalogLinkRepair
    {
        private const string RegistryPath =
            "Assets/_Project/Settings/Runtime/" +
            "RuntimeAssetRegistry.asset";

        private const string CatalogPath =
            "Assets/_Project/Data/Catalogs/" +
            "RepresentativePrefabCatalog.asset";

        [MenuItem(
            "Tools/Cartridge & Cloud/" +
            "Representative Assets/" +
            "Repair Runtime Catalog Link")]
        public static void Repair()
        {
            if (EditorApplication
                .isPlayingOrWillChangePlaymode)
            {
                EditorUtility.DisplayDialog(
                    "Runtime catalog link",
                    "Exit Play Mode before repairing the link.",
                    "OK");
                return;
            }

            try
            {
                Phase1RuntimeAssetRegistryAsset registry =
                    AssetDatabase.LoadAssetAtPath<
                        Phase1RuntimeAssetRegistryAsset>(
                            RegistryPath);

                RepresentativePrefabCatalogAsset catalog =
                    AssetDatabase.LoadAssetAtPath<
                        RepresentativePrefabCatalogAsset>(
                            CatalogPath);

                if (registry == null)
                {
                    throw new InvalidOperationException(
                        "RuntimeAssetRegistry.asset was not found.");
                }

                if (catalog == null)
                {
                    throw new InvalidOperationException(
                        "RepresentativePrefabCatalog.asset was not found.");
                }

                ValidateCatalog(catalog);

                registry.ConfigureRepresentativePrefabs(
                    catalog);

                EditorUtility.SetDirty(registry);

                ConfigurePreloadedAssets(
                    registry,
                    catalog);

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh(
                    ImportAssetOptions
                        .ForceSynchronousImport);

                Phase1RuntimeAssetRegistryAsset
                    reloadedRegistry =
                        AssetDatabase.LoadAssetAtPath<
                            Phase1RuntimeAssetRegistryAsset>(
                                RegistryPath);

                if (reloadedRegistry == null ||
                    reloadedRegistry
                        .RepresentativePrefabs !=
                    catalog)
                {
                    throw new InvalidOperationException(
                        "The representative catalog reference " +
                        "was not persisted in RuntimeAssetRegistry.");
                }

                Debug.Log(
                    "[Representative Assets] " +
                    "Runtime catalog link PASS. " +
                    "Representative prefabs are now resolved " +
                    "through RuntimeAssetRegistry.");

                EditorUtility.DisplayDialog(
                    "Runtime catalog link",
                    "Repair PASS.\n\n" +
                    "The representative catalog is now referenced " +
                    "directly by RuntimeAssetRegistry.\n\n" +
                    "Enter Play Mode from a clean scene load and " +
                    "verify the Store visuals.",
                    "OK");
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);

                EditorUtility.DisplayDialog(
                    "Runtime catalog link failed",
                    exception.Message,
                    "OK");
            }
        }

        [MenuItem(
            "Tools/Cartridge & Cloud/" +
            "Representative Assets/" +
            "Validate Runtime Catalog Link")]
        public static void ValidateLink()
        {
            List<string> errors =
                new List<string>();

            Phase1RuntimeAssetRegistryAsset registry =
                AssetDatabase.LoadAssetAtPath<
                    Phase1RuntimeAssetRegistryAsset>(
                        RegistryPath);

            RepresentativePrefabCatalogAsset catalog =
                AssetDatabase.LoadAssetAtPath<
                    RepresentativePrefabCatalogAsset>(
                        CatalogPath);

            if (registry == null)
            {
                errors.Add(
                    "RuntimeAssetRegistry.asset is missing.");
            }

            if (catalog == null)
            {
                errors.Add(
                    "RepresentativePrefabCatalog.asset is missing.");
            }

            if (registry != null &&
                catalog != null &&
                registry.RepresentativePrefabs !=
                catalog)
            {
                errors.Add(
                    "RuntimeAssetRegistry does not reference " +
                    "RepresentativePrefabCatalog.");
            }

            if (registry != null &&
                !PlayerSettings
                    .GetPreloadedAssets()
                    .Contains(registry))
            {
                errors.Add(
                    "RuntimeAssetRegistry is not configured " +
                    "as a Preloaded Asset.");
            }

            if (catalog != null)
            {
                try
                {
                    ValidateCatalog(catalog);
                }
                catch (Exception exception)
                {
                    errors.Add(exception.Message);
                }
            }

            if (errors.Count == 0)
            {
                Debug.Log(
                    "[Representative Assets] " +
                    "Runtime catalog link validation PASS.");

                EditorUtility.DisplayDialog(
                    "Runtime catalog link",
                    "Validation PASS.",
                    "OK");
                return;
            }

            string detail =
                string.Join(
                    "\n",
                    errors);

            Debug.LogError(
                "[Representative Assets] " +
                "Runtime catalog link validation failed:\n" +
                detail);

            EditorUtility.DisplayDialog(
                "Runtime catalog link",
                "Validation failed:\n\n" +
                detail,
                "OK");
        }

        private static void ConfigurePreloadedAssets(
            Phase1RuntimeAssetRegistryAsset registry,
            RepresentativePrefabCatalogAsset catalog)
        {
            List<UnityEngine.Object> preloaded =
                PlayerSettings
                    .GetPreloadedAssets()
                    .Where(
                        asset =>
                            asset != null &&
                            asset != catalog &&
                            asset != registry)
                    .ToList();

            preloaded.Add(registry);

            PlayerSettings.SetPreloadedAssets(
                preloaded.ToArray());
        }

        private static void ValidateCatalog(
            RepresentativePrefabCatalogAsset catalog)
        {
            if (catalog.FindArchitecture(
                    "architecture.floor.200") ==
                null)
            {
                throw new InvalidOperationException(
                    "Architecture catalog entry is missing: " +
                    "architecture.floor.200");
            }

            if (catalog.FindFurniture(
                    "central-shelf") ==
                null)
            {
                throw new InvalidOperationException(
                    "Furniture catalog entry is missing: " +
                    "central-shelf");
            }

            if (catalog.FindProduct(
                    "game-neon-drift") ==
                null)
            {
                throw new InvalidOperationException(
                    "Product catalog entry is missing: " +
                    "game-neon-drift");
            }
        }
    }
}
