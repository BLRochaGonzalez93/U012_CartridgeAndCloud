using System;
using System.IO;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Application.GameSession;
using VRMGames.CartridgeAndCloud.Application.Persistence;
using VRMGames.CartridgeAndCloud.Domain.GameSession;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;
using VRMGames.CartridgeAndCloud.Domain.Persistence;
using VRMGames.CartridgeAndCloud.Infrastructure.GameSession;

namespace VRMGames.CartridgeAndCloud.Infrastructure.Persistence
{
    public sealed class SaveRecoveryTechnicalScenarioRunner :
        MonoBehaviour
    {
        [SerializeField]
        private SaveIntegrationSettingsAsset _settings;

        [SerializeField]
        private bool _runOnStart;

        public bool LastScenarioPassed { get; private set; }
        public bool LastFirstSaveSucceeded { get; private set; }
        public bool LastSecondSaveSucceeded { get; private set; }
        public bool LastBackupCreated { get; private set; }
        public bool LastRecoverySucceeded { get; private set; }
        public long LastRecoveredCashCents { get; private set; }
        public int LastRecoveredRecordCount { get; private set; }
        public bool LastPrimaryRepaired { get; private set; }
        public bool LastCorruptionRejected { get; private set; }
        public int LastRestoreCount { get; private set; }
        public bool LastLegacyRecoverySucceeded { get; private set; }
        public bool LastTemporaryFilesClean { get; private set; }

        private void Start()
        {
            if (_runOnStart)
            {
                RunScenario();
            }
        }

        public void Configure(
            SaveIntegrationSettingsAsset settings,
            bool runOnStart)
        {
            _settings = settings;
            _runOnStart = runOnStart;
        }

        [ContextMenu(
            "Run Technical Save Recovery Scenario")]
        public void RunScenario()
        {
            if (_settings == null)
            {
                throw new InvalidOperationException(
                    "Save integration settings asset is required.");
            }

            string rootDirectory =
                Path.Combine(
                    UnityEngine.Application
                        .temporaryCachePath,
                    _settings.TechnicalDirectoryName);

            if (Directory.Exists(rootDirectory))
            {
                Directory.Delete(
                    rootDirectory,
                    true);
            }

            Directory.CreateDirectory(rootDirectory);

            SaveSlotId slot =
                _settings.TechnicalSlot;
            JsonIntegratedSaveRepository repository =
                new JsonIntegratedSaveRepository(
                    Path.Combine(
                        rootDirectory,
                        "integrated"));
            FixedUtcClock clock =
                new FixedUtcClock(
                    new DateTime(
                        2026,
                        6,
                        29,
                        12,
                        0,
                        0,
                        DateTimeKind.Utc));
            IntegratedSaveService service =
                new IntegratedSaveService(
                    repository,
                    clock);

            TechnicalCaptureSource firstSource =
                new TechnicalCaptureSource(
                    1000,
                    1);
            IntegratedSaveOperationResult firstSave =
                service.Save(
                    slot,
                    firstSource);
            LastFirstSaveSucceeded =
                firstSave.Succeeded;

            clock.UtcNow =
                clock.UtcNow.AddMinutes(1);

            TechnicalCaptureSource secondSource =
                new TechnicalCaptureSource(
                    2000,
                    2);
            IntegratedSaveOperationResult secondSave =
                service.Save(
                    slot,
                    secondSource);
            LastSecondSaveSucceeded =
                secondSave.Succeeded;

            string primary =
                repository.GetPrimaryPath(slot);
            string backup =
                repository.GetBackupPath(slot);
            LastBackupCreated =
                File.Exists(backup);

            File.WriteAllText(
                primary,
                "{corrupt-primary");

            InMemoryRestoreTarget target =
                new InMemoryRestoreTarget();
            IntegratedSaveOperationResult recovery =
                service.Load(
                    slot,
                    target);

            LastRecoverySucceeded =
                recovery.Succeeded &&
                recovery.RecoveredFromBackup;
            LastRecoveredCashCents =
                target.Current == null
                    ? 0
                    : target.Current.CashCents;
            LastRecoveredRecordCount =
                target.Current == null
                    ? 0
                    : target.Current.TotalRecordCount;
            LastRestoreCount =
                target.RestoreCount;

            IntegratedSaveRepositoryResult repairedLoad =
                repository.Load(
                    slot,
                    out IntegratedGameStateSnapshot repaired);
            LastPrimaryRepaired =
                repairedLoad.Succeeded &&
                !repairedLoad.RecoveredFromBackup &&
                repaired != null &&
                repaired.CashCents == 1000;

            File.WriteAllText(
                primary,
                "{corrupt-primary");
            File.WriteAllText(
                backup,
                "{corrupt-backup");

            InMemoryRestoreTarget rejectedTarget =
                new InMemoryRestoreTarget();
            IntegratedSaveOperationResult rejected =
                service.Load(
                    slot,
                    rejectedTarget);

            LastCorruptionRejected =
                !rejected.Succeeded &&
                rejectedTarget.RestoreCount == 0;

            LastLegacyRecoverySucceeded =
                RunLegacyRecovery(
                    Path.Combine(
                        rootDirectory,
                        "legacy"),
                    slot);

            LastTemporaryFilesClean =
                !File.Exists(
                    repository.GetTemporaryPath(slot)) &&
                !File.Exists(
                    repository.GetRecoveryPath(slot));

            LastScenarioPassed =
                LastFirstSaveSucceeded &&
                LastSecondSaveSucceeded &&
                LastBackupCreated &&
                LastRecoverySucceeded &&
                LastRecoveredCashCents == 1000 &&
                LastRecoveredRecordCount == 12 &&
                LastPrimaryRepaired &&
                LastCorruptionRejected &&
                LastRestoreCount == 1 &&
                LastLegacyRecoverySucceeded &&
                LastTemporaryFilesClean;

            if (LastScenarioPassed)
            {
                Debug.Log(
                    "Sprint 14 technical save recovery scenario PASS.");
            }
            else
            {
                Debug.LogError(
                    "Sprint 14 technical save recovery scenario FAILED.");
            }
        }

        private static bool RunLegacyRecovery(
            string rootDirectory,
            SaveSlotId slot)
        {
            JsonSaveGameRepository repository =
                new JsonSaveGameRepository(
                    rootDirectory);
            StableId sessionId =
                StableId.Parse(
                    "11111111111111111111111111111111");
            DateTime created =
                new DateTime(
                    2026,
                    6,
                    29,
                    10,
                    0,
                    0,
                    DateTimeKind.Utc);

            repository.Save(
                new GameSessionSnapshot(
                    GameSessionSnapshot
                        .CurrentSchemaVersion,
                    sessionId,
                    slot,
                    created,
                    created.AddMinutes(1),
                    1,
                    100));

            repository.Save(
                new GameSessionSnapshot(
                    GameSessionSnapshot
                        .CurrentSchemaVersion,
                    sessionId,
                    slot,
                    created,
                    created.AddMinutes(2),
                    1,
                    200));

            File.WriteAllText(
                repository.GetSlotPath(slot),
                "{corrupt-primary");

            bool loaded =
                repository.TryLoad(
                    slot,
                    out GameSessionSnapshot snapshot);

            return loaded &&
                snapshot != null &&
                snapshot.CashCents == 100 &&
                File.Exists(
                    repository.GetSlotPath(slot)) &&
                File.Exists(
                    repository.GetBackupPath(slot));
        }

        private sealed class FixedUtcClock :
            IUtcClock
        {
            public DateTime UtcNow { get; set; }

            public FixedUtcClock(DateTime utcNow)
            {
                UtcNow = utcNow;
            }
        }

        private sealed class TechnicalCaptureSource :
            IIntegratedGameStateCaptureSource
        {
            private readonly long _cashCents;
            private readonly int _generationMarker;

            public TechnicalCaptureSource(
                long cashCents,
                int generationMarker)
            {
                _cashCents = cashCents;
                _generationMarker =
                    generationMarker;
            }

            public IntegratedGameStateSnapshot Capture(
                SaveSlotId slotId,
                DateTime updatedUtc)
            {
                DateTime createdUtc =
                    new DateTime(
                        2026,
                        6,
                        29,
                        9,
                        0,
                        0,
                        DateTimeKind.Utc);

                return new IntegratedGameStateSnapshot(
                    IntegratedGameStateSnapshot
                        .CurrentSchemaVersion,
                    StableId.Parse(
                        "22222222222222222222222222222222"),
                    slotId,
                    createdUtc,
                    updatedUtc,
                    3,
                    _cashCents,
                    "EUR",
                    new[]
                    {
                        new InventoryContainerSaveRecord(
                            "store-inventory",
                            20,
                            new[]
                            {
                                new ProductQuantitySaveRecord(
                                    "cartridge-pixel-quest",
                                    5 + _generationMarker)
                            }),
                        new InventoryContainerSaveRecord(
                            "backroom-inventory",
                            30,
                            new ProductQuantitySaveRecord[0])
                    },
                    new[]
                    {
                        new SupplierOrderSaveRecord(
                            "order-001",
                            "supplier-retro",
                            "cartridge-pixel-quest",
                            "Received",
                            3,
                            3,
                            1200)
                    },
                    new[]
                    {
                        new DisplaySaveRecord(
                            "display-001",
                            "display-shelf",
                            "cartridge-pixel-quest",
                            "store-inventory")
                    },
                    new[]
                    {
                        new CustomerSaveRecord(
                            "customer-001",
                            "profile-collector",
                            "Despawned",
                            0,
                            2)
                    },
                    new[]
                    {
                        new ShoppingSessionSaveRecord(
                            "customer-001",
                            "intent-001",
                            "cart-001",
                            "CheckedOut",
                            3)
                    },
                    new[]
                    {
                        new ReservationSaveRecord(
                            "reservation-001",
                            "customer-001",
                            "cart-001",
                            "display-001",
                            "cartridge-pixel-quest",
                            1,
                            "Consumed")
                    },
                    new CheckoutQueueEntrySaveRecord[0],
                    new CheckoutStationSaveRecord(
                        "station-001",
                        "Closed",
                        string.Empty),
                    new[]
                    {
                        new CheckoutTransactionSaveRecord(
                            "transaction-001",
                            "customer-001",
                            "cart-001",
                            "station-001",
                            "Completed",
                            1,
                            1)
                    },
                    new DayCycleSaveRecord(
                        "day-003",
                        "Closed",
                        300,
                        300,
                        true),
                    new[]
                    {
                        new EconomyLedgerSaveRecord(
                            "ledger-revenue-001",
                            "CheckoutRevenue",
                            "transaction-001",
                            "day-003",
                            2999,
                            "EUR"),
                        new EconomyLedgerSaveRecord(
                            "ledger-cost-001",
                            "SupplierReceivingCost",
                            "receipt-001",
                            "day-003",
                            1200,
                            "EUR")
                    });
            }
        }
    }
}
