using NUnit.Framework;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Infrastructure.Persistence;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Persistence
{
    public sealed class SavePersistenceAuthoringTests
    {
        [Test] public void SettingsAsset_DefaultSlotIsValid()
        {
            SaveIntegrationSettingsAsset asset =
                ScriptableObject.CreateInstance<
                    SaveIntegrationSettingsAsset>();

            try
            {
                Assert.That(
                    asset.TechnicalSlot.Value,
                    Is.EqualTo(0));
                Assert.That(
                    asset.TechnicalDirectoryName,
                    Is.Not.Empty);
            }
            finally
            {
                Object.DestroyImmediate(asset);
            }
        }

        [Test] public void SettingsAsset_StoresConfiguration()
        {
            SaveIntegrationSettingsAsset asset =
                ScriptableObject.CreateInstance<
                    SaveIntegrationSettingsAsset>();

            try
            {
                asset.Configure(
                    "TechnicalSaveTest",
                    2);

                Assert.That(
                    asset.TechnicalDirectoryName,
                    Is.EqualTo("TechnicalSaveTest"));
                Assert.That(
                    asset.TechnicalSlot.Value,
                    Is.EqualTo(2));
            }
            finally
            {
                Object.DestroyImmediate(asset);
            }
        }

        [Test] public void SettingsAsset_InvalidSlotIsRejectedOnRead()
        {
            SaveIntegrationSettingsAsset asset =
                ScriptableObject.CreateInstance<
                    SaveIntegrationSettingsAsset>();

            try
            {
                asset.Configure(
                    "TechnicalSaveTest",
                    3);

                Assert.Throws<
                    System.ArgumentOutOfRangeException>(
                    () =>
                    {
                        var unused =
                            asset.TechnicalSlot;
                    });
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
                    "SaveRecoveryRunnerTest");

            try
            {
                SaveRecoveryTechnicalScenarioRunner
                    runner =
                        gameObject.AddComponent<
                            SaveRecoveryTechnicalScenarioRunner>();

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
            SaveIntegrationSettingsAsset asset =
                ScriptableObject.CreateInstance<
                    SaveIntegrationSettingsAsset>();
            GameObject gameObject =
                new GameObject(
                    "SaveRecoveryRunnerTest");

            try
            {
                asset.Configure(
                    "CC_S14_AuthoringTest",
                    0);
                SaveRecoveryTechnicalScenarioRunner
                    runner =
                        gameObject.AddComponent<
                            SaveRecoveryTechnicalScenarioRunner>();
                runner.Configure(asset, false);
                runner.RunScenario();

                Assert.That(
                    runner.LastScenarioPassed,
                    Is.True);
                Assert.That(
                    runner.LastFirstSaveSucceeded,
                    Is.True);
                Assert.That(
                    runner.LastSecondSaveSucceeded,
                    Is.True);
                Assert.That(
                    runner.LastBackupCreated,
                    Is.True);
                Assert.That(
                    runner.LastRecoverySucceeded,
                    Is.True);
                Assert.That(
                    runner.LastRecoveredCashCents,
                    Is.EqualTo(1000));
                Assert.That(
                    runner.LastRecoveredRecordCount,
                    Is.EqualTo(12));
                Assert.That(
                    runner.LastPrimaryRepaired,
                    Is.True);
                Assert.That(
                    runner.LastCorruptionRejected,
                    Is.True);
                Assert.That(
                    runner.LastLegacyRecoverySucceeded,
                    Is.True);
                Assert.That(
                    runner.LastTemporaryFilesClean,
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
