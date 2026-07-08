using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using VRMGames.CartridgeAndCloud.Application.SceneFlow;
using VRMGames.CartridgeAndCloud.Infrastructure.SceneFlow;
using VRMGames.CartridgeAndCloud.Runtime.Composition;

namespace VRMGames.CartridgeAndCloud.Tests.PlayMode.Store
{
    public sealed class W8RuntimeRegressionPlayModeTests
    {
        private const float TimeoutSeconds = 12f;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            yield return DestroyPersistentRoots();
            yield return LoadScene("TestLab");
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            yield return DestroyPersistentRoots();
            yield return LoadScene("TestLab");
        }

        [UnityTest]
        public IEnumerator W8_SceneRoundTripKeepsOneRootAndAllCrossWaveServices()
        {
            yield return LoadBootstrapAndWaitForMainMenu();

            ApplicationRoot application = ApplicationRoot.Instance;
            Assert.That(application, Is.Not.Null);

            application.RequestLoad(SceneId.StoreInitial);
            yield return WaitForScene("StoreInitial");
            yield return null;

            UIRuntimeCompositionRoot runtime =
                UIRuntimeCompositionRoot.Instance;

            Assert.That(FindRoots<ApplicationRoot>().Length, Is.EqualTo(1));
            Assert.That(FindRoots<UIRuntimeCompositionRoot>().Length, Is.EqualTo(1));
            Assert.That(runtime, Is.Not.Null);
            Assert.That(runtime.ActiveSession, Is.Not.Null);
            Assert.That(runtime.SimulationClock, Is.Not.Null);
            Assert.That(runtime.PauseService, Is.Not.Null);
            Assert.That(runtime.ManualSave, Is.Not.Null);
            Assert.That(runtime.SaveMutations, Is.Not.Null);
            Assert.That(runtime.Autosave, Is.Not.Null);
            Assert.That(runtime.Projection, Is.Not.Null);

            application.RequestLoad(SceneId.MainMenu);
            yield return WaitForScene("MainMenu");

            Assert.That(FindRoots<ApplicationRoot>().Length, Is.EqualTo(1));
            Assert.That(FindRoots<UIRuntimeCompositionRoot>().Length, Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator W8_RepeatedStoreTransitionDoesNotCreateDuplicateCompositionRoots()
        {
            yield return LoadBootstrapAndWaitForMainMenu();

            ApplicationRoot application = ApplicationRoot.Instance;
            application.RequestLoad(SceneId.StoreInitial);
            yield return WaitForScene("StoreInitial");
            application.RequestLoad(SceneId.MainMenu);
            yield return WaitForScene("MainMenu");
            application.RequestLoad(SceneId.StoreInitial);
            yield return WaitForScene("StoreInitial");
            yield return null;

            Assert.That(FindRoots<ApplicationRoot>().Length, Is.EqualTo(1));
            Assert.That(FindRoots<UIRuntimeCompositionRoot>().Length, Is.EqualTo(1));
        }

        private static IEnumerator LoadBootstrapAndWaitForMainMenu()
        {
            yield return LoadScene("Bootstrap");
            yield return WaitForScene("MainMenu");
        }

        private static IEnumerator LoadScene(string sceneName)
        {
            AsyncOperation operation =
                SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            Assert.That(operation, Is.Not.Null);

            while (!operation.isDone)
            {
                yield return null;
            }

            yield return null;
        }

        private static IEnumerator WaitForScene(string expected)
        {
            float timeout = Time.realtimeSinceStartup + TimeoutSeconds;
            while (SceneManager.GetActiveScene().name != expected)
            {
                if (Time.realtimeSinceStartup >= timeout)
                {
                    Assert.Fail(
                        $"Timed out waiting for '{expected}'. Current scene: " +
                        SceneManager.GetActiveScene().name);
                }

                yield return null;
            }

            yield return null;
        }

        private static IEnumerator DestroyPersistentRoots()
        {
            foreach (ApplicationRoot root in FindRoots<ApplicationRoot>())
            {
                if (root != null)
                {
                    Object.Destroy(root.gameObject);
                }
            }

            // UIRuntimeCompositionRoot is installed once before the test player
            // starts and is intentionally preserved between scene transitions.
            yield return null;
        }

        private static T[] FindRoots<T>() where T : Object
        {
            return Object.FindObjectsByType<T>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None);
        }
    }
}
