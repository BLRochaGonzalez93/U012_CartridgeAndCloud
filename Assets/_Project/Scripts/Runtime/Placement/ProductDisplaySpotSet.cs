using System;
using System.Collections.Generic;
using UnityEngine;

namespace VRMGames.CartridgeAndCloud.Runtime.Placement
{
    /// <summary>
    /// Authored/runtime-generated product positions belonging to a placed fixture.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class ProductDisplaySpotSet : MonoBehaviour
    {
        [SerializeField]
        private Transform[] _spots = Array.Empty<Transform>();

        public IReadOnlyList<Transform> Spots => _spots;

        public void Configure(Transform[] spots)
        {
            _spots = spots ?? Array.Empty<Transform>();
        }
    }
}
