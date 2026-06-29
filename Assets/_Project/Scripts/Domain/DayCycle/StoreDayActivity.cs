using System;

namespace VRMGames.CartridgeAndCloud.Domain.DayCycle
{
    public sealed class StoreDayActivitySummary
    {
        public StoreDayId DayId { get; }

        public StoreDayState FinalState { get; }

        public int ElapsedOpenSeconds { get; }

        public int CustomerArrivals { get; }

        public int CustomersDirectedToExit { get; }

        public int CompletedCheckouts { get; }

        public int CancelledQueueEntries { get; }

        public int AbandonedShoppingSessions { get; }

        public int ReleasedReservations { get; }

        public int FinalActiveCustomers { get; }

        public int FinalQueueEntries { get; }

        public int FinalActiveReservations { get; }

        public StoreDayActivitySummary(
            StoreDayId dayId,
            StoreDayState finalState,
            int elapsedOpenSeconds,
            int customerArrivals,
            int customersDirectedToExit,
            int completedCheckouts,
            int cancelledQueueEntries,
            int abandonedShoppingSessions,
            int releasedReservations,
            int finalActiveCustomers,
            int finalQueueEntries,
            int finalActiveReservations)
        {
            DayId = dayId;
            FinalState = finalState;
            ElapsedOpenSeconds =
                RequireNonNegative(
                    elapsedOpenSeconds,
                    nameof(elapsedOpenSeconds));
            CustomerArrivals =
                RequireNonNegative(
                    customerArrivals,
                    nameof(customerArrivals));
            CustomersDirectedToExit =
                RequireNonNegative(
                    customersDirectedToExit,
                    nameof(customersDirectedToExit));
            CompletedCheckouts =
                RequireNonNegative(
                    completedCheckouts,
                    nameof(completedCheckouts));
            CancelledQueueEntries =
                RequireNonNegative(
                    cancelledQueueEntries,
                    nameof(cancelledQueueEntries));
            AbandonedShoppingSessions =
                RequireNonNegative(
                    abandonedShoppingSessions,
                    nameof(abandonedShoppingSessions));
            ReleasedReservations =
                RequireNonNegative(
                    releasedReservations,
                    nameof(releasedReservations));
            FinalActiveCustomers =
                RequireNonNegative(
                    finalActiveCustomers,
                    nameof(finalActiveCustomers));
            FinalQueueEntries =
                RequireNonNegative(
                    finalQueueEntries,
                    nameof(finalQueueEntries));
            FinalActiveReservations =
                RequireNonNegative(
                    finalActiveReservations,
                    nameof(finalActiveReservations));
        }

        private static int RequireNonNegative(
            int value,
            string parameterName)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(
                    parameterName);
            }

            return value;
        }
    }

    public sealed class StoreDayActivityTracker
    {
        public int CustomerArrivals { get; private set; }

        public int CustomersDirectedToExit { get; private set; }

        public int CompletedCheckouts { get; private set; }

        public int CancelledQueueEntries { get; private set; }

        public int AbandonedShoppingSessions { get; private set; }

        public int ReleasedReservations { get; private set; }

        public void RecordCustomerArrivals(int count)
        {
            CustomerArrivals =
                AddNonNegative(
                    CustomerArrivals,
                    count,
                    nameof(count));
        }

        public void RecordCustomersDirectedToExit(int count)
        {
            CustomersDirectedToExit =
                AddNonNegative(
                    CustomersDirectedToExit,
                    count,
                    nameof(count));
        }

        public void RecordCompletedCheckouts(int count)
        {
            CompletedCheckouts =
                AddNonNegative(
                    CompletedCheckouts,
                    count,
                    nameof(count));
        }

        public void RecordCancelledQueueEntries(int count)
        {
            CancelledQueueEntries =
                AddNonNegative(
                    CancelledQueueEntries,
                    count,
                    nameof(count));
        }

        public void RecordAbandonedShoppingSessions(int count)
        {
            AbandonedShoppingSessions =
                AddNonNegative(
                    AbandonedShoppingSessions,
                    count,
                    nameof(count));
        }

        public void RecordReleasedReservations(int count)
        {
            ReleasedReservations =
                AddNonNegative(
                    ReleasedReservations,
                    count,
                    nameof(count));
        }

        public StoreDayActivitySummary CreateSummary(
            StoreDay day,
            StoreClosureSnapshot finalSnapshot)
        {
            if (day == null)
            {
                throw new ArgumentNullException(nameof(day));
            }

            if (finalSnapshot == null)
            {
                throw new ArgumentNullException(
                    nameof(finalSnapshot));
            }

            return new StoreDayActivitySummary(
                day.Id,
                day.State,
                day.ElapsedOpenSeconds,
                CustomerArrivals,
                CustomersDirectedToExit,
                CompletedCheckouts,
                CancelledQueueEntries,
                AbandonedShoppingSessions,
                ReleasedReservations,
                finalSnapshot.ActiveCustomers,
                finalSnapshot.ActiveQueueEntries,
                finalSnapshot.ActiveReservations);
        }

        private static int AddNonNegative(
            int current,
            int increment,
            string parameterName)
        {
            if (increment < 0)
            {
                throw new ArgumentOutOfRangeException(
                    parameterName);
            }

            return checked(current + increment);
        }
    }
}
