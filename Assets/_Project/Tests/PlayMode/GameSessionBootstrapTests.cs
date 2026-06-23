using System.Collections;
using NUnit.Framework;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using VRMGames.CartridgeAndCloud.Application.SceneFlow;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;
using VRMGames.CartridgeAndCloud.Infrastructure.SceneFlow;

namespace VRMGames.CartridgeAndCloud.Tests.PlayMode
{
    public sealed class GameSessionBootstrapTests
    {
        [UnityTest]
        public IEnumerator Bootstrap_CreatesOneActiveMinimalSession()
        {
            yield return SceneManager.LoadSceneAsync("Bootstrap");
            yield return null;
            yield return null;

            ApplicationRoot root = ApplicationRoot.Instance;

            Assert.That(root, Is.Not.Null);
            Assert.That(root.GameSessionService, Is.Not.Null);
            Assert.That(root.GameSessionService.HasActiveSession, Is.True);
            Assert.That(root.GameSessionService.Current.SlotId, Is.EqualTo(new SaveSlotId(0)));
            Assert.That(root.GameSessionService.Current.CurrentDay, Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator SceneRoundTrip_PreservesSameSessionIdentity()
        {
            yield return SceneManager.LoadSceneAsync("Bootstrap");
            yield return null;
            yield return null;

            ApplicationRoot root = ApplicationRoot.Instance;
            StableId sessionId = root.GameSessionService.Current.SessionId;

            root.RequestLoad(SceneId.Store);

            while (SceneManager.GetActiveScene().name != "Store")
            {
                yield return null;
            }

            Assert.That(ApplicationRoot.Instance, Is.SameAs(root));
            Assert.That(
                ApplicationRoot.Instance.GameSessionService.Current.SessionId,
                Is.EqualTo(sessionId));
        }
    }
}
