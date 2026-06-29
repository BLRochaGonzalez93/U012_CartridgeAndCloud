using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using VRMGames.CartridgeAndCloud.Application.UIUX;
using VRMGames.CartridgeAndCloud.Infrastructure.UIUX;

namespace VRMGames.CartridgeAndCloud.Tests.PlayMode.UIUX
{
    public sealed class Sprint15RuntimePlayModeTests
    {
        [UnityTest]
        public IEnumerator RuntimeRoot_IsInstalled()
        {
            yield return null;

            Assert.That(
                Sprint15RuntimeCompositionRoot.Instance,
                Is.Not.Null);
        }

        [UnityTest]
        public IEnumerator RuntimeRoot_HasSettings()
        {
            yield return null;

            Assert.That(
                Sprint15RuntimeCompositionRoot
                    .Instance.Settings,
                Is.Not.Null);
        }

        [UnityTest]
        public IEnumerator RuntimeRoot_HasAccessibility()
        {
            yield return null;

            Assert.That(
                Sprint15RuntimeCompositionRoot
                    .Instance.Accessibility,
                Is.Not.Null);
        }

        [UnityTest]
        public IEnumerator RuntimeRoot_HasTutorialService()
        {
            yield return null;

            Assert.That(
                Sprint15RuntimeCompositionRoot
                    .Instance.Tutorial,
                Is.Not.Null);
        }

        [UnityTest]
        public IEnumerator RuntimeRoot_HasSlotService()
        {
            yield return null;

            Assert.That(
                Sprint15RuntimeCompositionRoot
                    .Instance.Slots,
                Is.Not.Null);
        }

        [UnityTest]
        public IEnumerator RuntimeRoot_HasAutosaveService()
        {
            yield return null;

            Assert.That(
                Sprint15RuntimeCompositionRoot
                    .Instance.Autosave,
                Is.Not.Null);
        }

        [UnityTest]
        public IEnumerator RuntimeRoot_HasProjectionService()
        {
            yield return null;

            Assert.That(
                Sprint15RuntimeCompositionRoot
                    .Instance.Projection,
                Is.Not.Null);
        }

        [UnityTest]
        public IEnumerator RuntimeRoot_HasActiveSessionService()
        {
            yield return null;

            Assert.That(
                Sprint15RuntimeCompositionRoot
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
                Sprint15RuntimeStateBridge bridge =
                    gameObject.AddComponent<
                        Sprint15RuntimeStateBridge>();

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

            Sprint15InputMapGate gate =
                new Sprint15InputMapGate();

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
        public IEnumerator TechnicalRunner_Completes()
        {
            yield return null;

            GameObject gameObject =
                new GameObject(
                    "Sprint15TechnicalRunner");
            Sprint15SettingsAsset settings =
                ScriptableObject.CreateInstance<
                    Sprint15SettingsAsset>();

            try
            {
                Sprint15TechnicalScenarioRunner runner =
                    gameObject.AddComponent<
                        Sprint15TechnicalScenarioRunner>();
                runner.Configure(settings, false);
                runner.RunScenario();

                Assert.That(
                    runner.LastScenarioPassed,
                    Is.True);
            }
            finally
            {
                Object.Destroy(gameObject);
                Object.Destroy(settings);
            }

            yield return null;
        }

        [UnityTest]
        public IEnumerator AutosaveWithoutSession_FailsSafely()
        {
            yield return null;

            Sprint15RuntimeCompositionRoot root =
                Sprint15RuntimeCompositionRoot.Instance;
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
