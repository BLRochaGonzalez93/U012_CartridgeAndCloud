using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRMGames.CartridgeAndCloud.Application.GameSession;
using VRMGames.CartridgeAndCloud.Application.InputContexts;
using VRMGames.CartridgeAndCloud.Application.SceneFlow;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;
using VRMGames.CartridgeAndCloud.Infrastructure.GameSession;

namespace VRMGames.CartridgeAndCloud.Infrastructure.SceneFlow
{
    /// <summary>
    /// Persistent composition root for scene flow, game session and input context.
    /// </summary>
    public sealed class ApplicationRoot : MonoBehaviour, ISceneNavigator
    {
        public const string RootObjectName = "ApplicationRoot";

        private readonly SceneTransitionGate _transitionGate =
            new SceneTransitionGate();

        public static ApplicationRoot Instance { get; private set; }

        public bool IsTransitioning => _transitionGate.IsEntered;

        public SceneId ActiveScene =>
            ResolveSceneId(SceneManager.GetActiveScene().name);

        public IGameSessionService GameSessionService { get; private set; }

        public IInputContextService InputContextService { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            gameObject.name = RootObjectName;
            DontDestroyOnLoad(gameObject);

            GameSessionService = new GameSessionService(
                JsonSaveGameRepository.CreateDefault(),
                new SystemUtcClock());

            GameSessionService.StartNew(new SaveSlotId(0));
            InputContextService = new InputContextService();

            SceneManager.sceneLoaded += HandleSceneLoaded;
        }

        private void Start()
        {
            Scene activeScene = SceneManager.GetActiveScene();

            ApplySceneInputContext(activeScene);
            InjectSceneConsumers(activeScene);

            if (activeScene.name == GetSceneName(SceneId.Bootstrap))
            {
                RequestLoad(SceneId.MainMenu);
            }
        }

        private void OnDestroy()
        {
            if (Instance != this)
            {
                return;
            }

            SceneManager.sceneLoaded -= HandleSceneLoaded;
            Instance = null;
        }

        public SceneTransitionRequestResult RequestLoad(SceneId sceneId)
        {
            string targetSceneName = GetSceneName(sceneId);

            if (string.IsNullOrEmpty(targetSceneName))
            {
                return SceneTransitionRequestResult.UnknownScene;
            }

            if (SceneManager.GetActiveScene().name == targetSceneName)
            {
                return SceneTransitionRequestResult.AlreadyActive;
            }

            if (!_transitionGate.TryEnter())
            {
                return SceneTransitionRequestResult.TransitionInProgress;
            }

            StartCoroutine(LoadSceneRoutine(targetSceneName));
            return SceneTransitionRequestResult.Accepted;
        }

        public void Quit()
        {
#if UNITY_EDITOR
            Debug.Log(
                "[SceneFlow] Quit requested. Application.Quit is ignored inside the Unity Editor.");
#else
            UnityEngine.Application.Quit();
#endif
        }

        private IEnumerator LoadSceneRoutine(string targetSceneName)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(
                targetSceneName,
                LoadSceneMode.Single);

            if (operation == null)
            {
                Debug.LogError(
                    $"[SceneFlow] Unity could not start loading scene '{targetSceneName}'.");
                _transitionGate.Exit();
                yield break;
            }

            while (!operation.isDone)
            {
                yield return null;
            }

            _transitionGate.Exit();
        }

        private void HandleSceneLoaded(
            Scene scene,
            LoadSceneMode loadSceneMode)
        {
            ApplySceneInputContext(scene);
            InjectSceneConsumers(scene);
        }

        private void ApplySceneInputContext(Scene scene)
        {
            InputContextId context = ResolveInputContext(scene.name);
            InputContextService.SetContext(context);
        }

        private void InjectSceneConsumers(Scene scene)
        {
            if (!scene.IsValid() || !scene.isLoaded)
            {
                return;
            }

            GameObject[] roots = scene.GetRootGameObjects();

            foreach (GameObject root in roots)
            {
                MonoBehaviour[] behaviours =
                    root.GetComponentsInChildren<MonoBehaviour>(true);

                foreach (MonoBehaviour behaviour in behaviours)
                {
                    if (behaviour is ISceneNavigationConsumer sceneConsumer)
                    {
                        sceneConsumer.Initialize(this);
                    }

                    if (behaviour is IGameSessionConsumer sessionConsumer)
                    {
                        sessionConsumer.Initialize(GameSessionService);
                    }

                    if (behaviour is IInputContextConsumer inputConsumer)
                    {
                        inputConsumer.Initialize(InputContextService);
                    }
                }
            }
        }

        private static InputContextId ResolveInputContext(string sceneName)
        {
            switch (sceneName)
            {
                case "MainMenu":
                    return InputContextId.UI;
                case "Store":
                case "TestLab":
                    return InputContextId.Gameplay;
                default:
                    return InputContextId.None;
            }
        }

        private static string GetSceneName(SceneId sceneId)
        {
            switch (sceneId)
            {
                case SceneId.Bootstrap:
                    return "Bootstrap";
                case SceneId.MainMenu:
                    return "MainMenu";
                case SceneId.Store:
                    return "Store";
                case SceneId.TestLab:
                    return "TestLab";
                default:
                    return string.Empty;
            }
        }

        private static SceneId ResolveSceneId(string sceneName)
        {
            switch (sceneName)
            {
                case "MainMenu":
                    return SceneId.MainMenu;
                case "Store":
                    return SceneId.Store;
                case "TestLab":
                    return SceneId.TestLab;
                default:
                    return SceneId.Bootstrap;
            }
        }
    }
}
