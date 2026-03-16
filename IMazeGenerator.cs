namespace Epsi.MazeCs;

public interface IMazeGenerator
{
    Vec2d Size { get; }
    CellType[,] Generate();
}
