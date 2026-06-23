using System;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;

namespace VRMGames.CartridgeAndCloud.Domain.GameSession
{
    public sealed class GameSessionSnapshot
    {
        public const int CurrentSchemaVersion = 1;

        public int SchemaVersion { get; }
        public StableId SessionId { get; }
        public SaveSlotId SlotId { get; }
        public DateTime CreatedUtc { get; }
        public DateTime UpdatedUtc { get; }
        public int CurrentDay { get; }
        public long CashCents { get; }

        public GameSessionSnapshot(
            int schemaVersion,
            StableId sessionId,
            SaveSlotId slotId,
            DateTime createdUtc,
            DateTime updatedUtc,
            int currentDay,
            long cashCents)
        {
            if (schemaVersion != CurrentSchemaVersion)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(schemaVersion),
                    schemaVersion,
                    $"Only schema version {CurrentSchemaVersion} is supported.");
            }

            if (createdUtc.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException("CreatedUtc must use UTC.", nameof(createdUtc));
            }

            if (updatedUtc.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException("UpdatedUtc must use UTC.", nameof(updatedUtc));
            }

            if (updatedUtc < createdUtc)
            {
                throw new ArgumentException(
                    "UpdatedUtc cannot be earlier than CreatedUtc.",
                    nameof(updatedUtc));
            }

            if (currentDay < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(currentDay),
                    currentDay,
                    "Current day must be at least 1.");
            }

            SchemaVersion = schemaVersion;
            SessionId = sessionId;
            SlotId = slotId;
            CreatedUtc = createdUtc;
            UpdatedUtc = updatedUtc;
            CurrentDay = currentDay;
            CashCents = cashCents;
        }
    }
}
