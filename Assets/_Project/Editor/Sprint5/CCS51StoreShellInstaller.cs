using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRMGames.CartridgeAndCloud.Application.Camera;
using VRMGames.CartridgeAndCloud.Application.Store;
using VRMGames.CartridgeAndCloud.Infrastructure.InputSystem.Actions;
using VRMGames.CartridgeAndCloud.Presentation.Camera;
using VRMGames.CartridgeAndCloud.Presentation.PlayerMovement;
using VRMGames.CartridgeAndCloud.Presentation.SceneFlow;
using VRMGames.CartridgeAndCloud.Presentation.Store;

namespace VRMGames.CartridgeAndCloud.Editor.Sprint5
{
    public static class CCS51StoreShellInstaller
    {
        private const string StoreScenePath =
            "Assets/_Project/Scenes/Store.unity";

        private const string ShellRootName =
            "S5_StoreShell";

        private const string FloorName =
            "WalkableFloor";

        private const string EntranceApronName =
            "EntranceApron";

        private const string EntranceThresholdName =
            "EntranceThreshold";

        private const string PlayerSpawnName =
            "PlayerSpawn";

        private const string PlayerName =
            "TechnicalPlayer";

        private const string CameraTargetName =
            "CameraTarget";

        private const string MaterialFolder =
            "Assets/_Project/Art/Materials/Technical";

        private const string FloorMaterialPath =
            MaterialFolder +
            "/CC_S5_StoreFloorTechnical.mat";

        private const string WallMaterialPath =
            MaterialFolder +
            "/CC_S5_StoreWallTechnical.mat";

        private const string EntranceMaterialPath =
            MaterialFolder +
            "/CC_S5_StoreEntranceTechnical.mat";

        private const string PlayerMaterialPath =
            MaterialFolder +
            "/CC_S5_StorePlayerTechnical.mat";

        private const string ExpectedVersion =
            "0.0.6";

