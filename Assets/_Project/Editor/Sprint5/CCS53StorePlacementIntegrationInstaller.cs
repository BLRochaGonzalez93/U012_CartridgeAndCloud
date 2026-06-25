using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRMGames.CartridgeAndCloud.Application.Access;
using VRMGames.CartridgeAndCloud.Infrastructure.InputSystem.Actions;
using VRMGames.CartridgeAndCloud.Infrastructure.InputSystem.Placement;
using VRMGames.CartridgeAndCloud.Presentation.Placement;

namespace VRMGames.CartridgeAndCloud.Editor.Sprint5
{
    public static class CCS53StorePlacementIntegrationInstaller
    {
        private const string StoreScenePath =
            "Assets/_Project/Scenes/Store.unity";

        private const string ShellRootName =
            "S5_StoreShell";

        private const string FloorName =
            "WalkableFloor";

        private const string PlacementRootName =
            "S5_StorePlacement";

        private const string GhostVisualName =
            "GhostVisual";

        private const string PlacedObjectsName =
            "PlacedObjects";

        private const string AccessMarkersName =
            "AccessMarkers";

        private const string DefinitionFolder =
            "Assets/_Project/Data/Placement/Technical";

        private const string DefinitionPath =
            DefinitionFolder +
            "/CC_S5_TechnicalShelf4x2.asset";

        private const string MaterialFolder =
            "Assets/_Project/Art/Materials/Technical";

        private const string ValidMaterialPath =
            MaterialFolder +
            "/CC_S5_PlacementValid.mat";

        private const string InvalidMaterialPath =
            MaterialFolder +
            "/CC_S5_PlacementInvalid.mat";

        private const string PlacedMaterialPath =
            MaterialFolder +
            "/CC_S5_PlacedTechnical.mat";

        private const string EntranceMarkerMaterialPath =
            MaterialFolder +
            "/CC_S5_EntranceReserveMarker.mat";

        private const string AnchorMarkerMaterialPath =
            MaterialFolder +
            "/CC_S5_AccessAnchorMarker.mat";

        private const string ExpectedVersion =
            "0.0.6";

        private static readonly Vector3 GridOrigin =
            new Vector3(
                -5f,
                0f,
                -7.5f);

