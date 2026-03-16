namespace Epsi.MazeCs;

public class MazeGen(Vec2d size) : IMazeGenerator
{
    private static readonly int[][] Orders = [
        [0, 1, 2, 3], [0, 1, 3, 2], [0, 2, 1, 3], [0, 2, 3, 1], [0, 3, 1, 2], [0, 3, 2, 1],
        [1, 0, 2, 3], [1, 0, 3, 2], [1, 2, 0, 3], [1, 2, 3, 0], [1, 3, 0, 2], [1, 3, 2, 0],
        [2, 0, 1, 3], [2, 0, 3, 1], [2, 1, 0, 3], [2, 1, 3, 0], [2, 3, 0, 1], [2, 3, 1, 0],
        [3, 0, 1, 2], [3, 0, 2, 1], [3, 1, 0, 2], [3, 1, 2, 0], [3, 2, 0, 1], [3, 2, 1, 0]
    ];

    private static readonly int[] Dx = [0, 1, 0, -1];
    private static readonly int[] Dy = [-1, 0, 1, 0];

    public Vec2d Size { get; } = size;

    public CellType[,] Generate()
    {
        var grid = new CellType[Size.X, Size.Y];
        var rng = new Random();

        for (var y = 0; y < Size.Y; y++)
            for (var x = 0; x < Size.X; x++)
                grid[x, y] = CellType.Wall;

        Carve(grid, 0, 0, rng);

        grid[0, 0] = CellType.Corridor;
        grid[(Size.X - 1) & ~1, (Size.Y - 1) & ~1] = CellType.Exit;

        return grid;
    }

    private void Carve(CellType[,] grid, int x, int y, Random rng)
    {
        grid[x, y] = CellType.Corridor;
        foreach (var dir in Orders[rng.Next(Orders.Length)])
        {
            var nx = x + Dx[dir] * 2;
            var ny = y + Dy[dir] * 2;

            if (nx >= 0 && nx < Size.X && ny >= 0 && ny < Size.Y && grid[nx, ny] == CellType.Wall)
            {
                grid[(x + nx) / 2, (y + ny) / 2] = CellType.Corridor;
                Carve(grid, nx, ny, rng);
            }
        }
    }
}
