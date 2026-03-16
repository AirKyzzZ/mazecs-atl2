namespace Epsi.MazeCs;

public class Player(Vec2d position, ConsoleColor color, IController controller)
{
    private const string Symbol = "@";

    public Vec2d Position { get; private set; } = position;
    public ConsoleColor Color { get; } = color;

    public (bool Moved, bool Quit) Update(Maze maze, IGridDisplay display)
    {
        controller.ReadInput();

        if (controller.IsEscPressed)
            return (false, true);

        if (controller.Direction is not { } direction)
            return (false, false);

        return (TryMove(direction, maze, display), false);
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
