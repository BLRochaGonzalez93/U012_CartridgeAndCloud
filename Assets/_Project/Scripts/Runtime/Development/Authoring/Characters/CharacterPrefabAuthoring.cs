using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.Characters;
using VRMGames.CartridgeAndCloud.Infrastructure.Store;
using VRMGames.CartridgeAndCloud.Presentation.Characters;
namespace VRMGames.CartridgeAndCloud.Runtime.Development.Authoring.Characters
{
    public sealed class CharacterPrefabAuthoring :
        MonoBehaviour
    {
        [SerializeField]
        private string _characterId;

        [SerializeField]
        private CharacterRole _role;

        [SerializeField]
        private string _materialVariantId;

        public string CharacterId =>
            _characterId;

        public CharacterRole Role =>
            _role;

        private void Awake()
        {
            BuildBlockout();
        }

        [ContextMenu("Build Blockout")]
        public void BuildBlockout()
        {
            EnsurePresence();

            if (transform.childCount > 0)
            {
                return;
            }

            StoreRuntimeAssetRegistry registry =
                StoreRuntimeAssetRegistry
                    .FindLoaded();

            if (registry == null ||
                registry.MaterialPalette == null)
            {
                return;
            }

            GameObject body =
                GameObject.CreatePrimitive(
                    PrimitiveType.Capsule);
            body.name = "Body";
            body.transform.SetParent(
                transform,
                false);
            body.transform.localPosition =
                new Vector3(0f, 0.9f, 0f);
            body.transform.localScale =
                new Vector3(
                    0.65f,
                    0.9f,
                    0.65f);

            Renderer renderer =
                body.GetComponent<Renderer>();

            if (renderer != null)
            {
                renderer.sharedMaterial =
                    registry.MaterialPalette.Find(
                        _materialVariantId);
            }
        }

        public void Configure(
            string characterId,
            CharacterRole role,
            string materialVariantId)
        {
            _characterId =
                characterId ?? string.Empty;
            _role = role;
            _materialVariantId =
                materialVariantId ??
                string.Empty;

            if (UnityEngine.Application.isPlaying)
            {
                EnsurePresence();
            }
        }

        private void EnsurePresence()
        {
            CharacterPresence presence =
                GetComponent<
                    CharacterPresence>();

            if (presence == null)
            {
                presence =
                    gameObject.AddComponent<
                        CharacterPresence>();
            }

            presence.Configure(
                _characterId,
                _role);
        }
    }
}
