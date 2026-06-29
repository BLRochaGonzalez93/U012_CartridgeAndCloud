using System;
using VRMGames.CartridgeAndCloud.Domain.DayCycle;
using VRMGames.CartridgeAndCloud.Domain.Economy;

namespace VRMGames.CartridgeAndCloud.Application.Economy
{
    public enum DailyResultsFailureReason
    {
        None = 0,
        DayNotClosed = 1,
        CheckoutCountMismatch = 2
    }

    public sealed class DailyResultsCreationResult
    {
        public bool Succeeded { get; }

        public DailyResultsFailureReason FailureReason { get; }

        public DailyEconomicResult Result { get; }

        private DailyResultsCreationResult(
            bool succeeded,
            DailyResultsFailureReason failureReason,
            DailyEconomicResult result)
        {
            Succeeded = succeeded;
            FailureReason = failureReason;
            Result = result;
        }

        public static DailyResultsCreationResult Success(
            DailyEconomicResult result)
        {
            return new DailyResultsCreationResult(
                true,
                DailyResultsFailureReason.None,
                result);
        }

        public static DailyResultsCreationResult Failure(
            DailyResultsFailureReason reason)
        {
            return new DailyResultsCreationResult(
                false,
                reason,
                null);
        }
    }

    public sealed class DailyResultsService
    {
        public DailyResultsCreationResult TryCreate(
            StoreDayActivitySummary activity,
            EconomyLedger ledger)
        {
            if (activity == null)
            {
                throw new ArgumentNullException(nameof(activity));
            }

            if (ledger == null)
            {
                throw new ArgumentNullException(nameof(ledger));
            }

            if (activity.FinalState != StoreDayState.Closed)
            {
                return DailyResultsCreationResult.Failure(
                    DailyResultsFailureReason.DayNotClosed);
            }

            int checkoutPostingCount =
                ledger.GetPostingCount(
                    activity.DayId,
                    EconomyPostingType.CheckoutRevenue);

            if (checkoutPostingCount !=
                activity.CompletedCheckouts)
            {
                return DailyResultsCreationResult.Failure(
                    DailyResultsFailureReason
                        .CheckoutCountMismatch);
            }

            Money revenue =
                ledger.GetTotal(
                    activity.DayId,
                    EconomyPostingType.CheckoutRevenue);

            Money supplierCost =
                ledger.GetTotal(
                    activity.DayId,
                    EconomyPostingType
                        .SupplierReceivingCost);

            int supplierReceiptCount =
                ledger.GetPostingCount(
                    activity.DayId,
                    EconomyPostingType
                        .SupplierReceivingCost);

            return DailyResultsCreationResult.Success(
                new DailyEconomicResult(
                    activity.DayId,
                    revenue,
                    supplierCost,
                    checkoutPostingCount,
                    supplierReceiptCount,
                    activity.CustomerArrivals,
                    activity.CompletedCheckouts,
                    activity.ElapsedOpenSeconds));
        }
    }
}
