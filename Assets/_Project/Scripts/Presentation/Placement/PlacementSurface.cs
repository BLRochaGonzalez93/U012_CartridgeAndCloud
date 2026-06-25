using System;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Application.Grid;
using VRMGames.CartridgeAndCloud.Application.Placement;
using VRMGames.CartridgeAndCloud.Domain.Grid;

namespace VRMGames.CartridgeAndCloud.Presentation.Placement
{
    public sealed class PlacementSurface : MonoBehaviour
    {
        [SerializeField]
        private Collider _surfaceCollider;

        [SerializeField]
        private Vector3 _gridOrigin = new Vector3(-4f, 0f, -4f);

        [SerializeField, Min(1)]
        private int _gridWidth = 16;

        [SerializeField, Min(1)]
        private int _gridDepth = 16;

        [SerializeField, Min(0.01f)]
        private float _cellSize = GridProjectionCalculator.DefaultCellSize;

        [SerializeField, Min(0.01f)]
        private float _raycastDistance = 500f;

        public Collider SurfaceCollider => _surfaceCollider;
        public Vector3 GridOrigin => _gridOrigin;
        public int GridWidth => _gridWidth;
        public int GridDepth => _gridDepth;
        public float CellSize => _cellSize;

        public GridBounds Bounds => new GridBounds(
            new GridCoordinate(0, 0),
            new GridSize(_gridWidth, _gridDepth));

        private void Awake()
        {
            if (_surfaceCollider == null)
            {
                _surfaceCollider = GetComponent<Collider>();
            }
        }

        public void Configure(
            Collider surfaceCollider,
            Vector3 gridOrigin,
            int gridWidth,
            int gridDepth,
            float cellSize,
            float raycastDistance)
        {
            if (surfaceCollider == null)
            {
                throw new ArgumentNullException(nameof(surfaceCollider));
            }

            if (gridWidth <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(gridWidth));
            }

            if (gridDepth <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(gridDepth));
            }

            ValidatePositiveFinite(cellSize, nameof(cellSize));
            ValidatePositiveFinite(raycastDistance, nameof(raycastDistance));

            _surfaceCollider = surfaceCollider;
            _gridOrigin = gridOrigin;
            _gridWidth = gridWidth;
            _gridDepth = gridDepth;
            _cellSize = cellSize;
            _raycastDistance = raycastDistance;
        }

        public bool TryProjectScreenPoint(
            UnityEngine.Camera camera,
            Vector2 screenPosition,
            out GridCoordinate cell,
            out Vector3 worldHit)
        {
            cell = default;
            worldHit = default;

            if (camera == null ||
                _surfaceCollider == null ||
                !_surfaceCollider.enabled)
            {
                return false;
            }

            Ray ray = camera.ScreenPointToRay(screenPosition);

            if (!_surfaceCollider.Raycast(
                    ray,
                    out RaycastHit hit,
                    _raycastDistance))
            {
                return false;
            }

            worldHit = hit.point;
            cell = WorldToCell(hit.point);
            return true;
        }

        public GridCoordinate WorldToCell(Vector3 worldPosition)
        {
            return GridProjectionCalculator.WorldToCell(
                worldPosition.x,
                worldPosition.z,
                _gridOrigin.x,
                _gridOrigin.z,
                _cellSize);
        }

        public Vector3 GetFootprintWorldCenter(
            PlacementPreviewState state,
            float previewHeight)
        {
            GridWorldPosition anchorCenter =
                GridProjectionCalculator.CellToWorldCenter(
                    state.Anchor,
                    _gridOrigin.x,
                    _gridOrigin.z,
                    _cellSize);

            return new Vector3(
                anchorCenter.X + state.CenterOffsetCellsX * _cellSize,
                _gridOrigin.y + previewHeight * 0.5f + 0.01f,
                anchorCenter.Z + state.CenterOffsetCellsZ * _cellSize);
        }

        private static void ValidatePositiveFinite(
            float value,
            string parameterName)
        {
            if (value <= 0f ||
                float.IsNaN(value) ||
                float.IsInfinity(value))
            {
                throw new ArgumentOutOfRangeException(
                    parameterName,
                    value,
                    "Value must be finite and greater than zero.");
            }
        }

        private void OnValidate()
        {
            _gridWidth = Mathf.Max(1, _gridWidth);
            _gridDepth = Mathf.Max(1, _gridDepth);

            if (_cellSize <= 0f ||
                float.IsNaN(_cellSize) ||
                float.IsInfinity(_cellSize))
            {
                _cellSize = GridProjectionCalculator.DefaultCellSize;
            }

            if (_raycastDistance <= 0f ||
                float.IsNaN(_raycastDistance) ||
                float.IsInfinity(_raycastDistance))
            {
                _raycastDistance = 500f;
            }
        }
    }
}
