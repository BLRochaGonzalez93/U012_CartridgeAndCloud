using System;

namespace VRMGames.CartridgeAndCloud.Domain.Economy
{
    public sealed class WeeklyTaxPolicy
    {
        public const int RatePercent = 10;

        public long CalculateTaxCents(long weeklyGrossResultCents)
        {
            long taxableBase = Math.Max(0, weeklyGrossResultCents);
            return checked(taxableBase * RatePercent / 100);
        }
    }
}
