namespace Epsi.MazeCs;

public class ConsoleScreen(Vec2d offset) : IGridDisplay
{
    public Vec2d Offset { get; } = offset;

    public void Clear()
    {
        Console.Clear();
        Console.CursorVisible = false;
    }

    public void ShowCursor() => Console.CursorVisible = true;

    public void DrawText(Vec2d pos, string text, ConsoleColor? color = null)
    {
        Console.SetCursorPosition(pos.X, pos.Y);
        if (color.HasValue)
            Console.ForegroundColor = color.Value;
        Console.Write(text);
        Console.ResetColor();
    }

    public void DrawBoxedText(Vec2d pos, string title, ConsoleColor color)
    {
        var len = title.Length + 4;
        var top    = "╔" + new string('═', len) + "╗";
        var middle = "║  " + title + "  ║";
        var bottom = "╚" + new string('═', len) + "╝";

        DrawText(pos,                      top,    color);
        DrawText(pos with { Y = pos.Y + 1 }, middle, color);
        DrawText(pos with { Y = pos.Y + 2 }, bottom, color);
    }

    public void DrawCharAt(Vec2d pos, string ch, ConsoleColor color) =>
        DrawText(Offset + pos, ch, color);
}
