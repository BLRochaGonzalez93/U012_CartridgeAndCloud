using UnityEngine;
using UnityCamera = UnityEngine.Camera;
using VRMGames.CartridgeAndCloud.Application.InputContexts;

namespace VRMGames.CartridgeAndCloud.Presentation.PlayerMovement
{
    [RequireComponent(typeof(ClickToMoveAgent))]
    public sealed class ClickDestinationInput : MonoBehaviour, IInputContextConsumer
    {
        [SerializeField] private UnityCamera _worldCamera;
        [SerializeField] private LayerMask _walkableLayers = ~0;
        [SerializeField, Min(0.01f)] private float _maxRayDistance = 500f;
        [SerializeField] private bool _allowStandaloneGameplay;

        private ClickToMoveAgent _agent;
        private IInputContextService _inputContextService;

        public bool IsInputEnabled =>
            _inputContextService != null
                ? _inputContextService.CurrentContext == InputContextId.Gameplay
                : _allowStandaloneGameplay;

        private void Awake()
        {
            _agent = GetComponent<ClickToMoveAgent>();
        }

        public void Initialize(IInputContextService inputContextService)
        {
            _inputContextService = inputContextService;
        }

        public void Configure(
            UnityCamera worldCamera,
            LayerMask walkableLayers,
            float maxRayDistance,
            bool allowStandaloneGameplay)
        {
            _worldCamera = worldCamera;
            _walkableLayers = walkableLayers;
            _maxRayDistance = Mathf.Max(0.01f, maxRayDistance);
            _allowStandaloneGameplay = allowStandaloneGameplay;
        }

        public bool TrySetDestinationFromScreenPosition(Vector2 screenPosition)
        {
            if (!IsInputEnabled) return false;

            EnsureAgent();

            UnityCamera activeCamera =
                _worldCamera != null
                    ? _worldCamera
                    : UnityCamera.main;

            if (activeCamera == null) return false;

            Ray ray = activeCamera.ScreenPointToRay(screenPosition);

            if (!Physics.Raycast(
                    ray,
                    out RaycastHit hit,
                    _maxRayDistance,
                    _walkableLayers,
                    QueryTriggerInteraction.Ignore))
            {
                return false;
            }

            _agent.SetDestination(hit.point);
            return true;
        }

        private void EnsureAgent()
        {
            if (_agent == null)
            {
                _agent = GetComponent<ClickToMoveAgent>();
            }
        }
    }
}
