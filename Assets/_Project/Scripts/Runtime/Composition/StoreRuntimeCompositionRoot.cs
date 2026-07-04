using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRMGames.CartridgeAndCloud.Application.UIUX;
using VRMGames.CartridgeAndCloud.Domain.Persistence;
using VRMGames.CartridgeAndCloud.Infrastructure.UIUX;
using VRMGames.CartridgeAndCloud.Application.Store;
using VRMGames.CartridgeAndCloud.Infrastructure.Audio;
using VRMGames.CartridgeAndCloud.Infrastructure.GameSession;
using VRMGames.CartridgeAndCloud.Infrastructure.Persistence;
using VRMGames.CartridgeAndCloud.Infrastructure.Store;
using VRMGames.CartridgeAndCloud.Runtime.Audio;
using VRMGames.CartridgeAndCloud.Runtime.Characters;
using VRMGames.CartridgeAndCloud.Runtime.Development.Blockout;
using VRMGames.CartridgeAndCloud.Runtime.Inventory;
using VRMGames.CartridgeAndCloud.Runtime.Placement;
using VRMGames.CartridgeAndCloud.Runtime.UIUX;
using VRMGames.CartridgeAndCloud.Domain.GameSession;
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
        private StoreMaterialPaletteAsset
            _paletteAsset;
        private StorePresentationCatalogAsset
            _presentationAsset;
        private AudioEventCatalogAsset
            _audioAsset;

        private GameObject _storeRuntime;
        private Coroutine _initialization;
        private StoreOperationsFacade _service;
        private GameplayFeedbackPresenter _feedback;
        private StorePlacementCoordinator
            _placement;
        private StoreCharacterLoopController
            _characters;
        private StoreBlockoutBuilder
            _blockout;
        private StoreOperationsScreen _operations;
        private StoreAudioRouter _audio;
        private InventoryVisualSynchronizer
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
            _paletteAsset =
                registry.MaterialPalette;
            _presentationAsset =
                registry.PresentationCatalog;
            _audioAsset = registry.AudioCatalog;
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
            while (UIRuntimeCompositionRoot
                       .Instance == null)
            {
                yield return null;
            }

            UIRuntimeCompositionRoot s15 =
                UIRuntimeCompositionRoot
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
                    s15.ActiveSession,
                    new SystemUtcClock());

            _service.InitializeForActiveSlot();

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

            _blockout =
                _storeRuntime.AddComponent<
                    StoreBlockoutBuilder>();
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
                    StorePlacementCoordinator>();
            _placement.Configure(
                _service,
                catalog,
                _paletteAsset);

            _characters =
                _storeRuntime.AddComponent<
                    StoreCharacterLoopController>();
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
                    InventoryVisualSynchronizer>();
            _inventoryVisuals.Configure(
                _service,
                catalog,
                _paletteAsset,
                _blockout.BackroomAnchor);

            _operations =
                _storeRuntime.AddComponent<
                    StoreOperationsScreen>();
            _operations.Configure(
                _service,
                catalog,
                new StoreOpeningProcedure(
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
                new GameplayFeedbackEvent(
                    GameplayFeedbackType
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
                "[Runtime] Required project assets are missing.");
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
            GameplayFeedbackEvent feedback)
        {
            _feedback?.Present(feedback);
            _inventoryVisuals?.Refresh();

            if (feedback.Kind ==
                GameplayFeedbackType.OrderReceived)
            {
                _characters
                    ?.PresentSupplierDelivery();
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
                try
                {
                    _service.SaveCheckpoint();

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
                catch (Exception exception)
                {
                    HandleFeedback(
                        new GameplayFeedbackEvent(
                            GameplayFeedbackType
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
                    new GameplayFeedbackEvent(
                        GameplayFeedbackType
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
                    new GameplayFeedbackEvent(
                        GameplayFeedbackType
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

            UIRuntimeCompositionRoot s15 =
                UIRuntimeCompositionRoot
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
