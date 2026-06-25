using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRMGames.CartridgeAndCloud.Application.Camera;
using VRMGames.CartridgeAndCloud.Infrastructure.InputSystem.Camera;
using VRMGames.CartridgeAndCloud.Presentation.Camera;

namespace VRMGames.CartridgeAndCloud.Editor.Sprint3
{
    public static class CCS33OrbitZoomCameraInstaller
    {
        private const string TestLabScenePath =
            "Assets/_Project/Scenes/TestLab.unity";

        private const string LabRootName =
            "S3_ClickToMoveLab";

        private const string PlayerName =
            "TechnicalPlayer";

        private const string CameraTargetName =
            "CameraTarget";

        [MenuItem(
            "Cartridge & Cloud/Sprint 3/CC_S3.3 Build Orbit & Zoom Camera TestLab")]
        public static void BuildTestLab()
        {
            bool confirmed = EditorUtility.DisplayDialog(
                "Build CC_S3.3 Camera TestLab",
                "This adds the orbit camera rig to Main Camera and " +
                "a CameraTarget below TechnicalPlayer. " +
                "Approved TestLab roots remain unchanged.",
                "Build",
                "Cancel");

            if (!confirmed)
            {
                return;
            }

            Scene scene = EditorSceneManager.OpenScene(
                TestLabScenePath,
                OpenSceneMode.Single);

            GameObject labRoot =
                GameObject.Find(LabRootName);

            if (labRoot == null)
            {
                Debug.LogError(
                    "[CC_S3.3] S3_ClickToMoveLab was not found. " +
                    "Integrate and build CC_S3.2 first.");
                return;
            }

            Transform player =
                labRoot.transform.Find(PlayerName);

            if (player == null)
            {
                Debug.LogError(
                    "[CC_S3.3] TechnicalPlayer was not found.");
                return;
            }

            UnityEngine.Camera camera =
                UnityEngine.Camera.main;

            if (camera == null)
            {
                Debug.LogError(
                    "[CC_S3.3] TestLab Main Camera was not found.");
                return;
            }

            Transform cameraTarget =
                GetOrCreateCameraTarget(player);

            OrbitCameraRig rig =
                camera.GetComponent<OrbitCameraRig>();

            if (rig == null)
            {
                rig = Undo.AddComponent<OrbitCameraRig>(
                    camera.gameObject);
            }

            OrbitCameraConstraints constraints =
                new OrbitCameraConstraints(
                    25f,
                    75f,
                    6f,
                    24f);

            rig.Configure(
                cameraTarget,
                0f,
                45f,
                18f,
                constraints);

            MouseOrbitZoomDriver driver =
                camera.GetComponent<MouseOrbitZoomDriver>();

            if (driver == null)
            {
                driver =
                    Undo.AddComponent<MouseOrbitZoomDriver>(
                        camera.gameObject);
            }

            driver.Configure(
                0.2f,
                0.02f,
                true);

            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);

            Selection.activeGameObject =
                camera.gameObject;

            EditorGUIUtility.PingObject(
                camera.gameObject);

            Debug.Log(
                "[CC_S3.3] Orbit camera installed. " +
                "Right-drag to orbit and use the mouse wheel to zoom.");
        }

        [MenuItem(
            "Cartridge & Cloud/Sprint 3/CC_S3.3 Validate Orbit & Zoom Camera TestLab")]
        public static void ValidateTestLab()
        {
            Scene scene = EditorSceneManager.OpenScene(
                TestLabScenePath,
                OpenSceneMode.Single);

            GameObject labRoot =
                GameObject.Find(LabRootName);

            Transform player =
                labRoot != null
                    ? labRoot.transform.Find(PlayerName)
                    : null;

            Transform cameraTarget =
                player != null
                    ? player.Find(CameraTargetName)
                    : null;

            UnityEngine.Camera camera =
                UnityEngine.Camera.main;

            OrbitCameraRig rig =
                camera != null
                    ? camera.GetComponent<OrbitCameraRig>()
                    : null;

            MouseOrbitZoomDriver driver =
                camera != null
                    ? camera.GetComponent<MouseOrbitZoomDriver>()
                    : null;

            bool valid =
                scene.IsValid() &&
                HasApprovedRootObjects(scene) &&
                cameraTarget != null &&
                rig != null &&
                rig.Target == cameraTarget &&
                driver != null;

            EditorUtility.DisplayDialog(
                "CC_S3.3 Camera Validation",
                valid
                    ? "PASS: orbit camera, target and input driver " +
                      "are present without changing approved roots."
                    : "FAIL: run the CC_S3.3 Build command.",
                "OK");
        }

        private static Transform GetOrCreateCameraTarget(
            Transform player)
        {
            Transform existing =
                player.Find(CameraTargetName);

            if (existing != null)
            {
                existing.localPosition =
                    new Vector3(0f, 1f, 0f);

                existing.localRotation =
                    Quaternion.identity;

                existing.localScale =
                    Vector3.one;

                return existing;
            }

            GameObject targetObject =
                new GameObject(CameraTargetName);

            Undo.RegisterCreatedObjectUndo(
                targetObject,
                "Create camera target");

            targetObject.transform.SetParent(
                player,
                worldPositionStays: false);

            targetObject.transform.localPosition =
                new Vector3(0f, 1f, 0f);

            return targetObject.transform;
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
                if (root.name == "Main Camera")
                {
                    hasMainCamera = true;
                }
                else if (root.name == "Directional Light")
                {
                    hasDirectionalLight = true;
                }
                else
                {
                    return false;
                }
            }

            return hasMainCamera &&
                   hasDirectionalLight;
        }
    }
}
