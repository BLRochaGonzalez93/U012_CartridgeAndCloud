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

    public readonly struct PlacementInstanceId :
        IEquatable<PlacementInstanceId>
    {
        public string Value { get; }

        public PlacementInstanceId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "Placement ID cannot be empty.",
                    nameof(value));
            }

            Value = value;
        }

        public bool Equals(PlacementInstanceId other)
        {
            return string.Equals(
                Value,
                other.Value,
                StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is PlacementInstanceId other &&
                   Equals(other);
        }

        public override int GetHashCode()
        {
            return StringComparer.Ordinal.GetHashCode(Value);
        }

        public override string ToString()
        {
            return Value;
        }

        public static bool operator ==(
            PlacementInstanceId left,
            PlacementInstanceId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(
            PlacementInstanceId left,
            PlacementInstanceId right)
        {
            return !left.Equals(right);
        }
    }

    public enum PlacementFailureReason
    {
        None = 0,
        OutOfBounds = 1,
        Overlap = 2,
        DuplicateId = 3,
        NotFound = 4,
        AccessBlocked = 5
    }
}
