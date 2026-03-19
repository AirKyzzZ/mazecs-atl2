namespace Epsi.MazeCs;

public class Player(Vec2d position, ConsoleColor color, IController controller)
{
    private const string Symbol = "@";
    private readonly List<ICollectable> _inventory = [];

    public Vec2d Position { get; private set; } = position;
    public ConsoleColor Color { get; } = color;
    public int Points { get; private set; }
    public IReadOnlyList<ICollectable> Inventory => _inventory;

    public event Action<int>? PointsChanged;
    public event Action<ICollectable>? InventoryChanged;

    public (bool Moved, bool Quit) Update(Maze maze, IGridDisplay display)
    {
        controller.ReadInput();

        if (controller.IsEscPressed)
            return (false, true);

        if (controller.IsPickUpPressed)
        {
            TryPickUp(maze, display);
            return (false, false);
        }

        if (controller.Direction is not { } direction)
            return (false, false);

        return (TryMove(direction, maze, display), false);
    }

    private void TryPickUp(Maze maze, IGridDisplay display)
    {
        var item = maze[Position].PickUp();
        if (item is null) return;

        Collect(item);

        var cell = maze[Position];
        display.DrawCharAt(Position, cell.Symbol, cell.Color);
        Draw(display);
    }

    private void Collect(ICollectable item)
    {
        if (item.Points > 0)
        {
            Points += item.Points;
            PointsChanged?.Invoke(Points);
        }

        if (item.IsPersistent)
        {
            _inventory.Add(item);
            InventoryChanged?.Invoke(item);
        }
    }

    private bool TryMove(Vec2d direction, Maze maze, IGridDisplay display)
    {
        var next = Position + direction;

        if (!maze.IsWalkable(next))
            return false;

        var cell = maze[Position];
        display.DrawCharAt(Position, cell.Symbol, cell.Color);
        Position = next;
        Draw(display);
        return true;
    }

    public bool IsOnExit(Maze maze) => Position == maze.Exit;

    public void Draw(IGridDisplay display) =>
        display.DrawCharAt(Position, Symbol, Color);
}
