namespace VRMGames.CartridgeAndCloud.Domain.Grid
{
    public sealed class GridFootprint
    {
        public GridSize BaseSize { get; }

        public GridFootprint(GridSize baseSize)
        {
            BaseSize = baseSize;
        }

        public GridSize GetOrientedSize(
            GridRotation rotation)
        {
            return BaseSize.GetOriented(rotation);
        }

        public GridCoordinate[] GetOccupiedCells(
            GridCoordinate anchor,
            GridRotation rotation)
        {
            GridSize orientedSize =
                GetOrientedSize(rotation);

            GridCoordinate[] cells =
                new GridCoordinate[
                    orientedSize.Area];

            int index = 0;

            for (int z = 0;
                 z < orientedSize.Depth;
                 z++)
            {
                for (int x = 0;
                     x < orientedSize.Width;
                     x++)
                {
                    cells[index] =
                        anchor.Offset(x, z);

                    index++;
                }
            }

            return cells;
        }

        public bool ContainsCell(
            GridCoordinate anchor,
            GridRotation rotation,
            GridCoordinate cell)
        {
            GridSize orientedSize =
                GetOrientedSize(rotation);

            long offsetX =
                (long)cell.X - anchor.X;

            long offsetZ =
                (long)cell.Z - anchor.Z;

            return offsetX >= 0 &&
                   offsetZ >= 0 &&
                   offsetX < orientedSize.Width &&
                   offsetZ < orientedSize.Depth;
        }
    }
}
