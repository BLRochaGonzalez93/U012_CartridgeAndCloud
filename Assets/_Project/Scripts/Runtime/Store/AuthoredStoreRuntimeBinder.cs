using System;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Infrastructure.Store;
using VRMGames.CartridgeAndCloud.Presentation.Store.Authoring;
using VRMGames.CartridgeAndCloud.Presentation.Store.Doors;
using VRMGames.CartridgeAndCloud.Presentation.Store.Occlusion;

namespace VRMGames.CartridgeAndCloud.Runtime.Store
{
    /// <summary>
    /// Binds the authored StoreInitial contract to runtime services. It never
    /// creates shell geometry, fixtures, colliders, anchors or fallback visuals.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class AuthoredStoreRuntimeBinder : MonoBehaviour
    {
        private StoreLayoutAsset _shell;
        private StoreRuntimeSettingsAsset _settings;

        public Transform EntranceAnchor { get; private set; }
        public Transform CheckoutAnchor { get; private set; }
        public Transform ReceivingAnchor { get; private set; }
        public Transform BackroomAnchor { get; private set; }
        public AutomaticSlidingDoorController Door { get; private set; }
        public WallOcclusionController WallOcclusion { get; private set; }

        public void Configure(
            StoreLayoutAsset shell,
            StoreRuntimeSettingsAsset settings)
        {
            _shell = shell ?? throw new ArgumentNullException(nameof(shell));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public void Bind(StoreInitialSceneContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (_shell == null || _settings == null)
            {
                throw new InvalidOperationException(
                    "Authored store binder must be configured before binding.");
            }

            context.ValidateOrThrow();

            EntranceAnchor = context.EntranceAnchor;
            CheckoutAnchor = context.CheckoutAnchor;
            ReceivingAnchor = context.ReceivingAnchor;
            BackroomAnchor = context.BackroomAnchor;
            Door = context.Door;
            WallOcclusion = context.WallOcclusion;

            float entranceWidth = _shell.EntranceWidthCells * _shell.CellSize;
            Door.Configure(
                context.DoorParts.LeftPanel,
                context.DoorParts.RightPanel,
                entranceWidth * 0.48f,
                _shell.DoorOpenDistance,
                _shell.DoorSpeed);

            if (WallOcclusion != null)
            {
                WallOcclusion.Configure(
                    context.GameplayCamera,
                    context.TechnicalPlayer,
                    _settings.HideOccludingWalls);
            }
        }
    }
}
