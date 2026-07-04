using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.Checkout;
using VRMGames.CartridgeAndCloud.Application.Economy;
using VRMGames.CartridgeAndCloud.Domain.Checkout;
using VRMGames.CartridgeAndCloud.Domain.DayCycle;
using VRMGames.CartridgeAndCloud.Domain.Economy;
using VRMGames.CartridgeAndCloud.Domain.Inventory;
using VRMGames.CartridgeAndCloud.Domain.Shopping;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Economy
{
    public sealed class EconomyPostingServiceTests
    {
        [Test]
        public void ReceiptCost_Succeeds()
        {
            var product = EconomyTestFactory.Product();
            EconomyLedger ledger =
                new EconomyLedger(EconomyTestFactory.Eur);

            SupplierReceivingEconomyResult result =
                new SupplierReceivingEconomyService()
                    .TryRecordReceivedCost(
                        new StoreDayId("day-a"),
                        "receipt-a",
                        EconomyTestFactory.SupplierEntry(
                            product),
                        new Quantity(3),
                        ledger);

            Assert.That(result.Succeeded, Is.True);
        }

        [Test]
        public void ReceiptCost_IsExact()
        {
            var product = EconomyTestFactory.Product();
            EconomyLedger ledger =
                new EconomyLedger(EconomyTestFactory.Eur);

            SupplierReceivingEconomyResult result =
                new SupplierReceivingEconomyService()
                    .TryRecordReceivedCost(
                        new StoreDayId("day-a"),
                        "receipt-a",
                        EconomyTestFactory.SupplierEntry(
                            product,
                            unitCostCents: 1299),
                        new Quantity(6),
                        ledger);

            Assert.That(
                result.Cost.MinorUnits,
                Is.EqualTo(7794));
        }

        [Test]
        public void ReceiptCost_RejectsZeroQuantity()
        {
            var product = EconomyTestFactory.Product();
            EconomyLedger ledger =
                new EconomyLedger(EconomyTestFactory.Eur);

            SupplierReceivingEconomyResult result =
                new SupplierReceivingEconomyService()
                    .TryRecordReceivedCost(
                        new StoreDayId("day-a"),
                        "receipt-a",
                        EconomyTestFactory.SupplierEntry(
                            product),
                        new Quantity(0),
                        ledger);

            Assert.That(
                result.FailureReason,
                Is.EqualTo(
                    SupplierReceivingEconomyFailureReason
                        .InvalidQuantity));
            Assert.That(ledger.Count, Is.EqualTo(0));
        }

        [Test]
        public void ReceiptCost_BlocksDuplicateReceipt()
        {
            var product = EconomyTestFactory.Product();
            EconomyLedger ledger =
                new EconomyLedger(EconomyTestFactory.Eur);
            SupplierReceivingEconomyService service =
                new SupplierReceivingEconomyService();

            service.TryRecordReceivedCost(
                new StoreDayId("day-a"),
                "receipt-a",
                EconomyTestFactory.SupplierEntry(product),
                new Quantity(3),
                ledger);

            SupplierReceivingEconomyResult duplicate =
                service.TryRecordReceivedCost(
                    new StoreDayId("day-a"),
                    "receipt-a",
                    EconomyTestFactory.SupplierEntry(product),
                    new Quantity(3),
                    ledger);

            Assert.That(
                duplicate.FailureReason,
                Is.EqualTo(
                    SupplierReceivingEconomyFailureReason
                        .PostingAlreadyExists));
        }

        [Test]
        public void ReceiptCost_PostsOneEntry()
        {
            var product = EconomyTestFactory.Product();
            EconomyLedger ledger =
                new EconomyLedger(EconomyTestFactory.Eur);

            new SupplierReceivingEconomyService()
                .TryRecordReceivedCost(
                    new StoreDayId("day-a"),
                    "receipt-a",
                    EconomyTestFactory.SupplierEntry(product),
                    new Quantity(3),
                    ledger);

            Assert.That(ledger.Count, Is.EqualTo(1));
        }

        [Test]
        public void ReceiptCost_UsesCostPostingType()
        {
            var product = EconomyTestFactory.Product();
            StoreDayId dayId = new StoreDayId("day-a");
            EconomyLedger ledger =
                new EconomyLedger(EconomyTestFactory.Eur);

            new SupplierReceivingEconomyService()
                .TryRecordReceivedCost(
                    dayId,
                    "receipt-a",
                    EconomyTestFactory.SupplierEntry(product),
                    new Quantity(3),
                    ledger);

            Assert.That(
                ledger.GetPostingCount(
                    dayId,
                    EconomyPostingType
                        .SupplierReceivingCost),
                Is.EqualTo(1));
        }

        [Test]
        public void DifferentReceipts_AreAllowed()
        {
            var product = EconomyTestFactory.Product();
            StoreDayId dayId = new StoreDayId("day-a");
            EconomyLedger ledger =
                new EconomyLedger(EconomyTestFactory.Eur);
            SupplierReceivingEconomyService service =
                new SupplierReceivingEconomyService();

            service.TryRecordReceivedCost(
                dayId,
                "receipt-a",
                EconomyTestFactory.SupplierEntry(product),
                new Quantity(3),
                ledger);
            service.TryRecordReceivedCost(
                dayId,
                "receipt-b",
                EconomyTestFactory.SupplierEntry(product),
                new Quantity(3),
                ledger);

            Assert.That(ledger.Count, Is.EqualTo(2));
        }

        [Test]
        public void ReceiptCost_IsAssignedToDay()
        {
            var product = EconomyTestFactory.Product();
            StoreDayId dayA = new StoreDayId("day-a");
            StoreDayId dayB = new StoreDayId("day-b");
            EconomyLedger ledger =
                new EconomyLedger(EconomyTestFactory.Eur);

            new SupplierReceivingEconomyService()
                .TryRecordReceivedCost(
                    dayA,
                    "receipt-a",
                    EconomyTestFactory.SupplierEntry(product),
                    new Quantity(3),
                    ledger);

            Assert.That(
                ledger.GetTotal(
                    dayB,
                    EconomyPostingType
                        .SupplierReceivingCost)
                    .IsZero,
                Is.True);
        }

        [Test]
        public void ReceiptCost_RejectsNullCatalogEntry()
        {
            Assert.Throws<System.ArgumentNullException>(
                () => new SupplierReceivingEconomyService()
                    .TryRecordReceivedCost(
                        new StoreDayId("day-a"),
                        "receipt-a",
                        null,
                        new Quantity(1),
                        new EconomyLedger(
                            EconomyTestFactory.Eur)));
        }

        [Test]
        public void ReceiptCost_RejectsNullLedger()
        {
            var product = EconomyTestFactory.Product();

            Assert.Throws<System.ArgumentNullException>(
                () => new SupplierReceivingEconomyService()
                    .TryRecordReceivedCost(
                        new StoreDayId("day-a"),
                        "receipt-a",
                        EconomyTestFactory.SupplierEntry(product),
                        new Quantity(1),
                        null));
        }

        [Test] public void Checkout_Succeeds()
        {
            EconomyCheckoutScenario scenario =
                EconomyTestFactory.CheckoutScenario();

            Assert.That(
                EconomyTestFactory.Checkout(scenario)
                    .Succeeded,
                Is.True);
        }

        [Test] public void Checkout_RecordsRevenue()
        {
            EconomyCheckoutScenario scenario =
                EconomyTestFactory.CheckoutScenario(
                    quantity: 2,
                    unitPriceCents: 2500);

            EconomyTestFactory.Checkout(scenario);

            Assert.That(
                scenario.Ledger.GetTotal(
                    scenario.DayId,
                    EconomyPostingType.CheckoutRevenue)
                    .MinorUnits,
                Is.EqualTo(5000));
        }

        [Test] public void Checkout_ReturnsQuote()
        {
            EconomyCheckoutScenario scenario =
                EconomyTestFactory.CheckoutScenario(
                    quantity: 2,
                    unitPriceCents: 1999);

            EconomicCheckoutResult result =
                EconomyTestFactory.Checkout(scenario);

            Assert.That(
                result.QuoteResult.Quote.Total.MinorUnits,
                Is.EqualTo(3998));
        }

        [Test] public void Checkout_ReducesPhysicalStock()
        {
            EconomyCheckoutScenario scenario =
                EconomyTestFactory.CheckoutScenario(
                    quantity: 2,
                    stock: 5);

            EconomyTestFactory.Checkout(scenario);

            Assert.That(
                scenario.Display.Inventory.GetQuantity(
                    scenario.Product.Id).Value,
                Is.EqualTo(3));
        }

        [Test] public void Checkout_ConsumesReservation()
        {
            EconomyCheckoutScenario scenario =
                EconomyTestFactory.CheckoutScenario();

            EconomyTestFactory.Checkout(scenario);

            Assert.That(
                scenario.Reservations.Reservations[0]
                    .State,
                Is.EqualTo(
                    ShoppingReservationState.Consumed));
        }

        [Test] public void Checkout_EmptiesCart()
        {
            EconomyCheckoutScenario scenario =
                EconomyTestFactory.CheckoutScenario();

            EconomyTestFactory.Checkout(scenario);

            Assert.That(scenario.Cart.IsEmpty, Is.True);
        }

        [Test] public void Checkout_MarksSessionCheckedOut()
        {
            EconomyCheckoutScenario scenario =
                EconomyTestFactory.CheckoutScenario();

            EconomyTestFactory.Checkout(scenario);

            Assert.That(
                scenario.Session.State,
                Is.EqualTo(
                    CustomerShoppingState.CheckedOut));
        }

        [Test] public void MissingPrice_DoesNotMutateStock()
        {
            EconomyCheckoutScenario scenario =
                EconomyTestFactory.CheckoutScenario();
            scenario.Prices =
                new ProductSalePriceCatalog(
                    EconomyTestFactory.Eur,
                    new ProductSalePrice[0]);

            int before =
                scenario.Display.Inventory.GetQuantity(
                    scenario.Product.Id).Value;

            EconomicCheckoutResult result =
                EconomyTestFactory.Checkout(scenario);

            Assert.That(
                result.FailureReason,
                Is.EqualTo(
                    EconomicCheckoutFailureReason
                        .QuoteFailed));
            Assert.That(
                scenario.Display.Inventory.GetQuantity(
                    scenario.Product.Id).Value,
                Is.EqualTo(before));
            Assert.That(
                scenario.Ledger.Count,
                Is.EqualTo(0));
        }

        [Test] public void CurrencyMismatch_DoesNotCheckout()
        {
            EconomyCheckoutScenario scenario =
                EconomyTestFactory.CheckoutScenario();
            scenario.Ledger =
                new EconomyLedger(
                    new CurrencyCode("USD"));

            EconomicCheckoutResult result =
                EconomyTestFactory.Checkout(scenario);

            Assert.That(
                result.FailureReason,
                Is.EqualTo(
                    EconomicCheckoutFailureReason
                        .LedgerCurrencyMismatch));
            Assert.That(
                scenario.Cart.IsEmpty,
                Is.False);
        }

        [Test] public void ExistingPosting_BlocksCheckout()
        {
            EconomyCheckoutScenario scenario =
                EconomyTestFactory.CheckoutScenario();

            scenario.Ledger.TryPost(
                new EconomyLedgerEntry(
                    new EconomyLedgerEntryId("existing"),
                    new EconomyPostingKey(
                        EconomyPostingType.CheckoutRevenue,
                        "transaction-a"),
                    scenario.DayId,
                    EconomyTestFactory.EurMoney(1)));

            EconomicCheckoutResult result =
                EconomyTestFactory.Checkout(scenario);

            Assert.That(
                result.FailureReason,
                Is.EqualTo(
                    EconomicCheckoutFailureReason
                        .PostingAlreadyExists));
            Assert.That(
                scenario.Cart.IsEmpty,
                Is.False);
        }

        [Test] public void CheckoutFailure_DoesNotPostRevenue()
        {
            EconomyCheckoutScenario scenario =
                EconomyTestFactory.CheckoutScenario();
            scenario.Station.TryCompleteProcessing(
                scenario.Entry.Id);

            EconomicCheckoutResult result =
                EconomyTestFactory.Checkout(scenario);

            Assert.That(
                result.FailureReason,
                Is.EqualTo(
                    EconomicCheckoutFailureReason
                        .CheckoutFailed));
            Assert.That(
                scenario.Ledger.Count,
                Is.EqualTo(0));
        }

        [Test] public void SecondCheckout_IsBlocked()
        {
            EconomyCheckoutScenario scenario =
                EconomyTestFactory.CheckoutScenario();

            EconomyTestFactory.Checkout(scenario);
            EconomicCheckoutResult second =
                EconomyTestFactory.Checkout(
                    scenario,
                    "transaction-b");

            Assert.That(second.Succeeded, Is.False);
            Assert.That(
                second.FailureReason,
                Is.EqualTo(
                    EconomicCheckoutFailureReason
                        .SessionAlreadyCheckedOut));
            Assert.That(
                scenario.Ledger.Count,
                Is.EqualTo(1));
        }

        [Test] public void RevenuePosting_UsesTransactionSource()
        {
            EconomyCheckoutScenario scenario =
                EconomyTestFactory.CheckoutScenario();

            EconomyTestFactory.Checkout(
                scenario,
                "transaction-custom");

            Assert.That(
                scenario.Ledger.ContainsPosting(
                    new EconomyPostingKey(
                        EconomyPostingType.CheckoutRevenue,
                        "transaction-custom")),
                Is.True);
        }

        [Test] public void TwoUnits_ProduceExactRevenue()
        {
            EconomyCheckoutScenario scenario =
                EconomyTestFactory.CheckoutScenario(
                    quantity: 2,
                    unitPriceCents: 2999);

            EconomicCheckoutResult result =
                EconomyTestFactory.Checkout(scenario);

            Assert.That(result.Succeeded, Is.True);
            Assert.That(
                result.LedgerResult.Entry.Amount
                    .MinorUnits,
                Is.EqualTo(5998));
        }
    }
}
