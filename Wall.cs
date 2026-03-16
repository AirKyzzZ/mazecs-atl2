namespace Epsi.MazeCs;

public class Wall : Cell
{
    public override bool IsWalkable => false;
    public override string Symbol => "█";
    public override ConsoleColor Color => ConsoleColor.DarkGray;
}
