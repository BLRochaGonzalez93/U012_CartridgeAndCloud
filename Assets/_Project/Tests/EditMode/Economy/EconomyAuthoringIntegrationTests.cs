using NUnit.Framework;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Application.Economy;
using VRMGames.CartridgeAndCloud.Domain.Economy;
using VRMGames.CartridgeAndCloud.Domain.Inventory;
using VRMGames.CartridgeAndCloud.Infrastructure.Economy;

using VRMGames.CartridgeAndCloud.Runtime.Development.Scenarios;
namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Economy
{
    public sealed class EconomyAuthoringIntegrationTests
    {
        [Test] public void SettingsAsset_BuildsCurrency()
        {
            EconomySettingsAsset asset =
                ScriptableObject.CreateInstance<
                    EconomySettingsAsset>();

            try
            {
                asset.Configure("eur");
                Assert.That(
                    asset.BuildCurrency().Value,
                    Is.EqualTo("EUR"));
            }
            finally
            {
                Object.DestroyImmediate(asset);
            }
        }

        [Test] public void SettingsAsset_BuildsLedger()
        {
            EconomySettingsAsset asset =
                ScriptableObject.CreateInstance<
                    EconomySettingsAsset>();

            try
            {
                asset.Configure("EUR");
                EconomyLedger ledger =
                    asset.BuildLedger();
                Assert.That(
                    ledger.Currency.Value,
                    Is.EqualTo("EUR"));
                Assert.That(ledger.Count, Is.EqualTo(0));
            }
            finally
            {
                Object.DestroyImmediate(asset);
            }
        }

        [Test] public void SalePriceAsset_BuildsCatalog()
        {
            ProductSalePriceCatalogAsset asset =
                ScriptableObject.CreateInstance<
                    ProductSalePriceCatalogAsset>();

            try
            {
                asset.Configure(
                    new[] { "product-a" },
                    new long[] { 2999 });

                ProductSalePriceCatalog catalog =
                    asset.BuildCatalog(
                        EconomyTestFactory.Eur);

                Assert.That(catalog.Count, Is.EqualTo(1));
                Assert.That(
                    catalog.Entries[0]
                        .UnitPrice.MinorUnits,
                    Is.EqualTo(2999));
            }
            finally
            {
                Object.DestroyImmediate(asset);
            }
        }

        [Test] public void SalePriceAsset_RejectsMismatchedArrays()
        {
            ProductSalePriceCatalogAsset asset =
                ScriptableObject.CreateInstance<
                    ProductSalePriceCatalogAsset>();

            try
            {
                Assert.Throws<System.ArgumentException>(
                    () => asset.Configure(
                        new[] { "product-a" },
                        new long[0]));
            }
            finally
            {
                Object.DestroyImmediate(asset);
            }
        }

        [Test] public void TechnicalRunner_RequiresSettings()
        {
            GameObject gameObject =
                new GameObject("EconomyRunnerTest");

            try
            {
                EconomyScenarioRunner runner =
                    gameObject.AddComponent<
                        EconomyScenarioRunner>();

                Assert.Throws<
                    System.InvalidOperationException>(
                    () => runner.RunScenario());
            }
            finally
            {
                Object.DestroyImmediate(gameObject);
            }
        }

        [Test] public void TechnicalRunner_CompletesScenario()
        {
            EconomySettingsAsset settings =
                ScriptableObject.CreateInstance<
                    EconomySettingsAsset>();
            ProductSalePriceCatalogAsset prices =
                ScriptableObject.CreateInstance<
                    ProductSalePriceCatalogAsset>();
            GameObject gameObject =
                new GameObject("EconomyRunnerTest");

            try
            {
                settings.Configure("EUR");
                prices.Configure(
                    new[] { "product-a" },
                    new long[] { 2999 });

                EconomyScenarioRunner runner =
                    gameObject.AddComponent<
                        EconomyScenarioRunner>();
                runner.Configure(
                    settings,
                    prices,
                    false);
                runner.RunScenario();

                Assert.That(
                    runner.LastScenarioPassed,
                    Is.True);
                Assert.That(
                    runner.LastCheckoutRevenueCents,
                    Is.EqualTo(5998));
                Assert.That(
                    runner.LastSupplierCostCents,
                    Is.EqualTo(3600));
                Assert.That(
                    runner.LastGrossResultCents,
                    Is.EqualTo(2398));
                Assert.That(
                    runner.LastLedgerEntries,
                    Is.EqualTo(2));
                Assert.That(
                    runner.LastDuplicateCheckoutBlocked,
                    Is.True);
                Assert.That(
                    runner.LastDuplicateReceiptBlocked,
                    Is.True);
            }
            finally
            {
                Object.DestroyImmediate(gameObject);
                Object.DestroyImmediate(settings);
                Object.DestroyImmediate(prices);
            }
        }
        [Test]
        public void CheckoutReceiptAndDayResult_AreConsistent()
        {
            EconomyCheckoutScenario scenario =
                EconomyTestFactory.CheckoutScenario(
                    quantity: 2,
                    unitPriceCents: 2999,
                    stock: 5);

            var checkout =
                EconomyTestFactory.Checkout(scenario);

            var receipt =
                new SupplierReceivingEconomyService()
                    .TryRecordReceivedCost(
                        scenario.DayId,
                        "receipt-a",
                        EconomyTestFactory.SupplierEntry(
                            scenario.Product,
                            unitCostCents: 1200),
                        new Quantity(3),
                        scenario.Ledger);

            var daily =
                new DailyResultsService().TryCreate(
                    EconomyTestFactory.ClosedActivity(
                        scenario.DayId),
                    scenario.Ledger);

            Assert.That(checkout.Succeeded, Is.True);
            Assert.That(receipt.Succeeded, Is.True);
            Assert.That(daily.Succeeded, Is.True);
            Assert.That(
                daily.Result.CheckoutRevenue.MinorUnits,
                Is.EqualTo(5998));
            Assert.That(
                daily.Result.SupplierReceivingCost.MinorUnits,
                Is.EqualTo(3600));
            Assert.That(
                daily.Result.GrossResult.MinorUnits,
                Is.EqualTo(2398));
        }

        [Test]
        public void DuplicateOperations_DoNotChangeDailyTotals()
        {
            EconomyCheckoutScenario scenario =
                EconomyTestFactory.CheckoutScenario(
                    quantity: 2,
                    unitPriceCents: 2999,
                    stock: 5);

            EconomyTestFactory.Checkout(scenario);

            SupplierReceivingEconomyService supplier =
                new SupplierReceivingEconomyService();
            supplier.TryRecordReceivedCost(
                scenario.DayId,
                "receipt-a",
                EconomyTestFactory.SupplierEntry(
                    scenario.Product,
                    unitCostCents: 1200),
                new Quantity(3),
                scenario.Ledger);

            EconomyTestFactory.Checkout(
                scenario,
                "transaction-b");
            supplier.TryRecordReceivedCost(
                scenario.DayId,
                "receipt-a",
                EconomyTestFactory.SupplierEntry(
                    scenario.Product,
                    unitCostCents: 1200),
                new Quantity(3),
                scenario.Ledger);

            var daily =
                new DailyResultsService().TryCreate(
                    EconomyTestFactory.ClosedActivity(
                        scenario.DayId),
                    scenario.Ledger);

            Assert.That(
                scenario.Ledger.Count,
                Is.EqualTo(2));
            Assert.That(
                daily.Result.GrossResult.MinorUnits,
                Is.EqualTo(2398));
        }
    }
}
