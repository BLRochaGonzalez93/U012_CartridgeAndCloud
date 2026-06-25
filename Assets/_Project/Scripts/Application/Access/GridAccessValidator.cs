using System;
using System.Collections.Generic;
using VRMGames.CartridgeAndCloud.Domain.Access;
using VRMGames.CartridgeAndCloud.Domain.Grid;

namespace VRMGames.CartridgeAndCloud.Application.Access
{
    public sealed class GridAccessValidator
    {
        private static readonly GridCoordinate[] NeighborOffsets =
        {
            new GridCoordinate(1, 0),
            new GridCoordinate(-1, 0),
            new GridCoordinate(0, 1),
            new GridCoordinate(0, -1)
        };

        public AccessValidationResult Validate(
            StoreAccessLayout layout,
            IEnumerable<GridCoordinate> blockedCells)
        {
            if (layout == null)
            {
                throw new ArgumentNullException(
                    nameof(layout));
            }

            if (blockedCells == null)
            {
                throw new ArgumentNullException(
                    nameof(blockedCells));
            }

            HashSet<GridCoordinate> blocked =
                new HashSet<GridCoordinate>();

            foreach (GridCoordinate cell in blockedCells)
            {
                if (!layout.Bounds.Contains(cell))
                {
                    return AccessValidationResult.Invalid(
                        AccessValidationFailureReason
                            .BlockedCellOutsideBounds);
                }

                blocked.Add(cell);
            }

            List<GridCoordinate> openEntranceCells =
                CollectOpenEntranceCells(
                    layout,
                    blocked);

            if (openEntranceCells.Count == 0)
            {
                return AccessValidationResult.Invalid(
                    AccessValidationFailureReason
                        .NoOpenEntrance);
            }

            int maximumOpenWidth =
                GetMaximumContiguousEntranceWidth(
                    openEntranceCells);

            if (maximumOpenWidth <
                layout.MinimumOpenEntranceWidthCells)
            {
                return AccessValidationResult.Invalid(
                    AccessValidationFailureReason
                        .EntranceWidthInsufficient);
            }

            foreach (AccessAnchor anchor in
                     layout.RequiredAnchors)
            {
                if (blocked.Contains(anchor.Cell))
                {
                    return AccessValidationResult.Invalid(
                        AccessValidationFailureReason
                            .RequiredAnchorBlocked,
                        anchor.Id);
                }
            }

            HashSet<GridCoordinate> reachable =
                Traverse(
                    layout.Bounds,
                    openEntranceCells,
                    blocked);

            foreach (AccessAnchor anchor in
                     layout.RequiredAnchors)
            {
                if (!reachable.Contains(anchor.Cell))
                {
                    return AccessValidationResult.Invalid(
                        AccessValidationFailureReason
                            .RequiredAnchorUnreachable,
                        anchor.Id,
                        reachable.Count);
                }
            }

            return AccessValidationResult.Valid(
                reachable.Count);
        }

        public AccessValidationResult ValidateWithCandidate(
            StoreAccessLayout layout,
            IEnumerable<GridCoordinate> existingBlockedCells,
            IEnumerable<GridCoordinate> candidateCells)
        {
            if (layout == null)
            {
                throw new ArgumentNullException(
                    nameof(layout));
            }

            if (existingBlockedCells == null)
            {
                throw new ArgumentNullException(
                    nameof(existingBlockedCells));
            }

            if (candidateCells == null)
            {
                throw new ArgumentNullException(
                    nameof(candidateCells));
            }

            HashSet<GridCoordinate> combined =
                new HashSet<GridCoordinate>();

            foreach (GridCoordinate cell in
                     existingBlockedCells)
            {
                combined.Add(cell);
            }

            foreach (GridCoordinate cell in candidateCells)
            {
                if (layout.IsEntranceCell(cell))
                {
                    return AccessValidationResult.Invalid(
                        AccessValidationFailureReason
                            .ReservedEntranceBlocked);
                }

                combined.Add(cell);
            }

            return Validate(
                layout,
                combined);
        }

        private static List<GridCoordinate>
            CollectOpenEntranceCells(
                StoreAccessLayout layout,
                HashSet<GridCoordinate> blocked)
        {
            List<GridCoordinate> open =
                new List<GridCoordinate>();

            foreach (GridCoordinate cell in
                     layout.EntranceCells)
            {
                if (!blocked.Contains(cell))
                {
                    open.Add(cell);
                }
            }

            return open;
        }

        private static int
            GetMaximumContiguousEntranceWidth(
                List<GridCoordinate> openEntranceCells)
        {
            HashSet<GridCoordinate> open =
                new HashSet<GridCoordinate>(
                    openEntranceCells);

            int maximumWidth = 0;

            foreach (GridCoordinate cell in
                     openEntranceCells)
            {
                GridCoordinate previous =
                    cell.Offset(-1, 0);

                if (open.Contains(previous))
                {
                    continue;
                }

                int width = 1;
                GridCoordinate next =
                    cell.Offset(1, 0);

                while (open.Contains(next))
                {
                    width++;
                    next = next.Offset(1, 0);
                }

                if (width > maximumWidth)
                {
                    maximumWidth = width;
                }
            }

            return maximumWidth;
        }

        private static HashSet<GridCoordinate> Traverse(
            GridBounds bounds,
            IEnumerable<GridCoordinate> startingCells,
            HashSet<GridCoordinate> blocked)
        {
            HashSet<GridCoordinate> visited =
                new HashSet<GridCoordinate>();

            Queue<GridCoordinate> frontier =
                new Queue<GridCoordinate>();

            foreach (GridCoordinate start in
                     startingCells)
            {
                if (visited.Add(start))
                {
                    frontier.Enqueue(start);
                }
            }

            while (frontier.Count > 0)
            {
                GridCoordinate current =
                    frontier.Dequeue();

                for (int index = 0;
                     index < NeighborOffsets.Length;
                     index++)
                {
                    GridCoordinate offset =
                        NeighborOffsets[index];

                    GridCoordinate neighbor =
                        current.Offset(
                            offset.X,
                            offset.Z);

                    if (!bounds.Contains(neighbor) ||
                        blocked.Contains(neighbor) ||
                        !visited.Add(neighbor))
                    {
                        continue;
                    }

                    frontier.Enqueue(neighbor);
                }
            }

            return visited;
        }
    }
}
