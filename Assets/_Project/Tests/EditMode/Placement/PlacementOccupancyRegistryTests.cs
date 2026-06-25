using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.Placement;
using VRMGames.CartridgeAndCloud.Domain.Grid;
using VRMGames.CartridgeAndCloud.Domain.Placement;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode
{
    public sealed class PlacementOccupancyRegistryTests
    {
        private static PlacementOccupancyRegistry
            CreateRegistry()
        {
            return new PlacementOccupancyRegistry(
                new GridBounds(
                    new GridCoordinate(0, 0),
                    new GridSize(10, 10)));
        }

        private static PlacedObjectRecord CreateRecord(
            string id,
            int x,
            int z,
            GridRotation rotation =
                GridRotation.Degrees0)
        {
            return new PlacedObjectRecord(
                new PlacementInstanceId(id),
                "technical-shelf-4x2",
                new GridCoordinate(x, z),
                rotation,
                new GridSize(4, 2));
        }

        [Test]
        public void Validate_EmptyInsideBounds_IsValid()
        {
            PlacementOccupancyRegistry registry =
                CreateRegistry();

            PlacementValidationResult result =
                registry.Validate(
                    new GridCoordinate(2, 3),
                    new GridSize(4, 2),
                    GridRotation.Degrees0);

            Assert.That(result.IsValid, Is.True);
            Assert.That(
                result.FailureReason,
                Is.EqualTo(
                    PlacementFailureReason.None));
        }

        [Test]
        public void Validate_OutsideBounds_ReturnsOutOfBounds()
        {
            PlacementOccupancyRegistry registry =
                CreateRegistry();

            PlacementValidationResult result =
                registry.Validate(
                    new GridCoordinate(8, 9),
                    new GridSize(4, 2),
                    GridRotation.Degrees0);

            Assert.That(result.IsValid, Is.False);
            Assert.That(
                result.FailureReason,
                Is.EqualTo(
                    PlacementFailureReason.OutOfBounds));
        }

        [Test]
        public void TryPlace_ValidRecord_OccupiesAllCells()
        {
            PlacementOccupancyRegistry registry =
                CreateRegistry();

            PlacementValidationResult result =
                registry.TryPlace(
                    CreateRecord(
                        "shelf-1",
                        1,
                        2));

            Assert.That(result.IsValid, Is.True);
            Assert.That(registry.Count, Is.EqualTo(1));
            Assert.That(
                registry.OccupiedCellCount,
                Is.EqualTo(8));

            Assert.That(
                registry.IsOccupied(
                    new GridCoordinate(1, 2)),
                Is.True);

            Assert.That(
                registry.IsOccupied(
                    new GridCoordinate(4, 3)),
                Is.True);
        }

        [Test]
        public void TryPlace_Overlap_IsRejectedAtomically()
        {
            PlacementOccupancyRegistry registry =
                CreateRegistry();

            registry.TryPlace(
                CreateRecord(
                    "shelf-1",
                    1,
                    1));

            PlacementValidationResult result =
                registry.TryPlace(
                    CreateRecord(
                        "shelf-2",
                        3,
                        2));

            Assert.That(result.IsValid, Is.False);
            Assert.That(
                result.FailureReason,
                Is.EqualTo(
                    PlacementFailureReason.Overlap));

            Assert.That(registry.Count, Is.EqualTo(1));
            Assert.That(
                registry.OccupiedCellCount,
                Is.EqualTo(8));
        }

        [Test]
        public void TryPlace_DuplicateId_IsRejected()
        {
            PlacementOccupancyRegistry registry =
                CreateRegistry();

            registry.TryPlace(
                CreateRecord(
                    "shelf-1",
                    0,
                    0));

            PlacementValidationResult result =
                registry.TryPlace(
                    CreateRecord(
                        "shelf-1",
                        5,
                        5));

            Assert.That(result.IsValid, Is.False);
            Assert.That(
                result.FailureReason,
                Is.EqualTo(
                    PlacementFailureReason.DuplicateId));

            Assert.That(registry.Count, Is.EqualTo(1));
        }

        [Test]
        public void TryRemove_ExistingRecord_FreesEveryCell()
        {
            PlacementOccupancyRegistry registry =
                CreateRegistry();

            PlacedObjectRecord record =
                CreateRecord(
                    "shelf-1",
                    1,
                    1);

            registry.TryPlace(record);

            bool removed =
                registry.TryRemove(
                    record.Id,
                    out PlacedObjectRecord removedRecord);

            Assert.That(removed, Is.True);
            Assert.That(
                removedRecord,
                Is.SameAs(record));

            Assert.That(registry.Count, Is.Zero);
            Assert.That(
                registry.OccupiedCellCount,
                Is.Zero);

            Assert.That(
                registry.Validate(
                    new GridCoordinate(1, 1),
                    new GridSize(4, 2),
                    GridRotation.Degrees0)
                    .IsValid,
                Is.True);
        }

        [Test]
        public void TryRemove_UnknownId_ReturnsFalse()
        {
            PlacementOccupancyRegistry registry =
                CreateRegistry();

            bool removed =
                registry.TryRemove(
                    new PlacementInstanceId(
                        "missing"),
                    out PlacedObjectRecord record);

            Assert.That(removed, Is.False);
            Assert.That(record, Is.Null);
        }

        [Test]
        public void TryPlace_RotatedRecord_UsesOrientedCells()
        {
            PlacementOccupancyRegistry registry =
                CreateRegistry();

            PlacedObjectRecord record =
                CreateRecord(
                    "shelf-1",
                    2,
                    2,
                    GridRotation.Degrees90);

            PlacementValidationResult result =
                registry.TryPlace(record);

            Assert.That(result.IsValid, Is.True);

            Assert.That(
                registry.IsOccupied(
                    new GridCoordinate(3, 5)),
                Is.True);

            Assert.That(
                registry.IsOccupied(
                    new GridCoordinate(4, 2)),
                Is.False);
        }

        [Test]
        public void TryGetOccupant_ReturnsOwningId()
        {
            PlacementOccupancyRegistry registry =
                CreateRegistry();

            PlacedObjectRecord record =
                CreateRecord(
                    "shelf-1",
                    0,
                    0);

            registry.TryPlace(record);

            bool found =
                registry.TryGetOccupant(
                    new GridCoordinate(2, 1),
                    out PlacementInstanceId id);

            Assert.That(found, Is.True);
            Assert.That(id, Is.EqualTo(record.Id));
        }
    }
}
