using System;
using System.Collections.Generic;
using VRMGames.CartridgeAndCloud.Application.Checkout;
using VRMGames.CartridgeAndCloud.Domain.Checkout;
using VRMGames.CartridgeAndCloud.Domain.DayCycle;
using VRMGames.CartridgeAndCloud.Domain.Displays;
using VRMGames.CartridgeAndCloud.Domain.Economy;
using VRMGames.CartridgeAndCloud.Domain.Shopping;

namespace VRMGames.CartridgeAndCloud.Application.Economy
{
    public enum CheckoutQuoteFailureReason
    {
        None = 0,
        EmptyCart = 1,
        MissingSalePrice = 2
    }

    public sealed class CheckoutQuoteResult
    {
        public bool Succeeded { get; }

        public CheckoutQuoteFailureReason FailureReason { get; }

        public CheckoutQuote Quote { get; }

        public string MissingProductId { get; }

        private CheckoutQuoteResult(
            bool succeeded,
            CheckoutQuoteFailureReason failureReason,
            CheckoutQuote quote,
            string missingProductId)
        {
            Succeeded = succeeded;
            FailureReason = failureReason;
            Quote = quote;
            MissingProductId = missingProductId;
        }

        public static CheckoutQuoteResult Success(
            CheckoutQuote quote)
        {
            return new CheckoutQuoteResult(
                true,
                CheckoutQuoteFailureReason.None,
                quote,
                string.Empty);
        }

        public static CheckoutQuoteResult Failure(
            CheckoutQuoteFailureReason reason,
            string missingProductId = "")
        {
            return new CheckoutQuoteResult(
                false,
                reason,
                null,
                missingProductId ?? string.Empty);
        }
    }

    public sealed class CheckoutQuoteService
    {
        public CheckoutQuoteResult TryCreate(
            ShoppingCart cart,
            ProductSalePriceCatalog prices)
        {
            if (cart == null)
                throw new ArgumentNullException(nameof(cart));
            if (prices == null)
                throw new ArgumentNullException(nameof(prices));

            if (cart.IsEmpty)
            {
                return CheckoutQuoteResult.Failure(
                    CheckoutQuoteFailureReason.EmptyCart);
            }

            List<CheckoutQuoteLine> lines =
                new List<CheckoutQuoteLine>();

            foreach (ShoppingCartLine line in cart.Lines)
            {
                if (!prices.TryGet(
                        line.ProductId,
                        out ProductSalePrice price))
                {
                    return CheckoutQuoteResult.Failure(
                        CheckoutQuoteFailureReason
                            .MissingSalePrice,
                        line.ProductId.Value);
                }

                lines.Add(
                    new CheckoutQuoteLine(
                        line,
                        price.UnitPrice));
            }

            return CheckoutQuoteResult.Success(
                new CheckoutQuote(
                    cart.Id,
                    lines,
                    prices.Currency));
        }
    }

    public enum EconomicCheckoutFailureReason
    {
        None = 0,
        SessionAlreadyCheckedOut = 1,
        LedgerCurrencyMismatch = 2,
        PostingAlreadyExists = 3,
        QuoteFailed = 4,
        CheckoutFailed = 5,
        LedgerPostFailed = 6
    }

    public sealed class EconomicCheckoutResult
    {
        public bool Succeeded { get; }

        public EconomicCheckoutFailureReason FailureReason { get; }

        public CheckoutQuoteResult QuoteResult { get; }

        public CheckoutResult CheckoutResult { get; }

        public EconomyLedgerPostResult LedgerResult { get; }

        private EconomicCheckoutResult(
            bool succeeded,
            EconomicCheckoutFailureReason failureReason,
            CheckoutQuoteResult quoteResult,
            CheckoutResult checkoutResult,
            EconomyLedgerPostResult ledgerResult)
        {
            Succeeded = succeeded;
            FailureReason = failureReason;
            QuoteResult = quoteResult;
            CheckoutResult = checkoutResult;
            LedgerResult = ledgerResult;
        }

        public static EconomicCheckoutResult Success(
            CheckoutQuoteResult quoteResult,
            CheckoutResult checkoutResult,
            EconomyLedgerPostResult ledgerResult)
        {
            return new EconomicCheckoutResult(
                true,
                EconomicCheckoutFailureReason.None,
                quoteResult,
                checkoutResult,
                ledgerResult);
        }

