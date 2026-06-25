using UnityEngine;
using VRMGames.CartridgeAndCloud.Application.Placement;
using VRMGames.CartridgeAndCloud.Domain.Grid;

namespace VRMGames.CartridgeAndCloud.Presentation.Placement
{
    public sealed class PlacementGhostView : MonoBehaviour
    {
        [SerializeField]
        private Transform _visualRoot;

        [SerializeField]
        private Renderer[] _renderers;

        [SerializeField]
        private Material _validMaterial;

        [SerializeField]
        private Material _invalidMaterial;

        public Transform VisualRoot => _visualRoot;

        public bool IsVisible =>
            _visualRoot != null &&
            _visualRoot.gameObject.activeSelf;

        public bool IsValid { get; private set; }

        public void Configure(
            Transform visualRoot,
            Renderer[] renderers,
            Material validMaterial,
            Material invalidMaterial)
        {
            _visualRoot = visualRoot;
            _renderers = renderers;
            _validMaterial = validMaterial;
            _invalidMaterial = invalidMaterial;
        }

        public void ApplyPreview(
            PlacementSurface surface,
            TechnicalPlaceableDefinition definition,
            PlacementPreviewState state)
        {
            if (_visualRoot == null ||
                surface == null ||
                definition == null)
            {
                return;
            }

            _visualRoot.gameObject.SetActive(true);
            _visualRoot.position =
                surface.GetFootprintWorldCenter(
                    state,
                    definition.PreviewHeight);

            _visualRoot.rotation = Quaternion.Euler(
                0f,
                state.Rotation.ToDegrees(),
                0f);

            _visualRoot.localScale = new Vector3(
                definition.WidthCells * surface.CellSize,
                definition.PreviewHeight,
                definition.DepthCells * surface.CellSize);

            IsValid = state.IsWithinBounds;
            ApplyMaterial(IsValid ? _validMaterial : _invalidMaterial);
        }

        public void ClearPreview()
        {
            IsValid = false;

            if (_visualRoot != null)
            {
                _visualRoot.gameObject.SetActive(false);
            }
        }

        private void ApplyMaterial(Material material)
        {
            if (material == null || _renderers == null)
            {
                return;
            }

            foreach (Renderer renderer in _renderers)
            {
                if (renderer != null)
                {
                    renderer.sharedMaterial = material;
                }
            }
        }
    }
}
