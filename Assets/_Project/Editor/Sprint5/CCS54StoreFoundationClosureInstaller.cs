using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRMGames.CartridgeAndCloud.Infrastructure.InputSystem.Actions;
using VRMGames.CartridgeAndCloud.Infrastructure.InputSystem.Placement;
using VRMGames.CartridgeAndCloud.Presentation.Placement;
using VRMGames.CartridgeAndCloud.Presentation.SceneFlow;
using VRMGames.CartridgeAndCloud.Presentation.Store;

namespace VRMGames.CartridgeAndCloud.Editor.Sprint5
{
    public static class CCS54StoreFoundationClosureInstaller
    {
        private const string BootstrapScenePath =
            "Assets/_Project/Scenes/Bootstrap.unity";

        private const string MainMenuScenePath =
            "Assets/_Project/Scenes/MainMenu.unity";

        private const string StoreScenePath =
            "Assets/_Project/Scenes/Store.unity";

        private const string ShellRootName =
            "S5_StoreShell";

        private const string FloorName =
            "WalkableFloor";

        private const string PlayerName =
            "TechnicalPlayer";

        private const string PlacementRootName =
            "S5_StorePlacement";

        private const string GhostVisualName =
            "GhostVisual";

        private const string PlacedObjectsName =
            "PlacedObjects";

        private const string AccessMarkersName =
            "AccessMarkers";

        private const string ExpectedDefinitionId =
            "technical-shelf-4x2";

        private const string TargetVersion =
            "0.0.6";

        private static readonly Vector3 ExpectedGridOrigin =
            new Vector3(
                -5f,
                0f,
                -7.5f);

        [MenuItem(
            "Cartridge & Cloud/Sprint 5/" +
            "CC_S5.4 Integrate Final Store Foundation")]
        public static void Integrate()
        {
            bool confirmed =
                EditorUtility.DisplayDialog(
                    "Integrate CC_S5.4",
                    "This resets Store to an empty placement baseline, " +
                    "disables construction mode and sets the application " +
                    "version to 0.0.6.",
                    "Integrate",
                    "Cancel");

            if (!confirmed)
            {
                return;
            }

            Scene scene =
                EditorSceneManager.OpenScene(
                    StoreScenePath,
                    OpenSceneMode.Single);

            if (!TryResolveIntegration(
                    out StoreShellDescriptor shellDescriptor,
                    out PlacementSurface surface,
                    out PlacementPreviewController previewController,
                    out PlacementRuntimeController runtimeController,
                    out Transform placedObjectsRoot,
                    out PlacementInputActionDriver placementDriver,
                    out GameplayInputActionDriver gameplayDriver,
                    out Transform ghostVisual,
                    out Transform accessMarkers,
                    out Transform returnButton))
            {
                return;
            }

            runtimeController.CancelPlacement();
            previewController.ClearPreview();
            ClearChildren(placedObjectsRoot);

            if (ghostVisual != null)
            {
                ghostVisual.gameObject.SetActive(false);
            }

            PlayerSettings.bundleVersion =
                TargetVersion;

            EditorUtility.SetDirty(
                shellDescriptor.gameObject);

            EditorUtility.SetDirty(
                surface.gameObject);

            EditorUtility.SetDirty(
                runtimeController.gameObject);

            EditorUtility.SetDirty(
                previewController.gameObject);

            EditorUtility.SetDirty(
                placementDriver.gameObject);

            EditorUtility.SetDirty(
                gameplayDriver.gameObject);

            if (returnButton != null)
            {
                EditorUtility.SetDirty(
                    returnButton.gameObject);
            }

            EditorSceneManager.MarkSceneDirty(
                scene);

            EditorSceneManager.SaveScene(
                scene);

            AssetDatabase.SaveAssets();

            Selection.activeGameObject =
                runtimeController.gameObject;

            EditorGUIUtility.PingObject(
                runtimeController.gameObject);

            Debug.Log(
                "[CC_S5.4] Final Store foundation integrated. " +
                "Store reset, construction inactive and version set to 0.0.6.");
        }

