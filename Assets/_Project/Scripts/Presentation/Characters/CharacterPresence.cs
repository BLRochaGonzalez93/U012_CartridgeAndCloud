using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.Characters;
namespace VRMGames.CartridgeAndCloud.Presentation.Characters
{
    public sealed class CharacterPresence :
        MonoBehaviour
    {
        [SerializeField]
        private string _characterId;

        [SerializeField]
        private CharacterRole _role;

        public string CharacterId =>
            _characterId;

        public CharacterRole Role =>
            _role;

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
