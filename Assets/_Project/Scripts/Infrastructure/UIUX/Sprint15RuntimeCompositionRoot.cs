using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRMGames.CartridgeAndCloud.Application.Persistence;
using VRMGames.CartridgeAndCloud.Application.UIUX;
using VRMGames.CartridgeAndCloud.Domain.Persistence;
using VRMGames.CartridgeAndCloud.Infrastructure.Persistence;

namespace VRMGames.CartridgeAndCloud.Infrastructure.UIUX
{
    [DefaultExecutionOrder(-10000)]
    public sealed class Sprint15RuntimeCompositionRoot :
        MonoBehaviour
    {
        private const string SettingsResourceName =
            "CC_Sprint15Settings";

        private int _lastInstalledSceneHandle =
            int.MinValue;

        public static Sprint15RuntimeCompositionRoot
            Instance { get; private set; }

        public Sprint15SettingsAsset Settings {
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

        public StoreUiProjectionService Projection {
            get;
            private set;
        }

        public Sprint15InputMapGate InputGate {
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
                Sprint15RuntimeCompositionRoot>();
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
                    Sprint15SettingsAsset>(
                    SettingsResourceName);

            if (Settings == null)
            {
                Settings =
                    ScriptableObject.CreateInstance<
                        Sprint15SettingsAsset>();
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
                    new SystemUtcClock());

            Autosave =
                new DailyAutosaveService(
                    saveRepository,
                    autosaveMarkerRepository,
                    ActiveSession);

            Projection =
                new StoreUiProjectionService();

            InputGate =
                new Sprint15InputMapGate();

            SceneManager.sceneLoaded +=
                HandleSceneLoaded;
        }

        private void Start()
        {
            InstallForScene(
                SceneManager.GetActiveScene());
        }

        private void OnDestroy()
        {
            if (Instance != this)
            {
                return;
            }

            SceneManager.sceneLoaded -=
                HandleSceneLoaded;
            InputGate?.ExitUiExclusive();
            Instance = null;
        }

        public void SetUserMessage(string message)
        {
            LastUserMessage =
                message ?? string.Empty;
        }

        public void EnterStore()
        {
            SceneManager.LoadSceneAsync(
                Settings.StoreSceneName,
                LoadSceneMode.Single);
        }

        public void ReturnToMainMenu()
        {
            InputGate.ExitUiExclusive();

            SceneManager.LoadSceneAsync(
                Settings.MainMenuSceneName,
                LoadSceneMode.Single);
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
                    StringComparison.Ordinal))
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

            bool validTransition =
                (currentState == "BeforeOpen" &&
                 targetState == "Open") ||
                (currentState == "Open" &&
                 targetState == "Closing") ||
                (currentState == "Closing" &&
                 targetState == "Closed");

            if (!validTransition)
            {
                reason =
                    $"Cannot transition from " +
                    $"{currentState} to {targetState}.";
                return false;
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

            int elapsed =
                targetState == "Closed"
                    ? current.DayCycle
                        .OpenDurationSeconds
                    : current.DayCycle
                        .ElapsedOpenSeconds;

            try
            {
                IntegratedGameStateSnapshot next =
                    new IntegratedGameStateSnapshot(
                        current.SchemaVersion,
                        current.SessionId,
                        current.SlotId,
                        current.CreatedUtc,
                        DateTime.UtcNow,
                        current.CurrentDay,
                        current.CashCents,
                        current.CurrencyCode,
                        current.Inventories,
                        current.SupplierOrders,
                        current.Displays,
                        current.Customers,
                        current.ShoppingSessions,
                        current.Reservations,
                        current.QueueEntries,
                        new CheckoutStationSaveRecord(
                            current.CheckoutStation
                                .StationId,
                            stationState,
                            currentEntryId),
                        current.Transactions,
                        new DayCycleSaveRecord(
                            current.DayCycle.DayId,
                            targetState,
                            current.DayCycle
                                .OpenDurationSeconds,
                            elapsed,
                            current.DayCycle
                                .AutoBeginClosing),
                        current.LedgerEntries);

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
                    StringComparison.Ordinal))
            {
                throw new InvalidOperationException(
                    "The current day must be closed.");
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
                            .AutoBeginClosing),
                    new EconomyLedgerSaveRecord[0]);

            ActiveSession.Replace(next);
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
                GameObject screen =
                    new GameObject(
                        "Sprint15StoreUI");
                screen.AddComponent<
                    StoreHudScreen>()
                    .Initialize(this);
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
