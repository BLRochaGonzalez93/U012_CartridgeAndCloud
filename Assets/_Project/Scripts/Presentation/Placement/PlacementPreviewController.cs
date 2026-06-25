using UnityEngine;
using VRMGames.CartridgeAndCloud.Application.Placement;
using VRMGames.CartridgeAndCloud.Domain.Grid;

namespace VRMGames.CartridgeAndCloud.Presentation.Placement
{
    public sealed class PlacementPreviewController : MonoBehaviour
    {
        [SerializeField]
        private PlacementSurface _surface;

        [SerializeField]
        private TechnicalPlaceableDefinition _definition;

        [SerializeField]
        private PlacementGhostView _ghostView;

        [SerializeField]
        private UnityEngine.Camera _camera;

        [SerializeField]
        private GridRotation _rotation =
            GridRotation.Degrees0;

        public PlacementSurface Surface => _surface;
        public TechnicalPlaceableDefinition Definition => _definition;
        public PlacementGhostView GhostView => _ghostView;
        public GridRotation Rotation => _rotation;
        public bool HasPreview { get; private set; }
        public PlacementPreviewState CurrentState { get; private set; }

        private void Awake()
        {
            ResolveReferences();

            if (!HasPreview &&
                _ghostView != null)
            {
                _ghostView.ClearPreview();
            }
        }

        public void Configure(
            PlacementSurface surface,
            TechnicalPlaceableDefinition definition,
            PlacementGhostView ghostView,
            UnityEngine.Camera camera)
        {
            _surface = surface;
            _definition = definition;
            _ghostView = ghostView;
            _camera = camera;
        }

        public bool TryUpdatePreviewFromScreenPosition(
            Vector2 screenPosition)
        {
            ResolveReferences();

            if (_surface == null ||
                _definition == null ||
                _ghostView == null ||
                _camera == null)
            {
                ClearPreview();
                return false;
            }

            if (!_surface.TryProjectScreenPoint(
                    _camera,
                    screenPosition,
                    out GridCoordinate cell,
                    out _))
            {
                ClearPreview();
                return false;
            }

            UpdatePreviewAtCell(cell);
            return true;
        }

        public void UpdatePreviewAtCell(
            GridCoordinate anchor)
        {
            ResolveReferences();

            if (_surface == null ||
                _definition == null ||
                _ghostView == null)
            {
                ClearPreview();
                return;
            }

            CurrentState =
                PlacementPreviewCalculator.Calculate(
                    anchor,
                    _definition.GridSize,
                    _rotation,
                    _surface.Bounds);

            HasPreview = true;

            _ghostView.ApplyPreview(
                _surface,
                _definition,
                CurrentState);
        }

        public void ApplyValidity(bool isValid)
        {
            if (!HasPreview ||
                _ghostView == null ||
                _surface == null ||
                _definition == null)
            {
                return;
            }

            _ghostView.ApplyPreview(
                _surface,
                _definition,
                CurrentState,
                isValid);
        }

        public void RotateClockwise()
        {
            _rotation =
                _rotation.RotateClockwise();

            RefreshCurrentPreview();
        }

        public void RotateCounterClockwise()
        {
            _rotation =
                _rotation.RotateCounterClockwise();

            RefreshCurrentPreview();
        }

        public void ClearPreview()
        {
            HasPreview = false;
            _ghostView?.ClearPreview();
        }

        private void RefreshCurrentPreview()
        {
            if (HasPreview)
            {
                UpdatePreviewAtCell(
                    CurrentState.Anchor);
            }
        }

        private void ResolveReferences()
        {
            if (_surface == null)
            {
                _surface =
                    Object.FindFirstObjectByType<
                        PlacementSurface>();
            }

            if (_ghostView == null)
            {
                _ghostView =
                    GetComponent<PlacementGhostView>();
            }

            if (_camera == null)
            {
                _camera =
                    UnityEngine.Camera.main;
            }
        }
    }
}
