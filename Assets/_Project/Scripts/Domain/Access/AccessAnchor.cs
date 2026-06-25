using System;
using VRMGames.CartridgeAndCloud.Domain.Grid;

namespace VRMGames.CartridgeAndCloud.Domain.Access
{
    public sealed class AccessAnchor
    {
        public AccessAnchorId Id { get; }
        public GridCoordinate Cell { get; }

        public AccessAnchor(
            AccessAnchorId id,
            GridCoordinate cell)
        {
            if (string.IsNullOrWhiteSpace(
                    id.Value))
            {
                throw new ArgumentException(
                    "Access anchor ID cannot be empty.",
                    nameof(id));
            }

            Id = id;
            Cell = cell;
        }
    }
}
