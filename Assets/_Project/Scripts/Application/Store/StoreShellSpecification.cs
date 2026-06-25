using System;

namespace VRMGames.CartridgeAndCloud.Application.Store
{
    public readonly struct StoreShellSpecification
    {
        public int WidthCells { get; }
        public int DepthCells { get; }
        public int EntranceWidthCells { get; }
        public float CellSize { get; }
        public float WallHeight { get; }
        public float WallThickness { get; }

        public float WidthMeters =>
            WidthCells * CellSize;

        public float DepthMeters =>
            DepthCells * CellSize;

        public float EntranceWidthMeters =>
            EntranceWidthCells * CellSize;

        public float FrontWallSegmentWidthMeters =>
            (WidthMeters - EntranceWidthMeters) * 0.5f;

        public StoreShellSpecification(
            int widthCells,
            int depthCells,
            int entranceWidthCells,
            float cellSize,
            float wallHeight,
            float wallThickness)
        {
            if (widthCells <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(widthCells));
            }

            if (depthCells <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(depthCells));
            }

            if (entranceWidthCells <= 0 ||
                entranceWidthCells >= widthCells)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(entranceWidthCells));
            }

            ValidatePositiveFinite(
                cellSize,
                nameof(cellSize));

            ValidatePositiveFinite(
                wallHeight,
                nameof(wallHeight));

            ValidatePositiveFinite(
                wallThickness,
                nameof(wallThickness));

            if (wallThickness >= MetersFrom(
                    widthCells,
                    cellSize) ||
                wallThickness >= MetersFrom(
                    depthCells,
                    cellSize))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(wallThickness));
            }

            WidthCells = widthCells;
            DepthCells = depthCells;
            EntranceWidthCells = entranceWidthCells;
            CellSize = cellSize;
            WallHeight = wallHeight;
            WallThickness = wallThickness;
        }

        public static StoreShellSpecification InitialTier()
        {
            return new StoreShellSpecification(
                widthCells: 20,
                depthCells: 30,
                entranceWidthCells: 4,
                cellSize: 0.5f,
                wallHeight: 3f,
                wallThickness: 0.2f);
        }

        private static void ValidatePositiveFinite(
            float value,
            string parameterName)
        {
            if (value <= 0f ||
                float.IsNaN(value) ||
                float.IsInfinity(value))
            {
                throw new ArgumentOutOfRangeException(
                    parameterName);
            }
        }

        private static float MetersFrom(
            int cells,
            float cellSize)
        {
            return cells * cellSize;
        }
    }
}