        [MenuItem(
            "Cartridge & Cloud/Sprint 5/" +
            "CC_S5.3 Build Store Placement Integration")]
        public static void BuildStorePlacementIntegration()
        {
            bool confirmed =
                EditorUtility.DisplayDialog(
                    "Build CC_S5.3 Store Placement",
                    "This replaces only the S5_StorePlacement child " +
                    "inside S5_StoreShell and clears technical placed " +
                    "objects. The Store shell and approved scene roots " +
                    "remain unchanged.",
                    "Build",
                    "Cancel");

            if (!confirmed)
            {
                return;
            }

            Scene scene =
                EditorSceneManager.OpenScene(
                    StoreScenePath,
                    OpenSceneMode.Single);

            GameObject shellRoot =
                GameObject.Find(
                    ShellRootName);

            Transform floorTransform =
                shellRoot != null
                    ? shellRoot.transform.Find(
                        FloorName)
                    : null;

            UnityEngine.Camera camera =
                UnityEngine.Camera.main;

            GameplayInputActionDriver gameplayDriver =
                camera != null
                    ? camera.GetComponent<
                        GameplayInputActionDriver>()
                    : null;

            BoxCollider floorCollider =
                floorTransform != null
                    ? floorTransform.GetComponent<
                        BoxCollider>()
                    : null;

            if (shellRoot == null ||
                floorTransform == null ||
                floorCollider == null ||
                camera == null ||
                gameplayDriver == null)
            {
                Debug.LogError(
                    "[CC_S5.3] Store shell, floor collider, " +
                    "Main Camera or GameplayInputActionDriver " +
                    "was not found. Integrate CC_S5.1 first.");

                return;
            }

            RemoveExistingPlacementRoot(
                shellRoot.transform);

            PlacementSurface surface =
                floorTransform.GetComponent<
                    PlacementSurface>();

            if (surface == null)
            {
                surface =
                    Undo.AddComponent<
                        PlacementSurface>(
                            floorTransform.gameObject);
            }

            surface.Configure(
                floorCollider,
                GridOrigin,
                gridWidth: 20,
                gridDepth: 30,
                cellSize: 0.5f,
                raycastDistance: 500f);

            TechnicalPlaceableDefinition definition =
                CreateOrUpdateDefinition();

            Material validMaterial =
                CreateOrUpdateMaterial(
                    ValidMaterialPath,
                    new Color(
                        0.12f,
                        0.72f,
                        0.35f,
                        1f));

            Material invalidMaterial =
                CreateOrUpdateMaterial(
                    InvalidMaterialPath,
                    new Color(
                        0.82f,
                        0.12f,
                        0.12f,
                        1f));

            Material placedMaterial =
                CreateOrUpdateMaterial(
                    PlacedMaterialPath,
                    new Color(
                        0.16f,
                        0.42f,
                        0.75f,
                        1f));

            Material entranceMarkerMaterial =
                CreateOrUpdateMaterial(
                    EntranceMarkerMaterialPath,
                    new Color(
                        0.12f,
                        0.72f,
                        0.7f,
                        1f));

            Material anchorMarkerMaterial =
                CreateOrUpdateMaterial(
                    AnchorMarkerMaterialPath,
                    new Color(
                        0.95f,
                        0.72f,
                        0.12f,
                        1f));

            if (definition == null ||
                validMaterial == null ||
                invalidMaterial == null ||
                placedMaterial == null ||
                entranceMarkerMaterial == null ||
                anchorMarkerMaterial == null)
            {
                return;
            }

            GameObject placementRoot =
                new GameObject(
                    PlacementRootName);

            placementRoot.transform.SetParent(
                shellRoot.transform,
                worldPositionStays: false);

            Undo.RegisterCreatedObjectUndo(
                placementRoot,
                "Create CC_S5.3 placement root");

            GameObject ghostVisual =
                CreateGhostVisual(
                    placementRoot.transform);

            Transform placedObjectsRoot =
                CreateEmptyChild(
                    placementRoot.transform,
                    PlacedObjectsName);

            StoreAccessLayout accessLayout =
                StoreAccessLayout.InitialTier();

            CreateAccessMarkers(
                placementRoot.transform,
                accessLayout,
                entranceMarkerMaterial,
                anchorMarkerMaterial);

            PlacementGhostView ghostView =
                placementRoot.AddComponent<
                    PlacementGhostView>();

            ghostView.Configure(
                ghostVisual.transform,
                ghostVisual.GetComponents<
                    Renderer>(),
                validMaterial,
                invalidMaterial);

            PlacementPreviewController previewController =
                placementRoot.AddComponent<
                    PlacementPreviewController>();

            previewController.Configure(
                surface,
                definition,
                ghostView,
                camera);

            PlacementRuntimeController runtimeController =
                placementRoot.AddComponent<
                    PlacementRuntimeController>();

            runtimeController.Configure(
                surface,
                previewController,
                definition,
                placedObjectsRoot,
                placedMaterial,
                camera);

            runtimeController.ConfigureInitialStoreAccessValidation();

            runtimeController.SetPlacementMode(
                false);

            PlacementInputActionDriver placementDriver =
                placementRoot.AddComponent<
                    PlacementInputActionDriver>();

            placementDriver.Configure(
                contextRouter: null,
                previewController: previewController,
                runtimeController: runtimeController);

            gameplayDriver.SetPlacementRuntimeController(
                runtimeController);

            EditorUtility.SetDirty(
                floorTransform.gameObject);

            EditorUtility.SetDirty(
                placementRoot);

            EditorUtility.SetDirty(
                camera.gameObject);

            AssetDatabase.SaveAssets();

            EditorSceneManager.MarkSceneDirty(
                scene);

            EditorSceneManager.SaveScene(
                scene);

            Selection.activeGameObject =
                placementRoot;

            EditorGUIUtility.PingObject(
                placementRoot);

            Debug.Log(
                "[CC_S5.3] Store placement integration installed. " +
                "B toggles placement, left click confirms, " +
                "Q/E rotate, Escape cancels and Delete removes. " +
                "Access-blocking candidates are shown in red.");
        }

