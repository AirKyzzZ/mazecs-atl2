namespace Epsi.MazeCs;

public class Key : ICollectable
{
    public bool IsPersistent => true;
    public int Points => 0;
    public string Symbol => "♦";
    public ConsoleColor Color => ConsoleColor.Cyan;
}
