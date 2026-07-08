using System;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using VRMGames.CartridgeAndCloud.Infrastructure.Store;
using VRMGames.CartridgeAndCloud.Presentation.Characters;
using VRMGames.CartridgeAndCloud.Presentation.Store.Authoring;
using VRMGames.CartridgeAndCloud.Presentation.Store.Doors;
using VRMGames.CartridgeAndCloud.Runtime.Characters;
using VRMGames.CartridgeAndCloud.Runtime.Navigation;

namespace VRMGames.CartridgeAndCloud.Editor.ProjectOrganization.Validation
{
    /// <summary>
    /// Read-only validation for the authored StoreInitial baseline. It never
    /// creates, repairs, saves or rewrites assets.
    /// </summary>
    public static class StoreStructuralValidationTool
    {
        private const string EnvironmentPath =
            "Assets/_Project/Prefabs/Store/StoreInitialEnvironment.prefab";
        private const string CatalogPath =
            "Assets/_Project/Data/Catalogs/RepresentativePrefabCatalog.asset";
        private const string PresentationCatalogPath =
            "Assets/_Project/Data/Catalogs/PresentationCatalog.asset";
        private const string SettingsPath =
            "Assets/_Project/Settings/Runtime/StoreRuntimeSettings.asset";

        [MenuItem("Cartridge & Cloud/Validation/Validate Store Structure")]
        public static void ValidateFromMenu()
        {
            IReadOnlyList<string> errors = Validate();
            if (errors.Count == 0)
            {
                Debug.Log("Store structural validation passed.");
                return;
            }

            Debug.LogError(
                "Store structural validation failed:\n- " +
                string.Join("\n- ", errors));
        }

        public static IReadOnlyList<string> Validate()
        {
            List<string> errors = new List<string>();
            ValidateRuntimeSettings(errors);
            ValidateEnvironment(errors);
            ValidateFurniture(errors);
            ValidateArchitectureProductsAndFutureContent(errors);
            ValidateActorsAndDelivery(errors);
            ValidateMissingScripts(errors);
            return errors;
        }

        private static void ValidateEnvironment(List<string> errors)
        {
            GameObject environment =
                AssetDatabase.LoadAssetAtPath<GameObject>(EnvironmentPath);
            if (environment == null)
            {
                errors.Add("StoreInitialEnvironment.prefab is missing.");
                return;
            }

            StoreNavigationAuthoring authoring =
                environment.GetComponent<StoreNavigationAuthoring>();
            if (authoring == null)
            {
                errors.Add("StoreNavigationAuthoring is missing.");
            }
            else if (!authoring.TryValidate(out string report))
            {
                errors.Add(report);
            }

            NavMeshSurface[] surfaces =
                environment.GetComponentsInChildren<NavMeshSurface>(true);
            if (surfaces.Length != 1)
            {
                errors.Add("StoreInitialEnvironment requires exactly one NavMeshSurface.");
            }
            else if (surfaces[0].collectObjects != CollectObjects.Children ||
                     surfaces[0].useGeometry != NavMeshCollectGeometry.PhysicsColliders)
            {
                errors.Add("NavMeshSurface must collect authored children using physics colliders.");
            }

            if (environment.transform.Find("Collision") == null ||
                environment.transform.Find("Navigation") == null ||
                environment.transform.Find("Anchors") == null)
            {
                errors.Add("Environment requires Collision, Navigation and Anchors roots.");
            }
        }

        private static void ValidateRuntimeSettings(
            List<string> errors)
        {
            StoreRuntimeSettingsAsset settings =
                AssetDatabase.LoadAssetAtPath<
                    StoreRuntimeSettingsAsset>(
                        SettingsPath);

            if (settings == null)
            {
                errors.Add(
                    "StoreRuntimeSettings.asset is missing.");
                return;
            }

            if (!settings.TryValidateAuthoring(
                    out string report))
            {
                errors.Add(report);
            }
        }

