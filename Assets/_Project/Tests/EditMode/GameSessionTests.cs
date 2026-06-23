using System;
using NUnit.Framework;
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
    }
}