        [MenuItem(
            "Cartridge & Cloud/Sprint 5/" +
            "CC_S5.3 Validate Store Placement Integration")]
        public static void ValidateStorePlacementIntegration()
        {
            Scene scene =
                EditorSceneManager.OpenScene(
                    StoreScenePath,
                    OpenSceneMode.Single);

            GameObject shellRoot =
                GameObject.Find(
                    ShellRootName);

            Transform floorTransform =
                shellRoot != null
                    ? shellRoot.transform.Find(
                        FloorName)
                    : null;

            Transform placementRoot =
                shellRoot != null
                    ? shellRoot.transform.Find(
                        PlacementRootName)
                    : null;

            PlacementSurface surface =
                floorTransform != null
                    ? floorTransform.GetComponent<
                        PlacementSurface>()
                    : null;

            PlacementGhostView ghostView =
                placementRoot != null
                    ? placementRoot.GetComponent<
                        PlacementGhostView>()
                    : null;

            PlacementPreviewController previewController =
                placementRoot != null
                    ? placementRoot.GetComponent<
                        PlacementPreviewController>()
                    : null;

            PlacementRuntimeController runtimeController =
                placementRoot != null
                    ? placementRoot.GetComponent<
                        PlacementRuntimeController>()
                    : null;

            PlacementInputActionDriver placementDriver =
                placementRoot != null
                    ? placementRoot.GetComponent<
                        PlacementInputActionDriver>()
                    : null;

            Transform ghostVisual =
                placementRoot != null
                    ? placementRoot.Find(
                        GhostVisualName)
                    : null;

            Transform placedObjects =
                placementRoot != null
                    ? placementRoot.Find(
                        PlacedObjectsName)
                    : null;

            Transform accessMarkers =
                placementRoot != null
                    ? placementRoot.Find(
                        AccessMarkersName)
                    : null;

            UnityEngine.Camera camera =
                UnityEngine.Camera.main;

            GameplayInputActionDriver gameplayDriver =
                camera != null
                    ? camera.GetComponent<
                        GameplayInputActionDriver>()
                    : null;

            bool actionsValid;

            using (ProjectInputActions actions =
                   new ProjectInputActions())
            {
                actionsValid =
                    actions.TogglePlacementMode != null &&
                    actions.SetDestination != null &&
                    actions
                        .RotatePlacementCounterClockwise != null &&
                    actions
                        .RotatePlacementClockwise != null &&
                    actions.CancelPlacement != null &&
                    actions.RemovePlacement != null;
            }

            bool layoutValid =
                runtimeController != null &&
                runtimeController.IsAccessValidationEnabled &&
                runtimeController.AccessLayout != null;

            bool valid =
                scene.IsValid() &&
                HasExpectedRootObjects(scene) &&
                shellRoot != null &&
                shellRoot.transform.parent == null &&
                floorTransform != null &&
                placementRoot != null &&
                placementRoot.parent ==
                    shellRoot.transform &&
                surface != null &&
                surface.SurfaceCollider != null &&
                surface.GridOrigin ==
                    GridOrigin &&
                surface.GridWidth == 20 &&
                surface.GridDepth == 30 &&
                Mathf.Approximately(
                    surface.CellSize,
                    0.5f) &&
                ghostView != null &&
                previewController != null &&
                previewController.Surface ==
                    surface &&
                previewController.Definition != null &&
                previewController.Definition.DefinitionId ==
                    "technical-shelf-4x2" &&
                previewController.Definition.WidthCells ==
                    4 &&
                previewController.Definition.DepthCells ==
                    2 &&
                runtimeController != null &&
                runtimeController.PlacedCount == 0 &&
                !runtimeController.IsPlacementModeActive &&
                layoutValid &&
                placementDriver != null &&
                placementDriver.PreviewController ==
                    previewController &&
                placementDriver.RuntimeController ==
                    runtimeController &&
                gameplayDriver != null &&
                gameplayDriver.PlacementRuntimeController ==
                    runtimeController &&
                ghostVisual != null &&
                !ghostVisual.gameObject.activeSelf &&
                placedObjects != null &&
                placedObjects.childCount == 0 &&
                accessMarkers != null &&
                accessMarkers.childCount == 7 &&
                actionsValid &&
                PlayerSettings.bundleVersion ==
                    ExpectedVersion;

            EditorUtility.DisplayDialog(
                "CC_S5.3 Store Placement Validation",
                valid
                    ? "PASS: Store placement, occupancy, access " +
                      "validation, input routing and scene roots are correct."
                    : "FAIL: run the CC_S5.3 build command " +
                      "and review the Console.",
                "OK");

            if (!valid)
            {
                Debug.LogError(
                    "[CC_S5.3] Validation failed. Check Store shell, " +
                    "placement surface, ghost, runtime, access layout, " +
                    "input routing and technical assets.");
            }
        }

