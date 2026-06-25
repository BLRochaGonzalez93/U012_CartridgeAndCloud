using System;
using System.Collections.Generic;
using VRMGames.CartridgeAndCloud.Domain.Grid;

namespace VRMGames.CartridgeAndCloud.Application.Access
{
    public sealed class StorePlacementAccessValidator
    {
        private readonly GridAccessValidator
            _gridAccessValidator;

        public StoreAccessLayout Layout { get; }

        public StorePlacementAccessValidator(
            StoreAccessLayout layout)
        {
            Layout =
                layout ??
                throw new ArgumentNullException(
                    nameof(layout));

            _gridAccessValidator =
                new GridAccessValidator();
        }

        public AccessValidationResult ValidateCandidate(
            IEnumerable<GridCoordinate> existingBlockedCells,
            GridCoordinate anchor,
            GridSize baseSize,
            GridRotation rotation)
        {
            if (existingBlockedCells == null)
            {
                throw new ArgumentNullException(
                    nameof(existingBlockedCells));
            }

            GridFootprint footprint =
                new GridFootprint(baseSize);

            GridCoordinate[] candidateCells =
                footprint.GetOccupiedCells(
                    anchor,
                    rotation);

            return _gridAccessValidator
                .ValidateWithCandidate(
                    Layout,
                    existingBlockedCells,
                    candidateCells);
        }
    }
}
