using System;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.Access;
using VRMGames.CartridgeAndCloud.Domain.Access;
using VRMGames.CartridgeAndCloud.Domain.Grid;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode
{
    public sealed class StoreAccessModelTests
    {
        [Test]
        public void InitialTier_UsesTwentyByThirtyBounds()
        {
            StoreAccessLayout layout =
                StoreAccessLayout.InitialTier();

            Assert.That(
                layout.Bounds.Minimum,
                Is.EqualTo(new GridCoordinate(0, 0)));

            Assert.That(
                layout.Bounds.Size,
                Is.EqualTo(new GridSize(20, 30)));
        }

        [Test]
        public void InitialTier_UsesFourCellEntranceAndTwoCellMinimum()
        {
            StoreAccessLayout layout =
                StoreAccessLayout.InitialTier();

            Assert.That(
                layout.EntranceCells.Count,
                Is.EqualTo(4));

            Assert.That(
                layout.MinimumOpenEntranceWidthCells,
                Is.EqualTo(2));

            for (int index = 0;
                 index < layout.EntranceCells.Count;
                 index++)
            {
                Assert.That(
                    layout.EntranceCells[index],
                    Is.EqualTo(
                        new GridCoordinate(8 + index, 0)));
            }
        }

        [Test]
        public void InitialTier_ReservesWarehouseThreshold()
        {
            StoreAccessLayout layout =
                StoreAccessLayout.InitialTier();

            Assert.That(
                layout.RequiredAnchors.Count,
                Is.EqualTo(8));

            for (int index = 0;
                 index < 4;
                 index++)
            {
                Assert.That(
                    layout.RequiredAnchors[index].Id.Value,
                    Is.EqualTo(
                        $"warehouse-entry-front-{index + 1:00}"));

                Assert.That(
                    layout.RequiredAnchors[index].Cell,
                    Is.EqualTo(
                        new GridCoordinate(8 + index, 23)));

                Assert.That(
                    layout.RequiredAnchors[index + 4].Id.Value,
                    Is.EqualTo(
                        $"warehouse-entry-rear-{index + 1:00}"));

                Assert.That(
                    layout.RequiredAnchors[index + 4].Cell,
                    Is.EqualTo(
                        new GridCoordinate(8 + index, 24)));
            }
        }

        [Test]
        public void Constructor_RejectsInvalidEntranceCells()
        {
            GridBounds bounds =
                CreateBounds();

            Assert.Throws<ArgumentException>(
                () => new StoreAccessLayout(
                    bounds,
                    new[]
                    {
                        new GridCoordinate(1, 0)
                    },
                    CreateSingleAnchor(),
                    minimumOpenEntranceWidthCells: 2));

            Assert.Throws<ArgumentException>(
                () => new StoreAccessLayout(
                    bounds,
                    new[]
                    {
                        new GridCoordinate(1, 0),
                        new GridCoordinate(1, 0)
                    },
                    CreateSingleAnchor(),
                    minimumOpenEntranceWidthCells: 2));

            Assert.Throws<ArgumentOutOfRangeException>(
                () => new StoreAccessLayout(
                    bounds,
                    new[]
                    {
                        new GridCoordinate(1, 0),
                        new GridCoordinate(9, 0)
                    },
                    CreateSingleAnchor(),
                    minimumOpenEntranceWidthCells: 2));
        }

        [Test]
        public void Constructor_RejectsInvalidRequiredAnchors()
        {
            GridBounds bounds =
                CreateBounds();

            Assert.Throws<ArgumentException>(
                () => new StoreAccessLayout(
                    bounds,
                    CreateEntrance(),
                    new AccessAnchor[]
                    {
                        null
                    },
                    minimumOpenEntranceWidthCells: 2));

            AccessAnchor duplicateA =
                new AccessAnchor(
                    new AccessAnchorId("duplicate"),
                    new GridCoordinate(2, 3));

            AccessAnchor duplicateB =
                new AccessAnchor(
                    new AccessAnchorId("duplicate"),
                    new GridCoordinate(3, 3));

            Assert.Throws<ArgumentException>(
                () => new StoreAccessLayout(
                    bounds,
                    CreateEntrance(),
                    new[]
                    {
                        duplicateA,
                        duplicateB
                    },
                    minimumOpenEntranceWidthCells: 2));

            Assert.Throws<ArgumentOutOfRangeException>(
                () => new StoreAccessLayout(
                    bounds,
                    CreateEntrance(),
                    new[]
                    {
                        new AccessAnchor(
                            new AccessAnchorId("outside"),
                            new GridCoordinate(12, 12))
                    },
                    minimumOpenEntranceWidthCells: 2));
        }

        [Test]
        public void AccessAnchorId_RejectsEmptyValue()
        {
            Assert.Throws<ArgumentException>(
                () => new AccessAnchorId(string.Empty));

            Assert.Throws<ArgumentException>(
                () => new AccessAnchorId("   "));
        }

        [Test]
        public void AccessAnchorId_UsesOrdinalEquality()
        {
            AccessAnchorId lower =
                new AccessAnchorId("rear-service");

            AccessAnchorId same =
                new AccessAnchorId("rear-service");

            AccessAnchorId upper =
                new AccessAnchorId("REAR-SERVICE");

            Assert.That(lower, Is.EqualTo(same));
            Assert.That(lower, Is.Not.EqualTo(upper));
        }

        [Test]
        public void AccessAnchor_RejectsDefaultId()
        {
            Assert.Throws<ArgumentException>(
                () => new AccessAnchor(
                    default,
                    new GridCoordinate(2, 3)));
        }

        [Test]
        public void AccessAnchor_StoresIdAndCell()
        {
            AccessAnchorId id =
                new AccessAnchorId("checkout");

            GridCoordinate cell =
                new GridCoordinate(4, 9);

            AccessAnchor anchor =
                new AccessAnchor(id, cell);

            Assert.That(anchor.Id, Is.EqualTo(id));
            Assert.That(anchor.Cell, Is.EqualTo(cell));
        }

        private static GridBounds CreateBounds()
        {
            return new GridBounds(
                new GridCoordinate(0, 0),
                new GridSize(6, 6));
        }

        private static GridCoordinate[] CreateEntrance()
        {
            return new[]
            {
                new GridCoordinate(2, 0),
                new GridCoordinate(3, 0)
            };
        }

        private static AccessAnchor[] CreateSingleAnchor()
        {
            return new[]
            {
                new AccessAnchor(
                    new AccessAnchorId("target"),
                    new GridCoordinate(3, 5))
            };
        }
    }
}
