using System;
using VRMGames.CartridgeAndCloud.Application.Checkout;
using VRMGames.CartridgeAndCloud.Application.Shopping;
using VRMGames.CartridgeAndCloud.Domain.Checkout;
using VRMGames.CartridgeAndCloud.Domain.Customers;
using VRMGames.CartridgeAndCloud.Domain.Displays;
using VRMGames.CartridgeAndCloud.Domain.Inventory;
using VRMGames.CartridgeAndCloud.Domain.Products;
using VRMGames.CartridgeAndCloud.Domain.Shopping;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Checkout
{
    internal sealed class CheckoutTestScenario
    {
        public ProductDefinition Product { get; set; }
        public ProductDefinitionRegistry Products { get; set; }
        public DisplayInstance Display { get; set; }
        public DisplayInstanceRegistry Displays { get; set; }
        public ShoppingReservationRegistry Reservations { get; set; }
        public ShoppingCart Cart { get; set; }
        public CustomerShoppingSession Session { get; set; }
        public CheckoutPolicy Policy { get; set; }
        public CheckoutQueueService QueueService { get; set; }
        public CheckoutQueue Queue { get; set; }
        public CheckoutStation Station { get; set; }
        public CheckoutQueueEntry Entry { get; set; }
        public CheckoutTransactionRegistry Transactions { get; set; }
    }

    internal static class CheckoutTestFactory
    {
        public static ProductCategoryId VideoGame =>
            new ProductCategoryId("video-game");

        public static ProductDefinition Product(
            string id = "product-a")
        {
            return new ProductDefinition(
                new ProductDefinitionId(id),
                "product." + id + ".name",
                VideoGame,
                Array.Empty<ProductTagId>());
        }

        public static ProductDefinitionRegistry Products(
            params ProductDefinition[] products)
        {
            return new ProductDefinitionRegistry(products);
        }

        public static DisplayInstance Display(
            ProductDefinitionRegistry products,
            ProductDefinition product,
            string id = "display-a",
            int stock = 3,
            int capacity = 6)
        {
            DisplayDefinition definition =
                new DisplayDefinition(
                    new DisplayDefinitionId(
                        "definition-" + id),
                    "display." + id + ".name",
                    new InventoryCapacity(capacity),
                    Math.Min(3, capacity),
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

        public static ShoppingReservation Reservation(
            int index,
            CustomerInstanceId customerId,
            DisplayInstance display,
            ProductDefinition product,
            int quantity = 1)
        {
            return new ShoppingReservation(
                new ShoppingReservationId(
                    "reservation-" + index),
                customerId,
                display.Id,
                product.Id,
                new Quantity(quantity));
        }

        public static CheckoutTestScenario ReadyScenario(
            int stock = 3,
            int reservationCount = 1,
            int quantityPerReservation = 1,
            bool processEntry = true,
            int queueCapacity = 6)
        {
            ProductDefinition product = Product();
            ProductDefinitionRegistry products =
                Products(product);
            DisplayInstance display =
                Display(
                    products,
                    product,
                    stock: stock,
                    capacity: Math.Max(6, stock));
            DisplayInstanceRegistry displays =
                new DisplayInstanceRegistry(
                    new[] { display });

            CustomerInstanceId customerId =
                new CustomerInstanceId("customer-a");
            ShoppingReservationRegistry reservations =
                new ShoppingReservationRegistry();
            ShoppingCart cart =
                new ShoppingCart(
                    new ShoppingCartId("cart-a"),
                    customerId,
                    Math.Max(
                        3,
                        reservationCount *
                        quantityPerReservation));

            for (int index = 0;
                 index < reservationCount;
                 index++)
            {
                ShoppingReservation reservation =
                    Reservation(
                        index,
                        customerId,
                        display,
                        product,
                        quantityPerReservation);
                reservations.TryRegister(reservation);
                cart.TryAdd(reservation);
            }

            ShoppingIntent intent =
                new ShoppingIntent(
                    new ShoppingIntentId("intent-a"),
                    customerId,
                    new[] { VideoGame },
                    reservationCount *
                    quantityPerReservation);
            CustomerShoppingSession session =
                new CustomerShoppingSession(
                    customerId,
                    intent,
                    cart);
            session.TryMarkReadyForCheckout();

            CheckoutPolicy policy =
                new CheckoutPolicy(queueCapacity);
            CheckoutQueueService queueService =
                new CheckoutQueueService(policy);
            CheckoutQueue queue =
                queueService.CreateQueue();
            CheckoutStation station =
                new CheckoutStation(
                    new CheckoutStationId("station-a"));
            station.TryOpen();

            CheckoutQueueServiceResult enqueue =
                queueService.TryEnqueue(
                    queue,
                    new CheckoutQueueEntryId("entry-a"),
                    session);
            CheckoutQueueEntry entry = enqueue.Entry;

            if (processEntry)
            {
                queueService.TryCallNext(queue);
                queueService.TryBeginProcessing(
                    queue,
                    station,
                    entry.Id);
            }

            return new CheckoutTestScenario
            {
                Product = product,
                Products = products,
                Display = display,
                Displays = displays,
                Reservations = reservations,
                Cart = cart,
                Session = session,
                Policy = policy,
                QueueService = queueService,
                Queue = queue,
                Station = station,
                Entry = entry,
                Transactions =
                    new CheckoutTransactionRegistry()
            };
        }

        public static ShoppingCartService CartService(
            CheckoutTestScenario scenario)
        {
            ShoppingReservationService reservationService =
                new ShoppingReservationService(
                    scenario.Products,
                    scenario.Reservations,
                    new ShoppingPolicy(8, 4, true));

            return new ShoppingCartService(
                scenario.Reservations,
                reservationService);
        }
    }
}
