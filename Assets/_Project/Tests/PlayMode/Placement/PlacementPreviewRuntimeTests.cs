using NUnit.Framework;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.Grid;
using VRMGames.CartridgeAndCloud.Presentation.Placement;

namespace VRMGames.CartridgeAndCloud.Tests.PlayMode
{
    public sealed class PlacementPreviewRuntimeTests
    {
        private GameObject _surfaceObject;
        private GameObject _previewObject;
        private GameObject _visualObject;
        private TechnicalPlaceableDefinition _definition;
        private PlacementPreviewController _controller;
        private PlacementGhostView _ghostView;

        [SetUp]
        public void SetUp()
        {
            _surfaceObject =
                new GameObject("PlacementSurface");

            BoxCollider collider =
                _surfaceObject.AddComponent<BoxCollider>();

            collider.center =
                new Vector3(2.5f, -0.05f, 2.5f);
            collider.size =
                new Vector3(5f, 0.1f, 5f);

            PlacementSurface surface =
                _surfaceObject.AddComponent<PlacementSurface>();

            surface.Configure(
                collider,
                Vector3.zero,
                10,
                10,
                0.5f,
                100f);

            _definition =
                ScriptableObject.CreateInstance<
                    TechnicalPlaceableDefinition>();
            _definition.Configure(4, 2, 1f);

            _previewObject =
                new GameObject("PreviewRoot");

            _visualObject =
                GameObject.CreatePrimitive(PrimitiveType.Cube);

            Object.DestroyImmediate(
                _visualObject.GetComponent<Collider>());

            _visualObject.transform.SetParent(
                _previewObject.transform,
                worldPositionStays: true);

            _ghostView =
                _previewObject.AddComponent<PlacementGhostView>();

            _ghostView.Configure(
                _visualObject.transform,
                _visualObject.GetComponents<Renderer>(),
                validMaterial: null,
                invalidMaterial: null);

            _controller =
                _previewObject.AddComponent<
                    PlacementPreviewController>();

            _controller.Configure(
                surface,
                _definition,
                _ghostView,
                camera: null);
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_surfaceObject);
            Object.DestroyImmediate(_previewObject);
            Object.DestroyImmediate(_definition);
        }

        [Test]
        public void UpdatePreviewAtCell_ShowsValidSnappedGhost()
        {
            _controller.UpdatePreviewAtCell(
                new GridCoordinate(0, 0));

            Assert.That(_controller.HasPreview, Is.True);
            Assert.That(_ghostView.IsVisible, Is.True);
            Assert.That(_ghostView.IsValid, Is.True);

            Assert.That(
                _visualObject.transform.position.x,
                Is.EqualTo(1f).Within(0.0001f));

            Assert.That(
                _visualObject.transform.position.z,
                Is.EqualTo(0.5f).Within(0.0001f));
        }

        [Test]
        public void RotateClockwise_UpdatesOrientationAndCenter()
        {
            _controller.UpdatePreviewAtCell(
                new GridCoordinate(0, 0));

            _controller.RotateClockwise();

            Assert.That(
                _controller.Rotation,
                Is.EqualTo(GridRotation.Degrees90));

            Assert.That(
                _controller.CurrentState.OrientedSize,
                Is.EqualTo(new GridSize(2, 4)));

            Assert.That(
                Mathf.DeltaAngle(
                    90f,
                    _visualObject.transform.eulerAngles.y),
                Is.EqualTo(0f).Within(0.001f));

            Assert.That(
                _visualObject.transform.position.x,
                Is.EqualTo(0.5f).Within(0.0001f));

            Assert.That(
                _visualObject.transform.position.z,
                Is.EqualTo(1f).Within(0.0001f));
        }

        [Test]
        public void ClearPreview_HidesGhost()
        {
            _controller.UpdatePreviewAtCell(
                new GridCoordinate(0, 0));

            _controller.ClearPreview();

            Assert.That(_controller.HasPreview, Is.False);
            Assert.That(_ghostView.IsVisible, Is.False);
        }
    }
}
