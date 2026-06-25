using System;

namespace VRMGames.CartridgeAndCloud.Domain.Customers
{
    public sealed class CustomerArrivalClock
    {
        public int IntervalSeconds { get; }

        public int AccumulatedSeconds { get; private set; }

        public CustomerArrivalClock(int intervalSeconds)
        {
            if (intervalSeconds <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(intervalSeconds));
            }

            IntervalSeconds = intervalSeconds;
        }

        public int Advance(int elapsedSeconds)
        {
            if (elapsedSeconds < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(elapsedSeconds));
            }

            checked
            {
                AccumulatedSeconds += elapsedSeconds;
            }

            int due = AccumulatedSeconds / IntervalSeconds;
            AccumulatedSeconds %= IntervalSeconds;
            return due;
        }

        public void Reset()
        {
            AccumulatedSeconds = 0;
        }
    }
}
