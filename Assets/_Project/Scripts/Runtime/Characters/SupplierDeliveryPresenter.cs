using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using VRMGames.CartridgeAndCloud.Domain.Characters;
using VRMGames.CartridgeAndCloud.Infrastructure.Store;
using VRMGames.CartridgeAndCloud.Runtime.Store;

namespace VRMGames.CartridgeAndCloud.Runtime.Characters
{
    /// <summary>
    /// Presents one visual delivery per correlation ID. Deliveries are queued so
    /// only one supplier can use the entrance and receiving passage at a time.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class SupplierDeliveryPresenter : MonoBehaviour
    {
        private const float DeliveryCrateScale = 0.55f;

        private readonly DeliveryPresentationRegistry _registry =
            new DeliveryPresentationRegistry();
        private readonly Queue<string> _pendingDeliveries =
            new Queue<string>();

        private StorePresentationCatalogAsset _presentationCatalog;
        private Transform _entrance;
        private Transform _receiving;
        private Transform _presentationRoot;
        private bool _isProcessingQueue;

        public int ActiveDeliveryCount => _registry.ActiveCount;

        public void Configure(
            StorePresentationCatalogAsset presentationCatalog,
            Transform entrance,
            Transform receiving)
        {
            _presentationCatalog = presentationCatalog ??
                throw new ArgumentNullException(nameof(presentationCatalog));
            _entrance = entrance ??
                throw new ArgumentNullException(nameof(entrance));
            _receiving = receiving ??
                throw new ArgumentNullException(nameof(receiving));

            if (_presentationCatalog.SupplierDeliveryViewPrefab == null)
            {
                throw new InvalidOperationException(
                    "Presentation catalog requires SupplierDeliveryView.prefab.");
            }

            Transform existing = transform.Find("SupplierDeliveries");
            if (existing != null)
            {
                Destroy(existing.gameObject);
            }

            GameObject root = new GameObject("SupplierDeliveries");
            root.transform.SetParent(transform, false);
            _presentationRoot = root.transform;
        }

        public bool Present(string deliveryId)
        {
            if (!_registry.TryBegin(deliveryId))
            {
                return false;
            }

            string normalizedId = deliveryId.Trim();
            _pendingDeliveries.Enqueue(normalizedId);

            if (!_isProcessingQueue)
            {
                StartCoroutine(ProcessQueue());
            }

            return true;
        }

        public bool HasPresented(string deliveryId)
        {
            return _registry.HasSeen(deliveryId);
        }

        private IEnumerator ProcessQueue()
        {
            _isProcessingQueue = true;
            try
            {
                while (_pendingDeliveries.Count > 0)
                {
                    string deliveryId = _pendingDeliveries.Dequeue();
                    yield return PresentRoutine(deliveryId);
                }
            }
            finally
            {
                _isProcessingQueue = false;
            }
        }

        private IEnumerator PresentRoutine(string deliveryId)
        {
            GameObject deliveryRoot = null;
            bool completed = false;

            try
            {
                Vector3 spawn = ResolveExteriorSpawnPosition();
                deliveryRoot = Instantiate(
                    _presentationCatalog.SupplierDeliveryViewPrefab,
                    spawn,
                    Quaternion.identity,
                    _presentationRoot);
                deliveryRoot.name = "delivery-" + deliveryId;

                SupplierDeliveryView view =
                    deliveryRoot.GetComponent<SupplierDeliveryView>();
                if (view == null)
                {
                    throw new InvalidOperationException(
                        "SupplierDeliveryView prefab is missing its authored component.");
                }

                view.ValidateOrThrow();
                string actorId = ComputeDeterministicParity(deliveryId) == 0
                    ? "supplier-female-01"
                    : "supplier-male-01";

                GameObject supplier = CharacterPrefabFactory.Instantiate(
                    _presentationCatalog,
                    actorId,
                    view.SupplierMount,
                    "supplier-" + deliveryId,
                    CharacterRole.Supplier,
                    spawn);

                GameObject crate = StorePrefabFactory.InstantiateStandaloneFurniture(
                    "receiving-crate",
                    view.CrateMount,
                    "delivery-crate-" + deliveryId,
                    spawn);

                Transform carrySocket = FindNamedChild(
                    supplier.transform,
                    "CarrySocket");
                if (carrySocket == null)
                {
                    throw new InvalidOperationException(
                        $"Supplier prefab '{supplier.name}' is missing CarrySocket.");
                }

                SetPhysicalState(crate, false);
                AttachCrateToCarrySocket(crate, carrySocket);

                NavMeshActorMovement.Result inbound =
                    new NavMeshActorMovement.Result();
                yield return NavMeshActorMovement.MoveTo(
                    supplier.transform,
                    _receiving.position,
                    1.8f,
                    inbound);

                if (!inbound.Succeeded)
                {
                    Debug.LogWarning(
                        $"[Delivery] Supplier '{deliveryId}' could not reach receiving: " +
                        inbound.FailureReason,
                        supplier);
                    yield break;
                }

                yield return new WaitForSeconds(0.8f);

                crate.transform.SetParent(view.CrateMount, true);
                crate.transform.SetPositionAndRotation(
                    _receiving.position,
                    _receiving.rotation);
                crate.transform.localScale =
                    Vector3.one * DeliveryCrateScale;
                SetDroppedCrateState(crate);

                NavMeshActorMovement.Result outbound =
                    new NavMeshActorMovement.Result();
                yield return NavMeshActorMovement.MoveTo(
                    supplier.transform,
                    _entrance.position - _entrance.forward * 1.8f,
                    1.8f,
                    outbound);

                if (!outbound.Succeeded)
                {
                    Debug.LogWarning(
                        $"[Delivery] Supplier '{deliveryId}' could not complete its exit: " +
                        outbound.FailureReason,
                        supplier);
                }

                Destroy(supplier);
                yield return new WaitForSeconds(2f);
                completed = true;
                _registry.Complete(deliveryId);
            }
            finally
            {
                if (!completed)
                {
                    _registry.Cancel(deliveryId);
                }

                if (deliveryRoot != null)
                {
                    Destroy(deliveryRoot);
                }
            }
        }

        private static void AttachCrateToCarrySocket(
            GameObject crate,
            Transform carrySocket)
        {
            crate.transform.SetParent(carrySocket, false);
            crate.transform.localPosition = Vector3.zero;
            crate.transform.localRotation = Quaternion.identity;
            crate.transform.localScale = Vector3.one * DeliveryCrateScale;

            Transform crateCarrySocket = FindNamedChild(
                crate.transform,
                "CarrySocket");
            if (crateCarrySocket != null)
            {
                crate.transform.position +=
                    carrySocket.position - crateCarrySocket.position;
            }
        }

        private static int ComputeDeterministicParity(string value)
        {
            int parity = 0;
            foreach (char character in value)
            {
                parity ^= character;
            }

            return parity & 1;
        }

        private Vector3 ResolveExteriorSpawnPosition()
        {
            Vector3 candidate = _entrance.position - _entrance.forward * 3f;
            if (NavMesh.SamplePosition(
                    candidate,
                    out NavMeshHit hit,
                    4f,
                    NavMesh.AllAreas))
            {
                return hit.position;
            }

            return candidate;
        }

        private static Transform FindNamedChild(Transform root, string name)
        {
            foreach (Transform child in root.GetComponentsInChildren<Transform>(true))
            {
                if (string.Equals(child.name, name, StringComparison.Ordinal))
                {
                    return child;
                }
            }

            return null;
        }

        private static void SetPhysicalState(GameObject target, bool enabled)
        {
            foreach (Collider collider in target.GetComponentsInChildren<Collider>(true))
            {
                collider.enabled = enabled;
            }

            foreach (NavMeshObstacle obstacle in
                     target.GetComponentsInChildren<NavMeshObstacle>(true))
            {
                obstacle.enabled = enabled;
            }
        }

        private static void SetDroppedCrateState(GameObject crate)
        {
            foreach (Collider collider in crate.GetComponentsInChildren<Collider>(true))
            {
                collider.enabled = true;
            }

            // The delivery crate is a transient presentation object. It must not
            // carve the narrow receiving route while the supplier is leaving.
            foreach (NavMeshObstacle obstacle in
                     crate.GetComponentsInChildren<NavMeshObstacle>(true))
            {
                obstacle.enabled = false;
            }
        }
    }
}
