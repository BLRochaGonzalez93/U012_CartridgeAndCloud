using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRMGames.CartridgeAndCloud.Infrastructure.InputSystem.Actions;
using VRMGames.CartridgeAndCloud.Infrastructure.InputSystem.Placement;
using VRMGames.CartridgeAndCloud.Presentation.Placement;

namespace VRMGames.CartridgeAndCloud.Editor.Sprint4
{
    public static class CCS43OccupancyInstaller
    {
        private const string TestLabScenePath =
            "Assets/_Project/Scenes/TestLab.unity";

        private const string S3RootName =
            "S3_ClickToMoveLab";

        private const string S4RootName =
            "S4_PlacementPreviewLab";

        private const string PlacedObjectsName =
            "PlacedObjects";

        private const string PlacedMaterialFolder =
            "Assets/_Project/Art/Materials/Technical";

        private const string PlacedMaterialPath =
            PlacedMaterialFolder +
            "/CC_S4_PlacedTechnical.mat";

        [MenuItem(
            "Cartridge & Cloud/Sprint 4/" +
            "CC_S4.3 Build Occupancy Validation TestLab")]
        public static void BuildTestLab()
        {
            bool confirmed =
                EditorUtility.DisplayDialog(
                    "Build CC_S4.3 TestLab",
                    "This upgrades the existing CC_S4.2 preview " +
                    "with placement mode, confirmation, overlap " +
                    "validation and removal. Existing runtime placed " +
                    "objects are cleared.",
                    "Build",
                    "Cancel");

            if (!confirmed)
            {
                return;
            }

            Scene scene =
                EditorSceneManager.OpenScene(
                    TestLabScenePath,
                    OpenSceneMode.Single);

            GameObject s3Root =
                GameObject.Find(S3RootName);

            if (s3Root == null)
            {
                Debug.LogError(
                    "[CC_S4.3] S3_ClickToMoveLab was not found.");
                return;
            }

            Transform s4RootTransform =
                s3Root.transform.Find(S4RootName);

            if (s4RootTransform == null)
            {
                Debug.LogError(
                    "[CC_S4.3] S4_PlacementPreviewLab was not found. " +
                    "Run the CC_S4.2 installer first.");
                return;
            }

            GameObject s4Root =
                s4RootTransform.gameObject;

            PlacementPreviewController previewController =
                s4Root.GetComponent<
                    PlacementPreviewController>();

            PlacementInputActionDriver placementDriver =
                s4Root.GetComponent<
                    PlacementInputActionDriver>();

            PlacementSurface surface =
                previewController != null
                    ? previewController.Surface
                    : null;

            TechnicalPlaceableDefinition definition =
                previewController != null
                    ? previewController.Definition
                    : null;

            UnityEngine.Camera camera =
                UnityEngine.Camera.main;

            if (previewController == null ||
                placementDriver == null ||
                surface == null ||
                definition == null ||
                camera == null)
            {
                Debug.LogError(
                    "[CC_S4.3] Required CC_S4.2 components are missing.");
                return;
            }

            definition.Configure(
                "technical-shelf-4x2",
                widthCells: 4,
                depthCells: 2,
                previewHeight: 1.2f);

            EditorUtility.SetDirty(definition);

            Transform placedObjectsRoot =
                EnsurePlacedObjectsRoot(
                    s4Root.transform);

            ClearChildren(placedObjectsRoot);

            Material placedMaterial =
                CreateOrUpdatePlacedMaterial();

            if (placedMaterial == null)
            {
                return;
            }

            PlacementRuntimeController runtime =
                s4Root.GetComponent<
                    PlacementRuntimeController>();

            if (runtime == null)
            {
                runtime =
                    Undo.AddComponent<
                        PlacementRuntimeController>(
                            s4Root);
            }

            runtime.Configure(
                surface,
                previewController,
                definition,
                placedObjectsRoot,
                placedMaterial,
                camera);

            runtime.SetPlacementMode(false);

            placementDriver.Configure(
                contextRouter: null,
                previewController: previewController,
                runtimeController: runtime);

            GameplayInputActionDriver gameplayDriver =
                camera.GetComponent<
                    GameplayInputActionDriver>();

            if (gameplayDriver == null)
            {
                Debug.LogError(
                    "[CC_S4.3] GameplayInputActionDriver was not found " +
                    "on Main Camera.");
                return;
            }

            gameplayDriver.SetPlacementRuntimeController(
                runtime);

            EditorUtility.SetDirty(s4Root);
            EditorUtility.SetDirty(camera.gameObject);

            AssetDatabase.SaveAssets();
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);

            Selection.activeGameObject = s4Root;
            EditorGUIUtility.PingObject(s4Root);

