using System;
using VRMGames.CartridgeAndCloud.Application.Checkout;
using VRMGames.CartridgeAndCloud.Application.Economy;
using VRMGames.CartridgeAndCloud.Domain.Checkout;
using VRMGames.CartridgeAndCloud.Domain.Customers;
using VRMGames.CartridgeAndCloud.Domain.DayCycle;
using VRMGames.CartridgeAndCloud.Domain.Displays;
using VRMGames.CartridgeAndCloud.Domain.Economy;
using VRMGames.CartridgeAndCloud.Domain.Inventory;
using VRMGames.CartridgeAndCloud.Domain.Products;
using VRMGames.CartridgeAndCloud.Domain.Shopping;
using VRMGames.CartridgeAndCloud.Domain.Suppliers;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Economy
{
    internal sealed class EconomyCheckoutScenario
    {
        public ProductDefinition Product { get; set; }
        public ProductDefinitionRegistry Products { get; set; }
        public DisplayInstance Display { get; set; }
        public DisplayInstanceRegistry Displays { get; set; }
        public ShoppingReservationRegistry Reservations { get; set; }
        public ShoppingCart Cart { get; set; }
        public CustomerShoppingSession Session { get; set; }
        public CheckoutQueue Queue { get; set; }
        public CheckoutQueueEntry Entry { get; set; }
        public CheckoutStation Station { get; set; }
        public CheckoutTransactionRegistry Transactions { get; set; }
        public ProductSalePriceCatalog Prices { get; set; }
        public EconomyLedger Ledger { get; set; }
        public StoreDayId DayId { get; set; }
    }

    internal static class EconomyTestFactory
    {
        public static CurrencyCode Eur =>
            new CurrencyCode("EUR");

        public static Money EurMoney(long cents)
        {
            return new Money(cents, Eur);
        }

        public static ProductDefinition Product(
            string id = "product-a")
        {
            return new ProductDefinition(
                new ProductDefinitionId(id),
                "product." + id + ".name",
                new ProductCategoryId("video-game"),
                Array.Empty<ProductTagId>());
        }

        public static ProductSalePriceCatalog Prices(
            ProductDefinition product,
            long unitPriceCents = 2500)
        {
            return new ProductSalePriceCatalog(
                Eur,
                new[]
                {
                    new ProductSalePrice(
                        product.Id,
                        EurMoney(unitPriceCents))
                });
        }

        public static SupplierCatalogEntry SupplierEntry(
            ProductDefinition product,
            int unitCostCents = 1200,
            int unitsPerBox = 3)
        {
            return new SupplierCatalogEntry(
                product.Id,
                unitCostCents,
                new Quantity(unitsPerBox),
                1,
                12);
        }

        public static EconomyCheckoutScenario CheckoutScenario(
            int quantity = 1,
            long unitPriceCents = 2500,
            int stock = 5)
        {
            ProductDefinition product = Product();
            ProductDefinitionRegistry products =
                new ProductDefinitionRegistry(
                    new[] { product });

            DisplayDefinition definition =
                new DisplayDefinition(
                    new DisplayDefinitionId("definition-a"),
                    "display.a.name",
                    new InventoryCapacity(8),
                    3,
                    new[] { product.CategoryId },
                    "technical-shelf-4x2");
            DisplayInstance display =
                new DisplayInstance(
                    new DisplayInstanceId("display-a"),
                    definition);
            display.TryAssignProduct(products, product.Id);
            display.Inventory.TryAdd(
                product.Id,
                new Quantity(stock));

            CustomerInstanceId customerId =
                new CustomerInstanceId("customer-a");
            ShoppingReservation reservation =
                new ShoppingReservation(
                    new ShoppingReservationId("reservation-a"),
                    customerId,
                    display.Id,
                    product.Id,
                    new Quantity(quantity));
            ShoppingReservationRegistry reservations =
                new ShoppingReservationRegistry();
            reservations.TryRegister(reservation);

            ShoppingCart cart =
                new ShoppingCart(
                    new ShoppingCartId("cart-a"),
                    customerId,
                    Math.Max(3, quantity));
            cart.TryAdd(reservation);

            ShoppingIntent intent =
                new ShoppingIntent(
                    new ShoppingIntentId("intent-a"),
                    customerId,
                    new[] { product.CategoryId },
                    quantity);
            CustomerShoppingSession session =
                new CustomerShoppingSession(
                    customerId,
                    intent,
                    cart);
            session.TryMarkReadyForCheckout();

            CheckoutQueue queue = new CheckoutQueue(4);
            CheckoutQueueEntry entry =
                new CheckoutQueueEntry(
                    new CheckoutQueueEntryId("entry-a"),
                    customerId,
                    cart.Id);
            queue.TryEnqueue(entry);
            queue.TryCallNext();
            queue.TryBeginProcessing(entry.Id);

            CheckoutStation station =
                new CheckoutStation(
                    new CheckoutStationId("station-a"));
            station.TryOpen();
            station.TryBeginProcessing(entry.Id);

            return new EconomyCheckoutScenario
            {
                Product = product,
                Products = products,
                Display = display,
                Displays =
                    new DisplayInstanceRegistry(
                        new[] { display }),
                Reservations = reservations,
                Cart = cart,
                Session = session,
                Queue = queue,
                Entry = entry,
                Station = station,
                Transactions =
                    new CheckoutTransactionRegistry(),
                Prices = Prices(
                    product,
                    unitPriceCents),
                Ledger = new EconomyLedger(Eur),
                DayId = new StoreDayId("day-a")
            };
        }

        public static EconomicCheckoutResult Checkout(
            EconomyCheckoutScenario scenario,
            string transactionId = "transaction-a")
        {
            return new EconomicCheckoutService(
                new CheckoutQuoteService(),
                new CheckoutService())
                .TryCheckoutAndRecord(
                    new CheckoutTransactionId(transactionId),
                    scenario.DayId,
                    scenario.Queue,
                    scenario.Station,
                    scenario.Session,
                    scenario.Displays,
                    scenario.Reservations,
                    scenario.Transactions,
                    scenario.Prices,
                    scenario.Ledger);
        }

        public static StoreDayActivitySummary ClosedActivity(
            StoreDayId dayId,
            int completedCheckouts = 1)
        {
            return new StoreDayActivitySummary(
                dayId,
                StoreDayState.Closed,
                300,
                2,
                0,
                completedCheckouts,
                0,
                0,
                0,
                0,
                0,
                0);
        }
    }
}
