using System;
using VRMGames.CartridgeAndCloud.Domain.Checkout;
using VRMGames.CartridgeAndCloud.Domain.Customers;
using VRMGames.CartridgeAndCloud.Domain.DayCycle;
using VRMGames.CartridgeAndCloud.Domain.Shopping;

namespace VRMGames.CartridgeAndCloud.Application.DayCycle
{
    public sealed class StoreClosureSnapshotFactory
    {
        public StoreClosureSnapshot Create(
            CustomerInstanceRegistry customers,
            CheckoutQueue queue,
            CheckoutStation station,
            ShoppingReservationRegistry reservations,
            CustomerShoppingSessionRegistry sessions)
        {
            if (customers == null)
                throw new ArgumentNullException(nameof(customers));
            if (queue == null)
                throw new ArgumentNullException(nameof(queue));
            if (station == null)
                throw new ArgumentNullException(nameof(station));
            if (reservations == null)
                throw new ArgumentNullException(nameof(reservations));
            if (sessions == null)
                throw new ArgumentNullException(nameof(sessions));

            return new StoreClosureSnapshot(
                customers.ActiveCount,
                queue.ActiveCount,
                station.State,
                reservations.ActiveCount,
                sessions.PendingCount);
        }
    }
}
