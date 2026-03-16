namespace Epsi.MazeCs;

public class Room(bool isExit = false) : Cell
{
    public bool IsExit { get; } = isExit;

    public override bool IsWalkable => true;
    public override string Symbol => IsExit ? "★" : "·";
    public override ConsoleColor Color => IsExit ? ConsoleColor.Green : ConsoleColor.DarkBlue;
}
