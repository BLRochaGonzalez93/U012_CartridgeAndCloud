using NUnit.Framework;
using System;
using System.Collections.Generic;
using VRMGames.CartridgeAndCloud.Application.GameSession;
using VRMGames.CartridgeAndCloud.Domain.GameSession;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode
{
    public sealed class GameSessionTests
    {
        [Test]
        public void CreateNew_UsesMinimumValidDefaults()
        {
            DateTime created = new DateTime(638863200000000000L, DateTimeKind.Utc);
            GameSession session = GameSession.CreateNew(
                new SaveSlotId(1),
                StableId.New(),
                created);

            Assert.That(session.SlotId, Is.EqualTo(new SaveSlotId(1)));
            Assert.That(session.CurrentDay, Is.EqualTo(1));
            Assert.That(session.CashCents, Is.EqualTo(0L));
            Assert.That(session.CreatedUtc, Is.EqualTo(created));
        }

        [Test]
        public void Snapshot_Restore_PreservesMinimumState()
        {
            DateTime created = new DateTime(638863200000000000L, DateTimeKind.Utc);
            DateTime updated = created.AddMinutes(5);
            StableId id = StableId.New();

            GameSession source = GameSession.CreateNew(
                new SaveSlotId(2),
                id,
                created);

            source.SetCurrentDay(4);
            source.SetCashCents(12345L);

            GameSession restored = GameSession.Restore(source.CaptureSnapshot(updated));

            Assert.That(restored.SessionId, Is.EqualTo(id));
            Assert.That(restored.SlotId, Is.EqualTo(new SaveSlotId(2)));
            Assert.That(restored.CurrentDay, Is.EqualTo(4));
            Assert.That(restored.CashCents, Is.EqualTo(12345L));
        }

        [Test]
        public void SaveLoadDelete_CompletesSlotLifecycle()
        {
            MemoryRepository repository = new MemoryRepository();
            FixedClock clock = new FixedClock(
                new DateTime(638863200000000000L, DateTimeKind.Utc));
            GameSessionService service = new GameSessionService(repository, clock);
            SaveSlotId slot = new SaveSlotId(0);

            service.StartNew(slot);
            StableId originalId = service.Current.SessionId;
            service.Current.SetCurrentDay(3);
            service.Current.SetCashCents(5000L);

            Assert.That(service.SaveCurrent(), Is.EqualTo(GameSessionOperationResult.Success));
            Assert.That(service.SlotExists(slot), Is.True);

            service.Current.SetCashCents(0L);

            Assert.That(service.Load(slot), Is.EqualTo(GameSessionOperationResult.Success));
            Assert.That(service.Current.SessionId, Is.EqualTo(originalId));
            Assert.That(service.Current.CurrentDay, Is.EqualTo(3));
            Assert.That(service.Current.CashCents, Is.EqualTo(5000L));

            Assert.That(service.Delete(slot), Is.EqualTo(GameSessionOperationResult.Success));
            Assert.That(service.HasActiveSession, Is.False);
            Assert.That(service.SlotExists(slot), Is.False);
        }

        [Test]
        public void Load_EmptySlot_ReturnsSlotEmpty()
        {
            GameSessionService service = new GameSessionService(
                new MemoryRepository(),
                new FixedClock(DateTime.UtcNow));

            Assert.That(
                service.Load(new SaveSlotId(1)),
                Is.EqualTo(GameSessionOperationResult.SlotEmpty));
        }

        private sealed class FixedClock : IUtcClock
        {
            public DateTime UtcNow { get; }

            public FixedClock(DateTime utcNow)
            {
                UtcNow = utcNow;
            }
        }

        private sealed class MemoryRepository : ISaveGameRepository
        {
            private readonly Dictionary<int, GameSessionSnapshot> _snapshots =
                new Dictionary<int, GameSessionSnapshot>();

            public bool Exists(SaveSlotId slotId)
            {
                return _snapshots.ContainsKey(slotId.Value);
            }

            public void Save(GameSessionSnapshot snapshot)
            {
                _snapshots[snapshot.SlotId.Value] = snapshot;
            }

            public bool TryLoad(
                SaveSlotId slotId,
                out GameSessionSnapshot snapshot)
            {
                return _snapshots.TryGetValue(slotId.Value, out snapshot);
            }

            public bool Delete(SaveSlotId slotId)
            {
                return _snapshots.Remove(slotId.Value);
            }
        }
    }
}
