using System;
using System.Collections;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Application.VerticalSlicePhase1;
using VRMGames.CartridgeAndCloud.Domain.VerticalSlicePhase1;
using VRMGames.CartridgeAndCloud.Infrastructure.VerticalSlicePhase1;

namespace VRMGames.CartridgeAndCloud.Runtime.VerticalSlicePhase1
{
    public sealed class Phase1CharacterLoopController :
        MonoBehaviour
    {
        private Phase1VerticalSliceService
            _service;
        private IPhase1Catalog _catalog;
        private Phase1MaterialPaletteAsset
            _palette;
        private Transform _entrance;
        private Transform _checkout;
        private Transform _receiving;
        private int _maximumCustomers;
        private int _activeCustomers;
        private Transform _characterRoot;

        public bool IsCustomerSequenceRunning =>
            _activeCustomers > 0;

        public Phase1OperationResult
            LastCustomerPurchaseResult {
                get;
                private set;
            }

        public void Configure(
            Phase1VerticalSliceService service,
            IPhase1Catalog catalog,
            Phase1MaterialPaletteAsset palette,
            Transform entrance,
            Transform checkout,
            Transform receiving,
            int maximumCustomers)
        {
            _service = service ??
                throw new ArgumentNullException(
                    nameof(service));
            _catalog = catalog ??
                throw new ArgumentNullException(
                    nameof(catalog));
            _palette = palette ??
                throw new ArgumentNullException(
                    nameof(palette));
            _entrance = entrance ??
                throw new ArgumentNullException(
                    nameof(entrance));
            _checkout = checkout ??
                throw new ArgumentNullException(
                    nameof(checkout));
            _receiving = receiving ??
                throw new ArgumentNullException(
                    nameof(receiving));
            _maximumCustomers =
                Mathf.Max(1, maximumCustomers);

            GameObject root =
                new GameObject(
                    "S16_P1_Characters");
            root.transform.SetParent(
                transform,
                false);
            _characterRoot = root.transform;

            SpawnEmployee();
            SpawnSupplierPlaceholder();
        }

        public Phase1OperationResult
            TryServeNextCustomer()
        {
            if (_activeCustomers >=
                _maximumCustomers)
            {
                return Phase1OperationResult
                    .Failure(
                        Phase1OperationStatus
                            .InvalidState,
                        "Maximum blockout customers reached.");
            }

            Phase1OperationResult validation =
                _service
                    .ValidateNextCustomerPurchase();

            if (!validation.Succeeded)
            {
                LastCustomerPurchaseResult =
                    validation;

                _service.PublishFeedback(
                    new Phase1FeedbackEvent(
                        Phase1FeedbackKind
                            .CustomerFrustrated,
                        validation.Detail,
                        "store-entrance"));

                return validation;
            }

            LastCustomerPurchaseResult = null;

            StartCoroutine(
                CustomerSequence());

            return Phase1OperationResult.Success(
                "Customer sequence started.");
        }

        public void PresentSupplierDelivery()
        {
            StartCoroutine(
                SupplierDeliverySequence());
        }

        private IEnumerator CustomerSequence()
        {
            _activeCustomers++;

            string customerId =
                "customer-" +
                _service.State
                    .NextCustomerSequence
                    .ToString("0000");

            GameObject customer =
                Phase1BlockoutVisualFactory
                    .BuildCharacter(
                        _characterRoot,
                        customerId,
                        Phase1CharacterRole.Customer,
                        _palette.Find(
                            "character-customer"),
                        _entrance.position +
                        _entrance.forward *
                        -1.4f);

            try
            {
                Transform display =
                    FindStockedDisplayTransform();

                if (display == null)
                {
                    LastCustomerPurchaseResult =
                        Phase1OperationResult
                            .Failure(
                                Phase1OperationStatus
                                    .InvalidState,
                                "The stocked display visual is unavailable.");

                    _service.PublishFeedback(
                        new Phase1FeedbackEvent(
                            Phase1FeedbackKind
                                .CustomerFrustrated,
                            LastCustomerPurchaseResult
                                .Detail,
                            "store-entrance"));

                    yield return AnimateState(
                        customer.transform,
                        "frustrated",
                        0.9f);

                    yield return MoveTo(
                        customer.transform,
                        _entrance.position +
                        _entrance.forward *
                        -1.8f,
                        2f);
                    yield break;
                }

                yield return MoveTo(
                    customer.transform,
                    display.position +
                    Vector3.forward * 0.8f,
                    2f);

                _service.PublishFeedback(
                    new Phase1FeedbackEvent(
                        Phase1FeedbackKind
                            .ObjectHovered,
                        "Customer is evaluating a product.",
                        display.name));

                yield return AnimateState(
                    customer.transform,
                    "observe",
                    0.9f);

                yield return AnimateState(
                    customer.transform,
                    "pick",
                    0.55f);

                _service.PublishFeedback(
                    new Phase1FeedbackEvent(
                        Phase1FeedbackKind.Reserved,
                        "Product reserved for customer cart.",
                        display.name));

                yield return MoveTo(
                    customer.transform,
                    _checkout.position +
                    Vector3.back * 0.8f,
                    2f);

                yield return AnimateState(
                    customer.transform,
                    "queue",
                    0.7f);

                Phase1OperationResult purchase =
                    _service
                        .ProcessNextCustomerPurchase();

                LastCustomerPurchaseResult =
                    purchase;

                if (!purchase.Succeeded)
                {
                    _service.PublishFeedback(
                        new Phase1FeedbackEvent(
                            Phase1FeedbackKind
                                .CustomerFrustrated,
                            purchase.Detail,
                            "customer"));
                }

                yield return AnimateState(
                    customer.transform,
                    purchase.Succeeded
                        ? "satisfied"
                        : "frustrated",
                    0.85f);

                yield return MoveTo(
                    customer.transform,
                    _entrance.position +
                    _entrance.forward *
                    -1.8f,
                    2f);
            }
            finally
            {
                if (customer != null)
                {
                    Destroy(customer);
                }

                _activeCustomers--;
            }
        }

        private IEnumerator
            SupplierDeliverySequence()
        {
            GameObject supplier =
                Phase1BlockoutVisualFactory
                    .BuildCharacter(
                        _characterRoot,
                        "supplier-delivery",
                        Phase1CharacterRole.Supplier,
                        _palette.Find(
                            "character-supplier"),
                        _entrance.position +
                        _entrance.forward *
                        -1.6f);

            GameObject crate =
                GameObject.CreatePrimitive(
                    PrimitiveType.Cube);
            crate.name =
                "SupplierDeliveryCrate";
            crate.transform.SetParent(
                supplier.transform,
                false);
            crate.transform.localPosition =
                new Vector3(
                    0f,
                    0.7f,
                    0.55f);
            crate.transform.localScale =
                new Vector3(
                    0.55f,
                    0.45f,
                    0.45f);

            Renderer crateRenderer =
                crate.GetComponent<Renderer>();

            if (crateRenderer != null)
            {
                crateRenderer.sharedMaterial =
                    _palette.Find(
                        "furniture-crate");
            }

            yield return MoveTo(
                supplier.transform,
                _receiving.position,
                1.8f);

            yield return AnimateState(
                supplier.transform,
                "move-crate",
                0.8f);

            crate.transform.SetParent(
                _characterRoot,
                true);
            crate.transform.position =
                _receiving.position +
                Vector3.up * 0.4f;

            yield return MoveTo(
                supplier.transform,
                _entrance.position +
                _entrance.forward *
                -1.8f,
                1.8f);

            Destroy(supplier);
            Destroy(crate, 2f);
        }

        private void SpawnEmployee()
        {
            Phase1BlockoutVisualFactory
                .BuildCharacter(
                    _characterRoot,
                    "employee-main",
                    Phase1CharacterRole.Employee,
                    _palette.Find(
                        "character-employee"),
                    _checkout.position +
                    Vector3.forward * 0.65f);
        }

        private void SpawnSupplierPlaceholder()
        {
            GameObject supplier =
                Phase1BlockoutVisualFactory
                    .BuildCharacter(
                        _characterRoot,
                        "supplier-placeholder",
                        Phase1CharacterRole.Supplier,
                        _palette.Find(
                            "character-supplier"),
                        _receiving.position +
                        Vector3.right * 1.1f);

            supplier.SetActive(false);
        }

        private Transform
            FindStockedDisplayTransform()
        {
            foreach (Phase1PlacedFixtureRecord
                     fixture in _service.State.Fixtures)
            {
                if (fixture.ProductQuantity < 1)
                {
                    continue;
                }

                Phase1PlacedFixtureVisual[] visuals =
                    UnityEngine.Object
                        .FindObjectsByType<
                            Phase1PlacedFixtureVisual>(
                                FindObjectsInactive
                                    .Exclude,
                                FindObjectsSortMode.None);

                foreach (Phase1PlacedFixtureVisual
                         visual in visuals)
                {
                    if (string.Equals(
                            visual.InstanceId,
                            fixture.InstanceId,
                            StringComparison.Ordinal))
                    {
                        return visual.transform;
                    }
                }
            }

            return null;
        }

        private static IEnumerator MoveTo(
            Transform target,
            Vector3 destination,
            float speed)
        {
            while (target != null &&
                   (target.position - destination)
                       .sqrMagnitude > 0.02f)
            {
                Vector3 direction =
                    destination -
                    target.position;
                direction.y = 0f;

                if (direction.sqrMagnitude >
                    0.001f)
                {
                    target.rotation =
                        Quaternion.Slerp(
                            target.rotation,
                            Quaternion.LookRotation(
                                direction.normalized,
                                Vector3.up),
                            10f *
                            Time.unscaledDeltaTime);
                }

                target.position =
                    Vector3.MoveTowards(
                        target.position,
                        destination,
                        speed *
                        Time.unscaledDeltaTime);

                float bob =
                    Mathf.Sin(
                        Time.unscaledTime *
                        12f) *
                    0.025f;
                target.localScale =
                    new Vector3(
                        1f,
                        1f + bob,
                        1f);

                yield return null;
            }

            if (target != null)
            {
                target.localScale =
                    Vector3.one;
            }
        }

        private static IEnumerator AnimateState(
            Transform target,
            string state,
            float duration)
        {
            if (target == null)
            {
                yield break;
            }

            Vector3 original =
                target.localScale;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed +=
                    Time.unscaledDeltaTime;
                float normalized =
                    Mathf.Clamp01(
                        elapsed / duration);

                float pulse =
                    Mathf.Sin(
                        normalized *
                        Mathf.PI *
                        (state == "frustrated"
                            ? 4f
                            : 2f));

                float amplitude =
                    state == "satisfied"
                        ? 0.12f
                        : state == "frustrated"
                            ? 0.08f
                            : 0.04f;

                target.localScale =
                    original *
                    (1f + pulse * amplitude);

                yield return null;
            }

            if (target != null)
            {
                target.localScale = original;
            }
        }
    }
}
