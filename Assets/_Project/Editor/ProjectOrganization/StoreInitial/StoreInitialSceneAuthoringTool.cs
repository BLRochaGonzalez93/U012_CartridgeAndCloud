using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRMGames.CartridgeAndCloud.Infrastructure.Store;
using VRMGames.CartridgeAndCloud.Infrastructure.UIUX;
using VRMGames.CartridgeAndCloud.Presentation.Placement;
using VRMGames.CartridgeAndCloud.Presentation.SceneFlow;
using VRMGames.CartridgeAndCloud.Presentation.Store;
using VRMGames.CartridgeAndCloud.Presentation.Store.Authoring;
using VRMGames.CartridgeAndCloud.Presentation.Store.Doors;
using VRMGames.CartridgeAndCloud.Presentation.Store.Occlusion;

namespace VRMGames.CartridgeAndCloud.Editor.ProjectOrganization.StoreInitial
{
    /// <summary>
    /// Deterministically rebuilds StoreInitial in place from its technical
    /// contract. The authored scene contains only permanent architecture,
    /// lighting, the Warehouse and technical anchors. Purchasable furniture and
    /// decoration remain exclusively player-placed runtime content.
    /// </summary>
    public static class StoreInitialSceneAuthoringTool
    {
        private const string MenuRoot =
            "Tools/Cartridge & Cloud/Store Initial/";

        private const string TargetScenePath =
            "Assets/_Project/Scenes/StoreInitial.unity";

        private const string EnvironmentPrefabPath =
            "Assets/_Project/Prefabs/Store/StoreInitialEnvironment.prefab";

        private const string UiSettingsPath =
            "Assets/_Project/Resources/CC_Sprint15Settings.asset";

        private const string StoreSettingsPath =
            "Assets/_Project/Settings/Runtime/StoreRuntimeSettings.asset";

        private const string WindowsDevelopmentProfilePath =
            "Assets/_Project/Settings/BuildProfiles/Windows_Development.asset";

        private static readonly string[] ApprovedScenePaths =
        {
            "Assets/_Project/Scenes/Bootstrap.unity",
            "Assets/_Project/Scenes/MainMenu.unity",
            TargetScenePath,
            "Assets/_Project/Scenes/TestLab.unity"
        };

        private const float CellSize = 0.5f;
        private const int GridWidth = 20;
        private const int GridDepth = 30;
        private const float GridOriginX = -5f;
        private const float GridOriginZ = -7.5f;

        [MenuItem(MenuRoot + "Rebuild StoreInitial", false, 100)]
        private static void RebuildStoreInitial()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                EditorUtility.DisplayDialog(
                    "StoreInitial",
                    "Exit Play Mode before rebuilding StoreInitial.",
                    "OK");
                return;
            }

            bool confirmed = EditorUtility.DisplayDialog(
                "Rebuild StoreInitial",
                "This will clean and rebuild StoreInitial.unity in place " +
                "and regenerate StoreInitialEnvironment.prefab. No separate " +
                "legacy scene is required.",
                "Rebuild",
                "Cancel");

            if (!confirmed ||
                !EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                return;
            }

