using System;
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

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.DayCycle
{
    internal sealed class DayCycleTestScenario
    {
        public StoreDay Day { get; set; }
        public StoreDayActivityTracker Activity { get; set; }
        public CustomerInstanceRegistry Customers { get; set; }
        public ProductDefinition Product { get; set; }
        public ProductDefinitionRegistry Products { get; set; }
        public DisplayInstance Display { get; set; }
        public DisplayInstanceRegistry Displays { get; set; }
        public ShoppingReservationRegistry Reservations { get; set; }
        public CustomerShoppingSessionRegistry Sessions { get; set; }
        public CheckoutQueue Queue { get; set; }
        public CheckoutStation Station { get; set; }
        public ShoppingCartService CartService { get; set; }
        public CheckoutCancellationService Cancellation { get; set; }
        public StoreClosureCoordinator Coordinator { get; set; }
        public StoreDayService DayService { get; set; }
        public CheckoutTransactionRegistry Transactions { get; set; }
    }

    internal static class DayCycleTestFactory
    {
        public static ProductCategoryId Category =>
            new ProductCategoryId("video-game");

        public static StoreDay Day(
            int duration = 60,
            bool autoClose = true,
            bool open = true)
        {
            StoreDay day = new StoreDay(
                new StoreDayId("day-001"),
                new StoreDayPolicy(duration, autoClose));

            if (open)
            {
                day.TryOpen();
            }

            return day;
        }

        public static ProductDefinition Product(
            string id = "product-a")
        {
            return new ProductDefinition(
                new ProductDefinitionId(id),
                "product." + id + ".name",
                Category,
                Array.Empty<ProductTagId>());
        }

        public static DisplayInstance Display(
            ProductDefinitionRegistry products,
            ProductDefinition product,
            int stock = 8,
            string id = "display-a")
        {
            DisplayDefinition definition =
                new DisplayDefinition(
                    new DisplayDefinitionId(
                        "definition-" + id),
                    "display." + id + ".name",
                    new InventoryCapacity(
                        Math.Max(8, stock)),
                    3,
                    new[] { product.CategoryId },
                    "technical-shelf-4x2");
            DisplayInstance display =
                new DisplayInstance(
                    new DisplayInstanceId(id),
                    definition);
            display.TryAssignProduct(products, product.Id);

            if (stock > 0)
            {
                display.Inventory.TryAdd(
                    product.Id,
                    new Quantity(stock));
            }

            return display;
        }

        public static CustomerInstance Customer(
            string id = "customer-a",
            CustomerState state =
                CustomerState.Browsing)
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
                    new CustomerProfileId("profile-a"),
                    plan,
                    30);

            switch (state)
            {
                case CustomerState.WaitingToEnter:
                    break;

                case CustomerState.Entering:
                    customer.BeginEntering();
                    break;

                case CustomerState.Browsing:
                    customer.BeginEntering();
                    customer.ArriveAtCurrentTarget();
                    break;

                case CustomerState.Leaving:
                    customer.BeginEntering();
                    customer.BeginLeaving();
                    break;

                case CustomerState.Despawned:
                    customer.BeginEntering();
                    customer.BeginLeaving();
                    customer.ArriveAtCurrentTarget();
                    break;

                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(state));
            }

            return customer;
        }

        public static CustomerShoppingSession AddReadySession(
            DayCycleTestScenario scenario,
            CustomerInstanceId customerId,
            string suffix = "a",
            bool enqueue = true,
            bool processing = false)
        {
            ShoppingReservation reservation =
                new ShoppingReservation(
                    new ShoppingReservationId(
                        "reservation-" + suffix),
                    customerId,
                    scenario.Display.Id,
                    scenario.Product.Id,
                    new Quantity(1));
            scenario.Reservations.TryRegister(
                reservation);

            ShoppingCart cart =
                new ShoppingCart(
                    new ShoppingCartId(
                        "cart-" + suffix),
                    customerId,
                    3);
            cart.TryAdd(reservation);

            ShoppingIntent intent =
                new ShoppingIntent(
                    new ShoppingIntentId(
                        "intent-" + suffix),
                    customerId,
                    new[] { Category },
                    1);
            CustomerShoppingSession session =
                new CustomerShoppingSession(
                    customerId,
                    intent,
                    cart);
            session.TryMarkReadyForCheckout();
            scenario.Sessions.TryRegister(session);

            if (enqueue)
            {
                CheckoutQueueEntry entry =
                    new CheckoutQueueEntry(
                        new CheckoutQueueEntryId(
                            "entry-" + suffix),
                        customerId,
                        cart.Id);
                scenario.Queue.TryEnqueue(entry);

                if (processing)
                {
                    scenario.Queue.TryCallNext();
                    scenario.Station.TryBeginProcessing(
                        entry.Id);
                    scenario.Queue.TryBeginProcessing(
                        entry.Id);
                }
            }

            return session;
        }

        public static DayCycleTestScenario Scenario(
            int customerCount = 1,
            bool addReadySession = true,
            bool enqueue = true,
            bool processing = false,
            int stock = 8,
            int duration = 60,
            bool autoClose = true)
        {
            ProductDefinition product = Product();
            ProductDefinitionRegistry products =
                new ProductDefinitionRegistry(
                    new[] { product });
            DisplayInstance display =
                Display(products, product, stock);
            DisplayInstanceRegistry displays =
                new DisplayInstanceRegistry(
                    new[] { display });

            DayCycleTestScenario scenario =
                new DayCycleTestScenario
                {
                    Day = Day(duration, autoClose, true),
                    Activity =
                        new StoreDayActivityTracker(),
                    Customers =
                        new CustomerInstanceRegistry(),
                    Product = product,
                    Products = products,
                    Display = display,
                    Displays = displays,
                    Reservations =
                        new ShoppingReservationRegistry(),
                    Sessions =
                        new CustomerShoppingSessionRegistry(),
                    Queue = new CheckoutQueue(8),
                    Station = new CheckoutStation(
                        new CheckoutStationId(
                            "station-a")),
                    Transactions =
                        new CheckoutTransactionRegistry()
                };

            scenario.Station.TryOpen();

            for (int index = 0;
                 index < customerCount;
                 index++)
            {
                CustomerInstance customer =
                    Customer(
                        "customer-" + index,
                        index == 0
                            ? CustomerState.Browsing
                            : CustomerState.WaitingToEnter);
                scenario.Customers.Add(customer);
            }

            ShoppingReservationService
                reservationService =
                    new ShoppingReservationService(
                        products,
                        scenario.Reservations,
                        new ShoppingPolicy(8, 1, true));
            scenario.CartService =
                new ShoppingCartService(
                    scenario.Reservations,
                    reservationService);
            scenario.Cancellation =
                new CheckoutCancellationService(
                    scenario.CartService,
                    scenario.Reservations);
            scenario.Coordinator =
                new StoreClosureCoordinator(
                    new CustomerDrainService(),
                    scenario.Cancellation,
                    scenario.CartService,
                    new StoreClosureSnapshotFactory(),
                    scenario.Activity);
            scenario.DayService =
                new StoreDayService(
                    scenario.Day,
                    scenario.Coordinator);

            if (addReadySession)
            {
                if (customerCount == 0)
                {
                    throw new ArgumentException(
                        "A customer is required for a session.");
                }

                AddReadySession(
                    scenario,
                    scenario.Customers.Instances[0].Id,
                    "a",
                    enqueue,
                    processing);
            }

            scenario.Activity.RecordCustomerArrivals(
                customerCount);

            return scenario;
        }

        public static void DespawnAll(
            CustomerInstanceRegistry customers)
        {
            foreach (CustomerInstance customer
                     in customers.Instances)
            {
                if (customer.State ==
                    CustomerState.Leaving)
                {
                    customer.ArriveAtCurrentTarget();
                }
            }
        }

        public static CheckoutResult CompleteProcessingCheckout(
            DayCycleTestScenario scenario,
            string transactionId = "transaction-a")
        {
            CustomerShoppingSession session =
                scenario.Sessions.Sessions[0];

            return new CheckoutService().TryCheckout(
                new CheckoutTransactionId(transactionId),
                scenario.Queue,
                scenario.Station,
                session,
                scenario.Displays,
                scenario.Reservations,
                scenario.Transactions);
        }
    }
}
