using System.Collections;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace VRMGames.CartridgeAndCloud.Tests.PlayMode
{
    public sealed class SceneLoadingSmokeTests
    {
        [UnityTest]
        public IEnumerator Bootstrap_LoadsWithNoRootGameObjects()
        {
            yield return LoadAndValidateScene("Bootstrap");
        }

        [UnityTest]
        public IEnumerator MainMenu_LoadsWithApprovedBaselineObjects()
        {
            yield return LoadAndValidateScene(
                "MainMenu",
                "Main Camera",
                "Directional Light");
        }

        [UnityTest]
        public IEnumerator Store_LoadsWithApprovedBaselineObjects()
        {
            yield return LoadAndValidateScene(
                "Store",
                "Main Camera",
                "Directional Light");
        }

        [UnityTest]
        public IEnumerator TestLab_LoadsWithApprovedBaselineObjects()
        {
            yield return LoadAndValidateScene(
                "TestLab",
                "Main Camera",
                "Directional Light");
        }

        private static IEnumerator LoadAndValidateScene(
            string sceneName,
            params string[] expectedRootNames)
        {
            Assert.That(Application.isPlaying, Is.True);

            AsyncOperation loadOperation =
                SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

            Assert.That(
                loadOperation,
                Is.Not.Null,
                $"Unity did not start loading scene {sceneName}.");

            while (!loadOperation.isDone)
            {
                yield return null;
            }

            // Allow one frame for Awake/OnEnable and scene activation to complete.
            yield return null;

            Scene activeScene = SceneManager.GetActiveScene();

            Assert.That(activeScene.isLoaded, Is.True);
            Assert.That(activeScene.name, Is.EqualTo(sceneName));

            GameObject[] rootObjects = activeScene.GetRootGameObjects();
            string[] actualRootNames = rootObjects.Select(root => root.name).ToArray();

            CollectionAssert.AreEquivalent(
                expectedRootNames,
                actualRootNames,
                $"Unexpected root objects in scene {sceneName}.");

            if (expectedRootNames.Length == 0)
            {
                yield break;
            }

            GameObject cameraObject =
                rootObjects.Single(root => root.name == "Main Camera");

            Assert.That(cameraObject.GetComponent<Camera>(), Is.Not.Null);
            Assert.That(cameraObject.GetComponent<AudioListener>(), Is.Not.Null);

            GameObject lightObject =
                rootObjects.Single(root => root.name == "Directional Light");

            Light directionalLight = lightObject.GetComponent<Light>();

            Assert.That(directionalLight, Is.Not.Null);
            Assert.That(directionalLight.type, Is.EqualTo(LightType.Directional));
        }
    }
}
