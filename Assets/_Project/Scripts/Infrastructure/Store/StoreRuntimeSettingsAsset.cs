using System.Text;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Application.Camera;

namespace VRMGames.CartridgeAndCloud.Infrastructure.Store
{
    [CreateAssetMenu(
        menuName =
            "Cartridge & Cloud/Sprint 16 Phase 1/Settings",
        fileName =
            "CC_S16_P1_Settings")]
    public sealed class StoreRuntimeSettingsAsset :
        ScriptableObject
    {
        [Header("Scene")]
        [SerializeField]
        private string _storeSceneName =
            "StoreInitial";

        [Header("H6 Visibility Contract")]
        [SerializeField]
        private bool _hideOccludingWalls;

        [SerializeField]
        private bool _allowWallOcclusionToggle;

        [Header("Runtime")]
        [SerializeField]
        private bool _showProcedurePanel =
            true;

        [SerializeField, Min(1)]
        private int _vfxPoolSize = 24;

        [SerializeField, Min(1)]
        private int _maximumCustomers = 8;

        [Header("Authored Camera")]
        [SerializeField]
        private float _cameraYawDegrees;

        [SerializeField]
        private float _cameraPitchDegrees = 48f;

        [SerializeField, Min(0.01f)]
        private float _cameraDistance = 12f;

        [SerializeField]
        private float _cameraMinimumPitchDegrees = 25f;

        [SerializeField]
        private float _cameraMaximumPitchDegrees = 75f;

        [SerializeField, Min(0.01f)]
        private float _cameraMinimumDistance = 5f;

        [SerializeField, Min(0.01f)]
        private float _cameraMaximumDistance = 18f;

        [SerializeField, Min(0f)]
        private float _cameraOrbitSensitivity = 0.2f;

        [SerializeField, Min(0f)]
        private float _cameraZoomSensitivity = 0.5f;

        [SerializeField, Range(20f, 100f)]
        private float _cameraFieldOfView = 60f;

        [SerializeField, Min(0.01f)]
        private float _cameraNearClipPlane = 0.3f;

        [SerializeField, Min(1f)]
        private float _cameraFarClipPlane = 120f;

        [Header("Placement Authoring")]
        [SerializeField, Min(0.0001f)]
        private float _groundAnchorTolerance = 0.02f;

        public string StoreSceneName =>
            _storeSceneName;

        public bool HideOccludingWalls =>
            _hideOccludingWalls;

        public bool AllowWallOcclusionToggle =>
            _allowWallOcclusionToggle;

        public bool ShowProcedurePanel =>
            _showProcedurePanel;

        public int VfxPoolSize =>
            _vfxPoolSize;

        public int MaximumCustomers =>
            _maximumCustomers;

        public float CameraYawDegrees =>
            _cameraYawDegrees;

        public float CameraPitchDegrees =>
            _cameraPitchDegrees;

        public float CameraDistance =>
            _cameraDistance;

        public float CameraMinimumPitchDegrees =>
            _cameraMinimumPitchDegrees;

        public float CameraMaximumPitchDegrees =>
            _cameraMaximumPitchDegrees;

        public float CameraMinimumDistance =>
            _cameraMinimumDistance;

        public float CameraMaximumDistance =>
            _cameraMaximumDistance;

        public float CameraOrbitSensitivity =>
            _cameraOrbitSensitivity;

        public float CameraZoomSensitivity =>
            _cameraZoomSensitivity;

        public float CameraFieldOfView =>
            _cameraFieldOfView;

        public float CameraNearClipPlane =>
            _cameraNearClipPlane;

        public float CameraFarClipPlane =>
            _cameraFarClipPlane;

        public float GroundAnchorTolerance =>
            _groundAnchorTolerance;

        public OrbitCameraConstraints
            CreateCameraConstraints()
        {
            return new OrbitCameraConstraints(
                _cameraMinimumPitchDegrees,
                _cameraMaximumPitchDegrees,
                _cameraMinimumDistance,
                _cameraMaximumDistance);
        }

        public bool TryValidateAuthoring(
            out string report)
        {
            StringBuilder errors =
                new StringBuilder();

            if (string.IsNullOrWhiteSpace(
                    _storeSceneName))
            {
                errors.AppendLine(
                    "- Store scene name is required.");
            }

            if (_hideOccludingWalls ||
                _allowWallOcclusionToggle)
            {
                errors.AppendLine(
                    "- Automatic wall occlusion and its player toggle must remain disabled for H6.");
            }

            if (_cameraMinimumPitchDegrees >
                _cameraMaximumPitchDegrees)
            {
                errors.AppendLine(
                    "- Camera pitch limits are invalid.");
            }

            if (_cameraMinimumDistance <= 0f ||
                _cameraMinimumDistance >
                _cameraMaximumDistance)
            {
                errors.AppendLine(
                    "- Camera distance limits are invalid.");
            }

            if (_cameraDistance <
                    _cameraMinimumDistance ||
                _cameraDistance >
                    _cameraMaximumDistance)
            {
                errors.AppendLine(
                    "- Initial camera distance is outside its limits.");
            }

            if (_cameraPitchDegrees <
                    _cameraMinimumPitchDegrees ||
                _cameraPitchDegrees >
                    _cameraMaximumPitchDegrees)
            {
                errors.AppendLine(
                    "- Initial camera pitch is outside its limits.");
            }

            if (_cameraNearClipPlane <= 0f ||
                _cameraFarClipPlane <=
                    _cameraNearClipPlane)
            {
                errors.AppendLine(
                    "- Camera clipping planes are invalid.");
            }

            if (_groundAnchorTolerance <= 0f)
            {
                errors.AppendLine(
                    "- Ground anchor tolerance must be positive.");
            }

            report = errors.Length == 0
                ? "Store runtime authoring settings are valid."
                : errors.ToString().TrimEnd();

            return errors.Length == 0;
        }

        private void OnValidate()
        {
            _vfxPoolSize =
                Mathf.Max(1, _vfxPoolSize);

            _maximumCustomers =
                Mathf.Max(1, _maximumCustomers);

            _cameraMinimumDistance =
                Mathf.Max(
                    0.01f,
                    _cameraMinimumDistance);

            _cameraMaximumDistance =
                Mathf.Max(
                    _cameraMinimumDistance,
                    _cameraMaximumDistance);

            _cameraDistance =
                Mathf.Clamp(
                    _cameraDistance,
                    _cameraMinimumDistance,
                    _cameraMaximumDistance);

            if (_cameraMinimumPitchDegrees >
                _cameraMaximumPitchDegrees)
            {
                float temporary =
                    _cameraMinimumPitchDegrees;

                _cameraMinimumPitchDegrees =
                    _cameraMaximumPitchDegrees;

                _cameraMaximumPitchDegrees =
                    temporary;
            }

            _cameraPitchDegrees =
                Mathf.Clamp(
                    _cameraPitchDegrees,
                    _cameraMinimumPitchDegrees,
                    _cameraMaximumPitchDegrees);

            _cameraOrbitSensitivity =
                Mathf.Max(
                    0f,
                    _cameraOrbitSensitivity);

            _cameraZoomSensitivity =
                Mathf.Max(
                    0f,
                    _cameraZoomSensitivity);

            _cameraNearClipPlane =
                Mathf.Max(
                    0.01f,
                    _cameraNearClipPlane);

            _cameraFarClipPlane =
                Mathf.Max(
                    _cameraNearClipPlane +
                    1f,
                    _cameraFarClipPlane);

            _groundAnchorTolerance =
                Mathf.Max(
                    0.0001f,
                    _groundAnchorTolerance);
        }
    }
}
