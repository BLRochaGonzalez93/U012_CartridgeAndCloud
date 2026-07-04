using System;
using UnityEngine;

namespace VRMGames.CartridgeAndCloud.Presentation.Store.Authoring
{
    /// <summary>
    /// Identifies a fixture authored directly in StoreInitial and provides the
    /// stable placement data required to merge it with save-game state.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class AuthoredStoreFixture : MonoBehaviour
    {
        [SerializeField]
        private string _instanceId = string.Empty;

        [SerializeField]
        private string _definitionId = string.Empty;

        [SerializeField, Min(0)]
        private int _anchorX;

        [SerializeField, Min(0)]
        private int _anchorZ;

        [SerializeField, Range(0, 3)]
        private int _rotationQuarterTurns;

        [SerializeField]
        private string _initialProductId = string.Empty;

        [SerializeField, Min(0)]
        private int _initialProductQuantity;

        public string InstanceId => _instanceId;
        public string DefinitionId => _definitionId;
        public int AnchorX => _anchorX;
        public int AnchorZ => _anchorZ;
        public int RotationQuarterTurns => _rotationQuarterTurns;
        public string InitialProductId => _initialProductId;
        public int InitialProductQuantity => _initialProductQuantity;

        public void Configure(
            string instanceId,
            string definitionId,
            int anchorX,
            int anchorZ,
            int rotationQuarterTurns,
            string initialProductId = "",
            int initialProductQuantity = 0)
        {
            if (string.IsNullOrWhiteSpace(instanceId))
            {
                throw new ArgumentException(
                    "A stable fixture instance ID is required.",
                    nameof(instanceId));
            }

            if (string.IsNullOrWhiteSpace(definitionId))
            {
                throw new ArgumentException(
                    "A furniture definition ID is required.",
                    nameof(definitionId));
            }

            if (anchorX < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(anchorX));
            }

            if (anchorZ < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(anchorZ));
            }

            if (rotationQuarterTurns < 0 ||
                rotationQuarterTurns > 3)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(rotationQuarterTurns));
            }

            if (initialProductQuantity < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(initialProductQuantity));
            }

            if (initialProductQuantity > 0 &&
                string.IsNullOrWhiteSpace(initialProductId))
            {
                throw new ArgumentException(
                    "Initial stock requires a product ID.",
                    nameof(initialProductId));
            }

            _instanceId = instanceId.Trim();
            _definitionId = definitionId.Trim();
            _anchorX = anchorX;
            _anchorZ = anchorZ;
            _rotationQuarterTurns = rotationQuarterTurns;
            _initialProductId = initialProductId?.Trim() ?? string.Empty;
            _initialProductQuantity = initialProductQuantity;
        }

        private void OnValidate()
        {
            _instanceId = _instanceId?.Trim() ?? string.Empty;
            _definitionId = _definitionId?.Trim() ?? string.Empty;
            _anchorX = Mathf.Max(0, _anchorX);
            _anchorZ = Mathf.Max(0, _anchorZ);
            _rotationQuarterTurns =
                Mathf.Clamp(_rotationQuarterTurns, 0, 3);
            _initialProductId =
                _initialProductId?.Trim() ?? string.Empty;
            _initialProductQuantity =
                Mathf.Max(0, _initialProductQuantity);

            if (_initialProductQuantity == 0)
            {
                _initialProductId = string.Empty;
            }
        }
    }
}
