using NUnit.Framework;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Application.Camera;
using VRMGames.CartridgeAndCloud.Presentation.Camera;

namespace VRMGames.CartridgeAndCloud.Tests.PlayMode
{
    public sealed class OrbitCameraRigTests
    {
        private GameObject _targetObject;
        private GameObject _cameraObject;
        private OrbitCameraRig _rig;

        [SetUp]
        public void SetUp()
        {
            _targetObject =
                new GameObject("CameraTarget");

            _cameraObject =
                new GameObject("OrbitCamera");

            _cameraObject.AddComponent<UnityEngine.Camera>();

            _rig =
                _cameraObject.AddComponent<OrbitCameraRig>();

            _rig.Configure(
                _targetObject.transform,
                0f,
                45f,
                10f,
                CreateConstraints());
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_cameraObject);
            Object.DestroyImmediate(_targetObject);
        }

        [Test]
        public void SnapToTarget_PreservesDistanceAndLooksAtTarget()
        {
            _rig.SnapToTarget();

            float distance = Vector3.Distance(
                _cameraObject.transform.position,
                _targetObject.transform.position);

            Vector3 directionToTarget =
                (_targetObject.transform.position -
                 _cameraObject.transform.position).normalized;

            float alignment = Vector3.Dot(
                _cameraObject.transform.forward,
                directionToTarget);

            Assert.That(distance, Is.EqualTo(10f).Within(0.001f));
            Assert.That(alignment, Is.GreaterThan(0.999f));
        }

        [Test]
        public void OrbitInput_ChangesPositionAndKeepsDistance()
        {
            Vector3 initialPosition =
                _cameraObject.transform.position;

            _rig.ApplyOrbitInput(90f, 0f);

            float distance = Vector3.Distance(
                _cameraObject.transform.position,
                _targetObject.transform.position);

            Assert.That(
                _cameraObject.transform.position,
                Is.Not.EqualTo(initialPosition));

            Assert.That(
                _rig.CurrentState.YawDegrees,
                Is.EqualTo(90f));

            Assert.That(
                distance,
                Is.EqualTo(10f).Within(0.001f));
        }

        [Test]
        public void ZoomInput_ClampsToConfiguredLimits()
        {
            _rig.ApplyZoomInput(100f);

            Assert.That(
                _rig.CurrentState.Distance,
                Is.EqualTo(6f));

            _rig.ApplyZoomInput(-100f);

            Assert.That(
                _rig.CurrentState.Distance,
                Is.EqualTo(24f));
        }

        [Test]
        public void MovingTarget_MovesCameraBySameOffset()
        {
            Vector3 initialCameraPosition =
                _cameraObject.transform.position;

            Vector3 targetOffset =
                new Vector3(3f, 0f, 2f);

            _targetObject.transform.position +=
                targetOffset;

            _rig.SnapToTarget();

            Vector3 cameraOffset =
                _cameraObject.transform.position -
                initialCameraPosition;

            Assert.That(
                cameraOffset.x,
                Is.EqualTo(targetOffset.x).Within(0.001f));

            Assert.That(
                cameraOffset.y,
                Is.EqualTo(targetOffset.y).Within(0.001f));

            Assert.That(
                cameraOffset.z,
                Is.EqualTo(targetOffset.z).Within(0.001f));
        }

        private static OrbitCameraConstraints CreateConstraints()
        {
            return new OrbitCameraConstraints(
                25f,
                75f,
                6f,
                24f);
        }
    }
}
