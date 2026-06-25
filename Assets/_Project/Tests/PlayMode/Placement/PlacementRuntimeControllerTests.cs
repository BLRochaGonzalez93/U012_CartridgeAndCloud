using NUnit.Framework;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.Grid;
using VRMGames.CartridgeAndCloud.Domain.Placement;
using VRMGames.CartridgeAndCloud.Presentation.Placement;

namespace VRMGames.CartridgeAndCloud.Tests.PlayMode
{
    public sealed class PlacementRuntimeControllerTests
    {
        private GameObject _surfaceObject;
        private GameObject _runtimeObject;
        private GameObject _visualObject;
        private GameObject _placedObjectsObject;
        private TechnicalPlaceableDefinition _definition;
        private PlacementSurface _surface;
        private PlacementPreviewController _previewController;
        private PlacementGhostView _ghostView;
        private PlacementRuntimeController _runtime;

        [SetUp]
        public void SetUp()
        {
            _surfaceObject =
                new GameObject("PlacementSurface");

            BoxCollider surfaceCollider =
                _surfaceObject.AddComponent<
                    BoxCollider>();

            surfaceCollider.center =
                new Vector3(
                    2.5f,
                    -0.05f,
                    2.5f);

            surfaceCollider.size =
                new Vector3(
                    5f,
                    0.1f,
                    5f);

            _surface =
                _surfaceObject.AddComponent<
                    PlacementSurface>();

            _surface.Configure(
                surfaceCollider,
                Vector3.zero,
                gridWidth: 10,
                gridDepth: 10,
                cellSize: 0.5f,
                raycastDistance: 100f);

            _definition =
                ScriptableObject.CreateInstance<
                    TechnicalPlaceableDefinition>();

            _definition.Configure(
                "technical-shelf-4x2",
                widthCells: 4,
                depthCells: 2,
                previewHeight: 1f);

            _runtimeObject =
                new GameObject("PlacementRuntime");

            _visualObject =
                GameObject.CreatePrimitive(
                    PrimitiveType.Cube);

            UnityEngine.Object.DestroyImmediate(
                _visualObject.GetComponent<
                    Collider>());

            _visualObject.transform.SetParent(
                _runtimeObject.transform,
                worldPositionStays: true);

            _ghostView =
                _runtimeObject.AddComponent<
                    PlacementGhostView>();

            _ghostView.Configure(
                _visualObject.transform,
                _visualObject.GetComponents<
                    Renderer>(),
                validMaterial: null,
                invalidMaterial: null);

            _previewController =
                _runtimeObject.AddComponent<
                    PlacementPreviewController>();

            _previewController.Configure(
                _surface,
                _definition,
                _ghostView,
                camera: null);

            _placedObjectsObject =
                new GameObject("PlacedObjects");

            _placedObjectsObject.transform.SetParent(
                _runtimeObject.transform,
                worldPositionStays: false);

            _runtime =
                _runtimeObject.AddComponent<
                    PlacementRuntimeController>();

            _runtime.Configure(
                _surface,
                _previewController,
                _definition,
                _placedObjectsObject.transform,
                placedMaterial: null,
                camera: null);

            _runtime.SetPlacementMode(true);
        }

        [TearDown]
        public void TearDown()
        {
            UnityEngine.Object.DestroyImmediate(
                _surfaceObject);

            UnityEngine.Object.DestroyImmediate(
                _runtimeObject);

            UnityEngine.Object.DestroyImmediate(
                _definition);
        }

        [Test]
        public void ConfirmValidPreview_OccupiesCellsAndCreatesView()
        {
            _previewController.UpdatePreviewAtCell(
                new GridCoordinate(1, 1));

            bool confirmed =
                _runtime.TryConfirmCurrentPlacement();

            Assert.That(confirmed, Is.True);
            Assert.That(_runtime.PlacedCount, Is.EqualTo(1));
            Assert.That(
                _runtime.Registry.OccupiedCellCount,
                Is.EqualTo(8));

            Assert.That(
                _placedObjectsObject.transform.childCount,
                Is.EqualTo(1));

            Assert.That(
                _ghostView.IsValid,
                Is.False);

            Assert.That(
                _runtime.CurrentFailureReason,
                Is.EqualTo(
                    PlacementFailureReason.Overlap));
        }

        [Test]
        public void ConfirmOverlappingPreview_IsRejectedAtomically()
        {
            _previewController.UpdatePreviewAtCell(
                new GridCoordinate(1, 1));

            Assert.That(
                _runtime.TryConfirmCurrentPlacement(),
                Is.True);

            bool secondConfirmation =
                _runtime.TryConfirmCurrentPlacement();

            Assert.That(
                secondConfirmation,
                Is.False);

            Assert.That(
                _runtime.PlacedCount,
                Is.EqualTo(1));

            Assert.That(
                _runtime.Registry.OccupiedCellCount,
                Is.EqualTo(8));

            Assert.That(
                _runtime.CurrentFailureReason,
                Is.EqualTo(
                    PlacementFailureReason.Overlap));
        }

        [Test]
        public void RemoveExistingPlacement_FreesOccupiedCells()
        {
            GridCoordinate anchor =
                new GridCoordinate(1, 1);

            _previewController.UpdatePreviewAtCell(
                anchor);

            Assert.That(
                _runtime.TryConfirmCurrentPlacement(),
                Is.True);

            bool removed =
                _runtime.TryRemoveAtCell(anchor);

            Assert.That(removed, Is.True);
            Assert.That(_runtime.PlacedCount, Is.Zero);
            Assert.That(
                _runtime.Registry.OccupiedCellCount,
                Is.Zero);

            Assert.That(
                _runtime.Registry.Validate(
                    anchor,
                    new GridSize(4, 2),
                    GridRotation.Degrees0)
                    .IsValid,
                Is.True);
        }

        [Test]
        public void ConfirmOutOfBoundsPreview_IsRejected()
        {
            _previewController.UpdatePreviewAtCell(
                new GridCoordinate(9, 9));

            bool confirmed =
                _runtime.TryConfirmCurrentPlacement();

            Assert.That(confirmed, Is.False);
            Assert.That(_runtime.PlacedCount, Is.Zero);

            Assert.That(
                _runtime.CurrentFailureReason,
                Is.EqualTo(
                    PlacementFailureReason.OutOfBounds));
        }

        [Test]
        public void CancelPlacement_DisablesModeAndHidesGhost()
        {
            _previewController.UpdatePreviewAtCell(
                new GridCoordinate(0, 0));

            Assert.That(_ghostView.IsVisible, Is.True);

            _runtime.CancelPlacement();

            Assert.That(
                _runtime.IsPlacementModeActive,
                Is.False);

            Assert.That(
                _ghostView.IsVisible,
                Is.False);
        }
    }
}
