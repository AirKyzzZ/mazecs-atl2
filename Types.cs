namespace Epsi.MazeCs;

public record Vec2d(int X, int Y)
{
    public static Vec2d operator +(Vec2d a, Vec2d b) => new(a.X + b.X, a.Y + b.Y);
    public bool InBounds(Vec2d max) => X >= 0 && X < max.X && Y >= 0 && Y < max.Y;
}

public enum State
{
    Playing,
    Won,
    Canceled
}

public enum CellType
{
    Corridor = 0,
    Wall = 1,
    Player = 2,
    Exit = 3
}
