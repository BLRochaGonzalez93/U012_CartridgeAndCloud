using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using VRMGames.CartridgeAndCloud.Application.InputContexts;
using VRMGames.CartridgeAndCloud.Application.SceneFlow;
using VRMGames.CartridgeAndCloud.Infrastructure.SceneFlow;

namespace VRMGames.CartridgeAndCloud.Tests.PlayMode
{
    public sealed class InputContextSceneFlowTests
    {
        [UnitySetUp]
        public IEnumerator ResetPersistentRoot()
        {
            if (ApplicationRoot.Instance != null)
            {
                Object.Destroy(ApplicationRoot.Instance.gameObject);
                yield return null;
            }
        }

        [UnityTearDown]
        public IEnumerator CleanupPersistentRoot()
        {
            if (ApplicationRoot.Instance != null)
            {
                Object.Destroy(ApplicationRoot.Instance.gameObject);
                yield return null;
            }
        }

        [UnityTest]
        public IEnumerator BootstrapToMainMenu_ActivatesUiContext()
        {
            yield return SceneManager.LoadSceneAsync("Bootstrap");

            while (SceneManager.GetActiveScene().name != "MainMenu")
            {
                yield return null;
            }

            ApplicationRoot root = ApplicationRoot.Instance;

            Assert.That(root, Is.Not.Null);
            Assert.That(root.InputContextService, Is.Not.Null);
            Assert.That(
                root.InputContextService.CurrentContext,
                Is.EqualTo(InputContextId.UI));
        }

        [UnityTest]
        public IEnumerator MainMenuToStore_ActivatesGameplayContext()
        {
            yield return SceneManager.LoadSceneAsync("Bootstrap");

            while (SceneManager.GetActiveScene().name != "MainMenu")
            {
                yield return null;
            }

            ApplicationRoot root = ApplicationRoot.Instance;

            while (root == null || root.IsTransitioning)
            {
                root = ApplicationRoot.Instance;
                yield return null;
            }

            IInputContextService service = root.InputContextService;

            SceneTransitionRequestResult request =
                root.RequestLoad(SceneId.Store);

            Assert.That(
                request,
                Is.EqualTo(SceneTransitionRequestResult.Accepted));

            while (SceneManager.GetActiveScene().name != "Store")
            {
                yield return null;
            }

            Assert.That(ApplicationRoot.Instance, Is.SameAs(root));
            Assert.That(
                ApplicationRoot.Instance.InputContextService,
                Is.SameAs(service));
            Assert.That(
                service.CurrentContext,
                Is.EqualTo(InputContextId.Gameplay));
        }
    }
}
