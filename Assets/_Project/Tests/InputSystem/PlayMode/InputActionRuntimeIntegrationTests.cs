using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using VRMGames.CartridgeAndCloud.Application.Camera;
using VRMGames.CartridgeAndCloud.Application.SceneFlow;
using VRMGames.CartridgeAndCloud.Infrastructure.InputSystem.Actions;
using VRMGames.CartridgeAndCloud.Infrastructure.SceneFlow;
using VRMGames.CartridgeAndCloud.Presentation.Camera;

namespace VRMGames.CartridgeAndCloud.InputSystem.Tests.PlayMode
{
    public sealed class InputActionRuntimeIntegrationTests
    {
        [UnitySetUp]
        public IEnumerator ResetPersistentRoot()
        {
            if (ApplicationRoot.Instance != null)
            {
                Object.Destroy(
                    ApplicationRoot.Instance.gameObject);

                yield return null;
            }
        }

        [UnityTearDown]
        public IEnumerator CleanupPersistentRoot()
        {
            if (ApplicationRoot.Instance != null)
            {
                Object.Destroy(
                    ApplicationRoot.Instance.gameObject);

                yield return null;
            }
        }

        [UnityTest]
        public IEnumerator BootstrapToMainMenu_AttachesUiRouter()
        {
            yield return SceneManager.LoadSceneAsync(
                "Bootstrap");

            while (
                SceneManager.GetActiveScene().name !=
                "MainMenu")
            {
                yield return null;
            }

            ApplicationRoot root =
                ApplicationRoot.Instance;

            while (root == null ||
                   root.IsTransitioning)
            {
                root =
                    ApplicationRoot.Instance;

                yield return null;
            }

            InputActionContextRouter router =
                root.GetComponent<
                    InputActionContextRouter>();

            Assert.That(router, Is.Not.Null);
            Assert.That(
                router.IsUiMapEnabled,
                Is.True);
            Assert.That(
                router.IsGameplayMapEnabled,
                Is.False);
        }

        [UnityTest]
        public IEnumerator MainMenuToTestLab_SwitchesRouterToGameplay()
        {
            yield return SceneManager.LoadSceneAsync(
                "Bootstrap");

            while (
                SceneManager.GetActiveScene().name !=
                "MainMenu")
            {
                yield return null;
            }

            ApplicationRoot root =
                ApplicationRoot.Instance;

            while (root == null ||
                   root.IsTransitioning)
            {
                root =
                    ApplicationRoot.Instance;

                yield return null;
            }

            InputActionContextRouter router =
                root.GetComponent<
                    InputActionContextRouter>();

            SceneTransitionRequestResult result =
                root.RequestLoad(
                    SceneId.TestLab);

            Assert.That(
                result,
                Is.EqualTo(
                    SceneTransitionRequestResult.Accepted));

            while (
                SceneManager.GetActiveScene().name !=
                "TestLab")
            {
                yield return null;
            }

            yield return null;

            Assert.That(
                ApplicationRoot.Instance,
                Is.SameAs(root));

            Assert.That(
                root.GetComponent<
                    InputActionContextRouter>(),
                Is.SameAs(router));

            Assert.That(
                router.IsUiMapEnabled,
                Is.False);

            Assert.That(
                router.IsGameplayMapEnabled,
                Is.True);
        }

        [UnityTest]
        public IEnumerator StandaloneDriver_AppliesOrbitAndZoom()
        {
            GameObject targetObject =
                new GameObject("CameraTarget");

            GameObject cameraObject =
                new GameObject("InputActionCamera");

            try
            {
                cameraObject.AddComponent<
                    UnityEngine.Camera>();

                OrbitCameraRig rig =
                    cameraObject.AddComponent<
                        OrbitCameraRig>();

                rig.Configure(
                    targetObject.transform,
                    0f,
                    45f,
                    10f,
                    new OrbitCameraConstraints(
                        25f,
                        75f,
                        6f,
                        24f));

                GameplayInputActionDriver driver =
                    cameraObject.AddComponent<
                        GameplayInputActionDriver>();

                driver.Configure(
                    contextRouter: null,
                    destinationInput: null,
                    cameraRig: rig,
                    orbitSensitivity: 1f,
                    zoomSensitivity: 1f);

                yield return null;

                bool handled =
                    driver.ApplyFrame(
                        new GameplayInputFrame(
                            Vector2.zero,
                            destinationPressed: false,
                            orbitHeld: true,
                            orbitDelta:
                                new Vector2(10f, 0f),
                            zoomDelta: 1f));

                Assert.That(handled, Is.True);

                Assert.That(
                    rig.CurrentState.YawDegrees,
                    Is.EqualTo(10f));

                Assert.That(
                    rig.CurrentState.Distance,
                    Is.EqualTo(9f));
            }
            finally
            {
                Object.Destroy(cameraObject);
                Object.Destroy(targetObject);
            }

            yield return null;
        }
    }
}
