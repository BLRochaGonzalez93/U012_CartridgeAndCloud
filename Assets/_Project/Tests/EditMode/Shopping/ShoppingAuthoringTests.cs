using NUnit.Framework;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Infrastructure.Shopping;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Shopping
{
    public sealed class ShoppingAuthoringTests
    {
        [Test] public void SettingsAsset_BuildsPolicy()
        {
            ShoppingSettingsAsset asset =
                ScriptableObject.CreateInstance<ShoppingSettingsAsset>();
            try
            {
                asset.Configure(4, 2, false);
                var policy = asset.BuildPolicy();
                Assert.That(policy.MaxCartUnits, Is.EqualTo(4));
                Assert.That(policy.MaxUnitsPerReservation, Is.EqualTo(2));
                Assert.That(policy.AllowFallbackCategories, Is.False);
            }
            finally
            {
                Object.DestroyImmediate(asset);
            }
        }

        [Test] public void SettingsAsset_DefaultPolicyIsValid()
        {
            ShoppingSettingsAsset asset =
                ScriptableObject.CreateInstance<ShoppingSettingsAsset>();
            try
            {
                Assert.DoesNotThrow(() => asset.BuildPolicy());
            }
            finally
            {
                Object.DestroyImmediate(asset);
            }
        }

        [Test] public void TechnicalRunner_RequiresSettings()
        {
            GameObject gameObject = new GameObject("ShoppingRunnerTest");
            try
            {
                var runner =
                    gameObject.AddComponent<ShoppingTechnicalScenarioRunner>();
                Assert.Throws<System.InvalidOperationException>(
                    () => runner.RunScenario());
            }
            finally
            {
                Object.DestroyImmediate(gameObject);
            }
        }

        [Test] public void TechnicalRunner_CompletesScenario()
        {
            ShoppingSettingsAsset asset =
                ScriptableObject.CreateInstance<ShoppingSettingsAsset>();
            GameObject gameObject = new GameObject("ShoppingRunnerTest");
            try
            {
                asset.Configure(3, 1, true);
                var runner =
                    gameObject.AddComponent<ShoppingTechnicalScenarioRunner>();
                runner.Configure(asset, false);
                runner.RunScenario();
                Assert.That(runner.LastScenarioPassed, Is.True);
                Assert.That(runner.LastOnHand, Is.EqualTo(3));
                Assert.That(runner.LastReserved, Is.EqualTo(1));
                Assert.That(runner.LastAvailable, Is.EqualTo(2));
                Assert.That(runner.LastCartUnits, Is.EqualTo(1));
                Assert.That(runner.LastAvailableAfterRelease, Is.EqualTo(3));
            }
            finally
            {
                Object.DestroyImmediate(gameObject);
                Object.DestroyImmediate(asset);
            }
        }
    }
}
