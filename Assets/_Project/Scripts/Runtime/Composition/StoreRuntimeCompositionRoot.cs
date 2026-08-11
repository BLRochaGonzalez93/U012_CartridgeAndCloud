using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRMGames.CartridgeAndCloud.Application.Composition;
using VRMGames.CartridgeAndCloud.Application.Employees;
using VRMGames.CartridgeAndCloud.Application.PlayerAgency;
using VRMGames.CartridgeAndCloud.Application.UIUX;
using VRMGames.CartridgeAndCloud.Domain.Persistence;
using VRMGames.CartridgeAndCloud.Domain.PlayerAgency;
using VRMGames.CartridgeAndCloud.Domain.Store;
using VRMGames.CartridgeAndCloud.Infrastructure.UIUX;
using VRMGames.CartridgeAndCloud.Application.Store;
using VRMGames.CartridgeAndCloud.Infrastructure.Audio;
using VRMGames.CartridgeAndCloud.Infrastructure.Employees;
using VRMGames.CartridgeAndCloud.Infrastructure.Persistence;
using VRMGames.CartridgeAndCloud.Infrastructure.Store;
using VRMGames.CartridgeAndCloud.Runtime.Audio;
using VRMGames.CartridgeAndCloud.Runtime.Employees;
using VRMGames.CartridgeAndCloud.Runtime.Characters;
using VRMGames.CartridgeAndCloud.Runtime.Store;
using VRMGames.CartridgeAndCloud.Runtime.Inventory;
using VRMGames.CartridgeAndCloud.Runtime.Navigation;
using VRMGames.CartridgeAndCloud.Runtime.Placement;
using VRMGames.CartridgeAndCloud.Runtime.UIUX;
using VRMGames.CartridgeAndCloud.Domain.DayCycle;
using VRMGames.CartridgeAndCloud.Domain.GameSession;
using VRMGames.CartridgeAndCloud.Presentation.Store.Authoring;
using VRMGames.CartridgeAndCloud.Presentation.Interaction;
using VRMGames.CartridgeAndCloud.Presentation.PlayerAgency;
using VRMGames.CartridgeAndCloud.Presentation.PlayerMovement;
namespace VRMGames.CartridgeAndCloud.Runtime.Composition
{
    [DefaultExecutionOrder(-9900)]
    public sealed class StoreRuntimeCompositionRoot :
        MonoBehaviour
    {
public static StoreRuntimeCompositionRoot
            Instance { get; private set; }

        private StoreRuntimeSettingsAsset _settings;
        private StoreContentCatalogAsset
            _contentAsset;
        private StoreLayoutAsset _shellAsset;
        private StorePresentationCatalogAsset
            _presentationAsset;
        private AudioEventCatalogAsset
            _audioAsset;
        private EmployeeHiringCatalogAsset
            _employeeHiringAsset;

        private GameObject _storeRuntime;
        private Coroutine _initialization;
        private StoreOperationsFacade _service;
        private GameplayFeedbackPresenter _feedback;
        private StorePlacementCoordinator
            _placement;
        private StoreCharacterLoopController
            _characters;
        private AuthoredStoreRuntimeBinder
            _binder;
        private StoreOperationsScreen _operations;
        private StoreAudioRouter _audio;
        private InventoryVisualSynchronizer
            _inventoryVisuals;
        private DynamicStoreNavMeshController
            _navMesh;
        private SupplierDeliveryPresenter
            _supplierDeliveries;
        private StoreOperationalGate
            _operationalGate;
        private EmployeeHiringService
            _employeeHiring;
        private EmployeePayrollService
            _employeePayroll;
        private EmployeeScheduleService
            _employeeSchedule;
        private EmployeeStateService
            _employeeState;
        private EmployeePresenceService
            _employeePresence;
        private EmployeePersistenceService
            _employeePersistence;
        private EmployeePresencePresenter
            _employeePresencePresenter;
        private PlayerWorldInteractionController
            _playerInteraction;
        private ContextInspectionPanel
            _contextInspection;
        private ContextualWorldFeedbackPresenter
            _contextualWorldFeedback;
        private PlayerToolSelectionService
            _playerToolSelection;
        private PlayerToolWheelInputBridge
            _playerToolWheelInput;
        private PlayerToolWheelPresenter
            _playerToolWheelPresenter;
        private PlayerCarryLoadService
            _playerCarryLoad;
        private PlayerMovementInputBridge
            _playerMovementInput;
        private PhysicalWorkExecutionService
            _physicalWork;
        private string _lastObservedDayState = string.Empty;

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
                    "StoreRuntimeCompositionRoot");

