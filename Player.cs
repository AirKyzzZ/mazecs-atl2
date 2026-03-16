namespace Epsi.MazeCs;

public class Player(Vec2d position, ConsoleColor color, IController controller)
{
    private const string Symbol = "@";

    public Vec2d Position { get; private set; } = position;
    public ConsoleColor Color { get; } = color;

    public (bool Moved, bool Quit) Update(Maze maze, ConsoleScreen screen, ConsoleColor corridorColor)
    {
        controller.ReadInput();

        if (controller.IsEscPressed)
            return (false, true);

        if (controller.Direction is not { } direction)
            return (false, false);

        return (TryMove(direction, maze, screen, corridorColor), false);
    }

    private bool TryMove(Vec2d direction, Maze maze, ConsoleScreen screen, ConsoleColor corridorColor)
    {
        var next = Position + direction;

        if (!maze.IsWalkable(next))
            return false;

        screen.DrawCharAt(Position, "·", corridorColor);
        Position = next;
        Draw(screen);
        return true;
    }

    public bool IsOnExit(Maze maze) => Position == maze.Exit;

    public void Draw(ConsoleScreen screen) =>
        screen.DrawCharAt(Position, Symbol, Color);
}
