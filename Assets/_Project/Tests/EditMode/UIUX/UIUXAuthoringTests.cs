using NUnit.Framework;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Infrastructure.UIUX;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.UIUX
{
    public sealed class UIUXAuthoringTests
    {
        [Test] public void SettingsAsset_HasValidDefaults()
        {
            Sprint15SettingsAsset asset =
                ScriptableObject.CreateInstance<
                    Sprint15SettingsAsset>();

            try
            {
                Assert.That(
                    asset.MainMenuSceneName,
                    Is.EqualTo("MainMenu"));
                Assert.That(
                    asset.StoreSceneName,
                    Is.EqualTo("Store"));
                Assert.That(
                    asset.CurrencyCode,
                    Is.EqualTo("EUR"));
                Assert.That(
                    asset.DayDurationSeconds,
                    Is.GreaterThan(0));
            }
            finally
            {
                Object.DestroyImmediate(asset);
            }
        }

        [Test] public void SettingsAsset_StoresConfiguration()
        {
            Sprint15SettingsAsset asset =
                ScriptableObject.CreateInstance<
                    Sprint15SettingsAsset>();

            try
            {
                asset.Configure(
                    "Menu",
                    "Shop",
                    "USD",
                    5000,
                    120,
                    false);

                Assert.That(
                    asset.MainMenuSceneName,
                    Is.EqualTo("Menu"));
                Assert.That(
                    asset.StoreSceneName,
                    Is.EqualTo("Shop"));
                Assert.That(
                    asset.CurrencyCode,
                    Is.EqualTo("USD"));
                Assert.That(
                    asset.InitialCashCents,
                    Is.EqualTo(5000));
                Assert.That(
                    asset.DayDurationSeconds,
                    Is.EqualTo(120));
                Assert.That(
                    asset.ShowTutorialOnNewGame,
                    Is.False);
            }
            finally
            {
                Object.DestroyImmediate(asset);
            }
        }

        [Test] public void TechnicalRunner_RequiresSettings()
        {
            GameObject gameObject =
                new GameObject(
                    "Sprint15RunnerTest");

            try
            {
                Sprint15TechnicalScenarioRunner
                    runner =
                        gameObject.AddComponent<
                            Sprint15TechnicalScenarioRunner>();

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
            GameObject gameObject =
                new GameObject(
                    "Sprint15RunnerTest");
            Sprint15SettingsAsset asset =
                ScriptableObject.CreateInstance<
                    Sprint15SettingsAsset>();

            try
            {
                Sprint15TechnicalScenarioRunner
                    runner =
                        gameObject.AddComponent<
                            Sprint15TechnicalScenarioRunner>();
                runner.Configure(asset, false);
                runner.RunScenario();

                Assert.That(
                    runner.LastScenarioPassed,
                    Is.True);
                Assert.That(
                    runner.LastThreeSlotsVisible,
                    Is.True);
                Assert.That(
                    runner.LastAutosaveIdempotent,
                    Is.True);
                Assert.That(
                    runner.LastAllPanelsProjected,
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