            root.AddComponent<
                StoreRuntimeCompositionRoot>();
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

            LoadAssets();

            SceneManager.sceneLoaded +=
                HandleSceneLoaded;
        }

        private void Start()
        {
            HandleSceneLoaded(
                SceneManager.GetActiveScene(),
                LoadSceneMode.Single);
        }

        private void Update()
        {
            if (_operationalGate == null)
            {
                return;
            }

            _operationalGate.Tick(
                Time.unscaledDeltaTime);

            if (!_operationalGate
                    .TryConsumeControlledClosingRequest(
                        out string reason))
            {
                return;
            }

            UIRuntimeCompositionRoot root =
                UIRuntimeCompositionRoot.Instance;

            if (root == null ||
                !root.ActiveSession.HasActiveSession ||
                !string.Equals(
                    root.ActiveSession.Snapshot
                        .DayCycle.State,
                    "Open",
                    StringComparison.Ordinal))
            {
                return;
            }

            bool transitioned = root.TryTransitionDay(
                "Closing",
                out string transitionReason);
            string detail = transitioned
                ? reason
                : reason + " " + transitionReason;
            root.SetUserMessage(detail);

            HandleFeedback(
                new GameplayFeedbackEvent(
                    GameplayFeedbackType
                        .ClosingWarning,
                    detail,
                    "checkout-zone"));
        }

        private void OnDestroy()
        {
            if (Instance != this)
            {
                return;
            }

            SceneManager.sceneLoaded -=
                HandleSceneLoaded;
            CleanupStoreRuntime();
            _employeePersistence?.Dispose();
            _employeePersistence = null;
            Instance = null;
        }

        private void LoadAssets()
        {
            StoreRuntimeAssetRegistry
                registry =
                    StoreRuntimeAssetRegistry
                        .FindLoaded();

            if (registry == null)
            {
                Debug.LogError(
                    "[Runtime] Preloaded asset registry is missing.");
                return;
            }

            _settings = registry.Settings;
            _contentAsset =
                registry.ContentCatalog;
            _shellAsset = registry.StoreShell;
            _presentationAsset =
                registry.PresentationCatalog;
            _audioAsset = registry.AudioCatalog;
            _employeeHiringAsset =
                registry.EmployeeHiringCatalog;
        }

        private void HandleSceneLoaded(
            Scene scene,
            LoadSceneMode mode)
        {
            CleanupStoreRuntime();

            if (_settings == null ||
                scene.name != _settings.StoreSceneName)
            {
                return;
            }

            _initialization =
                StartCoroutine(
                    InitializeStoreRoutine());
        }

        private IEnumerator InitializeStoreRoutine()
        {
            while (UIRuntimeCompositionRoot
                       .Instance == null)
            {
                yield return null;
            }

            UIRuntimeCompositionRoot s15 =
                UIRuntimeCompositionRoot
                    .Instance;

            while (s15.ApplicationContext == null)
            {
                yield return null;
            }

            IGameApplicationContext application =
                s15.ApplicationContext;

            while (!application.ActiveSession
                        .HasActiveSession)
            {
                yield return null;
            }

            if (!ValidateAssets())
            {
                yield break;
            }

            _storeRuntime =
                new GameObject(
                    "S16_P1_Runtime");
            _storeRuntime.transform.SetParent(
                transform,
                false);

            StoreContentCatalog catalog =
                _contentAsset.BuildCatalog();

            JsonStoreOperationsStateRepository repository =
                new JsonStoreOperationsStateRepository(
                    Path.Combine(
                        UnityEngine.Application
                            .persistentDataPath,
                        "Sprint16Phase1"));

            _service =
                new StoreOperationsFacade(
                    catalog,
                    repository,
                    application.ActiveSession,
                    application.UtcClock,
                    application.SaveMutations);

            _service.InitializeForActiveSlot(
                s15.Slots.LastLoadRecoveredFromBackup);

            StoreEmployeeHiringAccess hiringAccess =
                new StoreEmployeeHiringAccess(
                    _service,
                    catalog);

            if (_employeeHiring == null)
            {
                _employeeHiring =
                    new EmployeeHiringService(
                        _employeeHiringAsset.BuildCatalog(),
                        application.ActiveSession,
                        application.SaveMutations,
                        application.UtcClock,
                        hiringAccess);
            }
            else
            {
                _employeeHiring.BindStoreAccess(
                    hiringAccess);
            }

            if (_employeeSchedule == null)
            {
                _employeeSchedule =
                    new EmployeeScheduleService(
                        _employeeHiring,
                        application.ActiveSession);
            }

            if (_employeeState == null)
            {
                _employeeState =
                    new EmployeeStateService(
                        _employeeHiring,
                        application.ActiveSession,
                        _employeeSchedule);
            }

            if (_employeePayroll == null)
            {
                _employeePayroll =
                    new EmployeePayrollService(
                        _employeeHiring,
                        application.ActiveSession,
                        application.SaveMutations,
                        application.UtcClock,
                        _employeeSchedule,
                        hiringAccess);
            }
            else
            {
                _employeePayroll.BindStoreAccess(
                    hiringAccess);
            }

            _employeeHiring.BindPayrollStatus(
                _employeePayroll);

            if (_employeePersistence == null)
            {
                _employeePersistence =
                    new EmployeePersistenceService(
                        _employeeHiring,
                        _employeeSchedule,
                        _employeePayroll,
                        application.ActiveSession,
                        application.SaveMutations,
                        application.UtcClock);
            }

            _employeePersistence.RestoreFromActiveSnapshot();

            if (_employeePresence == null)
            {
                _employeePresence =
                    new EmployeePresenceService(
                        _employeeHiring,
                        _employeeState);
            }

            _audio =
                _storeRuntime.AddComponent<
                    StoreAudioRouter>();
            _audio.Configure(_audioAsset);

            _feedback =
                _storeRuntime.AddComponent<
                    GameplayFeedbackPresenter>();
            _feedback.Configure(
                _presentationAsset,
                _audio,
                _settings.VfxPoolSize);

            _binder =
                _storeRuntime.AddComponent<
                    AuthoredStoreRuntimeBinder>();
            _binder.Configure(
                _shellAsset,
                _settings);

            StoreInitialSceneContext sceneContext =
                UnityEngine.Object.FindFirstObjectByType<
                    StoreInitialSceneContext>(
                        FindObjectsInactive.Include);

            if (sceneContext != null)
            {
                try
                {
                    _binder.Bind(
                        sceneContext);
                }
                catch (Exception exception)
                {
                    Debug.LogError(
                        "[Runtime] StoreInitial scene contract failed: " +
                        exception.Message);
                    yield break;
                }
            }
            else
            {
                Debug.LogError(
                    "[Runtime] StoreInitial has no StoreInitialSceneContext. " +
                    "The authored scene contract is required.");
                yield break;
            }

            _playerInteraction =
                sceneContext.TechnicalPlayer
                    .GetComponent<
                        PlayerWorldInteractionController>();

            if (_playerInteraction == null)
            {
                _playerInteraction =
                    sceneContext.TechnicalPlayer
                        .gameObject.AddComponent<
                            PlayerWorldInteractionController>();
            }

            _playerInteraction.Configure(
                sceneContext.GameplayCamera,
                Physics.DefaultRaycastLayers,
                500f);

            _playerInteraction.InteractionAttempted +=
                HandlePlayerInteractionAttempted;

            _playerToolWheelInput =
                sceneContext.TechnicalPlayer
                    .GetComponent<
                        PlayerToolWheelInputBridge>();

            if (_playerToolWheelInput == null)
            {
                _playerToolWheelInput =
                    sceneContext.TechnicalPlayer
                        .gameObject.AddComponent<
                            PlayerToolWheelInputBridge>();
            }

            _playerToolSelection =
                new PlayerToolSelectionService();

            _playerCarryLoad =
                new PlayerCarryLoadService();

            _physicalWork =
                new PhysicalWorkExecutionService();
            _physicalWork.RegisterHandler(
                new DelegatePhysicalWorkHandler(
                    PhysicalWorkKind.InspectTarget));

            _playerMovementInput =
                sceneContext.TechnicalPlayer
                    .GetComponent<
                        PlayerMovementInputBridge>();

            if (_playerMovementInput == null)
            {
                _playerMovementInput =
                    sceneContext.TechnicalPlayer
                        .gameObject.AddComponent<
                            PlayerMovementInputBridge>();
            }

            _playerMovementInput.Configure(
                _playerCarryLoad);

            _playerToolWheelPresenter =
                _storeRuntime.AddComponent<
                    PlayerToolWheelPresenter>();
            _playerToolWheelPresenter.Configure(
                _playerToolSelection,
                _playerToolWheelInput,
                s15.InputGate);

            _contextInspection =
                _storeRuntime.AddComponent<
                    ContextInspectionPanel>();
            _contextInspection.Configure(
                _service,
                catalog,
                application.ActiveSession,
                _employeeHiring,
                _employeeSchedule,
                _employeeState,
                s15.InputGate);

            _contextualWorldFeedback =
                _storeRuntime.AddComponent<
                    ContextualWorldFeedbackPresenter>();
            _contextualWorldFeedback.Configure(
                _service,
                catalog,
                application.ActiveSession,
                _employeeState,
                _playerInteraction,
                s15.InputGate,
                _playerToolWheelInput,
                sceneContext.GameplayCamera);

            _placement =
                _storeRuntime.AddComponent<
                    StorePlacementCoordinator>();
            _placement.Configure(
                _service,
                catalog,
                sceneContext);

            _navMesh =
                _storeRuntime.AddComponent<
                    DynamicStoreNavMeshController>();
            _navMesh.Configure(
                sceneContext.Navigation);

            _operationalGate =
                new StoreOperationalGate(
                    _service,
                    catalog,
                    _settings.MaximumCustomers,
                    application.ActiveSession);
            s15.RegisterStoreOperationalGate(
                _operationalGate);
            s15.RegisterStoreClosingEconomyService(
                _service);
            s15.RegisterEmployeePayrollClosingService(
                _employeePayroll);
            s15.RegisterStoreManagementStateProvider(
                _service);
            s15.RegisterManualSaveCheckpointParticipant(
                _service);

            _characters =
                _storeRuntime.AddComponent<
                    StoreCharacterLoopController>();
            _characters.Configure(
                _service,
                catalog,
                _presentationAsset,
                _binder.EntranceAnchor,
                _binder.CheckoutAnchor,
                _binder.ReceivingAnchor,
                _settings
                    .MaximumCustomers,
                sceneContext.CustomerSpawnAnchors,
                _operationalGate);

            _employeePresencePresenter =
                _storeRuntime.AddComponent<
                    EmployeePresencePresenter>();
            _employeePresencePresenter.Configure(
                _employeeHiring,
                _employeePresence,
                _employeeSchedule,
                _presentationAsset,
                _binder.BackroomAnchor);

            _inventoryVisuals =
                _storeRuntime.AddComponent<
                    InventoryVisualSynchronizer>();
            _inventoryVisuals.Configure(
                _service,
                catalog,
                _binder.BackroomAnchor);

            _operations =
                _storeRuntime.AddComponent<
                    StoreOperationsScreen>();
            _supplierDeliveries =
                _storeRuntime.AddComponent<SupplierDeliveryPresenter>();
            _supplierDeliveries.Configure(
                _presentationAsset,
                _service,
                _binder.EntranceAnchor,
                _binder.ReceivingAnchor);

            foreach (StoreDeliveryRunRecord run in _service.State.DeliveryRuns)
            {
                if (run.Status == StoreDeliveryRunStatus.InTransit)
                {
                    _supplierDeliveries.Present(run.DeliveryRunId);
                }
            }

            _operations.Configure(
                _service,
                catalog,
                new StoreOpeningProcedure(
                    catalog),
                _placement,
                _characters,
                _employeeHiring,
                _employeePayroll,
                _employeeSchedule,
                _employeeState,
                _employeePresence,
                _binder,
                _audio);

            RegisterFeedbackAnchors();

            _service.FeedbackRaised +=
                HandleFeedback;
            _placement.FeedbackRaised +=
                HandleFeedback;

            if (_binder.Door != null)
            {
                _binder.Door
                    .OpenStateChanged +=
                        HandleDoorStateChanged;
            }

            s15.Autosave.Completed +=
                HandleAutosaveCompleted;
            s15.ActiveSession.SnapshotChanged +=
                HandleSnapshotChanged;

            _audio.Play("music.store");
            _audio.Play("ambience.store");

            _feedback.Present(
                new GameplayFeedbackEvent(
                    GameplayFeedbackType
                        .ObjectSelected,
                    "Authored StoreInitial ready.",
                    "store-center"));
        }

        private bool ValidateAssets()
        {
            if (_settings != null &&
                _contentAsset != null &&
                _shellAsset != null &&
                _presentationAsset != null &&
                _audioAsset != null &&
                _employeeHiringAsset != null)
            {
                return true;
            }

            Debug.LogError(
                "[Runtime] Required project assets are missing.");
            return false;
        }

        private void RegisterFeedbackAnchors()
        {
            _feedback.RegisterAnchor(
                "store-entrance",
                _binder.EntranceAnchor);
            _feedback.RegisterAnchor(
                "receiving-zone",
                _binder.ReceivingAnchor);
            _feedback.RegisterAnchor(
                "checkout-zone",
                _binder.CheckoutAnchor);
            _feedback.RegisterAnchor(
                "store-center",
                _storeRuntime.transform);
            _feedback.RegisterAnchor(
                "placement-preview",
                _storeRuntime.transform);
            _feedback.RegisterAnchor(
                "cash-hud",
                _storeRuntime.transform);
            _feedback.RegisterAnchor(
                "customer",
                _binder.EntranceAnchor);
        }

        private void HandleFeedback(
            GameplayFeedbackEvent feedback)
        {
            _feedback?.Present(feedback);
            _inventoryVisuals?.Refresh();

            if (feedback.Kind ==
                GameplayFeedbackType.DeliveryRunStarted &&
                !string.IsNullOrWhiteSpace(feedback.CorrelationId))
            {
                _supplierDeliveries?.Present(feedback.CorrelationId);
            }
        }

        private void HandlePlayerInteractionAttempted(
            PlayerInteractionAttempt attempt)
        {
            if (attempt.Status ==
                PlayerInteractionAttemptStatus.NoTarget)
            {
                return;
            }

            string target =
                string.IsNullOrWhiteSpace(attempt.TargetId)
                    ? attempt.TargetKind.ToString()
                    : attempt.TargetKind +
                      " [" + attempt.TargetId + "]";

            switch (attempt.Status)
            {
                case PlayerInteractionAttemptStatus.Accepted:
                    IWorldInteractionTarget interactionTarget =
                        _playerInteraction == null
                            ? null
                            : _playerInteraction.CurrentTarget;

                    string workFailure = string.Empty;

                    if (interactionTarget != null &&
                        _contextInspection != null &&
                        TryExecuteInspectionWork(
                            interactionTarget,
                            attempt.Distance,
                            out workFailure))
                    {
                        break;
                    }

                    if (!string.IsNullOrWhiteSpace(workFailure))
                    {
                        HandleFeedback(
                            new GameplayFeedbackEvent(
                                GameplayFeedbackType.ObjectHovered,
                                workFailure));
                        break;
                    }

                    HandleFeedback(
                        new GameplayFeedbackEvent(
                            GameplayFeedbackType.ObjectSelected,
                            "Interaction ready: " + target + "."));
                    break;

                case PlayerInteractionAttemptStatus.OutOfRange:
                    HandleFeedback(
                        new GameplayFeedbackEvent(
                            GameplayFeedbackType.ObjectHovered,
                            "Move closer to interact with " +
                            target + "."));
                    break;

                case PlayerInteractionAttemptStatus.Unavailable:
                    HandleFeedback(
                        new GameplayFeedbackEvent(
                            GameplayFeedbackType.ObjectHovered,
                            target + " is not interactive."));
                    break;
            }
        }

        private bool TryExecuteInspectionWork(
            IWorldInteractionTarget interactionTarget,
            float distance,
            out string failure)
        {
            failure = string.Empty;

            if (_physicalWork == null ||
                interactionTarget == null ||
                _contextInspection == null)
            {
                return false;
            }

            PhysicalWorkRequest request;

            try
            {
                request = new PhysicalWorkRequest(
                    PhysicalWorkKind.InspectTarget,
                    PhysicalWorkActorRef.Player,
                    new PhysicalWorkTargetRef(
                        interactionTarget.InteractionKind.ToString(),
                        interactionTarget.InteractionId));
            }
            catch (Exception exception)
            {
                failure =
                    "Interaction cannot start: " +
                    exception.Message;
                return false;
            }

            PhysicalWorkBeginResult begin =
                _physicalWork.TryBegin(request);

            if (!begin.Started || begin.Execution == null)
            {
                failure = string.IsNullOrWhiteSpace(begin.Reason)
                    ? "Interaction cannot start."
                    : begin.Reason;
                return false;
            }

            try
            {
                _contextInspection.Open(
                    interactionTarget,
                    distance);

                PhysicalWorkTransitionResult complete =
                    _physicalWork.TryComplete(
                        begin.Execution.WorkId);

                if (!complete.Succeeded)
                {
                    _physicalWork.TryCancel(
                        begin.Execution.WorkId);
                    failure = string.IsNullOrWhiteSpace(complete.Reason)
                        ? "Interaction could not complete."
                        : complete.Reason;
                    return false;
                }

                return true;
            }
            catch (Exception exception)
            {
                _physicalWork.TryCancel(
                    begin.Execution.WorkId);
                failure =
                    "Interaction failed: " +
                    exception.Message;
                return false;
            }
        }

        private void HandleDoorStateChanged(
            bool isOpen)
        {
            HandleFeedback(
                new GameplayFeedbackEvent(
                    isOpen
                        ? GameplayFeedbackType
                            .DoorOpened
                        : GameplayFeedbackType
                            .DoorClosed,
                    isOpen
                        ? "Automatic door opened."
                        : "Automatic door closed.",
                    "store-entrance"));
        }

        private void HandleAutosaveCompleted(
            DailyAutosaveResult result)
        {
            if (_service == null)
            {
                return;
            }

            bool succeeded =
                result.Status ==
                    DailyAutosaveStatus.Saved ||
                result.Status ==
                    DailyAutosaveStatus
                        .AlreadySaved;

            if (succeeded)
            {
                _operations
                    ?.SetAutosaveCompleted(
                        true);

                HandleFeedback(
                    new GameplayFeedbackEvent(
                        GameplayFeedbackType
                            .AutosaveSucceeded,
                        "Vertical slice state saved.",
                        "cash-hud"));
            }
            else
            {
                _operations
                    ?.SetAutosaveCompleted(false);

                HandleFeedback(
                    new GameplayFeedbackEvent(
                        GameplayFeedbackType
                            .AutosaveFailed,
                        result.Detail,
                        "cash-hud"));
            }
        }

        private void HandleSnapshotChanged(
            IntegratedGameStateSnapshot snapshot)
        {
            if (snapshot == null)
            {
                return;
            }

            _service?.SynchronizeManagementDay();
            _employeeHiring?.RefreshForCurrentDay();

            string state = snapshot.DayCycle.State;
            bool stateChanged = !string.Equals(
                _lastObservedDayState,
                state,
                StringComparison.Ordinal);
            _lastObservedDayState = state;

            if (!string.Equals(
                    state,
                    "Closed",
                    StringComparison.Ordinal))
            {
                _operations?.SetAutosaveCompleted(false);
            }

            if (!stateChanged)
            {
                return;
            }

            if (string.Equals(state, "Closing", StringComparison.Ordinal))
            {
                HandleFeedback(
                    new GameplayFeedbackEvent(
                        GameplayFeedbackType.ClosingWarning,
                        "Store is closing.",
                        "store-entrance"));
            }
            else if (string.Equals(state, "Closed", StringComparison.Ordinal))
            {
                HandleFeedback(
                    new GameplayFeedbackEvent(
                        GameplayFeedbackType.DayClosed,
                        StoreTradingHoursPolicy.IsDayComplete(
                            snapshot.DayCycle.ElapsedDaySeconds,
                            snapshot.DayCycle.DayDurationSeconds)
                            ? "Day closed at 24:00."
                            : "Store closed. It may reopen before 22:00.",
                        "checkout-zone"));
            }
        }

        private void CleanupStoreRuntime()
        {
            if (_initialization != null)
            {
                StopCoroutine(_initialization);
                _initialization = null;
            }

            UIRuntimeCompositionRoot s15 =
                UIRuntimeCompositionRoot
                    .Instance;

            if (s15 != null)
            {
                s15.Autosave.Completed -=
                    HandleAutosaveCompleted;
                s15.ActiveSession.SnapshotChanged -=
                    HandleSnapshotChanged;

                if (_operationalGate != null)
                {
                    s15.UnregisterStoreOperationalGate(
                        _operationalGate);
                }

                if (_employeePayroll != null)
                {
                    s15.UnregisterEmployeePayrollClosingService(
                        _employeePayroll);
                }

                if (_service != null)
                {
                    s15.UnregisterStoreClosingEconomyService(
                        _service);
                    s15.UnregisterStoreManagementStateProvider(
                        _service);
                    s15.UnregisterManualSaveCheckpointParticipant(
                        _service);
                }
            }

            _employeeHiring?.DetachStoreAccess();
            _employeePayroll?.DetachStoreAccess();
            _operationalGate?.ClearCustomers();

            if (_service != null)
            {
                _service.FeedbackRaised -=
                    HandleFeedback;
            }

            if (_placement != null)
            {
                _placement.FeedbackRaised -=
                    HandleFeedback;
            }

            if (_binder != null &&
                _binder.Door != null)
            {
                _binder.Door
                    .OpenStateChanged -=
                        HandleDoorStateChanged;
            }

            if (_playerInteraction != null)
            {
                _playerInteraction.InteractionAttempted -=
                    HandlePlayerInteractionAttempted;
            }

            if (_storeRuntime != null)
            {
                Destroy(_storeRuntime);
            }

            _storeRuntime = null;
            _service = null;
            _feedback = null;
            _placement = null;
            _characters = null;
            _binder = null;
            _operations = null;
            _audio = null;
            _inventoryVisuals = null;
            _navMesh = null;
            _supplierDeliveries = null;
            _employeePresencePresenter = null;
            _contextInspection = null;
            _contextualWorldFeedback = null;
            _playerToolWheelPresenter = null;
            _playerToolWheelInput = null;
            _playerMovementInput = null;
            _playerCarryLoad = null;
            _physicalWork = null;
            _playerToolSelection = null;
            _playerInteraction = null;
            _operationalGate = null;
        }
    }
}
