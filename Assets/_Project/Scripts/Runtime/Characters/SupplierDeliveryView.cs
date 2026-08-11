using System;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Presentation.Interaction;

namespace VRMGames.CartridgeAndCloud.Runtime.Characters
{
    /// <summary>
    /// Authored visual container for one delivery. Runtime only populates its
    /// supplier and crate mounts; it never invents delivery hierarchy or sockets.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class SupplierDeliveryView :
        MonoBehaviour,
        IWorldInteractionTarget
    {
        [SerializeField]
        private Transform _supplierMount;

        [SerializeField]
        private Transform _crateMount;

        private string _deliveryId = string.Empty;

        public Transform SupplierMount => _supplierMount;
        public Transform CrateMount => _crateMount;

        public string InteractionId =>
            _deliveryId;

        public WorldInteractionKind InteractionKind =>
            WorldInteractionKind.Delivery;

        public Transform InteractionTransform =>
            _crateMount != null
                ? _crateMount
                : transform;

        public float InteractionRange => 2.2f;

        public int InteractionPriority => 85;

        public bool IsInteractionAvailable =>
            !string.IsNullOrWhiteSpace(_deliveryId);

        public Vector3 GetInteractionPoint(
            Vector3 actorPosition)
        {
            return WorldInteractionGeometry.ResolveClosestPoint(
                InteractionTransform,
                actorPosition);
        }

        public void ConfigureInteraction(
            string deliveryId)
        {
            _deliveryId =
                deliveryId?.Trim() ?? string.Empty;
        }

        public void ValidateOrThrow()
        {
            if (_supplierMount == null || _crateMount == null)
            {
                throw new InvalidOperationException(
                    $"Supplier delivery view '{name}' requires both authored mounts.");
            }
        }
    }
}
