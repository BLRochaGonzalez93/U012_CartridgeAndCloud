using System;
using System.Collections.Generic;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.VerticalSlicePhase1;
using VRMGames.CartridgeAndCloud.Infrastructure.VerticalSlicePhase1;
using VRMGames.CartridgeAndCloud.Presentation.Placement;
using VRMGames.CartridgeAndCloud.Presentation.Store;

namespace VRMGames.CartridgeAndCloud.Runtime.VerticalSlicePhase1
{
    public sealed class Phase1StoreBlockoutBuilder :
        MonoBehaviour
    {
        public const string RootName =
            "S16_P1_StoreBlockout";

        private Phase1StoreShellAsset _shell;
        private Phase1MaterialPaletteAsset
            _palette;
        private Phase1SettingsAsset _settings;
        private PlacementSurface _surface;
        private GameObject _builtRoot;
        private readonly List<LegacyRendererState>
            _legacyRenderers =
                new List<LegacyRendererState>();
        private bool _ambientCaptured;
        private UnityEngine.Rendering.AmbientMode
            _previousAmbientMode;
        private Color _previousAmbientSky;
        private Color _previousAmbientEquator;
        private Color _previousAmbientGround;
        private float _previousAmbientIntensity;

        public Transform EntranceAnchor {
            get;
            private set;
        }

        public Transform CheckoutAnchor {
            get;
            private set;
        }

        public Transform ReceivingAnchor {
            get;
            private set;
        }

        public Transform BackroomAnchor {
            get;
            private set;
        }

        public AutomaticSlidingDoorController
            Door { get; private set; }

        public Phase1WallOcclusionController
            WallOcclusion { get; private set; }

        public void Configure(
            Phase1StoreShellAsset shell,
            Phase1MaterialPaletteAsset palette,
            Phase1SettingsAsset settings)
        {
            _shell = shell ??
                throw new ArgumentNullException(
                    nameof(shell));
            _palette = palette ??
                throw new ArgumentNullException(
                    nameof(palette));
            _settings = settings ??
                throw new ArgumentNullException(
                    nameof(settings));
        }

        public void Build()
        {
            if (_shell == null ||
                _palette == null ||
                _settings == null)
            {
                throw new InvalidOperationException(
                    "Blockout builder must be configured.");
            }

            _surface =
                UnityEngine.Object
                    .FindFirstObjectByType<
                        PlacementSurface>();

            if (_surface == null)
            {
                throw new InvalidOperationException(
                    "Store PlacementSurface was not found.");
            }

            SuppressLegacyShellVisuals();
            DestroyExisting();

            _builtRoot =
                new GameObject(RootName);
            _builtRoot.transform.SetParent(
                transform,
                false);

            float width =
                _surface.GridWidth *
                _surface.CellSize;
            float depth =
                _surface.GridDepth *
                _surface.CellSize;
            Vector3 origin =
                _surface.GridOrigin;
            Vector3 center =
                new Vector3(
                    origin.x + width * 0.5f,
                    origin.y,
                    origin.z + depth * 0.5f);

            BuildShell(
                center,
                width,
                depth);
            BuildZones(
                center,
                origin,
                width,
                depth);
            BuildLighting(
                center,
                width,
                depth);
            RegisterPlayer();
            ConfigureWallOcclusion(center);
        }

        private void BuildShell(
            Vector3 center,
            float width,
            float depth)
        {
            Material wall =
                _palette.Find("shell-wall");
            Material glass =
                _palette.Find("shell-glass");
            Material floor =
                _palette.Find("shell-floor");
            Material zone =
                _palette.Find("zone-marker");

            AddCube(
                "FloorOverlay",
                new Vector3(
                    width,
                    0.04f,
                    depth),
                center +
                Vector3.up * 0.02f,
                floor,
                false);

            float halfWidth = width * 0.5f;
            float halfDepth = depth * 0.5f;
            float wallY =
                _shell.WallHeight * 0.5f;

            CreateWall(
                "Wall_Left",
                new Vector3(
                    _shell.WallThickness,
                    _shell.WallHeight,
                    depth),
                center +
                new Vector3(
                    -halfWidth,
                    wallY,
                    0f),
                wall);

            CreateWall(
                "Wall_Right",
                new Vector3(
                    _shell.WallThickness,
                    _shell.WallHeight,
                    depth),
                center +
                new Vector3(
                    halfWidth,
                    wallY,
                    0f),
                wall);

            CreateWall(
                "Wall_Back",
                new Vector3(
                    width,
                    _shell.WallHeight,
                    _shell.WallThickness),
                center +
                new Vector3(
                    0f,
                    wallY,
                    halfDepth),
                wall);

            float entranceWidth =
                _shell.EntranceWidthCells *
                _shell.CellSize;
            float segmentWidth =
                (width - entranceWidth) *
                0.5f;

            CreateWall(
                "Wall_Front_Left",
                new Vector3(
                    segmentWidth,
                    _shell.WallHeight,
                    _shell.WallThickness),
                center +
                new Vector3(
                    -halfWidth +
                    segmentWidth * 0.5f,
                    wallY,
                    -halfDepth),
                wall);

            CreateWall(
                "Wall_Front_Right",
                new Vector3(
                    segmentWidth,
                    _shell.WallHeight,
                    _shell.WallThickness),
                center +
                new Vector3(
                    halfWidth -
                    segmentWidth * 0.5f,
                    wallY,
                    -halfDepth),
                wall);

            AddCube(
                "Window_Left",
                new Vector3(
                    segmentWidth * 0.75f,
                    _shell.WallHeight * 0.62f,
                    _shell.WallThickness * 0.55f),
                center +
                new Vector3(
                    -halfWidth +
                    segmentWidth * 0.5f,
                    wallY,
                    -halfDepth -
                    0.03f),
                glass,
                false);

            AddCube(
                "Window_Right",
                new Vector3(
                    segmentWidth * 0.75f,
                    _shell.WallHeight * 0.62f,
                    _shell.WallThickness * 0.55f),
                center +
                new Vector3(
                    halfWidth -
                    segmentWidth * 0.5f,
                    wallY,
                    -halfDepth -
                    0.03f),
                glass,
                false);

            GameObject entrance =
                new GameObject(
                    "EntranceAndDoor");
            entrance.transform.SetParent(
                _builtRoot.transform,
                false);
            entrance.transform.position =
                center +
                new Vector3(
                    0f,
                    0f,
                    -halfDepth);

            EntranceAnchor =
                CreateAnchor(
                    entrance.transform,
                    "EntranceAnchor",
                    Vector3.zero);

            GameObject leftPanel =
                AddCube(
                    "Door_Left",
                    new Vector3(
                        entranceWidth * 0.48f,
                        2.35f,
                        0.08f),
                    entrance.transform.position +
                    new Vector3(
                        -entranceWidth * 0.24f,
                        1.175f,
                        0f),
                    glass,
                    false);

            GameObject rightPanel =
                AddCube(
                    "Door_Right",
                    new Vector3(
                        entranceWidth * 0.48f,
                        2.35f,
                        0.08f),
                    entrance.transform.position +
                    new Vector3(
                        entranceWidth * 0.24f,
                        1.175f,
                        0f),
                    glass,
                    false);

            leftPanel.transform.SetParent(
                entrance.transform,
                true);
            rightPanel.transform.SetParent(
                entrance.transform,
                true);

            Door =
                entrance.AddComponent<
                    AutomaticSlidingDoorController>();

            Door.Configure(
                leftPanel.transform,
                rightPanel.transform,
                entranceWidth * 0.48f,
                _shell.DoorOpenDistance,
                _shell.DoorSpeed);

            float partitionZ =
                center.z +
                halfDepth -
                _shell.BackroomDepthMeters;

            CreateWall(
                "BackroomPartition_Left",
                new Vector3(
                    width * 0.38f,
                    _shell.WallHeight,
                    _shell.WallThickness),
                new Vector3(
                    center.x -
                    width * 0.31f,
                    wallY,
                    partitionZ),
                wall);

            CreateWall(
                "BackroomPartition_Right",
                new Vector3(
                    width * 0.38f,
                    _shell.WallHeight,
                    _shell.WallThickness),
                new Vector3(
                    center.x +
                    width * 0.31f,
                    wallY,
                    partitionZ),
                wall);

            AddCube(
                "StoreSign",
                new Vector3(
                    3.2f,
                    0.55f,
                    0.12f),
                center +
                new Vector3(
                    0f,
                    2.65f,
                    -halfDepth - 0.12f),
                zone,
                false);
        }

        private void BuildZones(
            Vector3 center,
            Vector3 origin,
            float width,
            float depth)
        {
            Material checkout =
                _palette.Find("zone-checkout");
            Material receiving =
                _palette.Find("zone-receiving");
            Material backroom =
                _palette.Find("zone-backroom");

            CheckoutAnchor =
                CreateZone(
                    "CheckoutZone",
                    center +
                    _shell.CheckoutZoneOffset,
                    _shell.CheckoutZoneSize,
                    checkout);

            ReceivingAnchor =
                CreateZone(
                    "ReceivingZone",
                    center +
                    _shell.ReceivingZoneOffset,
                    _shell.ReceivingZoneSize,
                    receiving);

            BackroomAnchor =
                CreateZone(
                    "BackroomZone",
                    center +
                    _shell.BackroomZoneOffset,
                    _shell.BackroomZoneSize,
                    backroom);

            AddCube(
                "BackroomStorageBlockout",
                new Vector3(
                    2.6f,
                    2.2f,
                    0.7f),
                BackroomAnchor.position +
                new Vector3(
                    -2.8f,
                    1.1f,
                    0.6f),
                _palette.Find(
                    "furniture-storage"),
                true);

            AddCube(
                "ReceivingCrateBlockout",
                new Vector3(
                    1.1f,
                    0.8f,
                    0.9f),
                ReceivingAnchor.position +
                new Vector3(
                    0f,
                    0.4f,
                    0f),
                _palette.Find(
                    "furniture-crate"),
                true);

            AddCube(
                "DecorationPlant",
                new Vector3(
                    0.55f,
                    1.2f,
                    0.55f),
                center +
                new Vector3(
                    -width * 0.38f,
                    0.6f,
                    -depth * 0.34f),
                _palette.Find(
                    "decoration"),
                false);
        }

        private void BuildLighting(
            Vector3 center,
            float width,
            float depth)
        {
            CaptureAmbientSettings();

            GameObject lighting =
                new GameObject("LightingRig");
            lighting.transform.SetParent(
                _builtRoot.transform,
                false);

            RenderSettings.ambientMode =
                UnityEngine.Rendering
                    .AmbientMode.Trilight;
            RenderSettings.ambientSkyColor =
                new Color(
                    0.42f,
                    0.39f,
                    0.31f);
            RenderSettings.ambientEquatorColor =
                new Color(
                    0.22f,
                    0.24f,
                    0.22f);
            RenderSettings.ambientGroundColor =
                new Color(
                    0.12f,
                    0.13f,
                    0.12f);
            RenderSettings.ambientIntensity =
                0.82f;

            CreateLight(
                lighting.transform,
                "GeneralLight_A",
                LightType.Point,
                center +
                new Vector3(
                    -width * 0.25f,
                    2.65f,
                    -depth * 0.2f),
                new Color(
                    1f,
                    0.82f,
                    0.62f),
                5.2f,
                9f);

            CreateLight(
                lighting.transform,
                "GeneralLight_B",
                LightType.Point,
                center +
                new Vector3(
                    width * 0.25f,
                    2.65f,
                    depth * 0.12f),
                new Color(
                    1f,
                    0.82f,
                    0.62f),
                5.2f,
                9f);

            CreateLight(
                lighting.transform,
                "EntranceAccent",
                LightType.Spot,
                EntranceAnchor.position +
                new Vector3(
                    0f,
                    2.6f,
                    1.2f),
                new Color(
                    0.65f,
                    0.95f,
                    0.82f),
                7f,
                6f,
                new Vector3(
                    70f,
                    0f,
                    0f));

            CreateLight(
                lighting.transform,
                "CheckoutAccent",
                LightType.Point,
                CheckoutAnchor.position +
                Vector3.up * 2.3f,
                new Color(
                    0.38f,
                    1f,
                    0.62f),
                4.5f,
                5f);

            CreateLight(
                lighting.transform,
                "FeaturedDisplayAccent",
                LightType.Spot,
                center +
                new Vector3(
                    0f,
                    2.8f,
                    0f),
                new Color(
                    0.45f,
                    0.88f,
                    1f),
                5f,
                6f,
                new Vector3(
                    90f,
                    0f,
                    0f));
        }

        private void RegisterPlayer()
        {
            StoreShellDescriptor descriptor =
                UnityEngine.Object
                    .FindFirstObjectByType<
                        StoreShellDescriptor>();

            Transform player =
                descriptor != null
                    ? descriptor.TechnicalPlayer
                    : null;

            if (player == null)
            {
                CharacterController controller =
                    UnityEngine.Object
                        .FindFirstObjectByType<
                            CharacterController>();

                player =
                    controller != null
                        ? controller.transform
                        : null;
            }

            if (player != null &&
                player.GetComponent<
                    Phase1CharacterPresence>() ==
                null)
            {
                Phase1CharacterPresence presence =
                    player.gameObject.AddComponent<
                        Phase1CharacterPresence>();
                presence.Configure(
                    "player",
                    Phase1CharacterRole.Player);
            }
        }

        private void ConfigureWallOcclusion(
            Vector3 center)
        {
            Camera camera =
                Camera.main ??
                UnityEngine.Object
                    .FindFirstObjectByType<Camera>();

            Transform target = null;

            StoreShellDescriptor descriptor =
                UnityEngine.Object
                    .FindFirstObjectByType<
                        StoreShellDescriptor>();

            if (descriptor != null)
            {
                target =
                    descriptor.TechnicalPlayer;
            }

            if (target == null)
            {
                Phase1CharacterPresence[] presences =
                    UnityEngine.Object
                        .FindObjectsByType<
                            Phase1CharacterPresence>(
                                FindObjectsInactive
                                    .Exclude,
                                FindObjectsSortMode.None);

                foreach (Phase1CharacterPresence
                         presence in presences)
                {
                    if (presence.Role ==
                        Phase1CharacterRole.Player)
                    {
                        target =
                            presence.transform;
                        break;
                    }
                }
            }

            if (target == null)
            {
                GameObject fallback =
                    new GameObject(
                        "CameraOcclusionTarget");
                fallback.transform.SetParent(
                    _builtRoot.transform,
                    false);
                fallback.transform.position =
                    center;
                target = fallback.transform;
            }

            WallOcclusion =
                _builtRoot.AddComponent<
                    Phase1WallOcclusionController>();

            WallOcclusion.Configure(
                camera,
                target,
                _settings.HideOccludingWalls);
        }

        private Transform CreateZone(
            string name,
            Vector3 position,
            Vector3 size,
            Material material)
        {
            GameObject zone =
                AddCube(
                    name,
                    size,
                    position,
                    material,
                    false);

            Collider collider =
                zone.GetComponent<Collider>();

            if (collider != null)
            {
                collider.enabled = false;
            }

            return zone.transform;
        }

        private void CreateWall(
            string name,
            Vector3 size,
            Vector3 position,
            Material material)
        {
            GameObject wall =
                AddCube(
                    name,
                    size,
                    position,
                    material,
                    true);

            wall.AddComponent<
                Phase1OccludableWall>();
        }

        private GameObject AddCube(
            string name,
            Vector3 scale,
            Vector3 position,
            Material material,
            bool keepCollider)
        {
            GameObject cube =
                Phase1BlockoutVisualFactory
                    .AddCube(
                        _builtRoot.transform,
                        name,
                        scale,
                        Vector3.zero,
                        material);

            cube.transform.position = position;

            Collider collider =
                cube.GetComponent<Collider>();

            if (collider != null &&
                !keepCollider)
            {
                collider.enabled = false;
            }

            return cube;
        }

        private static Transform CreateAnchor(
            Transform parent,
            string name,
            Vector3 localPosition)
        {
            GameObject anchor =
                new GameObject(name);
            anchor.transform.SetParent(
                parent,
                false);
            anchor.transform.localPosition =
                localPosition;
            return anchor.transform;
        }

        private static void CreateLight(
            Transform parent,
            string name,
            LightType type,
            Vector3 position,
            Color color,
            float intensity,
            float range,
            Vector3? rotation = null)
        {
            GameObject gameObject =
                new GameObject(name);
            gameObject.transform.SetParent(
                parent,
                false);
            gameObject.transform.position =
                position;

            if (rotation.HasValue)
            {
                gameObject.transform.eulerAngles =
                    rotation.Value;
            }

            Light light =
                gameObject.AddComponent<Light>();
            light.type = type;
            light.color = color;
            light.intensity = intensity;
            light.range = range;
            light.shadows =
                LightShadows.None;

            if (type == LightType.Spot)
            {
                light.spotAngle = 55f;
                light.innerSpotAngle = 30f;
            }
        }


        private void CaptureAmbientSettings()
        {
            if (_ambientCaptured)
            {
                return;
            }

            _ambientCaptured = true;
            _previousAmbientMode =
                RenderSettings.ambientMode;
            _previousAmbientSky =
                RenderSettings.ambientSkyColor;
            _previousAmbientEquator =
                RenderSettings.ambientEquatorColor;
            _previousAmbientGround =
                RenderSettings.ambientGroundColor;
            _previousAmbientIntensity =
                RenderSettings.ambientIntensity;
        }

        private void RestoreAmbientSettings()
        {
            if (!_ambientCaptured)
            {
                return;
            }

            RenderSettings.ambientMode =
                _previousAmbientMode;
            RenderSettings.ambientSkyColor =
                _previousAmbientSky;
            RenderSettings.ambientEquatorColor =
                _previousAmbientEquator;
            RenderSettings.ambientGroundColor =
                _previousAmbientGround;
            RenderSettings.ambientIntensity =
                _previousAmbientIntensity;

            _ambientCaptured = false;
        }


        private void SuppressLegacyShellVisuals()
        {
            if (_legacyRenderers.Count > 0)
            {
                return;
            }

            StoreShellDescriptor legacyShell =
                UnityEngine.Object
                    .FindFirstObjectByType<
                        StoreShellDescriptor>(
                            FindObjectsInactive
                                .Include);

            if (legacyShell == null ||
                legacyShell.transform
                    .IsChildOf(transform))
            {
                return;
            }

            Transform technicalPlayer =
                legacyShell.TechnicalPlayer;
            Transform placementRoot =
                FindDescendant(
                    legacyShell.transform,
                    "S5_StorePlacement");
            Transform ghostRoot =
                FindDescendant(
                    placementRoot,
                    "GhostVisual");
            Transform placedObjectsRoot =
                FindDescendant(
                    placementRoot,
                    "PlacedObjects");

            Renderer[] renderers =
                legacyShell
                    .GetComponentsInChildren<
                        Renderer>(true);

            foreach (Renderer renderer
                     in renderers)
            {
                if (renderer == null ||
                    IsWithin(
                        renderer.transform,
                        technicalPlayer) ||
                    IsWithin(
                        renderer.transform,
                        ghostRoot) ||
                    IsWithin(
                        renderer.transform,
                        placedObjectsRoot))
                {
                    continue;
                }

                _legacyRenderers.Add(
                    new LegacyRendererState(
                        renderer,
                        renderer.enabled));

                renderer.enabled = false;
            }
        }

        private void RestoreLegacyShellVisuals()
        {
            foreach (LegacyRendererState state
                     in _legacyRenderers)
            {
                if (state.Renderer != null)
                {
                    state.Renderer.enabled =
                        state.WasEnabled;
                }
            }

            _legacyRenderers.Clear();
        }

        private static Transform FindDescendant(
            Transform root,
            string name)
        {
            if (root == null)
            {
                return null;
            }

            Transform[] descendants =
                root.GetComponentsInChildren<
                    Transform>(true);

            foreach (Transform descendant
                     in descendants)
            {
                if (string.Equals(
                        descendant.name,
                        name,
                        StringComparison.Ordinal))
                {
                    return descendant;
                }
            }

            return null;
        }

        private static bool IsWithin(
            Transform candidate,
            Transform root)
        {
            return candidate != null &&
                root != null &&
                (candidate == root ||
                 candidate.IsChildOf(root));
        }

        private void OnDestroy()
        {
            RestoreLegacyShellVisuals();
            RestoreAmbientSettings();
        }

        private sealed class LegacyRendererState
        {
            public LegacyRendererState(
                Renderer renderer,
                bool wasEnabled)
            {
                Renderer = renderer;
                WasEnabled = wasEnabled;
            }

            public Renderer Renderer {
                get;
            }

            public bool WasEnabled {
                get;
            }
        }

        private void DestroyExisting()
        {
            GameObject existing =
                GameObject.Find(RootName);

            if (existing != null)
            {
                if (UnityEngine.Application
                    .isPlaying)
                {
                    Destroy(existing);
                }
                else
                {
                    DestroyImmediate(existing);
                }
            }
        }
    }
}
