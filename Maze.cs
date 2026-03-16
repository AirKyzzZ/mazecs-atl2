namespace Epsi.MazeCs;

public class Maze
{
    private readonly Cell[,] _grid;

    public Vec2d Size { get; }
    public Vec2d Exit { get; }

    public Maze(IMazeGenerator generator)
    {
        _grid = generator.Generate();
        Size = generator.Size;
        Exit = new Vec2d((Size.X - 1) & ~1, (Size.Y - 1) & ~1);
    }

    public Cell this[Vec2d pos] => _grid[pos.X, pos.Y];

    public bool IsWalkable(Vec2d pos) =>
        pos.InBounds(Size) && _grid[pos.X, pos.Y].IsWalkable;

    public void Draw(IGridDisplay display)
    {
        for (var y = 0; y < Size.Y; y++)
            for (var x = 0; x < Size.X; x++)
            {
                var pos = new Vec2d(x, y);
                var cell = _grid[x, y];
                display.DrawCharAt(pos, cell.Symbol, cell.Color);
            }
    }
}
