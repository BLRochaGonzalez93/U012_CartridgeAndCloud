using System.IO;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;
using VRMGames.CartridgeAndCloud.Domain.VerticalSlicePhase1;
using VRMGames.CartridgeAndCloud.Infrastructure.VerticalSlicePhase1;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.VerticalSlicePhase1
{
    public sealed class Phase1RepositoryTests
    {
        private string _directory;
        private JsonPhase1StateRepository
            _repository;

        [SetUp]
        public void SetUp()
        {
            _directory =
                Phase1TestFactory.TempDirectory();

            _repository =
                new JsonPhase1StateRepository(
                    _directory);
        }

        [TearDown]
        public void TearDown()
        {
            Phase1TestFactory.DeleteDirectory(
                _directory);
        }

        [Test]
        public void MissingState_ReturnsNull()
        {
            Assert.That(
                _repository.Load(
                    new SaveSlotId(0)),
                Is.Null);
        }

        [Test]
        public void Save_CreatesPrimaryFile()
        {
            _repository.Save(State());

            Assert.That(
                File.Exists(
                    _repository.PrimaryPath(
                        new SaveSlotId(0))),
                Is.True);
        }

        [Test]
        public void SaveAndLoad_RoundTripsSession()
        {
            _repository.Save(State());

            Phase1StoreState loaded =
                _repository.Load(
                    new SaveSlotId(0));

            Assert.That(
                loaded.SessionId,
                Is.EqualTo("session-a"));
        }

        [Test]
        public void SaveAndLoad_RoundTripsOrders()
        {
            _repository.Save(State());

            Phase1StoreState loaded =
                _repository.Load(
                    new SaveSlotId(0));

            Assert.That(
                loaded.Orders.Count,
                Is.EqualTo(1));
            Assert.That(
                loaded.Orders[0].OrderId,
                Is.EqualTo("order-a"));
        }

        [Test]
        public void SecondSave_CreatesBackup()
        {
            _repository.Save(State());
            _repository.Save(State(generation: 2));

            Assert.That(
                File.Exists(
                    _repository.BackupPath(
                        new SaveSlotId(0))),
                Is.True);
        }

        [Test]
        public void CorruptPrimary_LoadsBackup()
        {
            _repository.Save(State());
            _repository.Save(State(generation: 2));

            File.WriteAllText(
                _repository.PrimaryPath(
                    new SaveSlotId(0)),
                "{broken");

            Phase1StoreState loaded =
                _repository.Load(
                    new SaveSlotId(0));

            Assert.That(loaded, Is.Not.Null);
            Assert.That(
                loaded.Generation,
                Is.EqualTo(1));
        }

        [Test]
        public void WrongSlotFile_IsRejected()
        {
            _repository.Save(State());

            string slotZero =
                _repository.PrimaryPath(
                    new SaveSlotId(0));
            string slotOne =
                _repository.PrimaryPath(
                    new SaveSlotId(1));

            File.Copy(
                slotZero,
                slotOne);

            Assert.That(
                _repository.Load(
                    new SaveSlotId(1)),
                Is.Null);
        }

        [Test]
        public void Delete_RemovesPrimaryAndBackup()
        {
            _repository.Save(State());
            _repository.Save(State(generation: 2));

            bool deleted =
                _repository.Delete(
                    new SaveSlotId(0));

            Assert.That(deleted, Is.True);
            Assert.That(
                File.Exists(
                    _repository.PrimaryPath(
                        new SaveSlotId(0))),
                Is.False);
            Assert.That(
                File.Exists(
                    _repository.BackupPath(
                        new SaveSlotId(0))),
                Is.False);
        }

        [Test]
        public void Delete_MissingReturnsFalse()
        {
            Assert.That(
                _repository.Delete(
                    new SaveSlotId(0)),
                Is.False);
        }

        [Test]
        public void Slots_AreIndependent()
        {
            _repository.Save(State());

            Phase1StoreState slotOne =
                new Phase1StoreState(
                    new SaveSlotId(1),
                    "session-b",
                    0,
                    1,
                    1,
                    1,
                    0,
                    0,
                    0,
                    new Phase1OrderRecord[0],
                    new Phase1StockRecord[0],
                    new Phase1StockRecord[0],
                    new Phase1PlacedFixtureRecord[0]);

            _repository.Save(slotOne);

            Assert.That(
                _repository.Load(
                    new SaveSlotId(0))
                    .SessionId,
                Is.EqualTo("session-a"));
            Assert.That(
                _repository.Load(
                    new SaveSlotId(1))
                    .SessionId,
                Is.EqualTo("session-b"));
        }

        private static Phase1StoreState State(
            int generation = 1)
        {
            return new Phase1StoreState(
                new SaveSlotId(0),
                "session-a",
                generation,
                2,
                1,
                1,
                0,
                0,
                100,
                new[]
                {
                    new Phase1OrderRecord(
                        "order-a",
                        "central-shelf",
                        true,
                        Phase1OrderState.Ordered,
                        1,
                        0,
                        100)
                },
                new Phase1StockRecord[0],
                new Phase1StockRecord[0],
                new Phase1PlacedFixtureRecord[0]);
        }
    }
}
