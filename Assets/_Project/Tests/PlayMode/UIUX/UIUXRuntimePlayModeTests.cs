using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using VRMGames.CartridgeAndCloud.Application.UIUX;
using VRMGames.CartridgeAndCloud.Infrastructure.UIUX;

using VRMGames.CartridgeAndCloud.Runtime.Composition;
using VRMGames.CartridgeAndCloud.Runtime.UIUX;
namespace VRMGames.CartridgeAndCloud.Tests.PlayMode.UIUX
{
    public sealed class UIUXRuntimePlayModeTests
    {
        [UnityTest]
        public IEnumerator RuntimeRoot_IsInstalled()
        {
            yield return null;

            Assert.That(
                UIRuntimeCompositionRoot.Instance,
                Is.Not.Null);
        }

        [UnityTest]
        public IEnumerator RuntimeRoot_HasSettings()
        {
            yield return null;

            Assert.That(
                UIRuntimeCompositionRoot
                    .Instance.Settings,
                Is.Not.Null);
        }

        [UnityTest]
        public IEnumerator RuntimeRoot_HasAccessibility()
        {
            yield return null;

            Assert.That(
                UIRuntimeCompositionRoot
                    .Instance.Accessibility,
                Is.Not.Null);
        }

        [UnityTest]
        public IEnumerator RuntimeRoot_HasTutorialService()
        {
            yield return null;

            Assert.That(
                UIRuntimeCompositionRoot
                    .Instance.Tutorial,
                Is.Not.Null);
        }

        [UnityTest]
        public IEnumerator RuntimeRoot_HasSlotService()
        {
            yield return null;

            Assert.That(
                UIRuntimeCompositionRoot
                    .Instance.Slots,
                Is.Not.Null);
        }

        [UnityTest]
        public IEnumerator RuntimeRoot_HasAutosaveService()
        {
            yield return null;

            Assert.That(
                UIRuntimeCompositionRoot
                    .Instance.Autosave,
                Is.Not.Null);
        }

        [UnityTest]
        public IEnumerator RuntimeRoot_HasProjectionService()
        {
            yield return null;

            Assert.That(
                UIRuntimeCompositionRoot
                    .Instance.Projection,
                Is.Not.Null);
        }

        [UnityTest]
        public IEnumerator RuntimeRoot_HasActiveSessionService()
        {
            yield return null;

            Assert.That(
                UIRuntimeCompositionRoot
                    .Instance.ActiveSession,
                Is.Not.Null);
        }

        [UnityTest]
        public IEnumerator RuntimeBridge_DetectsRuntime()
        {
            yield return null;

            GameObject gameObject =
                new GameObject("RuntimeBridgeTest");

            try
            {
                StoreUiStateBridge bridge =
                    gameObject.AddComponent<
                        StoreUiStateBridge>();

                Assert.That(
                    bridge.HasRuntime,
                    Is.True);
            }
            finally
            {
                Object.Destroy(gameObject);
            }
        }

        [UnityTest]
        public IEnumerator InputGate_EntersAndExits()
        {
            yield return null;

            UiInputContextGate gate =
                new UiInputContextGate();

            gate.EnterUiExclusive();
            Assert.That(
                gate.IsUiExclusive,
                Is.True);

            gate.ExitUiExclusive();
            Assert.That(
                gate.IsUiExclusive,
                Is.False);
        }

        [UnityTest]
        public IEnumerator AutosaveWithoutSession_FailsSafely()
        {
            yield return null;

            UIRuntimeCompositionRoot root =
                UIRuntimeCompositionRoot.Instance;
            root.ActiveSession.Clear();

            DailyAutosaveResult result =
                root.Autosave.TryAutosave();

            Assert.That(
                result.Status,
                Is.EqualTo(
                    DailyAutosaveStatus.Failed));
        }
    }
}
