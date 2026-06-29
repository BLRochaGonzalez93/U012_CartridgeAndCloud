using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.Persistence;
using VRMGames.CartridgeAndCloud.Domain.GameSession;
using VRMGames.CartridgeAndCloud.Domain.Persistence;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Persistence
{
    public sealed class IntegratedGameStateBuilderTests
    {
        [Test] public void Builder_RejectsNullSession()
        {
            Assert.Throws<System.ArgumentNullException>(
                () => new IntegratedGameStateBuilder()
                    .Build(
                        null,
                        "EUR",
                        new InventoryContainerSaveRecord[0],
                        new SupplierOrderSaveRecord[0],
                        new DisplaySaveRecord[0],
                        new CustomerSaveRecord[0],
                        new ShoppingSessionSaveRecord[0],
                        new ReservationSaveRecord[0],
                        new CheckoutQueueEntrySaveRecord[0],
                        new CheckoutStationSaveRecord(
                            "station",
                            "Closed",
                            string.Empty),
                        new CheckoutTransactionSaveRecord[0],
                        new DayCycleSaveRecord(
                            "day",
                            "Closed",
                            1,
                            1,
                            true),
                        new EconomyLedgerSaveRecord[0]));
        }

        [Test] public void Builder_UsesSessionIdentity()
        {
            IntegratedGameStateSnapshot source =
                PersistenceTestFactory.ClosedSnapshot();
            GameSessionSnapshot session =
                new IntegratedGameStateBuilder()
                    .ExtractGameSession(source);

            Assert.That(
                session.SessionId,
                Is.EqualTo(source.SessionId));
            Assert.That(
                session.SlotId,
                Is.EqualTo(source.SlotId));
        }

        [Test] public void Builder_UsesSessionEconomyFields()
        {
            IntegratedGameStateSnapshot source =
                PersistenceTestFactory.ClosedSnapshot(
                    cashCents: 9876);
            GameSessionSnapshot session =
                new IntegratedGameStateBuilder()
                    .ExtractGameSession(source);

            Assert.That(
                session.CurrentDay,
                Is.EqualTo(source.CurrentDay));
            Assert.That(
                session.CashCents,
                Is.EqualTo(9876));
        }

        [Test] public void Builder_RoundTripsSessionSnapshot()
        {
            IntegratedGameStateSnapshot source =
                PersistenceTestFactory.ClosedSnapshot();
            IntegratedGameStateBuilder builder =
                new IntegratedGameStateBuilder();
            GameSessionSnapshot session =
                builder.ExtractGameSession(source);

            IntegratedGameStateSnapshot rebuilt =
                builder.Build(
                    session,
                    source.CurrencyCode,
                    source.Inventories,
                    source.SupplierOrders,
                    source.Displays,
                    source.Customers,
                    source.ShoppingSessions,
                    source.Reservations,
                    source.QueueEntries,
                    source.CheckoutStation,
                    source.Transactions,
                    source.DayCycle,
                    source.LedgerEntries);

            Assert.That(
                rebuilt.TotalRecordCount,
                Is.EqualTo(source.TotalRecordCount));
            Assert.That(
                rebuilt.CashCents,
                Is.EqualTo(source.CashCents));
        }

        [Test] public void Builder_ExtractRejectsNull()
        {
            Assert.Throws<System.ArgumentNullException>(
                () => new IntegratedGameStateBuilder()
                    .ExtractGameSession(null));
        }
    }
}
