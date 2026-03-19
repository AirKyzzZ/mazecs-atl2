namespace Epsi.MazeCs;

public class MazeGen(Vec2d size, double coinProbability = 0, double doorProbability = 0) : IMazeGenerator
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

    public Cell[,] Generate()
    {
        var grid = new Cell[Size.X, Size.Y];
        var rng = new Random();

        for (var y = 0; y < Size.Y; y++)
            for (var x = 0; x < Size.X; x++)
                grid[x, y] = new Wall();

        var carvedRooms = new List<Room>();
        Carve(grid, 0, 0, rng, carvedRooms);

        grid[0, 0] = new Room();
        grid[(Size.X - 1) & ~1, (Size.Y - 1) & ~1] = new Room(isExit: true);

        return grid;
    }

    private void Carve(Cell[,] grid, int x, int y, Random rng, List<Room> carvedRooms)
    {
        var room = new Room();
        if (coinProbability > 0 && rng.NextDouble() < coinProbability)
            room.Item = new Coin();
        grid[x, y] = room;
        carvedRooms.Add(room);

        var reachableCount = carvedRooms.Count;

        foreach (var dir in Orders[rng.Next(Orders.Length)])
        {
            var nx = x + Dx[dir] * 2;
            var ny = y + Dy[dir] * 2;

            if (nx >= 0 && nx < Size.X && ny >= 0 && ny < Size.Y && !grid[nx, ny].IsWalkable)
            {
                if (doorProbability > 0 && rng.NextDouble() < doorProbability)
                {
                    var door = new Door();
                    var emptyRoom = FindEmptyRoom(carvedRooms, rng, reachableCount);
                    if (emptyRoom is not null)
                    {
                        grid[(x + nx) / 2, (y + ny) / 2] = door;
                        emptyRoom.Item = door.RequiredKey;
                    }
                    else
                    {
                        grid[(x + nx) / 2, (y + ny) / 2] = new Room();
                    }
                }
                else
                {
                    grid[(x + nx) / 2, (y + ny) / 2] = new Room();
                }

                Carve(grid, nx, ny, rng, carvedRooms);
            }
        }
    }

    private static Room? FindEmptyRoom(List<Room> rooms, Random rng, int limit)
    {
        var candidates = rooms.GetRange(0, limit).FindAll(r => r.Item is null && !r.IsExit);
        return candidates.Count > 0 ? candidates[rng.Next(candidates.Count)] : null;
    }
}
