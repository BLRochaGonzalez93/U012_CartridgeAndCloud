using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.Economy;
using VRMGames.CartridgeAndCloud.Domain.DayCycle;
using VRMGames.CartridgeAndCloud.Domain.Economy;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Economy
{
    public sealed class DailyResultsServiceTests
    {
        private static void Post(
            EconomyLedger ledger,
            StoreDayId dayId,
            string id,
            EconomyPostingType type,
            long cents)
        {
            ledger.TryPost(
                new EconomyLedgerEntry(
                    new EconomyLedgerEntryId(id),
                    new EconomyPostingKey(type, id),
                    dayId,
                    EconomyTestFactory.EurMoney(cents)));
        }

        [Test] public void DailyResult_CreatesForClosedDay()
        {
            StoreDayId dayId = new StoreDayId("day-a");
            EconomyLedger ledger =
                new EconomyLedger(EconomyTestFactory.Eur);
            Post(
                ledger,
                dayId,
                "checkout-a",
                EconomyPostingType.CheckoutRevenue,
                5000);

            DailyResultsCreationResult result =
                new DailyResultsService().TryCreate(
                    EconomyTestFactory.ClosedActivity(dayId),
                    ledger);

            Assert.That(result.Succeeded, Is.True);
        }

        [Test] public void DailyResult_RejectsOpenDay()
        {
            StoreDayId dayId = new StoreDayId("day-a");
            StoreDayActivitySummary activity =
                new StoreDayActivitySummary(
                    dayId,
                    StoreDayState.Open,
                    100,
                    1,
                    0,
                    0,
                    0,
                    0,
                    0,
                    1,
                    0,
                    0);

            DailyResultsCreationResult result =
                new DailyResultsService().TryCreate(
                    activity,
                    new EconomyLedger(
                        EconomyTestFactory.Eur));

            Assert.That(
                result.FailureReason,
                Is.EqualTo(
                    DailyResultsFailureReason.DayNotClosed));
        }

        [Test] public void DailyResult_SumsRevenue()
        {
            StoreDayId dayId = new StoreDayId("day-a");
            EconomyLedger ledger =
                new EconomyLedger(EconomyTestFactory.Eur);
            Post(
                ledger,
                dayId,
                "checkout-a",
                EconomyPostingType.CheckoutRevenue,
                2000);
            Post(
                ledger,
                dayId,
                "checkout-b",
                EconomyPostingType.CheckoutRevenue,
                3000);

            DailyResultsCreationResult result =
                new DailyResultsService().TryCreate(
                    EconomyTestFactory.ClosedActivity(
                        dayId,
                        completedCheckouts: 2),
                    ledger);

            Assert.That(
                result.Result.CheckoutRevenue.MinorUnits,
                Is.EqualTo(5000));
        }

        [Test] public void DailyResult_SumsSupplierCost()
        {
            StoreDayId dayId = new StoreDayId("day-a");
            EconomyLedger ledger =
                new EconomyLedger(EconomyTestFactory.Eur);
            Post(
                ledger,
                dayId,
                "checkout-a",
                EconomyPostingType.CheckoutRevenue,
                5000);
            Post(
                ledger,
                dayId,
                "receipt-a",
                EconomyPostingType.SupplierReceivingCost,
                1200);
            Post(
                ledger,
                dayId,
                "receipt-b",
                EconomyPostingType.SupplierReceivingCost,
                800);

            DailyResultsCreationResult result =
                new DailyResultsService().TryCreate(
                    EconomyTestFactory.ClosedActivity(dayId),
                    ledger);

            Assert.That(
                result.Result.SupplierReceivingCost
                    .MinorUnits,
                Is.EqualTo(2000));
        }

        [Test] public void DailyResult_CalculatesPositiveGross()
        {
            StoreDayId dayId = new StoreDayId("day-a");
            EconomyLedger ledger =
                new EconomyLedger(EconomyTestFactory.Eur);
            Post(
                ledger,
                dayId,
                "checkout-a",
                EconomyPostingType.CheckoutRevenue,
                5000);
            Post(
                ledger,
                dayId,
                "receipt-a",
                EconomyPostingType.SupplierReceivingCost,
                3000);

            DailyEconomicResult result =
                new DailyResultsService().TryCreate(
                    EconomyTestFactory.ClosedActivity(dayId),
                    ledger).Result;

            Assert.That(
                result.GrossResult.MinorUnits,
                Is.EqualTo(2000));
        }

        [Test] public void DailyResult_AllowsNegativeGross()
        {
            StoreDayId dayId = new StoreDayId("day-a");
            EconomyLedger ledger =
                new EconomyLedger(EconomyTestFactory.Eur);
            Post(
                ledger,
                dayId,
                "checkout-a",
                EconomyPostingType.CheckoutRevenue,
                1000);
            Post(
                ledger,
                dayId,
                "receipt-a",
                EconomyPostingType.SupplierReceivingCost,
                3000);

            DailyEconomicResult result =
                new DailyResultsService().TryCreate(
                    EconomyTestFactory.ClosedActivity(dayId),
                    ledger).Result;

            Assert.That(
                result.GrossResult.MinorUnits,
                Is.EqualTo(-2000));
        }

        [Test] public void DailyResult_RejectsCheckoutCountMismatch()
        {
            StoreDayId dayId = new StoreDayId("day-a");
            EconomyLedger ledger =
                new EconomyLedger(EconomyTestFactory.Eur);
            Post(
                ledger,
                dayId,
                "checkout-a",
                EconomyPostingType.CheckoutRevenue,
                1000);

            DailyResultsCreationResult result =
                new DailyResultsService().TryCreate(
                    EconomyTestFactory.ClosedActivity(
                        dayId,
                        completedCheckouts: 2),
                    ledger);

            Assert.That(
                result.FailureReason,
                Is.EqualTo(
                    DailyResultsFailureReason
                        .CheckoutCountMismatch));
        }

        [Test] public void DailyResult_CapturesPostingCounts()
        {
            StoreDayId dayId = new StoreDayId("day-a");
            EconomyLedger ledger =
                new EconomyLedger(EconomyTestFactory.Eur);
            Post(
                ledger,
                dayId,
                "checkout-a",
                EconomyPostingType.CheckoutRevenue,
                1000);
            Post(
                ledger,
                dayId,
                "receipt-a",
                EconomyPostingType.SupplierReceivingCost,
                500);

            DailyEconomicResult result =
                new DailyResultsService().TryCreate(
                    EconomyTestFactory.ClosedActivity(dayId),
                    ledger).Result;

            Assert.That(
                result.CheckoutPostingCount,
                Is.EqualTo(1));
            Assert.That(
                result.SupplierReceiptPostingCount,
                Is.EqualTo(1));
        }

        [Test] public void DailyResult_CopiesActivityMetrics()
        {
            StoreDayId dayId = new StoreDayId("day-a");
            EconomyLedger ledger =
                new EconomyLedger(EconomyTestFactory.Eur);
            Post(
                ledger,
                dayId,
                "checkout-a",
                EconomyPostingType.CheckoutRevenue,
                1000);

            DailyEconomicResult result =
                new DailyResultsService().TryCreate(
                    EconomyTestFactory.ClosedActivity(dayId),
                    ledger).Result;

            Assert.That(
                result.CustomerArrivals,
                Is.EqualTo(2));
            Assert.That(
                result.ElapsedOpenSeconds,
                Is.EqualTo(300));
        }

        [Test] public void DailyResult_RejectsNullArguments()
        {
            DailyResultsService service =
                new DailyResultsService();

            Assert.Throws<System.ArgumentNullException>(
                () => service.TryCreate(
                    null,
                    new EconomyLedger(
                        EconomyTestFactory.Eur)));
            Assert.Throws<System.ArgumentNullException>(
                () => service.TryCreate(
                    EconomyTestFactory.ClosedActivity(
                        new StoreDayId("day-a"),
                        completedCheckouts: 0),
                    null));
        }
    }
}