        [MenuItem(
            "Cartridge & Cloud/Sprint 5/" +
            "CC_S5.1 Build Store Shell")]
        public static void BuildStoreShell()
        {
            bool confirmed =
                EditorUtility.DisplayDialog(
                    "Build CC_S5.1 Store Shell",
                    "This replaces only S5_StoreShell, " +
                    "disables the old full-screen prototype overlay " +
                    "and preserves the Return button and scene flow.",
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

            UnityEngine.Camera camera =
                UnityEngine.Camera.main;

            GameObject directionalLight =
                GameObject.Find("Directional Light");

            GameObject canvas =
                GameObject.Find("Canvas");

            GameObject eventSystem =
                GameObject.Find("EventSystem");

            StoreSceneController sceneController =
                UnityEngine.Object
                    .FindFirstObjectByType<
                        StoreSceneController>();

            if (camera == null ||
                directionalLight == null ||
                canvas == null ||
                eventSystem == null ||
                sceneController == null)
            {
                Debug.LogError(
                    "[CC_S5.1] Required Store prototype roots " +
                    "were not found.");
                return;
            }

            RemoveExistingShell();
            DisablePrototypeOverlay(
                canvas.transform);

            ConfigureReturnButton(
                canvas.transform);

            StoreShellSpecification specification =
                StoreShellSpecification.InitialTier();

            Material floorMaterial =
                CreateOrUpdateMaterial(
                    FloorMaterialPath,
                    new Color(
                        0.58f,
                        0.42f,
                        0.25f,
                        1f));

            Material wallMaterial =
                CreateOrUpdateMaterial(
                    WallMaterialPath,
                    new Color(
                        0.82f,
                        0.76f,
                        0.63f,
                        1f));

            Material entranceMaterial =
                CreateOrUpdateMaterial(
                    EntranceMaterialPath,
                    new Color(
                        0.12f,
                        0.62f,
                        0.58f,
                        1f));

            Material playerMaterial =
                CreateOrUpdateMaterial(
                    PlayerMaterialPath,
                    new Color(
                        0.9f,
                        0.48f,
                        0.12f,
                        1f));

            if (floorMaterial == null ||
                wallMaterial == null ||
                entranceMaterial == null ||
                playerMaterial == null)
            {
                return;
            }

            GameObject shellRoot =
                new GameObject(ShellRootName);

            Undo.RegisterCreatedObjectUndo(
                shellRoot,
                "Create CC_S5.1 store shell");

            GameObject floor =
                CreateBox(
                    FloorName,
                    shellRoot.transform,
                    new Vector3(
                        0f,
                        -0.1f,
                        0f),
                    new Vector3(
                        specification.WidthMeters,
                        0.2f,
                        specification.DepthMeters),
                    floorMaterial,
                    layer: 0,
                    keepCollider: true);

            GameObject entranceApron =
                CreateBox(
                    EntranceApronName,
                    shellRoot.transform,
                    new Vector3(
                        0f,
                        -0.075f,
                        -8.5f),
                    new Vector3(
                        2.5f,
                        0.15f,
                        2f),
                    floorMaterial,
                    layer: 0,
                    keepCollider: true);

            CreateWalls(
                shellRoot.transform,
                specification,
                wallMaterial);

            GameObject entranceThreshold =
                CreateBox(
                    EntranceThresholdName,
                    shellRoot.transform,
                    new Vector3(
                        0f,
                        0.015f,
                        -7f),
                    new Vector3(
                        specification.EntranceWidthMeters,
                        0.03f,
                        1f),
                    entranceMaterial,
                    layer: 2,
                    keepCollider: false);

            GameObject playerSpawn =
                new GameObject(PlayerSpawnName);

            playerSpawn.transform.SetParent(
                shellRoot.transform,
                worldPositionStays: false);

            playerSpawn.transform.localPosition =
                new Vector3(
                    0f,
                    0f,
                    -5.5f);

            Undo.RegisterCreatedObjectUndo(
                playerSpawn,
                "Create Store player spawn");

            GameObject player =
                CreateTechnicalPlayer(
                    shellRoot.transform,
                    playerSpawn.transform.position,
                    playerMaterial);

            Transform cameraTarget =
                CreateCameraTarget(
                    player.transform);

            ConfigureCameraAndInput(
                camera,
                cameraTarget,
                player.GetComponent<
                    ClickDestinationInput>(),
                floor.layer);

            ConfigureDirectionalLight(
                directionalLight);

            StoreShellDescriptor descriptor =
                shellRoot.AddComponent<
                    StoreShellDescriptor>();

            descriptor.Configure(
                specification,
                floor.transform,
                entranceThreshold.transform,
                playerSpawn.transform,
                player.transform);

            EditorUtility.SetDirty(
                descriptor);

            EditorUtility.SetDirty(
                camera.gameObject);

            EditorUtility.SetDirty(
                directionalLight);

            EditorUtility.SetDirty(
                canvas);

            EditorSceneManager.MarkSceneDirty(
                scene);

            AssetDatabase.SaveAssets();
            EditorSceneManager.SaveScene(scene);

            Selection.activeGameObject =
                shellRoot;

            EditorGUIUtility.PingObject(
                shellRoot);

            Debug.Log(
                "[CC_S5.1] Store shell installed. " +
                "The 10x15 metre blockout, player and orbit camera " +
                "are ready for Play Mode.");
        }

        [MenuItem(
            "Cartridge & Cloud/Sprint 5/" +
            "CC_S5.1 Validate Store Shell")]
        public static void ValidateStoreShell()
        {
            Scene scene =
                EditorSceneManager.OpenScene(
                    StoreScenePath,
                    OpenSceneMode.Single);

            GameObject shellRoot =
                GameObject.Find(ShellRootName);

            StoreShellDescriptor descriptor =
                shellRoot != null
                    ? shellRoot.GetComponent<
                        StoreShellDescriptor>()
                    : null;

            Transform floor =
                shellRoot != null
                    ? shellRoot.transform.Find(
                        FloorName)
                    : null;

            Transform entranceApron =
                shellRoot != null
                    ? shellRoot.transform.Find(
                        EntranceApronName)
                    : null;

            Transform entranceThreshold =
                shellRoot != null
                    ? shellRoot.transform.Find(
                        EntranceThresholdName)
                    : null;

            Transform playerSpawn =
                shellRoot != null
                    ? shellRoot.transform.Find(
                        PlayerSpawnName)
                    : null;

            Transform player =
                shellRoot != null
                    ? shellRoot.transform.Find(
                        PlayerName)
                    : null;

            Transform cameraTarget =
                player != null
                    ? player.Find(
                        CameraTargetName)
                    : null;

            UnityEngine.Camera camera =
                UnityEngine.Camera.main;

            OrbitCameraRig cameraRig =
                camera != null
                    ? camera.GetComponent<
                        OrbitCameraRig>()
                    : null;

            GameplayInputActionDriver inputDriver =
                camera != null
                    ? camera.GetComponent<
                        GameplayInputActionDriver>()
                    : null;

            ClickDestinationInput destinationInput =
                player != null
                    ? player.GetComponent<
                        ClickDestinationInput>()
                    : null;

            ClickToMoveAgent movementAgent =
                player != null
                    ? player.GetComponent<
                        ClickToMoveAgent>()
                    : null;

            CharacterController characterController =
                player != null
                    ? player.GetComponent<
                        CharacterController>()
                    : null;

            GameObject canvas =
                GameObject.Find("Canvas");

            Transform background =
                canvas != null
                    ? canvas.transform.Find(
                        "Background")
                    : null;

            Transform title =
                canvas != null
                    ? canvas.transform.Find(
                        "Title")
                    : null;

            Transform scopeNotice =
                canvas != null
                    ? canvas.transform.Find(
                        "ScopeNotice")
                    : null;

            Transform returnButton =
                canvas != null
                    ? canvas.transform.Find(
                        "ReturnButton")
                    : null;

            StoreShellSpecification specification =
                descriptor != null
                    ? descriptor.Specification
                    : default;

            bool valid =
                scene.IsValid() &&
                HasExpectedRootObjects(scene) &&
                shellRoot != null &&
                shellRoot.transform.parent == null &&
                descriptor != null &&
                descriptor.IsConfigured &&
                specification.WidthCells == 20 &&
                specification.DepthCells == 30 &&
                specification.EntranceWidthCells == 4 &&
                Mathf.Approximately(
                    specification.CellSize,
                    0.5f) &&
                Mathf.Approximately(
                    specification.WidthMeters,
                    10f) &&
                Mathf.Approximately(
                    specification.DepthMeters,
                    15f) &&
                floor != null &&
                floor.GetComponent<
                    BoxCollider>() != null &&
                Approximately(
                    floor.localScale,
                    new Vector3(
                        10f,
                        0.2f,
                        15f)) &&
                entranceApron != null &&
                entranceApron.GetComponent<
                    BoxCollider>() != null &&
                entranceThreshold != null &&
                playerSpawn != null &&
                player != null &&
                cameraTarget != null &&
                movementAgent != null &&
                destinationInput != null &&
                characterController != null &&
                camera != null &&
                cameraRig != null &&
                cameraRig.Target ==
                    cameraTarget &&
                inputDriver != null &&
                inputDriver.DestinationInput ==
                    destinationInput &&
                inputDriver.CameraRig ==
                    cameraRig &&
                inputDriver.PlacementRuntimeController ==
                    null &&
                HasRequiredWalls(
                    shellRoot.transform) &&
                background != null &&
                !background.gameObject.activeSelf &&
                title != null &&
                !title.gameObject.activeSelf &&
                scopeNotice != null &&
                !scopeNotice.gameObject.activeSelf &&
                returnButton != null &&
                returnButton.gameObject.activeSelf &&
                UnityEngine.Object
                    .FindFirstObjectByType<
                        StoreSceneController>() != null &&
                PlayerSettings.bundleVersion ==
                    ExpectedVersion;

            EditorUtility.DisplayDialog(
                "CC_S5.1 Store Shell Validation",
                valid
                    ? "PASS: 10x15 metre Store shell, " +
                      "2 metre entrance, technical player, " +
                      "orbit camera and preserved scene flow are correct."
                    : "FAIL: run the CC_S5.1 Build Store Shell command " +
                      "and review the Console.",
                "OK");

            if (!valid)
            {
                Debug.LogError(
                    "[CC_S5.1] Validation failed. " +
                    "Check shell root, walls, UI overlay, " +
                    "player/camera wiring and application version.");
            }
        }

        private static void CreateWalls(
            Transform parent,
            StoreShellSpecification specification,
            Material wallMaterial)
        {
            float halfWidth =
                specification.WidthMeters * 0.5f;

            float halfDepth =
                specification.DepthMeters * 0.5f;

            float halfHeight =
                specification.WallHeight * 0.5f;

            float halfThickness =
                specification.WallThickness * 0.5f;

            float frontSegmentWidth =
                specification.FrontWallSegmentWidthMeters;

            float frontSegmentOffset =
                specification.EntranceWidthMeters * 0.5f +
                frontSegmentWidth * 0.5f;

            CreateBox(
                "BackWall",
                parent,
                new Vector3(
                    0f,
                    halfHeight,
                    halfDepth - halfThickness),
                new Vector3(
                    specification.WidthMeters,
                    specification.WallHeight,
                    specification.WallThickness),
                wallMaterial,
                layer: 2,
                keepCollider: true);

            CreateBox(
                "LeftWall",
                parent,
                new Vector3(
                    -halfWidth + halfThickness,
                    halfHeight,
                    0f),
                new Vector3(
                    specification.WallThickness,
                    specification.WallHeight,
                    specification.DepthMeters),
                wallMaterial,
                layer: 2,
                keepCollider: true);

            CreateBox(
                "RightWall",
                parent,
                new Vector3(
                    halfWidth - halfThickness,
                    halfHeight,
                    0f),
                new Vector3(
                    specification.WallThickness,
                    specification.WallHeight,
                    specification.DepthMeters),
                wallMaterial,
                layer: 2,
                keepCollider: true);

            CreateBox(
                "FrontWall_Left",
                parent,
                new Vector3(
                    -frontSegmentOffset,
                    halfHeight,
                    -halfDepth + halfThickness),
                new Vector3(
                    frontSegmentWidth,
                    specification.WallHeight,
                    specification.WallThickness),
                wallMaterial,
                layer: 2,
                keepCollider: true);

            CreateBox(
                "FrontWall_Right",
                parent,
                new Vector3(
                    frontSegmentOffset,
                    halfHeight,
                    -halfDepth + halfThickness),
                new Vector3(
                    frontSegmentWidth,
                    specification.WallHeight,
                    specification.WallThickness),
                wallMaterial,
                layer: 2,
                keepCollider: true);
        }

        private static GameObject CreateTechnicalPlayer(
            Transform parent,
            Vector3 spawnPosition,
            Material playerMaterial)
        {
            GameObject player =
                GameObject.CreatePrimitive(
                    PrimitiveType.Capsule);

            player.name =
                PlayerName;

            player.layer = 2;

            player.transform.SetParent(
                parent,
                worldPositionStays: true);

            player.transform.position =
                spawnPosition +
                Vector3.up;

            player.transform.rotation =
                Quaternion.identity;

            CapsuleCollider capsuleCollider =
                player.GetComponent<
                    CapsuleCollider>();

            if (capsuleCollider != null)
            {
                UnityEngine.Object.DestroyImmediate(
                    capsuleCollider);
            }

            CharacterController characterController =
                player.AddComponent<
                    CharacterController>();

            characterController.center =
                Vector3.zero;

            characterController.height =
                2f;

            characterController.radius =
                0.5f;

            characterController.stepOffset =
                0.3f;

            ClickToMoveAgent agent =
                player.AddComponent<
                    ClickToMoveAgent>();

            agent.Configure(
                moveSpeed: 4f,
                rotationSpeedDegrees: 720f,
                stoppingDistance: 0.1f,
                gravity: -20f);

            player.AddComponent<
                ClickDestinationInput>();

            Renderer renderer =
                player.GetComponent<
                    Renderer>();

            if (renderer != null)
            {
                renderer.sharedMaterial =
                    playerMaterial;
            }

            Undo.RegisterCreatedObjectUndo(
                player,
                "Create Store technical player");

            return player;
        }

        private static Transform CreateCameraTarget(
            Transform player)
        {
            GameObject target =
                new GameObject(
                    CameraTargetName);

            target.transform.SetParent(
                player,
                worldPositionStays: false);

            target.transform.localPosition =
                new Vector3(
                    0f,
                    1f,
                    0f);

            target.transform.localRotation =
                Quaternion.identity;

            target.transform.localScale =
                Vector3.one;

            Undo.RegisterCreatedObjectUndo(
                target,
                "Create Store camera target");

            return target.transform;
        }

        private static void ConfigureCameraAndInput(
            UnityEngine.Camera camera,
            Transform cameraTarget,
            ClickDestinationInput destinationInput,
            int walkableLayer)
        {
            OrbitCameraRig cameraRig =
                camera.GetComponent<
                    OrbitCameraRig>();

            if (cameraRig == null)
            {
                cameraRig =
                    Undo.AddComponent<
                        OrbitCameraRig>(
                            camera.gameObject);
            }

            cameraRig.Configure(
                cameraTarget,
                yawDegrees: 0f,
                pitchDegrees: 48f,
                distance: 12f,
                constraints: new OrbitCameraConstraints(
                    minimumPitchDegrees: 25f,
                    maximumPitchDegrees: 75f,
                    minimumDistance: 5f,
                    maximumDistance: 18f));

            destinationInput.Configure(
                camera,
                1 << walkableLayer,
                maxRayDistance: 500f,
                allowStandaloneGameplay: true);

            GameplayInputActionDriver inputDriver =
                camera.GetComponent<
                    GameplayInputActionDriver>();

            if (inputDriver == null)
            {
                inputDriver =
                    Undo.AddComponent<
                        GameplayInputActionDriver>(
                            camera.gameObject);
            }

            inputDriver.Configure(
                contextRouter: null,
                destinationInput,
                cameraRig,
                orbitSensitivity: 0.2f,
                zoomSensitivity: 0.5f);

            inputDriver.SetPlacementRuntimeController(
                null);
        }

        private static GameObject CreateBox(
            string name,
            Transform parent,
            Vector3 localPosition,
            Vector3 localScale,
            Material material,
            int layer,
            bool keepCollider)
        {
            GameObject box =
                GameObject.CreatePrimitive(
                    PrimitiveType.Cube);

            box.name =
                name;

            box.layer =
                layer;

            box.transform.SetParent(
                parent,
                worldPositionStays: false);

            box.transform.localPosition =
                localPosition;

            box.transform.localRotation =
                Quaternion.identity;

            box.transform.localScale =
                localScale;

            Renderer renderer =
                box.GetComponent<
                    Renderer>();

            if (renderer != null)
            {
                renderer.sharedMaterial =
                    material;
            }

            if (!keepCollider)
            {
                Collider collider =
                    box.GetComponent<
                        Collider>();

                if (collider != null)
                {
                    UnityEngine.Object.DestroyImmediate(
                        collider);
                }
            }

            Undo.RegisterCreatedObjectUndo(
                box,
                "Create Store shell object");

            return box;
        }

        private static void DisablePrototypeOverlay(
            Transform canvas)
        {
            SetDirectChildActive(
                canvas,
                "Background",
                false);

            SetDirectChildActive(
                canvas,
                "Title",
                false);

            SetDirectChildActive(
                canvas,
                "ScopeNotice",
                false);
        }

        private static void ConfigureReturnButton(
            Transform canvas)
        {
            Transform returnButton =
                canvas.Find(
                    "ReturnButton");

            if (returnButton == null)
            {
                Debug.LogError(
                    "[CC_S5.1] ReturnButton was not found.");
                return;
            }

            returnButton.gameObject.SetActive(
                true);

            RectTransform rectTransform =
                returnButton as RectTransform;

            if (rectTransform == null)
            {
                return;
            }

            rectTransform.anchorMin =
                new Vector2(
                    1f,
                    1f);

            rectTransform.anchorMax =
                new Vector2(
                    1f,
                    1f);

            rectTransform.pivot =
                new Vector2(
                    1f,
                    1f);

            rectTransform.anchoredPosition =
                new Vector2(
                    -24f,
                    -24f);

            rectTransform.sizeDelta =
                new Vector2(
                    320f,
                    48f);
        }

        private static void SetDirectChildActive(
            Transform parent,
            string childName,
            bool active)
        {
            Transform child =
                parent.Find(
                    childName);

            if (child != null)
            {
                child.gameObject.SetActive(
                    active);
            }
        }

        private static void ConfigureDirectionalLight(
            GameObject directionalLight)
        {
            Light light =
                directionalLight.GetComponent<
                    Light>();

            if (light != null)
            {
                light.intensity =
                    1.15f;

                light.shadows =
                    LightShadows.Soft;

                light.color =
                    new Color(
                        1f,
                        0.93f,
                        0.82f,
                        1f);
            }

            directionalLight.transform.rotation =
                Quaternion.Euler(
                    50f,
                    -30f,
                    0f);
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
                    "[CC_S5.1] No compatible shader was found.");
                return null;
            }

