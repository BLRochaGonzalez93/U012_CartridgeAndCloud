using System;

namespace VRMGames.CartridgeAndCloud.Domain.Checkout
{
    public enum CheckoutStationState
    {
        Closed = 0,
        Available = 1,
        Busy = 2
    }

    public sealed class CheckoutStation
    {
        private CheckoutQueueEntryId _currentEntryId;

        public CheckoutStationId Id { get; }

        public CheckoutStationState State { get; private set; }

        public bool IsOpen =>
            State != CheckoutStationState.Closed;

        public bool IsBusy =>
            State == CheckoutStationState.Busy;

        public CheckoutQueueEntryId CurrentEntryId =>
            _currentEntryId;

        public CheckoutStation(CheckoutStationId id)
        {
            if (string.IsNullOrWhiteSpace(id.Value))
            {
                throw new ArgumentException(
                    "Checkout station ID must be initialized.",
                    nameof(id));
            }

            Id = id;
            State = CheckoutStationState.Closed;
            _currentEntryId = default(CheckoutQueueEntryId);
        }

        public bool TryOpen()
        {
            if (State != CheckoutStationState.Closed)
            {
                return false;
            }

            State = CheckoutStationState.Available;
            return true;
        }

        public bool TryClose()
        {
            if (State != CheckoutStationState.Available)
            {
                return false;
            }

            State = CheckoutStationState.Closed;
            return true;
        }

        public bool TryBeginProcessing(
            CheckoutQueueEntryId entryId)
        {
            if (State != CheckoutStationState.Available ||
                string.IsNullOrWhiteSpace(entryId.Value))
            {
                return false;
            }

            _currentEntryId = entryId;
            State = CheckoutStationState.Busy;
            return true;
        }

        public bool TryCompleteProcessing(
            CheckoutQueueEntryId entryId)
        {
            if (State != CheckoutStationState.Busy ||
                _currentEntryId != entryId)
            {
                return false;
            }

            _currentEntryId =
                default(CheckoutQueueEntryId);
            State = CheckoutStationState.Available;
            return true;
        }
    }
}
