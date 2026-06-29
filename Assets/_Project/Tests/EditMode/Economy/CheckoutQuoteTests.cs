using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.Economy;
using VRMGames.CartridgeAndCloud.Domain.Customers;
using VRMGames.CartridgeAndCloud.Domain.Economy;
using VRMGames.CartridgeAndCloud.Domain.Inventory;
using VRMGames.CartridgeAndCloud.Domain.Products;
using VRMGames.CartridgeAndCloud.Domain.Shopping;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Economy
{
    public sealed class CheckoutQuoteTests
    {
        [Test] public void Quote_RejectsEmptyCart()
        {
            ProductDefinition product =
                EconomyTestFactory.Product();
            CustomerInstanceId customerId =
                new CustomerInstanceId("customer");
            ShoppingCart cart =
                new ShoppingCart(
                    new ShoppingCartId("cart"),
                    customerId,
                    3);

            CheckoutQuoteResult result =
                new CheckoutQuoteService().TryCreate(
                    cart,
                    EconomyTestFactory.Prices(product));

            Assert.That(result.Succeeded, Is.False);
            Assert.That(
                result.FailureReason,
                Is.EqualTo(
                    CheckoutQuoteFailureReason.EmptyCart));
        }

        [Test] public void Quote_RejectsMissingPrice()
        {
            EconomyCheckoutScenario scenario =
                EconomyTestFactory.CheckoutScenario();
            ProductSalePriceCatalog empty =
                new ProductSalePriceCatalog(
                    EconomyTestFactory.Eur,
                    new ProductSalePrice[0]);

            CheckoutQuoteResult result =
                new CheckoutQuoteService().TryCreate(
                    scenario.Cart,
                    empty);

            Assert.That(
                result.FailureReason,
                Is.EqualTo(
                    CheckoutQuoteFailureReason
                        .MissingSalePrice));
            Assert.That(
                result.MissingProductId,
                Is.EqualTo(
                    scenario.Product.Id.Value));
        }

        [Test] public void Quote_CreatesSingleLine()
        {
            EconomyCheckoutScenario scenario =
                EconomyTestFactory.CheckoutScenario();

            CheckoutQuoteResult result =
                new CheckoutQuoteService().TryCreate(
                    scenario.Cart,
                    scenario.Prices);

            Assert.That(result.Succeeded, Is.True);
            Assert.That(
                result.Quote.LineCount,
                Is.EqualTo(1));
        }

        [Test] public void Quote_StoresCartId()
        {
            EconomyCheckoutScenario scenario =
                EconomyTestFactory.CheckoutScenario();

            CheckoutQuote quote =
                new CheckoutQuoteService()
                    .TryCreate(
                        scenario.Cart,
                        scenario.Prices)
                    .Quote;

            Assert.That(
                quote.CartId,
                Is.EqualTo(scenario.Cart.Id));
        }

        [Test] public void Quote_CountsUnits()
        {
            EconomyCheckoutScenario scenario =
                EconomyTestFactory.CheckoutScenario(
                    quantity: 3);

            CheckoutQuote quote =
                new CheckoutQuoteService()
                    .TryCreate(
                        scenario.Cart,
                        scenario.Prices)
                    .Quote;

            Assert.That(
                quote.UnitCount,
                Is.EqualTo(3));
        }

        [Test] public void Quote_MultipliesLineTotalExactly()
        {
            EconomyCheckoutScenario scenario =
                EconomyTestFactory.CheckoutScenario(
                    quantity: 3,
                    unitPriceCents: 1299);

            CheckoutQuote quote =
                new CheckoutQuoteService()
                    .TryCreate(
                        scenario.Cart,
                        scenario.Prices)
                    .Quote;

            Assert.That(
                quote.Lines[0].LineTotal.MinorUnits,
                Is.EqualTo(3897));
        }

        [Test] public void Quote_TotalMatchesLineTotal()
        {
            EconomyCheckoutScenario scenario =
                EconomyTestFactory.CheckoutScenario(
                    quantity: 2,
                    unitPriceCents: 2500);

            CheckoutQuote quote =
                new CheckoutQuoteService()
                    .TryCreate(
                        scenario.Cart,
                        scenario.Prices)
                    .Quote;

            Assert.That(
                quote.Total.MinorUnits,
                Is.EqualTo(5000));
        }

        [Test] public void Quote_LinePreservesReservationId()
        {
            EconomyCheckoutScenario scenario =
                EconomyTestFactory.CheckoutScenario();

            CheckoutQuoteLine line =
                new CheckoutQuoteService()
                    .TryCreate(
                        scenario.Cart,
                        scenario.Prices)
                    .Quote.Lines[0];

            Assert.That(
                line.ReservationId.Value,
                Is.EqualTo("reservation-a"));
        }

        [Test] public void Quote_RejectsNullCart()
        {
            ProductDefinition product =
                EconomyTestFactory.Product();

            Assert.Throws<System.ArgumentNullException>(
                () => new CheckoutQuoteService()
                    .TryCreate(
                        null,
                        EconomyTestFactory.Prices(
                            product)));
        }

        [Test] public void Quote_RejectsNullCatalog()
        {
            EconomyCheckoutScenario scenario =
                EconomyTestFactory.CheckoutScenario();

            Assert.Throws<System.ArgumentNullException>(
                () => new CheckoutQuoteService()
                    .TryCreate(
                        scenario.Cart,
                        null));
        }
    }
}
