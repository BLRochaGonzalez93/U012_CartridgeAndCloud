using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Application.InputContexts;
using VRMGames.CartridgeAndCloud.Infrastructure.InputSystem.Actions;

namespace VRMGames.CartridgeAndCloud.InputSystem.Tests.EditMode
{
    public sealed class ProjectInputActionsTests
    {
        [Test]
        public void Actions_DefineExpectedMapsAndActions()
        {
            using ProjectInputActions actions =
                new ProjectInputActions();

            CollectionAssert.AreEquivalent(
                new[]
                {
                    "Point",
                    "Click",
                    "Submit",
                    "Cancel"
                },
                actions.UI.actions
                    .Select(action => action.name)
                    .ToArray());

            CollectionAssert.AreEquivalent(
                new[]
                {
                    "PointerPosition",
                    "SetDestination",
                    "OrbitDelta",
                    "OrbitHold",
                    "Zoom",
                    "TogglePlacementMode",
                    "RotatePlacementCounterClockwise",
                    "RotatePlacementClockwise",
                    "CancelPlacement",
                    "RemovePlacement"
                },
                actions.Gameplay.actions
                    .Select(action => action.name)
                    .ToArray());
        }

        [Test]
        public void Actions_StartWithBothMapsDisabled()
        {
            using ProjectInputActions actions =
                new ProjectInputActions();

            Assert.That(actions.UI.enabled, Is.False);
            Assert.That(actions.Gameplay.enabled, Is.False);
        }

        [Test]
        public void Router_UiContext_EnablesOnlyUiMap()
        {
            TestRouterContext(
                InputContextId.UI,
                expectedUi: true,
                expectedGameplay: false);
        }

        [Test]
        public void Router_GameplayContext_EnablesOnlyGameplayMap()
        {
            TestRouterContext(
                InputContextId.Gameplay,
                expectedUi: false,
                expectedGameplay: true);
        }

        [Test]
        public void Router_NoneContext_DisablesBothMaps()
        {
            TestRouterContext(
                InputContextId.None,
                expectedUi: false,
                expectedGameplay: false);
        }

        [Test]
        public void Router_ContextChange_SwitchesMapsExclusively()
        {
            GameObject routerObject =
                new GameObject("InputRouterTest");

            try
            {
                InputContextService service =
                    new InputContextService();
                InputActionContextRouter router =
                    routerObject.AddComponent<
                        InputActionContextRouter>();
                router.Configure(
                    allowStandaloneGameplay: false);
                router.Initialize(service);

                service.SetContext(InputContextId.UI);
                Assert.That(
                    router.IsUiMapEnabled,
                    Is.True);
                Assert.That(
                    router.IsGameplayMapEnabled,
                    Is.False);

                service.SetContext(
                    InputContextId.Gameplay);
                Assert.That(
                    router.IsUiMapEnabled,
                    Is.False);
                Assert.That(
                    router.IsGameplayMapEnabled,
                    Is.True);
            }
            finally
            {
                Object.DestroyImmediate(routerObject);
            }
        }

        [Test]
        public void ProjectVersion_IsSprintSixteenPhaseOneTarget()
        {
            Assert.That(
                PlayerSettings.bundleVersion,
                Is.EqualTo("0.0.17"));
        }

        private static void TestRouterContext(
            InputContextId context,
            bool expectedUi,
            bool expectedGameplay)
        {
            GameObject routerObject =
                new GameObject("InputRouterTest");

            try
            {
                InputContextService service =
                    new InputContextService();
                service.SetContext(context);

                InputActionContextRouter router =
                    routerObject.AddComponent<
                        InputActionContextRouter>();
                router.Configure(
                    allowStandaloneGameplay: false);
                router.Initialize(service);

                Assert.That(
                    router.IsUiMapEnabled,
                    Is.EqualTo(expectedUi));
                Assert.That(
                    router.IsGameplayMapEnabled,
                    Is.EqualTo(expectedGameplay));
                Assert.That(
                    router.EffectiveContext,
                    Is.EqualTo(context));
            }
            finally
            {
                Object.DestroyImmediate(routerObject);
            }
        }
    }
}
