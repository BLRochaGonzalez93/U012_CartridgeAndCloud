using System;
using UnityEngine;
using UnityEngine.AI;
using VRMGames.CartridgeAndCloud.Application.Customers;
using VRMGames.CartridgeAndCloud.Application.GameSession;
using VRMGames.CartridgeAndCloud.Application.Store;
using VRMGames.CartridgeAndCloud.Domain.Customers;
using VRMGames.CartridgeAndCloud.Domain.Placement;
using VRMGames.CartridgeAndCloud.Domain.Store;
using VRMGames.CartridgeAndCloud.Presentation.Placement;
using VRMGames.CartridgeAndCloud.Runtime.Placement;

namespace VRMGames.CartridgeAndCloud.Runtime.Store
{
    public sealed class StoreOperationalGate :
        ICustomerActivityRuntime
    {
        private const float ControlledClosingDelaySeconds = 1f;

        private readonly StoreOperationsFacade _service;
        private readonly IStoreContentCatalog _catalog;
        private readonly ActiveCustomerRegistry _customers;
        private readonly StoreCustomerAdmissionPolicy _policy;
        private readonly int _maximumCustomers;
        private readonly IActiveGameSession
            _activeSession;

        private float _invalidCheckoutSeconds;
        private bool _controlledClosingRequested;
        private bool _checkoutOperational;
        private bool _admissionBlocked;
        private string _statusDetail = string.Empty;

        public int ActiveCustomerCount =>
            _customers.ActiveCount;

        public int MaximumCustomerCount =>
            _maximumCustomers;

        public bool CheckoutOperational =>
            _checkoutOperational;

        public bool AdmissionBlocked =>
            _admissionBlocked;

        public string StatusDetail =>
            _statusDetail;

        public StoreOperationalGate(
            StoreOperationsFacade service,
            IStoreContentCatalog catalog,
            int maximumCustomers,
            IActiveGameSession activeSession)
        {
            _service = service ??
                throw new ArgumentNullException(
                    nameof(service));
            _catalog = catalog ??
                throw new ArgumentNullException(
                    nameof(catalog));

            if (maximumCustomers <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(maximumCustomers));
            }

            _maximumCustomers = maximumCustomers;
            _activeSession = activeSession ??
                throw new ArgumentNullException(
                    nameof(activeSession));
            _customers = new ActiveCustomerRegistry();
            _policy = new StoreCustomerAdmissionPolicy();
            RefreshCheckoutStatus();
        }

