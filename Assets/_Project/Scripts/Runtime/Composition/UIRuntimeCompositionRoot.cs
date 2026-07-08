using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRMGames.CartridgeAndCloud.Application.DayCycle;
using VRMGames.CartridgeAndCloud.Application.Customers;
using VRMGames.CartridgeAndCloud.Application.Persistence;
using VRMGames.CartridgeAndCloud.Application.Store;
using VRMGames.CartridgeAndCloud.Application.UIUX;
using VRMGames.CartridgeAndCloud.Domain.Checkout;
using VRMGames.CartridgeAndCloud.Domain.DayCycle;
using VRMGames.CartridgeAndCloud.Domain.Economy;
using VRMGames.CartridgeAndCloud.Domain.GameSession;
using VRMGames.CartridgeAndCloud.Domain.Persistence;
using VRMGames.CartridgeAndCloud.Domain.Store;
using VRMGames.CartridgeAndCloud.Infrastructure.GameSession;
using VRMGames.CartridgeAndCloud.Infrastructure.Persistence;
using VRMGames.CartridgeAndCloud.Infrastructure.UIUX;
using VRMGames.CartridgeAndCloud.Runtime.UIUX;

namespace VRMGames.CartridgeAndCloud.Runtime.Composition
{
    [DefaultExecutionOrder(-10000)]
    public sealed class UIRuntimeCompositionRoot :
        MonoBehaviour
    {
        private const string SettingsResourceName =
            "CC_Sprint15Settings";

        private int _lastInstalledSceneHandle =
            int.MinValue;
        private string _midnightFinalizedDayId = string.Empty;
        private string _clockSessionId =
            string.Empty;
        private string _clockDayId =
            string.Empty;

        public static UIRuntimeCompositionRoot
            Instance { get; private set; }

        public UIRuntimeSettingsAsset Settings {
            get;
            private set;
        }

        public AccessibilitySettingsService
            Accessibility { get; private set; }

        public TutorialService Tutorial {
            get;
            private set;
        }

        public ActiveGameSessionService ActiveSession {
            get;
            private set;
        }

        public SlotSelectionService Slots {
            get;
            private set;
        }

        public DailyAutosaveService Autosave {
            get;
            private set;
        }

        public ManualSaveService ManualSave {
            get;
            private set;
        }

        public ISaveMutationRegistry SaveMutations {
            get;
            private set;
        }

        public StoreUiProjectionService Projection {
            get;
            private set;
        }

        public UiInputContextGate InputGate {
            get;
            private set;
        }

        public ISimulationClock SimulationClock {
            get;
            private set;
        }

        public IPauseService PauseService {
            get;
            private set;
        }

        public IStoreOperationalGate StoreOperationalGate {
            get;
            private set;
        }

        public IStoreClosingEconomyService
            StoreClosingEconomyService {
                get;
                private set;
            }

        public IStoreManagementStateProvider
            StoreManagementStateProvider {
                get;
                private set;
            }

        public string LastUserMessage {
            get;
            private set;
        }

        [RuntimeInitializeOnLoadMethod(
            RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Install()
        {
            if (Instance != null)
            {
                return;
            }

            GameObject root =
                new GameObject(
                    "Sprint15RuntimeRoot");
            root.AddComponent<
                UIRuntimeCompositionRoot>();
        }

        private void Awake()
        {
            if (Instance != null &&
                Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            Settings =
                Resources.Load<
                    UIRuntimeSettingsAsset>(
                    SettingsResourceName);

            if (Settings == null)
            {
                Settings =
                    ScriptableObject.CreateInstance<
                        UIRuntimeSettingsAsset>();
            }

            string uiRoot =
                Path.Combine(
                    UnityEngine.Application.persistentDataPath,
                    "Sprint15UIUX");

            JsonIntegratedSaveRepository saveRepository =
                new JsonIntegratedSaveRepository(
                    Path.Combine(
                        UnityEngine.Application.persistentDataPath,
                        JsonIntegratedSaveRepository
                            .SaveDirectoryName));

            JsonTutorialProgressRepository
                tutorialRepository =
                    new JsonTutorialProgressRepository(
                        Path.Combine(
                            uiRoot,
                            "Tutorial"));

            JsonAutosaveMarkerRepository
                autosaveMarkerRepository =
                    new JsonAutosaveMarkerRepository(
                        Path.Combine(
                            uiRoot,
                            "Autosave"));

            Accessibility =
                new AccessibilitySettingsService(
                    new JsonAccessibilitySettingsRepository(
                        Path.Combine(
                            uiRoot,
                            "accessibility.json")));

            Tutorial =
                new TutorialService(
                    tutorialRepository);

            ActiveSession =
                new ActiveGameSessionService();

            SystemUtcClock utcClock =
                new SystemUtcClock();
            SaveMutations =
                new SaveMutationRegistry();
            SimulationClock =
                new SimulationClock();
            PauseService =
                new PauseService();

            DefaultIntegratedGameStateFactory
                factory =
                    new DefaultIntegratedGameStateFactory(
                        Settings.CurrencyCode,
                        Settings.InitialCashCents,
                        Settings.DayDurationSeconds);

            Slots =
                new SlotSelectionService(
                    saveRepository,
                    tutorialRepository,
                    autosaveMarkerRepository,
                    ActiveSession,
                    factory,
                    utcClock);

            Autosave =
                new DailyAutosaveService(
                    saveRepository,
                    autosaveMarkerRepository,
                    ActiveSession);

            ManualSave =
                new ManualSaveService(
                    saveRepository,
                    ActiveSession,
                    PauseService,
                    SaveMutations,
                    utcClock);

            Projection =
                new StoreUiProjectionService();

            InputGate =
                new UiInputContextGate();

            ActiveSession.SnapshotChanged +=
                HandleSnapshotChanged;
            PauseService.Changed +=
                HandlePauseChanged;
            SceneManager.sceneLoaded +=
                HandleSceneLoaded;
        }

        private void Start()
        {
            InstallForScene(
                SceneManager.GetActiveScene());
        }

        private void Update()
        {
            if (!IsStoreSceneActive() ||
                !ActiveSession.HasActiveSession)
            {
                ApplyUnitySimulationRate();
                return;
            }

            IntegratedGameStateSnapshot snapshot =
                ActiveSession.Snapshot;
            SynchronizeSimulationClock(snapshot);

            SimulationClockTickResult tick =
                SimulationClock.Tick(
                    Time.unscaledDeltaTime,
                    PauseService.IsPaused);

            if (tick.WholeSecondChanged ||
                tick.ReachedEnd)
            {
                PublishSimulationTime(
                    snapshot,
                    tick.ReachedEnd);
            }

            TryFinalizeDayAtMidnight();
        }

        private void OnDestroy()
        {
            if (Instance != this)
            {
                return;
            }

            SceneManager.sceneLoaded -=
                HandleSceneLoaded;

            if (ActiveSession != null)
            {
                ActiveSession.SnapshotChanged -=
                    HandleSnapshotChanged;
            }

            if (PauseService != null)
            {
                PauseService.Changed -=
                    HandlePauseChanged;
                PauseService.Clear();
            }

            InputGate?.ExitUiExclusive();
            StoreOperationalGate = null;
            StoreClosingEconomyService = null;
            StoreManagementStateProvider = null;
            Time.timeScale = 1f;
            Instance = null;
        }

        public void SetUserMessage(string message)
        {
            LastUserMessage =
                message ?? string.Empty;
        }

        public ManualSaveEvaluation EvaluateManualSave()
        {
            return ManualSave.Evaluate();
        }

        public ManualSaveResult TryManualSave()
        {
            ManualSaveResult result =
                ManualSave.Save();
            LastUserMessage = result.Detail;
            return result;
        }

        public void RegisterManualSaveCheckpointParticipant(
            IManualSaveCheckpointParticipant participant)
        {
            ManualSave.RegisterCheckpointParticipant(
                participant);
            Autosave.RegisterCheckpointParticipant(
                participant);
        }

        public void UnregisterManualSaveCheckpointParticipant(
            IManualSaveCheckpointParticipant participant)
        {
            ManualSave.UnregisterCheckpointParticipant(
                participant);
            Autosave.UnregisterCheckpointParticipant(
                participant);
        }

        public void RegisterStoreOperationalGate(
            IStoreOperationalGate gate)
        {
            StoreOperationalGate = gate ??
                throw new ArgumentNullException(
                    nameof(gate));
        }

        public void UnregisterStoreOperationalGate(
            IStoreOperationalGate gate)
        {
            if (ReferenceEquals(
                    StoreOperationalGate,
                    gate))
            {
                StoreOperationalGate = null;
            }
        }

        public void RegisterStoreClosingEconomyService(
            IStoreClosingEconomyService service)
        {
            StoreClosingEconomyService = service ??
                throw new ArgumentNullException(
                    nameof(service));
        }

        public void UnregisterStoreClosingEconomyService(
            IStoreClosingEconomyService service)
        {
            if (ReferenceEquals(
                    StoreClosingEconomyService,
                    service))
            {
                StoreClosingEconomyService = null;
            }
        }

        public void RegisterStoreManagementStateProvider(
            IStoreManagementStateProvider provider)
        {
            StoreManagementStateProvider = provider ??
                throw new ArgumentNullException(nameof(provider));
        }

        public void UnregisterStoreManagementStateProvider(
            IStoreManagementStateProvider provider)
        {
            if (ReferenceEquals(
                    StoreManagementStateProvider,
                    provider))
            {
                StoreManagementStateProvider = null;
            }
        }

        public void EnterStore()
        {
            PauseService.Clear();
            Time.timeScale = 1f;

            SceneManager.LoadSceneAsync(
                Settings.StoreSceneName,
                LoadSceneMode.Single);
        }

        public void ReturnToMainMenu()
        {
            PauseService.Clear();
            InputGate.ExitUiExclusive();
            Time.timeScale = 1f;

            SceneManager.LoadSceneAsync(
                Settings.MainMenuSceneName,
                LoadSceneMode.Single);
        }

        public bool TrySetSimulationSpeed(
            float multiplier,
            out string reason)
        {
            if (!ActiveSession.HasActiveSession)
            {
                reason = "No active session.";
                return false;
            }

            try
            {
                SimulationClock.SetSpeed(multiplier);

                IntegratedGameStateSnapshot current =
                    ActiveSession.Snapshot;
                DayCycleSaveRecord nextDay =
                    new DayCycleSaveRecord(
                        current.DayCycle.DayId,
                        current.DayCycle.State,
                        current.DayCycle
                            .OpenDurationSeconds,
                        current.DayCycle
                            .ElapsedOpenSeconds,
                        current.DayCycle
                            .AutoBeginClosing,
                        SimulationClock
                            .SelectedSpeedMultiplier);

                ActiveSession.Replace(
                    CloneWithDayCycle(
                        current,
                        nextDay,
                        current.CheckoutStation));

                reason =
                    "Simulation speed set to " +
                    SimulationSpeedPolicy.Format(
                        SimulationClock
                            .SelectedSpeedMultiplier) +
                    ".";
                LastUserMessage = reason;
                ApplyUnitySimulationRate();
                return true;
            }
            catch (Exception exception)
            {
                reason = exception.Message;
                LastUserMessage = reason;
                return false;
            }
        }

        public DailyAutosaveResult
            PublishAuthoritativeSnapshot(
                IntegratedGameStateSnapshot snapshot)
        {
            if (snapshot == null)
            {
                throw new ArgumentNullException(
                    nameof(snapshot));
            }

            if (!ActiveSession.HasActiveSession)
            {
                ActiveSession.Activate(
                    snapshot.SlotId,
                    snapshot);
            }
            else
            {
                ActiveSession.Replace(snapshot);
            }

            if (!string.Equals(
                    snapshot.DayCycle.State,
                    "Closed",
                    StringComparison.Ordinal) ||
                !StoreTradingHoursPolicy.IsDayComplete(
                    snapshot.DayCycle.ElapsedDaySeconds,
                    snapshot.DayCycle.DayDurationSeconds))
            {
                return new DailyAutosaveResult(
                    DailyAutosaveStatus.NotClosed,
                    snapshot.DayCycle.DayId,
                    "Snapshot published.");
            }

            DailyAutosaveResult result =
                Autosave.TryAutosave();
            LastUserMessage = result.Detail;
            return result;
        }

        public bool TryTransitionDay(
            string targetState,
            out string reason)
        {
            if (!ActiveSession.HasActiveSession)
            {
                reason = "No active session.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(targetState))
            {
                reason = "A target state is required.";
                return false;
            }

            IntegratedGameStateSnapshot current =
                ActiveSession.Snapshot;
            string currentState =
                current.DayCycle.State;

            bool dayComplete =
                StoreTradingHoursPolicy.IsDayComplete(
                    current.DayCycle.ElapsedDaySeconds,
                    current.DayCycle.DayDurationSeconds);

            if (targetState == "Open" &&
                (currentState == "BeforeOpen" || currentState == "Closed") &&
                !StoreTradingHoursPolicy.CanOpen(
                    current.DayCycle.ElapsedDaySeconds,
                    current.DayCycle.DayDurationSeconds))
            {
                reason =
                    "The store can only open between 08:00 and 22:00. Current time: " +
                    StoreTradingHoursPolicy.FormatTime(
                        current.DayCycle.ElapsedDaySeconds,
                        current.DayCycle.DayDurationSeconds) + ".";
                LastUserMessage = reason;
                return false;
            }

            bool validTransition = StoreTradingHoursPolicy.CanTransition(
                currentState,
                targetState,
                current.DayCycle.ElapsedDaySeconds,
                current.DayCycle.DayDurationSeconds);

            if (!validTransition)
            {
                reason =
                    $"Cannot transition from " +
                    $"{currentState} to {targetState}.";
                return false;
            }

            if (targetState == "Open")
            {

                if (StoreOperationalGate == null &&
                    IsStoreSceneActive())
                {
                    reason =
                        "Store runtime is still initializing. Try opening again in a moment.";
                    return false;
                }

                if (StoreOperationalGate != null)
                {
                    StoreCustomerAdmissionDecision opening =
                        StoreOperationalGate
                            .EvaluateOpening();

                    if (!opening.Allowed)
                    {
                        reason = opening.Detail;
                        LastUserMessage = reason;
                        return false;
                    }
                }
            }

            if (targetState == "Closed" &&
                StoreOperationalGate != null)
            {
                StoreCustomerAdmissionDecision closing =
                    StoreOperationalGate
                        .EvaluateClosingCompletion();

                if (!closing.Allowed)
                {
                    reason = closing.Detail;
                    LastUserMessage = reason;
                    return false;
                }
            }

            if (targetState == "Closed" &&
                dayComplete &&
                StoreClosingEconomyService != null)
            {
                StoreOperationResult settlement =
                    StoreClosingEconomyService
                        .SettleClosingEconomy();

                if (!settlement.Succeeded)
                {
                    reason = settlement.Detail;
                    LastUserMessage = reason;
                    return false;
                }

                current = ActiveSession.Snapshot;
            }

            string stationState =
                current.CheckoutStation.State;
            string currentEntryId =
                current.CheckoutStation
                    .CurrentEntryId;

            if (targetState == "Open" &&
                stationState == "Closed")
            {
                stationState = "Available";
                currentEntryId = string.Empty;
            }

            if (targetState == "Closed")
            {
                if (stationState == "Busy")
                {
                    reason =
                        "Checkout is still processing.";
                    return false;
                }

                stationState = "Closed";
                currentEntryId = string.Empty;
            }

            try
            {
                DayCycleSaveRecord nextDay =
                    new DayCycleSaveRecord(
                        current.DayCycle.DayId,
                        targetState,
                        current.DayCycle
                            .OpenDurationSeconds,
                        current.DayCycle
                            .ElapsedOpenSeconds,
                        current.DayCycle
                            .AutoBeginClosing,
                        current.DayCycle
                            .SimulationSpeedMultiplier);

                IntegratedGameStateSnapshot next =
                    CloneWithDayCycle(
                        current,
                        nextDay,
                        new CheckoutStationSaveRecord(
                            current.CheckoutStation
                                .StationId,
                            stationState,
                            currentEntryId));

                DailyAutosaveResult result =
                    PublishAuthoritativeSnapshot(next);

                reason =
                    targetState == "Closed"
                        ? result.Detail
                        : $"Store day is now " +
                          $"{targetState}.";
                LastUserMessage = reason;
                return true;
            }
            catch (Exception exception)
            {
                reason =
                    "The day cannot transition yet: " +
                    exception.Message;
                LastUserMessage = reason;
                return false;
            }
        }

        public void BeginNextDay()
        {
            if (!ActiveSession.HasActiveSession)
            {
                throw new InvalidOperationException(
                    "No active session.");
            }

            IntegratedGameStateSnapshot current =
                ActiveSession.Snapshot;

            if (!string.Equals(
                    current.DayCycle.State,
                    "Closed",
                    StringComparison.Ordinal) ||
                !StoreTradingHoursPolicy.IsDayComplete(
                    current.DayCycle.ElapsedDaySeconds,
                    current.DayCycle.DayDurationSeconds))
            {
                throw new InvalidOperationException(
                    "The current day must be closed at 24:00.");
            }

            int nextDay = checked(
                current.CurrentDay + 1);
            string dayId =
                "day-" + nextDay.ToString("000");

            IntegratedGameStateSnapshot next =
                new IntegratedGameStateSnapshot(
                    IntegratedGameStateSnapshot
                        .CurrentSchemaVersion,
                    current.SessionId,
                    current.SlotId,
                    current.CreatedUtc,
                    DateTime.UtcNow,
                    nextDay,
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
                    new CheckoutTransactionSaveRecord[0],
                    new DayCycleSaveRecord(
                        dayId,
                        "BeforeOpen",
                        current.DayCycle
                            .OpenDurationSeconds,
                        0,
                        current.DayCycle
                            .AutoBeginClosing,
                        current.DayCycle
                            .SimulationSpeedMultiplier),
                    new EconomyLedgerSaveRecord[0]);

            PauseService.Clear();
            _midnightFinalizedDayId = string.Empty;
            ActiveSession.Replace(next);
        }

        private void PublishSimulationTime(
            IntegratedGameStateSnapshot source,
            bool reachedEnd)
        {
            if (!ActiveSession.HasActiveSession ||
                source != ActiveSession.Snapshot)
            {
                return;
            }

            int elapsed = SimulationClock.ElapsedWholeSeconds;
            int duration = source.DayCycle.DayDurationSeconds;
            string nextState = source.DayCycle.State;

            if (string.Equals(nextState, "Open", StringComparison.Ordinal) &&
                StoreTradingHoursPolicy.HasReachedForcedClosing(
                    elapsed,
                    duration))
            {
                nextState = "Closing";
            }
            else if (reachedEnd &&
                     string.Equals(nextState, "BeforeOpen", StringComparison.Ordinal))
            {
                nextState = "Closed";
            }

            DayCycleSaveRecord nextDay =
                new DayCycleSaveRecord(
                    source.DayCycle.DayId,
                    nextState,
                    duration,
                    elapsed,
                    source.DayCycle.AutoBeginClosing,
                    SimulationClock.SelectedSpeedMultiplier);

            ActiveSession.Replace(
                CloneWithDayCycle(
                    source,
                    nextDay,
                    source.CheckoutStation));

            if (!string.Equals(
                    source.DayCycle.State,
                    nextState,
                    StringComparison.Ordinal))
            {
                LastUserMessage =
                    nextState == "Closing"
                        ? "It is 22:00. The store is closing and no new customers may enter."
                        : "The day reached 24:00 and is ready to finalize.";
            }
        }

        private void TryFinalizeDayAtMidnight()
        {
            if (!ActiveSession.HasActiveSession)
            {
                return;
            }

            IntegratedGameStateSnapshot snapshot = ActiveSession.Snapshot;
            if (!StoreTradingHoursPolicy.IsDayComplete(
                    snapshot.DayCycle.ElapsedDaySeconds,
                    snapshot.DayCycle.DayDurationSeconds) ||
                string.Equals(
                    _midnightFinalizedDayId,
                    snapshot.DayCycle.DayId,
                    StringComparison.Ordinal))
            {
                return;
            }

            if (string.Equals(snapshot.DayCycle.State, "Open", StringComparison.Ordinal))
            {
                TryTransitionDay("Closing", out _);
                snapshot = ActiveSession.Snapshot;
            }

            if (string.Equals(snapshot.DayCycle.State, "Closing", StringComparison.Ordinal))
            {
                if (StoreOperationalGate != null)
                {
                    StoreCustomerAdmissionDecision closing =
                        StoreOperationalGate.EvaluateClosingCompletion();
                    if (!closing.Allowed)
                    {
                        return;
                    }
                }

                if (TryTransitionDay("Closed", out _))
                {
                    _midnightFinalizedDayId = snapshot.DayCycle.DayId;
                }
                return;
            }

            if (string.Equals(snapshot.DayCycle.State, "BeforeOpen", StringComparison.Ordinal))
            {
                if (TryTransitionDay("Closed", out _))
                {
                    _midnightFinalizedDayId = snapshot.DayCycle.DayId;
                }
                return;
            }

            if (string.Equals(snapshot.DayCycle.State, "Closed", StringComparison.Ordinal))
            {
                if (StoreClosingEconomyService != null)
                {
                    StoreOperationResult settlement =
                        StoreClosingEconomyService.SettleClosingEconomy();
                    if (!settlement.Succeeded)
                    {
                        LastUserMessage = settlement.Detail;
                        return;
                    }
                    snapshot = ActiveSession.Snapshot;
                }

                _midnightFinalizedDayId = snapshot.DayCycle.DayId;
                PublishAuthoritativeSnapshot(snapshot);
            }
        }

        private static IntegratedGameStateSnapshot
            CloneWithDayCycle(
                IntegratedGameStateSnapshot source,
                DayCycleSaveRecord dayCycle,
                CheckoutStationSaveRecord checkoutStation)
        {
            return new IntegratedGameStateSnapshot(
                source.SchemaVersion,
                source.SessionId,
                source.SlotId,
                source.CreatedUtc,
                DateTime.UtcNow,
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
                checkoutStation,
                source.Transactions,
                dayCycle,
                source.LedgerEntries);
        }

        private void HandleSnapshotChanged(
            IntegratedGameStateSnapshot snapshot)
        {
            if (snapshot == null)
            {
                return;
            }

            SynchronizeSimulationClock(snapshot);
            ApplyUnitySimulationRate();
        }

        private void SynchronizeSimulationClock(
            IntegratedGameStateSnapshot snapshot)
        {
            string sessionId =
                snapshot.SessionId.ToString();
            float selectedSpeed =
                SimulationSpeedPolicy
                    .NormalizeOrDefault(
                        snapshot.DayCycle
                            .SimulationSpeedMultiplier);

            bool requiresFullSynchronization =
                !string.Equals(
                    _clockSessionId,
                    sessionId,
                    StringComparison.Ordinal) ||
                !string.Equals(
                    _clockDayId,
                    snapshot.DayCycle.DayId,
                    StringComparison.Ordinal) ||
                SimulationClock.DurationSeconds !=
                    snapshot.DayCycle
                        .OpenDurationSeconds ||
                SimulationClock.ElapsedWholeSeconds !=
                    snapshot.DayCycle
                        .ElapsedOpenSeconds;

            if (requiresFullSynchronization)
            {
                SimulationClock.Synchronize(
                    snapshot.DayCycle
                        .OpenDurationSeconds,
                    snapshot.DayCycle
                        .ElapsedOpenSeconds,
                    selectedSpeed);
                _clockSessionId = sessionId;
                _clockDayId =
                    snapshot.DayCycle.DayId;
                return;
            }

            if (Math.Abs(
                    SimulationClock
                        .SelectedSpeedMultiplier -
                    selectedSpeed) > 0.0001f)
            {
                SimulationClock.SetSpeed(
                    selectedSpeed);
            }
        }

        private void HandlePauseChanged(bool isPaused)
        {
            ApplyUnitySimulationRate();
            LastUserMessage = isPaused
                ? "Simulation paused."
                : "Simulation resumed at " +
                  SimulationSpeedPolicy.Format(
                      SimulationClock
                          .SelectedSpeedMultiplier) +
                  ".";
        }

        private void ApplyUnitySimulationRate()
        {
            float targetRate = 1f;

            if (IsStoreSceneActive() &&
                ActiveSession != null &&
                ActiveSession.HasActiveSession)
            {
                if (PauseService.IsPaused)
                {
                    targetRate = 0f;
                }
                else
                {
                    targetRate =
                        SimulationClock.SelectedSpeedMultiplier;
                }
            }

            if (!Mathf.Approximately(
                    Time.timeScale,
                    targetRate))
            {
                Time.timeScale = targetRate;
            }
        }

        private bool IsStoreSceneActive()
        {
            return Settings != null &&
                   string.Equals(
                       SceneManager
                           .GetActiveScene().name,
                       Settings.StoreSceneName,
                       StringComparison.Ordinal);
        }

        private void HandleSceneLoaded(
            Scene scene,
            LoadSceneMode mode)
        {
            InstallForScene(scene);
        }

        private void InstallForScene(Scene scene)
        {
            if (scene.handle ==
                _lastInstalledSceneHandle)
            {
                return;
            }

            _lastInstalledSceneHandle =
                scene.handle;

            DestroyExistingScreen(
                "Sprint15MainMenuUI");
            DestroyExistingScreen(
                "Sprint15StoreUI");

            if (scene.name ==
                Settings.MainMenuSceneName)
            {
                PauseService.Clear();
                Time.timeScale = 1f;

                GameObject screen =
                    new GameObject(
                        "Sprint15MainMenuUI");
                screen.AddComponent<
                    MainMenuSlotScreen>()
                    .Initialize(this);
            }
            else if (scene.name ==
                     Settings.StoreSceneName)
            {
                if (ActiveSession.HasActiveSession)
                {
                    SynchronizeSimulationClock(
                        ActiveSession.Snapshot);
                }

                ApplyUnitySimulationRate();

                GameObject screen =
                    new GameObject(
                        "Sprint15StoreUI");
                screen.AddComponent<
                    StoreHudScreen>()
                    .Initialize(this);
            }
            else
            {
                PauseService.Clear();
                Time.timeScale = 1f;
            }
        }

        private static void DestroyExistingScreen(
            string name)
        {
            GameObject existing =
                GameObject.Find(name);

            if (existing != null)
            {
                Destroy(existing);
            }
        }
    }
}