        private static void ValidateFurniture(
            List<string> errors)
        {
            foreach (string guid in AssetDatabase.FindAssets(
                         "t:Prefab",
                         new[]
                         {
                             "Assets/_Project/Prefabs/Furniture"
                         }))
            {
                string path =
                    AssetDatabase.GUIDToAssetPath(
                        guid);

                GameObject prefab =
                    AssetDatabase.LoadAssetAtPath<
                        GameObject>(path);

                if (prefab == null)
                {
                    continue;
                }

                StoreFixturePrefabAuthoring fixture =
                    prefab.GetComponent<
                        StoreFixturePrefabAuthoring>();

                if (fixture == null)
                {
                    errors.Add(
                        $"{path} lacks StoreFixturePrefabAuthoring.");
                    continue;
                }

                if (!fixture.TryValidate(
                        out string report))
                {
                    errors.Add(
                        $"{path}: {report}");
                    continue;
                }

                Transform groundAnchor =
                    prefab.transform.Find(
                        PrefabPhysicalContractAuthoring
                            .GroundAnchorName);

                StoreRuntimeSettingsAsset settings =
                    AssetDatabase.LoadAssetAtPath<
                        StoreRuntimeSettingsAsset>(
                            SettingsPath);

                float groundTolerance =
                    settings == null
                        ? 0.02f
                        : settings.GroundAnchorTolerance;

                if (groundAnchor == null)
                {
                    errors.Add(
                        $"{path} requires a direct GroundAnchor child.");
                }
                else if (groundAnchor.localPosition.sqrMagnitude >
                         groundTolerance * groundTolerance)
                {
                    errors.Add(
                        $"{path} GroundAnchor exceeds the configured base tolerance.");
                }

                if (path.EndsWith(
                        "CheckoutCounter.prefab",
                        StringComparison.Ordinal))
                {
                    Transform staffPoint =
                        FindNamedChild(prefab.transform, "StaffPoint");
                    Transform staffLookTarget =
                        FindNamedChild(prefab.transform, "StaffLookTarget");
                    if (staffPoint == null || staffLookTarget == null)
                    {
                        errors.Add(
                            $"{path} requires StaffPoint and StaffLookTarget anchors.");
                    }
                }

                Transform collision =
                    prefab.transform.Find("Collision");

                if (collision == null ||
                    collision.GetComponent<
                        BoxCollider>() == null ||
                    collision.GetComponent<
                        NavMeshObstacle>() == null ||
                    collision.GetComponent<
                        StoreFixtureNavigationObstacle>() == null)
                {
                    errors.Add(
                        $"{path} requires authored collider and carved obstacle.");
                }

                LODGroup lod =
                    prefab.GetComponent<LODGroup>();

                if (lod == null)
                {
                    errors.Add(
                        $"{path} requires an authored LODGroup.");
                    continue;
                }

                LOD[] levels = lod.GetLODs();
                if (levels.Length < 2)
                {
                    errors.Add(
                        $"{path} requires at least two authored LOD levels.");
                }

                if (levels.Length > 0 &&
                    levels[levels.Length - 1]
                        .screenRelativeTransitionHeight > 0f)
                {
                    errors.Add(
                        $"{path} last LOD must remain visible at the full gameplay zoom range.");
                }

                for (int index = 0;
                     index < levels.Length;
                     index++)
                {
                    if (levels[index].renderers == null ||
                        levels[index].renderers.Length == 0)
                    {
                        errors.Add(
                            $"{path} LOD {index} has no renderers.");
                    }
                }
            }
        }