        public void Tick(float unscaledDeltaTime)
        {
            if (unscaledDeltaTime < 0f ||
                float.IsNaN(unscaledDeltaTime) ||
                float.IsInfinity(unscaledDeltaTime))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(unscaledDeltaTime));
            }

            RefreshCheckoutStatus();

            if (!IsState("Open") ||
                _checkoutOperational)
            {
                _invalidCheckoutSeconds = 0f;
                _controlledClosingRequested = false;
                _admissionBlocked = false;
                return;
            }

            _admissionBlocked = true;
            _invalidCheckoutSeconds +=
                unscaledDeltaTime;

            if (_invalidCheckoutSeconds >=
                ControlledClosingDelaySeconds)
            {
                _controlledClosingRequested = true;
            }
        }

        public bool TryConsumeControlledClosingRequest(
            out string reason)
        {
            if (!_controlledClosingRequested)
            {
                reason = string.Empty;
                return false;
            }

            _controlledClosingRequested = false;
            reason =
                "Checkout became unavailable while the store was open. " +
                "New admissions were blocked and a controlled close was started.";
            return true;
        }

        public StoreCustomerAdmissionDecision EvaluateOpening()
        {
            RefreshCheckoutStatus();
            return _policy.EvaluateOpening(
                BuildContext());
        }

        public StoreCustomerAdmissionDecision EvaluateAdmission()
        {
            RefreshCheckoutStatus();
            return _policy.EvaluateSpawn(
                BuildContext());
        }

        public StoreCustomerAdmissionDecision
            EvaluateClosingCompletion()
        {
            RefreshCheckoutStatus();
            return _policy.EvaluateClosingCompletion(
                BuildContext());
        }

        public StoreCustomerAdmissionDecision TryAdmit(
            CustomerInstanceId customerId)
        {
            StoreCustomerAdmissionDecision decision =
                EvaluateAdmission();

            if (!decision.Allowed)
            {
                return decision;
            }

            if (!_customers.TryRegister(
                    customerId,
                    ActiveCustomerState.Entering))
            {
                return StoreCustomerAdmissionDecision.Block(
                    StoreCustomerAdmissionFailureReason
                        .DuplicateCustomer,
                    "The customer is already registered as active.");
            }

            return StoreCustomerAdmissionDecision.Allow(
                "Customer admitted. " +
                ActiveCustomerCount + "/" +
                MaximumCustomerCount + " active.");
        }

        public bool TrySetState(
            CustomerInstanceId customerId,
            ActiveCustomerState state)
        {
            return _customers.TrySetState(
                customerId,
                state);
        }

        public void ClearCustomers()
        {
            _customers.Clear();
        }

        private StoreCustomerAdmissionContext BuildContext()
        {
            return new StoreCustomerAdmissionContext(
                CurrentStoreState(),
                _checkoutOperational,
                _admissionBlocked,
                ActiveCustomerCount,
                MaximumCustomerCount);
        }

        private void RefreshCheckoutStatus()
        {
            if (_service.State == null)
            {
                _checkoutOperational = false;
                _statusDetail =
                    "Store operations are not initialized.";
                return;
            }

            string stationState =
                CurrentCheckoutStationState();
            bool requiresOpenStation =
                IsState("Open") ||
                IsState("Closing");

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

                Transform checkoutRoot =
                    FindPlacedFixtureTransform(
                        fixture.InstanceId);

                if (checkoutRoot == null ||
                    !checkoutRoot.gameObject.activeInHierarchy)
                {
                    continue;
                }

                Transform customerPoint =
                    FindNamedChild(
                        checkoutRoot,
                        "CustomerCheckoutStandPoint")
                    ?? FindNamedChild(
                        checkoutRoot,
                        "CustomerStandPoint");

                if (customerPoint == null)
                {
                    continue;
                }

                if (!NavMesh.SamplePosition(
                        customerPoint.position,
                        out _,
                        2f,
                        NavMesh.AllAreas))
                {
                    continue;
                }

                if (requiresOpenStation &&
                    string.Equals(
                        stationState,
                        "Closed",
                        StringComparison.Ordinal))
                {
                    _checkoutOperational = false;
                    _statusDetail =
                        "The checkout station is closed while customers require service.";
                    return;
                }

                _checkoutOperational = true;
                _statusDetail =
                    "Checkout is placed, active and reachable.";
                return;
            }

            _checkoutOperational = false;
            _statusDetail =
                "Place an active checkout with a reachable customer stand point.";
        }

        private static Transform FindPlacedFixtureTransform(
            string fixtureInstanceId)
        {
            PlacementInstanceId targetId =
                new PlacementInstanceId(
                    fixtureInstanceId);

            PlacedObjectView[] placedViews =
                UnityEngine.Object.FindObjectsByType<
                    PlacedObjectView>(
                        FindObjectsInactive.Include,
                        FindObjectsSortMode.None);

            foreach (PlacedObjectView view in placedViews)
            {
                if (view != null &&
                    view.Id == targetId)
                {
                    return view.transform;
                }
            }

            PlacedFixtureVisual[] visuals =
                UnityEngine.Object.FindObjectsByType<
                    PlacedFixtureVisual>(
                        FindObjectsInactive.Include,
                        FindObjectsSortMode.None);

            foreach (PlacedFixtureVisual visual in visuals)
            {
                if (visual != null &&
                    string.Equals(
                        visual.InstanceId,
                        fixtureInstanceId,
                        StringComparison.Ordinal))
                {
                    return visual.transform;
                }
            }

            return null;
        }

        private static Transform FindNamedChild(
            Transform root,
            string childName)
        {
            if (root == null)
            {
                return null;
            }

            Transform[] children =
                root.GetComponentsInChildren<Transform>(
                    true);

            foreach (Transform child in children)
            {
                if (string.Equals(
                        child.name,
                        childName,
                        StringComparison.Ordinal))
                {
                    return child;
                }
            }

            return null;
        }

        private string CurrentStoreState()
        {
            if (!_activeSession.HasActiveSession)
            {
                return "BeforeOpen";
            }

            return _activeSession.Snapshot
                .DayCycle.State;
        }

        private string CurrentCheckoutStationState()
        {
            if (!_activeSession.HasActiveSession)
            {
                return "Closed";
            }

            return _activeSession.Snapshot
                .CheckoutStation.State;
        }

        private bool IsState(string state)
        {
            return string.Equals(
                CurrentStoreState(),
                state,
                StringComparison.Ordinal);
        }
    }
}
