using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Application.Camera;
using VRMGames.CartridgeAndCloud.Infrastructure.Store;
using VRMGames.CartridgeAndCloud.Presentation.Grounding;
using VRMGames.CartridgeAndCloud.Presentation.Store.Authoring;
using VRMGames.CartridgeAndCloud.Presentation.Store.Occlusion;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Store
{
    public sealed class W7AuthoringCharacterizationTests
    {
        private const string FurnitureRoot =
            "Assets/_Project/Prefabs/Furniture";

        private const string SettingsPath =
            "Assets/_Project/Settings/Runtime/StoreRuntimeSettings.asset";

        [Test]
        public void CHAR_ART_001_AllPlaceablesHaveValidGroundReference()
        {
            string[] guids =
                AssetDatabase.FindAssets(
                    "t:Prefab",
                    new[] { FurnitureRoot });

            Assert.That(guids, Has.Length.EqualTo(8));

            foreach (string guid in guids)
            {
                string path =
                    AssetDatabase.GUIDToAssetPath(guid);

                GameObject prefab =
                    AssetDatabase.LoadAssetAtPath<
                        GameObject>(path);

                PrefabPhysicalContractAuthoring contract =
                    prefab.GetComponent<
                        PrefabPhysicalContractAuthoring>();

                Assert.That(contract, Is.Not.Null, path);
                Assert.That(
                    contract.TryValidate(
                        out string report),
                    Is.True,
                    report);

                Assert.That(
                    prefab.transform.Find(
                        PrefabPhysicalContractAuthoring
                            .GroundAnchorName),
                    Is.Not.Null,
                    path);
            }
        }

        [Test]
        public void CHAR_ART_002_GroundAnchorAlignsPlaceableWithoutRendererBounds()
        {
            GameObject prefab =
                AssetDatabase.LoadAssetAtPath<
                    GameObject>(
                        FurnitureRoot +
                        "/CentralShelf.prefab");

            GameObject instance =
                Object.Instantiate(prefab);

            try
            {
                instance.transform.position =
                    new Vector3(4f, 9f, -3f);

                Assert.That(
                    GroundingUtility
                        .AlignRootToGroundAnchor(
                            instance.transform,
                            2.5f),
                    Is.True);

                Transform anchor =
                    instance.transform.Find(
                        PrefabPhysicalContractAuthoring
                            .GroundAnchorName);

                Assert.That(
                    anchor.position.y,
                    Is.EqualTo(2.5f)
                        .Within(0.0001f));
            }
            finally
            {
                Object.DestroyImmediate(instance);
            }
        }

        [Test]
        public void CHAR_ART_003_RotationKeepsGroundAnchorOnBasePlane()
        {
            GameObject prefab =
                AssetDatabase.LoadAssetAtPath<
                    GameObject>(
                        FurnitureRoot +
                        "/CheckoutCounter.prefab");

            GameObject instance =
                Object.Instantiate(prefab);

            try
            {
                GroundingUtility
                    .AlignRootToGroundAnchor(
                        instance.transform,
                        0f);

                instance.transform.rotation =
                    Quaternion.Euler(
                        0f,
                        90f,
                        0f);

                Transform anchor =
                    instance.transform.Find(
                        PrefabPhysicalContractAuthoring
                            .GroundAnchorName);

                Assert.That(
                    anchor.position.y,
                    Is.EqualTo(0f)
                        .Within(0.0001f));
            }
            finally
            {
                Object.DestroyImmediate(instance);
            }
        }

        [Test]
        public void CHAR_ART_004_FurnitureRootsRemainIdentityAuthored()
        {
            foreach (string guid in
                     AssetDatabase.FindAssets(
                         "t:Prefab",
                         new[] { FurnitureRoot }))
            {
                string path =
                    AssetDatabase.GUIDToAssetPath(guid);

                GameObject prefab =
                    AssetDatabase.LoadAssetAtPath<
                        GameObject>(path);

                Assert.That(
                    prefab.transform.localPosition,
                    Is.EqualTo(Vector3.zero),
                    path);

                Assert.That(
                    prefab.transform.localRotation,
                    Is.EqualTo(Quaternion.identity),
                    path);

                Assert.That(
                    prefab.transform.localScale,
                    Is.EqualTo(Vector3.one),
                    path);
            }
        }

        [Test]
        public void CHAR_ART_005_FurnitureInteractionContractsAreComplete()
        {
            foreach (string guid in
                     AssetDatabase.FindAssets(
                         "t:Prefab",
                         new[] { FurnitureRoot }))
            {
                string path =
                    AssetDatabase.GUIDToAssetPath(guid);

                GameObject prefab =
                    AssetDatabase.LoadAssetAtPath<
                        GameObject>(path);

                StoreFixturePrefabAuthoring fixture =
                    prefab.GetComponent<
                        StoreFixturePrefabAuthoring>();

                Assert.That(fixture, Is.Not.Null, path);
                Assert.That(
                    fixture.TryValidate(
                        out string report),
                    Is.True,
                    report);
            }
        }

        [Test]
        public void CHAR_ART_006_CameraUsesCentralConstraintsAndClampsZoom()
        {
            StoreRuntimeSettingsAsset settings =
                AssetDatabase.LoadAssetAtPath<
                    StoreRuntimeSettingsAsset>(
                        SettingsPath);

            OrbitCameraConstraints constraints =
                settings.CreateCameraConstraints();

            OrbitCameraState state =
                OrbitCameraCalculator.CreateState(
                    settings.CameraYawDegrees,
                    settings.CameraPitchDegrees,
                    settings.CameraDistance,
                    constraints);

            OrbitCameraState closest =
                OrbitCameraCalculator.ApplyZoom(
                    state,
                    1000f,
                    constraints);

            OrbitCameraState farthest =
                OrbitCameraCalculator.ApplyZoom(
                    state,
                    -1000f,
                    constraints);

            Assert.That(
                closest.Distance,
                Is.EqualTo(
                    settings.CameraMinimumDistance));

            Assert.That(
                farthest.Distance,
                Is.EqualTo(
                    settings.CameraMaximumDistance));
        }

        [Test]
        public void CHAR_ART_007_WallOcclusionRemainsDisabledForH6()
        {
            StoreRuntimeSettingsAsset settings =
                AssetDatabase.LoadAssetAtPath<
                    StoreRuntimeSettingsAsset>(
                        SettingsPath);

            Assert.That(
                settings.HideOccludingWalls,
                Is.False);

            Assert.That(
                settings.AllowWallOcclusionToggle,
                Is.False);

            GameObject cameraObject =
                new GameObject("Camera");

            GameObject targetObject =
                new GameObject("Target");

            GameObject controllerObject =
                new GameObject("WallOcclusion");

            try
            {
                UnityEngine.Camera camera =
                    cameraObject.AddComponent<
                        UnityEngine.Camera>();

                WallOcclusionController controller =
                    controllerObject.AddComponent<
                        WallOcclusionController>();

                controller.Configure(
                    camera,
                    targetObject.transform,
                    defaultValue: true,
                    allowUserToggle: false);

                controller.SetEnabled(true);

                Assert.That(
                    controller.HideOccludingWalls,
                    Is.False);

                Assert.That(
                    controller.CanChangeVisibility,
                    Is.False);
            }
            finally
            {
                Object.DestroyImmediate(
                    controllerObject);

                Object.DestroyImmediate(
                    targetObject);

                Object.DestroyImmediate(
                    cameraObject);

                PlayerPrefs.DeleteKey(
                    WallOcclusionController
                        .PlayerPrefsKey);
            }
        }

        [Test]
        public void CHAR_ART_008_RuntimeAuthoringSettingsAreCentralAndValid()
        {
            StoreRuntimeSettingsAsset settings =
                AssetDatabase.LoadAssetAtPath<
                    StoreRuntimeSettingsAsset>(
                        SettingsPath);

            Assert.That(settings, Is.Not.Null);
            Assert.That(
                settings.TryValidateAuthoring(
                    out string report),
                Is.True,
                report);

            Assert.That(
                settings.CameraOrbitSensitivity,
                Is.GreaterThan(0f));

            Assert.That(
                settings.CameraZoomSensitivity,
                Is.GreaterThan(0f));

            Assert.That(
                settings.CameraFarClipPlane,
                Is.GreaterThan(
                    settings.CameraNearClipPlane));
        }
    }
}