        [MenuItem(
            "Cartridge & Cloud/Sprint 5/" +
            "CC_S5.4 Validate Final Store Foundation")]
        public static void Validate()
        {
            Scene scene =
                EditorSceneManager.OpenScene(
                    StoreScenePath,
                    OpenSceneMode.Single);

            bool resolved =
                TryResolveIntegration(
                    out StoreShellDescriptor shellDescriptor,
                    out PlacementSurface surface,
                    out PlacementPreviewController previewController,
                    out PlacementRuntimeController runtimeController,
                    out Transform placedObjectsRoot,
                    out PlacementInputActionDriver placementDriver,
                    out GameplayInputActionDriver gameplayDriver,
                    out Transform ghostVisual,
                    out Transform accessMarkers,
                    out Transform returnButton);

            bool actionsValid =
                ValidateActions();

            bool buildScenesValid =
                HasEnabledBuildScene(
                    BootstrapScenePath) &&
                HasEnabledBuildScene(
                    MainMenuScenePath) &&
                HasEnabledBuildScene(
                    StoreScenePath) &&
                GetEnabledBuildSceneIndex(
                    BootstrapScenePath) == 0;

            bool valid =
                scene.IsValid() &&
                HasApprovedRootObjects(scene) &&
                resolved &&
                shellDescriptor != null &&
                shellDescriptor.IsConfigured &&
                shellDescriptor.WalkableFloor != null &&
                shellDescriptor.EntranceThreshold != null &&
                shellDescriptor.PlayerSpawn != null &&
                shellDescriptor.TechnicalPlayer != null &&
                shellDescriptor.TechnicalPlayer.name ==
                    PlayerName &&
                surface != null &&
                surface.SurfaceCollider != null &&
                surface.GridWidth == 20 &&
                surface.GridDepth == 30 &&
                Mathf.Approximately(
                    surface.CellSize,
                    0.5f) &&
                Approximately(
                    surface.GridOrigin,
                    ExpectedGridOrigin) &&
                previewController != null &&
                previewController.Surface ==
                    surface &&
                previewController.Definition != null &&
                previewController.Definition.DefinitionId ==
                    ExpectedDefinitionId &&
                previewController.Definition.WidthCells ==
                    4 &&
                previewController.Definition.DepthCells ==
                    2 &&
                !previewController.HasPreview &&
                runtimeController != null &&
                !runtimeController.IsPlacementModeActive &&
                runtimeController.IsAccessValidationEnabled &&
                runtimeController.PlacedCount == 0 &&
                runtimeController.Registry.OccupiedCellCount == 0 &&
                placedObjectsRoot != null &&
                placedObjectsRoot.childCount == 0 &&
                placementDriver != null &&
                placementDriver.PreviewController ==
                    previewController &&
                placementDriver.RuntimeController ==
                    runtimeController &&
                gameplayDriver != null &&
                gameplayDriver.PlacementRuntimeController ==
                    runtimeController &&
                gameplayDriver.DestinationInput != null &&
                gameplayDriver.CameraRig != null &&
                ghostVisual != null &&
                !ghostVisual.gameObject.activeSelf &&
                accessMarkers != null &&
                accessMarkers.childCount == 7 &&
                returnButton != null &&
                returnButton.gameObject.activeSelf &&
                UnityEngine.Object.FindFirstObjectByType<
                    StoreSceneController>() != null &&
                actionsValid &&
                buildScenesValid &&
                PlayerSettings.bundleVersion ==
                    TargetVersion;

            EditorUtility.DisplayDialog(
                "CC_S5.4 Final Integration Validation",
                valid
                    ? "PASS: Sprint 5 Store shell, placement, access, " +
                      "scene flow, build scenes, empty baseline and " +
                      "version 0.0.6 are correct."
                    : "FAIL: run the CC_S5.4 integration command " +
                      "and review the Console.",
                "OK");

            if (!valid)
            {
                LogValidationDetails(
                    scene,
                    resolved,
                    shellDescriptor,
                    surface,
                    previewController,
                    runtimeController,
                    placedObjectsRoot,
                    placementDriver,
                    gameplayDriver,
                    ghostVisual,
                    accessMarkers,
                    returnButton,
                    actionsValid,
                    buildScenesValid);
            }
        }

