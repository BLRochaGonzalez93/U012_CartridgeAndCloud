using System;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;

namespace VRMGames.CartridgeAndCloud.Domain.GameSession
{
    public sealed class GameSession
    {
        public StableId SessionId { get; }
        public SaveSlotId SlotId { get; }
        public DateTime CreatedUtc { get; }
        public int CurrentDay { get; private set; }
        public long CashCents { get; private set; }

        private GameSession(
            StableId sessionId,
            SaveSlotId slotId,
            DateTime createdUtc,
            int currentDay,
            long cashCents)
        {
            SessionId = sessionId;
            SlotId = slotId;
            CreatedUtc = createdUtc;
            CurrentDay = currentDay;
            CashCents = cashCents;
        }

        public static GameSession CreateNew(
            SaveSlotId slotId,
            StableId sessionId,
            DateTime createdUtc)
        {
            if (createdUtc.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException("CreatedUtc must use UTC.", nameof(createdUtc));
            }

            return new GameSession(sessionId, slotId, createdUtc, 1, 0L);
        }

        public static GameSession Restore(GameSessionSnapshot snapshot)
        {
            if (snapshot == null)
            {
                throw new ArgumentNullException(nameof(snapshot));
            }

            return new GameSession(
                snapshot.SessionId,
                snapshot.SlotId,
                snapshot.CreatedUtc,
                snapshot.CurrentDay,
                snapshot.CashCents);
        }

        public void SetCashCents(long cashCents)
        {
            CashCents = cashCents;
        }

        public void SetCurrentDay(int currentDay)
        {
            if (currentDay < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(currentDay),
                    currentDay,
                    "Current day must be at least 1.");
            }

            CurrentDay = currentDay;
        }

        public GameSessionSnapshot CaptureSnapshot(DateTime updatedUtc)
        {
            return new GameSessionSnapshot(
                GameSessionSnapshot.CurrentSchemaVersion,
                SessionId,
                SlotId,
                CreatedUtc,
                updatedUtc,
                CurrentDay,
                CashCents);
        }
    }
}