            Material material =
                AssetDatabase.LoadAssetAtPath<
                    Material>(path);

            if (material == null)
            {
                material =
                    new Material(shader);

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
            string folder)
        {
            if (AssetDatabase.IsValidFolder(
                    folder))
            {
                return;
            }

            Directory.CreateDirectory(
                folder);

            AssetDatabase.Refresh();
        }

        private static void RemoveExistingShell()
        {
            GameObject existing =
                GameObject.Find(
                    ShellRootName);

            if (existing != null)
            {
                Undo.DestroyObjectImmediate(
                    existing);
            }
        }

        private static bool HasRequiredWalls(
            Transform shellRoot)
        {
            return
                shellRoot.Find(
                    "BackWall") != null &&
                shellRoot.Find(
                    "LeftWall") != null &&
                shellRoot.Find(
                    "RightWall") != null &&
                shellRoot.Find(
                    "FrontWall_Left") != null &&
                shellRoot.Find(
                    "FrontWall_Right") != null;
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

                foreach (GameObject root in
                         roots)
                {
                    if (root.name ==
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
            return
                Mathf.Approximately(
                    left.x,
                    right.x) &&
                Mathf.Approximately(
                    left.y,
                    right.y) &&
                Mathf.Approximately(
                    left.z,
                    right.z);
        }
    }
}
