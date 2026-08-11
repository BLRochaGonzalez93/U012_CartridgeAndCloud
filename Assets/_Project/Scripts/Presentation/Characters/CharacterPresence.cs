using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.Characters;
using VRMGames.CartridgeAndCloud.Presentation.Interaction;
namespace VRMGames.CartridgeAndCloud.Presentation.Characters
{
    public sealed class CharacterPresence :
        MonoBehaviour,
        IWorldInteractionTarget
    {
        [SerializeField]
        private string _characterId;

        [SerializeField]
        private CharacterRole _role;

        public string CharacterId =>
            _characterId;

        public CharacterRole Role =>
            _role;

        public string InteractionId =>
            _characterId ?? string.Empty;

        public WorldInteractionKind InteractionKind =>
            _role == CharacterRole.Customer
                ? WorldInteractionKind.Customer
                : _role == CharacterRole.Employee
                    ? WorldInteractionKind.Employee
                    : _role == CharacterRole.Supplier
                        ? WorldInteractionKind.Supplier
                        : WorldInteractionKind.Unknown;

        public Transform InteractionTransform =>
            transform;

        public float InteractionRange => 1.8f;

        public int InteractionPriority => 90;

        public bool IsInteractionAvailable =>
            _role != CharacterRole.Player &&
            !string.IsNullOrWhiteSpace(_characterId);

        public Vector3 GetInteractionPoint(
            Vector3 actorPosition)
        {
            return WorldInteractionGeometry.ResolveClosestPoint(
                transform,
                actorPosition);
        }

        public void Configure(
            string characterId,
            CharacterRole role)
        {
            _characterId =
                characterId ?? string.Empty;
            _role = role;
        }
    }
}
