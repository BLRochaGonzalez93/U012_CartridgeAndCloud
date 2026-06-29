using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRMGames.CartridgeAndCloud.Application.UIUX;
using VRMGames.CartridgeAndCloud.Application.VerticalSlicePhase1;
using VRMGames.CartridgeAndCloud.Domain.Persistence;
using VRMGames.CartridgeAndCloud.Domain.VerticalSlicePhase1;
using VRMGames.CartridgeAndCloud.Infrastructure.UIUX;
using VRMGames.CartridgeAndCloud.Infrastructure.VerticalSlicePhase1;

namespace VRMGames.CartridgeAndCloud.Runtime.VerticalSlicePhase1
{
    [DefaultExecutionOrder(-9900)]
    public sealed class Sprint16Phase1RuntimeRoot :
        MonoBehaviour
    {
        private const string SettingsPath =
            "Sprint16Phase1/CC_S16_P1_Settings";
        private const string ContentPath =
            "Sprint16Phase1/CC_S16_P1_ContentCatalog";
        private const string ShellPath =
            "Sprint16Phase1/CC_S16_P1_StoreShell";
        private const string PalettePath =
            "Sprint16Phase1/CC_S16_P1_MaterialPalette";
        private const string PresentationPath =
            "Sprint16Phase1/CC_S16_P1_PresentationCatalog";
        private const string AudioPath =
            "Sprint16Phase1/CC_S16_P1_AudioCatalog";

        public static Sprint16Phase1RuntimeRoot
            Instance { get; private set; }

        private Phase1SettingsAsset _settings;
        private Phase1ContentCatalogAsset
            _contentAsset;
        private Phase1StoreShellAsset _shellAsset;
        private Phase1MaterialPaletteAsset
            _paletteAsset;
        private Phase1PresentationCatalogAsset
            _presentationAsset;
        private Phase1AudioCatalogAsset
            _audioAsset;

        private GameObject _storeRuntime;
        private Coroutine _initialization;
        private Phase1VerticalSliceService _service;
        private Phase1FeedbackPresenter _feedback;
        private Phase1PlacementCompatibilityBridge
            _placement;
        private Phase1CharacterLoopController
            _characters;
        private Phase1StoreBlockoutBuilder
            _blockout;
        private Phase1OperationsScreen _operations;
        private Phase1AudioRouter _audio;
        private Phase1InventoryVisualSynchronizer
            _inventoryVisuals;

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
                    "Sprint16Phase1RuntimeRoot");

            root.AddComponent<
                Sprint16Phase1RuntimeRoot>();
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

        private void OnDestroy()
        {
            if (Instance != this)
            {
                return;
            }

            SceneManager.sceneLoaded -=
                HandleSceneLoaded;
            CleanupStoreRuntime();
            Instance = null;
        }

        private void LoadAssets()
        {
            _settings =
                Resources.Load<
                    Phase1SettingsAsset>(
                        SettingsPath);
            _contentAsset =
                Resources.Load<
                    Phase1ContentCatalogAsset>(
                        ContentPath);
            _shellAsset =
                Resources.Load<
                    Phase1StoreShellAsset>(
                        ShellPath);
            _paletteAsset =
                Resources.Load<
                    Phase1MaterialPaletteAsset>(
                        PalettePath);
            _presentationAsset =
                Resources.Load<
                    Phase1PresentationCatalogAsset>(
                        PresentationPath);
            _audioAsset =
                Resources.Load<
                    Phase1AudioCatalogAsset>(
                        AudioPath);
        }

        private void HandleSceneLoaded(
            Scene scene,
            LoadSceneMode mode)
        {
            CleanupStoreRuntime();

            if (_settings == null ||
                scene.name !=
                    _settings.StoreSceneName)
            {
                return;
            }

            _initialization =
                StartCoroutine(
                    InitializeStoreRoutine());
        }

