using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRMGames.CartridgeAndCloud.Infrastructure.InputSystem.Actions;
using VRMGames.CartridgeAndCloud.Infrastructure.InputSystem.Camera;
using VRMGames.CartridgeAndCloud.Infrastructure.InputSystem.PlayerMovement;
using VRMGames.CartridgeAndCloud.Presentation.Camera;
using VRMGames.CartridgeAndCloud.Presentation.PlayerMovement;

namespace VRMGames.CartridgeAndCloud.Editor.Sprint3
{
    public static class CCS34InputIntegrationClosureInstaller
    {
        private const string TestLabScenePath =
            "Assets/_Project/Scenes/TestLab.unity";

        private const string LabRootName =
            "S3_ClickToMoveLab";

        private const string PlayerName =
            "TechnicalPlayer";

        private const string TargetVersion =
            "0.0.4";

        [MenuItem(
            "Cartridge & Cloud/Sprint 3/CC_S3.4 Integrate Final Input Actions")]
        public static void Integrate()
        {
            bool confirmed =
                EditorUtility.DisplayDialog(
                    "Integrate CC_S3.4",
                    "This replaces the temporary mouse drivers in " +
                    "TestLab with final Input Actions and sets the " +
                    "application version to 0.0.4.",
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

            GameObject labRoot =
                GameObject.Find(LabRootName);

            if (labRoot == null)
            {
                Debug.LogError(
                    "[CC_S3.4] S3_ClickToMoveLab was not found.");
                return;
            }

            Transform playerTransform =
                labRoot.transform.Find(PlayerName);

            if (playerTransform == null)
            {
                Debug.LogError(
                    "[CC_S3.4] TechnicalPlayer was not found.");
                return;
            }

            UnityEngine.Camera camera =
                UnityEngine.Camera.main;

            if (camera == null)
            {
                Debug.LogError(
                    "[CC_S3.4] Main Camera was not found.");
                return;
            }

            ClickDestinationInput destinationInput =
                playerTransform.GetComponent<
                    ClickDestinationInput>();

            OrbitCameraRig cameraRig =
                camera.GetComponent<OrbitCameraRig>();

            if (destinationInput == null ||
                cameraRig == null)
            {
                Debug.LogError(
                    "[CC_S3.4] CC_S3.2 and CC_S3.3 " +
                    "components are required.");
                return;
            }

            RemoveComponent<
                MouseClickDestinationDriver>(
                    playerTransform.gameObject);

            RemoveComponent<
                MouseOrbitZoomDriver>(
                    camera.gameObject);

            GameplayInputActionDriver driver =
                camera.GetComponent<
                    GameplayInputActionDriver>();

            if (driver == null)
            {
                driver =
                    Undo.AddComponent<
                        GameplayInputActionDriver>(
                            camera.gameObject);
            }

            driver.Configure(
                contextRouter: null,
                destinationInput,
                cameraRig,
                orbitSensitivity: 0.2f,
                zoomSensitivity: 0.5f);

            PlayerSettings.bundleVersion =
                TargetVersion;

            EditorUtility.SetDirty(
                camera.gameObject);

            EditorUtility.SetDirty(
                playerTransform.gameObject);

            EditorSceneManager.MarkSceneDirty(
                scene);

            EditorSceneManager.SaveScene(scene);
            AssetDatabase.SaveAssets();

            Selection.activeGameObject =
                camera.gameObject;

            EditorGUIUtility.PingObject(
                camera.gameObject);

            Debug.Log(
                "[CC_S3.4] Final Input Actions integrated. " +
                "Application version set to 0.0.4.");
        }

        [MenuItem(
            "Cartridge & Cloud/Sprint 3/CC_S3.4 Validate Final Integration")]
        public static void Validate()
        {
            Scene scene =
                EditorSceneManager.OpenScene(
                    TestLabScenePath,
                    OpenSceneMode.Single);

            GameObject labRoot =
                GameObject.Find(LabRootName);

            Transform playerTransform =
                labRoot != null
                    ? labRoot.transform.Find(PlayerName)
                    : null;

            UnityEngine.Camera camera =
                UnityEngine.Camera.main;

            GameplayInputActionDriver driver =
                camera != null
                    ? camera.GetComponent<
                        GameplayInputActionDriver>()
                    : null;

            bool valid =
                scene.IsValid() &&
                HasApprovedRootObjects(scene) &&
                playerTransform != null &&
                camera != null &&
                driver != null &&
                driver.DestinationInput != null &&
                driver.CameraRig != null &&
                playerTransform.GetComponent<
                    MouseClickDestinationDriver>() == null &&
                camera.GetComponent<
                    MouseOrbitZoomDriver>() == null &&
                PlayerSettings.bundleVersion ==
                    TargetVersion;

            EditorUtility.DisplayDialog(
                "CC_S3.4 Final Integration Validation",
                valid
                    ? "PASS: final Input Actions, scene " +
                      "baselines and version 0.0.4 are correct."
                    : "FAIL: run the CC_S3.4 integration command.",
                "OK");
        }

        private static void RemoveComponent<T>(
            GameObject target)
            where T : Component
        {
            T component =
                target.GetComponent<T>();

            if (component != null)
            {
                Undo.DestroyObjectImmediate(
                    component);
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

            foreach (GameObject root in roots)
            {
                switch (root.name)
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
