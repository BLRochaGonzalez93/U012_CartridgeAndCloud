using System;
using VRMGames.CartridgeAndCloud.Domain.Customers;
using VRMGames.CartridgeAndCloud.Domain.Displays;
using VRMGames.CartridgeAndCloud.Domain.Inventory;
using VRMGames.CartridgeAndCloud.Domain.Products;
using VRMGames.CartridgeAndCloud.Domain.Shopping;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Shopping
{
    internal static class ShoppingTestFactory
    {
        public static ProductCategoryId VideoGame =>
            new ProductCategoryId("video-game");

        public static ProductCategoryId Accessory =>
            new ProductCategoryId("accessory");

        public static ProductDefinition Product(
            string id = "product-a")
        {
            return ProductWithCategory(id, VideoGame);
        }

        public static ProductDefinition ProductWithCategory(
            string id,
            ProductCategoryId category)
        {
            return new ProductDefinition(
                new ProductDefinitionId(id),
                "product." + id + ".name",
                category,
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
            DisplayDefinition definition = new DisplayDefinition(
                new DisplayDefinitionId("definition-" + id),
                "display." + id + ".name",
                new InventoryCapacity(capacity),
                Math.Min(3, capacity),
                new[] { product.CategoryId },
                "technical-shelf-4x2");
            DisplayInstance display = new DisplayInstance(
                new DisplayInstanceId(id),
                definition);
            display.TryAssignProduct(products, product.Id);
            if (stock > 0)
                display.Inventory.TryAdd(product.Id, new Quantity(stock));
            return display;
        }

        public static CustomerProfile Profile(
            string id = "profile-a")
        {
            return new CustomerProfile(
                new CustomerProfileId(id),
                "customer." + id + ".name",
                new[] { VideoGame },
                1,
                30,
                1,
                2f);
        }

        public static ShoppingPolicy Policy(
            int cart = 3,
            int reservation = 1,
            bool fallback = true)
        {
            return new ShoppingPolicy(cart, reservation, fallback);
        }

        public static ShoppingIntent Intent(
            CustomerInstanceId customerId,
            int desired = 1)
        {
            return new ShoppingIntent(
                new ShoppingIntentId("intent-a"),
                customerId,
                new[] { VideoGame },
                desired);
        }

        public static ShoppingIntent IntentWithCategories(
            CustomerInstanceId customerId,
            int desired,
            params ProductCategoryId[] categories)
        {
            return new ShoppingIntent(
                new ShoppingIntentId("intent-a"),
                customerId,
                categories,
                desired);
        }

        public static ShoppingReservation Reservation(
            string id = "reservation-a",
            string customer = "customer-a",
            string display = "display-a",
            string product = "product-a",
            int quantity = 1)
        {
            return new ShoppingReservation(
                new ShoppingReservationId(id),
                new CustomerInstanceId(customer),
                new DisplayInstanceId(display),
                new ProductDefinitionId(product),
                new Quantity(quantity));
        }
    }
}