        private static TechnicalPlaceableDefinition
            CreateOrUpdateDefinition()
        {
            EnsureFolder(
                DefinitionFolder);

            TechnicalPlaceableDefinition definition =
                AssetDatabase.LoadAssetAtPath<
                    TechnicalPlaceableDefinition>(
                        DefinitionPath);

            if (definition == null)
            {
                definition =
                    ScriptableObject.CreateInstance<
                        TechnicalPlaceableDefinition>();

                AssetDatabase.CreateAsset(
                    definition,
                    DefinitionPath);
            }

            definition.Configure(
                "technical-shelf-4x2",
                widthCells: 4,
                depthCells: 2,
                previewHeight: 1.2f);

            EditorUtility.SetDirty(
                definition);

            return definition;
        }

        private static GameObject CreateGhostVisual(
            Transform parent)
        {
            GameObject ghost =
                GameObject.CreatePrimitive(
                    PrimitiveType.Cube);

            ghost.name =
                GhostVisualName;

            ghost.layer =
                2;

            ghost.transform.SetParent(
                parent,
                worldPositionStays: false);

            Collider collider =
                ghost.GetComponent<
                    Collider>();

            if (collider != null)
            {
                UnityEngine.Object.DestroyImmediate(
                    collider);
            }

            ghost.SetActive(
                false);

            Undo.RegisterCreatedObjectUndo(
                ghost,
                "Create CC_S5.3 ghost visual");

            return ghost;
        }

        private static Transform CreateEmptyChild(
            Transform parent,
            string name)
        {
            GameObject child =
                new GameObject(
                    name);

            child.transform.SetParent(
                parent,
                worldPositionStays: false);

            Undo.RegisterCreatedObjectUndo(
                child,
                $"Create {name}");

            return child.transform;
        }

        private static void CreateAccessMarkers(
            Transform parent,
            StoreAccessLayout layout,
            Material entranceMaterial,
            Material anchorMaterial)
        {
            if (layout == null)
            {
                Debug.LogError(
                    "[CC_S5.3] Initial access layout was not created.");

                return;
            }

            Transform markersRoot =
                CreateEmptyChild(
                    parent,
                    AccessMarkersName);

            CreateMarker(
                markersRoot,
                "EntranceReserve_8_0",
                cellX: 8,
                cellZ: 0,
                material: entranceMaterial,
                scale: new Vector3(0.42f, 0.03f, 0.42f));

            CreateMarker(
                markersRoot,
                "EntranceReserve_9_0",
                cellX: 9,
                cellZ: 0,
                material: entranceMaterial,
                scale: new Vector3(0.42f, 0.03f, 0.42f));

            CreateMarker(
                markersRoot,
                "EntranceReserve_10_0",
                cellX: 10,
                cellZ: 0,
                material: entranceMaterial,
                scale: new Vector3(0.42f, 0.03f, 0.42f));

            CreateMarker(
                markersRoot,
                "EntranceReserve_11_0",
                cellX: 11,
                cellZ: 0,
                material: entranceMaterial,
                scale: new Vector3(0.42f, 0.03f, 0.42f));

            CreateMarker(
                markersRoot,
                "RequiredAnchor_rear-service",
                cellX: 10,
                cellZ: 27,
                material: anchorMaterial,
                scale: new Vector3(0.3f, 0.12f, 0.3f));

            CreateMarker(
                markersRoot,
                "RequiredAnchor_left-display",
                cellX: 3,
                cellZ: 15,
                material: anchorMaterial,
                scale: new Vector3(0.3f, 0.12f, 0.3f));

            CreateMarker(
                markersRoot,
                "RequiredAnchor_right-display",
                cellX: 16,
                cellZ: 15,
                material: anchorMaterial,
                scale: new Vector3(0.3f, 0.12f, 0.3f));
        }

