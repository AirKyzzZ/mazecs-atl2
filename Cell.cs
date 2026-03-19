namespace Epsi.MazeCs;

public abstract class Cell
{
    public abstract bool IsWalkable { get; }
    public abstract string Symbol { get; }
    public abstract ConsoleColor Color { get; }
    public virtual ICollectable? PickUp() => null;
}