        private static bool TryResolveIntegration(
            out StoreShellDescriptor shellDescriptor,
            out PlacementSurface surface,
            out PlacementPreviewController previewController,
            out PlacementRuntimeController runtimeController,
            out Transform placedObjectsRoot,
            out PlacementInputActionDriver placementDriver,
            out GameplayInputActionDriver gameplayDriver,
            out Transform ghostVisual,
            out Transform accessMarkers,
            out Transform returnButton)
        {
            shellDescriptor = null;
            surface = null;
            previewController = null;
            runtimeController = null;
            placedObjectsRoot = null;
            placementDriver = null;
            gameplayDriver = null;
            ghostVisual = null;
            accessMarkers = null;
            returnButton = null;

            GameObject shellRoot =
                GameObject.Find(
                    ShellRootName);

            if (shellRoot == null)
            {
                Debug.LogError(
                    "[CC_S5.4] S5_StoreShell was not found.");

                return false;
            }

            shellDescriptor =
                shellRoot.GetComponent<
                    StoreShellDescriptor>();

            Transform floor =
                shellRoot.transform.Find(
                    FloorName);

            Transform placementRoot =
                shellRoot.transform.Find(
                    PlacementRootName);

            if (floor == null ||
                placementRoot == null)
            {
                Debug.LogError(
                    "[CC_S5.4] Store floor or placement root was not found.");

                return false;
            }

            surface =
                floor.GetComponent<
                    PlacementSurface>();

            previewController =
                placementRoot.GetComponent<
                    PlacementPreviewController>();

            runtimeController =
                placementRoot.GetComponent<
                    PlacementRuntimeController>();

            placementDriver =
                placementRoot.GetComponent<
                    PlacementInputActionDriver>();

            placedObjectsRoot =
                placementRoot.Find(
                    PlacedObjectsName);

            ghostVisual =
                placementRoot.Find(
                    GhostVisualName);

            accessMarkers =
                placementRoot.Find(
                    AccessMarkersName);

            UnityEngine.Camera camera =
                UnityEngine.Camera.main;

            gameplayDriver =
                camera != null
                    ? camera.GetComponent<
                        GameplayInputActionDriver>()
                    : null;

            GameObject canvas =
                GameObject.Find(
                    "Canvas");

            returnButton =
                canvas != null
                    ? canvas.transform.Find(
                        "ReturnButton")
                    : null;

            if (shellDescriptor == null ||
                surface == null ||
                previewController == null ||
                runtimeController == null ||
                placementDriver == null ||
                placedObjectsRoot == null ||
                ghostVisual == null ||
                accessMarkers == null ||
                camera == null ||
                gameplayDriver == null ||
                returnButton == null)
            {
                Debug.LogError(
                    "[CC_S5.4] Required Sprint 5 integration components " +
                    "are missing.");

                return false;
            }

            return true;
        }

        private static bool ValidateActions()
        {
            using (ProjectInputActions actions =
                   new ProjectInputActions())
            {
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
                    actions.RotatePlacementCounterClockwise.bindings.Any(
                        binding =>
                            binding.path == "<Keyboard>/q") &&
                    actions.RotatePlacementClockwise.bindings.Any(
                        binding =>
                            binding.path == "<Keyboard>/e") &&
                    actions.CancelPlacement.bindings.Any(
                        binding =>
                            binding.path == "<Keyboard>/escape") &&
                    actions.RemovePlacement.bindings.Any(
                        binding =>
                            binding.path == "<Keyboard>/delete") &&
                    actions.RemovePlacement.bindings.Any(
                        binding =>
                            binding.path == "<Keyboard>/backspace");
            }
        }

        private static bool HasEnabledBuildScene(
            string expectedPath)
        {
            return EditorBuildSettings.scenes.Any(
                scene =>
                    scene.enabled &&
                    scene.path == expectedPath);
        }

        private static int GetEnabledBuildSceneIndex(
            string expectedPath)
        {
            string[] enabledScenes =
                EditorBuildSettings.scenes
                    .Where(scene => scene.enabled)
                    .Select(scene => scene.path)
                    .ToArray();

            for (int index = 0;
                 index < enabledScenes.Length;
                 index++)
            {
                if (enabledScenes[index] ==
                    expectedPath)
                {
                    return index;
                }
            }

            return -1;
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

        private static bool Approximately(
            Vector3 left,
            Vector3 right)
        {
            return Mathf.Approximately(
                       left.x,
                       right.x) &&
                   Mathf.Approximately(
                       left.y,
                       right.y) &&
                   Mathf.Approximately(
                       left.z,
                       right.z);
        }

        private static void LogValidationDetails(
            Scene scene,
            bool resolved,
            StoreShellDescriptor shellDescriptor,
            PlacementSurface surface,
            PlacementPreviewController previewController,
            PlacementRuntimeController runtimeController,
            Transform placedObjectsRoot,
            PlacementInputActionDriver placementDriver,
            GameplayInputActionDriver gameplayDriver,
            Transform ghostVisual,
            Transform accessMarkers,
            Transform returnButton,
            bool actionsValid,
            bool buildScenesValid)
        {
            Debug.LogError(
                "[CC_S5.4] Validation details: " +
                $"sceneValid={scene.IsValid()}, " +
                $"approvedRoots={HasApprovedRootObjects(scene)}, " +
                $"resolved={resolved}, " +
                $"shellDescriptor={shellDescriptor != null}, " +
                $"surface={surface != null}, " +
                $"preview={previewController != null}, " +
                $"runtime={runtimeController != null}, " +
                $"placedRoot={placedObjectsRoot != null}, " +
                $"placementDriver={placementDriver != null}, " +
                $"gameplayDriver={gameplayDriver != null}, " +
                $"ghost={ghostVisual != null}, " +
                $"accessMarkers={accessMarkers != null}, " +
                $"returnButton={returnButton != null}, " +
                $"actions={actionsValid}, " +
                $"buildScenes={buildScenesValid}, " +
                $"version={PlayerSettings.bundleVersion}.");
        }
    }
}
