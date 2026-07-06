#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;
using VRMGames.CartridgeAndCloud.Domain.Characters;
using VRMGames.CartridgeAndCloud.Infrastructure.Customers;
using VRMGames.CartridgeAndCloud.Presentation.Characters;
using VRMGames.CartridgeAndCloud.Runtime.Characters;

namespace VRMGames.CartridgeAndCloud.Editor.AssetPipeline
{
    public static class SyntyCharacterIntegrationInstaller
    {
        private const string Root = "Assets/_Project/Resources/Characters";
        private const string ControllerRoot = "Assets/_Project/Animations/Characters/Controllers";
        private const string Marker = Root + "/.synty_character_integration_v2";
        private const string ShopsRoot = "Assets/ThirdParty/Synty/POLYGON/Shops/PolygonShops/Prefabs/Characters";
        private const string AnimationRoot = "Assets/ThirdParty/Synty/PackageHelper/Synty/AnimationBaseLocomotion/Animations/Polygon";

        private const string MasculineControllerPath = ControllerRoot + "/AC_CC_Character_Masculine.controller";
        private const string FeminineControllerPath = ControllerRoot + "/AC_CC_Character_Feminine.controller";

        [MenuItem("Cartridge & Cloud/Asset Pipeline/Install Synty Characters")]
        public static void Install()
        {
            try
            {
                EnsureFolders();

                RuntimeAnimatorController masculine = CreateMinimalController(
                    MasculineControllerPath,
                    AnimationRoot + "/Masculine/Idle/A_Idle_Standing_Masc.fbx",
                    AnimationRoot + "/Masculine/Locomotion/Walk/A_Walk_F_Masc.fbx",
                    AnimationRoot + "/Masculine/Locomotion/Run/A_Run_F_Masc.fbx");

                RuntimeAnimatorController feminine = CreateMinimalController(
                    FeminineControllerPath,
                    AnimationRoot + "/Feminine/Idle/A_Idle_Standing_Femn.fbx",
                    AnimationRoot + "/Feminine/Locomotion/Walk/A_Walk_F_Femn.fbx",
                    AnimationRoot + "/Feminine/Locomotion/Run/A_Run_F_Femn.fbx");

                GameObject player = CreateOrUpdateWrapper(
                    "Player/PF_CC_Player_Clerk_Male_01",
                    "SM_Chr_Clerk_Male_01",
                    CharacterRole.Player,
                    masculine,
                    false);

                GameObject employeeFemale = CreateOrUpdateWrapper(
                    "Employees/PF_CC_Employee_Attendant_Female_01",
                    "SM_Chr_Attendant_Female_01",
                    CharacterRole.Employee,
                    feminine,
                    false);

                GameObject employeeMale = CreateOrUpdateWrapper(
                    "Employees/PF_CC_Employee_Attendant_Male_01",
                    "SM_Chr_Attendant_Male_01",
                    CharacterRole.Employee,
                    masculine,
                    false);

                GameObject supplierFemale = CreateOrUpdateWrapper(
                    "Suppliers/PF_CC_Supplier_Worker_Female_01",
                    "SM_Chr_Worker_Female_01",
                    CharacterRole.Supplier,
                    feminine,
                    true);

                GameObject supplierMale = CreateOrUpdateWrapper(
                    "Suppliers/PF_CC_Supplier_Worker_Male_01",
                    "SM_Chr_Worker_Male_01",
                    CharacterRole.Supplier,
                    masculine,
                    true);

                GameObject customerFemale = CreateOrUpdateWrapper(
                    "Customers/PF_CC_Customer_Shopper_Female_01",
                    "SM_Chr_Shopper_Female_01",
                    CharacterRole.Customer,
                    feminine,
                    false);

                GameObject customerMale = CreateOrUpdateWrapper(
                    "Customers/PF_CC_Customer_Shopper_Male_01",
                    "SM_Chr_Shopper_Male_01",
                    CharacterRole.Customer,
                    masculine,
                    false);

                AssignCustomerProfiles(customerFemale, customerMale);

                File.WriteAllText(Marker, DateTime.UtcNow.ToString("O"));
                AssetDatabase.ImportAsset(Marker, ImportAssetOptions.ForceUpdate);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                Debug.Log(
                    "[Characters] Minimal Synty locomotion installed. " +
                    "All character wrappers now use Idle/Walk/Run only; jump and fall states are unavailable.");
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
        }

        private static RuntimeAnimatorController CreateMinimalController(
            string controllerPath,
            string idlePath,
            string walkPath,
            string runPath)
        {
            AnimationClip idle = LoadAnimationClip(idlePath);
            AnimationClip walk = LoadAnimationClip(walkPath);
            AnimationClip run = LoadAnimationClip(runPath);

            if (idle == null || walk == null || run == null)
            {
                throw new InvalidOperationException(
                    "One or more required Synty in-place locomotion clips could not be loaded. " +
                    $"Idle: {idlePath}; Walk: {walkPath}; Run: {runPath}");
            }

            AssetDatabase.DeleteAsset(controllerPath);
            AnimatorController controller =
                AnimatorController.CreateAnimatorControllerAtPath(controllerPath);
            controller.AddParameter("MoveSpeed", AnimatorControllerParameterType.Float);

            AnimatorStateMachine stateMachine = controller.layers[0].stateMachine;
            stateMachine.name = "Base Layer";

            BlendTree blendTree = new BlendTree
            {
                name = "Locomotion",
                blendType = BlendTreeType.Simple1D,
                blendParameter = "MoveSpeed",
                useAutomaticThresholds = false
            };
            AssetDatabase.AddObjectToAsset(blendTree, controller);

            blendTree.AddChild(idle, 0f);
            blendTree.AddChild(walk, 1.4f);
            blendTree.AddChild(run, 2.5f);

            AnimatorState locomotion = stateMachine.AddState("Locomotion");
            locomotion.motion = blendTree;
            locomotion.writeDefaultValues = true;
            stateMachine.defaultState = locomotion;

            EditorUtility.SetDirty(blendTree);
            EditorUtility.SetDirty(controller);
            AssetDatabase.SaveAssets();
            return controller;
        }

        private static AnimationClip LoadAnimationClip(string assetPath)
        {
            UnityEngine.Object[] assets = AssetDatabase.LoadAllAssetsAtPath(assetPath);
            foreach (UnityEngine.Object asset in assets)
            {
                if (asset is AnimationClip clip &&
                    !clip.name.StartsWith("__preview__", StringComparison.OrdinalIgnoreCase))
                {
                    return clip;
                }
            }

            return null;
        }

        private static GameObject CreateOrUpdateWrapper(
            string relativePath,
            string sourceName,
            CharacterRole role,
            RuntimeAnimatorController controller,
            bool addCarrySocket)
        {
            string sourcePath = ShopsRoot + "/" + sourceName + ".prefab";
            GameObject source = AssetDatabase.LoadAssetAtPath<GameObject>(sourcePath);
            if (source == null)
            {
                throw new FileNotFoundException(
                    "Synty character prefab not found.",
                    sourcePath);
            }

            string destination = Root + "/" + relativePath + ".prefab";
            EnsureFolder(Path.GetDirectoryName(destination)?.Replace('\\', '/'));

            GameObject existing = AssetDatabase.LoadAssetAtPath<GameObject>(destination);
            if (existing == null)
            {
                return CreateWrapper(
                    destination,
                    source,
                    sourceName,
                    role,
                    controller,
                    addCarrySocket);
            }

            GameObject root = PrefabUtility.LoadPrefabContents(destination);
            try
            {
                EnsureActorContract(root, role);

                CharacterPresence presence = root.GetComponent<CharacterPresence>();
                if (presence == null)
                {
                    presence = root.AddComponent<CharacterPresence>();
                }
                presence.Configure(Path.GetFileNameWithoutExtension(destination), role);

                CharacterLocomotionAnimator driver =
                    root.GetComponent<CharacterLocomotionAnimator>();
                if (driver == null)
                {
                    driver = root.AddComponent<CharacterLocomotionAnimator>();
                }

                Animator animator = root.GetComponentInChildren<Animator>(true);
                if (animator == null)
                {
                    throw new InvalidOperationException(
                        $"Wrapper contains no Animator: {destination}");
                }

                animator.runtimeAnimatorController = controller;
                animator.applyRootMotion = false;
                animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
                driver.Configure(animator);

                if (addCarrySocket)
                {
                    EnsureCarrySocket(root.transform);
                }

                PrefabUtility.SaveAsPrefabAsset(root, destination);
            }
            finally
            {
                PrefabUtility.UnloadPrefabContents(root);
            }

            return AssetDatabase.LoadAssetAtPath<GameObject>(destination);
        }

        private static GameObject CreateWrapper(
            string destination,
            GameObject source,
            string sourceName,
            CharacterRole role,
            RuntimeAnimatorController controller,
            bool addCarrySocket)
        {
            GameObject root = new GameObject(Path.GetFileNameWithoutExtension(destination));
            root.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            EnsureActorContract(root, role);

            CharacterPresence presence = root.AddComponent<CharacterPresence>();
            presence.Configure(Path.GetFileNameWithoutExtension(destination), role);
            CharacterLocomotionAnimator driver =
                root.AddComponent<CharacterLocomotionAnimator>();

            GameObject visualRoot = new GameObject("VisualRoot");
            visualRoot.transform.SetParent(root.transform, false);
            GameObject visual = (GameObject)PrefabUtility.InstantiatePrefab(
                source,
                visualRoot.transform);
            visual.name = sourceName;
            visual.transform.localPosition = Vector3.zero;
            visual.transform.localRotation = Quaternion.identity;
            visual.transform.localScale = Vector3.one;

            Animator animator = visual.GetComponentInChildren<Animator>(true);
            if (animator == null)
            {
                animator = visual.AddComponent<Animator>();
            }

            animator.runtimeAnimatorController = controller;
            animator.applyRootMotion = false;
            animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
            driver.Configure(animator);

            if (addCarrySocket)
            {
                EnsureCarrySocket(root.transform);
            }

            GameObject saved = PrefabUtility.SaveAsPrefabAsset(root, destination);
            UnityEngine.Object.DestroyImmediate(root);
            return saved;
        }

        private static void EnsureActorContract(
            GameObject root,
            CharacterRole role)
        {
            CapsuleCollider capsule = root.GetComponent<CapsuleCollider>();
            if (capsule == null)
            {
                capsule = root.AddComponent<CapsuleCollider>();
            }

            NavMeshAgent agent = root.GetComponent<NavMeshAgent>();
            if (agent == null)
            {
                agent = root.AddComponent<NavMeshAgent>();
            }

            ActorPrefabAuthoring authoring = root.GetComponent<ActorPrefabAuthoring>();
            if (authoring == null)
            {
                authoring = root.AddComponent<ActorPrefabAuthoring>();
            }

            authoring.Configure(role);
            EditorUtility.SetDirty(capsule);
            EditorUtility.SetDirty(agent);
            EditorUtility.SetDirty(authoring);
        }

        private static void EnsureCarrySocket(Transform root)
        {
            Transform anchors = root.Find("Anchors");
            if (anchors == null)
            {
                GameObject anchorsObject = new GameObject("Anchors");
                anchorsObject.transform.SetParent(root, false);
                anchors = anchorsObject.transform;
            }

            Transform socket = anchors.Find("CarrySocket");
            if (socket == null)
            {
                GameObject socketObject = new GameObject("CarrySocket");
                socketObject.transform.SetParent(anchors, false);
                socket = socketObject.transform;
            }

            socket.localPosition = new Vector3(0f, 1.05f, 0.38f);
            socket.localRotation = Quaternion.identity;
            socket.localScale = Vector3.one;
        }

        private static void AssignCustomerProfiles(GameObject female, GameObject male)
        {
            string[] profileGuids = AssetDatabase.FindAssets(
                "t:CustomerProfileAsset",
                new[] { "Assets/_Project/Data/Customers" });

            Array.Sort(profileGuids, StringComparer.Ordinal);
            for (int index = 0; index < profileGuids.Length; index++)
            {
                string path = AssetDatabase.GUIDToAssetPath(profileGuids[index]);
                CustomerProfileAsset profile =
                    AssetDatabase.LoadAssetAtPath<CustomerProfileAsset>(path);
                if (profile == null)
                {
                    continue;
                }

                SerializedObject serialized = new SerializedObject(profile);
                SerializedProperty property =
                    serialized.FindProperty("_technicalPrefab");
                property.objectReferenceValue = index % 2 == 0 ? female : male;
                serialized.ApplyModifiedPropertiesWithoutUndo();
                EditorUtility.SetDirty(profile);
            }
        }

        private static void EnsureFolders()
        {
            EnsureFolder("Assets/_Project/Animations");
            EnsureFolder("Assets/_Project/Animations/Characters");
            EnsureFolder(ControllerRoot);
            EnsureFolder("Assets/_Project/Resources");
            EnsureFolder(Root);
            EnsureFolder(Root + "/Player");
            EnsureFolder(Root + "/Employees");
            EnsureFolder(Root + "/Suppliers");
            EnsureFolder(Root + "/Customers");
        }

        private static void EnsureFolder(string path)
        {
            if (string.IsNullOrWhiteSpace(path) || AssetDatabase.IsValidFolder(path))
            {
                return;
            }

            string parent = Path.GetDirectoryName(path)?.Replace('\\', '/');
            string name = Path.GetFileName(path);
            EnsureFolder(parent);
            AssetDatabase.CreateFolder(parent, name);
        }
    }
}
#endif
