using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.Checkout;
using VRMGames.CartridgeAndCloud.Application.Economy;
using VRMGames.CartridgeAndCloud.Domain.Checkout;
using VRMGames.CartridgeAndCloud.Domain.Economy;
using VRMGames.CartridgeAndCloud.Domain.Shopping;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Economy
{
    public sealed class EconomicCheckoutServiceTests
    {
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
