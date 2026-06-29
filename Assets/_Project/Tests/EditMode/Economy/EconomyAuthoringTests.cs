using NUnit.Framework;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.Economy;
using VRMGames.CartridgeAndCloud.Infrastructure.Economy;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Economy
{
    public sealed class EconomyAuthoringTests
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
                EconomyTechnicalScenarioRunner runner =
                    gameObject.AddComponent<
                        EconomyTechnicalScenarioRunner>();

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

                EconomyTechnicalScenarioRunner runner =
                    gameObject.AddComponent<
                        EconomyTechnicalScenarioRunner>();
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
    }
}