        public static EconomicCheckoutResult Failure(
            EconomicCheckoutFailureReason reason,
            CheckoutQuoteResult quoteResult = null,
            CheckoutResult checkoutResult = null,
            EconomyLedgerPostResult ledgerResult = null)
        {
            return new EconomicCheckoutResult(
                false,
                reason,
                quoteResult,
                checkoutResult,
                ledgerResult);
        }
    }

    public sealed class EconomicCheckoutService
    {
        private readonly CheckoutQuoteService _quotes;
        private readonly CheckoutService _checkout;

        public EconomicCheckoutService(
            CheckoutQuoteService quotes,
            CheckoutService checkout)
        {
            _quotes = quotes ??
                throw new ArgumentNullException(nameof(quotes));
            _checkout = checkout ??
                throw new ArgumentNullException(nameof(checkout));
        }

        public EconomicCheckoutResult TryCheckoutAndRecord(
            CheckoutTransactionId transactionId,
            StoreDayId dayId,
            CheckoutQueue queue,
            CheckoutStation station,
            CustomerShoppingSession session,
            DisplayInstanceRegistry displays,
            ShoppingReservationRegistry reservations,
            CheckoutTransactionRegistry transactions,
            ProductSalePriceCatalog prices,
            EconomyLedger ledger)
        {
            if (queue == null)
                throw new ArgumentNullException(nameof(queue));
            if (station == null)
                throw new ArgumentNullException(nameof(station));
            if (session == null)
                throw new ArgumentNullException(nameof(session));
            if (displays == null)
                throw new ArgumentNullException(nameof(displays));
            if (reservations == null)
                throw new ArgumentNullException(nameof(reservations));
            if (transactions == null)
                throw new ArgumentNullException(nameof(transactions));
            if (prices == null)
                throw new ArgumentNullException(nameof(prices));
            if (ledger == null)
                throw new ArgumentNullException(nameof(ledger));

            if (session.State ==
                CustomerShoppingState.CheckedOut)
            {
                return EconomicCheckoutResult.Failure(
                    EconomicCheckoutFailureReason
                        .SessionAlreadyCheckedOut);
            }

            if (ledger.Currency != prices.Currency)
            {
                return EconomicCheckoutResult.Failure(
                    EconomicCheckoutFailureReason
                        .LedgerCurrencyMismatch);
            }

            EconomyPostingKey postingKey =
                new EconomyPostingKey(
                    EconomyPostingType.CheckoutRevenue,
                    transactionId.Value);

            if (ledger.ContainsPosting(postingKey))
            {
                return EconomicCheckoutResult.Failure(
                    EconomicCheckoutFailureReason
                        .PostingAlreadyExists);
            }

            CheckoutQuoteResult quoteResult =
                _quotes.TryCreate(
                    session.Cart,
                    prices);

            if (!quoteResult.Succeeded)
            {
                return EconomicCheckoutResult.Failure(
                    EconomicCheckoutFailureReason.QuoteFailed,
                    quoteResult);
            }

            CheckoutResult checkoutResult =
                _checkout.TryCheckout(
                    transactionId,
                    queue,
                    station,
                    session,
                    displays,
                    reservations,
                    transactions);

            if (!checkoutResult.Succeeded)
            {
                return EconomicCheckoutResult.Failure(
                    EconomicCheckoutFailureReason
                        .CheckoutFailed,
                    quoteResult,
                    checkoutResult);
            }

            EconomyLedgerEntry ledgerEntry =
                new EconomyLedgerEntry(
                    new EconomyLedgerEntryId(
                        "checkout-revenue-" +
                        transactionId.Value),
                    postingKey,
                    dayId,
                    quoteResult.Quote.Total);

            EconomyLedgerPostResult ledgerResult =
                ledger.TryPost(ledgerEntry);

            if (!ledgerResult.Succeeded)
            {
                return EconomicCheckoutResult.Failure(
                    EconomicCheckoutFailureReason
                        .LedgerPostFailed,
                    quoteResult,
                    checkoutResult,
                    ledgerResult);
            }

            return EconomicCheckoutResult.Success(
                quoteResult,
                checkoutResult,
                ledgerResult);
        }
    }
}
