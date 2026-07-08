using System;
using UnityEngine;

namespace VRMGames.CartridgeAndCloud.Presentation.Store.Authoring
{
    /// <summary>
    /// Immutable identity contract for a complete furniture prefab. Geometry,
    /// collision, anchors and grounding must already be authored in the prefab.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class StoreFixturePrefabAuthoring : MonoBehaviour
    {
        [SerializeField]
        private string _definitionId;

        [SerializeField, Min(0.01f)]
        private float _cellSize = 0.5f;

        public string DefinitionId => _definitionId;
        public float CellSize => _cellSize;

        public bool TryValidate(out string report)
        {
            if (string.IsNullOrWhiteSpace(_definitionId))
            {
                report = "Fixture definition ID is missing.";
                return false;
            }

            Transform collision = transform.Find("Collision");
            if (collision == null ||
                collision.GetComponent<BoxCollider>() == null)
            {
                report =
                    $"{name} is missing Collision/BoxCollider.";
                return false;
            }

            if (transform.Find("Anchors") == null)
            {
                report = $"{name} is missing Anchors.";
                return false;
            }

            PrefabPhysicalContractAuthoring physical =
                GetComponent<PrefabPhysicalContractAuthoring>();

            if (physical == null)
            {
                report =
                    $"{name} is missing PrefabPhysicalContractAuthoring.";
                return false;
            }

            if (!physical.TryValidate(out report))
            {
                return false;
            }

            if (!physical.TryGetGroundAnchor(
                    out Transform groundAnchor) ||
                groundAnchor == null)
            {
                report =
                    $"{name} has no valid ground reference.";
                return false;
            }

            report = "Furniture prefab contract is valid.";
            return true;
        }

        public void Configure(
            string definitionId,
            float cellSize)
        {
            _definitionId =
                definitionId?.Trim() ??
                string.Empty;

            _cellSize =
                Mathf.Max(0.01f, cellSize);
        }
    }
}