            Debug.Log(
                "[CC_S4.3] Occupancy validation installed. " +
                "B toggles placement mode, left click confirms, " +
                "Escape cancels and Delete removes.");
        }

        [MenuItem(
            "Cartridge & Cloud/Sprint 4/" +
            "CC_S4.3 Validate Occupancy TestLab")]
        public static void ValidateTestLab()
        {
            Scene scene =
                EditorSceneManager.OpenScene(
                    TestLabScenePath,
                    OpenSceneMode.Single);

            GameObject s3Root =
                GameObject.Find(S3RootName);

            Transform s4Root =
                s3Root != null
                    ? s3Root.transform.Find(S4RootName)
                    : null;

            PlacementPreviewController previewController =
                s4Root != null
                    ? s4Root.GetComponent<
                        PlacementPreviewController>()
                    : null;

            PlacementInputActionDriver placementDriver =
                s4Root != null
                    ? s4Root.GetComponent<
                        PlacementInputActionDriver>()
                    : null;

            PlacementRuntimeController runtime =
                s4Root != null
                    ? s4Root.GetComponent<
                        PlacementRuntimeController>()
                    : null;

            Transform placedObjects =
                s4Root != null
                    ? s4Root.Find(PlacedObjectsName)
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
                    actions.CancelPlacement != null &&
                    actions.RemovePlacement != null &&
                    actions.RotatePlacementCounterClockwise != null &&
                    actions.RotatePlacementClockwise != null;
            }

            bool valid =
                scene.IsValid() &&
                HasApprovedRootObjects(scene) &&
                s3Root != null &&
                s4Root != null &&
                previewController != null &&
                previewController.Surface != null &&
                previewController.Definition != null &&
                previewController.Definition.DefinitionId ==
                    "technical-shelf-4x2" &&
                placementDriver != null &&
                runtime != null &&
                placementDriver.RuntimeController == runtime &&
                placedObjects != null &&
                gameplayDriver != null &&
                gameplayDriver.PlacementRuntimeController == runtime &&
                actionsValid;

            EditorUtility.DisplayDialog(
                "CC_S4.3 Occupancy Validation",
                valid
                    ? "PASS: placement mode, occupancy runtime, " +
                      "input routing and approved TestLab roots are correct."
                    : "FAIL: run the CC_S4.3 build command.",
                "OK");
        }

        private static Transform EnsurePlacedObjectsRoot(
            Transform parent)
        {
            Transform existing =
                parent.Find(PlacedObjectsName);

            if (existing != null)
            {
                return existing;
            }

            GameObject root =
                new GameObject(PlacedObjectsName);

            root.transform.SetParent(
                parent,
                worldPositionStays: false);

            Undo.RegisterCreatedObjectUndo(
                root,
                "Create CC_S4.3 placed objects root");

            return root.transform;
        }

        private static void ClearChildren(Transform parent)
        {
            for (int index =
                     parent.childCount - 1;
                 index >= 0;
                 index--)
            {
                Undo.DestroyObjectImmediate(
                    parent.GetChild(index).gameObject);
            }
        }

        private static Material CreateOrUpdatePlacedMaterial()
        {
            EnsureFolder(PlacedMaterialFolder);

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
                    Shader.Find("Standard");
            }

            if (shader == null)
            {
                Debug.LogError(
                    "[CC_S4.3] No compatible shader was found.");
                return null;
            }

            Material material =
                AssetDatabase.LoadAssetAtPath<
                    Material>(PlacedMaterialPath);

            if (material == null)
            {
                material =
                    new Material(shader);

                AssetDatabase.CreateAsset(
                    material,
                    PlacedMaterialPath);
            }
            else
            {
                material.shader = shader;
            }

            Color color =
                new Color(
                    0.15f,
                    0.45f,
                    0.75f,
                    1f);

            material.color = color;

            if (material.HasProperty("_BaseColor"))
            {
                material.SetColor(
                    "_BaseColor",
                    color);
            }

            EditorUtility.SetDirty(material);
            return material;
        }

        private static void EnsureFolder(string assetFolder)
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

        private static bool HasApprovedRootObjects(
            Scene scene)
        {
            GameObject[] roots =
                scene.GetRootGameObjects();

            if (roots.Length != 2)
            {
                return false;
            }

            bool hasMainCamera = false;
            bool hasDirectionalLight = false;

            foreach (GameObject rootObject in roots)
            {
                switch (rootObject.name)
                {
                    case "Main Camera":
                        hasMainCamera = true;
                        break;

                    case "Directional Light":
                        hasDirectionalLight = true;
                        break;

                    default:
                        return false;
                }
            }

            return hasMainCamera &&
                   hasDirectionalLight;
        }
    }
}
