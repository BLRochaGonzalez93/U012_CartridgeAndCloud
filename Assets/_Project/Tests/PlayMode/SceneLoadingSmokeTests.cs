using System.Collections;
using System.Linq;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.SceneFlow;
using VRMGames.CartridgeAndCloud.Infrastructure.SceneFlow;
using VRMGames.CartridgeAndCloud.Presentation.SceneFlow;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace VRMGames.CartridgeAndCloud.Tests.PlayMode
{
    public sealed class SceneLoadingSmokeTests
    {
        private const float SceneLoadTimeoutSeconds = 10f;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            yield return DestroyApplicationRoots();
            yield return LoadScene("TestLab");
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            yield return DestroyApplicationRoots();
            yield return LoadScene("TestLab");
        }

        [UnityTest]
        public IEnumerator Bootstrap_WhenLoaded_OpensMainMenuWithSingleApplicationRoot()
        {
            yield return LoadBootstrapAndWaitForMainMenu();

            ApplicationRoot[] roots = FindApplicationRoots();

            Assert.That(roots.Length, Is.EqualTo(1));
            Assert.That(roots[0].name, Is.EqualTo(ApplicationRoot.RootObjectName));
            Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("MainMenu"));
        }

        [UnityTest]
        public IEnumerator MainMenu_LoadsWithExpectedTechnicalObjects()
        {
            yield return LoadScene("MainMenu");

            AssertActiveSceneRootNames(
                "Main Camera",
                "Directional Light",
                "MainMenuController",
                "Canvas",
                "EventSystem");

            Assert.That(
                Object.FindFirstObjectByType<MainMenuController>(),
                Is.Not.Null);
        }

        [UnityTest]
        public IEnumerator Store_LoadsWithExpectedTechnicalObjects()
        {
            yield return LoadScene("Store");

            AssertActiveSceneRootNames(
                "Main Camera",
                "Directional Light",
                "StoreSceneController",
                "Canvas",
                "EventSystem");

            Assert.That(
                Object.FindFirstObjectByType<StoreSceneController>(),
                Is.Not.Null);
        }

        [UnityTest]
        public IEnumerator TestLab_LoadsWithApprovedBaselineObjects()
        {
            yield return LoadScene("TestLab");

            AssertActiveSceneRootNames(
                "Main Camera",
                "Directional Light");
        }

        [UnityTest]
        public IEnumerator MainMenuController_WhenEnterStoreInvoked_LoadsStore()
        {
            yield return LoadBootstrapAndWaitForMainMenu();

            MainMenuController controller =
                Object.FindFirstObjectByType<MainMenuController>();

            Assert.That(controller, Is.Not.Null);

            controller.EnterStore();
            yield return WaitForActiveScene("Store");
        }

        [UnityTest]
        public IEnumerator StoreController_WhenReturnInvoked_LoadsMainMenu()
        {
            yield return LoadBootstrapAndWaitForMainMenu();

            ApplicationRoot.Instance.RequestLoad(SceneId.Store);
            yield return WaitForActiveScene("Store");

            StoreSceneController controller =
                Object.FindFirstObjectByType<StoreSceneController>();

            Assert.That(controller, Is.Not.Null);

            controller.ReturnToMainMenu();
            yield return WaitForActiveScene("MainMenu");
        }

        [UnityTest]
        public IEnumerator SceneFlow_AfterRoundTrip_KeepsSingleApplicationRoot()
        {
            yield return LoadBootstrapAndWaitForMainMenu();

            ApplicationRoot.Instance.RequestLoad(SceneId.Store);
            yield return WaitForActiveScene("Store");

            ApplicationRoot.Instance.RequestLoad(SceneId.MainMenu);
            yield return WaitForActiveScene("MainMenu");

            Assert.That(FindApplicationRoots().Length, Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator SceneFlow_WhenRequestedRepeatedly_RejectsConcurrentTransition()
        {
            yield return LoadBootstrapAndWaitForMainMenu();

            SceneTransitionRequestResult firstResult =
                ApplicationRoot.Instance.RequestLoad(SceneId.Store);

            SceneTransitionRequestResult concurrentResult =
                ApplicationRoot.Instance.RequestLoad(SceneId.TestLab);

            Assert.That(firstResult, Is.EqualTo(SceneTransitionRequestResult.Accepted));
            Assert.That(
                concurrentResult,
                Is.EqualTo(SceneTransitionRequestResult.TransitionInProgress));

            yield return WaitForActiveScene("Store");
        }

        private static IEnumerator LoadBootstrapAndWaitForMainMenu()
        {
            AsyncOperation operation =
                SceneManager.LoadSceneAsync("Bootstrap", LoadSceneMode.Single);

            Assert.That(
                operation,
                Is.Not.Null,
                "Unity did not start loading scene 'Bootstrap'.");

            while (!operation.isDone)
            {
                yield return null;
            }

            yield return WaitForActiveScene("MainMenu");
        }

        private static IEnumerator LoadScene(string sceneName)
        {
            AsyncOperation operation =
                SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

            Assert.That(
                operation,
                Is.Not.Null,
                $"Unity did not start loading scene '{sceneName}'.");

            while (!operation.isDone)
            {
                yield return null;
            }

            yield return null;

            Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo(sceneName));
        }

        private static IEnumerator WaitForActiveScene(string expectedSceneName)
        {
            float timeoutAt = Time.realtimeSinceStartup + SceneLoadTimeoutSeconds;

            while (SceneManager.GetActiveScene().name != expectedSceneName)
            {
                if (Time.realtimeSinceStartup >= timeoutAt)
                {
                    Assert.Fail(
                        $"Timed out waiting for active scene '{expectedSceneName}'. " +
                        $"Current scene: '{SceneManager.GetActiveScene().name}'.");
                }

                yield return null;
            }

            yield return null;
        }

        private static IEnumerator DestroyApplicationRoots()
        {
            ApplicationRoot[] roots = FindApplicationRoots();

            foreach (ApplicationRoot root in roots)
            {
                if (root != null)
                {
                    Object.Destroy(root.gameObject);
                }
            }

            if (roots.Length > 0)
            {
                yield return null;
            }
        }

        private static ApplicationRoot[] FindApplicationRoots()
        {
            return Object.FindObjectsByType<ApplicationRoot>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None);
        }

        private static void AssertActiveSceneRootNames(params string[] expectedRootNames)
        {
            Scene activeScene = SceneManager.GetActiveScene();
            string[] actualRootNames = activeScene
                .GetRootGameObjects()
                .Select(root => root.name)
                .ToArray();

            CollectionAssert.AreEquivalent(
                expectedRootNames,
                actualRootNames,
                $"Unexpected root objects in scene '{activeScene.name}'.");
        }
    }
}
