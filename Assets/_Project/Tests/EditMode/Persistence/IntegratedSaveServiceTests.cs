using System;
using System.IO;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.Persistence;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;
using VRMGames.CartridgeAndCloud.Domain.Persistence;
using VRMGames.CartridgeAndCloud.Infrastructure.Persistence;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Persistence
{
    public sealed class IntegratedSaveServiceTests
    {
        private static void WithService(
            Action<
                IntegratedSaveService,
                JsonIntegratedSaveRepository,
                PersistenceTestFactory.FixedClock>
                    action)
        {
            string root =
                PersistenceTestFactory
                    .CreateTempDirectory();

            try
            {
                JsonIntegratedSaveRepository repository =
                    new JsonIntegratedSaveRepository(root);
                PersistenceTestFactory.FixedClock clock =
                    new PersistenceTestFactory.FixedClock(
                        PersistenceTestFactory.UpdatedUtc);
                IntegratedSaveService service =
                    new IntegratedSaveService(
                        repository,
                        clock);

                action(
                    service,
                    repository,
                    clock);
            }
            finally
            {
                PersistenceTestFactory
                    .DeleteDirectory(root);
            }
        }

        [Test] public void Service_SaveSucceeds()
        {
            WithService(
                (service, repository, _) =>
                {
                    SaveSlotId slot =
                        PersistenceTestFactory.Slot();
                    var source =
                        new PersistenceTestFactory
                            .CaptureSource(
                                (capturedSlot, updated) =>
                                    PersistenceTestFactory
                                        .ClosedSnapshot(
                                            capturedSlot,
                                            updatedUtc:
                                                updated));

                    IntegratedSaveOperationResult result =
                        service.Save(slot, source);

                    Assert.That(result.Succeeded, Is.True);
                    Assert.That(
                        repository.Exists(slot),
                        Is.True);
                });
        }

        [Test] public void Service_SaveUsesClockTime()
        {
            WithService(
                (service, repository, clock) =>
                {
                    SaveSlotId slot =
                        PersistenceTestFactory.Slot();
                    clock.UtcNow =
                        PersistenceTestFactory
                            .UpdatedUtc
                            .AddHours(2);
                    var source =
                        new PersistenceTestFactory
                            .CaptureSource(
                                (capturedSlot, updated) =>
                                    PersistenceTestFactory
                                        .ClosedSnapshot(
                                            capturedSlot,
                                            updatedUtc:
                                                updated));

                    service.Save(slot, source);
                    repository.Load(
                        slot,
                        out IntegratedGameStateSnapshot
                            loaded);

                    Assert.That(
                        loaded.UpdatedUtc,
                        Is.EqualTo(clock.UtcNow));
                });
        }

        [Test] public void Service_CaptureExceptionReturnsFailure()
        {
            WithService(
                (service, repository, _) =>
                {
                    SaveSlotId slot =
                        PersistenceTestFactory.Slot();
                    var source =
                        new PersistenceTestFactory
                            .CaptureSource(
                                (_, __) =>
                                    throw new
                                        InvalidOperationException(
                                            "capture failed"));

                    IntegratedSaveOperationResult result =
                        service.Save(slot, source);

                    Assert.That(
                        result.Status,
                        Is.EqualTo(
                            IntegratedSaveOperationStatus
                                .CaptureFailure));
                    Assert.That(
                        repository.Exists(slot),
                        Is.False);
                });
        }

        [Test] public void Service_WrongCapturedSlotReturnsFailure()
        {
            WithService(
                (service, repository, _) =>
                {
                    SaveSlotId requested =
                        PersistenceTestFactory.Slot(0);
                    var source =
                        new PersistenceTestFactory
                            .CaptureSource(
                                (_, updated) =>
                                    PersistenceTestFactory
                                        .ClosedSnapshot(
                                            PersistenceTestFactory
                                                .Slot(1),
                                            updatedUtc:
                                                updated));

                    IntegratedSaveOperationResult result =
                        service.Save(
                            requested,
                            source);

                    Assert.That(
                        result.Status,
                        Is.EqualTo(
                            IntegratedSaveOperationStatus
                                .CaptureFailure));
                    Assert.That(
                        repository.Exists(requested),
                        Is.False);
                });
        }

        [Test] public void Service_LoadAppliesOnce()
        {
            WithService(
                (service, repository, _) =>
                {
                    SaveSlotId slot =
                        PersistenceTestFactory.Slot();
                    repository.Save(
                        PersistenceTestFactory
                            .ClosedSnapshot(slot));
                    InMemoryRestoreTarget target =
                        new InMemoryRestoreTarget();

                    IntegratedSaveOperationResult result =
                        service.Load(slot, target);

                    Assert.That(result.Succeeded, Is.True);
                    Assert.That(
                        target.RestoreCount,
                        Is.EqualTo(1));
                    Assert.That(
                        target.Current,
                        Is.Not.Null);
                });
        }

        [Test] public void Service_LoadReportsRecoveredBackup()
        {
            WithService(
                (service, repository, _) =>
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
                    File.WriteAllText(
                        repository.GetPrimaryPath(slot),
                        "{corrupt");
                    InMemoryRestoreTarget target =
                        new InMemoryRestoreTarget();

                    IntegratedSaveOperationResult result =
                        service.Load(slot, target);

                    Assert.That(
                        result.RecoveredFromBackup,
                        Is.True);
                    Assert.That(
                        target.Current.CashCents,
                        Is.EqualTo(100));
                });
        }

        [Test] public void Service_RejectedRestoreDoesNotApply()
        {
            WithService(
                (service, repository, _) =>
                {
                    SaveSlotId slot =
                        PersistenceTestFactory.Slot();
                    repository.Save(
                        PersistenceTestFactory
                            .ClosedSnapshot(slot));
                    InMemoryRestoreTarget target =
                        new InMemoryRestoreTarget(
                            _ => false);

                    IntegratedSaveOperationResult result =
                        service.Load(slot, target);

                    Assert.That(
                        result.Status,
                        Is.EqualTo(
                            IntegratedSaveOperationStatus
                                .RestoreRejected));
                    Assert.That(
                        target.RestoreCount,
                        Is.EqualTo(0));
                });
        }

        [Test] public void Service_EmptySlotReturnsSlotEmpty()
        {
            WithService(
                (service, _, __) =>
                {
                    IntegratedSaveOperationResult result =
                        service.Load(
                            PersistenceTestFactory.Slot(),
                            new InMemoryRestoreTarget());

                    Assert.That(
                        result.Status,
                        Is.EqualTo(
                            IntegratedSaveOperationStatus
                                .SlotEmpty));
                });
        }

        [Test] public void Service_DeleteAndExistsWork()
        {
            WithService(
                (service, repository, _) =>
                {
                    SaveSlotId slot =
                        PersistenceTestFactory.Slot();
                    repository.Save(
                        PersistenceTestFactory
                            .ClosedSnapshot(slot));

                    Assert.That(
                        service.SlotExists(slot),
                        Is.True);
                    Assert.That(
                        service.Delete(slot),
                        Is.True);
                    Assert.That(
                        service.SlotExists(slot),
                        Is.False);
                });
        }

        [Test] public void Service_RestoreExceptionReturnsFailure()
        {
            WithService(
                (service, repository, _) =>
                {
                    SaveSlotId slot =
                        PersistenceTestFactory.Slot();
                    repository.Save(
                        PersistenceTestFactory
                            .ClosedSnapshot(slot));

                    IntegratedSaveOperationResult result =
                        service.Load(
                            slot,
                            new ThrowingRestoreTarget());

                    Assert.That(
                        result.Status,
                        Is.EqualTo(
                            IntegratedSaveOperationStatus
                                .RestoreFailure));
                });
        }

        private sealed class ThrowingRestoreTarget :
            IIntegratedGameStateRestoreTarget
        {
            public bool CanRestore(
                IntegratedGameStateSnapshot snapshot,
                out string reason)
            {
                reason = string.Empty;
                return true;
            }

            public void Restore(
                IntegratedGameStateSnapshot snapshot)
            {
                throw new InvalidOperationException(
                    "restore failed");
            }
        }
    }
}
