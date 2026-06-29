using System;
using System.IO;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.Persistence;
using VRMGames.CartridgeAndCloud.Domain.GameSession;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;
using VRMGames.CartridgeAndCloud.Domain.Persistence;
using VRMGames.CartridgeAndCloud.Infrastructure.GameSession;
using VRMGames.CartridgeAndCloud.Infrastructure.Persistence;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Persistence
{
    public sealed class SaveRecoveryTests
    {
        private static void WithIntegrated(
            Action<
                JsonIntegratedSaveRepository,
                SaveSlotId> action)
        {
            string root =
                PersistenceTestFactory
                    .CreateTempDirectory();

            try
            {
                JsonIntegratedSaveRepository repository =
                    new JsonIntegratedSaveRepository(root);
                SaveSlotId slot =
                    PersistenceTestFactory.Slot();

                repository.Save(
                    PersistenceTestFactory
                        .ClosedSnapshot(
                            slot,
                            cashCents: 100));
                repository.Save(
                    PersistenceTestFactory
                        .ClosedSnapshot(
                            slot,
                            cashCents: 200,
                            updatedUtc:
                                PersistenceTestFactory
                                    .UpdatedUtc
                                    .AddMinutes(1)));

                action(repository, slot);
            }
            finally
            {
                PersistenceTestFactory
                    .DeleteDirectory(root);
            }
        }

        private static GameSessionSnapshot
            LegacySnapshot(
                SaveSlotId slot,
                int minute,
                long cash)
        {
            DateTime created =
                PersistenceTestFactory.CreatedUtc;

            return new GameSessionSnapshot(
                GameSessionSnapshot.CurrentSchemaVersion,
                StableId.Parse(
                    "44444444444444444444444444444444"),
                slot,
                created,
                created.AddMinutes(minute),
                2,
                cash);
        }

        [Test] public void CorruptPrimary_RecoversBackup()
        {
            WithIntegrated(
                (repository, slot) =>
                {
                    File.WriteAllText(
                        repository.GetPrimaryPath(slot),
                        "{corrupt");

                    IntegratedSaveRepositoryResult result =
                        repository.Load(
                            slot,
                            out IntegratedGameStateSnapshot
                                snapshot);

                    Assert.That(
                        result.RecoveredFromBackup,
                        Is.True);
                    Assert.That(
                        snapshot.CashCents,
                        Is.EqualTo(100));
                });
        }

        [Test] public void MissingPrimary_RecoversBackup()
        {
            WithIntegrated(
                (repository, slot) =>
                {
                    File.Delete(
                        repository.GetPrimaryPath(slot));

                    IntegratedSaveRepositoryResult result =
                        repository.Load(
                            slot,
                            out IntegratedGameStateSnapshot
                                snapshot);

                    Assert.That(
                        result.RecoveredFromBackup,
                        Is.True);
                    Assert.That(
                        snapshot.CashCents,
                        Is.EqualTo(100));
                });
        }

        [Test] public void Recovery_RepairsPrimary()
        {
            WithIntegrated(
                (repository, slot) =>
                {
                    File.WriteAllText(
                        repository.GetPrimaryPath(slot),
                        "{corrupt");

                    repository.Load(
                        slot,
                        out _);

                    IntegratedSaveRepositoryResult second =
                        repository.Load(
                            slot,
                            out IntegratedGameStateSnapshot
                                snapshot);

                    Assert.That(second.Succeeded, Is.True);
                    Assert.That(
                        second.RecoveredFromBackup,
                        Is.False);
                    Assert.That(
                        snapshot.CashCents,
                        Is.EqualTo(100));
                });
        }

        [Test] public void Recovery_PreservesBackup()
        {
            WithIntegrated(
                (repository, slot) =>
                {
                    File.WriteAllText(
                        repository.GetPrimaryPath(slot),
                        "{corrupt");

                    repository.Load(
                        slot,
                        out _);

                    Assert.That(
                        File.Exists(
                            repository.GetBackupPath(slot)),
                        Is.True);
                });
        }

        [Test] public void Recovery_CleansRecoveryFile()
        {
            WithIntegrated(
                (repository, slot) =>
                {
                    File.WriteAllText(
                        repository.GetPrimaryPath(slot),
                        "{corrupt");

                    repository.Load(
                        slot,
                        out _);

                    Assert.That(
                        File.Exists(
                            repository.GetRecoveryPath(slot)),
                        Is.False);
                });
        }

        [Test] public void BothCorrupt_AreRejected()
        {
            WithIntegrated(
                (repository, slot) =>
                {
                    File.WriteAllText(
                        repository.GetPrimaryPath(slot),
                        "{primary");
                    File.WriteAllText(
                        repository.GetBackupPath(slot),
                        "{backup");

                    IntegratedSaveRepositoryResult result =
                        repository.Load(
                            slot,
                            out IntegratedGameStateSnapshot
                                snapshot);

                    Assert.That(result.Succeeded, Is.False);
                    Assert.That(snapshot, Is.Null);
                    Assert.That(
                        result.Status,
                        Is.EqualTo(
                            IntegratedSaveRepositoryStatus
                                .CorruptPrimaryNoBackup));
                });
        }

        [Test] public void ChecksumMismatch_RecoversBackup()
        {
            WithIntegrated(
                (repository, slot) =>
                {
                    string path =
                        repository.GetPrimaryPath(slot);
                    string json =
                        File.ReadAllText(path);
                    json = System.Text.RegularExpressions
                        .Regex.Replace(
                            json,
                            "\"payloadSha256\"\\s*:\\s*\"[0-9a-f]+\"",
                            "\"payloadSha256\": \"bad\"");
                    File.WriteAllText(path, json);

                    IntegratedSaveRepositoryResult result =
                        repository.Load(
                            slot,
                            out IntegratedGameStateSnapshot
                                snapshot);

                    Assert.That(
                        result.RecoveredFromBackup,
                        Is.True);
                    Assert.That(
                        snapshot.CashCents,
                        Is.EqualTo(100));
                });
        }

        [Test] public void UnsupportedPrimary_UsesValidBackup()
        {
            WithIntegrated(
                (repository, slot) =>
                {
                    string path =
                        repository.GetPrimaryPath(slot);
                    string json =
                        File.ReadAllText(path)
                            .Replace(
                                "\"schemaVersion\": 2",
                                "\"schemaVersion\": 99");
                    File.WriteAllText(path, json);

                    IntegratedSaveRepositoryResult result =
                        repository.Load(
                            slot,
                            out IntegratedGameStateSnapshot
                                snapshot);

                    Assert.That(
                        result.RecoveredFromBackup,
                        Is.True);
                    Assert.That(
                        snapshot.CashCents,
                        Is.EqualTo(100));
                });
        }

        [Test] public void BothUnsupported_ReturnUnsupportedSchema()
        {
            WithIntegrated(
                (repository, slot) =>
                {
                    string primary =
                        File.ReadAllText(
                            repository.GetPrimaryPath(slot))
                            .Replace(
                                "\"schemaVersion\": 2",
                                "\"schemaVersion\": 99");
                    string backup =
                        File.ReadAllText(
                            repository.GetBackupPath(slot))
                            .Replace(
                                "\"schemaVersion\": 2",
                                "\"schemaVersion\": 99");
                    File.WriteAllText(
                        repository.GetPrimaryPath(slot),
                        primary);
                    File.WriteAllText(
                        repository.GetBackupPath(slot),
                        backup);

                    IntegratedSaveRepositoryResult result =
                        repository.Load(
                            slot,
                            out _);

                    Assert.That(
                        result.Status,
                        Is.EqualTo(
                            IntegratedSaveRepositoryStatus
                                .UnsupportedSchema));
                });
        }

        [Test] public void WrongSlotPrimary_RecoversBackup()
        {
            WithIntegrated(
                (repository, slot) =>
                {
                    IntegratedGameStateSnapshot other =
                        PersistenceTestFactory
                            .ClosedSnapshot(
                                PersistenceTestFactory.Slot(1),
                                cashCents: 999);
                    string wrong =
                        new IntegratedSaveJsonCodec()
                            .Encode(other, 5);
                    File.WriteAllText(
                        repository.GetPrimaryPath(slot),
                        wrong);

                    IntegratedSaveRepositoryResult result =
                        repository.Load(
                            slot,
                            out IntegratedGameStateSnapshot
                                snapshot);

                    Assert.That(
                        result.RecoveredFromBackup,
                        Is.True);
                    Assert.That(
                        snapshot.SlotId,
                        Is.EqualTo(slot));
                });
        }

        [Test] public void LegacyCorruptPrimary_RecoversBackup()
        {
            string root =
                PersistenceTestFactory
                    .CreateTempDirectory();

            try
            {
                SaveSlotId slot =
                    PersistenceTestFactory.Slot();
                JsonSaveGameRepository repository =
                    new JsonSaveGameRepository(root);
                repository.Save(
                    LegacySnapshot(slot, 1, 100));
                repository.Save(
                    LegacySnapshot(slot, 2, 200));
                File.WriteAllText(
                    repository.GetSlotPath(slot),
                    "{corrupt");

                bool loaded =
                    repository.TryLoad(
                        slot,
                        out GameSessionSnapshot snapshot);

                Assert.That(loaded, Is.True);
                Assert.That(
                    snapshot.CashCents,
                    Is.EqualTo(100));
            }
            finally
            {
                PersistenceTestFactory
                    .DeleteDirectory(root);
            }
        }

        [Test] public void LegacyMissingPrimary_RecoversBackup()
        {
            string root =
                PersistenceTestFactory
                    .CreateTempDirectory();

            try
            {
                SaveSlotId slot =
                    PersistenceTestFactory.Slot();
                JsonSaveGameRepository repository =
                    new JsonSaveGameRepository(root);
                repository.Save(
                    LegacySnapshot(slot, 1, 100));
                repository.Save(
                    LegacySnapshot(slot, 2, 200));
                File.Delete(
                    repository.GetSlotPath(slot));

                bool loaded =
                    repository.TryLoad(
                        slot,
                        out GameSessionSnapshot snapshot);

                Assert.That(loaded, Is.True);
                Assert.That(
                    snapshot.CashCents,
                    Is.EqualTo(100));
            }
            finally
            {
                PersistenceTestFactory
                    .DeleteDirectory(root);
            }
        }

        [Test] public void LegacyBothCorrupt_Throws()
        {
            string root =
                PersistenceTestFactory
                    .CreateTempDirectory();

            try
            {
                SaveSlotId slot =
                    PersistenceTestFactory.Slot();
                JsonSaveGameRepository repository =
                    new JsonSaveGameRepository(root);
                repository.Save(
                    LegacySnapshot(slot, 1, 100));
                repository.Save(
                    LegacySnapshot(slot, 2, 200));
                File.WriteAllText(
                    repository.GetSlotPath(slot),
                    "{primary");
                File.WriteAllText(
                    repository.GetBackupPath(slot),
                    "{backup");

                Assert.Throws<InvalidDataException>(
                    () => repository.TryLoad(
                        slot,
                        out _));
            }
            finally
            {
                PersistenceTestFactory
                    .DeleteDirectory(root);
            }
        }

        [Test] public void LegacyDelete_RemovesBackup()
        {
            string root =
                PersistenceTestFactory
                    .CreateTempDirectory();

            try
            {
                SaveSlotId slot =
                    PersistenceTestFactory.Slot();
                JsonSaveGameRepository repository =
                    new JsonSaveGameRepository(root);
                repository.Save(
                    LegacySnapshot(slot, 1, 100));
                repository.Save(
                    LegacySnapshot(slot, 2, 200));

                repository.Delete(slot);

                Assert.That(
                    File.Exists(
                        repository.GetSlotPath(slot)),
                    Is.False);
                Assert.That(
                    File.Exists(
                        repository.GetBackupPath(slot)),
                    Is.False);
            }
            finally
            {
                PersistenceTestFactory
                    .DeleteDirectory(root);
            }
        }

        [Test] public void LegacyExists_RecognizesBackupOnly()
        {
            string root =
                PersistenceTestFactory
                    .CreateTempDirectory();

            try
            {
                SaveSlotId slot =
                    PersistenceTestFactory.Slot();
                JsonSaveGameRepository repository =
                    new JsonSaveGameRepository(root);
                repository.Save(
                    LegacySnapshot(slot, 1, 100));
                repository.Save(
                    LegacySnapshot(slot, 2, 200));
                File.Delete(
                    repository.GetSlotPath(slot));

                Assert.That(
                    repository.Exists(slot),
                    Is.True);
            }
            finally
            {
                PersistenceTestFactory
                    .DeleteDirectory(root);
            }
        }
    }
}
