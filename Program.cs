using Epsi.MazeCs;

#region Constants
var size = new Vec2d(50, 20);

const int marginYMessage = 3;
const int messageHeight = 5;

const string sHeader = "🏃 LABYRINTHE ASCII  C# 🏃";
const string sInstructions = "  [Z/↑] Haut   [S/↓] Bas   [Q/←] Gauche   [D/→] Droite   [Échap] Quitter";
const string sWin = "🎉  FÉLICITATIONS ! Vous avez trouvé la sortie !";
const string sCanceled = "\n  Partie abandonnée. À bientôt !";
const string sPressKey = "  Appuyez sur une key pour quitter...";

const ConsoleColor SuccessColor     = ConsoleColor.Green;
const ConsoleColor DangerColor      = ConsoleColor.Red;
const ConsoleColor InfoColor        = ConsoleColor.Cyan;
const ConsoleColor InstructionColor = ConsoleColor.DarkCyan;
const ConsoleColor PlayerColor      = ConsoleColor.Yellow;
#endregion

IController keyboard = new KeyboardController();
var screen = new ConsoleScreen(new Vec2d(0, 3));
var maze = new Maze(new MazeGen(size, coinProbability: 0.3));
var player = new Player(new Vec2d(0, 0), PlayerColor, keyboard);
var mode = State.Playing;

screen.Clear();
screen.DrawBoxedText(new Vec2d(4, 0), sHeader, InfoColor);
maze.Draw(screen);
player.Draw(screen);
screen.DrawText(new Vec2d(0, screen.Offset.Y + maze.Size.Y), sInstructions, InstructionColor);

while (mode == State.Playing)
{
    var (moved, quit) = player.Update(maze, screen);

    if (quit)
    {
        mode = State.Canceled;
        break;
    }

    if (moved && player.IsOnExit(maze))
        mode = State.Won;
}

var msgPos = new Vec2d(0, screen.Offset.Y + maze.Size.Y + marginYMessage);

if (mode == State.Won)
    screen.DrawBoxedText(new Vec2d(4, msgPos.Y), sWin, SuccessColor);
else
    screen.DrawText(msgPos, sCanceled, DangerColor);

screen.DrawText(new Vec2d(0, msgPos.Y + messageHeight), sPressKey);
screen.ShowCursor();
keyboard.WaitForKey();