            try
            {
                ValidateRequiredAssets();

                Scene targetScene = OpenTargetSceneForRebuild();
                SceneHierarchy hierarchy = PrepareSceneForRebuild(targetScene);

                SuppressLegacyTechnicalVisuals(targetScene);
                RemoveLegacyDirectionalLights(targetScene);

                EnvironmentBuild environment =
                    BuildEnvironment(targetScene, hierarchy.Environment);

                ConfigureSceneLighting();

                StoreInitialSceneContext context =
                    ConfigureSceneContext(
                        targetScene,
                        hierarchy,
                        environment);

                EnsureFolder("Assets/_Project/Prefabs/Store");
                PrefabUtility.SaveAsPrefabAssetAndConnect(
                    environment.Root,
                    EnvironmentPrefabPath,
                    InteractionMode.AutomatedAction);

                context.Configure(
                    context.PlacementSurface,
                    context.ShellDescriptor,
                    context.PlayerSpawn,
                    context.TechnicalPlayer,
                    environment.EntranceAnchor,
                    environment.CheckoutAnchor,
                    environment.ReceivingAnchor,
                    environment.BackroomAnchor,
                    environment.CustomerSpawns,
                    environment.RequiredAccessAnchors,
                    environment.Door,
                    environment.DoorParts,
                    context.GameplayCamera,
                    context.WallOcclusion,
                    environment.Root,
                    environment.InitialFurniture,
                    hierarchy.DynamicFurniture,
                    hierarchy.DynamicProducts,
                    hierarchy.Customers,
                    environment.Lighting);

                UpdateRuntimeSettings();
                UpdateBuildSettings();

                EditorSceneManager.MarkSceneDirty(targetScene);

                if (!context.TryValidate(out string report))
                {
                    throw new InvalidOperationException(report);
                }

                if (!EditorSceneManager.SaveScene(
                        targetScene,
                        TargetScenePath,
                        false))
                {
                    throw new IOException(
                        "Unity could not save StoreInitial.unity.");
                }

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                Selection.activeGameObject = context.gameObject;
                EditorGUIUtility.PingObject(context.gameObject);

                Debug.Log(
                    "[StoreInitial] Rebuild completed. Static architecture, " +
                    "lighting and the Warehouse are authored in scene. The " +
                    "main sales floor starts empty and runtime procedural " +
                    "blockout is disabled for StoreInitial.");

                EditorUtility.DisplayDialog(
                    "StoreInitial generated",
                    "StoreInitial.unity and StoreInitialEnvironment.prefab " +
                    "were rebuilt successfully. Runtime and UI settings now " +
                    "target StoreInitial.",
                    "OK");
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                EditorUtility.DisplayDialog(
                    "StoreInitial rebuild failed",
                    exception.Message,
                    "OK");
            }
        }

        [MenuItem(MenuRoot + "Validate StoreInitial", false, 101)]
        private static void ValidateStoreInitial()
        {
            if (!File.Exists(TargetScenePath))
            {
                EditorUtility.DisplayDialog(
                    "StoreInitial validation",
                    "StoreInitial.unity does not exist. Run Rebuild StoreInitial first.",
                    "OK");
                return;
            }

            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                return;
            }

            Scene scene = EditorSceneManager.OpenScene(
                TargetScenePath,
                OpenSceneMode.Single);

            StoreInitialSceneContext context =
                FindInScene<StoreInitialSceneContext>(scene, true);

            string report = string.Empty;
            bool valid = context != null && context.TryValidate(out report);

            if (context == null)
            {
                report = "StoreInitialSceneContext is missing.";
            }

            if (valid)
            {
                valid = ValidateSettings(out string settingsReport);
                report += "\n" + settingsReport;
            }

            EditorUtility.DisplayDialog(
                valid
                    ? "StoreInitial valid"
                    : "StoreInitial invalid",
                report,
                "OK");

            if (valid)
            {
                Debug.Log("[StoreInitial] " + report, context);
            }
            else
            {
                Debug.LogError("[StoreInitial] " + report, context);
            }
        }

        [MenuItem(MenuRoot + "Open StoreInitial", false, 102)]
        private static void OpenStoreInitial()
        {
            if (!File.Exists(TargetScenePath))
            {
                EditorUtility.DisplayDialog(
                    "StoreInitial",
                    "StoreInitial.unity does not exist.",
                    "OK");
                return;
            }

            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene(
                    TargetScenePath,
                    OpenSceneMode.Single);
            }
        }

        private static void ValidateRequiredAssets()
        {
            List<string> missing = new List<string>();

            RequireAsset<SceneAsset>(TargetScenePath, missing);
            RequireAsset<UIRuntimeSettingsAsset>(UiSettingsPath, missing);
            RequireAsset<StoreRuntimeSettingsAsset>(StoreSettingsPath, missing);
            RequireAsset<UnityEngine.Object>(
                WindowsDevelopmentProfilePath,
                missing);

            foreach (string path in RequiredPrefabPaths())
            {
                RequireAsset<GameObject>(path, missing);
            }

            if (missing.Count > 0)
            {
                throw new FileNotFoundException(
                    "Required StoreInitial assets are missing:\n- " +
                    string.Join("\n- ", missing));
            }
        }

        private static Scene OpenTargetSceneForRebuild()
        {
            Scene scene = EditorSceneManager.OpenScene(
                TargetScenePath,
                OpenSceneMode.Single);

            if (!scene.IsValid() || !scene.isLoaded)
            {
                throw new InvalidOperationException(
                    "StoreInitial.unity could not be opened for rebuilding.");
            }

            return scene;
        }

        private static SceneHierarchy PrepareSceneForRebuild(Scene scene)
        {
            DestroyComponentsWithGameObjects<StoreInitialSceneContext>(scene);
            DestroyNamedObjectInScene(scene, "Sprint15StoreUI");
            DestroyRootByName(scene, "StoreInitialEnvironment");
            DestroyRootByName(scene, "StoreEnvironment");

            SceneHierarchy hierarchy = OrganizeTechnicalHierarchy(scene);

            ClearChildrenExcept(
                hierarchy.DynamicFurniture,
                "PlacedObjects");
            Transform placedObjects = FindDescendant(
                hierarchy.DynamicFurniture,
                "PlacedObjects");
            ClearChildren(placedObjects);
            ClearChildren(hierarchy.DynamicProducts);
            ClearChildren(hierarchy.Customers);
            ClearChildren(hierarchy.Debug);

            StoreShellDescriptor shell =
                FindInScene<StoreShellDescriptor>(scene, true);
            Transform staleAccessMarkers =
                FindDescendant(
                    shell != null ? shell.transform : null,
                    "AccessMarkers");

            if (staleAccessMarkers != null)
            {
                UnityEngine.Object.DestroyImmediate(
                    staleAccessMarkers.gameObject);
            }

            return hierarchy;
        }

        private static SceneHierarchy OrganizeTechnicalHierarchy(Scene scene)
        {
            Transform systems = FindOrCreateRoot("Systems", scene);
            Transform cameras = FindOrCreateRoot("Cameras", scene);
            Transform ui = FindOrCreateRoot("UI", scene);
            Transform environment =
                FindOrCreateRoot("StoreInitialEnvironment", scene);
            Transform dynamicFurniture =
                FindOrCreateRoot("DynamicFurniture", scene);
            Transform dynamicProducts =
                FindOrCreateRoot("DynamicProducts", scene);
            Transform customers = FindOrCreateRoot("Customers", scene);
            Transform debug = FindOrCreateRoot("Debug", scene);

            Camera camera = FindInScene<Camera>(scene, true);

            if (camera != null)
            {
                camera.transform.SetParent(cameras, true);
            }

            Canvas[] canvases = FindAllInScene<Canvas>(scene, true);

            foreach (Canvas canvas in canvases)
            {
                canvas.transform.SetParent(ui, true);
            }

            GameObject eventSystem =
                FindGameObjectInScene(scene, "EventSystem");
            StoreSceneController controller =
                FindInScene<StoreSceneController>(scene, true);
            StoreShellDescriptor shell =
                FindInScene<StoreShellDescriptor>(scene, true);

            if (eventSystem != null)
            {
                eventSystem.transform.SetParent(ui, true);
            }

            if (controller != null)
            {
                controller.transform.SetParent(systems, true);
            }

            if (shell != null)
            {
                shell.transform.SetParent(systems, true);
                shell.gameObject.name = "TechnicalStoreShell";
            }

            PlacementRuntimeController placementRuntime =
                FindInScene<PlacementRuntimeController>(scene, true);

            Transform placedObjects = FindDescendant(
                placementRuntime != null
                    ? placementRuntime.transform
                    : null,
                "PlacedObjects");

            if (placedObjects == null)
            {
                placedObjects = FindDescendant(
                    dynamicFurniture,
                    "PlacedObjects");
            }

            if (placedObjects != null)
            {
                placedObjects.SetParent(dynamicFurniture, true);
            }

            foreach (GameObject root in scene.GetRootGameObjects())
            {
                if (root.transform == systems ||
                    root.transform == cameras ||
                    root.transform == ui ||
                    root.transform == environment ||
                    root.transform == dynamicFurniture ||
                    root.transform == dynamicProducts ||
                    root.transform == customers ||
                    root.transform == debug)
                {
                    continue;
                }

                root.transform.SetParent(systems, true);
            }

            return new SceneHierarchy(
                systems,
                cameras,
                ui,
                environment,
                dynamicFurniture,
                dynamicProducts,
                customers,
                debug);
        }

        private static void SuppressLegacyTechnicalVisuals(Scene scene)
        {
            StoreShellDescriptor shell =
                FindInScene<StoreShellDescriptor>(scene, true);

            if (shell == null)
            {
                throw new InvalidOperationException(
                    "The technical StoreShellDescriptor is missing from StoreInitial.");
            }

            Transform ghost = FindDescendant(shell.transform, "GhostVisual");
            Transform technicalPlayer = shell.TechnicalPlayer;

            foreach (Renderer renderer in
                     shell.GetComponentsInChildren<Renderer>(true))
            {
                if (IsWithin(renderer.transform, ghost) ||
                    IsWithin(renderer.transform, technicalPlayer))
                {
                    continue;
                }

                renderer.enabled = false;
            }
        }

        private static void RemoveLegacyDirectionalLights(Scene scene)
        {
            foreach (Light light in FindAllInScene<Light>(scene, true))
            {
                if (light.type == LightType.Directional)
                {
                    UnityEngine.Object.DestroyImmediate(light.gameObject);
                }
            }
        }

        private static EnvironmentBuild BuildEnvironment(
            Scene scene,
            Transform environmentRoot)
        {
            environmentRoot.name = "StoreInitialEnvironment";
            Transform architecture = CreateChild("Architecture", environmentRoot);
            Transform initialFurniture = CreateChild("AuthoredFixtures", environmentRoot);
            Transform lighting = CreateChild("Lighting", environmentRoot);
            Transform anchors = CreateChild("Anchors", environmentRoot);
            Transform technicalColliders =
                CreateChild("TechnicalColliders", environmentRoot);

            BuildFloor(scene, architecture);
            BuildPerimeter(scene, architecture);
            BuildZones(scene, architecture);

            GameObject facade = InstantiatePrefab(
                scene,
                "Assets/_Project/Prefabs/Architecture/StorefrontFacade.prefab",
                architecture,
                new Vector3(0f, 0f, -7.5f),
                Quaternion.identity,
                "StorefrontFacade");

            GameObject doorObject = InstantiatePrefab(
                scene,
                "Assets/_Project/Prefabs/Architecture/AutomaticDoor.prefab",
                architecture,
                new Vector3(0f, 0f, -7.5f),
                Quaternion.identity,
                "AutomaticDoor");

            AutomaticDoorParts doorParts =
                doorObject.GetComponentInChildren<AutomaticDoorParts>(true);

            if (doorParts == null)
            {
                throw new InvalidOperationException(
                    "AutomaticDoor prefab has no AutomaticDoorParts component.");
            }

            AutomaticSlidingDoorController door =
                doorObject.GetComponent<AutomaticSlidingDoorController>();

            if (door == null)
            {
                door = doorObject.AddComponent<
                    AutomaticSlidingDoorController>();
            }

            InstantiatePrefab(
                scene,
                "Assets/_Project/Prefabs/Architecture/EntranceThreshold.prefab",
                architecture,
                new Vector3(0f, 0f, -7.5f),
                Quaternion.identity,
                "EntranceThreshold");

            InstantiatePrefab(
                scene,
                "Assets/_Project/Prefabs/Architecture/BackroomPartition.prefab",
                architecture,
                new Vector3(0f, 0f, 4.5f),
                Quaternion.identity,
                "BackroomPartition");

            InstantiatePrefab(
                scene,
                "Assets/_Project/Prefabs/Architecture/StoreSign.prefab",
                architecture,
                new Vector3(0f, 2.65f, -7.62f),
                Quaternion.identity,
                "StoreSign");

            _ = facade;

            BuildPartitionColliders(technicalColliders);
            BuildLighting(scene, lighting);

            Transform entranceAnchor =
                CreateAnchor("EntranceAnchor", anchors, new Vector3(0f, 0f, -7.3f));
            Transform checkoutAnchor =
                CreateAnchor("CheckoutAnchor", anchors, new Vector3(2.8f, 0f, -4.8f));
            Transform receivingAnchor =
                CreateAnchor("ReceivingAnchor", anchors, new Vector3(-2.8f, 0f, 5.5f));
            Transform backroomAnchor =
                CreateAnchor("BackroomAnchor", anchors, new Vector3(0f, 0f, 5.8f));

            Transform[] customerSpawns =
            {
                CreateAnchor("CustomerSpawn_01", anchors, new Vector3(-1.5f, 0f, -9f)),
                CreateAnchor("CustomerSpawn_02", anchors, new Vector3(0f, 0f, -9.3f)),
                CreateAnchor("CustomerSpawn_03", anchors, new Vector3(1.5f, 0f, -9f))
            };

            Transform[] requiredAccessAnchors =
            {
                CreateCellAnchor("Access_WarehouseEntryFront_01", anchors, 8, 23),
                CreateCellAnchor("Access_WarehouseEntryFront_02", anchors, 9, 23),
                CreateCellAnchor("Access_WarehouseEntryFront_03", anchors, 10, 23),
                CreateCellAnchor("Access_WarehouseEntryFront_04", anchors, 11, 23),
                CreateCellAnchor("Access_WarehouseEntryRear_01", anchors, 8, 24),
                CreateCellAnchor("Access_WarehouseEntryRear_02", anchors, 9, 24),
                CreateCellAnchor("Access_WarehouseEntryRear_03", anchors, 10, 24),
                CreateCellAnchor("Access_WarehouseEntryRear_04", anchors, 11, 24)
            };

            return new EnvironmentBuild(
                environmentRoot.gameObject,
                initialFurniture,
                lighting,
                entranceAnchor,
                checkoutAnchor,
                receivingAnchor,
                backroomAnchor,
                customerSpawns,
                requiredAccessAnchors,
                door,
                doorParts);
        }

        private static void BuildFloor(Scene scene, Transform parent)
        {
            const string floor200 =
                "Assets/_Project/Prefabs/Architecture/Floor200.prefab";
            const string floor100 =
                "Assets/_Project/Prefabs/Architecture/Floor100.prefab";

            for (int z = 0; z < 7; z++)
            {
                for (int x = 0; x < 5; x++)
                {
                    InstantiatePrefab(
                        scene,
                        floor200,
                        parent,
                        new Vector3(-4f + x * 2f, 0f, -6.5f + z * 2f),
                        Quaternion.identity,
                        $"Floor200_{x:00}_{z:00}");
                }
            }

            for (int x = 0; x < 10; x++)
            {
                InstantiatePrefab(
                    scene,
                    floor100,
                    parent,
                    new Vector3(-4.5f + x, 0f, 7f),
                    Quaternion.identity,
                    $"Floor100_{x:00}_Top");
            }
        }

        private static void BuildPerimeter(Scene scene, Transform parent)
        {
            PlaceWallLine(
                scene,
                parent,
                "LeftWall",
                new Vector3(-5f, 0f, -7.5f),
                Vector3.forward,
                Quaternion.Euler(0f, 90f, 0f),
                new[] { 4, 4, 4, 2, 1 });

            PlaceWallLine(
                scene,
                parent,
                "RightWall",
                new Vector3(5f, 0f, -7.5f),
                Vector3.forward,
                Quaternion.Euler(0f, 90f, 0f),
                new[] { 4, 4, 4, 2, 1 });

            PlaceWallLine(
                scene,
                parent,
                "BackWall",
                new Vector3(-5f, 0f, 7.5f),
                Vector3.right,
                Quaternion.identity,
                new[] { 4, 4, 2 });
        }

        private static void PlaceWallLine(
            Scene scene,
            Transform parent,
            string prefix,
            Vector3 start,
            Vector3 direction,
            Quaternion rotation,
            int[] lengths)
        {
            float cursor = 0f;

            for (int index = 0; index < lengths.Length; index++)
            {
                int length = lengths[index];
                Vector3 position =
                    start + direction * (cursor + length * 0.5f);

                GameObject wall = InstantiatePrefab(
                    scene,
                    $"Assets/_Project/Prefabs/Architecture/Wall{length * 100}.prefab",
                    parent,
                    position,
                    rotation,
                    $"{prefix}_{index + 1:00}_{length}m");

                if (wall.GetComponent<OccludableWall>() == null)
                {
                    wall.AddComponent<OccludableWall>();
                }

                BoxCollider collider = wall.GetComponent<BoxCollider>();

                if (collider == null)
                {
                    collider = wall.AddComponent<BoxCollider>();
                }

                collider.center = new Vector3(0f, 1.5f, 0f);
                collider.size = new Vector3(length, 3f, 0.2f);

                cursor += length;
            }
        }

        private static void BuildZones(Scene scene, Transform parent)
        {
            InstantiatePrefab(
                scene,
                "Assets/_Project/Prefabs/Architecture/ZoneBackroom.prefab",
                parent,
                new Vector3(0f, 0.01f, 5.8f),
                Quaternion.identity,
                "ZoneBackroom");
        }

        private static void ConfigureSceneLighting()
        {
            RenderSettings.ambientMode =
                UnityEngine.Rendering.AmbientMode.Trilight;
            RenderSettings.ambientSkyColor =
                new Color(0.42f, 0.39f, 0.31f);
            RenderSettings.ambientEquatorColor =
                new Color(0.22f, 0.24f, 0.22f);
            RenderSettings.ambientGroundColor =
                new Color(0.12f, 0.13f, 0.12f);
            RenderSettings.ambientIntensity = 0.82f;
        }

        private static void BuildLighting(Scene scene, Transform parent)
        {
            Vector3[] railPositions =
            {
                new Vector3(-2.5f, 2.85f, -4f),
                new Vector3(2.5f, 2.85f, -4f),
                new Vector3(-2.5f, 2.85f, 0.5f),
                new Vector3(2.5f, 2.85f, 0.5f),
                new Vector3(-2.5f, 2.85f, 5.5f),
                new Vector3(2.5f, 2.85f, 5.5f)
            };

            for (int index = 0; index < railPositions.Length; index++)
            {
                InstantiatePrefab(
                    scene,
                    "Assets/_Project/Prefabs/Architecture/LightRail200.prefab",
                    parent,
                    railPositions[index],
                    Quaternion.identity,
                    $"LightRail_{index + 1:00}");

                CreatePointLight(
                    $"GeneralLight_{index + 1:00}",
                    parent,
                    railPositions[index] + Vector3.down * 0.2f,
                    index >= 4 ? 3.5f : 4.4f,
                    index >= 4 ? 650f : 850f,
                    new Color(1f, 0.89f, 0.72f),
                    index == 2 || index == 3);
            }

        }

        private static void CreatePointLight(
            string name,
            Transform parent,
            Vector3 position,
            float range,
            float intensity,
            Color color,
            bool castsShadows)
        {
            GameObject lightObject = new GameObject(name);
            lightObject.transform.SetParent(parent, false);
            lightObject.transform.position = position;

            Light light = lightObject.AddComponent<Light>();
            light.type = LightType.Point;
            light.range = range;
            light.intensity = intensity;
            light.color = color;
            light.shadows = castsShadows
                ? LightShadows.Soft
                : LightShadows.None;
        }

        private static void BuildPartitionColliders(Transform parent)
        {
            CreateBoxCollider(
                "BackroomPartitionCollider_Left",
                parent,
                new Vector3(-3f, 1.5f, 4.5f),
                new Vector3(4f, 3f, 0.2f));

            CreateBoxCollider(
                "BackroomPartitionCollider_Right",
                parent,
                new Vector3(3f, 1.5f, 4.5f),
                new Vector3(4f, 3f, 0.2f));
        }

        private static void CreateBoxCollider(
            string name,
            Transform parent,
            Vector3 position,
            Vector3 size)
        {
            GameObject colliderObject = new GameObject(name);
            colliderObject.transform.SetParent(parent, false);
            colliderObject.transform.position = position;
            BoxCollider collider = colliderObject.AddComponent<BoxCollider>();
            collider.size = size;
        }

        private static StoreInitialSceneContext ConfigureSceneContext(
            Scene scene,
            SceneHierarchy hierarchy,
            EnvironmentBuild environment)
        {
            PlacementSurface surface = FindInScene<PlacementSurface>(scene, true);
            StoreShellDescriptor shell =
                FindInScene<StoreShellDescriptor>(scene, true);
            Camera camera = FindInScene<Camera>(scene, true);

            if (surface == null || shell == null || camera == null)
            {
                throw new InvalidOperationException(
                    "StoreInitial is missing PlacementSurface, " +
                    "StoreShellDescriptor or Camera.");
            }

            if (surface.GridWidth != GridWidth ||
                surface.GridDepth != GridDepth ||
                !Mathf.Approximately(surface.CellSize, CellSize))
            {
                throw new InvalidOperationException(
                    "The StoreInitial placement grid no longer matches " +
                    "the documented 20 x 30 cells at 0.5 m.");
            }

            WallOcclusionController wallOcclusion =
                hierarchy.Systems.GetComponent<WallOcclusionController>();

            if (wallOcclusion == null)
            {
                wallOcclusion = hierarchy.Systems.gameObject
                    .AddComponent<WallOcclusionController>();
            }

            GameObject contextObject =
                new GameObject("StoreInitialSceneContext");
            SceneManager.MoveGameObjectToScene(contextObject, scene);
            contextObject.transform.SetParent(hierarchy.Systems, false);

            StoreInitialSceneContext context =
                contextObject.AddComponent<StoreInitialSceneContext>();

            context.Configure(
                surface,
                shell,
                shell.PlayerSpawn,
                shell.TechnicalPlayer,
                environment.EntranceAnchor,
                environment.CheckoutAnchor,
                environment.ReceivingAnchor,
                environment.BackroomAnchor,
                environment.CustomerSpawns,
                environment.RequiredAccessAnchors,
                environment.Door,
                environment.DoorParts,
                camera,
                wallOcclusion,
                environment.Root,
                environment.InitialFurniture,
                hierarchy.DynamicFurniture,
                hierarchy.DynamicProducts,
                hierarchy.Customers,
                environment.Lighting);

            return context;
        }

        private static void UpdateRuntimeSettings()
        {
            UIRuntimeSettingsAsset uiSettings =
                AssetDatabase.LoadAssetAtPath<UIRuntimeSettingsAsset>(
                    UiSettingsPath);

            uiSettings.Configure(
                uiSettings.MainMenuSceneName,
                "StoreInitial",
                uiSettings.CurrencyCode,
                uiSettings.InitialCashCents,
                uiSettings.DayDurationSeconds,
                uiSettings.ShowTutorialOnNewGame);
            EditorUtility.SetDirty(uiSettings);

            StoreRuntimeSettingsAsset storeSettings =
                AssetDatabase.LoadAssetAtPath<StoreRuntimeSettingsAsset>(
                    StoreSettingsPath);

            SerializedObject serialized = new SerializedObject(storeSettings);
            serialized.FindProperty("_storeSceneName").stringValue =
                "StoreInitial";
            serialized.FindProperty("_buildBlockoutOnLoad").boolValue =
                false;
            serialized.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(storeSettings);
        }

        private static void UpdateBuildSettings()
        {
            EditorBuildSettings.scenes =
                ApprovedScenePaths
                    .Select(path =>
                        new EditorBuildSettingsScene(
                            path,
                            true))
                    .ToArray();

            UnityEngine.Object buildProfile =
                AssetDatabase.LoadMainAssetAtPath(
                    WindowsDevelopmentProfilePath);

            if (buildProfile == null)
            {
                throw new FileNotFoundException(
                    "Windows development build profile not found.",
                    WindowsDevelopmentProfilePath);
            }

            SerializedObject serializedProfile =
                new SerializedObject(buildProfile);
            SerializedProperty scenes =
                serializedProfile.FindProperty("m_Scenes");

            if (scenes == null)
            {
                throw new InvalidOperationException(
                    "Windows development build profile has no scene list.");
            }

            scenes.arraySize = ApprovedScenePaths.Length;

            for (int index = 0;
                 index < ApprovedScenePaths.Length;
                 index++)
            {
                SerializedProperty scene =
                    scenes.GetArrayElementAtIndex(index);
                scene.FindPropertyRelative("m_enabled").boolValue = true;
                scene.FindPropertyRelative("m_path").stringValue =
                    ApprovedScenePaths[index];
            }

            serializedProfile.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(buildProfile);
        }

        private static bool ValidateSettings(out string report)
        {
            UIRuntimeSettingsAsset uiSettings =
                AssetDatabase.LoadAssetAtPath<UIRuntimeSettingsAsset>(
                    UiSettingsPath);
            StoreRuntimeSettingsAsset storeSettings =
                AssetDatabase.LoadAssetAtPath<StoreRuntimeSettingsAsset>(
                    StoreSettingsPath);

            List<string> errors = new List<string>();

            if (uiSettings == null ||
                uiSettings.StoreSceneName != "StoreInitial")
            {
                errors.Add(
                    "UI runtime settings do not target StoreInitial.");
            }

            if (storeSettings == null ||
                storeSettings.StoreSceneName != "StoreInitial")
            {
                errors.Add(
                    "Store runtime settings do not target StoreInitial.");
            }

            if (storeSettings != null &&
                storeSettings.BuildBlockoutOnLoad)
            {
                errors.Add(
                    "Procedural blockout generation is still enabled.");
            }

            EditorBuildSettingsScene[] configuredScenes =
                EditorBuildSettings.scenes;

            bool buildSettingsMatch =
                configuredScenes.Length == ApprovedScenePaths.Length &&
                configuredScenes.All(scene => scene.enabled) &&
                configuredScenes
                    .Select(scene => scene.path)
                    .SequenceEqual(ApprovedScenePaths);

            if (!buildSettingsMatch)
            {
                errors.Add(
                    "Build Settings must contain only Bootstrap, MainMenu, " +
                    "StoreInitial and TestLab, in that order.");
            }

            if (!BuildProfileMatchesApprovedScenes())
            {
                errors.Add(
                    "Windows_Development build profile must contain only " +
                    "Bootstrap, MainMenu, StoreInitial and TestLab, in that order.");
            }

            report = errors.Count == 0
                ? "Runtime settings and Build Settings are valid."
                : string.Join("\n", errors);

            return errors.Count == 0;
        }

        private static bool BuildProfileMatchesApprovedScenes()
        {
            UnityEngine.Object buildProfile =
                AssetDatabase.LoadMainAssetAtPath(
                    WindowsDevelopmentProfilePath);

            if (buildProfile == null)
            {
                return false;
            }

            SerializedObject serializedProfile =
                new SerializedObject(buildProfile);
            SerializedProperty scenes =
                serializedProfile.FindProperty("m_Scenes");

            if (scenes == null ||
                scenes.arraySize != ApprovedScenePaths.Length)
            {
                return false;
            }

            for (int index = 0;
                 index < ApprovedScenePaths.Length;
                 index++)
            {
                SerializedProperty scene =
                    scenes.GetArrayElementAtIndex(index);
                SerializedProperty enabled =
                    scene.FindPropertyRelative("m_enabled");
                SerializedProperty path =
                    scene.FindPropertyRelative("m_path");

                if (enabled == null ||
                    path == null ||
                    !enabled.boolValue ||
                    !string.Equals(
                        path.stringValue,
                        ApprovedScenePaths[index],
                        StringComparison.Ordinal))
                {
                    return false;
                }
            }

            return true;
        }

        private static GameObject InstantiatePrefab(
            Scene scene,
            string path,
            Transform parent,
            Vector3 position,
            Quaternion rotation,
            string name,
            bool localSpace = false)
        {
            GameObject prefab =
                AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (prefab == null)
            {
                throw new FileNotFoundException(
                    "Prefab not found.",
                    path);
            }

            GameObject instance =
                (GameObject)PrefabUtility.InstantiatePrefab(prefab, scene);
            instance.name = name;
            instance.transform.SetParent(parent, false);

            if (localSpace)
            {
                instance.transform.localPosition = position;
                instance.transform.localRotation = rotation;
            }
            else
            {
                instance.transform.position = position;
                instance.transform.rotation = rotation;
            }

            return instance;
        }

        private static Transform CreateRoot(string name, Scene scene)
        {
            GameObject root = new GameObject(name);
            SceneManager.MoveGameObjectToScene(root, scene);
            return root.transform;
        }

        private static Transform FindOrCreateRoot(
            string name,
            Scene scene)
        {
            GameObject existing = FindRootByName(scene, name);
            return existing != null
                ? existing.transform
                : CreateRoot(name, scene);
        }

        private static void DestroyRootByName(
            Scene scene,
            string name)
        {
            GameObject root = FindRootByName(scene, name);

            if (root != null)
            {
                UnityEngine.Object.DestroyImmediate(root);
            }
        }

        private static void DestroyNamedObjectInScene(
            Scene scene,
            string name)
        {
            GameObject target = FindGameObjectInScene(scene, name);

            if (target != null)
            {
                UnityEngine.Object.DestroyImmediate(target);
            }
        }

        private static void DestroyComponentsWithGameObjects<T>(
            Scene scene)
            where T : Component
        {
            foreach (T component in FindAllInScene<T>(scene, true))
            {
                if (component != null)
                {
                    UnityEngine.Object.DestroyImmediate(
                        component.gameObject);
                }
            }
        }

        private static void ClearChildren(Transform parent)
        {
            if (parent == null)
            {
                return;
            }

            for (int index = parent.childCount - 1;
                 index >= 0;
                 index--)
            {
                UnityEngine.Object.DestroyImmediate(
                    parent.GetChild(index).gameObject);
            }
        }

        private static void ClearChildrenExcept(
            Transform parent,
            params string[] preservedNames)
        {
            if (parent == null)
            {
                return;
            }

            HashSet<string> preserved =
                new HashSet<string>(
                    preservedNames ?? Array.Empty<string>(),
                    StringComparer.Ordinal);

            for (int index = parent.childCount - 1;
                 index >= 0;
                 index--)
            {
                Transform child = parent.GetChild(index);

                if (!preserved.Contains(child.name))
                {
                    UnityEngine.Object.DestroyImmediate(
                        child.gameObject);
                }
            }
        }

        private static Transform CreateChild(
            string name,
            Transform parent)
        {
            GameObject child = new GameObject(name);
            child.transform.SetParent(parent, false);
            return child.transform;
        }

        private static Transform CreateAnchor(
            string name,
            Transform parent,
            Vector3 worldPosition)
        {
            Transform anchor = CreateChild(name, parent);
            anchor.position = worldPosition;
            return anchor;
        }

        private static Transform CreateCellAnchor(
            string name,
            Transform parent,
            int cellX,
            int cellZ)
        {
            return CreateAnchor(
                name,
                parent,
                new Vector3(
                    GridOriginX + (cellX + 0.5f) * CellSize,
                    0f,
                    GridOriginZ + (cellZ + 0.5f) * CellSize));
        }

        private static void EnsureFolder(string path)
        {
            string[] parts = path.Split('/');
            string current = parts[0];

            for (int index = 1; index < parts.Length; index++)
            {
                string next = current + "/" + parts[index];

                if (!AssetDatabase.IsValidFolder(next))
                {
                    AssetDatabase.CreateFolder(current, parts[index]);
                }

                current = next;
            }
        }

        private static void RequireAsset<T>(
            string path,
            ICollection<string> missing)
            where T : UnityEngine.Object
        {
            if (AssetDatabase.LoadAssetAtPath<T>(path) == null)
            {
                missing.Add(path);
            }
        }

        private static IEnumerable<string> RequiredPrefabPaths()
        {
            string architecture =
                "Assets/_Project/Prefabs/Architecture/";

            return new[]
            {
                architecture + "Floor100.prefab",
                architecture + "Floor200.prefab",
                architecture + "Wall100.prefab",
                architecture + "Wall200.prefab",
                architecture + "Wall400.prefab",
                architecture + "AutomaticDoor.prefab",
                architecture + "BackroomPartition.prefab",
                architecture + "EntranceThreshold.prefab",
                architecture + "LightRail200.prefab",
                architecture + "StorefrontFacade.prefab",
                architecture + "StoreSign.prefab",
                architecture + "ZoneBackroom.prefab"
            };
        }

        private static T FindInScene<T>(Scene scene, bool includeInactive)
            where T : Component
        {
            foreach (GameObject root in scene.GetRootGameObjects())
            {
                T component = root.GetComponentInChildren<T>(includeInactive);

                if (component != null)
                {
                    return component;
                }
            }

            return null;
        }

        private static T[] FindAllInScene<T>(Scene scene, bool includeInactive)
            where T : Component
        {
            List<T> result = new List<T>();

            foreach (GameObject root in scene.GetRootGameObjects())
            {
                result.AddRange(
                    root.GetComponentsInChildren<T>(includeInactive));
            }

            return result.ToArray();
        }

        private static GameObject FindGameObjectInScene(
            Scene scene,
            string name)
        {
            foreach (GameObject root in scene.GetRootGameObjects())
            {
                foreach (Transform candidate in
                         root.GetComponentsInChildren<Transform>(true))
                {
                    if (string.Equals(
                            candidate.name,
                            name,
                            StringComparison.Ordinal))
                    {
                        return candidate.gameObject;
                    }
                }
            }

            return null;
        }

        private static GameObject FindRootByName(Scene scene, string name)
        {
            return scene.GetRootGameObjects().FirstOrDefault(root =>
                string.Equals(root.name, name, StringComparison.Ordinal));
        }

        private static Transform FindDescendant(
            Transform root,
            string name)
        {
            if (root == null)
            {
                return null;
            }

            foreach (Transform descendant in
                     root.GetComponentsInChildren<Transform>(true))
            {
                if (string.Equals(
                        descendant.name,
                        name,
                        StringComparison.Ordinal))
                {
                    return descendant;
                }
            }

            return null;
        }

        private static bool IsWithin(
            Transform candidate,
            Transform root)
        {
            return candidate != null &&
                   root != null &&
                   (candidate == root || candidate.IsChildOf(root));
        }

        private readonly struct SceneHierarchy
        {
            public SceneHierarchy(
                Transform systems,
                Transform cameras,
                Transform ui,
                Transform environment,
                Transform dynamicFurniture,
                Transform dynamicProducts,
                Transform customers,
                Transform debug)
            {
                Systems = systems;
                Cameras = cameras;
                UI = ui;
                Environment = environment;
                DynamicFurniture = dynamicFurniture;
                DynamicProducts = dynamicProducts;
                Customers = customers;
                Debug = debug;
            }

            public Transform Systems { get; }
            public Transform Cameras { get; }
            public Transform UI { get; }
            public Transform Environment { get; }
            public Transform DynamicFurniture { get; }
            public Transform DynamicProducts { get; }
            public Transform Customers { get; }
            public Transform Debug { get; }
        }

        private readonly struct EnvironmentBuild
        {
            public EnvironmentBuild(
                GameObject root,
                Transform initialFurniture,
                Transform lighting,
                Transform entranceAnchor,
                Transform checkoutAnchor,
                Transform receivingAnchor,
                Transform backroomAnchor,
                Transform[] customerSpawns,
                Transform[] requiredAccessAnchors,
                AutomaticSlidingDoorController door,
                AutomaticDoorParts doorParts)
            {
                Root = root;
                InitialFurniture = initialFurniture;
                Lighting = lighting;
                EntranceAnchor = entranceAnchor;
                CheckoutAnchor = checkoutAnchor;
                ReceivingAnchor = receivingAnchor;
                BackroomAnchor = backroomAnchor;
                CustomerSpawns = customerSpawns;
                RequiredAccessAnchors = requiredAccessAnchors;
                Door = door;
                DoorParts = doorParts;
            }

            public GameObject Root { get; }
            public Transform InitialFurniture { get; }
            public Transform Lighting { get; }
            public Transform EntranceAnchor { get; }
            public Transform CheckoutAnchor { get; }
            public Transform ReceivingAnchor { get; }
            public Transform BackroomAnchor { get; }
            public Transform[] CustomerSpawns { get; }
            public Transform[] RequiredAccessAnchors { get; }
            public AutomaticSlidingDoorController Door { get; }
            public AutomaticDoorParts DoorParts { get; }
        }
    }
}
