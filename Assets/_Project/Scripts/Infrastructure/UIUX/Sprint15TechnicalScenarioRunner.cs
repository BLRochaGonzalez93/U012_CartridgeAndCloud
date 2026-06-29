using System;
using System.IO;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Application.GameSession;
using VRMGames.CartridgeAndCloud.Application.Persistence;
using VRMGames.CartridgeAndCloud.Application.UIUX;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;
using VRMGames.CartridgeAndCloud.Domain.Persistence;
using VRMGames.CartridgeAndCloud.Domain.UIUX;
using VRMGames.CartridgeAndCloud.Infrastructure.Persistence;

namespace VRMGames.CartridgeAndCloud.Infrastructure.UIUX
{
    public sealed class Sprint15TechnicalScenarioRunner :
        MonoBehaviour
    {
        [SerializeField]
        private Sprint15SettingsAsset _settings;

        [SerializeField]
        private bool _runOnStart;

        public bool LastScenarioPassed { get; private set; }
        public bool LastThreeSlotsVisible { get; private set; }
        public bool LastNewGameSucceeded { get; private set; }
        public bool LastContinueSucceeded { get; private set; }
        public bool LastSlotsIsolated { get; private set; }
        public bool LastHudProjected { get; private set; }
        public bool LastAllPanelsProjected { get; private set; }
        public bool LastTutorialPersisted { get; private set; }
        public bool LastAccessibilityPersisted { get; private set; }
        public bool LastAutosaveSucceeded { get; private set; }
        public bool LastAutosaveIdempotent { get; private set; }
        public bool LastBackupCreated { get; private set; }
        public bool LastDeleteSucceeded { get; private set; }

        private void Start()
        {
            if (_runOnStart)
            {
                RunScenario();
            }
        }

        public void Configure(
            Sprint15SettingsAsset settings,
            bool runOnStart)
        {
            _settings = settings;
            _runOnStart = runOnStart;
        }

        [ContextMenu(
            "Run Sprint 15 UI UX Scenario")]
        public void RunScenario()
        {
            if (_settings == null)
            {
                throw new InvalidOperationException(
                    "Sprint 15 settings are required.");
            }

            string rootDirectory =
                Path.Combine(
                    UnityEngine.Application.temporaryCachePath,
                    "CC_S15_UIUX_Technical");

            if (Directory.Exists(rootDirectory))
            {
                Directory.Delete(
                    rootDirectory,
                    true);
            }

            Directory.CreateDirectory(rootDirectory);

            JsonIntegratedSaveRepository saveRepository =
                new JsonIntegratedSaveRepository(
                    Path.Combine(
                        rootDirectory,
                        "Saves"));

            JsonTutorialProgressRepository tutorialRepository =
                new JsonTutorialProgressRepository(
                    Path.Combine(
                        rootDirectory,
                        "Tutorial"));

            JsonAutosaveMarkerRepository markerRepository =
                new JsonAutosaveMarkerRepository(
                    Path.Combine(
                        rootDirectory,
                        "Autosave"));

            JsonAccessibilitySettingsRepository
                accessibilityRepository =
                    new JsonAccessibilitySettingsRepository(
                        Path.Combine(
                            rootDirectory,
                            "accessibility.json"));

            ActiveGameSessionService active =
                new ActiveGameSessionService();

            FixedClock clock =
                new FixedClock(
                    new DateTime(
                        2026,
                        6,
                        29,
                        16,
                        0,
                        0,
                        DateTimeKind.Utc));

            SlotSelectionService slots =
                new SlotSelectionService(
                    saveRepository,
                    tutorialRepository,
                    markerRepository,
                    active,
                    new DefaultIntegratedGameStateFactory(
                        _settings.CurrencyCode,
                        _settings.InitialCashCents,
                        _settings.DayDurationSeconds),
                    clock);

            TutorialService tutorial =
                new TutorialService(
                    tutorialRepository);

            AccessibilitySettingsService accessibility =
                new AccessibilitySettingsService(
                    accessibilityRepository);

            DailyAutosaveService autosave =
                new DailyAutosaveService(
                    saveRepository,
                    markerRepository,
                    active);

            StoreUiProjectionService projection =
                new StoreUiProjectionService();

            LastThreeSlotsVisible =
                slots.InspectAll().Count == 3 &&
                slots.Inspect(
                    new SaveSlotId(0)).State ==
                    SlotPresentationState.Empty &&
                slots.Inspect(
                    new SaveSlotId(1)).State ==
                    SlotPresentationState.Empty &&
                slots.Inspect(
                    new SaveSlotId(2)).State ==
                    SlotPresentationState.Empty;

            SlotOperationResult created =
                slots.CreateNew(
                    new SaveSlotId(0),
                    overwriteConfirmed: false);
            LastNewGameSucceeded =
                created.Succeeded &&
                active.HasActiveSession &&
                active.ActiveSlotId ==
                    new SaveSlotId(0);

            SlotOperationResult secondCreated =
                slots.CreateNew(
                    new SaveSlotId(1),
                    overwriteConfirmed: false);

            SlotOperationResult continued =
                slots.Continue(
                    new SaveSlotId(0));
            LastContinueSucceeded =
                continued.Succeeded &&
                active.ActiveSlotId ==
                    new SaveSlotId(0);

            LastSlotsIsolated =
                secondCreated.Succeeded &&
                slots.Inspect(
                    new SaveSlotId(0)).State ==
                    SlotPresentationState.Ready &&
                slots.Inspect(
                    new SaveSlotId(1)).State ==
                    SlotPresentationState.Ready &&
                slots.Inspect(
                    new SaveSlotId(2)).State ==
                    SlotPresentationState.Empty;

            StoreHudSnapshot hud =
                projection.BuildHud(
                    active.Snapshot,
                    DailyAutosaveStatus.NotClosed);
            LastHudProjected =
                hud.CurrentDay == 1 &&
                hud.CurrencyCode ==
                    _settings.CurrencyCode &&
                hud.ActiveCustomers == 0 &&
                hud.QueueLength == 0;

            LastAllPanelsProjected =
                ProjectAllPanels(
                    projection,
                    active.Snapshot);

            TutorialProgress progress =
                tutorial.Start(
                    active.ActiveSlotId);
            progress =
                tutorial.Advance(
                    active.ActiveSlotId);
            TutorialProgress reloadedTutorial =
                new TutorialService(
                    tutorialRepository)
                    .GetProgress(
                        active.ActiveSlotId);
            LastTutorialPersisted =
                progress.CurrentStep ==
                    TutorialStepId
                        .MovementAndCamera &&
                reloadedTutorial.CurrentStep ==
                    progress.CurrentStep &&
                reloadedTutorial.State ==
                    TutorialProgressState.Active;

            UiAccessibilitySettings changed =
                accessibility.Current
                    .WithUiScale(120)
                    .WithTextScale(130)
                    .WithReduceMotion(true);
            accessibility.Apply(changed);

            AccessibilitySettingsService reloadedAccessibility =
                new AccessibilitySettingsService(
                    accessibilityRepository);
            LastAccessibilityPersisted =
                reloadedAccessibility.Current
                    .UiScalePercent == 120 &&
                reloadedAccessibility.Current
                    .TextScalePercent == 130 &&
                reloadedAccessibility.Current
                    .ReduceMotion;

            IntegratedGameStateSnapshot closed =
                CloseCurrentDay(
                    active.Snapshot,
                    clock.UtcNow.AddMinutes(5));
            active.Replace(closed);

            DailyAutosaveResult firstAutosave =
                autosave.TryAutosave();
            DailyAutosaveResult secondAutosave =
                autosave.TryAutosave();

            LastAutosaveSucceeded =
                firstAutosave.Status ==
                    DailyAutosaveStatus.Saved;
            LastAutosaveIdempotent =
                secondAutosave.Status ==
                    DailyAutosaveStatus
                        .AlreadySaved;
            LastBackupCreated =
                File.Exists(
                    saveRepository.GetBackupPath(
                        active.ActiveSlotId));

            SlotOperationResult deleted =
                slots.Delete(
                    new SaveSlotId(0),
                    confirmed: true);
            LastDeleteSucceeded =
                deleted.Succeeded &&
                slots.Inspect(
                    new SaveSlotId(0)).State ==
                    SlotPresentationState.Empty &&
                tutorialRepository.Load(
                    new SaveSlotId(0)).State ==
                    TutorialProgressState
                        .NotStarted &&
                string.IsNullOrEmpty(
                    markerRepository
                        .LoadLastSavedDay(
                            new SaveSlotId(0)));

            LastScenarioPassed =
                LastThreeSlotsVisible &&
                LastNewGameSucceeded &&
                LastContinueSucceeded &&
                LastSlotsIsolated &&
                LastHudProjected &&
                LastAllPanelsProjected &&
                LastTutorialPersisted &&
                LastAccessibilityPersisted &&
                LastAutosaveSucceeded &&
                LastAutosaveIdempotent &&
                LastBackupCreated &&
                LastDeleteSucceeded;

            if (LastScenarioPassed)
            {
                Debug.Log(
                    "Sprint 15 UI UX technical scenario PASS.");
            }
            else
            {
                Debug.LogError(
                    "Sprint 15 UI UX technical scenario FAILED.");
            }
        }

        private static bool ProjectAllPanels(
            StoreUiProjectionService service,
            IntegratedGameStateSnapshot snapshot)
        {
            ManagementPanelId[] panels =
            {
                ManagementPanelId.Inventory,
                ManagementPanelId.Suppliers,
                ManagementPanelId.Displays,
                ManagementPanelId.Customers,
                ManagementPanelId.Shopping,
                ManagementPanelId.Checkout,
                ManagementPanelId.DayCycle,
                ManagementPanelId.Economy,
                ManagementPanelId.Help,
                ManagementPanelId.Accessibility
            };

            foreach (ManagementPanelId panel in panels)
            {
                ManagementPanelSnapshot projected =
                    service.BuildPanel(
                        snapshot,
                        panel);

                if (projected == null ||
                    projected.Rows.Count == 0)
                {
                    return false;
                }
            }

            return true;
        }

        private static IntegratedGameStateSnapshot
            CloseCurrentDay(
                IntegratedGameStateSnapshot current,
                DateTime updatedUtc)
        {
            return new IntegratedGameStateSnapshot(
                current.SchemaVersion,
                current.SessionId,
                current.SlotId,
                current.CreatedUtc,
                updatedUtc,
                current.CurrentDay,
                current.CashCents,
                current.CurrencyCode,
                current.Inventories,
                current.SupplierOrders,
                current.Displays,
                new CustomerSaveRecord[0],
                new ShoppingSessionSaveRecord[0],
                new ReservationSaveRecord[0],
                new CheckoutQueueEntrySaveRecord[0],
                new CheckoutStationSaveRecord(
                    current.CheckoutStation.StationId,
                    "Closed",
                    string.Empty),
                current.Transactions,
                new DayCycleSaveRecord(
                    current.DayCycle.DayId,
                    "Closed",
                    current.DayCycle
                        .OpenDurationSeconds,
                    current.DayCycle
                        .OpenDurationSeconds,
                    current.DayCycle
                        .AutoBeginClosing),
                current.LedgerEntries);
        }

        private sealed class FixedClock :
            IUtcClock
        {
            public DateTime UtcNow { get; set; }

            public FixedClock(DateTime utcNow)
            {
                UtcNow = utcNow;
            }
        }
    }
}
