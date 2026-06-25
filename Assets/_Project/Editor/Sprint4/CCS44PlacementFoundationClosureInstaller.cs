using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRMGames.CartridgeAndCloud.Infrastructure.InputSystem.Actions;
using VRMGames.CartridgeAndCloud.Infrastructure.InputSystem.Placement;
using VRMGames.CartridgeAndCloud.Presentation.Placement;

namespace VRMGames.CartridgeAndCloud.Editor.Sprint4
{
    public static class CCS44PlacementFoundationClosureInstaller
    {
        private const string TestLabScenePath =
            "Assets/_Project/Scenes/TestLab.unity";

        private const string BootstrapScenePath =
            "Assets/_Project/Scenes/Bootstrap.unity";

        private const string MainMenuScenePath =
            "Assets/_Project/Scenes/MainMenu.unity";

        private const string StoreScenePath =
            "Assets/_Project/Scenes/Store.unity";

        private const string S3RootName =
            "S3_ClickToMoveLab";

        private const string S4RootName =
            "S4_PlacementPreviewLab";

        private const string PlacedObjectsName =
            "PlacedObjects";

        private const string ExpectedDefinitionId =
            "technical-shelf-4x2";

        private const string TargetVersion =
            "0.0.5";

        [MenuItem(
            "Cartridge & Cloud/Sprint 4/" +
            "CC_S4.4 Integrate Final Placement Foundation")]
        public static void Integrate()
        {
            bool confirmed =
                EditorUtility.DisplayDialog(
                    "Integrate CC_S4.4",
                    "This resets the Sprint 4 TestLab baseline, " +
                    "keeps no confirmed object in the scene and " +
                    "sets the application version to 0.0.5.",
                    "Integrate",
                    "Cancel");

            if (!confirmed)
            {
                return;
            }

            Scene scene =
                EditorSceneManager.OpenScene(
                    TestLabScenePath,
                    OpenSceneMode.Single);

            if (!TryResolveIntegration(
                    out PlacementPreviewController previewController,
                    out PlacementRuntimeController runtimeController,
                    out Transform placedObjectsRoot,
                    out _,
                    out _))
            {
                return;
            }

            runtimeController.CancelPlacement();
            previewController.ClearPreview();
            ClearChildren(placedObjectsRoot);

            PlayerSettings.bundleVersion =
                TargetVersion;

            EditorUtility.SetDirty(
                runtimeController.gameObject);

            EditorUtility.SetDirty(
                previewController.gameObject);

            EditorSceneManager.MarkSceneDirty(
                scene);

            EditorSceneManager.SaveScene(scene);
            AssetDatabase.SaveAssets();

            Selection.activeGameObject =
                runtimeController.gameObject;

            EditorGUIUtility.PingObject(
                runtimeController.gameObject);

            Debug.Log(
                "[CC_S4.4] Final placement foundation integrated. " +
                "TestLab reset and application version set to 0.0.5.");
        }

        [MenuItem(
            "Cartridge & Cloud/Sprint 4/" +
            "CC_S4.4 Validate Final Placement Foundation")]
        public static void Validate()
        {
            Scene scene =
                EditorSceneManager.OpenScene(
                    TestLabScenePath,
                    OpenSceneMode.Single);

            bool resolved =
                TryResolveIntegration(
                    out PlacementPreviewController previewController,
                    out PlacementRuntimeController runtimeController,
                    out Transform placedObjectsRoot,
                    out PlacementInputActionDriver placementDriver,
                    out GameplayInputActionDriver gameplayDriver);

            bool actionsValid = ValidateActions();

            bool valid =
                scene.IsValid() &&
                HasApprovedRootObjects(scene) &&
                resolved &&
                previewController != null &&
                previewController.Surface != null &&
                previewController.Surface.GridWidth == 16 &&
                previewController.Surface.GridDepth == 16 &&
                Mathf.Approximately(
                    previewController.Surface.CellSize,
                    0.5f) &&
                previewController.Definition != null &&
                previewController.Definition.DefinitionId ==
                    ExpectedDefinitionId &&
                runtimeController != null &&
                !runtimeController.IsPlacementModeActive &&
                runtimeController.PlacedCount == 0 &&
                placedObjectsRoot != null &&
                placedObjectsRoot.childCount == 0 &&
                placementDriver != null &&
                placementDriver.RuntimeController ==
                    runtimeController &&
                gameplayDriver != null &&
                gameplayDriver.PlacementRuntimeController ==
                    runtimeController &&
                actionsValid &&
                HasEnabledBuildScene(BootstrapScenePath) &&
                HasEnabledBuildScene(MainMenuScenePath) &&
                HasEnabledBuildScene(StoreScenePath) &&
                PlayerSettings.bundleVersion ==
                    TargetVersion;

            EditorUtility.DisplayDialog(
                "CC_S4.4 Final Integration Validation",
                valid
                    ? "PASS: Sprint 4 placement foundation, " +
                      "build scenes, empty TestLab baseline and " +
                      "version 0.0.5 are correct."
                    : "FAIL: run the CC_S4.4 integration command " +
                      "and review the Console.",
                "OK");

            if (!valid)
            {
                LogValidationDetails(
                    scene,
                    resolved,
                    previewController,
                    runtimeController,
                    placedObjectsRoot,
                    placementDriver,
                    gameplayDriver,
                    actionsValid);
            }
        }

