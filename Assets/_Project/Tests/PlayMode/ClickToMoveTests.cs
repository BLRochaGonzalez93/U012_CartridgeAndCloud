using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using VRMGames.CartridgeAndCloud.Application.InputContexts;
using VRMGames.CartridgeAndCloud.Presentation.PlayerMovement;

namespace VRMGames.CartridgeAndCloud.Tests.PlayMode
{
    public sealed class ClickToMoveTests
    {
        private GameObject _playerObject;
        private GameObject _cameraObject;
        private GameObject _floorObject;

        [TearDown]
        public void TearDown()
        {
            DestroyImmediate(_playerObject);
            DestroyImmediate(_cameraObject);
            DestroyImmediate(_floorObject);
        }

        [UnityTest]
        public IEnumerator Agent_Tick_ReachesDestinationWithoutOvershoot()
        {
            ClickToMoveAgent agent = CreateAgent();
            agent.enabled = false;
            agent.Configure(4f, 720f, 0.1f, 0f);
            agent.SetDestination(new Vector3(2f, 1f, 0f));

            for (int index = 0;
                 index < 20 && agent.HasDestination;
                 index++)
            {
                agent.Tick(0.1f);
                yield return null;
            }

            Assert.That(agent.HasDestination, Is.False);
            Assert.That(
                _playerObject.transform.position.x,
                Is.EqualTo(1.9f).Within(0.02f));
            Assert.That(
                _playerObject.transform.position.x,
                Is.LessThanOrEqualTo(2f));
        }

        [Test]
        public void Agent_SetDestination_ReplacesPreviousDestination()
        {
            ClickToMoveAgent agent = CreateAgent();

            agent.SetDestination(new Vector3(1f, 0f, 1f));
            agent.SetDestination(new Vector3(3f, 0f, 4f));

            Assert.That(agent.HasDestination, Is.True);
            Assert.That(agent.Destination.x, Is.EqualTo(3f));
            Assert.That(agent.Destination.z, Is.EqualTo(4f));
        }

        [UnityTest]
        public IEnumerator Input_Raycast_SetsDestinationInStandaloneGameplay()
        {
            ClickToMoveAgent agent = CreateAgent();
            Camera camera = CreateCamera();
            CreateFloor();

            ClickDestinationInput input =
                _playerObject.AddComponent<ClickDestinationInput>();

            input.Configure(
                camera,
                1 << 0,
                100f,
                true);

            Physics.SyncTransforms();
            yield return null;

            Vector3 worldTarget = new Vector3(3f, 0f, 2f);
            Vector3 screenTarget =
                camera.WorldToScreenPoint(worldTarget);

            bool accepted =
                input.TrySetDestinationFromScreenPosition(
                    screenTarget);

            Assert.That(accepted, Is.True);
            Assert.That(agent.HasDestination, Is.True);
            Assert.That(
                agent.Destination.x,
                Is.EqualTo(3f).Within(0.05f));
            Assert.That(
                agent.Destination.z,
                Is.EqualTo(2f).Within(0.05f));
        }

        [UnityTest]
        public IEnumerator Input_WhenUiContext_RejectsDestination()
        {
            CreateAgent();
            Camera camera = CreateCamera();
            CreateFloor();

            ClickDestinationInput input =
                _playerObject.AddComponent<ClickDestinationInput>();

            input.Configure(
                camera,
                1 << 0,
                100f,
                true);

            InputContextService contextService =
                new InputContextService();

            contextService.SetContext(InputContextId.UI);
            input.Initialize(contextService);

            Physics.SyncTransforms();
            yield return null;

            bool accepted =
                input.TrySetDestinationFromScreenPosition(
                    camera.WorldToScreenPoint(Vector3.zero));

            Assert.That(accepted, Is.False);
            Assert.That(
                _playerObject
                    .GetComponent<ClickToMoveAgent>()
                    .HasDestination,
                Is.False);
        }

        private ClickToMoveAgent CreateAgent()
        {
            _playerObject =
                new GameObject("TestClickToMoveAgent");

            _playerObject.transform.position =
                new Vector3(0f, 1f, 0f);

            CharacterController controller =
                _playerObject.AddComponent<CharacterController>();

            controller.center = Vector3.zero;
            controller.height = 2f;
            controller.radius = 0.5f;

            return _playerObject.AddComponent<ClickToMoveAgent>();
        }

        private Camera CreateCamera()
        {
            _cameraObject = new GameObject("TestCamera");
            Camera camera = _cameraObject.AddComponent<Camera>();

            _cameraObject.transform.position =
                new Vector3(0f, 10f, -10f);

            _cameraObject.transform.LookAt(Vector3.zero);

            return camera;
        }

        private void CreateFloor()
        {
            _floorObject =
                GameObject.CreatePrimitive(PrimitiveType.Plane);

            _floorObject.transform.position = Vector3.zero;
            _floorObject.transform.localScale =
                new Vector3(2f, 1f, 2f);
        }

        private static void DestroyImmediate(GameObject target)
        {
            if (target != null)
            {
                Object.DestroyImmediate(target);
            }
        }
    }
}
