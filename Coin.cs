namespace Epsi.MazeCs;

public class Coin : ICollectable
{
    public bool IsPersistent => false;
    public int Points => 10;
    public string Symbol => "¢";
    public ConsoleColor Color => ConsoleColor.Yellow;
}
