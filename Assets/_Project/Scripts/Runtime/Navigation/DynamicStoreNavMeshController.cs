using System;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using VRMGames.CartridgeAndCloud.Presentation.Store.Authoring;

namespace VRMGames.CartridgeAndCloud.Runtime.Navigation
{
    /// <summary>
    /// Builds one NavMesh from the authored Navigation subtree. Runtime economy,
    /// inventory and door events never expand or replace the source geometry.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class DynamicStoreNavMeshController : MonoBehaviour
    {
        private NavMeshSurface _surface;
        private StoreNavigationAuthoring _authoring;

        public NavMeshSurface Surface => _surface;

        public void Configure(StoreNavigationAuthoring authoring)
        {
            _authoring = authoring ?? throw new ArgumentNullException(nameof(authoring));
            _authoring.ValidateOrThrow();

            GameObject owner = _authoring.NavigationRoot.gameObject;
            _surface = owner.GetComponent<NavMeshSurface>();
            if (_surface == null)
            {
                throw new InvalidOperationException(
                    "The authored Navigation root requires one serialized NavMeshSurface.");
            }

            _surface.collectObjects = CollectObjects.Children;
            _surface.useGeometry = NavMeshCollectGeometry.PhysicsColliders;
            _surface.layerMask = ~0;
            BuildFromAuthoredSources();
        }

        public void RebuildFromAuthoredSources()
        {
            if (_surface == null || _authoring == null)
            {
                throw new InvalidOperationException(
                    "Navigation must be configured before rebuilding.");
            }

            _authoring.ValidateOrThrow();
            BuildFromAuthoredSources();
        }

        private void BuildFromAuthoredSources()
        {
            SetBlockingSourcesEnabled(true);
            try
            {
                _surface.BuildNavMesh();
            }
            finally
            {
                SetBlockingSourcesEnabled(false);
            }
        }

        private void SetBlockingSourcesEnabled(bool enabled)
        {
            foreach (BoxCollider blocker in _authoring.BlockingSources)
            {
                if (blocker != null)
                {
                    blocker.enabled = enabled;
                }
            }
        }
    }
}
