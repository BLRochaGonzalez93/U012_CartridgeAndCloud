using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.Economy;
using VRMGames.CartridgeAndCloud.Domain.DayCycle;
using VRMGames.CartridgeAndCloud.Domain.Economy;
using VRMGames.CartridgeAndCloud.Domain.Inventory;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Economy
{
    public sealed class EconomyIntegrationTests
    {
        [Test] public void CheckoutReceiptAndDayResult_AreConsistent()
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

        [Test] public void DuplicateOperations_DoNotChangeDailyTotals()
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
