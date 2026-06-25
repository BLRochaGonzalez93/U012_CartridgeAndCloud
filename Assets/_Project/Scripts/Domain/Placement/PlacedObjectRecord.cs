using System;
using VRMGames.CartridgeAndCloud.Domain.Grid;

namespace VRMGames.CartridgeAndCloud.Domain.Placement
{
    public sealed class PlacedObjectRecord
    {
        public PlacementInstanceId Id { get; }
        public string DefinitionId { get; }
        public GridCoordinate Anchor { get; }
        public GridRotation Rotation { get; }
        public GridSize BaseSize { get; }

        public PlacedObjectRecord(
            PlacementInstanceId id,
            string definitionId,
            GridCoordinate anchor,
            GridRotation rotation,
            GridSize baseSize)
        {
            if (string.IsNullOrWhiteSpace(definitionId))
            {
                throw new ArgumentException(
                    "Definition ID cannot be empty.",
                    nameof(definitionId));
            }

            GridRotationExtensions.Validate(rotation);

            Id = id;
            DefinitionId = definitionId;
            Anchor = anchor;
            Rotation = rotation;
            BaseSize = baseSize;
        }

        public GridFootprint CreateFootprint()
        {
            return new GridFootprint(BaseSize);
        }

        public GridCoordinate[] GetOccupiedCells()
        {
            return CreateFootprint().GetOccupiedCells(
                Anchor,
                Rotation);
        }
    }
}
