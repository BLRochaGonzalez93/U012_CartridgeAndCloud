using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using VRMGames.CartridgeAndCloud.Infrastructure.InputSystem.Actions;
using VRMGames.CartridgeAndCloud.Infrastructure.InputSystem.Placement;
using VRMGames.CartridgeAndCloud.Presentation.Placement;

namespace VRMGames.CartridgeAndCloud.Editor.Sprint4
{
    public static class CCS42PlacementPreviewInstaller
    {
        private const string TestLabScenePath =
            "Assets/_Project/Scenes/TestLab.unity";
        private const string S3RootName = "S3_ClickToMoveLab";
        private const string FloorName = "WalkableSurface";
        private const string S4RootName = "S4_PlacementPreviewLab";
        private const string DefinitionFolder =
            "Assets/_Project/Content/Placement";
        private const string DefinitionPath =
            DefinitionFolder + "/TechnicalShelf_4x2.asset";
        private const string MaterialFolder =
            "Assets/_Project/Art/Materials/Technical";
        private const string ValidMaterialPath =
            MaterialFolder + "/CC_S4_GhostValid.mat";
        private const string InvalidMaterialPath =
            MaterialFolder + "/CC_S4_GhostInvalid.mat";

        [MenuItem(
            "Cartridge & Cloud/Sprint 4/" +
            "CC_S4.2 Build Placement Preview TestLab")]
        public static void BuildTestLab()
        {
            bool confirmed = EditorUtility.DisplayDialog(
                "Build CC_S4.2 TestLab",
                "This adds a snapped 4x2 placement ghost " +
                "to the existing Sprint 3 TestLab. " +
                "It does not confirm or occupy cells.",
                "Build",
                "Cancel");

            if (!confirmed)
            {
                return;
            }

            Scene scene = EditorSceneManager.OpenScene(
                TestLabScenePath,
                OpenSceneMode.Single);

            GameObject s3Root = GameObject.Find(S3RootName);

            if (s3Root == null)
            {
                Debug.LogError(
                    "[CC_S4.2] S3_ClickToMoveLab was not found.");
                return;
            }

            Transform floorTransform =
                s3Root.transform.Find(FloorName);

            if (floorTransform == null)
            {
                Debug.LogError(
                    "[CC_S4.2] WalkableSurface was not found.");
                return;
            }

            Collider floorCollider =
                floorTransform.GetComponent<Collider>();

            if (floorCollider == null)
            {
                Debug.LogError(
                    "[CC_S4.2] WalkableSurface requires a Collider.");
                return;
            }

            UnityEngine.Camera camera = UnityEngine.Camera.main;

            if (camera == null)
            {
                Debug.LogError(
                    "[CC_S4.2] Main Camera was not found.");
                return;
            }

            RemoveExistingLabRoot();

            PlacementSurface surface =
                floorTransform.GetComponent<PlacementSurface>();

            if (surface == null)
            {
                surface =
                    Undo.AddComponent<PlacementSurface>(
                        floorTransform.gameObject);
            }

            surface.Configure(
                floorCollider,
                new Vector3(-4f, 0f, -4f),
                gridWidth: 16,
                gridDepth: 16,
                cellSize: 0.5f,
                raycastDistance: 500f);

            TechnicalPlaceableDefinition definition =
                CreateOrUpdateDefinition();

            Material validMaterial = CreateOrUpdateGhostMaterial(
                ValidMaterialPath,
                new Color(0.15f, 0.85f, 0.35f, 0.45f));

            Material invalidMaterial = CreateOrUpdateGhostMaterial(
                InvalidMaterialPath,
                new Color(0.95f, 0.2f, 0.2f, 0.45f));

            if (definition == null ||
                validMaterial == null ||
                invalidMaterial == null)
            {
                return;
            }

            GameObject labRoot = new GameObject(S4RootName);
            labRoot.transform.SetParent(
                s3Root.transform,
                worldPositionStays: false);

            Undo.RegisterCreatedObjectUndo(
                labRoot,
                "Create CC_S4.2 placement lab");

            GameObject visual =
                GameObject.CreatePrimitive(PrimitiveType.Cube);
            visual.name = "TechnicalShelfGhost_4x2";
            visual.transform.SetParent(
                labRoot.transform,
                worldPositionStays: true);

            Collider visualCollider =
                visual.GetComponent<Collider>();

            if (visualCollider != null)
            {
                Object.DestroyImmediate(visualCollider);
            }

            Renderer[] renderers =
                visual.GetComponentsInChildren<Renderer>(true);

            PlacementGhostView ghostView =
                labRoot.AddComponent<PlacementGhostView>();
            ghostView.Configure(
                visual.transform,
                renderers,
                validMaterial,
                invalidMaterial);

            PlacementPreviewController controller =
                labRoot.AddComponent<PlacementPreviewController>();
            controller.Configure(
                surface,
                definition,
                ghostView,
                camera);

            PlacementInputActionDriver inputDriver =
                labRoot.AddComponent<PlacementInputActionDriver>();
            inputDriver.Configure(
                contextRouter: null,
                previewController: controller);

            controller.ClearPreview();

            EditorUtility.SetDirty(floorTransform.gameObject);
            EditorUtility.SetDirty(labRoot);
            EditorSceneManager.MarkSceneDirty(scene);
            AssetDatabase.SaveAssets();
            EditorSceneManager.SaveScene(scene);

            Selection.activeGameObject = labRoot;
            EditorGUIUtility.PingObject(labRoot);

            Debug.Log(
                "[CC_S4.2] Placement preview installed. " +
                "Enter Play Mode, point at the floor and use Q/E.");
        }

