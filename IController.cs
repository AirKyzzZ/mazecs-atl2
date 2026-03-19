namespace Epsi.MazeCs;

public interface IController
{
    void ReadInput();
    Vec2d? Direction { get; }
    bool IsEscPressed { get; }
    bool IsPickUpPressed { get; }
    void WaitForKey();
}
