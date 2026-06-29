using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.VerticalSlicePhase1;

namespace VRMGames.CartridgeAndCloud.Infrastructure.VerticalSlicePhase1
{
    public sealed class Phase1CharacterPresence :
        MonoBehaviour
    {
        [SerializeField]
        private string _characterId;

        [SerializeField]
        private Phase1CharacterRole _role;

        public string CharacterId =>
            _characterId;

        public Phase1CharacterRole Role =>
            _role;

        public void Configure(
            string characterId,
            Phase1CharacterRole role)
        {
            _characterId =
                characterId ?? string.Empty;
            _role = role;
        }
    }
}
