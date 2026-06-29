using System;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Application.Checkout;
using VRMGames.CartridgeAndCloud.Application.DayCycle;
using VRMGames.CartridgeAndCloud.Application.Shopping;
using VRMGames.CartridgeAndCloud.Domain.Checkout;
using VRMGames.CartridgeAndCloud.Domain.Customers;
using VRMGames.CartridgeAndCloud.Domain.DayCycle;
using VRMGames.CartridgeAndCloud.Domain.Displays;
using VRMGames.CartridgeAndCloud.Domain.Inventory;
using VRMGames.CartridgeAndCloud.Domain.Products;
using VRMGames.CartridgeAndCloud.Domain.Shopping;

namespace VRMGames.CartridgeAndCloud.Infrastructure.DayCycle
{
    public sealed class StoreDayTechnicalScenarioRunner :
        MonoBehaviour
    {
        [SerializeField]
        private StoreDaySettingsAsset _settings;

        [SerializeField]
        private bool _runOnStart;

        public bool LastScenarioPassed { get; private set; }
        public int LastCustomersBefore { get; private set; }
        public int LastCustomersAfter { get; private set; }
        public int LastQueueBefore { get; private set; }
        public int LastQueueAfter { get; private set; }
        public int LastReservationsBefore { get; private set; }
        public int LastReservationsAfter { get; private set; }
        public int LastReleasedReservations { get; private set; }
        public int LastAbandonedSessions { get; private set; }
        public bool LastQueueSealed { get; private set; }
        public bool LastStationClosed { get; private set; }
        public bool LastDayClosed { get; private set; }
        public bool LastSpawnAdmissionBlocked { get; private set; }

        private void Start()
        {
            if (_runOnStart)
            {
                RunScenario();
            }
        }

        public void Configure(
            StoreDaySettingsAsset settings,
            bool runOnStart)
        {
            _settings = settings;
            _runOnStart = runOnStart;
        }

        [ContextMenu("Run Technical Day Closure Scenario")]
        public void RunScenario()
        {
            if (_settings == null)
            {
                throw new InvalidOperationException(
                    "Store day settings asset is required.");
            }

            StoreDay day = _settings.BuildDay();
            day.TryOpen();

            ProductCategoryId category =
                new ProductCategoryId("video-game");
            ProductDefinition product =
                new ProductDefinition(
                    new ProductDefinitionId(
                        "technical-day-product"),
                    "product.technical_day.name",
                    category,
                    Array.Empty<ProductTagId>());
            ProductDefinitionRegistry products =
                new ProductDefinitionRegistry(
                    new[] { product });

            DisplayDefinition displayDefinition =
                new DisplayDefinition(
                    new DisplayDefinitionId(
                        "technical-day-display"),
                    "display.technical_day.name",
                    new InventoryCapacity(6),
                    3,
                    new[] { category },
                    "technical-shelf-4x2");
            DisplayInstance display =
                new DisplayInstance(
                    new DisplayInstanceId(
                        "technical-day-display-001"),
                    displayDefinition);
            display.TryAssignProduct(products, product.Id);
            display.Inventory.TryAdd(
                product.Id,
                new Quantity(3));
            DisplayInstanceRegistry displays =
                new DisplayInstanceRegistry(
                    new[] { display });

            CustomerInstanceRegistry customers =
                new CustomerInstanceRegistry();
            CustomerInstance customerA =
                CreateCustomer(
                    "technical-day-customer-a",
                    startBrowsing: true);
            CustomerInstance customerB =
                CreateCustomer(
                    "technical-day-customer-b",
                    startBrowsing: false);
            customers.Add(customerA);
            customers.Add(customerB);

            ShoppingReservationRegistry reservations =
                new ShoppingReservationRegistry();
            CustomerShoppingSessionRegistry sessions =
                new CustomerShoppingSessionRegistry();

            CustomerShoppingSession session =
                CreateReadySession(
                    customerA.Id,
                    category,
                    display,
                    product,
                    reservations);
            sessions.TryRegister(session);

            CheckoutQueue queue = new CheckoutQueue(6);
            CheckoutQueueEntry entry =
                new CheckoutQueueEntry(
                    new CheckoutQueueEntryId(
                        "technical-day-entry"),
                    session.CustomerId,
                    session.Cart.Id);
            queue.TryEnqueue(entry);

            CheckoutStation station =
                new CheckoutStation(
                    new CheckoutStationId(
                        "technical-day-station"));
            station.TryOpen();

            ShoppingReservationService reservationService =
                new ShoppingReservationService(
                    products,
                    reservations,
                    new ShoppingPolicy(3, 1, true));
            ShoppingCartService cartService =
                new ShoppingCartService(
                    reservations,
                    reservationService);
            CheckoutCancellationService cancellation =
                new CheckoutCancellationService(
                    cartService,
                    reservations);
            StoreDayActivityTracker activity =
                new StoreDayActivityTracker();
            activity.RecordCustomerArrivals(2);

            StoreClosureCoordinator coordinator =
                new StoreClosureCoordinator(
                    new CustomerDrainService(),
                    cancellation,
                    cartService,
                    new StoreClosureSnapshotFactory(),
                    activity);
            StoreDayService dayService =
                new StoreDayService(day, coordinator);

            LastCustomersBefore = customers.ActiveCount;
            LastQueueBefore = queue.ActiveCount;
            LastReservationsBefore =
                reservations.ActiveCount;

            StoreDayServiceResult first =
                dayService.RequestClose(
                    customers,
                    queue,
                    station,
                    sessions,
                    reservations,
                    displays);

            customerA.ArriveAtCurrentTarget();
            customerB.ArriveAtCurrentTarget();

            StoreDayServiceResult second =
                dayService.ContinueClosing(
                    customers,
                    queue,
                    station,
                    sessions,
                    reservations,
                    displays);

            StoreClosureSnapshot finalSnapshot =
                new StoreClosureSnapshotFactory().Create(
                    customers,
                    queue,
                    station,
                    reservations,
                    sessions);
            StoreDayActivitySummary summary =
                activity.CreateSummary(
                    day,
                    finalSnapshot);

            CheckoutQueueResult blocked =
                queue.TryEnqueue(
                    new CheckoutQueueEntry(
                        new CheckoutQueueEntryId(
                            "blocked-entry"),
                        new CustomerInstanceId(
                            "blocked-customer"),
                        new ShoppingCartId(
                            "blocked-cart")));

            LastCustomersAfter = customers.ActiveCount;
            LastQueueAfter = queue.ActiveCount;
            LastReservationsAfter =
                reservations.ActiveCount;
            LastReleasedReservations =
                summary.ReleasedReservations;
            LastAbandonedSessions =
                summary.AbandonedShoppingSessions;
            LastQueueSealed =
                !queue.IsAcceptingEntries;
            LastStationClosed =
                station.State ==
                CheckoutStationState.Closed;
            LastDayClosed =
                day.State == StoreDayState.Closed;
            LastSpawnAdmissionBlocked =
                !blocked.Succeeded &&
                blocked.FailureReason ==
                    CheckoutQueueFailureReason.QueueSealed;

            LastScenarioPassed =
                first.Succeeded &&
                first.ClosureProgress != null &&
                !first.ClosureProgress.Completed &&
                second.Succeeded &&
                second.ClosureProgress != null &&
                second.ClosureProgress.Completed &&
                LastCustomersBefore == 2 &&
                LastCustomersAfter == 0 &&
                LastQueueBefore == 1 &&
                LastQueueAfter == 0 &&
                LastReservationsBefore == 1 &&
                LastReservationsAfter == 0 &&
                LastReleasedReservations == 1 &&
                LastAbandonedSessions == 1 &&
                LastQueueSealed &&
                LastStationClosed &&
                LastDayClosed &&
                LastSpawnAdmissionBlocked;

            if (LastScenarioPassed)
            {
                Debug.Log(
                    "Sprint 12 technical day closure scenario PASS.");
            }
            else
            {
                Debug.LogError(
                    "Sprint 12 technical day closure scenario FAILED.");
            }
        }

        private static CustomerInstance CreateCustomer(
            string id,
            bool startBrowsing)
        {
            CustomerNavigationPlan plan =
                new CustomerNavigationPlan(
                    new[]
                    {
                        new CustomerNavigationTarget(
                            new CustomerNavigationPointId(
                                id + "-entry"),
                            CustomerNavigationTargetType.Entry,
                            0),
                        new CustomerNavigationTarget(
                            new CustomerNavigationPointId(
                                id + "-browse"),
                            CustomerNavigationTargetType.Browse,
                            0),
                        new CustomerNavigationTarget(
                            new CustomerNavigationPointId(
                                id + "-exit"),
                            CustomerNavigationTargetType.Exit,
                            0)
                    });

            CustomerInstance customer =
                new CustomerInstance(
                    new CustomerInstanceId(id),
                    new CustomerProfileId(
                        "technical-day-profile"),
                    plan,
                    30);

            if (startBrowsing)
            {
                customer.BeginEntering();
                customer.ArriveAtCurrentTarget();
            }

            return customer;
        }

        private static CustomerShoppingSession
            CreateReadySession(
                CustomerInstanceId customerId,
                ProductCategoryId category,
                DisplayInstance display,
                ProductDefinition product,
                ShoppingReservationRegistry reservations)
        {
            ShoppingReservation reservation =
                new ShoppingReservation(
                    new ShoppingReservationId(
                        "technical-day-reservation"),
                    customerId,
                    display.Id,
                    product.Id,
                    new Quantity(1));
            reservations.TryRegister(reservation);

            ShoppingCart cart =
                new ShoppingCart(
                    new ShoppingCartId(
                        "technical-day-cart"),
                    customerId,
                    3);
            cart.TryAdd(reservation);

            ShoppingIntent intent =
                new ShoppingIntent(
                    new ShoppingIntentId(
                        "technical-day-intent"),
                    customerId,
                    new[] { category },
                    1);
            CustomerShoppingSession session =
                new CustomerShoppingSession(
                    customerId,
                    intent,
                    cart);
            session.TryMarkReadyForCheckout();
            return session;
        }
    }
}
