using System;
using VRMGames.CartridgeAndCloud.Domain.Grid;

namespace VRMGames.CartridgeAndCloud.Application.Grid
{
    public static class GridProjectionCalculator
    {
        public const float DefaultCellSize = 0.5f;

        public static GridCoordinate WorldToCell(
            float worldX,
            float worldZ,
            float originX = 0f,
            float originZ = 0f,
            float cellSize = DefaultCellSize)
        {
            ValidateFinite(worldX, nameof(worldX));
            ValidateFinite(worldZ, nameof(worldZ));
            ValidateFinite(originX, nameof(originX));
            ValidateFinite(originZ, nameof(originZ));
            ValidateCellSize(cellSize);

            double scaledX =
                (worldX - originX) / cellSize;

            double scaledZ =
                (worldZ - originZ) / cellSize;

            double flooredX =
                Math.Floor(scaledX);

            double flooredZ =
                Math.Floor(scaledZ);

            if (flooredX < int.MinValue ||
                flooredX > int.MaxValue)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(worldX),
                    worldX,
                    "World X is outside the supported grid range.");
            }

            if (flooredZ < int.MinValue ||
                flooredZ > int.MaxValue)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(worldZ),
                    worldZ,
                    "World Z is outside the supported grid range.");
            }

            return new GridCoordinate(
                (int)flooredX,
                (int)flooredZ);
        }

        public static GridWorldPosition CellToWorldCenter(
            GridCoordinate cell,
            float originX = 0f,
            float originZ = 0f,
            float cellSize = DefaultCellSize)
        {
            ValidateFinite(originX, nameof(originX));
            ValidateFinite(originZ, nameof(originZ));
            ValidateCellSize(cellSize);

            double worldX =
                originX +
                (cell.X + 0.5d) *
                cellSize;

            double worldZ =
                originZ +
                (cell.Z + 0.5d) *
                cellSize;

            return new GridWorldPosition(
                (float)worldX,
                (float)worldZ);
        }

        private static void ValidateCellSize(
            float cellSize)
        {
            if (cellSize <= 0f ||
                float.IsNaN(cellSize) ||
                float.IsInfinity(cellSize))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(cellSize),
                    cellSize,
                    "Cell size must be finite and greater than zero.");
            }
        }

        private static void ValidateFinite(
            float value,
            string parameterName)
        {
            if (float.IsNaN(value) ||
                float.IsInfinity(value))
            {
                throw new ArgumentOutOfRangeException(
                    parameterName,
                    value,
                    "Value must be finite.");
            }
        }
    }
}
