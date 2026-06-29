using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.VerticalSlicePhase1;

namespace VRMGames.CartridgeAndCloud.Infrastructure.VerticalSlicePhase1
{
    public sealed class Phase1CharacterPrefabAuthoring :
        MonoBehaviour
    {
        [SerializeField]
        private string _characterId;

        [SerializeField]
        private Phase1CharacterRole _role;

        [SerializeField]
        private string _materialVariantId;

        public string CharacterId =>
            _characterId;

        public Phase1CharacterRole Role =>
            _role;

        private void Awake()
        {
            BuildBlockout();
        }

        [ContextMenu("Build Blockout")]
        public void BuildBlockout()
        {
            if (transform.childCount > 0)
            {
                return;
            }

            Phase1MaterialPaletteAsset palette =
                Resources.Load<
                    Phase1MaterialPaletteAsset>(
                        "Sprint16Phase1/" +
                        "CC_S16_P1_MaterialPalette");

            if (palette == null)
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
                    palette.Find(
                        _materialVariantId);
            }

            Phase1CharacterPresence presence =
                GetComponent<
                    Phase1CharacterPresence>();

            if (presence == null)
            {
                presence =
                    gameObject.AddComponent<
                        Phase1CharacterPresence>();
            }

            presence.Configure(
                _characterId,
                _role);
        }

        public void Configure(
            string characterId,
            Phase1CharacterRole role,
            string materialVariantId)
        {
            _characterId =
                characterId ?? string.Empty;
            _role = role;
            _materialVariantId =
                materialVariantId ??
                string.Empty;
        }
    }
}
