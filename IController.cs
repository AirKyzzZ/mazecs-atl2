namespace Epsi.MazeCs;

public interface IController
{
    void ReadInput();
    Vec2d? Direction { get; }
    bool IsEscPressed { get; }
    void WaitForKey();
}