        private static void ValidateArchitectureProductsAndFutureContent(
            List<string> errors)
        {
            ValidateRoleFolder(
                "Assets/_Project/Prefabs/Architecture",
                null,
                errors);
            ValidateRoleFolder(
                "Assets/_Project/Prefabs/Products",
                PrefabPhysicalContractAuthoring.PhysicalRole.DisplayOnly,
                errors);
            ValidateRoleFolder(
                "Assets/_Project/Prefabs/Expansions",
                PrefabPhysicalContractAuthoring.PhysicalRole.FutureContent,
                errors);

            StoreVisualPrefabCatalogAsset catalog =
                AssetDatabase.LoadAssetAtPath<StoreVisualPrefabCatalogAsset>(CatalogPath);
            if (catalog == null || catalog.Expansions.Length != 0)
            {
                errors.Add("Future expansions must be absent from the StoreInitial runtime catalog.");
            }

            GameObject door = AssetDatabase.LoadAssetAtPath<GameObject>(
                "Assets/_Project/Prefabs/Architecture/Modular/AutomaticDoor.prefab");
            if (door == null ||
                door.GetComponent<AutomaticSlidingDoorController>() == null ||
                door.GetComponent<AutomaticDoorParts>() == null ||
                door.GetComponentInChildren<AutomaticDoorSensor>(true) == null ||
                door.transform.Find("Anchors") == null ||
                door.GetComponentsInChildren<Collider>(true).Length < 3)
            {
                errors.Add("AutomaticDoor.prefab is not self-contained.");
            }
        }

        private static void ValidateRoleFolder(
            string folder,
            PrefabPhysicalContractAuthoring.PhysicalRole? requiredRole,
            List<string> errors)
        {
            foreach (string guid in AssetDatabase.FindAssets(
                         "t:Prefab",
                         new[] { folder }))
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                PrefabPhysicalContractAuthoring contract =
                    prefab == null
                        ? null
                        : prefab.GetComponent<PrefabPhysicalContractAuthoring>();
                if (contract == null)
                {
                    errors.Add($"{path} lacks a physical contract.");
                    continue;
                }

                if (!contract.TryValidate(out string report))
                {
                    errors.Add(report);
                    continue;
                }

                if (requiredRole.HasValue && contract.Role != requiredRole.Value)
                {
                    errors.Add($"{path} has physical role {contract.Role}, expected {requiredRole.Value}.");
                }
            }
        }

        private static void ValidateActorsAndDelivery(List<string> errors)
        {
            StorePresentationCatalogAsset presentation =
                AssetDatabase.LoadAssetAtPath<StorePresentationCatalogAsset>(
                    PresentationCatalogPath);
            if (presentation == null || presentation.Actors.Length != 7)
            {
                errors.Add("Presentation catalog requires exactly seven current actors.");
            }

            string[] actorGuids = AssetDatabase.FindAssets(
                "t:Prefab",
                new[] { "Assets/_Project/Resources/Characters" });
            if (actorGuids.Length != 7)
            {
                errors.Add("Exactly seven authored actor wrapper prefabs are required.");
            }

            foreach (string guid in actorGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GameObject actor = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                CapsuleCollider capsule = actor == null
                    ? null
                    : actor.GetComponent<CapsuleCollider>();
                if (actor == null ||
                    capsule == null ||
                    !capsule.isTrigger ||
                    actor.GetComponent<NavMeshAgent>() == null ||
                    actor.GetComponent<ActorPrefabAuthoring>() == null ||
                    actor.GetComponent<CharacterPresence>() == null ||
                    actor.GetComponent<CharacterLocomotionAnimator>() == null)
                {
                    errors.Add($"{path} lacks its serialized actor contract.");
                }
            }

            if (presentation == null ||
                presentation.SupplierDeliveryViewPrefab == null ||
                presentation.SupplierDeliveryViewPrefab
                    .GetComponent<SupplierDeliveryView>() == null)
            {
                errors.Add("Presentation catalog requires SupplierDeliveryView.prefab.");
            }
        }

        private static void ValidateMissingScripts(List<string> errors)
        {
            string[] roots =
            {
                "Assets/_Project/Prefabs",
                "Assets/_Project/Resources/Characters"
            };

            foreach (string guid in AssetDatabase.FindAssets("t:Prefab", roots))
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (prefab != null &&
                    GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(prefab) > 0)
                {
                    errors.Add($"{path} contains missing scripts.");
                }
            }
        }
        private static Transform FindNamedChild(
            Transform root,
            string name)
        {
            foreach (Transform child in
                     root.GetComponentsInChildren<Transform>(true))
            {
                if (string.Equals(
                        child.name,
                        name,
                        StringComparison.Ordinal))
                {
                    return child;
                }
            }

            return null;
        }

    }
}
