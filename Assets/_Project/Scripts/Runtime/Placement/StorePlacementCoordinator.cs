using System;
using System.Reflection;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.Grid;
using VRMGames.CartridgeAndCloud.Domain.Placement;
using VRMGames.CartridgeAndCloud.Presentation.Placement;

using VRMGames.CartridgeAndCloud.Application.Store;
using VRMGames.CartridgeAndCloud.Application.UIUX;
using VRMGames.CartridgeAndCloud.Domain.Store;
using VRMGames.CartridgeAndCloud.Infrastructure.Store;
using VRMGames.CartridgeAndCloud.Runtime.Development.Blockout;
namespace VRMGames.CartridgeAndCloud.Runtime.Placement
{
    public sealed class StorePlacementCoordinator :
        MonoBehaviour
    {
        private static readonly FieldInfo
            RuntimeDefinitionField =
                typeof(PlacementRuntimeController)
                    .GetField(
                        "_definition",
                        BindingFlags.Instance |
                        BindingFlags.NonPublic);

        private static readonly MethodInfo
            CreatePlacedViewMethod =
                typeof(PlacementRuntimeController)
                    .GetMethod(
                        "CreatePlacedView",
                        BindingFlags.Instance |
                        BindingFlags.NonPublic);

        private PlacementRuntimeController
            _runtime;
        private PlacementPreviewController
            _preview;
        private PlacementSurface _surface;
        private StoreOperationsFacade
            _service;
        private IStoreContentCatalog _catalog;
        private StoreMaterialPaletteAsset
            _palette;

        private TechnicalPlaceableDefinition
            _temporaryDefinition;
        private StoreFixtureDefinition
            _activeDefinition;
        private int _lastPlacedCount;
        private PlacementFailureReason
            _lastFailure =
                PlacementFailureReason.None;

        public bool IsPlacing =>
            _activeDefinition != null &&
            _runtime != null &&
            _runtime.IsPlacementModeActive;

        public event Action<GameplayFeedbackEvent>
            FeedbackRaised;

        public void Configure(
            StoreOperationsFacade service,
            IStoreContentCatalog catalog,
            StoreMaterialPaletteAsset palette)
        {
            _service = service ??
                throw new ArgumentNullException(
                    nameof(service));
            _catalog = catalog ??
                throw new ArgumentNullException(
                    nameof(catalog));
            _palette = palette ??
                throw new ArgumentNullException(
                    nameof(palette));

            ResolveReferences();
            RestorePersistedFixtures();
        }

        public StoreOperationResult BeginPlacement(
            string definitionId)
        {
            ResolveReferences();

            if (_runtime == null ||
                _preview == null ||
                _surface == null)
            {
                return StoreOperationResult.Failure(
                    StoreOperationStatus.InvalidState,
                    "Placement runtime is not available.");
            }

            if (!_catalog.TryGetFurniture(
                    definitionId,
                    out StoreFixtureDefinition
                        definition))
            {
                return StoreOperationResult.Failure(
                    StoreOperationStatus.NotFound,
                    "Furniture definition is missing.");
            }

            if (_service
                    .GetFurnitureWarehouseQuantity(
                        definitionId) < 1)
            {
                return StoreOperationResult.Failure(
                    StoreOperationStatus
                        .InsufficientStock,
                    "Receive the furniture before placing it.");
            }

            DestroyTemporaryDefinition();

            _temporaryDefinition =
                ScriptableObject.CreateInstance<
                    TechnicalPlaceableDefinition>();

            _temporaryDefinition.Configure(
                definition.DefinitionId,
                definition.WidthCells,
                definition.DepthCells,
                definition.HeightMeters);

            if (RuntimeDefinitionField == null)
            {
                return StoreOperationResult.Failure(
                    StoreOperationStatus.InvalidState,
                    "Placement definition bridge is unavailable.");
            }

            RuntimeDefinitionField.SetValue(
                _runtime,
                _temporaryDefinition);

            _preview.Configure(
                _surface,
                _temporaryDefinition,
                _preview.GhostView,
                Camera.main);

            _activeDefinition = definition;
            _lastPlacedCount =
                _runtime.PlacedCount;
            _lastFailure =
                PlacementFailureReason.None;

            _runtime.SetPlacementMode(true);

            FeedbackRaised?.Invoke(
                new GameplayFeedbackEvent(
                    GameplayFeedbackType
                        .ObjectSelected,
                    $"{definition.DisplayName} selected.",
                    "placement-preview"));

            return StoreOperationResult.Success(
                "Placement mode started.");
        }

        public void CancelPlacement()
        {
            _runtime?.CancelPlacement();
            _activeDefinition = null;
            DestroyTemporaryDefinition();
        }

        private void Update()
        {
            ResolveReferences();
            SynchronizeRemovals();

            if (_runtime == null ||
                _activeDefinition == null)
            {
                return;
            }

            if (_runtime.PlacedCount >
                _lastPlacedCount)
            {
                if (TryCompletePendingPlacement())
                {
                    return;
                }
            }

            if (!_runtime.IsPlacementModeActive)
            {
                _activeDefinition = null;
                DestroyTemporaryDefinition();
                return;
            }

            PublishPlacementValidity();
        }

        private bool TryCompletePendingPlacement()
        {
            if (!TryFindPendingPlacement(
                    out PlacementInstanceId id,
                    out PlacedObjectRecord record))
            {
                return false;
            }

            StoreOperationResult result =
                _service.ConfirmFurniturePlacement(
                    _activeDefinition.DefinitionId,
                    id.Value,
                    record.Anchor.X,
                    record.Anchor.Z,
                    (int)record.Rotation);

            if (!result.Succeeded)
            {
                _runtime.TryRemove(id);

                FeedbackRaised?.Invoke(
                    new GameplayFeedbackEvent(
                        GameplayFeedbackType
                            .PlacementInvalid,
                        result.Detail,
                        "placement-preview"));
            }
            else
            {
                DecoratePlacedView(
                    id,
                    _activeDefinition);

                FeedbackRaised?.Invoke(
                    new GameplayFeedbackEvent(
                        GameplayFeedbackType
                            .PlacementValid,
                        "Placement confirmed.",
                        id.Value));
            }

            _lastPlacedCount =
                _runtime.PlacedCount;
            _runtime.SetPlacementMode(false);
            _activeDefinition = null;
            DestroyTemporaryDefinition();
            return true;
        }

        private bool TryFindPendingPlacement(
            out PlacementInstanceId id,
            out PlacedObjectRecord record)
        {
            id = default;
            record = null;

            if (_preview != null &&
                _preview.HasPreview)
            {
                GridCoordinate anchor =
                    _preview.CurrentState.Anchor;

                if (_runtime.Registry
                        .TryGetOccupant(
                            anchor,
                            out PlacementInstanceId
                                previewId) &&
                    _runtime.Registry
                        .TryGetRecord(
                            previewId,
                            out PlacedObjectRecord
                                previewRecord) &&
                    IsPendingPlacement(
                        previewRecord))
                {
                    id = previewId;
                    record = previewRecord;
                    return true;
                }
            }

            GridCoordinate[] occupiedCells =
                _runtime.Registry
                    .GetOccupiedCells();

            foreach (GridCoordinate cell
                     in occupiedCells)
            {
                if (!_runtime.Registry
                        .TryGetOccupant(
                            cell,
                            out PlacementInstanceId
                                candidateId) ||
                    !_runtime.Registry
                        .TryGetRecord(
                            candidateId,
                            out PlacedObjectRecord
                                candidateRecord) ||
                    !IsPendingPlacement(
                        candidateRecord))
                {
                    continue;
                }

                id = candidateId;
                record = candidateRecord;
                return true;
            }

            return false;
        }

        private bool IsPendingPlacement(
            PlacedObjectRecord record)
        {
            if (record == null ||
                _activeDefinition == null ||
                !string.Equals(
                    record.DefinitionId,
                    _activeDefinition.DefinitionId,
                    StringComparison.Ordinal))
            {
                return false;
            }

            foreach (PlacedStoreFixtureRecord
                     fixture in _service.State.Fixtures)
            {
                if (string.Equals(
                        fixture.InstanceId,
                        record.Id.Value,
                        StringComparison.Ordinal))
                {
                    return false;
                }
            }

            return true;
        }


        private void SynchronizeRemovals()
        {
            if (_runtime == null ||
                _service == null ||
                _service.State == null)
            {
                return;
            }

            PlacedStoreFixtureRecord[] fixtures =
                new PlacedStoreFixtureRecord[
                    _service.State.Fixtures.Count];

            for (int index = 0;
                 index < fixtures.Length;
                 index++)
            {
                fixtures[index] =
                    _service.State.Fixtures[index];
            }

            foreach (PlacedStoreFixtureRecord
                     fixture in fixtures)
            {
                PlacementInstanceId id =
                    new PlacementInstanceId(
                        fixture.InstanceId);

                if (_runtime.Registry.TryGetRecord(
                        id,
                        out _))
                {
                    continue;
                }

                StoreOperationResult result =
                    _service
                        .RemoveFurniturePlacement(
                            fixture.InstanceId);

                if (result.Succeeded)
                {
                    FeedbackRaised?.Invoke(
                        new GameplayFeedbackEvent(
                            GameplayFeedbackType
                                .ObjectSelected,
                            "Furniture removed.",
                            fixture.InstanceId));
                }
            }

        }

        private void PublishPlacementValidity()
        {
            PlacementFailureReason current =
                _runtime.CurrentFailureReason;

            if (current == _lastFailure)
            {
                return;
            }

            _lastFailure = current;

            FeedbackRaised?.Invoke(
                new GameplayFeedbackEvent(
                    current ==
                        PlacementFailureReason.None
                        ? GameplayFeedbackType
                            .PlacementValid
                        : GameplayFeedbackType
                            .PlacementInvalid,
                    current ==
                        PlacementFailureReason.None
                        ? "Placement valid."
                        : "Placement blocked: " +
                          current,
                    "placement-preview"));
        }

        private void RestorePersistedFixtures()
        {
            ResolveReferences();

            if (_runtime == null ||
                _surface == null ||
                _service.State == null)
            {
                return;
            }

            foreach (PlacedStoreFixtureRecord fixture
                     in _service.State.Fixtures)
            {
                PlacementInstanceId id =
                    new PlacementInstanceId(
                        fixture.InstanceId);

                if (_runtime.Registry.TryGetRecord(
                        id,
                        out _))
                {
                    continue;
                }

                if (!_catalog.TryGetFurniture(
                        fixture.DefinitionId,
                        out StoreFixtureDefinition
                            definition))
                {
                    continue;
                }

                PlacedObjectRecord record =
                    new PlacedObjectRecord(
                        id,
                        fixture.DefinitionId,
                        new GridCoordinate(
                            fixture.AnchorX,
                            fixture.AnchorZ),
                        (GridRotation)
                            fixture
                                .RotationQuarterTurns,
                        new GridSize(
                            definition.WidthCells,
                            definition.DepthCells));

                if (!_runtime.Registry
                        .TryPlace(record)
                        .IsValid)
                {
                    continue;
                }

                TechnicalPlaceableDefinition
                    temporary =
                        ScriptableObject
                            .CreateInstance<
                                TechnicalPlaceableDefinition>();

                temporary.Configure(
                    definition.DefinitionId,
                    definition.WidthCells,
                    definition.DepthCells,
                    definition.HeightMeters);

                object previous =
                    RuntimeDefinitionField?.GetValue(
                        _runtime);

                RuntimeDefinitionField?.SetValue(
                    _runtime,
                    temporary);

                CreatePlacedViewMethod?.Invoke(
                    _runtime,
                    new object[] { record });

                RuntimeDefinitionField?.SetValue(
                    _runtime,
                    previous);

                Destroy(temporary);
                DecoratePlacedView(
                    id,
                    definition);
            }

            _lastPlacedCount =
                _runtime.PlacedCount;
        }

        private void DecoratePlacedView(
            PlacementInstanceId id,
            StoreFixtureDefinition definition)
        {
            PlacedObjectView[] views =
                UnityEngine.Object
                    .FindObjectsByType<
                        PlacedObjectView>(
                            FindObjectsInactive
                                .Include,
                            FindObjectsSortMode.None);

            foreach (PlacedObjectView view
                     in views)
            {
                if (view.Id != id)
                {
                    continue;
                }

                Material material =
                    _palette.Find(
                        definition
                            .MaterialVariantId);

                StoreBlockoutVisualFactory
                    .BuildFurniture(
                        view.gameObject,
                        definition,
                        material,
                        _surface.CellSize);

                PlacedFixtureVisual marker =
                    view.GetComponent<
                        PlacedFixtureVisual>();

                if (marker == null)
                {
                    marker =
                        view.gameObject
                            .AddComponent<
                                PlacedFixtureVisual>();
                }

                marker.Configure(
                    definition.DefinitionId,
                    id.Value);
                return;
            }
        }

        private void ResolveReferences()
        {
            if (_runtime == null)
            {
                _runtime =
                    UnityEngine.Object
                        .FindFirstObjectByType<
                            PlacementRuntimeController>();
            }

            if (_preview == null)
            {
                _preview =
                    UnityEngine.Object
                        .FindFirstObjectByType<
                            PlacementPreviewController>();
            }

            if (_surface == null)
            {
                _surface =
                    UnityEngine.Object
                        .FindFirstObjectByType<
                            PlacementSurface>();
            }
        }

        private void OnDestroy()
        {
            DestroyTemporaryDefinition();
        }

        private void DestroyTemporaryDefinition()
        {
            if (_temporaryDefinition == null)
            {
                return;
            }

            Destroy(_temporaryDefinition);
            _temporaryDefinition = null;
        }
    }

}
