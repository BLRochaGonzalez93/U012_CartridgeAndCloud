using System;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Application.Shopping;
using VRMGames.CartridgeAndCloud.Domain.Customers;
using VRMGames.CartridgeAndCloud.Domain.Displays;
using VRMGames.CartridgeAndCloud.Domain.Inventory;
using VRMGames.CartridgeAndCloud.Domain.Products;
using VRMGames.CartridgeAndCloud.Domain.Shopping;

namespace VRMGames.CartridgeAndCloud.Infrastructure.Shopping
{
    public sealed class ShoppingTechnicalScenarioRunner : MonoBehaviour
    {
        [SerializeField]
        private ShoppingSettingsAsset _settings;

        [SerializeField]
        private bool _runOnStart;

        public bool LastScenarioPassed { get; private set; }
        public int LastOnHand { get; private set; }
        public int LastReserved { get; private set; }
        public int LastAvailable { get; private set; }
        public int LastCartUnits { get; private set; }
        public int LastAvailableAfterRelease { get; private set; }

        private void Start()
        {
            if (_runOnStart) RunScenario();
        }

        public void Configure(ShoppingSettingsAsset settings, bool runOnStart)
        {
            _settings = settings;
            _runOnStart = runOnStart;
        }

        [ContextMenu("Run Technical Shopping Scenario")]
        public void RunScenario()
        {
            if (_settings == null)
                throw new InvalidOperationException(
                    "Shopping settings asset is required.");

            ShoppingPolicy policy = _settings.BuildPolicy();
            ProductCategoryId category = new ProductCategoryId("video-game");
            ProductDefinition product = new ProductDefinition(
                new ProductDefinitionId("technical-shopping-product"),
                "product.technical_shopping.name",
                category,
                Array.Empty<ProductTagId>());
            ProductDefinitionRegistry products =
                new ProductDefinitionRegistry(new[] { product });

            DisplayDefinition definition = new DisplayDefinition(
                new DisplayDefinitionId("technical-shopping-display"),
                "display.technical_shopping.name",
                new InventoryCapacity(6),
                3,
                new[] { category },
                "technical-shelf-4x2");
            DisplayInstance display = new DisplayInstance(
                new DisplayInstanceId("technical-shopping-display-001"),
                definition);
            display.TryAssignProduct(products, product.Id);
            display.Inventory.TryAdd(product.Id, new Quantity(3));

            DisplayInstanceRegistry displays =
                new DisplayInstanceRegistry(new[] { display });
            ShoppingReservationRegistry reservations =
                new ShoppingReservationRegistry();
            ShoppingReservationService reservationService =
                new ShoppingReservationService(products, reservations, policy);
            ShoppingCart cart = new ShoppingCart(
                new ShoppingCartId("technical-shopping-cart"),
                new CustomerInstanceId("technical-shopping-customer"),
                policy.MaxCartUnits);
            ShoppingCartService cartService =
                new ShoppingCartService(reservations, reservationService);

            ShoppingReservationResult reserve =
                reservationService.TryReserve(
                    new ShoppingReservationId("technical-shopping-reservation"),
                    cart.CustomerId,
                    display,
                    product.Id,
                    new Quantity(1));

            if (!reserve.Succeeded)
            {
                LastScenarioPassed = false;
                Debug.LogError("Technical shopping scenario could not reserve stock.");
                return;
            }

            ShoppingCartMutationResult cartResult =
                cartService.TryAddReservation(cart, reserve.Reservation.Id);
            ShoppingAvailabilityService availability =
                new ShoppingAvailabilityService(reservations);
            ShoppingAvailabilitySnapshot before = availability.GetAvailability(display);

            LastOnHand = before.OnHand.Value;
            LastReserved = before.Reserved.Value;
            LastAvailable = before.Available.Value;
            LastCartUnits = cart.TotalUnits;

            cartService.Abandon(cart, displays);
            LastAvailableAfterRelease =
                availability.GetAvailability(display).Available.Value;

            LastScenarioPassed =
                cartResult.Succeeded &&
                LastOnHand == 3 &&
                LastReserved == 1 &&
                LastAvailable == 2 &&
                LastCartUnits == 1 &&
                LastAvailableAfterRelease == 3 &&
                cart.IsEmpty;

            if (LastScenarioPassed)
                Debug.Log("Sprint 10 technical shopping scenario PASS.");
            else
                Debug.LogError("Sprint 10 technical shopping scenario FAILED.");
        }
    }
}
