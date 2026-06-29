using System;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;

namespace VRMGames.CartridgeAndCloud.Domain.UIUX
{
    public enum SlotPresentationState
    {
        Empty = 0,
        Ready = 1,
        Recovered = 2,
        Corrupt = 3,
        UnsupportedSchema = 4,
        StorageFailure = 5
    }

    public sealed class SlotDescriptor
    {
        public SaveSlotId SlotId { get; }
        public SlotPresentationState State { get; }
        public int CurrentDay { get; }
        public long CashCents { get; }
        public DateTime? UpdatedUtc { get; }
        public string Detail { get; }

        public bool CanContinue =>
            State == SlotPresentationState.Ready ||
            State == SlotPresentationState.Recovered;

        public bool CanCreate =>
            State == SlotPresentationState.Empty ||
            State == SlotPresentationState.Corrupt ||
            State ==
                SlotPresentationState.UnsupportedSchema ||
            State ==
                SlotPresentationState.StorageFailure;

        public bool CanDelete =>
            State != SlotPresentationState.Empty;

        public SlotDescriptor(
            SaveSlotId slotId,
            SlotPresentationState state,
            int currentDay,
            long cashCents,
            DateTime? updatedUtc,
            string detail)
        {
            if (!Enum.IsDefined(
                    typeof(SlotPresentationState),
                    state))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(state));
            }

            if ((state == SlotPresentationState.Ready ||
                 state ==
                    SlotPresentationState.Recovered) &&
                currentDay < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(currentDay));
            }

            if (updatedUtc.HasValue &&
                updatedUtc.Value.Kind !=
                    DateTimeKind.Utc)
            {
                throw new ArgumentException(
                    "Updated time must use UTC.",
                    nameof(updatedUtc));
            }

            SlotId = slotId;
            State = state;
            CurrentDay = currentDay;
            CashCents = cashCents;
            UpdatedUtc = updatedUtc;
            Detail = detail ?? string.Empty;
        }

        public static SlotDescriptor Empty(
            SaveSlotId slotId)
        {
            return new SlotDescriptor(
                slotId,
                SlotPresentationState.Empty,
                0,
                0,
                null,
                "Empty slot.");
        }
    }
}
