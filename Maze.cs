namespace Epsi.MazeCs;

public class Maze
{
    private readonly CellType[,] _grid;

    public Vec2d Size { get; }
    public Vec2d Exit { get; }

    public Maze(IMazeGenerator generator)
    {
        _grid = generator.Generate();
        Size = generator.Size;
        Exit = new Vec2d((Size.X - 1) & ~1, (Size.Y - 1) & ~1);
    }

    public CellType this[Vec2d pos] => _grid[pos.X, pos.Y];

    public bool IsWalkable(Vec2d pos) =>
        pos.InBounds(Size) && _grid[pos.X, pos.Y] != CellType.Wall;

    public void Draw(ConsoleScreen screen, ConsoleColor wallColor, ConsoleColor corridorColor, ConsoleColor exitColor)
    {
        for (var y = 0; y < Size.Y; y++)
            for (var x = 0; x < Size.X; x++)
            {
                var pos = new Vec2d(x, y);
                var (ch, color) = _grid[x, y] switch
                {
                    CellType.Wall => ("█", wallColor),
                    CellType.Exit => ("★", exitColor),
                    _             => ("·", corridorColor)
                };
                screen.DrawCharAt(pos, ch, color);
            }
    }
}