        private static bool TryResolveIntegration(
            out PlacementPreviewController previewController,
            out PlacementRuntimeController runtimeController,
            out Transform placedObjectsRoot,
            out PlacementInputActionDriver placementDriver,
            out GameplayInputActionDriver gameplayDriver)
        {
            previewController = null;
            runtimeController = null;
            placedObjectsRoot = null;
            placementDriver = null;
            gameplayDriver = null;

            GameObject s3Root =
                GameObject.Find(S3RootName);

            if (s3Root == null)
            {
                Debug.LogError(
                    "[CC_S4.4] S3_ClickToMoveLab was not found.");
                return false;
            }

            Transform s4Root =
                s3Root.transform.Find(S4RootName);

            if (s4Root == null)
            {
                Debug.LogError(
                    "[CC_S4.4] S4_PlacementPreviewLab was not found.");
                return false;
            }

            previewController =
                s4Root.GetComponent<
                    PlacementPreviewController>();

            runtimeController =
                s4Root.GetComponent<
                    PlacementRuntimeController>();

            placementDriver =
                s4Root.GetComponent<
                    PlacementInputActionDriver>();

            placedObjectsRoot =
                s4Root.Find(PlacedObjectsName);

            UnityEngine.Camera camera =
                UnityEngine.Camera.main;

            gameplayDriver =
                camera != null
                    ? camera.GetComponent<
                        GameplayInputActionDriver>()
                    : null;

            if (previewController == null ||
                runtimeController == null ||
                placementDriver == null ||
                placedObjectsRoot == null ||
                camera == null ||
                gameplayDriver == null)
            {
                Debug.LogError(
                    "[CC_S4.4] Required Sprint 4 components are missing.");
                return false;
            }

            return true;
        }

        private static bool ValidateActions()
        {
            using ProjectInputActions actions =
                new ProjectInputActions();

            return
                actions.Gameplay.actions.Count == 10 &&
                actions.TogglePlacementMode != null &&
                actions.SetDestination != null &&
                actions.RotatePlacementCounterClockwise != null &&
                actions.RotatePlacementClockwise != null &&
                actions.CancelPlacement != null &&
                actions.RemovePlacement != null &&
                actions.TogglePlacementMode.bindings.Any(
                    binding =>
                        binding.path == "<Keyboard>/b") &&
                actions.RemovePlacement.bindings.Any(
                    binding =>
                        binding.path == "<Keyboard>/delete") &&
                actions.RemovePlacement.bindings.Any(
                    binding =>
                        binding.path == "<Keyboard>/backspace");
        }

        private static bool HasEnabledBuildScene(
            string expectedPath)
        {
            return EditorBuildSettings.scenes.Any(
                scene =>
                    scene.enabled &&
                    scene.path == expectedPath);
        }

        private static void ClearChildren(
            Transform parent)
        {
            if (parent == null)
            {
                return;
            }

            for (int index =
                     parent.childCount - 1;
                 index >= 0;
                 index--)
            {
                Undo.DestroyObjectImmediate(
                    parent.GetChild(index).gameObject);
            }
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

        private static void LogValidationDetails(
            Scene scene,
            bool resolved,
            PlacementPreviewController previewController,
            PlacementRuntimeController runtimeController,
            Transform placedObjectsRoot,
            PlacementInputActionDriver placementDriver,
            GameplayInputActionDriver gameplayDriver,
            bool actionsValid)
        {
            Debug.LogError(
                "[CC_S4.4] Validation details: " +
                $"sceneValid={scene.IsValid()}, " +
                $"approvedRoots={HasApprovedRootObjects(scene)}, " +
                $"resolved={resolved}, " +
                $"preview={previewController != null}, " +
                $"runtime={runtimeController != null}, " +
                $"placedRoot={placedObjectsRoot != null}, " +
                $"placementDriver={placementDriver != null}, " +
                $"gameplayDriver={gameplayDriver != null}, " +
                $"actions={actionsValid}, " +
                $"bootstrapBuildScene={HasEnabledBuildScene(BootstrapScenePath)}, " +
                $"mainMenuBuildScene={HasEnabledBuildScene(MainMenuScenePath)}, " +
                $"storeBuildScene={HasEnabledBuildScene(StoreScenePath)}, " +
                $"version={PlayerSettings.bundleVersion}.");
        }
    }
}
