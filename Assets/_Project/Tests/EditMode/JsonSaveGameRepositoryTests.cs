using System;
using System.IO;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.GameSession;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;
using VRMGames.CartridgeAndCloud.Infrastructure.GameSession;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode
{
    public sealed class JsonSaveGameRepositoryTests
    {
        private string _temporaryDirectory;

        [SetUp]
        public void SetUp()
        {
            _temporaryDirectory = Path.Combine(
                Path.GetTempPath(),
                "CartridgeAndCloudTests",
                Guid.NewGuid().ToString("N"));
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(_temporaryDirectory))
            {
                Directory.Delete(_temporaryDirectory, true);
            }
        }

        [Test]
        public void SaveLoadDelete_RoundTripsJsonFile()
        {
            JsonSaveGameRepository repository =
                new JsonSaveGameRepository(_temporaryDirectory);
            SaveSlotId slot = new SaveSlotId(2);
            DateTime created = new DateTime(638863200000000000L, DateTimeKind.Utc);
            GameSessionSnapshot snapshot = new GameSessionSnapshot(
                GameSessionSnapshot.CurrentSchemaVersion,
                StableId.New(),
                slot,
                created,
                created.AddSeconds(30),
                2,
                9876L);

            repository.Save(snapshot);

            Assert.That(repository.Exists(slot), Is.True);
            Assert.That(repository.TryLoad(slot, out GameSessionSnapshot loaded), Is.True);
            Assert.That(loaded.SessionId, Is.EqualTo(snapshot.SessionId));
            Assert.That(loaded.CurrentDay, Is.EqualTo(2));
            Assert.That(loaded.CashCents, Is.EqualTo(9876L));
            Assert.That(repository.Delete(slot), Is.True);
            Assert.That(repository.Exists(slot), Is.False);
        }
    }
}
