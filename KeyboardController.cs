namespace Epsi.MazeCs;

public class KeyboardController
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

    public (Vec2d? Direction, bool Quit) ReadInput()
    {
        var key = Console.ReadKey(true).Key;

        if (key == ConsoleKey.Escape)
            return (null, true);

        return (Directions.GetValueOrDefault(key), false);
    }

    public void WaitForKey() => Console.ReadKey(true);
}
