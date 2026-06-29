using NUnit.Framework;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.DayCycle;
using VRMGames.CartridgeAndCloud.Infrastructure.Customers;
using VRMGames.CartridgeAndCloud.Infrastructure.DayCycle;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.DayCycle
{
    public sealed class StoreDayAuthoringTests
    {
        [Test] public void SettingsAsset_BuildsConfiguredDay()
        {
            StoreDaySettingsAsset asset =
                ScriptableObject.CreateInstance<
                    StoreDaySettingsAsset>();

            try
            {
                asset.Configure(
                    "day-test",
                    120,
                    false,
                    false);

                StoreDay day = asset.BuildDay();

                Assert.That(
                    day.Id.Value,
                    Is.EqualTo("day-test"));
                Assert.That(
                    day.Policy.OpenDurationSeconds,
                    Is.EqualTo(120));
                Assert.That(
                    day.Policy.AutoBeginClosing,
                    Is.False);
            }
            finally
            {
                Object.DestroyImmediate(asset);
            }
        }

        [Test] public void SettingsAsset_DefaultPolicyIsValid()
        {
            StoreDaySettingsAsset asset =
                ScriptableObject.CreateInstance<
                    StoreDaySettingsAsset>();

            try
            {
                Assert.DoesNotThrow(
                    () => asset.BuildDay());
            }
            finally
            {
                Object.DestroyImmediate(asset);
            }
        }

        [Test] public void RuntimeController_RequiresSettings()
        {
            GameObject gameObject =
                new GameObject("StoreDayControllerTest");

            try
            {
                StoreDayRuntimeController controller =
                    gameObject.AddComponent<
                        StoreDayRuntimeController>();

                Assert.Throws<
                    System.InvalidOperationException>(
                    () => controller.Initialize());
            }
            finally
            {
                Object.DestroyImmediate(gameObject);
            }
        }

        [Test] public void RuntimeController_AutoOpensConfiguredDay()
        {
            StoreDaySettingsAsset asset =
                ScriptableObject.CreateInstance<
                    StoreDaySettingsAsset>();
            GameObject gameObject =
                new GameObject("StoreDayControllerTest");

            try
            {
                asset.Configure(
                    "day-test",
                    10,
                    true,
                    true);
                StoreDayRuntimeController controller =
                    gameObject.AddComponent<
                        StoreDayRuntimeController>();
                controller.Configure(
                    asset,
                    null,
                    false);
                controller.Initialize();

                Assert.That(
                    controller.Day.State,
                    Is.EqualTo(StoreDayState.Open));
            }
            finally
            {
                Object.DestroyImmediate(gameObject);
                Object.DestroyImmediate(asset);
            }
        }

        [Test] public void RuntimeController_CanRemainBeforeOpen()
        {
            StoreDaySettingsAsset asset =
                ScriptableObject.CreateInstance<
                    StoreDaySettingsAsset>();
            GameObject gameObject =
                new GameObject("StoreDayControllerTest");

            try
            {
                asset.Configure(
                    "day-test",
                    10,
                    false,
                    true);
                StoreDayRuntimeController controller =
                    gameObject.AddComponent<
                        StoreDayRuntimeController>();
                controller.Configure(
                    asset,
                    null,
                    false);
                controller.Initialize();

                Assert.That(
                    controller.Day.State,
                    Is.EqualTo(StoreDayState.BeforeOpen));
                Assert.That(
                    controller.TryOpen(),
                    Is.True);
            }
            finally
            {
                Object.DestroyImmediate(gameObject);
                Object.DestroyImmediate(asset);
            }
        }

        [Test] public void RuntimeController_TickStartsClosing()
        {
            StoreDaySettingsAsset asset =
                ScriptableObject.CreateInstance<
                    StoreDaySettingsAsset>();
            GameObject gameObject =
                new GameObject("StoreDayControllerTest");

            try
            {
                asset.Configure(
                    "day-test",
                    2,
                    true,
                    true);
                StoreDayRuntimeController controller =
                    gameObject.AddComponent<
                        StoreDayRuntimeController>();
                controller.Configure(
                    asset,
                    null,
                    false);
                controller.Initialize();
                controller.Tick(2f);

                Assert.That(
                    controller.Day.State,
                    Is.EqualTo(StoreDayState.Closing));
            }
            finally
            {
                Object.DestroyImmediate(gameObject);
                Object.DestroyImmediate(asset);
            }
        }

        [Test] public void RuntimeController_RejectsNegativeDelta()
        {
            StoreDaySettingsAsset asset =
                ScriptableObject.CreateInstance<
                    StoreDaySettingsAsset>();
            GameObject gameObject =
                new GameObject("StoreDayControllerTest");

            try
            {
                asset.Configure(
                    "day-test",
                    10,
                    true,
                    true);
                StoreDayRuntimeController controller =
                    gameObject.AddComponent<
                        StoreDayRuntimeController>();
                controller.Configure(
                    asset,
                    null,
                    false);
                controller.Initialize();

                Assert.Throws<
                    System.ArgumentOutOfRangeException>(
                    () => controller.Tick(-1f));
            }
            finally
            {
                Object.DestroyImmediate(gameObject);
                Object.DestroyImmediate(asset);
            }
        }

        [Test] public void RuntimeController_ManualCloseWorks()
        {
            StoreDaySettingsAsset asset =
                ScriptableObject.CreateInstance<
                    StoreDaySettingsAsset>();
            GameObject gameObject =
                new GameObject("StoreDayControllerTest");

            try
            {
                asset.Configure(
                    "day-test",
                    10,
                    true,
                    false);
                StoreDayRuntimeController controller =
                    gameObject.AddComponent<
                        StoreDayRuntimeController>();
                controller.Configure(
                    asset,
                    null,
                    false);
                controller.Initialize();

                Assert.That(
                    controller.TryBeginClosing(),
                    Is.True);
                Assert.That(
                    controller.Day.State,
                    Is.EqualTo(StoreDayState.Closing));
            }
            finally
            {
                Object.DestroyImmediate(gameObject);
                Object.DestroyImmediate(asset);
            }
        }

        [Test] public void TechnicalSpawner_GateCanBeDisabled()
        {
            GameObject gameObject =
                new GameObject("CustomerSpawnerTest");

            try
            {
                CustomerTechnicalSpawner spawner =
                    gameObject.AddComponent<
                        CustomerTechnicalSpawner>();
                spawner.SetSpawningEnabled(false);

                Assert.That(
                    spawner.SpawningEnabled,
                    Is.False);
            }
            finally
            {
                Object.DestroyImmediate(gameObject);
            }
        }

        [Test] public void TechnicalSpawner_GateCanBeEnabledAgain()
        {
            GameObject gameObject =
                new GameObject("CustomerSpawnerTest");

            try
            {
                CustomerTechnicalSpawner spawner =
                    gameObject.AddComponent<
                        CustomerTechnicalSpawner>();
                spawner.SetSpawningEnabled(false);
                spawner.SetSpawningEnabled(true);

                Assert.That(
                    spawner.SpawningEnabled,
                    Is.True);
            }
            finally
            {
                Object.DestroyImmediate(gameObject);
            }
        }

        [Test] public void TechnicalRunner_RequiresSettings()
        {
            GameObject gameObject =
                new GameObject("StoreDayRunnerTest");

            try
            {
                StoreDayTechnicalScenarioRunner runner =
                    gameObject.AddComponent<
                        StoreDayTechnicalScenarioRunner>();

                Assert.Throws<
                    System.InvalidOperationException>(
                    () => runner.RunScenario());
            }
            finally
            {
                Object.DestroyImmediate(gameObject);
            }
        }

        [Test] public void TechnicalRunner_CompletesClosureScenario()
        {
            StoreDaySettingsAsset asset =
                ScriptableObject.CreateInstance<
                    StoreDaySettingsAsset>();
            GameObject gameObject =
                new GameObject("StoreDayRunnerTest");

            try
            {
                asset.Configure(
                    "technical-day",
                    300,
                    true,
                    true);
                StoreDayTechnicalScenarioRunner runner =
                    gameObject.AddComponent<
                        StoreDayTechnicalScenarioRunner>();
                runner.Configure(asset, false);
                runner.RunScenario();

                Assert.That(
                    runner.LastScenarioPassed,
                    Is.True);
                Assert.That(
                    runner.LastCustomersBefore,
                    Is.EqualTo(2));
                Assert.That(
                    runner.LastCustomersAfter,
                    Is.EqualTo(0));
                Assert.That(
                    runner.LastQueueBefore,
                    Is.EqualTo(1));
                Assert.That(
                    runner.LastQueueAfter,
                    Is.EqualTo(0));
                Assert.That(
                    runner.LastReservationsBefore,
                    Is.EqualTo(1));
                Assert.That(
                    runner.LastReservationsAfter,
                    Is.EqualTo(0));
                Assert.That(
                    runner.LastDayClosed,
                    Is.True);
                Assert.That(
                    runner.LastSpawnAdmissionBlocked,
                    Is.True);
            }
            finally
            {
                Object.DestroyImmediate(gameObject);
                Object.DestroyImmediate(asset);
            }
        }
    }
}
