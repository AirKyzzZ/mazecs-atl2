namespace Epsi.MazeCs;

public class Door : Cell
{
    public Key RequiredKey { get; } = new();

    public override bool IsWalkable => true;
    public override string Symbol => "▒";
    public override ConsoleColor Color => ConsoleColor.DarkRed;
    public override ICollectable? RequiredItem => RequiredKey;
}
