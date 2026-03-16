namespace Epsi.MazeCs;

public class KeyboardController : IController
{
    private static readonly Dictionary<ConsoleKey, Vec2d> Directions = new()
    {
        [ConsoleKey.Z]          = new( 0, -1),
        [ConsoleKey.UpArrow]    = new( 0, -1),
        [ConsoleKey.S]          = new( 0,  1),
        [ConsoleKey.DownArrow]  = new( 0,  1),
        [ConsoleKey.Q]          = new(-1,  0),
        [ConsoleKey.LeftArrow]  = new(-1,  0),
        [ConsoleKey.D]          = new( 1,  0),
        [ConsoleKey.RightArrow] = new( 1,  0),
    };

    public Vec2d? Direction { get; private set; }
    public bool IsEscPressed { get; private set; }

    public void ReadInput()
    {
        var key = Console.ReadKey(true).Key;
        IsEscPressed = key == ConsoleKey.Escape;
        Direction = IsEscPressed ? null : Directions.GetValueOrDefault(key);
    }

    public void WaitForKey() => Console.ReadKey(true);
}
