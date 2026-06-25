using System;
using NUnit.Framework;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Application.Access;
using VRMGames.CartridgeAndCloud.Domain.Access;
using VRMGames.CartridgeAndCloud.Domain.Grid;
using VRMGames.CartridgeAndCloud.Domain.Placement;
using VRMGames.CartridgeAndCloud.Presentation.Placement;

namespace VRMGames.CartridgeAndCloud.Tests.PlayMode
{
    public sealed class StorePlacementRuntimeAccessTests
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
                    1.5f,
                    -0.05f,
                    1.5f);

            surfaceCollider.size =
                new Vector3(
                    3f,
                    0.1f,
                    3f);

            _surface =
                _surfaceObject.AddComponent<
                    PlacementSurface>();

            _surface.Configure(
                surfaceCollider,
                Vector3.zero,
                gridWidth: 6,
                gridDepth: 6,
                cellSize: 0.5f,
                raycastDistance: 100f);

            _definition =
                ScriptableObject.CreateInstance<
                    TechnicalPlaceableDefinition>();

            _definition.Configure(
                "technical-access-test",
                widthCells: 1,
                depthCells: 1,
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
        public void ConfigureInitialStoreAccessValidation_UsesInitialLayout()
        {
            BoxCollider surfaceCollider =
                _surfaceObject.GetComponent<
                    BoxCollider>();

            surfaceCollider.center =
                new Vector3(
                    5f,
                    -0.05f,
                    7.5f);

            surfaceCollider.size =
                new Vector3(
                    10f,
                    0.1f,
                    15f);

            _surface.Configure(
                surfaceCollider,
                new Vector3(-5f, 0f, -7.5f),
                gridWidth: 20,
                gridDepth: 30,
                cellSize: 0.5f,
                raycastDistance: 100f);

            _runtime.Configure(
                _surface,
                _previewController,
                _definition,
                _placedObjectsObject.transform,
                placedMaterial: null,
                camera: null);

            _runtime.ConfigureInitialStoreAccessValidation();

            Assert.That(
                _runtime.IsAccessValidationEnabled,
                Is.True);

            Assert.That(
                _runtime.AccessLayout,
                Is.Not.Null);

            Assert.That(
                _runtime.AccessLayout.Bounds.Size,
                Is.EqualTo(new GridSize(20, 30)));
        }

        [Test]
        public void ConfigureAccessValidation_MismatchedBounds_Throws()
        {
            StoreAccessLayout mismatched =
                new StoreAccessLayout(
                    new GridBounds(
                        new GridCoordinate(0, 0),
                        new GridSize(5, 5)),
                    new[]
                    {
                        new GridCoordinate(1, 0),
                        new GridCoordinate(2, 0)
                    },
                    new[]
                    {
                        new AccessAnchor(
                            new AccessAnchorId("target"),
                            new GridCoordinate(2, 4))
                    },
                    minimumOpenEntranceWidthCells: 2);

            Assert.Throws<ArgumentException>(
                () => _runtime.ConfigureAccessValidation(
                    mismatched));
        }

        [Test]
        public void ReservedEntrancePreview_IsRejected()
        {
            _runtime.ConfigureAccessValidation(
                CreateLayout());

            _previewController.UpdatePreviewAtCell(
                new GridCoordinate(2, 0));

            bool confirmed =
                _runtime.TryConfirmCurrentPlacement();

            Assert.That(confirmed, Is.False);

            Assert.That(
                _runtime.CurrentFailureReason,
                Is.EqualTo(
                    PlacementFailureReason.AccessBlocked));

            Assert.That(
                _runtime.CurrentAccessFailureReason,
                Is.EqualTo(
                    AccessValidationFailureReason
                        .ReservedEntranceBlocked));

            Assert.That(_ghostView.IsValid, Is.False);
        }

        [Test]
        public void AccessPreservingPreview_IsConfirmed()
        {
            _runtime.ConfigureAccessValidation(
                CreateLayout());

            _previewController.UpdatePreviewAtCell(
                new GridCoordinate(0, 1));

            bool confirmed =
                _runtime.TryConfirmCurrentPlacement();

            Assert.That(confirmed, Is.True);
            Assert.That(_runtime.PlacedCount, Is.EqualTo(1));
        }

        [Test]
        public void CandidateClosingOnlyGap_IsRejected()
        {
            _runtime.ConfigureAccessValidation(
                CreateLayout());

            AddBarrierWithGap();

            _previewController.UpdatePreviewAtCell(
                new GridCoordinate(3, 2));

            bool confirmed =
                _runtime.TryConfirmCurrentPlacement();

            Assert.That(confirmed, Is.False);

            Assert.That(
                _runtime.CurrentFailureReason,
                Is.EqualTo(
                    PlacementFailureReason.AccessBlocked));

            Assert.That(
                _runtime.CurrentAccessFailureReason,
                Is.EqualTo(
                    AccessValidationFailureReason
                        .RequiredAnchorUnreachable));
        }

        [Test]
        public void ClearAccessValidation_AllowsReservedCell()
        {
            _runtime.ConfigureAccessValidation(
                CreateLayout());

            _previewController.UpdatePreviewAtCell(
                new GridCoordinate(2, 0));

            Assert.That(
                _runtime.TryConfirmCurrentPlacement(),
                Is.False);

            _runtime.ClearAccessValidation();

            Assert.That(
                _runtime.IsAccessValidationEnabled,
                Is.False);

            Assert.That(
                _runtime.TryConfirmCurrentPlacement(),
                Is.True);
        }

        [Test]
        public void RemovePlacement_FreesCellsAndResetsAccessFailure()
        {
            _runtime.ConfigureAccessValidation(
                CreateLayout());

            GridCoordinate anchor =
                new GridCoordinate(0, 1);

            _previewController.UpdatePreviewAtCell(
                anchor);

            Assert.That(
                _runtime.TryConfirmCurrentPlacement(),
                Is.True);

            bool removed =
                _runtime.TryRemoveAtCell(
                    anchor);

            Assert.That(removed, Is.True);
            Assert.That(_runtime.PlacedCount, Is.Zero);
            Assert.That(
                _runtime.Registry.OccupiedCellCount,
                Is.Zero);

            Assert.That(
                _runtime.CurrentAccessFailureReason,
                Is.EqualTo(
                    AccessValidationFailureReason.None));
        }

        private static StoreAccessLayout CreateLayout()
        {
            return new StoreAccessLayout(
                new GridBounds(
                    new GridCoordinate(0, 0),
                    new GridSize(6, 6)),
                new[]
                {
                    new GridCoordinate(2, 0),
                    new GridCoordinate(3, 0)
                },
                new[]
                {
                    new AccessAnchor(
                        new AccessAnchorId("target"),
                        new GridCoordinate(3, 5))
                },
                minimumOpenEntranceWidthCells: 2);
        }

        private void AddBarrierWithGap()
        {
            int sequence = 1;

            for (int x = 0; x < 6; x++)
            {
                if (x == 3)
                {
                    continue;
                }

                PlacedObjectRecord record =
                    new PlacedObjectRecord(
                        new PlacementInstanceId(
                            $"barrier-{sequence}"),
                        "barrier",
                        new GridCoordinate(x, 2),
                        GridRotation.Degrees0,
                        new GridSize(1, 1));

                Assert.That(
                    _runtime.Registry.TryPlace(record).IsValid,
                    Is.True);

                sequence++;
            }
        }
    }
}
