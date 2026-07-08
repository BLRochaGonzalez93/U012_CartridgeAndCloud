using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Presentation.Camera;
using VRMGames.CartridgeAndCloud.Presentation.Placement;
using VRMGames.CartridgeAndCloud.Presentation.Store.Doors;
using VRMGames.CartridgeAndCloud.Presentation.Store.Occlusion;

namespace VRMGames.CartridgeAndCloud.Presentation.Store.Authoring
{
    /// <summary>
    /// Explicit scene contract for the authored StoreInitial scene. Runtime
    /// systems resolve this component instead of reconstructing static content.
    /// An empty authored-fixture set is valid: new games intentionally begin
    /// with a clear sales floor and player-placed furniture.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class StoreInitialSceneContext : MonoBehaviour
    {
        [Header("Technical Store Contract")]
        [SerializeField]
        private PlacementSurface _placementSurface;

        [SerializeField]
        private StoreShellDescriptor _shellDescriptor;

        [SerializeField]
        private Transform _playerSpawn;

        [SerializeField]
        private Transform _technicalPlayer;

        [Header("Gameplay Anchors")]
        [SerializeField]
        private Transform _entranceAnchor;

        [SerializeField]
        private Transform _checkoutAnchor;

        [SerializeField]
        private Transform _receivingAnchor;

        [SerializeField]
        private Transform _backroomAnchor;

        [SerializeField]
        private Transform[] _customerSpawnAnchors =
            Array.Empty<Transform>();

        [SerializeField]
        private Transform[] _requiredAccessAnchors =
            Array.Empty<Transform>();

        [Header("Door and Camera")]
        [SerializeField]
        private AutomaticSlidingDoorController _door;

        [SerializeField]
        private AutomaticDoorParts _doorParts;

        [SerializeField]
        private UnityEngine.Camera _gameplayCamera;

        [SerializeField]
        private WallOcclusionController _wallOcclusion;

        [Header("Authored and Dynamic Roots")]
        [SerializeField]
        private GameObject _environmentRoot;

        [SerializeField]
        private Transform _initialFurnitureRoot;

        [SerializeField]
        private Transform _dynamicFurnitureRoot;

        [SerializeField]
        private Transform _dynamicProductsRoot;

        [SerializeField]
        private Transform _customersRoot;

        [SerializeField]
        private Transform _lightingRoot;

        public PlacementSurface PlacementSurface => _placementSurface;
        public StoreShellDescriptor ShellDescriptor => _shellDescriptor;
        public Transform PlayerSpawn => _playerSpawn;
        public Transform TechnicalPlayer => _technicalPlayer;
        public Transform EntranceAnchor => _entranceAnchor;
        public Transform CheckoutAnchor => _checkoutAnchor;
        public Transform ReceivingAnchor => _receivingAnchor;
        public Transform BackroomAnchor => _backroomAnchor;
        public IReadOnlyList<Transform> CustomerSpawnAnchors =>
            _customerSpawnAnchors;
        public IReadOnlyList<Transform> RequiredAccessAnchors =>
            _requiredAccessAnchors;
        public AutomaticSlidingDoorController Door
        {
            get
            {
                ResolveDoorContract();
                return _door;
            }
        }

        public AutomaticDoorParts DoorParts
        {
            get
            {
                ResolveDoorContract();
                return _doorParts;
            }
        }
        public UnityEngine.Camera GameplayCamera => _gameplayCamera;
        public WallOcclusionController WallOcclusion => _wallOcclusion;
        public GameObject EnvironmentRoot => _environmentRoot;
        public Transform InitialFurnitureRoot => _initialFurnitureRoot;
        public Transform DynamicFurnitureRoot => _dynamicFurnitureRoot;
        public Transform DynamicProductsRoot => _dynamicProductsRoot;
        public Transform CustomersRoot => _customersRoot;
        public Transform LightingRoot => _lightingRoot;
        public StoreNavigationAuthoring Navigation =>
            _environmentRoot == null
                ? null
                : _environmentRoot.GetComponentInChildren<StoreNavigationAuthoring>(true);

        public AuthoredStoreFixture[] GetAuthoredFixtures()
        {
            return _initialFurnitureRoot == null
                ? Array.Empty<AuthoredStoreFixture>()
                : _initialFurnitureRoot
                    .GetComponentsInChildren<AuthoredStoreFixture>(true);
        }

        public void Configure(
            PlacementSurface placementSurface,
            StoreShellDescriptor shellDescriptor,
            Transform playerSpawn,
            Transform technicalPlayer,
            Transform entranceAnchor,
            Transform checkoutAnchor,
            Transform receivingAnchor,
            Transform backroomAnchor,
            Transform[] customerSpawnAnchors,
            Transform[] requiredAccessAnchors,
            AutomaticSlidingDoorController door,
            AutomaticDoorParts doorParts,
            UnityEngine.Camera gameplayCamera,
            WallOcclusionController wallOcclusion,
            GameObject environmentRoot,
            Transform initialFurnitureRoot,
            Transform dynamicFurnitureRoot,
            Transform dynamicProductsRoot,
            Transform customersRoot,
            Transform lightingRoot)
        {
            _placementSurface = placementSurface;
            _shellDescriptor = shellDescriptor;
            _playerSpawn = playerSpawn;
            _technicalPlayer = technicalPlayer;
            _entranceAnchor = entranceAnchor;
            _checkoutAnchor = checkoutAnchor;
            _receivingAnchor = receivingAnchor;
            _backroomAnchor = backroomAnchor;
            _customerSpawnAnchors =
                customerSpawnAnchors ?? Array.Empty<Transform>();
            _requiredAccessAnchors =
                requiredAccessAnchors ?? Array.Empty<Transform>();
            _door = door;
            _doorParts = doorParts;
            _gameplayCamera = gameplayCamera;
            _wallOcclusion = wallOcclusion;
            _environmentRoot = environmentRoot;
            _initialFurnitureRoot = initialFurnitureRoot;
            _dynamicFurnitureRoot = dynamicFurnitureRoot;
            _dynamicProductsRoot = dynamicProductsRoot;
            _customersRoot = customersRoot;
            _lightingRoot = lightingRoot;
        }

        public bool TryValidate(out string report)
        {
            ResolveDoorContract();

            StringBuilder errors = new StringBuilder();

            Require(_placementSurface, "PlacementSurface", errors);
            Require(_shellDescriptor, "StoreShellDescriptor", errors);
            Require(_playerSpawn, "PlayerSpawn", errors);
            Require(_technicalPlayer, "TechnicalPlayer", errors);
            Require(_entranceAnchor, "EntranceAnchor", errors);
            Require(_checkoutAnchor, "CheckoutAnchor", errors);
            Require(_receivingAnchor, "ReceivingAnchor", errors);
            Require(_backroomAnchor, "BackroomAnchor", errors);
            Require(_door, "AutomaticSlidingDoorController", errors);
            Require(_doorParts, "AutomaticDoorParts", errors);
            Require(_gameplayCamera, "GameplayCamera", errors);

            if (_gameplayCamera != null)
            {
                OrbitCameraRig cameraRig =
                    _gameplayCamera.GetComponent<
                        OrbitCameraRig>();

                Require(
                    cameraRig,
                    "GameplayCamera/OrbitCameraRig",
                    errors);

                if (cameraRig != null &&
                    !cameraRig.TryValidateAuthoring(
                        out string cameraReport))
                {
                    errors.AppendLine(
                        "- " + cameraReport);
                }
            }

            Require(_environmentRoot, "EnvironmentRoot", errors);
            Require(Navigation, "StoreNavigationAuthoring", errors);
            Require(_initialFurnitureRoot, "InitialFurnitureRoot", errors);
            Require(_dynamicFurnitureRoot, "DynamicFurnitureRoot", errors);
            Require(_dynamicProductsRoot, "DynamicProductsRoot", errors);
            Require(_customersRoot, "CustomersRoot", errors);
            Require(_lightingRoot, "LightingRoot", errors);

            if (Navigation != null &&
                !Navigation.TryValidate(out string navigationReport))
            {
                errors.AppendLine("- " + navigationReport.Replace("\n", "\n- "));
            }

            if (_placementSurface != null)
            {
                if (_placementSurface.GridWidth != 20 ||
                    _placementSurface.GridDepth != 30 ||
                    !Mathf.Approximately(
                        _placementSurface.CellSize,
                        0.5f))
                {
                    errors.AppendLine(
                        "- PlacementSurface must use the documented " +
                        "20 x 30 grid with 0.5 m cells.");
                }
            }

            if (_doorParts != null &&
                (_doorParts.LeftPanel == null ||
                 _doorParts.RightPanel == null))
            {
                errors.AppendLine(
                    "- AutomaticDoorParts must reference both sliding panels.");
            }

            if (_customerSpawnAnchors == null ||
                _customerSpawnAnchors.Length == 0)
            {
                errors.AppendLine(
                    "- At least one customer spawn anchor is required.");
            }
            else
            {
                RequireAll(
                    _customerSpawnAnchors,
                    "CustomerSpawnAnchors",
                    errors);
            }

            if (_requiredAccessAnchors == null ||
                _requiredAccessAnchors.Length == 0)
            {
                errors.AppendLine(
                    "- RequiredAccessAnchors cannot be empty.");
            }
            else
            {
                RequireAll(
                    _requiredAccessAnchors,
                    "RequiredAccessAnchors",
                    errors);
            }

            ValidateAuthoredFixtures(errors);

            report = errors.Length == 0
                ? "StoreInitial scene contract is valid."
                : errors.ToString().TrimEnd();

            return errors.Length == 0;
        }

        public void ValidateOrThrow()
        {
            if (!TryValidate(out string report))
            {
                throw new InvalidOperationException(
                    "StoreInitial scene contract is invalid:\n" + report);
            }
        }

        private void ResolveDoorContract()
        {
            if (_door == null && _environmentRoot != null)
            {
                _door = _environmentRoot
                    .GetComponentInChildren<AutomaticSlidingDoorController>(true);
            }

            if (_doorParts == null)
            {
                if (_door != null)
                {
                    _doorParts = _door.GetComponent<AutomaticDoorParts>();
                }

                if (_doorParts == null && _environmentRoot != null)
                {
                    _doorParts = _environmentRoot
                        .GetComponentInChildren<AutomaticDoorParts>(true);
                }
            }
        }

        private void ValidateAuthoredFixtures(StringBuilder errors)
        {
            AuthoredStoreFixture[] fixtures = GetAuthoredFixtures();

            HashSet<string> ids =
                new HashSet<string>(StringComparer.Ordinal);

            foreach (AuthoredStoreFixture fixture in fixtures)
            {
                if (fixture == null)
                {
                    errors.AppendLine(
                        "- InitialFurniture contains a missing fixture reference.");
                    continue;
                }

                if (string.IsNullOrWhiteSpace(fixture.InstanceId))
                {
                    errors.AppendLine(
                        $"- {fixture.name} has no stable instance ID.");
                }
                else if (!ids.Add(fixture.InstanceId))
                {
                    errors.AppendLine(
                        $"- Duplicate authored fixture ID: {fixture.InstanceId}.");
                }

                if (string.IsNullOrWhiteSpace(fixture.DefinitionId))
                {
                    errors.AppendLine(
                        $"- {fixture.name} has no furniture definition ID.");
                }
            }
        }

        private static void Require(
            UnityEngine.Object value,
            string label,
            StringBuilder errors)
        {
            if (value == null)
            {
                errors.AppendLine($"- Missing {label}.");
            }
        }

        private static void RequireAll(
            Transform[] values,
            string label,
            StringBuilder errors)
        {
            for (int index = 0; index < values.Length; index++)
            {
                if (values[index] == null)
                {
                    errors.AppendLine(
                        $"- {label}[{index}] is missing.");
                }
            }
        }
    }
}
