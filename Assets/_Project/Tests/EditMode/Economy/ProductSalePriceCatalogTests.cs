using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.Economy;
using VRMGames.CartridgeAndCloud.Domain.Products;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Economy
{
    public sealed class ProductSalePriceCatalogTests
    {
        [Test] public void SalePrice_RejectsUninitializedProduct()
        {
            Assert.Throws<System.ArgumentException>(
                () => new ProductSalePrice(
                    default(ProductDefinitionId),
                    EconomyTestFactory.EurMoney(100)));
        }

        [Test] public void SalePrice_RejectsZero()
        {
            Assert.Throws<
                System.ArgumentOutOfRangeException>(
                () => new ProductSalePrice(
                    new ProductDefinitionId("product"),
                    EconomyTestFactory.EurMoney(0)));
        }

        [Test] public void SalePrice_RejectsNegative()
        {
            Assert.Throws<
                System.ArgumentOutOfRangeException>(
                () => new ProductSalePrice(
                    new ProductDefinitionId("product"),
                    EconomyTestFactory.EurMoney(-1)));
        }

        [Test] public void SalePrice_StoresProductAndPrice()
        {
            ProductDefinition product =
                EconomyTestFactory.Product();
            ProductSalePrice price =
                new ProductSalePrice(
                    product.Id,
                    EconomyTestFactory.EurMoney(2500));
            Assert.That(
                price.ProductId,
                Is.EqualTo(product.Id));
            Assert.That(
                price.UnitPrice.MinorUnits,
                Is.EqualTo(2500));
        }

        [Test] public void Catalog_StoresCurrency()
        {
            ProductDefinition product =
                EconomyTestFactory.Product();
            ProductSalePriceCatalog catalog =
                EconomyTestFactory.Prices(product);
            Assert.That(
                catalog.Currency,
                Is.EqualTo(EconomyTestFactory.Eur));
        }

        [Test] public void Catalog_RejectsNullEntries()
        {
            Assert.Throws<System.ArgumentNullException>(
                () => new ProductSalePriceCatalog(
                    EconomyTestFactory.Eur,
                    null));
        }

        [Test] public void Catalog_RejectsCurrencyMismatch()
        {
            ProductDefinition product =
                EconomyTestFactory.Product();
            ProductSalePrice price =
                new ProductSalePrice(
                    product.Id,
                    new Money(
                        2500,
                        new CurrencyCode("USD")));

            Assert.Throws<System.ArgumentException>(
                () => new ProductSalePriceCatalog(
                    EconomyTestFactory.Eur,
                    new[] { price }));
        }

        [Test] public void Catalog_RejectsDuplicateProduct()
        {
            ProductDefinition product =
                EconomyTestFactory.Product();
            ProductSalePrice price =
                new ProductSalePrice(
                    product.Id,
                    EconomyTestFactory.EurMoney(2500));

            Assert.Throws<System.ArgumentException>(
                () => new ProductSalePriceCatalog(
                    EconomyTestFactory.Eur,
                    new[] { price, price }));
        }

        [Test] public void Catalog_ContainsProduct()
        {
            ProductDefinition product =
                EconomyTestFactory.Product();
            Assert.That(
                EconomyTestFactory.Prices(product)
                    .Contains(product.Id),
                Is.True);
        }

        [Test] public void Catalog_TryGetReturnsPrice()
        {
            ProductDefinition product =
                EconomyTestFactory.Product();
            ProductSalePriceCatalog catalog =
                EconomyTestFactory.Prices(
                    product,
                    3999);
            Assert.That(
                catalog.TryGet(
                    product.Id,
                    out ProductSalePrice price),
                Is.True);
            Assert.That(
                price.UnitPrice.MinorUnits,
                Is.EqualTo(3999));
        }

        [Test] public void Catalog_GetThrowsForMissing()
        {
            ProductDefinition product =
                EconomyTestFactory.Product();

            Assert.Throws<
                System.Collections.Generic
                    .KeyNotFoundException>(
                () => EconomyTestFactory.Prices(product)
                    .Get(
                        new ProductDefinitionId(
                            "missing")));
        }

        [Test] public void Catalog_OrdersEntriesByProductId()
        {
            ProductDefinition productZ =
                EconomyTestFactory.Product("z-product");
            ProductDefinition productA =
                EconomyTestFactory.Product("a-product");

            ProductSalePriceCatalog catalog =
                new ProductSalePriceCatalog(
                    EconomyTestFactory.Eur,
                    new[]
                    {
                        new ProductSalePrice(
                            productZ.Id,
                            EconomyTestFactory.EurMoney(200)),
                        new ProductSalePrice(
                            productA.Id,
                            EconomyTestFactory.EurMoney(100))
                    });

            Assert.That(
                catalog.Entries[0].ProductId.Value,
                Is.EqualTo("a-product"));
        }
    }
}
