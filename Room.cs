namespace Epsi.MazeCs;

public class Room(bool isExit = false) : Cell
{
    public bool IsExit { get; } = isExit;
    public ICollectable? Item { get; set; }

    public override bool IsWalkable => true;
    public override string Symbol => Item?.Symbol ?? (IsExit ? "★" : "·");
    public override ConsoleColor Color => Item?.Color ?? (IsExit ? ConsoleColor.Green : ConsoleColor.DarkBlue);

    public override ICollectable? PickUp()
    {
        var item = Item;
        Item = null;
        return item;
    }
}
