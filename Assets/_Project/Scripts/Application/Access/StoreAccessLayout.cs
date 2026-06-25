using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VRMGames.CartridgeAndCloud.Domain.Access;
using VRMGames.CartridgeAndCloud.Domain.Grid;

namespace VRMGames.CartridgeAndCloud.Application.Access
{
    public sealed class StoreAccessLayout
    {
        private readonly ReadOnlyCollection<GridCoordinate>
            _entranceCells;

        private readonly ReadOnlyCollection<AccessAnchor>
            _requiredAnchors;

        public GridBounds Bounds { get; }

        public IReadOnlyList<GridCoordinate> EntranceCells =>
            _entranceCells;

        public IReadOnlyList<AccessAnchor> RequiredAnchors =>
            _requiredAnchors;

        public int MinimumOpenEntranceWidthCells { get; }

        public StoreAccessLayout(
            GridBounds bounds,
            IEnumerable<GridCoordinate> entranceCells,
            IEnumerable<AccessAnchor> requiredAnchors,
            int minimumOpenEntranceWidthCells)
        {
            if (entranceCells == null)
            {
                throw new ArgumentNullException(
                    nameof(entranceCells));
            }

            if (requiredAnchors == null)
            {
                throw new ArgumentNullException(
                    nameof(requiredAnchors));
            }

            if (minimumOpenEntranceWidthCells <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(minimumOpenEntranceWidthCells));
            }

            List<GridCoordinate> entranceList =
                new List<GridCoordinate>(
                    entranceCells);

            if (entranceList.Count <
                minimumOpenEntranceWidthCells)
            {
                throw new ArgumentException(
                    "Entrance cells cannot be fewer than " +
                    "the minimum open entrance width.",
                    nameof(entranceCells));
            }

            HashSet<GridCoordinate> uniqueEntranceCells =
                new HashSet<GridCoordinate>();

            foreach (GridCoordinate cell in entranceList)
            {
                if (!bounds.Contains(cell))
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(entranceCells),
                        $"Entrance cell {cell} is outside bounds.");
                }

                if (!uniqueEntranceCells.Add(cell))
                {
                    throw new ArgumentException(
                        $"Entrance cell {cell} is duplicated.",
                        nameof(entranceCells));
                }
            }

            List<AccessAnchor> anchorList =
                new List<AccessAnchor>(
                    requiredAnchors);

            HashSet<AccessAnchorId> uniqueAnchorIds =
                new HashSet<AccessAnchorId>();

            foreach (AccessAnchor anchor in anchorList)
            {
                if (anchor == null)
                {
                    throw new ArgumentException(
                        "Required anchors cannot contain null.",
                        nameof(requiredAnchors));
                }

                if (!bounds.Contains(anchor.Cell))
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(requiredAnchors),
                        $"Required anchor {anchor.Id} is outside bounds.");
                }

                if (!uniqueAnchorIds.Add(anchor.Id))
                {
                    throw new ArgumentException(
                        $"Required anchor ID {anchor.Id} is duplicated.",
                        nameof(requiredAnchors));
                }
            }

            Bounds = bounds;
            MinimumOpenEntranceWidthCells =
                minimumOpenEntranceWidthCells;

            _entranceCells =
                new ReadOnlyCollection<GridCoordinate>(
                    entranceList);

            _requiredAnchors =
                new ReadOnlyCollection<AccessAnchor>(
                    anchorList);
        }

        public bool IsEntranceCell(
            GridCoordinate cell)
        {
            for (int index = 0;
                 index < _entranceCells.Count;
                 index++)
            {
                if (_entranceCells[index] == cell)
                {
                    return true;
                }
            }

            return false;
        }

        public static StoreAccessLayout InitialTier()
        {
            GridBounds bounds =
                new GridBounds(
                    new GridCoordinate(0, 0),
                    new GridSize(20, 30));

            GridCoordinate[] entranceCells =
            {
                new GridCoordinate(8, 0),
                new GridCoordinate(9, 0),
                new GridCoordinate(10, 0),
                new GridCoordinate(11, 0)
            };

            AccessAnchor[] requiredAnchors =
            {
                new AccessAnchor(
                    new AccessAnchorId(
                        "rear-service"),
                    new GridCoordinate(10, 27)),
                new AccessAnchor(
                    new AccessAnchorId(
                        "left-display"),
                    new GridCoordinate(3, 15)),
                new AccessAnchor(
                    new AccessAnchorId(
                        "right-display"),
                    new GridCoordinate(16, 15))
            };

            return new StoreAccessLayout(
                bounds,
                entranceCells,
                requiredAnchors,
                minimumOpenEntranceWidthCells: 2);
        }
    }
}
