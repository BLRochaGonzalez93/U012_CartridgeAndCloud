using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VRMGames.CartridgeAndCloud.Domain.Inventory;
using VRMGames.CartridgeAndCloud.Domain.Products;
using VRMGames.CartridgeAndCloud.Domain.Shopping;

namespace VRMGames.CartridgeAndCloud.Domain.Economy
{
    public sealed class ProductSalePrice
    {
        public ProductDefinitionId ProductId { get; }

        public Money UnitPrice { get; }

        public ProductSalePrice(
            ProductDefinitionId productId,
            Money unitPrice)
        {
            if (string.IsNullOrWhiteSpace(productId.Value))
            {
                throw new ArgumentException(
                    "Product ID must be initialized.",
                    nameof(productId));
            }

            if (!unitPrice.IsPositive)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(unitPrice),
                    "Sale price must be greater than zero.");
            }

            ProductId = productId;
            UnitPrice = unitPrice;
        }
    }

    public sealed class ProductSalePriceCatalog
    {
        private readonly Dictionary<
            ProductDefinitionId,
            ProductSalePrice> _byProduct;

        private readonly ReadOnlyCollection<
            ProductSalePrice> _entries;

        public CurrencyCode Currency { get; }

        public int Count => _entries.Count;

        public IReadOnlyList<ProductSalePrice> Entries =>
            _entries;

        public ProductSalePriceCatalog(
            CurrencyCode currency,
            IEnumerable<ProductSalePrice> entries)
        {
            if (string.IsNullOrWhiteSpace(currency.Value))
            {
                throw new ArgumentException(
                    "Currency must be initialized.",
                    nameof(currency));
            }

            if (entries == null)
            {
                throw new ArgumentNullException(nameof(entries));
            }

            Currency = currency;
            _byProduct =
                new Dictionary<
                    ProductDefinitionId,
                    ProductSalePrice>();

            List<ProductSalePrice> ordered =
                new List<ProductSalePrice>();

            foreach (ProductSalePrice entry in entries)
            {
                if (entry == null)
                {
                    throw new ArgumentException(
                        "Sale-price entries cannot contain null.",
                        nameof(entries));
                }

                if (entry.UnitPrice.Currency != currency)
                {
                    throw new ArgumentException(
                        "All sale prices must use the catalog currency.",
                        nameof(entries));
                }

                if (_byProduct.ContainsKey(entry.ProductId))
                {
                    throw new ArgumentException(
                        $"Product {entry.ProductId} is duplicated.",
                        nameof(entries));
                }

                _byProduct.Add(entry.ProductId, entry);
                ordered.Add(entry);
            }

            ordered.Sort(
                (left, right) =>
                    StringComparer.Ordinal.Compare(
                        left.ProductId.Value,
                        right.ProductId.Value));

            _entries =
                new ReadOnlyCollection<ProductSalePrice>(
                    ordered);
        }

        public bool Contains(ProductDefinitionId productId)
        {
            return _byProduct.ContainsKey(productId);
        }

        public bool TryGet(
            ProductDefinitionId productId,
            out ProductSalePrice price)
        {
            return _byProduct.TryGetValue(
                productId,
                out price);
        }

        public ProductSalePrice Get(
            ProductDefinitionId productId)
        {
            if (!_byProduct.TryGetValue(
                    productId,
                    out ProductSalePrice price))
            {
                throw new KeyNotFoundException(
                    $"Sale price for {productId} was not found.");
            }

            return price;
        }
    }

    public sealed class CheckoutQuoteLine
    {
        public ShoppingReservationId ReservationId { get; }

        public ProductDefinitionId ProductId { get; }

        public Quantity Quantity { get; }

        public Money UnitPrice { get; }

        public Money LineTotal { get; }

        public CheckoutQuoteLine(
            ShoppingCartLine cartLine,
            Money unitPrice)
        {
            if (cartLine == null)
            {
                throw new ArgumentNullException(nameof(cartLine));
            }

            if (!unitPrice.IsPositive)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(unitPrice));
            }

            ReservationId = cartLine.ReservationId;
            ProductId = cartLine.ProductId;
            Quantity = cartLine.Quantity;
            UnitPrice = unitPrice;
            LineTotal = unitPrice.Multiply(
                cartLine.Quantity.Value);
        }
    }

    public sealed class CheckoutQuote
    {
        private readonly ReadOnlyCollection<
            CheckoutQuoteLine> _lines;

        public ShoppingCartId CartId { get; }

        public IReadOnlyList<CheckoutQuoteLine> Lines =>
            _lines;

        public int LineCount => _lines.Count;

        public int UnitCount { get; }

        public Money Total { get; }

        public CheckoutQuote(
            ShoppingCartId cartId,
            IEnumerable<CheckoutQuoteLine> lines,
            CurrencyCode currency)
        {
            if (string.IsNullOrWhiteSpace(cartId.Value))
            {
                throw new ArgumentException(
                    "Cart ID must be initialized.",
                    nameof(cartId));
            }

            if (lines == null)
            {
                throw new ArgumentNullException(nameof(lines));
            }

            List<CheckoutQuoteLine> ordered =
                new List<CheckoutQuoteLine>();
            int units = 0;
            Money total = Money.Zero(currency);

            foreach (CheckoutQuoteLine line in lines)
            {
                if (line == null)
                {
                    throw new ArgumentException(
                        "Quote lines cannot contain null.",
                        nameof(lines));
                }

                if (line.UnitPrice.Currency != currency)
                {
                    throw new ArgumentException(
                        "Quote-line currency mismatch.",
                        nameof(lines));
                }

                ordered.Add(line);
                units = checked(
                    units + line.Quantity.Value);
                total = total.Add(line.LineTotal);
            }

            if (ordered.Count == 0)
            {
                throw new ArgumentException(
                    "Checkout quote must contain at least one line.",
                    nameof(lines));
            }

            ordered.Sort(
                (left, right) =>
                    StringComparer.Ordinal.Compare(
                        left.ReservationId.Value,
                        right.ReservationId.Value));

            CartId = cartId;
            UnitCount = units;
            Total = total;
            _lines =
                new ReadOnlyCollection<CheckoutQuoteLine>(
                    ordered);
        }
    }
}
