using System;
using System.Collections.Generic;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Application.Placement;
using VRMGames.CartridgeAndCloud.Domain.Grid;
using VRMGames.CartridgeAndCloud.Domain.Placement;

namespace VRMGames.CartridgeAndCloud.Presentation.Placement
{
    public sealed class PlacementRuntimeController :
        MonoBehaviour
    {
        [SerializeField]
        private PlacementSurface _surface;

        [SerializeField]
        private PlacementPreviewController _previewController;

        [SerializeField]
        private TechnicalPlaceableDefinition _definition;

        [SerializeField]
        private Transform _placedObjectsRoot;

        [SerializeField]
        private Material _placedMaterial;

        [SerializeField]
        private UnityEngine.Camera _camera;

        [SerializeField, Min(0.01f)]
        private float _removalRaycastDistance = 500f;

        private readonly Dictionary<
            PlacementInstanceId,
            PlacedObjectView> _viewsById =
                new Dictionary<
                    PlacementInstanceId,
                    PlacedObjectView>();

        private PlacementOccupancyRegistry _registry;
        private int _nextSequence = 1;

        public bool IsPlacementModeActive { get; private set; }

        public int PlacedCount =>
            _registry != null
                ? _registry.Count
                : 0;

        public PlacementFailureReason CurrentFailureReason
        {
            get;
            private set;
        }

        public PlacementOccupancyRegistry Registry
        {
            get
            {
                EnsureRegistry();
                return _registry;
            }
        }

        private void Awake()
        {
            ResolveReferences();
            EnsureRegistry();
            SetPlacementMode(false);
        }

        public void Configure(
            PlacementSurface surface,
            PlacementPreviewController previewController,
            TechnicalPlaceableDefinition definition,
            Transform placedObjectsRoot,
            Material placedMaterial,
            UnityEngine.Camera camera)
        {
            _surface = surface;
            _previewController = previewController;
            _definition = definition;
            _placedObjectsRoot = placedObjectsRoot;
            _placedMaterial = placedMaterial;
            _camera = camera;
            _registry = null;
            EnsureRegistry();
        }

        public void TogglePlacementMode()
        {
            SetPlacementMode(
                !IsPlacementModeActive);
        }

        public void SetPlacementMode(bool active)
        {
            IsPlacementModeActive = active;
            CurrentFailureReason =
                PlacementFailureReason.None;

            if (!active)
            {
                _previewController?.ClearPreview();
            }
        }

        public void CancelPlacement()
        {
            SetPlacementMode(false);
        }

        public bool TryUpdatePreviewFromScreenPosition(
            Vector2 screenPosition)
        {
            if (!IsPlacementModeActive)
            {
                _previewController?.ClearPreview();
                return false;
            }

            ResolveReferences();
            EnsureRegistry();

            if (_previewController == null)
            {
                return false;
            }

            bool projected =
                _previewController
                    .TryUpdatePreviewFromScreenPosition(
                        screenPosition);

            if (!projected)
            {
                CurrentFailureReason =
                    PlacementFailureReason.None;

                return false;
            }

            RefreshPreviewValidity();
            return true;
        }

        public void RotateClockwise()
        {
            if (!IsPlacementModeActive ||
                _previewController == null)
            {
                return;
            }

            _previewController.RotateClockwise();
            RefreshPreviewValidity();
        }

        public void RotateCounterClockwise()
        {
            if (!IsPlacementModeActive ||
                _previewController == null)
            {
                return;
            }

            _previewController.RotateCounterClockwise();
            RefreshPreviewValidity();
        }

        public bool TryConfirmCurrentPlacement()
        {
            ResolveReferences();
            EnsureRegistry();

            if (!IsPlacementModeActive ||
                _previewController == null ||
                !_previewController.HasPreview ||
                _definition == null)
            {
                return false;
            }

            PlacementValidationResult validation =
                ValidateCurrentPreview();

            if (!validation.IsValid)
            {
                CurrentFailureReason =
                    validation.FailureReason;

                _previewController.ApplyValidity(false);
                return false;
            }

            PlacementInstanceId id =
                CreateNextId();

            PlacedObjectRecord record =
                new PlacedObjectRecord(
                    id,
                    _definition.DefinitionId,
                    _previewController
                        .CurrentState.Anchor,
                    _previewController.Rotation,
                    _definition.GridSize);

            PlacementValidationResult result =
                _registry.TryPlace(record);

            if (!result.IsValid)
            {
                CurrentFailureReason =
                    result.FailureReason;

                _previewController.ApplyValidity(false);
                return false;
            }

            CreatePlacedView(record);
            CurrentFailureReason =
                PlacementFailureReason.None;

            RefreshPreviewValidity();
            return true;
        }

        public bool TryRemoveAtScreenPosition(
            Vector2 screenPosition)
        {
            ResolveReferences();

            if (_camera == null)
            {
                return false;
            }

            Ray ray =
                _camera.ScreenPointToRay(
                    screenPosition);

            RaycastHit[] hits =
                Physics.RaycastAll(
                    ray,
                    _removalRaycastDistance);

            Array.Sort(
                hits,
                (left, right) =>
                    left.distance.CompareTo(
                        right.distance));

            foreach (RaycastHit hit in hits)
            {
                PlacedObjectView view =
                    hit.collider.GetComponentInParent<
                        PlacedObjectView>();

                if (view != null)
                {
                    return TryRemove(view.Id);
                }
            }

            if (_surface != null &&
                _surface.TryProjectScreenPoint(
                    _camera,
                    screenPosition,
                    out GridCoordinate cell,
                    out _))
            {
                return TryRemoveAtCell(cell);
            }

            return false;
        }

        public bool TryRemoveAtCell(
            GridCoordinate cell)
        {
            EnsureRegistry();

            if (!_registry.TryGetOccupant(
                    cell,
                    out PlacementInstanceId id))
            {
                CurrentFailureReason =
                    PlacementFailureReason.NotFound;

                return false;
            }

            return TryRemove(id);
        }

        public bool TryRemove(
            PlacementInstanceId id)
        {
            EnsureRegistry();

            if (!_registry.TryRemove(
                    id,
                    out _))
            {
                CurrentFailureReason =
                    PlacementFailureReason.NotFound;

                return false;
            }

            if (_viewsById.TryGetValue(
                    id,
                    out PlacedObjectView view))
            {
                _viewsById.Remove(id);

                if (view != null)
                {
                    if (UnityEngine.Application.isPlaying)
                    {
                        Destroy(view.gameObject);
                    }
                    else
                    {
                        DestroyImmediate(view.gameObject);
                    }
                }
            }

            CurrentFailureReason =
                PlacementFailureReason.None;

            RefreshPreviewValidity();
            return true;
        }

        private PlacementValidationResult
            ValidateCurrentPreview()
        {
            if (_previewController == null ||
                !_previewController.HasPreview ||
                _definition == null)
            {
                return PlacementValidationResult.Invalid(
                    PlacementFailureReason.OutOfBounds);
            }

            return Registry.Validate(
                _previewController.CurrentState.Anchor,
                _definition.GridSize,
                _previewController.Rotation);
        }

        private void RefreshPreviewValidity()
        {
            if (_previewController == null ||
                !_previewController.HasPreview)
            {
                return;
            }

            PlacementValidationResult validation =
                ValidateCurrentPreview();

            CurrentFailureReason =
                validation.FailureReason;

            _previewController.ApplyValidity(
                validation.IsValid);
        }

        private PlacementInstanceId CreateNextId()
        {
            while (true)
            {
                PlacementInstanceId candidate =
                    new PlacementInstanceId(
                        $"technical-shelf-" +
                        $"{_nextSequence:0000}");

                _nextSequence++;

                if (!Registry.TryGetRecord(
                        candidate,
                        out _))
                {
                    return candidate;
                }
            }
        }

        private void CreatePlacedView(
            PlacedObjectRecord record)
        {
            if (_placedObjectsRoot == null ||
                _surface == null ||
                _definition == null)
            {
                return;
            }

            GameObject placedObject =
                GameObject.CreatePrimitive(
                    PrimitiveType.Cube);

            placedObject.transform.SetParent(
                _placedObjectsRoot,
                worldPositionStays: true);

            PlacementPreviewState state =
                PlacementPreviewCalculator.Calculate(
                    record.Anchor,
                    record.BaseSize,
                    record.Rotation,
                    _surface.Bounds);

            placedObject.transform.position =
                _surface.GetFootprintWorldCenter(
                    state,
                    _definition.PreviewHeight);

            placedObject.transform.rotation =
                Quaternion.Euler(
                    0f,
                    record.Rotation.ToDegrees(),
                    0f);

            placedObject.transform.localScale =
                new Vector3(
                    _definition.WidthCells *
                    _surface.CellSize,
                    _definition.PreviewHeight,
                    _definition.DepthCells *
                    _surface.CellSize);

            Renderer renderer =
                placedObject.GetComponent<Renderer>();

            if (renderer != null &&
                _placedMaterial != null)
            {
                renderer.sharedMaterial =
                    _placedMaterial;
            }

            PlacedObjectView view =
                placedObject.AddComponent<
                    PlacedObjectView>();

            view.Configure(record.Id);
            _viewsById.Add(record.Id, view);
        }

        private void EnsureRegistry()
        {
            ResolveReferences();

            if (_registry == null &&
                _surface != null)
            {
                _registry =
                    new PlacementOccupancyRegistry(
                        _surface.Bounds);
            }
        }

        private void ResolveReferences()
        {
            if (_surface == null)
            {
                _surface =
                    UnityEngine.Object.FindFirstObjectByType<
                        PlacementSurface>();
            }

            if (_previewController == null)
            {
                _previewController =
                    UnityEngine.Object.FindFirstObjectByType<
                        PlacementPreviewController>();
            }

            if (_definition == null &&
                _previewController != null)
            {
                _definition =
                    _previewController.Definition;
            }

            if (_camera == null)
            {
                _camera =
                    UnityEngine.Camera.main;
            }

            if (_placedObjectsRoot == null)
            {
                Transform existing =
                    transform.Find("PlacedObjects");

                if (existing != null)
                {
                    _placedObjectsRoot = existing;
                }
            }
        }
    }
}
