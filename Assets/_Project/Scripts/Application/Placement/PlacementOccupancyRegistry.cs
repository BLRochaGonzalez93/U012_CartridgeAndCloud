using System;
using System.Collections.Generic;
using VRMGames.CartridgeAndCloud.Domain.Grid;
using VRMGames.CartridgeAndCloud.Domain.Placement;

namespace VRMGames.CartridgeAndCloud.Application.Placement
{
    public sealed class PlacementOccupancyRegistry
    {
        private readonly Dictionary<
            GridCoordinate,
            PlacementInstanceId> _occupantsByCell =
                new Dictionary<
                    GridCoordinate,
                    PlacementInstanceId>();

        private readonly Dictionary<
            PlacementInstanceId,
            PlacedObjectRecord> _recordsById =
                new Dictionary<
                    PlacementInstanceId,
                    PlacedObjectRecord>();

        public GridBounds Bounds { get; }
        public int Count => _recordsById.Count;
        public int OccupiedCellCount => _occupantsByCell.Count;

        public PlacementOccupancyRegistry(GridBounds bounds)
        {
            Bounds = bounds;
        }

        public PlacementValidationResult Validate(
            GridCoordinate anchor,
            GridSize baseSize,
            GridRotation rotation)
        {
            GridFootprint footprint =
                new GridFootprint(baseSize);

            if (!Bounds.ContainsFootprint(
                    anchor,
                    footprint,
                    rotation))
            {
                return PlacementValidationResult.Invalid(
                    PlacementFailureReason.OutOfBounds);
            }

            GridCoordinate[] cells =
                footprint.GetOccupiedCells(
                    anchor,
                    rotation);

            foreach (GridCoordinate cell in cells)
            {
                if (_occupantsByCell.ContainsKey(cell))
                {
                    return PlacementValidationResult.Invalid(
                        PlacementFailureReason.Overlap);
                }
            }

            return PlacementValidationResult.Valid();
        }

        public PlacementValidationResult TryPlace(
            PlacedObjectRecord record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            if (_recordsById.ContainsKey(record.Id))
            {
                return PlacementValidationResult.Invalid(
                    PlacementFailureReason.DuplicateId);
            }

            PlacementValidationResult validation =
                Validate(
                    record.Anchor,
                    record.BaseSize,
                    record.Rotation);

            if (!validation.IsValid)
            {
                return validation;
            }

            GridCoordinate[] cells =
                record.GetOccupiedCells();

            _recordsById.Add(record.Id, record);

            foreach (GridCoordinate cell in cells)
            {
                _occupantsByCell.Add(cell, record.Id);
            }

            return PlacementValidationResult.Valid();
        }

        public bool TryRemove(
            PlacementInstanceId id,
            out PlacedObjectRecord removedRecord)
        {
            if (!_recordsById.TryGetValue(
                    id,
                    out removedRecord))
            {
                removedRecord = null;
                return false;
            }

            foreach (GridCoordinate cell in
                     removedRecord.GetOccupiedCells())
            {
                _occupantsByCell.Remove(cell);
            }

            _recordsById.Remove(id);
            return true;
        }

        public bool IsOccupied(GridCoordinate cell)
        {
            return _occupantsByCell.ContainsKey(cell);
        }

        public bool TryGetOccupant(
            GridCoordinate cell,
            out PlacementInstanceId id)
        {
            return _occupantsByCell.TryGetValue(
                cell,
                out id);
        }

        public bool TryGetRecord(
            PlacementInstanceId id,
            out PlacedObjectRecord record)
        {
            return _recordsById.TryGetValue(
                id,
                out record);
        }
    }
}