        [MenuItem(
            "Cartridge & Cloud/Sprint 4/" +
            "CC_S4.2 Validate Placement Preview TestLab")]
        public static void ValidateTestLab()
        {
            Scene scene = EditorSceneManager.OpenScene(
                TestLabScenePath,
                OpenSceneMode.Single);

            GameObject s3Root = GameObject.Find(S3RootName);

            Transform floor =
                s3Root != null
                    ? s3Root.transform.Find(FloorName)
                    : null;

            Transform s4Root =
                s3Root != null
                    ? s3Root.transform.Find(S4RootName)
                    : null;

            PlacementSurface surface =
                floor != null
                    ? floor.GetComponent<PlacementSurface>()
                    : null;

            PlacementPreviewController controller =
                s4Root != null
                    ? s4Root.GetComponent<PlacementPreviewController>()
                    : null;

            PlacementInputActionDriver inputDriver =
                s4Root != null
                    ? s4Root.GetComponent<PlacementInputActionDriver>()
                    : null;

            bool actionsValid;

            using (ProjectInputActions actions =
                   new ProjectInputActions())
            {
                actionsValid =
                    actions.RotatePlacementCounterClockwise != null &&
                    actions.RotatePlacementClockwise != null;
            }

            bool valid =
                scene.IsValid() &&
                HasApprovedRootObjects(scene) &&
                s3Root != null &&
                floor != null &&
                surface != null &&
                surface.GridWidth == 16 &&
                surface.GridDepth == 16 &&
                Mathf.Approximately(surface.CellSize, 0.5f) &&
                s4Root != null &&
                s4Root.parent == s3Root.transform &&
                controller != null &&
                controller.Surface == surface &&
                controller.Definition != null &&
                controller.GhostView != null &&
                inputDriver != null &&
                actionsValid;

            EditorUtility.DisplayDialog(
                "CC_S4.2 Placement Preview Validation",
                valid
                    ? "PASS: placement preview, 0.5 m grid, " +
                      "Q/E actions and approved TestLab roots are correct."
                    : "FAIL: run the CC_S4.2 build command.",
                "OK");
        }

        private static void RemoveExistingLabRoot()
        {
            GameObject existing = GameObject.Find(S4RootName);

            if (existing != null)
            {
                Undo.DestroyObjectImmediate(existing);
            }
        }

        private static TechnicalPlaceableDefinition
            CreateOrUpdateDefinition()
        {
            EnsureFolder(DefinitionFolder);

            TechnicalPlaceableDefinition definition =
                AssetDatabase.LoadAssetAtPath<
                    TechnicalPlaceableDefinition>(DefinitionPath);

            if (definition == null)
            {
                definition =
                    ScriptableObject.CreateInstance<
                        TechnicalPlaceableDefinition>();

                definition.Configure(4, 2, 1.2f);
                AssetDatabase.CreateAsset(definition, DefinitionPath);
            }
            else
            {
                definition.Configure(4, 2, 1.2f);
                EditorUtility.SetDirty(definition);
            }

            return definition;
        }

        private static Material CreateOrUpdateGhostMaterial(
            string path,
            Color color)
        {
            EnsureFolder(MaterialFolder);

            Shader shader =
                Shader.Find("Universal Render Pipeline/Unlit");

            if (shader == null)
            {
                shader = Shader.Find("Universal Render Pipeline/Lit");
            }

            if (shader == null)
            {
                shader = Shader.Find("Standard");
            }

            if (shader == null)
            {
                Debug.LogError(
                    "[CC_S4.2] No compatible shader was found.");
                return null;
            }

            Material material =
                AssetDatabase.LoadAssetAtPath<Material>(path);

            if (material == null)
            {
                material = new Material(shader);
                AssetDatabase.CreateAsset(material, path);
            }
            else
            {
                material.shader = shader;
            }

            material.color = color;

            if (material.HasProperty("_BaseColor"))
            {
                material.SetColor("_BaseColor", color);
            }

            if (material.HasProperty("_Surface"))
            {
                material.SetFloat("_Surface", 1f);
            }

            if (material.HasProperty("_SrcBlend"))
            {
                material.SetFloat(
                    "_SrcBlend",
                    (float)BlendMode.SrcAlpha);
            }

            if (material.HasProperty("_DstBlend"))
            {
                material.SetFloat(
                    "_DstBlend",
                    (float)BlendMode.OneMinusSrcAlpha);
            }

            if (material.HasProperty("_ZWrite"))
            {
                material.SetFloat("_ZWrite", 0f);
            }

            material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            material.renderQueue = (int)RenderQueue.Transparent;
            EditorUtility.SetDirty(material);
            return material;
        }

        private static void EnsureFolder(string assetFolder)
        {
            if (AssetDatabase.IsValidFolder(assetFolder))
            {
                return;
            }

            Directory.CreateDirectory(assetFolder);
            AssetDatabase.Refresh();
        }

        private static bool HasApprovedRootObjects(Scene scene)
        {
            GameObject[] roots = scene.GetRootGameObjects();

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

            return hasMainCamera && hasDirectionalLight;
        }
    }
}
