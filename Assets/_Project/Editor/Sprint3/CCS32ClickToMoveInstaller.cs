using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRMGames.CartridgeAndCloud.Infrastructure.InputSystem.PlayerMovement;
using VRMGames.CartridgeAndCloud.Presentation.PlayerMovement;

namespace VRMGames.CartridgeAndCloud.Editor.Sprint3
{
    public static class CCS32ClickToMoveInstaller
    {
        private const string TestLabScenePath = "Assets/_Project/Scenes/TestLab.unity";
        private const string RootName = "S3_ClickToMoveLab";
        private const string HostRootName = "Directional Light";

        [MenuItem("Cartridge & Cloud/Sprint 3/CC_S3.2 Build Click-to-Move TestLab")]
        public static void BuildTestLab()
        {
            if (!EditorUtility.DisplayDialog("Build CC_S3.2 TestLab", "This will replace only the S3_ClickToMoveLab content inside TestLab and reposition its camera.", "Build", "Cancel")) return;

            Scene scene = EditorSceneManager.OpenScene(TestLabScenePath, OpenSceneMode.Single);
            RemoveExistingLabRoot();
            GameObject hostRoot = GameObject.Find(HostRootName);
            if (hostRoot == null)
            {
                Debug.LogError($"[CC_S3.2] Required TestLab root '{HostRootName}' was not found.");
                return;
            }

            GameObject labRoot = new GameObject(RootName);
            labRoot.transform.SetParent(hostRoot.transform, worldPositionStays: true);
            Undo.RegisterCreatedObjectUndo(labRoot, "Create CC_S3.2 lab");

            GameObject floor = CreateFloor(labRoot.transform);
            GameObject player = CreatePlayer(labRoot.transform);
            Camera camera = ConfigureCamera();
            if (camera == null) return;

            player.GetComponent<ClickDestinationInput>().Configure(camera, 1 << floor.layer, 500f, true);
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            Selection.activeGameObject = player;
            EditorGUIUtility.PingObject(player);
            Debug.Log("[CC_S3.2] TestLab created below Directional Light. Left-click the floor to move the capsule.");
        }

        [MenuItem("Cartridge & Cloud/Sprint 3/CC_S3.2 Validate Click-to-Move TestLab")]
        public static void ValidateTestLab()
        {
            Scene scene = EditorSceneManager.OpenScene(TestLabScenePath, OpenSceneMode.Single);
            GameObject root = GameObject.Find(RootName);
            GameObject hostRoot = GameObject.Find(HostRootName);
            Camera camera = Camera.main;
            bool valid = scene.IsValid() && root != null && hostRoot != null &&
                         root.transform.parent == hostRoot.transform &&
                         root.transform.Find("WalkableSurface") != null &&
                         root.transform.Find("TechnicalPlayer") != null &&
                         root.GetComponentInChildren<MouseClickDestinationDriver>(true) != null &&
                         camera != null;
            EditorUtility.DisplayDialog("CC_S3.2 TestLab Validation", valid ? "PASS: required TestLab objects are present without changing the approved scene roots." : "FAIL: run the Build Click-to-Move TestLab menu item.", "OK");
        }

        private static void RemoveExistingLabRoot()
        {
            GameObject existing = GameObject.Find(RootName);
            if (existing != null) Undo.DestroyObjectImmediate(existing);
        }

        private static GameObject CreateFloor(Transform parent)
        {
            GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
            floor.name = "WalkableSurface";
            floor.transform.SetParent(parent, worldPositionStays: true);
            floor.transform.position = Vector3.zero;
            floor.transform.rotation = Quaternion.identity;
            floor.transform.localScale = new Vector3(2.5f, 1f, 2.5f);
            Undo.RegisterCreatedObjectUndo(floor, "Create walkable surface");
            return floor;
        }

        private static GameObject CreatePlayer(Transform parent)
        {
            GameObject player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            player.name = "TechnicalPlayer";
            player.transform.SetParent(parent, worldPositionStays: true);
            player.transform.position = new Vector3(0f, 1f, 0f);
            player.transform.rotation = Quaternion.identity;

            CapsuleCollider capsuleCollider = player.GetComponent<CapsuleCollider>();
            if (capsuleCollider != null) Object.DestroyImmediate(capsuleCollider);

            CharacterController controller = player.AddComponent<CharacterController>();
            controller.center = Vector3.zero;
            controller.height = 2f;
            controller.radius = 0.5f;
            controller.stepOffset = 0.3f;

            ClickToMoveAgent agent = player.AddComponent<ClickToMoveAgent>();
            agent.Configure(4f, 720f, 0.1f, -20f);
            player.AddComponent<ClickDestinationInput>();
            player.AddComponent<MouseClickDestinationDriver>();
            Undo.RegisterCreatedObjectUndo(player, "Create technical player");
            return player;
        }

        private static Camera ConfigureCamera()
        {
            Camera camera = Camera.main;
            if (camera == null)
            {
                Debug.LogError("[CC_S3.2] TestLab Main Camera was not found.");
                return null;
            }
            camera.transform.position = new Vector3(0f, 14f, -14f);
            camera.transform.LookAt(Vector3.zero);
            return camera;
        }
    }
}
