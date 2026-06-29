using System;
using System.IO;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.Persistence;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;
using VRMGames.CartridgeAndCloud.Domain.Persistence;
using VRMGames.CartridgeAndCloud.Infrastructure.Persistence;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Persistence
{
    public sealed class JsonIntegratedSaveRepositoryTests
    {
        private static void WithRepository(
            Action<
                JsonIntegratedSaveRepository,
                string> action)
        {
            string root =
                PersistenceTestFactory
                    .CreateTempDirectory();

            try
            {
                action(
                    new JsonIntegratedSaveRepository(
                        root),
                    root);
            }
            finally
            {
                PersistenceTestFactory
                    .DeleteDirectory(root);
            }
        }

        [Test] public void Repository_RejectsEmptyRoot()
        {
            Assert.Throws<ArgumentException>(
                () => new JsonIntegratedSaveRepository(
                    " "));
        }

        [Test] public void Repository_StartsWithoutSlot()
        {
            WithRepository(
                (repository, _) =>
                {
                    Assert.That(
                        repository.Exists(
                            PersistenceTestFactory.Slot()),
                        Is.False);
                });
        }

        [Test] public void Save_CreatesPrimary()
        {
            WithRepository(
                (repository, _) =>
                {
                    IntegratedGameStateSnapshot snapshot =
                        PersistenceTestFactory
                            .ClosedSnapshot();
                    IntegratedSaveRepositoryResult result =
                        repository.Save(snapshot);

                    Assert.That(result.Succeeded, Is.True);
                    Assert.That(
                        File.Exists(
                            repository.GetPrimaryPath(
                                snapshot.SlotId)),
                        Is.True);
                });
        }

        [Test] public void FirstSave_DoesNotCreateBackup()
        {
            WithRepository(
                (repository, _) =>
                {
                    IntegratedGameStateSnapshot snapshot =
                        PersistenceTestFactory
                            .ClosedSnapshot();
                    repository.Save(snapshot);

                    Assert.That(
                        File.Exists(
                            repository.GetBackupPath(
                                snapshot.SlotId)),
                        Is.False);
                });
        }

        [Test] public void SecondSave_CreatesBackup()
        {
            WithRepository(
                (repository, _) =>
                {
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

                    Assert.That(
                        File.Exists(
                            repository.GetBackupPath(slot)),
                        Is.True);
                });
        }

        [Test] public void Load_ReturnsLatestPrimary()
        {
            WithRepository(
                (repository, _) =>
                {
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

                    IntegratedSaveRepositoryResult result =
                        repository.Load(
                            slot,
                            out IntegratedGameStateSnapshot
                                loaded);

                    Assert.That(result.Succeeded, Is.True);
                    Assert.That(
                        result.RecoveredFromBackup,
                        Is.False);
                    Assert.That(
                        loaded.CashCents,
                        Is.EqualTo(200));
                });
        }

        [Test] public void Backup_ContainsPreviousGeneration()
        {
            WithRepository(
                (repository, _) =>
                {
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

                    string backupJson =
                        File.ReadAllText(
                            repository.GetBackupPath(slot));
                    IntegratedGameStateSnapshot backup =
                        new IntegratedSaveJsonCodec()
                            .Decode(
                                backupJson,
                                slot,
                                out long generation);

                    Assert.That(
                        backup.CashCents,
                        Is.EqualTo(100));
                    Assert.That(
                        generation,
                        Is.EqualTo(1));
                });
        }

        [Test] public void PrimaryGeneration_Increments()
        {
            WithRepository(
                (repository, _) =>
                {
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

                    string json =
                        File.ReadAllText(
                            repository.GetPrimaryPath(slot));
                    long generation =
                        new IntegratedSaveJsonCodec()
                            .ReadGeneration(
                                json,
                                slot);

                    Assert.That(
                        generation,
                        Is.EqualTo(2));
                });
        }

        [Test] public void Save_CleansTemporaryFile()
        {
            WithRepository(
                (repository, _) =>
                {
                    SaveSlotId slot =
                        PersistenceTestFactory.Slot();
                    repository.Save(
                        PersistenceTestFactory
                            .ClosedSnapshot(slot));

                    Assert.That(
                        File.Exists(
                            repository.GetTemporaryPath(slot)),
                        Is.False);
                });
        }

        [Test] public void Exists_RecognizesPrimary()
        {
            WithRepository(
                (repository, _) =>
                {
                    SaveSlotId slot =
                        PersistenceTestFactory.Slot();
                    repository.Save(
                        PersistenceTestFactory
                            .ClosedSnapshot(slot));

                    Assert.That(
                        repository.Exists(slot),
                        Is.True);
                });
        }

        [Test] public void Exists_RecognizesBackupOnly()
        {
            WithRepository(
                (repository, _) =>
                {
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
                    File.Delete(
                        repository.GetPrimaryPath(slot));

                    Assert.That(
                        repository.Exists(slot),
                        Is.True);
                });
        }

        [Test] public void Load_EmptySlotReturnsSlotEmpty()
        {
            WithRepository(
                (repository, _) =>
                {
                    IntegratedSaveRepositoryResult result =
                        repository.Load(
                            PersistenceTestFactory.Slot(),
                            out IntegratedGameStateSnapshot
                                snapshot);

                    Assert.That(
                        result.Status,
                        Is.EqualTo(
                            IntegratedSaveRepositoryStatus
                                .SlotEmpty));
                    Assert.That(snapshot, Is.Null);
                });
        }

        [Test] public void Delete_RemovesPrimaryAndBackup()
        {
            WithRepository(
                (repository, _) =>
                {
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

                    Assert.That(
                        repository.Delete(slot),
                        Is.True);
                    Assert.That(
                        repository.Exists(slot),
                        Is.False);
                });
        }

        [Test] public void Delete_EmptySlotReturnsFalse()
        {
            WithRepository(
                (repository, _) =>
                {
                    Assert.That(
                        repository.Delete(
                            PersistenceTestFactory.Slot()),
                        Is.False);
                });
        }

        [Test] public void Slots_AreIsolated()
        {
            WithRepository(
                (repository, _) =>
                {
                    SaveSlotId slot0 =
                        PersistenceTestFactory.Slot(0);
                    SaveSlotId slot1 =
                        PersistenceTestFactory.Slot(1);

                    repository.Save(
                        PersistenceTestFactory
                            .ClosedSnapshot(
                                slot0,
                                cashCents: 100));
                    repository.Save(
                        PersistenceTestFactory
                            .ClosedSnapshot(
                                slot1,
                                cashCents: 200));

                    repository.Load(
                        slot0,
                        out IntegratedGameStateSnapshot
                            first);
                    repository.Load(
                        slot1,
                        out IntegratedGameStateSnapshot
                            second);

                    Assert.That(
                        first.CashCents,
                        Is.EqualTo(100));
                    Assert.That(
                        second.CashCents,
                        Is.EqualTo(200));
                });
        }

        [Test] public void Save_ReplacesCorruptPrimary()
        {
            WithRepository(
                (repository, _) =>
                {
                    SaveSlotId slot =
                        PersistenceTestFactory.Slot();
                    repository.Save(
                        PersistenceTestFactory
                            .ClosedSnapshot(
                                slot,
                                cashCents: 100));
                    File.WriteAllText(
                        repository.GetPrimaryPath(slot),
                        "{corrupt");

                    IntegratedSaveRepositoryResult result =
                        repository.Save(
                            PersistenceTestFactory
                                .ClosedSnapshot(
                                    slot,
                                    cashCents: 300,
                                    updatedUtc:
                                        PersistenceTestFactory
                                            .UpdatedUtc
                                            .AddMinutes(2)));

                    repository.Load(
                        slot,
                        out IntegratedGameStateSnapshot
                            loaded);

                    Assert.That(result.Succeeded, Is.True);
                    Assert.That(
                        loaded.CashCents,
                        Is.EqualTo(300));
                });
        }

        [Test] public void Repository_RoundTripsOpenSnapshot()
        {
            WithRepository(
                (repository, _) =>
                {
                    IntegratedGameStateSnapshot source =
                        PersistenceTestFactory
                            .OpenSnapshot();

                    repository.Save(source);
                    repository.Load(
                        source.SlotId,
                        out IntegratedGameStateSnapshot
                            loaded);

                    Assert.That(
                        loaded.QueueEntries.Count,
                        Is.EqualTo(1));
                    Assert.That(
                        loaded.CheckoutStation.State,
                        Is.EqualTo("Busy"));
                });
        }
    }
}