        private IEnumerator InitializeStoreRoutine()
        {
            while (Sprint15RuntimeCompositionRoot
                       .Instance == null)
            {
                yield return null;
            }

            Sprint15RuntimeCompositionRoot s15 =
                Sprint15RuntimeCompositionRoot
                    .Instance;

            while (!s15.ActiveSession
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

            Phase1Catalog catalog =
                _contentAsset.BuildCatalog();

            JsonPhase1StateRepository repository =
                new JsonPhase1StateRepository(
                    Path.Combine(
                        UnityEngine.Application
                            .persistentDataPath,
                        "Sprint16Phase1"));

            _service =
                new Phase1VerticalSliceService(
                    catalog,
                    repository,
                    s15.ActiveSession,
                    new SystemPhase1UtcClock());

            _service.InitializeForActiveSlot();

            _audio =
                _storeRuntime.AddComponent<
                    Phase1AudioRouter>();
            _audio.Configure(_audioAsset);

            _feedback =
                _storeRuntime.AddComponent<
                    Phase1FeedbackPresenter>();
            _feedback.Configure(
                _presentationAsset,
                _audio,
                _settings.VfxPoolSize);

            _blockout =
                _storeRuntime.AddComponent<
                    Phase1StoreBlockoutBuilder>();
            _blockout.Configure(
                _shellAsset,
                _paletteAsset,
                _settings);

            if (_settings.BuildBlockoutOnLoad)
            {
                _blockout.Build();
            }

            _placement =
                _storeRuntime.AddComponent<
                    Phase1PlacementCompatibilityBridge>();
            _placement.Configure(
                _service,
                catalog,
                _paletteAsset);

            _characters =
                _storeRuntime.AddComponent<
                    Phase1CharacterLoopController>();
            _characters.Configure(
                _service,
                catalog,
                _paletteAsset,
                _blockout.EntranceAnchor,
                _blockout.CheckoutAnchor,
                _blockout.ReceivingAnchor,
                _settings
                    .MaximumBlockoutCustomers);

            _inventoryVisuals =
                _storeRuntime.AddComponent<
                    Phase1InventoryVisualSynchronizer>();
            _inventoryVisuals.Configure(
                _service,
                catalog,
                _paletteAsset,
                _blockout.BackroomAnchor);

            _operations =
                _storeRuntime.AddComponent<
                    Phase1OperationsScreen>();
            _operations.Configure(
                _service,
                catalog,
                new Phase1PlayableProcedure(
                    catalog),
                _placement,
                _characters,
                _blockout,
                _audio);

            RegisterFeedbackAnchors();

            _service.FeedbackRaised +=
                HandleFeedback;
            _placement.FeedbackRaised +=
                HandleFeedback;

            if (_blockout.Door != null)
            {
                _blockout.Door
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
                new Phase1FeedbackEvent(
                    Phase1FeedbackKind
                        .ObjectSelected,
                    "Playable blockout ready.",
                    "store-center"));
        }

        private bool ValidateAssets()
        {
            if (_settings != null &&
                _contentAsset != null &&
                _shellAsset != null &&
                _paletteAsset != null &&
                _presentationAsset != null &&
                _audioAsset != null)
            {
                return true;
            }

            Debug.LogError(
                "[Sprint16 Phase1] Required Resources assets are missing.");
            return false;
        }

        private void RegisterFeedbackAnchors()
        {
            _feedback.RegisterAnchor(
                "store-entrance",
                _blockout.EntranceAnchor);
            _feedback.RegisterAnchor(
                "receiving-zone",
                _blockout.ReceivingAnchor);
            _feedback.RegisterAnchor(
                "checkout-zone",
                _blockout.CheckoutAnchor);
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
                _blockout.EntranceAnchor);
        }

        private void HandleFeedback(
            Phase1FeedbackEvent feedback)
        {
            _feedback?.Present(feedback);
            _inventoryVisuals?.Refresh();

            if (feedback.Kind ==
                Phase1FeedbackKind.OrderReceived)
            {
                _characters
                    ?.PresentSupplierDelivery();
            }
        }

        private void HandleDoorStateChanged(
            bool isOpen)
        {
            HandleFeedback(
                new Phase1FeedbackEvent(
                    isOpen
                        ? Phase1FeedbackKind
                            .DoorOpened
                        : Phase1FeedbackKind
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
                try
                {
                    _service.SaveCheckpoint();

                    _operations
                        ?.SetAutosaveCompleted(
                            true);

                    HandleFeedback(
                        new Phase1FeedbackEvent(
                            Phase1FeedbackKind
                                .AutosaveSucceeded,
                            "Vertical slice state saved.",
                            "cash-hud"));
                }
                catch (Exception exception)
                {
                    HandleFeedback(
                        new Phase1FeedbackEvent(
                            Phase1FeedbackKind
                                .AutosaveFailed,
                            "Phase 1 sidecar save failed: " +
                            exception.Message,
                            "cash-hud"));
                }
            }
            else
            {
                _operations
                    ?.SetAutosaveCompleted(false);

                HandleFeedback(
                    new Phase1FeedbackEvent(
                        Phase1FeedbackKind
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

            if (!string.Equals(
                    snapshot.DayCycle.State,
                    "Closed",
                    StringComparison.Ordinal))
            {
                _operations
                    ?.SetAutosaveCompleted(false);
            }

            if (string.Equals(
                    snapshot.DayCycle.State,
                    "Closing",
                    StringComparison.Ordinal))
            {
                HandleFeedback(
                    new Phase1FeedbackEvent(
                        Phase1FeedbackKind
                            .ClosingWarning,
                        "Store is closing.",
                        "store-entrance"));
            }
            else if (string.Equals(
                         snapshot.DayCycle.State,
                         "Closed",
                         StringComparison.Ordinal))
            {
                HandleFeedback(
                    new Phase1FeedbackEvent(
                        Phase1FeedbackKind
                            .DayClosed,
                        "Day closed.",
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

            Sprint15RuntimeCompositionRoot s15 =
                Sprint15RuntimeCompositionRoot
                    .Instance;

            if (s15 != null)
            {
                s15.Autosave.Completed -=
                    HandleAutosaveCompleted;
                s15.ActiveSession.SnapshotChanged -=
                    HandleSnapshotChanged;
            }

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

            if (_blockout != null &&
                _blockout.Door != null)
            {
                _blockout.Door
                    .OpenStateChanged -=
                        HandleDoorStateChanged;
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
            _blockout = null;
            _operations = null;
            _audio = null;
            _inventoryVisuals = null;
        }
    }
}
