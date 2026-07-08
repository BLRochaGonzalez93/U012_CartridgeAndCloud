using System;
using System.Globalization;

namespace VRMGames.CartridgeAndCloud.Domain.DayCycle
{
    /// <summary>
    /// Maps the configurable real-time day duration to a deterministic
    /// 24-hour virtual clock and owns the H6 trading window.
    /// </summary>
    public static class StoreTradingHoursPolicy
    {
        public const int MinutesPerDay = 24 * 60;
        public const int OpeningMinute = 8 * 60;
        public const int ClosingMinute = 22 * 60;

        public static int OpeningSecond(int dayDurationSeconds)
        {
            return MapMinuteToSecond(OpeningMinute, dayDurationSeconds);
        }

        public static int ClosingSecond(int dayDurationSeconds)
        {
            return MapMinuteToSecond(ClosingMinute, dayDurationSeconds);
        }

        public static bool CanOpen(int elapsedDaySeconds, int dayDurationSeconds)
        {
            Validate(elapsedDaySeconds, dayDurationSeconds);
            return elapsedDaySeconds >= OpeningSecond(dayDurationSeconds) &&
                   elapsedDaySeconds < ClosingSecond(dayDurationSeconds);
        }


        public static bool CanTransition(
            string currentState,
            string targetState,
            int elapsedDaySeconds,
            int dayDurationSeconds)
        {
            Validate(elapsedDaySeconds, dayDurationSeconds);

            if (string.Equals(targetState, "Open", StringComparison.Ordinal))
            {
                return (string.Equals(currentState, "BeforeOpen", StringComparison.Ordinal) ||
                        string.Equals(currentState, "Closed", StringComparison.Ordinal)) &&
                       CanOpen(elapsedDaySeconds, dayDurationSeconds);
            }

            if (string.Equals(currentState, "Open", StringComparison.Ordinal) &&
                string.Equals(targetState, "Closing", StringComparison.Ordinal))
            {
                return true;
            }

            if (string.Equals(currentState, "Closing", StringComparison.Ordinal) &&
                string.Equals(targetState, "Closed", StringComparison.Ordinal))
            {
                return true;
            }

            return string.Equals(currentState, "BeforeOpen", StringComparison.Ordinal) &&
                   string.Equals(targetState, "Closed", StringComparison.Ordinal) &&
                   IsDayComplete(elapsedDaySeconds, dayDurationSeconds);
        }

        public static bool IsDayComplete(int elapsedDaySeconds, int dayDurationSeconds)
        {
            Validate(elapsedDaySeconds, dayDurationSeconds);
            return elapsedDaySeconds >= dayDurationSeconds;
        }

        public static bool HasReachedForcedClosing(int elapsedDaySeconds, int dayDurationSeconds)
        {
            Validate(elapsedDaySeconds, dayDurationSeconds);
            return elapsedDaySeconds >= ClosingSecond(dayDurationSeconds);
        }

        public static int VirtualMinute(int elapsedDaySeconds, int dayDurationSeconds)
        {
            Validate(elapsedDaySeconds, dayDurationSeconds);
            if (elapsedDaySeconds >= dayDurationSeconds)
            {
                return MinutesPerDay;
            }

            return Math.Min(
                MinutesPerDay - 1,
                (int)Math.Floor(
                    elapsedDaySeconds * (double)MinutesPerDay /
                    dayDurationSeconds));
        }

        public static string FormatTime(int elapsedDaySeconds, int dayDurationSeconds)
        {
            int minute = VirtualMinute(elapsedDaySeconds, dayDurationSeconds);
            if (minute >= MinutesPerDay)
            {
                return "24:00";
            }

            int hours = minute / 60;
            int minutes = minute % 60;
            return hours.ToString("00", CultureInfo.InvariantCulture) + ":" +
                   minutes.ToString("00", CultureInfo.InvariantCulture);
        }

        private static int MapMinuteToSecond(int virtualMinute, int dayDurationSeconds)
        {
            if (dayDurationSeconds <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(dayDurationSeconds));
            }

            return (int)Math.Round(
                dayDurationSeconds * (double)virtualMinute / MinutesPerDay,
                MidpointRounding.AwayFromZero);
        }

        private static void Validate(int elapsedDaySeconds, int dayDurationSeconds)
        {
            if (dayDurationSeconds <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(dayDurationSeconds));
            }

            if (elapsedDaySeconds < 0 || elapsedDaySeconds > dayDurationSeconds)
            {
                throw new ArgumentOutOfRangeException(nameof(elapsedDaySeconds));
            }
        }
    }
}
