using System;
using VRMGames.CartridgeAndCloud.Domain.DayCycle;

namespace VRMGames.CartridgeAndCloud.Domain.Economy
{
    public sealed class DailyEconomicResult
    {
        public StoreDayId DayId { get; }

        public CurrencyCode Currency { get; }

        public Money CheckoutRevenue { get; }

        public Money SupplierReceivingCost { get; }

        public Money EmployeeSalaryCost { get; }

        public Money GrossResult { get; }

        public Money OperatingResult { get; }

        public int CheckoutPostingCount { get; }

        public int SupplierReceiptPostingCount { get; }

        public int CustomerArrivals { get; }

        public int CompletedCheckouts { get; }

        public int ElapsedOpenSeconds { get; }

        public DailyEconomicResult(
            StoreDayId dayId,
            Money checkoutRevenue,
            Money supplierReceivingCost,
            Money employeeSalaryCost,
            int checkoutPostingCount,
            int supplierReceiptPostingCount,
            int customerArrivals,
            int completedCheckouts,
            int elapsedOpenSeconds)
        {
            if (checkoutRevenue.Currency !=
                    supplierReceivingCost.Currency ||
                checkoutRevenue.Currency !=
                    employeeSalaryCost.Currency)
            {
                throw new ArgumentException(
                    "Daily-result amounts must use the same currency.");
            }

            if (checkoutPostingCount < 0)
                throw new ArgumentOutOfRangeException(
                    nameof(checkoutPostingCount));
            if (supplierReceiptPostingCount < 0)
                throw new ArgumentOutOfRangeException(
                    nameof(supplierReceiptPostingCount));
            if (customerArrivals < 0)
                throw new ArgumentOutOfRangeException(
                    nameof(customerArrivals));
            if (completedCheckouts < 0)
                throw new ArgumentOutOfRangeException(
                    nameof(completedCheckouts));
            if (elapsedOpenSeconds < 0)
                throw new ArgumentOutOfRangeException(
                    nameof(elapsedOpenSeconds));

            DayId = dayId;
            Currency = checkoutRevenue.Currency;
            CheckoutRevenue = checkoutRevenue;
            SupplierReceivingCost = supplierReceivingCost;
            EmployeeSalaryCost = employeeSalaryCost;
            GrossResult =
                checkoutRevenue.Subtract(
                    supplierReceivingCost);
            OperatingResult =
                GrossResult.Subtract(employeeSalaryCost);
            CheckoutPostingCount = checkoutPostingCount;
            SupplierReceiptPostingCount =
                supplierReceiptPostingCount;
            CustomerArrivals = customerArrivals;
            CompletedCheckouts = completedCheckouts;
            ElapsedOpenSeconds = elapsedOpenSeconds;
        }
    }
}
