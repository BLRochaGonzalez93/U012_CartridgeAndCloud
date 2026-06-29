using System;
using UnityEngine;
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

namespace VRMGames.CartridgeAndCloud.Infrastructure.Economy
{
    public sealed class EconomyTechnicalScenarioRunner :
        MonoBehaviour
    {
        [SerializeField]
        private EconomySettingsAsset _settings;

        [SerializeField]
        private ProductSalePriceCatalogAsset _salePrices;

        [SerializeField]
        private bool _runOnStart;

        public bool LastScenarioPassed { get; private set; }

        public long LastCheckoutRevenueCents { get; private set; }

        public long LastSupplierCostCents { get; private set; }

        public long LastGrossResultCents { get; private set; }

        public int LastLedgerEntries { get; private set; }

        public int LastCheckoutPostings { get; private set; }

        public int LastSupplierReceiptPostings { get; private set; }

        public bool LastDuplicateCheckoutBlocked { get; private set; }

        public bool LastDuplicateReceiptBlocked { get; private set; }

        public bool LastDailyResultCreated { get; private set; }

        private void Start()
        {
            if (_runOnStart)
            {
                RunScenario();
            }
        }

        public void Configure(
            EconomySettingsAsset settings,
            ProductSalePriceCatalogAsset salePrices,
            bool runOnStart)
        {
            _settings = settings;
            _salePrices = salePrices;
            _runOnStart = runOnStart;
        }

        [ContextMenu("Run Technical Economy Scenario")]
        public void RunScenario()
        {
            if (_settings == null)
            {
                throw new InvalidOperationException(
                    "Economy settings asset is required.");
            }

            if (_salePrices == null)
            {
                throw new InvalidOperationException(
                    "Product sale-price catalog asset is required.");
            }

            CurrencyCode currency =
                _settings.BuildCurrency();
            EconomyLedger ledger =
                _settings.BuildLedger();
            ProductSalePriceCatalog prices =
                _salePrices.BuildCatalog(currency);

            if (prices.Count == 0)
            {
                throw new InvalidOperationException(
                    "At least one technical sale price is required.");
            }

            ProductDefinitionId preferredProductId =
                new ProductDefinitionId(
                    "cartridge-pixel-quest");
            ProductSalePrice selectedPrice =
                prices.TryGet(
                    preferredProductId,
                    out ProductSalePrice preferredPrice)
                    ? preferredPrice
                    : prices.Entries[0];
            ProductDefinitionId productId =
                selectedPrice.ProductId;

            ProductCategoryId category =
                new ProductCategoryId("video-game");
            ProductDefinition product =
                new ProductDefinition(
                    productId,
                    "product.technical_economy.name",
                    category,
                    Array.Empty<ProductTagId>());
            ProductDefinitionRegistry products =
                new ProductDefinitionRegistry(
                    new[] { product });

            DisplayDefinition displayDefinition =
                new DisplayDefinition(
                    new DisplayDefinitionId(
                        "technical-economy-display"),
                    "display.technical_economy.name",
                    new InventoryCapacity(8),
                    3,
                    new[] { category },
                    "technical-shelf-4x2");
            DisplayInstance display =
                new DisplayInstance(
                    new DisplayInstanceId(
                        "technical-economy-display-001"),
                    displayDefinition);
            display.TryAssignProduct(products, product.Id);
            display.Inventory.TryAdd(
                product.Id,
                new Quantity(5));
            DisplayInstanceRegistry displays =
                new DisplayInstanceRegistry(
                    new[] { display });

            CustomerInstanceId customerId =
                new CustomerInstanceId(
                    "technical-economy-customer");
            ShoppingReservation reservation =
                new ShoppingReservation(
                    new ShoppingReservationId(
                        "technical-economy-reservation"),
                    customerId,
                    display.Id,
                    product.Id,
                    new Quantity(2));
            ShoppingReservationRegistry reservations =
                new ShoppingReservationRegistry();
            reservations.TryRegister(reservation);

            ShoppingCart cart =
                new ShoppingCart(
                    new ShoppingCartId(
                        "technical-economy-cart"),
                    customerId,
                    3);
            cart.TryAdd(reservation);

            ShoppingIntent intent =
                new ShoppingIntent(
                    new ShoppingIntentId(
                        "technical-economy-intent"),
                    customerId,
                    new[] { category },
                    2);
            CustomerShoppingSession session =
                new CustomerShoppingSession(
                    customerId,
                    intent,
                    cart);
            session.TryMarkReadyForCheckout();

            CheckoutQueue queue =
                new CheckoutQueue(4);
            CheckoutQueueEntry entry =
                new CheckoutQueueEntry(
                    new CheckoutQueueEntryId(
                        "technical-economy-entry"),
                    customerId,
                    cart.Id);
            queue.TryEnqueue(entry);
            queue.TryCallNext();
            queue.TryBeginProcessing(entry.Id);

            CheckoutStation station =
                new CheckoutStation(
                    new CheckoutStationId(
                        "technical-economy-station"));
            station.TryOpen();
            station.TryBeginProcessing(entry.Id);

            CheckoutTransactionRegistry transactions =
                new CheckoutTransactionRegistry();
            EconomicCheckoutService checkout =
                new EconomicCheckoutService(
                    new CheckoutQuoteService(),
                    new CheckoutService());
            StoreDayId dayId =
                new StoreDayId("technical-economy-day");

            EconomicCheckoutResult checkoutResult =
                checkout.TryCheckoutAndRecord(
                    new CheckoutTransactionId(
                        "technical-economy-transaction"),
                    dayId,
                    queue,
                    station,
                    session,
                    displays,
                    reservations,
                    transactions,
                    prices,
                    ledger);

            EconomicCheckoutResult duplicateCheckout =
                checkout.TryCheckoutAndRecord(
                    new CheckoutTransactionId(
                        "technical-economy-transaction"),
                    dayId,
                    queue,
                    station,
                    session,
                    displays,
                    reservations,
                    transactions,
                    prices,
                    ledger);

            SupplierCatalogEntry supplierEntry =
                new SupplierCatalogEntry(
                    product.Id,
                    1200,
                    new Quantity(3),
                    1,
                    12);
            SupplierReceivingEconomyService supplierEconomy =
                new SupplierReceivingEconomyService();

            SupplierReceivingEconomyResult supplierResult =
                supplierEconomy.TryRecordReceivedCost(
                    dayId,
                    "technical-economy-receipt",
                    supplierEntry,
                    new Quantity(3),
                    ledger);

            SupplierReceivingEconomyResult duplicateReceipt =
                supplierEconomy.TryRecordReceivedCost(
                    dayId,
                    "technical-economy-receipt",
                    supplierEntry,
                    new Quantity(3),
                    ledger);

            StoreDayActivitySummary activity =
                new StoreDayActivitySummary(
                    dayId,
                    StoreDayState.Closed,
                    300,
                    1,
                    0,
                    1,
                    0,
                    0,
                    0,
                    0,
                    0,
                    0);

            DailyResultsCreationResult dailyResult =
                new DailyResultsService().TryCreate(
                    activity,
                    ledger);

            LastCheckoutRevenueCents =
                dailyResult.Succeeded
                    ? dailyResult.Result
                        .CheckoutRevenue.MinorUnits
                    : 0;
            LastSupplierCostCents =
                dailyResult.Succeeded
                    ? dailyResult.Result
                        .SupplierReceivingCost.MinorUnits
                    : 0;
            LastGrossResultCents =
                dailyResult.Succeeded
                    ? dailyResult.Result
                        .GrossResult.MinorUnits
                    : 0;
            LastLedgerEntries = ledger.Count;
            LastCheckoutPostings =
                ledger.GetPostingCount(
                    dayId,
                    EconomyPostingType.CheckoutRevenue);
            LastSupplierReceiptPostings =
                ledger.GetPostingCount(
                    dayId,
                    EconomyPostingType.SupplierReceivingCost);
            LastDuplicateCheckoutBlocked =
                !duplicateCheckout.Succeeded &&
                (duplicateCheckout.FailureReason ==
                    EconomicCheckoutFailureReason
                        .SessionAlreadyCheckedOut ||
                 duplicateCheckout.FailureReason ==
                    EconomicCheckoutFailureReason
                        .PostingAlreadyExists);
            LastDuplicateReceiptBlocked =
                !duplicateReceipt.Succeeded &&
                duplicateReceipt.FailureReason ==
                    SupplierReceivingEconomyFailureReason
                        .PostingAlreadyExists;
            LastDailyResultCreated =
                dailyResult.Succeeded;

            long expectedRevenue =
                checked(
                    selectedPrice.UnitPrice.MinorUnits * 2);
            long expectedCost = 3600;
            long expectedGross =
                expectedRevenue - expectedCost;

            LastScenarioPassed =
                checkoutResult.Succeeded &&
                supplierResult.Succeeded &&
                LastDailyResultCreated &&
                LastCheckoutRevenueCents ==
                    expectedRevenue &&
                LastSupplierCostCents ==
                    expectedCost &&
                LastGrossResultCents ==
                    expectedGross &&
                LastLedgerEntries == 2 &&
                LastCheckoutPostings == 1 &&
                LastSupplierReceiptPostings == 1 &&
                LastDuplicateCheckoutBlocked &&
                LastDuplicateReceiptBlocked;

            if (LastScenarioPassed)
            {
                Debug.Log(
                    "Sprint 13 technical economy scenario PASS.");
            }
            else
            {
                Debug.LogError(
                    "Sprint 13 technical economy scenario FAILED.");
            }
        }
    }
}
