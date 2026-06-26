using NUnit.Framework;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.Checkout;
using VRMGames.CartridgeAndCloud.Infrastructure.Checkout;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Checkout
{
    public sealed class CheckoutAuthoringTests
    {
        [Test] public void SettingsAsset_BuildsPolicy()
        {
            CheckoutSettingsAsset asset =
                ScriptableObject.CreateInstance<
                    CheckoutSettingsAsset>();
            try
            {
                asset.Configure(4, "station", true);
                Assert.That(
                    asset.BuildPolicy().MaxQueueLength,
                    Is.EqualTo(4));
            }
            finally
            {
                Object.DestroyImmediate(asset);
            }
        }

        [Test] public void SettingsAsset_BuildsOpenStation()
        {
            CheckoutSettingsAsset asset =
                ScriptableObject.CreateInstance<
                    CheckoutSettingsAsset>();
            try
            {
                asset.Configure(4, "station", true);
                CheckoutStation station =
                    asset.BuildStation();
                Assert.That(
                    station.Id.Value,
                    Is.EqualTo("station"));
                Assert.That(
                    station.State,
                    Is.EqualTo(
                        CheckoutStationState.Available));
            }
            finally
            {
                Object.DestroyImmediate(asset);
            }
        }

        [Test] public void StationAuthoring_RequiresSettings()
        {
            GameObject gameObject =
                new GameObject("CheckoutStationTest");
            try
            {
                CheckoutStationAuthoring authoring =
                    gameObject.AddComponent<
                        CheckoutStationAuthoring>();
                Assert.Throws<
                    System.InvalidOperationException>(
                    () => authoring.Initialize());
            }
            finally
            {
                Object.DestroyImmediate(gameObject);
            }
        }

        [Test] public void StationAuthoring_InitializesStation()
        {
            CheckoutSettingsAsset asset =
                ScriptableObject.CreateInstance<
                    CheckoutSettingsAsset>();
            GameObject gameObject =
                new GameObject("CheckoutStationTest");
            try
            {
                asset.Configure(4, "station", true);
                CheckoutStationAuthoring authoring =
                    gameObject.AddComponent<
                        CheckoutStationAuthoring>();
                authoring.Configure(asset, false);
                authoring.Initialize();
                Assert.That(
                    authoring.IsInitialized,
                    Is.True);
                Assert.That(
                    authoring.Station.State,
                    Is.EqualTo(
                        CheckoutStationState.Available));
            }
            finally
            {
                Object.DestroyImmediate(gameObject);
                Object.DestroyImmediate(asset);
            }
        }

        [Test] public void TechnicalRunner_RequiresSettings()
        {
            GameObject gameObject =
                new GameObject("CheckoutRunnerTest");
            try
            {
                CheckoutTechnicalScenarioRunner runner =
                    gameObject.AddComponent<
                        CheckoutTechnicalScenarioRunner>();
                Assert.Throws<
                    System.InvalidOperationException>(
                    () => runner.RunScenario());
            }
            finally
            {
                Object.DestroyImmediate(gameObject);
            }
        }

        [Test] public void TechnicalRunner_CompletesScenario()
        {
            CheckoutSettingsAsset asset =
                ScriptableObject.CreateInstance<
                    CheckoutSettingsAsset>();
            GameObject gameObject =
                new GameObject("CheckoutRunnerTest");
            try
            {
                asset.Configure(
                    6,
                    "technical-station",
                    true);
                CheckoutTechnicalScenarioRunner runner =
                    gameObject.AddComponent<
                        CheckoutTechnicalScenarioRunner>();
                runner.Configure(asset, false);
                runner.RunScenario();

                Assert.That(
                    runner.LastScenarioPassed,
                    Is.True);
                Assert.That(
                    runner.LastQueueBefore,
                    Is.EqualTo(1));
                Assert.That(
                    runner.LastQueueAfter,
                    Is.EqualTo(0));
                Assert.That(
                    runner.LastStockBefore,
                    Is.EqualTo(3));
                Assert.That(
                    runner.LastStockAfter,
                    Is.EqualTo(1));
                Assert.That(
                    runner.LastConsumedReservations,
                    Is.EqualTo(2));
                Assert.That(
                    runner.LastDoubleCheckoutBlocked,
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
