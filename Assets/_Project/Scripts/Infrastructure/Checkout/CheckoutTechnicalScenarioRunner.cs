using System;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Application.Checkout;
using VRMGames.CartridgeAndCloud.Domain.Checkout;
using VRMGames.CartridgeAndCloud.Domain.Customers;
using VRMGames.CartridgeAndCloud.Domain.Displays;
using VRMGames.CartridgeAndCloud.Domain.Inventory;
using VRMGames.CartridgeAndCloud.Domain.Products;
using VRMGames.CartridgeAndCloud.Domain.Shopping;

namespace VRMGames.CartridgeAndCloud.Infrastructure.Checkout
{
    public sealed class CheckoutTechnicalScenarioRunner :
        MonoBehaviour
    {
        [SerializeField]
        private CheckoutSettingsAsset _settings;

        [SerializeField]
        private bool _runOnStart;

        public bool LastScenarioPassed { get; private set; }

        public int LastQueueBefore { get; private set; }

        public int LastQueueAfter { get; private set; }

        public int LastStockBefore { get; private set; }

        public int LastStockAfter { get; private set; }

        public int LastCartUnitsAfter { get; private set; }

        public int LastConsumedReservations { get; private set; }

        public bool LastSessionCheckedOut { get; private set; }

        public bool LastTransactionCompleted { get; private set; }

        public bool LastStationAvailable { get; private set; }

        public bool LastDoubleCheckoutBlocked { get; private set; }

        private void Start()
        {
            if (_runOnStart)
            {
                RunScenario();
            }
        }

        public void Configure(
            CheckoutSettingsAsset settings,
            bool runOnStart)
        {
            _settings = settings;
            _runOnStart = runOnStart;
        }

        [ContextMenu("Run Technical Checkout Scenario")]
        public void RunScenario()
        {
            if (_settings == null)
            {
                throw new InvalidOperationException(
                    "Checkout settings asset is required.");
            }

            ProductCategoryId category =
                new ProductCategoryId("video-game");
            ProductDefinition product =
                new ProductDefinition(
                    new ProductDefinitionId(
                        "technical-checkout-product"),
                    "product.technical_checkout.name",
                    category,
                    Array.Empty<ProductTagId>());
            ProductDefinitionRegistry products =
                new ProductDefinitionRegistry(
                    new[] { product });

            DisplayDefinition definition =
                new DisplayDefinition(
                    new DisplayDefinitionId(
                        "technical-checkout-display"),
                    "display.technical_checkout.name",
                    new InventoryCapacity(6),
                    3,
                    new[] { category },
                    "technical-shelf-4x2");
            DisplayInstance display =
                new DisplayInstance(
                    new DisplayInstanceId(
                        "technical-checkout-display-001"),
                    definition);
            display.TryAssignProduct(products, product.Id);
            display.Inventory.TryAdd(
                product.Id,
                new Quantity(3));
            DisplayInstanceRegistry displays =
                new DisplayInstanceRegistry(
                    new[] { display });

            CustomerInstanceId customerId =
                new CustomerInstanceId(
                    "technical-checkout-customer");
            ShoppingReservationRegistry reservations =
                new ShoppingReservationRegistry();
            ShoppingCart cart =
                new ShoppingCart(
                    new ShoppingCartId(
                        "technical-checkout-cart"),
                    customerId,
                    3);

            for (int index = 0; index < 2; index++)
            {
                ShoppingReservation reservation =
                    new ShoppingReservation(
                        new ShoppingReservationId(
                            "technical-checkout-reservation-" +
                            index),
                        customerId,
                        display.Id,
                        product.Id,
                        new Quantity(1));
                reservations.TryRegister(reservation);
                cart.TryAdd(reservation);
            }

            ShoppingIntent intent =
                new ShoppingIntent(
                    new ShoppingIntentId(
                        "technical-checkout-intent"),
                    customerId,
                    new[] { category },
                    2);
            CustomerShoppingSession session =
                new CustomerShoppingSession(
                    customerId,
                    intent,
                    cart);
            session.TryMarkReadyForCheckout();

            CheckoutQueueService queueService =
                new CheckoutQueueService(
                    _settings.BuildPolicy());
            CheckoutQueue queue =
                queueService.CreateQueue();
            CheckoutStation station =
                _settings.BuildStation();

            CheckoutQueueServiceResult enqueue =
                queueService.TryEnqueue(
                    queue,
                    new CheckoutQueueEntryId(
                        "technical-checkout-entry"),
                    session);
            CheckoutQueueServiceResult called =
                queueService.TryCallNext(queue);
            CheckoutQueueServiceResult processing =
                queueService.TryBeginProcessing(
                    queue,
                    station,
                    enqueue.Entry.Id);

            LastQueueBefore = queue.ActiveCount;
            LastStockBefore =
                display.Inventory.GetQuantity(
                    product.Id).Value;

            CheckoutTransactionRegistry transactions =
                new CheckoutTransactionRegistry();
            CheckoutService checkout =
                new CheckoutService();

            CheckoutResult result =
                checkout.TryCheckout(
                    new CheckoutTransactionId(
                        "technical-checkout-transaction"),
                    queue,
                    station,
                    session,
                    displays,
                    reservations,
                    transactions);

            CheckoutResult second =
                checkout.TryCheckout(
                    new CheckoutTransactionId(
                        "technical-checkout-transaction-2"),
                    queue,
                    station,
                    session,
                    displays,
                    reservations,
                    transactions);

            LastQueueAfter = queue.ActiveCount;
            LastStockAfter =
                display.Inventory.GetQuantity(
                    product.Id).Value;
            LastCartUnitsAfter = cart.TotalUnits;

            int consumed = 0;
            foreach (ShoppingReservation reservation
                     in reservations.GetForCustomer(
                         customerId,
                         activeOnly: false))
            {
                if (reservation.State ==
                    ShoppingReservationState.Consumed)
                {
                    consumed++;
                }
            }

            LastConsumedReservations = consumed;
            LastSessionCheckedOut =
                session.State ==
                CustomerShoppingState.CheckedOut;
            LastTransactionCompleted =
                result.Transaction != null &&
                result.Transaction.State ==
                    CheckoutTransactionState.Completed;
            LastStationAvailable =
                station.State ==
                CheckoutStationState.Available;
            LastDoubleCheckoutBlocked =
                !second.Succeeded &&
                second.FailureReason ==
                    CheckoutFailureReason
                        .CartAlreadyCheckedOut;

            LastScenarioPassed =
                enqueue.Succeeded &&
                called.Succeeded &&
                processing.Succeeded &&
                result.Succeeded &&
                LastQueueBefore == 1 &&
                LastQueueAfter == 0 &&
                LastStockBefore == 3 &&
                LastStockAfter == 1 &&
                LastCartUnitsAfter == 0 &&
                LastConsumedReservations == 2 &&
                LastSessionCheckedOut &&
                LastTransactionCompleted &&
                LastStationAvailable &&
                LastDoubleCheckoutBlocked;

            if (LastScenarioPassed)
            {
                Debug.Log(
                    "Sprint 11 technical checkout scenario PASS.");
            }
            else
            {
                Debug.LogError(
                    "Sprint 11 technical checkout scenario FAILED.");
            }
        }
    }
}
