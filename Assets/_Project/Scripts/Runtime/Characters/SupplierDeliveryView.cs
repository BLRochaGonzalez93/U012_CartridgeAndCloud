using System;
using UnityEngine;

namespace VRMGames.CartridgeAndCloud.Runtime.Characters
{
    /// <summary>
    /// Authored visual container for one delivery. Runtime only populates its
    /// supplier and crate mounts; it never invents delivery hierarchy or sockets.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class SupplierDeliveryView : MonoBehaviour
    {
        [SerializeField]
        private Transform _supplierMount;

        [SerializeField]
        private Transform _crateMount;

        public Transform SupplierMount => _supplierMount;
        public Transform CrateMount => _crateMount;

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
