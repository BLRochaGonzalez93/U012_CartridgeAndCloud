using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.Placement;
using VRMGames.CartridgeAndCloud.Domain.Grid;
using VRMGames.CartridgeAndCloud.Domain.Placement;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode
{
    public sealed class PlacementOccupancySnapshotTests
    {
        [Test]
        public void GetOccupiedCells_EmptyRegistry_ReturnsEmptyArray()
        {
            PlacementOccupancyRegistry registry =
                CreateRegistry();

            Assert.That(
                registry.GetOccupiedCells(),
                Is.Empty);
        }

        [Test]
        public void GetOccupiedCells_AfterPlacement_ReturnsAllCells()
        {
            PlacementOccupancyRegistry registry =
                CreateRegistry();

            PlacedObjectRecord record =
                new PlacedObjectRecord(
                    new PlacementInstanceId("shelf-1"),
                    "technical-shelf",
                    new GridCoordinate(2, 3),
                    GridRotation.Degrees0,
                    new GridSize(2, 2));

            Assert.That(
                registry.TryPlace(record).IsValid,
                Is.True);

            CollectionAssert.AreEquivalent(
                record.GetOccupiedCells(),
                registry.GetOccupiedCells());
        }

        [Test]
        public void GetOccupiedCells_ReturnedArrayDoesNotMutateRegistry()
        {
            PlacementOccupancyRegistry registry =
                CreateRegistry();

            PlacedObjectRecord record =
                new PlacedObjectRecord(
                    new PlacementInstanceId("shelf-1"),
                    "technical-shelf",
                    new GridCoordinate(2, 3),
                    GridRotation.Degrees0,
                    new GridSize(2, 2));

            Assert.That(
                registry.TryPlace(record).IsValid,
                Is.True);

            GridCoordinate[] snapshot =
                registry.GetOccupiedCells();

            GridCoordinate original =
                snapshot[0];

            snapshot[0] =
                new GridCoordinate(99, 99);

            Assert.That(
                registry.IsOccupied(original),
                Is.True);

            Assert.That(
                registry.IsOccupied(
                    new GridCoordinate(99, 99)),
                Is.False);
        }

        private static PlacementOccupancyRegistry
            CreateRegistry()
        {
            return new PlacementOccupancyRegistry(
                new GridBounds(
                    new GridCoordinate(0, 0),
                    new GridSize(10, 10)));
        }
    }
}
