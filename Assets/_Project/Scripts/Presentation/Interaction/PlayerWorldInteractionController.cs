using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityCamera = UnityEngine.Camera;

namespace VRMGames.CartridgeAndCloud.Presentation.Interaction
{
    [DisallowMultipleComponent]
    public sealed class PlayerWorldInteractionController :
        MonoBehaviour
    {
        private const int RaycastBufferSize = 64;

        [SerializeField]
        private UnityCamera _worldCamera;

        [SerializeField]
        private LayerMask _targetLayers = ~0;

        [SerializeField, Min(0.1f)]
        private float _maxRayDistance = 500f;

        private readonly RaycastHit[] _raycastHits =
            new RaycastHit[RaycastBufferSize];

        private IWorldInteractionTarget _currentTarget;
        private float _currentDistance;

        public IWorldInteractionTarget CurrentTarget =>
            IsTargetAlive(_currentTarget)
                ? _currentTarget
                : null;

        public string CurrentTargetId =>
            CurrentTarget?.InteractionId ?? string.Empty;

        public WorldInteractionKind CurrentTargetKind =>
            CurrentTarget?.InteractionKind ??
            WorldInteractionKind.Unknown;

        public float CurrentDistance =>
            Mathf.Max(0f, _currentDistance);

        public bool IsCurrentTargetInRange
        {
            get
            {
                IWorldInteractionTarget target =
                    CurrentTarget;

                return target != null &&
                       target.IsInteractionAvailable &&
                       CurrentDistance <=
                           target.InteractionRange;
            }
        }

        public event Action<IWorldInteractionTarget>
            TargetChanged;

        public event Action<PlayerInteractionAttempt>
            InteractionAttempted;

        public void Configure(
            UnityCamera worldCamera,
            LayerMask targetLayers,
            float maxRayDistance)
        {
            _worldCamera = worldCamera;
            _targetLayers = targetLayers;
            _maxRayDistance =
                Mathf.Max(0.1f, maxRayDistance);
        }

        public void UpdateTargetFromScreenPosition(
            Vector2 screenPosition)
        {
            if (IsPointerOverUi())
            {
                ClearTarget();
                return;
            }

            UnityCamera activeCamera =
                _worldCamera != null
                    ? _worldCamera
                    : UnityCamera.main;

            if (activeCamera == null)
            {
                ClearTarget();
                return;
            }

            Ray ray =
                activeCamera.ScreenPointToRay(
                    screenPosition);

            int hitCount = Physics.RaycastNonAlloc(
                ray,
                _raycastHits,
                _maxRayDistance,
                _targetLayers,
                QueryTriggerInteraction.Collide);

            IWorldInteractionTarget bestTarget = null;
            float bestHitDistance =
                float.PositiveInfinity;
            int bestPriority = int.MinValue;

            for (int index = 0;
                 index < hitCount;
                 index++)
            {
                RaycastHit hit = _raycastHits[index];
                IWorldInteractionTarget candidate =
                    ResolveTarget(hit.transform);

                if (!IsTargetAlive(candidate) ||
                    IsOwnedByPlayer(candidate))
                {
                    continue;
                }

                int priority =
                    candidate.InteractionPriority;

                bool nearer =
                    hit.distance <
                    bestHitDistance - 0.001f;

                bool equivalentDistance =
                    Mathf.Abs(
                        hit.distance -
                        bestHitDistance) <= 0.001f;

                if (!nearer &&
                    !(equivalentDistance &&
                      priority > bestPriority))
                {
                    continue;
                }

                bestTarget = candidate;
                bestHitDistance = hit.distance;
                bestPriority = priority;
            }

            SetCurrentTarget(bestTarget);
            RefreshCurrentDistance();
        }

        public PlayerInteractionAttempt
            TryRequestCurrentInteraction()
        {
            IWorldInteractionTarget target =
                CurrentTarget;

            if (target == null)
            {
                return PublishAttempt(
                    new PlayerInteractionAttempt(
                        PlayerInteractionAttemptStatus.NoTarget,
                        string.Empty,
                        WorldInteractionKind.Unknown,
                        0f,
                        0f));
            }

            RefreshCurrentDistance();

            if (!target.IsInteractionAvailable)
            {
                return PublishAttempt(
                    BuildAttempt(
                        PlayerInteractionAttemptStatus.Unavailable,
                        target));
            }

            if (_currentDistance >
                target.InteractionRange)
            {
                return PublishAttempt(
                    BuildAttempt(
                        PlayerInteractionAttemptStatus.OutOfRange,
                        target));
            }

            return PublishAttempt(
                BuildAttempt(
                    PlayerInteractionAttemptStatus.Accepted,
                    target));
        }

        public void ClearTarget()
        {
            SetCurrentTarget(null);
            _currentDistance = 0f;
        }

        private void RefreshCurrentDistance()
        {
            IWorldInteractionTarget target =
                CurrentTarget;

            if (target == null)
            {
                _currentDistance = 0f;
                return;
            }

            Vector3 point =
                target.GetInteractionPoint(
                    transform.position);

            _currentDistance =
                WorldInteractionGeometry.PlanarDistance(
                    transform.position,
                    point);
        }

        private PlayerInteractionAttempt BuildAttempt(
            PlayerInteractionAttemptStatus status,
            IWorldInteractionTarget target)
        {
            return new PlayerInteractionAttempt(
                status,
                target.InteractionId,
                target.InteractionKind,
                _currentDistance,
                target.InteractionRange);
        }

        private PlayerInteractionAttempt PublishAttempt(
            PlayerInteractionAttempt attempt)
        {
            InteractionAttempted?.Invoke(attempt);
            return attempt;
        }

        private void SetCurrentTarget(
            IWorldInteractionTarget target)
        {
            if (ReferenceEquals(
                    _currentTarget,
                    target))
            {
                return;
            }

            _currentTarget = target;
            TargetChanged?.Invoke(CurrentTarget);
        }

        private IWorldInteractionTarget ResolveTarget(
            Transform hitTransform)
        {
            IWorldInteractionTarget best = null;
            int bestPriority = int.MinValue;
            int bestDepth = int.MaxValue;
            int depth = 0;

            Transform current = hitTransform;

            while (current != null)
            {
                MonoBehaviour[] behaviours =
                    current.GetComponents<MonoBehaviour>();

                foreach (MonoBehaviour behaviour
                         in behaviours)
                {
                    if (!(behaviour is
                          IWorldInteractionTarget candidate))
                    {
                        continue;
                    }

                    int priority =
                        candidate.InteractionPriority;

                    if (best == null ||
                        priority > bestPriority ||
                        (priority == bestPriority &&
                         depth < bestDepth))
                    {
                        best = candidate;
                        bestPriority = priority;
                        bestDepth = depth;
                    }
                }

                current = current.parent;
                depth++;
            }

            return best;
        }

        private bool IsOwnedByPlayer(
            IWorldInteractionTarget target)
        {
            Transform targetTransform =
                target.InteractionTransform;

            return targetTransform == transform ||
                   (targetTransform != null &&
                    targetTransform.IsChildOf(transform));
        }

        private static bool IsTargetAlive(
            IWorldInteractionTarget target)
        {
            if (target == null)
            {
                return false;
            }

            if (target is Behaviour behaviour &&
                !behaviour.isActiveAndEnabled)
            {
                return false;
            }

            if (target is UnityEngine.Object unityObject)
            {
                return unityObject != null;
            }

            return true;
        }

        private static bool IsPointerOverUi()
        {
            EventSystem eventSystem =
                EventSystem.current;

            return eventSystem != null &&
                   eventSystem.IsPointerOverGameObject();
        }
    }
}