        private static void CreateMarker(
            Transform parent,
            string name,
            int cellX,
            int cellZ,
            Material material,
            Vector3 scale)
        {
            float worldX =
                GridOrigin.x +
                (cellX + 0.5f) * 0.5f;

            float worldZ =
                GridOrigin.z +
                (cellZ + 0.5f) * 0.5f;

            GameObject marker =
                GameObject.CreatePrimitive(
                    PrimitiveType.Cube);

            marker.name =
                name;

            marker.layer =
                2;

            marker.transform.SetParent(
                parent,
                worldPositionStays: true);

            marker.transform.position =
                new Vector3(
                    worldX,
                    scale.y * 0.5f + 0.01f,
                    worldZ);

            marker.transform.rotation =
                Quaternion.identity;

            marker.transform.localScale =
                scale;

            Collider collider =
                marker.GetComponent<
                    Collider>();

            if (collider != null)
            {
                UnityEngine.Object.DestroyImmediate(
                    collider);
            }

            Renderer renderer =
                marker.GetComponent<
                    Renderer>();

            if (renderer != null)
            {
                renderer.sharedMaterial =
                    material;
            }

            Undo.RegisterCreatedObjectUndo(
                marker,
                "Create CC_S5.3 access marker");
        }

        private static Material CreateOrUpdateMaterial(
            string path,
            Color color)
        {
            EnsureFolder(
                MaterialFolder);

            Shader shader =
                Shader.Find(
                    "Universal Render Pipeline/Lit");

            if (shader == null)
            {
                shader =
                    Shader.Find(
                        "Universal Render Pipeline/Unlit");
            }

            if (shader == null)
            {
                shader =
                    Shader.Find(
                        "Standard");
            }

            if (shader == null)
            {
                Debug.LogError(
                    "[CC_S5.3] No compatible shader was found.");

                return null;
            }

            Material material =
                AssetDatabase.LoadAssetAtPath<
                    Material>(path);

            if (material == null)
            {
                material =
                    new Material(
                        shader);

                AssetDatabase.CreateAsset(
                    material,
                    path);
            }
            else
            {
                material.shader =
                    shader;
            }

            material.color =
                color;

            if (material.HasProperty(
                    "_BaseColor"))
            {
                material.SetColor(
                    "_BaseColor",
                    color);
            }

            EditorUtility.SetDirty(
                material);

            return material;
        }

        private static void EnsureFolder(
            string assetFolder)
        {
            if (AssetDatabase.IsValidFolder(
                    assetFolder))
            {
                return;
            }

            Directory.CreateDirectory(
                assetFolder);

            AssetDatabase.Refresh();
        }

        private static void RemoveExistingPlacementRoot(
            Transform shellRoot)
        {
            Transform existing =
                shellRoot.Find(
                    PlacementRootName);

            if (existing != null)
            {
                Undo.DestroyObjectImmediate(
                    existing.gameObject);
            }

            Transform floor =
                shellRoot.Find(
                    FloorName);

            PlacementSurface existingSurface =
                floor != null
                    ? floor.GetComponent<
                        PlacementSurface>()
                    : null;

            if (existingSurface != null)
            {
                Undo.DestroyObjectImmediate(
                    existingSurface);
            }
        }

        private static bool HasExpectedRootObjects(
            Scene scene)
        {
            GameObject[] roots =
                scene.GetRootGameObjects();

            if (roots.Length != 6)
            {
                return false;
            }

            string[] expectedNames =
            {
                "Main Camera",
                "Directional Light",
                "StoreSceneController",
                "Canvas",
                "EventSystem",
                ShellRootName
            };

            foreach (string expectedName in
                     expectedNames)
            {
                bool found = false;

                foreach (GameObject rootObject in
                         roots)
                {
                    if (rootObject.name ==
                        expectedName)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
