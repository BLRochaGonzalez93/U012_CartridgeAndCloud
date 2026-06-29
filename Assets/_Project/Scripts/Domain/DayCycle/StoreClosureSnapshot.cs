using System;
using VRMGames.CartridgeAndCloud.Domain.Checkout;

namespace VRMGames.CartridgeAndCloud.Domain.DayCycle
{
    public sealed class StoreClosureSnapshot
    {
        public int ActiveCustomers { get; }

        public int ActiveQueueEntries { get; }

        public CheckoutStationState StationState { get; }

        public int ActiveReservations { get; }

        public int PendingShoppingSessions { get; }

        public bool IsReadyToClose =>
            ActiveCustomers == 0 &&
            ActiveQueueEntries == 0 &&
            StationState != CheckoutStationState.Busy &&
            ActiveReservations == 0 &&
            PendingShoppingSessions == 0;

        public StoreClosureSnapshot(
            int activeCustomers,
            int activeQueueEntries,
            CheckoutStationState stationState,
            int activeReservations,
            int pendingShoppingSessions)
        {
            if (activeCustomers < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(activeCustomers));
            }

            if (activeQueueEntries < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(activeQueueEntries));
            }

            if (activeReservations < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(activeReservations));
            }

            if (pendingShoppingSessions < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(pendingShoppingSessions));
            }

            ActiveCustomers = activeCustomers;
            ActiveQueueEntries = activeQueueEntries;
            StationState = stationState;
            ActiveReservations = activeReservations;
            PendingShoppingSessions =
                pendingShoppingSessions;
        }
    }
}
