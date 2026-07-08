using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using NUnit.Framework;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Application.DayCycle;
using VRMGames.CartridgeAndCloud.Application.Persistence;
using VRMGames.CartridgeAndCloud.Application.UIUX;
using VRMGames.CartridgeAndCloud.Domain.DayCycle;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;
using VRMGames.CartridgeAndCloud.Domain.Persistence;
using VRMGames.CartridgeAndCloud.Infrastructure.Persistence;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Persistence
{
    public sealed class ManualSaveCharacterizationTests
    {
        [Test]
        public void CHAR_SAV_001_BeforeOpenCanSaveFromPause()
        {
            WithRepository(
                (repository, root) =>
                {
                    IntegratedGameStateSnapshot snapshot =
                        WithState(
                            PersistenceTestFactory
                                .ClosedSnapshot(),
                            "BeforeOpen");
                    TestContext context =
                        CreateContext(
                            repository,
                            snapshot);

                    ManualSaveResult result =
                        context.Service.Save();

                    Assert.That(result.Succeeded, Is.True);
                    Assert.That(
                        context.Participant.CommitCount,
                        Is.EqualTo(1));
                    Assert.That(
                        repository.Exists(snapshot.SlotId),
                        Is.True);
                });
        }

        [Test]
        public void CHAR_SAV_002_ClosedCanSaveFromPause()
        {
            WithRepository(
                (repository, root) =>
                {
                    IntegratedGameStateSnapshot snapshot =
                        PersistenceTestFactory
                            .ClosedSnapshot();
                    TestContext context =
                        CreateContext(
                            repository,
                            snapshot);

                    ManualSaveEvaluation evaluation =
                        context.Service.Evaluate();
                    ManualSaveResult result =
                        context.Service.Save();

                    Assert.That(
                        evaluation.Allowed,
                        Is.True);
                    Assert.That(result.Succeeded, Is.True);
                });
        }

        [Test]
        public void CHAR_SAV_003_ResultsCanSaveFromPause()
        {
            WithRepository(
                (repository, root) =>
                {
                    IntegratedGameStateSnapshot snapshot =
                        WithState(
                            PersistenceTestFactory
                                .ClosedSnapshot(),
                            "Results");
                    TestContext context =
                        CreateContext(
                            repository,
                            snapshot);

                    Assert.That(
                        context.Service.Save().Succeeded,
                        Is.True);
                });
        }

        [Test]
        public void CHAR_SAV_004_OpenRejectsManualSave()
        {
            WithRepository(
                (repository, root) =>
                {
                    IntegratedGameStateSnapshot snapshot =
                        WithState(
                            PersistenceTestFactory
                                .ClosedSnapshot(),
                            "Open");
                    TestContext context =
                        CreateContext(
                            repository,
                            snapshot);

                    ManualSaveResult result =
                        context.Service.Save();

                    Assert.That(
                        result.Status,
                        Is.EqualTo(
                            ManualSaveStatus.StateNotAllowed));
                    Assert.That(
                        repository.Exists(snapshot.SlotId),
                        Is.False);
                    Assert.That(
                        context.Participant.BeginCount,
                        Is.EqualTo(0));
                });
        }

        [Test]
        public void CHAR_SAV_005_PendingMutationRejectsManualSave()
        {
            WithRepository(
                (repository, root) =>
                {
                    TestContext context =
                        CreateContext(
                            repository,
                            PersistenceTestFactory
                                .ClosedSnapshot());

                    using (context.Mutations.Begin(
                               "DisplayTransfer"))
                    {
                        ManualSaveResult result =
                            context.Service.Save();

                        Assert.That(
                            result.Status,
                            Is.EqualTo(
                                ManualSaveStatus
                                    .MutationPending));
                        Assert.That(
                            result.Detail,
                            Does.Contain(
                                "DisplayTransfer"));
                    }
                });
        }

        [Test]
        public void CHAR_SAV_006_FailedPrimarySaveRollsBackCheckpoint()
        {
            IntegratedGameStateSnapshot snapshot =
                PersistenceTestFactory.ClosedSnapshot();
            FailingSaveRepository repository =
                new FailingSaveRepository();
            TestContext context =
                CreateContext(
                    repository,
                    snapshot);

            ManualSaveResult result =
                context.Service.Save();

            Assert.That(
                result.Status,
                Is.EqualTo(
                    ManualSaveStatus.RepositoryFailure));
            Assert.That(
                context.Participant.CommitCount,
                Is.EqualTo(0));
            Assert.That(
                context.Participant.RollbackCount,
                Is.EqualTo(1));
        }

        [Test]
        public void CHAR_SAV_007_RepeatedLoadDoesNotMultiplyRecords()
        {
            WithRepository(
                (repository, root) =>
                {
                    IntegratedGameStateSnapshot snapshot =
                        PersistenceTestFactory
                            .ClosedSnapshot();
                    TestContext context =
                        CreateContext(
                            repository,
                            snapshot);
                    Assert.That(
                        context.Service.Save().Succeeded,
                        Is.True);

                    IntegratedSaveService loader =
                        new IntegratedSaveService(
                            repository,
                            new PersistenceTestFactory
                                .FixedClock(
                                    PersistenceTestFactory
                                        .UpdatedUtc));
                    InMemoryRestoreTarget target =
                        new InMemoryRestoreTarget();

                    Assert.That(
                        loader.Load(
                            snapshot.SlotId,
                            target).Succeeded,
                        Is.True);
                    Assert.That(
                        loader.Load(
                            snapshot.SlotId,
                            target).Succeeded,
                        Is.True);

                    Assert.That(
                        target.Current.TotalRecordCount,
                        Is.EqualTo(
                            snapshot.TotalRecordCount));
                    Assert.That(
                        target.Current.Inventories.Count,
                        Is.EqualTo(
                            snapshot.Inventories.Count));
                    Assert.That(
                        target.Current.LedgerEntries.Count,
                        Is.EqualTo(
                            snapshot.LedgerEntries.Count));
                });
        }

        [Test]
        public void CHAR_SAV_008_LegacyDayDefaultsToNormalSpeed()
        {
            IntegratedSaveJsonCodec codec =
                new IntegratedSaveJsonCodec();
            IntegratedGameStateSnapshot source =
                PersistenceTestFactory.OpenSnapshot();
            string encoded = codec.Encode(source, 5);
            TestEnvelope envelope =
                JsonUtility.FromJson<TestEnvelope>(
                    encoded);

            envelope.payloadJson =
                Regex.Replace(
                    envelope.payloadJson,
                    ",\"simulationSpeedMultiplier\":" +
                    "(?:1(?:\\.0+)?)",
                    string.Empty);
            envelope.payloadSha256 =
                ComputeSha256(envelope.payloadJson);

            IntegratedGameStateSnapshot restored =
                codec.Decode(
                    JsonUtility.ToJson(
                        envelope,
                        true),
                    source.SlotId,
                    out _);

            Assert.That(
                restored.DayCycle
                    .SimulationSpeedMultiplier,
                Is.EqualTo(
                    SimulationSpeedPolicy.Normal));
        }

        private static TestContext CreateContext(
            IIntegratedSaveRepository repository,
            IntegratedGameStateSnapshot snapshot)
        {
            ActiveGameSessionService active =
                new ActiveGameSessionService();
            active.Activate(
                snapshot.SlotId,
                snapshot);

            PauseService pause =
                new PauseService();
            pause.RequestPause("characterization");

            SaveMutationRegistry mutations =
                new SaveMutationRegistry();
            TestCheckpointParticipant participant =
                new TestCheckpointParticipant();
            ManualSaveService service =
                new ManualSaveService(
                    repository,
                    active,
                    pause,
                    mutations,
                    new PersistenceTestFactory
                        .FixedClock(
                            PersistenceTestFactory
                                .UpdatedUtc
                                .AddMinutes(5)));
            service.RegisterCheckpointParticipant(
                participant);

            return new TestContext(
                service,
                mutations,
                participant);
        }

        private static IntegratedGameStateSnapshot WithState(
            IntegratedGameStateSnapshot source,
            string state)
        {
            return new IntegratedGameStateSnapshot(
                source.SchemaVersion,
                source.SessionId,
                source.SlotId,
                source.CreatedUtc,
                source.UpdatedUtc,
                source.CurrentDay,
                source.CashCents,
                source.CurrencyCode,
                source.Inventories,
                source.SupplierOrders,
                source.Displays,
                source.Customers,
                source.ShoppingSessions,
                source.Reservations,
                source.QueueEntries,
                source.CheckoutStation,
                source.Transactions,
                new DayCycleSaveRecord(
                    source.DayCycle.DayId,
                    state,
                    source.DayCycle
                        .OpenDurationSeconds,
                    source.DayCycle
                        .ElapsedOpenSeconds,
                    source.DayCycle
                        .AutoBeginClosing,
                    source.DayCycle
                        .SimulationSpeedMultiplier),
                source.LedgerEntries);
        }

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
                        Path.Combine(root, "Saves")),
                    root);
            }
            finally
            {
                PersistenceTestFactory
                    .DeleteDirectory(root);
            }
        }

        private static string ComputeSha256(
            string value)
        {
            byte[] bytes =
                Encoding.UTF8.GetBytes(value);

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(bytes);
                StringBuilder builder =
                    new StringBuilder(hash.Length * 2);

                foreach (byte item in hash)
                {
                    builder.Append(item.ToString("x2"));
                }

                return builder.ToString();
            }
        }

        private sealed class TestContext
        {
            public ManualSaveService Service { get; }
            public SaveMutationRegistry Mutations { get; }
            public TestCheckpointParticipant Participant {
                get;
            }

            public TestContext(
                ManualSaveService service,
                SaveMutationRegistry mutations,
                TestCheckpointParticipant participant)
            {
                Service = service;
                Mutations = mutations;
                Participant = participant;
            }
        }

        private sealed class TestCheckpointParticipant :
            IManualSaveCheckpointParticipant
        {
            public int BeginCount { get; private set; }
            public int CommitCount { get; private set; }
            public int RollbackCount { get; private set; }

            public bool CanCheckpoint(out string reason)
            {
                reason = string.Empty;
                return true;
            }

            public IManualSaveCheckpoint BeginCheckpoint()
            {
                BeginCount++;
                return new Checkpoint(this);
            }

            private sealed class Checkpoint :
                IManualSaveCheckpoint
            {
                private TestCheckpointParticipant _owner;
                private bool _committed;

                public Checkpoint(
                    TestCheckpointParticipant owner)
                {
                    _owner = owner;
                }

                public void Commit()
                {
                    if (_owner == null)
                    {
                        throw new ObjectDisposedException(
                            nameof(Checkpoint));
                    }

                    _committed = true;
                    _owner.CommitCount++;
                }

                public void Dispose()
                {
                    TestCheckpointParticipant owner =
                        _owner;
                    if (owner == null)
                    {
                        return;
                    }

                    _owner = null;
                    if (!_committed)
                    {
                        owner.RollbackCount++;
                    }
                }
            }
        }

        private sealed class FailingSaveRepository :
            IIntegratedSaveRepository
        {
            public bool Exists(SaveSlotId slotId)
            {
                return false;
            }

            public IntegratedSaveRepositoryResult Save(
                IntegratedGameStateSnapshot snapshot)
            {
                return IntegratedSaveRepositoryResult
                    .Failure(
                        IntegratedSaveRepositoryStatus
                            .StorageFailure,
                        "Injected storage failure.");
            }

            public IntegratedSaveRepositoryResult Load(
                SaveSlotId slotId,
                out IntegratedGameStateSnapshot snapshot)
            {
                snapshot = null;
                return IntegratedSaveRepositoryResult
                    .Failure(
                        IntegratedSaveRepositoryStatus
                            .SlotEmpty,
                        "Empty.");
            }

            public bool Delete(SaveSlotId slotId)
            {
                return false;
            }
        }

        [Serializable]
        private sealed class TestEnvelope
        {
            public int schemaVersion;
            public string slot;
            public long generation;
            public long updatedUtcTicks;
            public string payloadJson;
            public string payloadSha256;
        }
    }
}
