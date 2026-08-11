using UnityEngine;

namespace VRMGames.CartridgeAndCloud.Presentation.Interaction
{
    [DisallowMultipleComponent]
    public sealed class WorldInteractionTargetAuthoring :
        MonoBehaviour,
        IWorldInteractionTarget
    {
        [SerializeField]
        private string _interactionId;

        [SerializeField]
        private WorldInteractionKind _interactionKind =
            WorldInteractionKind.Fixture;

        [SerializeField, Min(0.1f)]
        private float _interactionRange = 2f;

        [SerializeField]
        private int _interactionPriority = 50;

        [SerializeField]
        private bool _isInteractionAvailable = true;

        [SerializeField]
        private Transform _interactionAnchor;

        public string InteractionId =>
            _interactionId ?? string.Empty;

        public WorldInteractionKind InteractionKind =>
            _interactionKind;

        public Transform InteractionTransform =>
            _interactionAnchor != null
                ? _interactionAnchor
                : transform;

        public float InteractionRange =>
            Mathf.Max(0.1f, _interactionRange);

        public int InteractionPriority =>
            _interactionPriority;

        public bool IsInteractionAvailable =>
            _isInteractionAvailable &&
            !string.IsNullOrWhiteSpace(_interactionId);

        public Vector3 GetInteractionPoint(
            Vector3 actorPosition)
        {
            return WorldInteractionGeometry.ResolveClosestPoint(
                InteractionTransform,
                actorPosition);
        }

        public void Configure(
            string interactionId,
            WorldInteractionKind interactionKind,
            float interactionRange,
            int interactionPriority,
            bool isInteractionAvailable,
            Transform interactionAnchor = null)
        {
            _interactionId =
                interactionId?.Trim() ?? string.Empty;
            _interactionKind = interactionKind;
            _interactionRange =
                Mathf.Max(0.1f, interactionRange);
            _interactionPriority = interactionPriority;
            _isInteractionAvailable =
                isInteractionAvailable;
            _interactionAnchor = interactionAnchor;
        }
    }
}
