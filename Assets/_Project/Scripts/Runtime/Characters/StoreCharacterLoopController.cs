using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using VRMGames.CartridgeAndCloud.Application.Store;
using VRMGames.CartridgeAndCloud.Application.Customers;
using VRMGames.CartridgeAndCloud.Application.UIUX;
using VRMGames.CartridgeAndCloud.Domain.Characters;
using VRMGames.CartridgeAndCloud.Domain.Customers;
using VRMGames.CartridgeAndCloud.Domain.Placement;
using VRMGames.CartridgeAndCloud.Domain.Store;
using VRMGames.CartridgeAndCloud.Infrastructure.Store;
using VRMGames.CartridgeAndCloud.Infrastructure.Customers;
using VRMGames.CartridgeAndCloud.Runtime.Placement;
using VRMGames.CartridgeAndCloud.Runtime.Composition;
using VRMGames.CartridgeAndCloud.Presentation.Placement;
using VRMGames.CartridgeAndCloud.Presentation.Characters;
using VRMGames.CartridgeAndCloud.Presentation.Grounding;
namespace VRMGames.CartridgeAndCloud.Runtime.Characters
{
    public sealed class StoreCharacterLoopController :
        MonoBehaviour
    {
        private StoreOperationsFacade
            _service;
        private IStoreContentCatalog _catalog;
        private StorePresentationCatalogAsset
            _presentationCatalog;
        private Transform _entrance;
        private Transform _checkout;
        private Transform _receiving;
        private int _maximumCustomers;
        private int _activeCustomers;
        private int _nextRuntimeCustomerSequence = 1;
        private ICustomerActivityRuntime _customerActivity;
        private Transform _characterRoot;
        private Transform _employee;
        private IReadOnlyList<Transform> _exteriorSpawnAnchors = Array.Empty<Transform>();

        public int ActiveCustomerCount =>
            _customerActivity != null
                ? _customerActivity.ActiveCustomerCount
                : _activeCustomers;

        public int MaximumCustomerCount =>
            _customerActivity != null
                ? _customerActivity.MaximumCustomerCount
                : _maximumCustomers;

        public bool IsCustomerSequenceRunning =>
            ActiveCustomerCount > 0;

        public bool CanServeNextCustomer =>
            _customerActivity == null
                ? _activeCustomers < _maximumCustomers
                : _customerActivity
                    .EvaluateAdmission().Allowed;

        public StoreOperationResult
            LastCustomerPurchaseResult {
                get;
                private set;
            }

        public void Configure(
            StoreOperationsFacade service,
            IStoreContentCatalog catalog,
            StorePresentationCatalogAsset presentationCatalog,
            Transform entrance,
            Transform checkout,
            Transform receiving,
            int maximumCustomers)
        {
            Configure(
                service,
                catalog,
                presentationCatalog,
                entrance,
                checkout,
                receiving,
                maximumCustomers,
                Array.Empty<Transform>());
        }

        public void Configure(
            StoreOperationsFacade service,
            IStoreContentCatalog catalog,
            StorePresentationCatalogAsset presentationCatalog,
            Transform entrance,
            Transform checkout,
            Transform receiving,
            int maximumCustomers,
            IReadOnlyList<Transform> exteriorSpawnAnchors)
        {
            _service = service ??
                throw new ArgumentNullException(
                    nameof(service));
            _catalog = catalog ??
                throw new ArgumentNullException(
                    nameof(catalog));
            _presentationCatalog = presentationCatalog ??
                throw new ArgumentNullException(
                    nameof(presentationCatalog));
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
            _exteriorSpawnAnchors =
                exteriorSpawnAnchors ?? Array.Empty<Transform>();
            _customerActivity =
                UIRuntimeCompositionRoot.Instance
                    ?.StoreOperationalGate as
                    ICustomerActivityRuntime;
            _nextRuntimeCustomerSequence = 1;

            GameObject root =
                new GameObject(
                    "S16_P1_Characters");
            root.transform.SetParent(
                transform,
                false);
            _characterRoot = root.transform;

            SpawnEmployee();

            _service.StateChanged +=
                HandleStateChanged;
            SynchronizeEmployeePosition();
        }

        public StoreOperationResult
            TryServeNextCustomer()
        {
            if (_customerActivity == null &&
                _activeCustomers >=
                _maximumCustomers)
            {
                return StoreOperationResult
                    .Failure(
                        StoreOperationStatus
                            .CapacityExceeded,
                        "Maximum active customer capacity reached.");
            }

            int sequence =
                _nextRuntimeCustomerSequence++;
            CustomerInstanceId customerId =
                new CustomerInstanceId(
                    "customer-" +
                    sequence.ToString("0000"));

            if (_customerActivity != null)
            {
                StoreCustomerAdmissionDecision admission =
                    _customerActivity.TryAdmit(
                        customerId);

                if (!admission.Allowed)
                {
                    StoreOperationStatus status =
                        admission.FailureReason ==
                        StoreCustomerAdmissionFailureReason
                            .CapacityReached
                            ? StoreOperationStatus
                                .CapacityExceeded
                            : admission.FailureReason ==
                              StoreCustomerAdmissionFailureReason
                                  .CheckoutUnavailable
                                ? StoreOperationStatus
                                    .CheckoutRequired
                                : admission.FailureReason ==
                                  StoreCustomerAdmissionFailureReason
                                      .StoreNotOpen ||
                                  admission.FailureReason ==
                                  StoreCustomerAdmissionFailureReason
                                      .StoreClosing
                                    ? StoreOperationStatus
                                        .StoreMustBeOpen
                                    : StoreOperationStatus
                                        .InvalidState;

                    return StoreOperationResult.Failure(
                        status,
                        admission.Detail);
                }
            }

            LastCustomerPurchaseResult = null;
            _service.RecordCustomerVisit(customerId.Value);

            if (_customerActivity == null)
            {
                // Reserve fallback capacity synchronously so repeated
                // admissions in the same frame cannot exceed the limit.
                _activeCustomers++;
            }

            string customerActorId =
                sequence % 2 == 0
                    ? "customer-female-01"
                    : "customer-male-01";

            StartCoroutine(
                CustomerSequence(
                    customerId,
                    customerActorId));

            return StoreOperationResult.Success(
                "Customer admitted. " +
                ActiveCustomerCount + "/" +
                MaximumCustomerCount + " active.");
        }

        private void OnDestroy()
        {
            if (_service != null)
            {
                _service.StateChanged -=
                    HandleStateChanged;
            }
        }

        private void HandleStateChanged(
            StoreOperationsState state)
        {
            SynchronizeEmployeePosition();
        }

        private void SynchronizeEmployeePosition()
        {
            if (_employee == null ||
                _service == null ||
                _service.State == null)
            {
                return;
            }

            Transform checkoutRoot =
                FindCheckoutRootTransform();
            Transform checkout =
                ResolveEmployeeStandPoint(checkoutRoot);

            if (checkout == null)
            {
                return;
            }

            NavMeshAgent employeeAgent =
                _employee.GetComponent<NavMeshAgent>();

            if (employeeAgent != null &&
                NavMesh.SamplePosition(
                    checkout.position,
                    out NavMeshHit employeeHit,
                    2f,
                    NavMesh.AllAreas))
            {
                employeeAgent.Warp(employeeHit.position);
            }
            else
            {
                _employee.position = checkout.position;
                GroundingUtility.TrySnapRootToGround(_employee);
            }

            if (employeeAgent != null)
            {
                employeeAgent.updateRotation = false;
                if (employeeAgent.isOnNavMesh)
                {
                    employeeAgent.ResetPath();
                }
            }

            Transform lookTarget =
                FindNamedChild(checkoutRoot, "StaffLookTarget")
                ?? FindNamedChild(checkoutRoot, "CustomerCheckoutStandPoint")
                ?? checkoutRoot;

            if (lookTarget != null &&
                StaffOrientationUtility.TryResolveHorizontalRotation(
                    _employee.position,
                    lookTarget.position,
                    out Quaternion staffRotation))
            {
                _employee.rotation = staffRotation;
            }
        }

        private IEnumerator CustomerSequence(
            CustomerInstanceId customerId,
            string customerActorId)
        {
            Vector3 spawnPosition = ResolveExteriorSpawnPosition();
            GameObject customer = null;
            bool purchaseSucceeded = false;

            try
            {
                customer =
                    CharacterPrefabFactory.Instantiate(
                        _presentationCatalog,
                        customerActorId,
                        _characterRoot,
                        customerId.Value,
                        CharacterRole.Customer,
                        spawnPosition);

                Transform display =
                    FindStockedDisplayTransform(
                        customer.transform.position);

                if (display == null &&
                    CustomerVisitFallbackPolicy.ShouldBrowseWithoutPurchase(
                        hasVisibleProduct: false,
                        admissionAllowed: true))
                {
                    Transform fallback =
                        FindFallbackBrowseTransform(
                            customer.transform.position);
                    Vector3 destination = fallback != null
                        ? fallback.position
                        : _entrance.position + _entrance.forward * 1.4f;

                    NavMeshActorMovement.Result fallbackMovement =
                        new NavMeshActorMovement.Result();
                    yield return NavMeshActorMovement.MoveTo(
                        customer.transform,
                        destination,
                        2f,
                        fallbackMovement);

                    if (!fallbackMovement.Succeeded)
                    {
                        PublishNavigationFailure(
                            customer,
                            "the fallback browsing point",
                            fallbackMovement.FailureReason);
                        yield break;
                    }

                    SetCustomerState(
                        customerId,
                        ActiveCustomerState.Browsing);
                    _service.PublishFeedback(
                        new GameplayFeedbackEvent(
                            GameplayFeedbackType.CustomerFrustrated,
                            "Customer found no visible products and is leaving without a purchase.",
                            fallback != null ? fallback.name : "store-entrance"));

                    yield return AnimateState(
                        customer.transform,
                        "observe",
                        0.75f);
                    yield return AnimateState(
                        customer.transform,
                        "frustrated",
                        0.65f);

                    SetCustomerState(
                        customerId,
                        ActiveCustomerState.Leaving);
                    NavMeshActorMovement.Result fallbackExit =
                        new NavMeshActorMovement.Result();
                    yield return NavMeshActorMovement.MoveTo(
                        customer.transform,
                        _entrance.position - _entrance.forward * 1.8f,
                        2f,
                        fallbackExit);
                    yield break;
                }

                NavMeshActorMovement.Result browseMovement =
                    new NavMeshActorMovement.Result();
                yield return NavMeshActorMovement.MoveTo(
                    customer.transform,
                    display.position,
                    2f,
                    browseMovement);

                if (!browseMovement.Succeeded)
                {
                    PublishNavigationFailure(
                        customer,
                        "the selected display",
                        browseMovement.FailureReason);
                    yield break;
                }

                SetCustomerState(
                    customerId,
                    ActiveCustomerState.Browsing);

                _service.PublishFeedback(
                    new GameplayFeedbackEvent(
                        GameplayFeedbackType
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
                    new GameplayFeedbackEvent(
                        GameplayFeedbackType.Reserved,
                        "Product reserved for customer cart.",
                        display.name));

                Transform checkoutRoot =
                    FindCheckoutRootTransform();
                Vector3 checkoutDestination =
                    ResolveCustomerCheckoutPosition(
                        checkoutRoot,
                        customer.transform.position);

                SynchronizeEmployeePosition();

                NavMeshActorMovement.Result checkoutMovement =
                    new NavMeshActorMovement.Result();
                yield return NavMeshActorMovement.MoveTo(
                    customer.transform,
                    checkoutDestination,
                    2f,
                    checkoutMovement);

                if (!checkoutMovement.Succeeded)
                {
                    PublishNavigationFailure(
                        customer,
                        "the checkout point",
                        checkoutMovement.FailureReason);
                    yield break;
                }

                SetCustomerState(
                    customerId,
                    ActiveCustomerState.Queueing);

                yield return AnimateState(
                    customer.transform,
                    "queue",
                    0.7f);

                SetCustomerState(
                    customerId,
                    ActiveCustomerState.CheckingOut);

                StoreOperationResult purchase =
                    _service
                        .ProcessNextCustomerPurchase();

                LastCustomerPurchaseResult =
                    purchase;
                purchaseSucceeded = purchase.Succeeded;

                if (!purchase.Succeeded)
                {
                    _service.PublishFeedback(
                        new GameplayFeedbackEvent(
                            GameplayFeedbackType
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

                SetCustomerState(
                    customerId,
                    ActiveCustomerState.Leaving);

                NavMeshActorMovement.Result completedExit =
                    new NavMeshActorMovement.Result();
                yield return NavMeshActorMovement.MoveTo(
                    customer.transform,
                    _entrance.position +
                    _entrance.forward *
                    -1.8f,
                    2f,
                    completedExit);
            }
            finally
            {
                try
                {
                    _service.RecordCustomerOutcome(
                        customerId.Value,
                        purchaseSucceeded
                            ? StoreCustomerVisitOutcome.Purchased
                            : StoreCustomerVisitOutcome.Abandoned);
                }
                finally
                {
                    if (customer != null)
                    {
                        Destroy(customer);
                    }

                    if (_customerActivity != null)
                    {
                        _customerActivity.TrySetState(
                            customerId,
                            ActiveCustomerState.Despawned);
                    }
                    else
                    {
                        _activeCustomers =
                            Mathf.Max(
                                0,
                                _activeCustomers - 1);
                    }
                }
            }
        }

        private void SetCustomerState(
            CustomerInstanceId customerId,
            ActiveCustomerState state)
        {
            _customerActivity?.TrySetState(
                customerId,
                state);
        }

        private void SpawnEmployee()
        {
            GameObject employee =
                CharacterPrefabFactory.Instantiate(
                    _presentationCatalog,
                    "employee-female-01",
                    _characterRoot,
                    "employee-main",
                    CharacterRole.Employee,
                    _checkout.position +
                    _checkout.forward * 0.65f);

            _employee = employee.transform;
        }

        private Transform FindCheckoutRootTransform()
        {
            PlacedObjectView[] placedViews =
                UnityEngine.Object
                    .FindObjectsByType<PlacedObjectView>(
                        FindObjectsInactive.Exclude,
                        FindObjectsSortMode.None);

            PlacedFixtureVisual[] visuals =
                UnityEngine.Object
                    .FindObjectsByType<PlacedFixtureVisual>(
                        FindObjectsInactive.Exclude,
                        FindObjectsSortMode.None);

            foreach (PlacedStoreFixtureRecord fixture
                     in _service.State.Fixtures)
            {
                if (!_catalog.TryGetFurniture(
                        fixture.DefinitionId,
                        out StoreFixtureDefinition definition) ||
                    definition.Kind !=
                    StoreFixtureKind.CheckoutCounter)
                {
                    continue;
                }

                PlacementInstanceId targetId =
                    new PlacementInstanceId(fixture.InstanceId);

                foreach (PlacedObjectView placedView in placedViews)
                {
                    if (placedView.Id == targetId)
                    {
                        return placedView.transform;
                    }
                }

                foreach (PlacedFixtureVisual visual in visuals)
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

            return _checkout;
        }

        private static Transform ResolveEmployeeStandPoint(
            Transform checkoutRoot)
        {
            return FindNamedChild(
                       checkoutRoot,
                       "StaffPoint")
                   ?? FindNamedChild(
                       checkoutRoot,
                       "EmployeeStandPoint")
                   ?? checkoutRoot;
        }

        private static Vector3 ResolveCustomerCheckoutPosition(
            Transform checkoutRoot,
            Vector3 customerPosition)
        {
            if (checkoutRoot == null)
            {
                return customerPosition;
            }

            Transform explicitPoint =
                FindNamedChild(
                    checkoutRoot,
                    "CustomerCheckoutStandPoint")
                ?? FindNamedChild(
                    checkoutRoot,
                    "CustomerStandPoint");

            if (explicitPoint != null)
            {
                Vector3 explicitPosition = explicitPoint.position;
                explicitPosition.y = customerPosition.y;
                return explicitPosition;
            }

            Vector3 destination =
                checkoutRoot.position -
                checkoutRoot.forward * 0.8f;
            destination.y = customerPosition.y;
            return destination;
        }

        private Transform FindFallbackBrowseTransform(
            Vector3 customerPosition)
        {
            CustomerBrowseFixtureAuthoring[] browseFixtures =
                UnityEngine.Object.FindObjectsByType<CustomerBrowseFixtureAuthoring>(
                    FindObjectsInactive.Exclude,
                    FindObjectsSortMode.None);

            Transform nearest = null;
            float nearestDistance = float.PositiveInfinity;
            foreach (CustomerBrowseFixtureAuthoring browse in browseFixtures)
            {
                if (browse == null)
                {
                    continue;
                }

                foreach (Transform point in browse.Points)
                {
                    if (point == null)
                    {
                        continue;
                    }

                    Vector3 flatPoint = point.position;
                    flatPoint.y = customerPosition.y;
                    float distance =
                        (flatPoint - customerPosition).sqrMagnitude;
                    if (distance < nearestDistance)
                    {
                        nearestDistance = distance;
                        nearest = point;
                    }
                }
            }

            return nearest;
        }

        private Transform FindStockedDisplayTransform(
            Vector3 customerPosition)
        {
            PlacedFixtureVisual[] visuals =
                UnityEngine.Object.FindObjectsByType<PlacedFixtureVisual>(
                    FindObjectsInactive.Exclude,
                    FindObjectsSortMode.None);

            foreach (PlacedStoreFixtureRecord fixture
                     in _service.State.Fixtures)
            {
                if (_service.GetUnreservedDisplayQuantity(
                        fixture.InstanceId) < 1)
                {
                    continue;
                }

                foreach (PlacedFixtureVisual visual in visuals)
                {
                    if (!string.Equals(
                            visual.InstanceId,
                            fixture.InstanceId,
                            StringComparison.Ordinal))
                    {
                        continue;
                    }

                    CustomerBrowseFixtureAuthoring browse =
                        visual.GetComponentInChildren<CustomerBrowseFixtureAuthoring>(true);

                    Transform nearest = null;
                    float nearestDistance = float.PositiveInfinity;

                    if (browse != null)
                    {
                        foreach (Transform point in browse.Points)
                        {
                            if (point == null)
                            {
                                continue;
                            }

                            Vector3 flatPoint = point.position;
                            flatPoint.y = customerPosition.y;
                            float distance =
                                (flatPoint - customerPosition).sqrMagnitude;

                            if (distance < nearestDistance)
                            {
                                nearestDistance = distance;
                                nearest = point;
                            }
                        }
                    }

                    return nearest ?? visual.transform;
                }
            }

            return null;
        }

        private static Transform FindNamedChild(Transform root, string childName)
        {
            if (root == null)
            {
                return null;
            }

            foreach (Transform child in root.GetComponentsInChildren<Transform>(true))
            {
                if (string.Equals(child.name, childName, StringComparison.Ordinal))
                {
                    return child;
                }
            }

            return null;
        }

        private Vector3 ResolveExteriorSpawnPosition()
        {
            Vector3 fallback =
                _entrance.position -
                _entrance.forward * 2.2f;

            float minX = fallback.x - 5f;
            float maxX = fallback.x + 5f;
            float minZ = fallback.z - 2f;
            float maxZ = fallback.z + 2f;
            bool hasAnchor = false;

            foreach (Transform anchor in _exteriorSpawnAnchors)
            {
                if (anchor == null)
                {
                    continue;
                }

                if (!hasAnchor)
                {
                    minX = maxX = anchor.position.x;
                    minZ = maxZ = anchor.position.z;
                    hasAnchor = true;
                }
                else
                {
                    minX = Mathf.Min(minX, anchor.position.x);
                    maxX = Mathf.Max(maxX, anchor.position.x);
                    minZ = Mathf.Min(minZ, anchor.position.z);
                    maxZ = Mathf.Max(maxZ, anchor.position.z);
                }
            }

            if (!hasAnchor ||
                maxX - minX < 2f ||
                maxZ - minZ < 2f)
            {
                minX = fallback.x - 5f;
                maxX = fallback.x + 5f;
                minZ = fallback.z - 2f;
                maxZ = fallback.z + 2f;
            }

            const float margin = 0.45f;
            for (int attempt = 0; attempt < 16; attempt++)
            {
                Vector3 candidate = new Vector3(
                    UnityEngine.Random.Range(minX + margin, maxX - margin),
                    _entrance.position.y,
                    UnityEngine.Random.Range(minZ + margin, maxZ - margin));

                if (NavMesh.SamplePosition(
                        candidate,
                        out NavMeshHit hit,
                        1.5f,
                        NavMesh.AllAreas) &&
                    IsSpawnPositionClear(hit.position))
                {
                    return hit.position;
                }
            }

            if (NavMesh.SamplePosition(
                    fallback,
                    out NavMeshHit fallbackHit,
                    2f,
                    NavMesh.AllAreas))
            {
                return fallbackHit.position;
            }

            return fallback;
        }

        private static bool IsSpawnPositionClear(Vector3 position)
        {
            CharacterPresence[] characters =
                UnityEngine.Object.FindObjectsByType<CharacterPresence>(
                    FindObjectsInactive.Exclude,
                    FindObjectsSortMode.None);

            foreach (CharacterPresence character in characters)
            {
                if (character == null)
                {
                    continue;
                }

                Vector3 delta = character.transform.position - position;
                delta.y = 0f;
                if (delta.sqrMagnitude < 1.44f)
                {
                    return false;
                }
            }

            return true;
        }

        private void PublishNavigationFailure(
            GameObject customer,
            string destinationLabel,
            string failureReason)
        {
            string detail =
                $"Customer could not reach {destinationLabel}: {failureReason}";

            LastCustomerPurchaseResult =
                StoreOperationResult.Failure(
                    StoreOperationStatus.InvalidState,
                    detail);

            _service.PublishFeedback(
                new GameplayFeedbackEvent(
                    GameplayFeedbackType.CustomerFrustrated,
                    detail,
                    customer != null
                        ? customer.name
                        : "store-entrance"));
        }

        private static IEnumerator AnimateState(
            Transform target,
            string state,
            float duration)
        {
            if (target == null || duration <= 0f)
            {
                yield break;
            }

            // Capsules used scale pulses as temporary feedback. Humanoid
            // characters must keep a stable transform, so state feedback now
            // preserves the timing without deforming the character hierarchy.
            float elapsed = 0f;
            while (target != null && elapsed < duration)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }
        }
    }
}
