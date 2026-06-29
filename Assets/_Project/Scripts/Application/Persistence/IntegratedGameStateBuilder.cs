using System;
using System.Collections.Generic;
using VRMGames.CartridgeAndCloud.Domain.GameSession;
using VRMGames.CartridgeAndCloud.Domain.Persistence;

namespace VRMGames.CartridgeAndCloud.Application.Persistence
{
    public sealed class IntegratedGameStateBuilder
    {
        public IntegratedGameStateSnapshot Build(
            GameSessionSnapshot session,
            string currencyCode,
            IEnumerable<InventoryContainerSaveRecord>
                inventories,
            IEnumerable<SupplierOrderSaveRecord>
                supplierOrders,
            IEnumerable<DisplaySaveRecord> displays,
            IEnumerable<CustomerSaveRecord> customers,
            IEnumerable<ShoppingSessionSaveRecord>
                shoppingSessions,
            IEnumerable<ReservationSaveRecord>
                reservations,
            IEnumerable<CheckoutQueueEntrySaveRecord>
                queueEntries,
            CheckoutStationSaveRecord checkoutStation,
            IEnumerable<CheckoutTransactionSaveRecord>
                transactions,
            DayCycleSaveRecord dayCycle,
            IEnumerable<EconomyLedgerSaveRecord>
                ledgerEntries)
        {
            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            return new IntegratedGameStateSnapshot(
                IntegratedGameStateSnapshot
                    .CurrentSchemaVersion,
                session.SessionId,
                session.SlotId,
                session.CreatedUtc,
                session.UpdatedUtc,
                session.CurrentDay,
                session.CashCents,
                currencyCode,
                inventories,
                supplierOrders,
                displays,
                customers,
                shoppingSessions,
                reservations,
                queueEntries,
                checkoutStation,
                transactions,
                dayCycle,
                ledgerEntries);
        }

        public GameSessionSnapshot ExtractGameSession(
            IntegratedGameStateSnapshot snapshot)
        {
            if (snapshot == null)
            {
                throw new ArgumentNullException(nameof(snapshot));
            }

            return new GameSessionSnapshot(
                GameSessionSnapshot.CurrentSchemaVersion,
                snapshot.SessionId,
                snapshot.SlotId,
                snapshot.CreatedUtc,
                snapshot.UpdatedUtc,
                snapshot.CurrentDay,
                snapshot.CashCents);
        }
    }
}
