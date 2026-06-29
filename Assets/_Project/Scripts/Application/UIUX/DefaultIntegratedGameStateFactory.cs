using System;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;
using VRMGames.CartridgeAndCloud.Domain.Persistence;

namespace VRMGames.CartridgeAndCloud.Application.UIUX
{
    public sealed class DefaultIntegratedGameStateFactory
    {
        private readonly string _currencyCode;
        private readonly long _initialCashCents;
        private readonly int _dayDurationSeconds;

        public DefaultIntegratedGameStateFactory(
            string currencyCode,
            long initialCashCents,
            int dayDurationSeconds)
        {
            if (string.IsNullOrWhiteSpace(
                    currencyCode) ||
                currencyCode.Length != 3)
            {
                throw new ArgumentException(
                    "A three-character currency is required.",
                    nameof(currencyCode));
            }

            if (dayDurationSeconds < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(dayDurationSeconds));
            }

            _currencyCode =
                currencyCode.ToUpperInvariant();
            _initialCashCents = initialCashCents;
            _dayDurationSeconds =
                dayDurationSeconds;
        }

        public IntegratedGameStateSnapshot Create(
            SaveSlotId slotId,
            DateTime utcNow)
        {
            if (utcNow.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException(
                    "Creation time must use UTC.",
                    nameof(utcNow));
            }

            return new IntegratedGameStateSnapshot(
                IntegratedGameStateSnapshot
                    .CurrentSchemaVersion,
                StableId.New(),
                slotId,
                utcNow,
                utcNow,
                1,
                _initialCashCents,
                _currencyCode,
                new[]
                {
                    new InventoryContainerSaveRecord(
                        "store-inventory",
                        100,
                        new ProductQuantitySaveRecord[0]),
                    new InventoryContainerSaveRecord(
                        "backroom-inventory",
                        200,
                        new ProductQuantitySaveRecord[0])
                },
                new SupplierOrderSaveRecord[0],
                new DisplaySaveRecord[0],
                new CustomerSaveRecord[0],
                new ShoppingSessionSaveRecord[0],
                new ReservationSaveRecord[0],
                new CheckoutQueueEntrySaveRecord[0],
                new CheckoutStationSaveRecord(
                    "checkout-station-main",
                    "Closed",
                    string.Empty),
                new CheckoutTransactionSaveRecord[0],
                new DayCycleSaveRecord(
                    "day-001",
                    "BeforeOpen",
                    _dayDurationSeconds,
                    0,
                    true),
                new EconomyLedgerSaveRecord[0]);
        }
    }
}
