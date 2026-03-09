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
const ConsoleColor WallColor        = ConsoleColor.DarkGray;
const ConsoleColor CorridorColor    = ConsoleColor.DarkBlue;
const ConsoleColor PlayerColor      = ConsoleColor.Yellow;
const ConsoleColor ExitColor        = ConsoleColor.Green;
#endregion

var screen = new ConsoleScreen(new Vec2d(0, 3));
var keyboard = new KeyboardController();
var grid = new CellType[size.X, size.Y];
var player = new Vec2d(0, 0);
var mode = State.Playing;

GenerateMaze(grid, player);
DrawScreen();

while (mode == State.Playing)
{
    var (direction, quit) = keyboard.ReadInput();

    if (quit)
    {
        mode = State.Canceled;
        break;
    }

    if (direction is null) continue;

    var next = player + direction;

    if (next.InBounds(size) && grid[next.X, next.Y] != CellType.Wall)
    {
        if (grid[next.X, next.Y] == CellType.Exit) mode = State.Won;

        UpdateCell(player, CellType.Corridor);
        player = next;
        UpdateCell(player, CellType.Player);
    }
}

var msgPos = new Vec2d(0, screen.Offset.Y + size.Y + marginYMessage);

if (mode == State.Won)
    screen.DrawBoxedText(new Vec2d(4, msgPos.Y), sWin, SuccessColor);
else
    screen.DrawText(msgPos, sCanceled, DangerColor);

screen.DrawText(new Vec2d(0, msgPos.Y + messageHeight), sPressKey);
screen.ShowCursor();
keyboard.WaitForKey();

#region Functions

void DrawCell(Vec2d pos) => screen.DrawCharAt(
    pos,
    grid[pos.X, pos.Y] switch
    {
        CellType.Wall   => "█",
        CellType.Player => "@",
        CellType.Exit   => "★",
        _               => "·"
    },
    grid[pos.X, pos.Y] switch
    {
        CellType.Wall   => WallColor,
        CellType.Player => PlayerColor,
        CellType.Exit   => ExitColor,
        _               => CorridorColor
    });

void UpdateCell(Vec2d pos, CellType type)
{
    grid[pos.X, pos.Y] = type;
    DrawCell(pos);
}

void DrawScreen()
{
    screen.Clear();
    screen.DrawBoxedText(new Vec2d(4, 0), sHeader, InfoColor);
    for (var y = 0; y < size.Y; y++)
        for (var x = 0; x < size.X; x++)
            DrawCell(new Vec2d(x, y));
    screen.DrawText(new Vec2d(0, screen.Offset.Y + size.Y), sInstructions, InstructionColor);
}

void GenerateMaze(CellType[,] grid, Vec2d start)
{
    for (var y = 0; y < size.Y; y++)
        for (var x = 0; x < size.X; x++)
            grid[x, y] = CellType.Wall;

    int[] dx = [0, 1, 0, -1];
    int[] dy = [-1, 0, 1, 0];
    int[][] orders = [
        [0, 1, 2, 3], [0, 1, 3, 2], [0, 2, 1, 3], [0, 2, 3, 1], [0, 3, 1, 2], [0, 3, 2, 1],
        [1, 0, 2, 3], [1, 0, 3, 2], [1, 2, 0, 3], [1, 2, 3, 0], [1, 3, 0, 2], [1, 3, 2, 0],
        [2, 0, 1, 3], [2, 0, 3, 1], [2, 1, 0, 3], [2, 1, 3, 0], [2, 3, 0, 1], [2, 3, 1, 0],
        [3, 0, 1, 2], [3, 0, 2, 1], [3, 1, 0, 2], [3, 1, 2, 0], [3, 2, 0, 1], [3, 2, 1, 0]
    ];
    var rng = new Random();

    GenerateMazeRec(start.X, start.Y);

    var exit = new Vec2d((size.X - 1) & ~1, (size.Y - 1) & ~1);

    grid[start.X, start.Y] = CellType.Player;
    grid[exit.X, exit.Y] = CellType.Exit;

    void GenerateMazeRec(int x, int y)
    {
        grid[x, y] = CellType.Corridor;
        foreach (var dir in orders[rng.Next(orders.Length)])
        {
            if (InMaze(x, dx[dir], size.X, out var nx) &&
                InMaze(y, dy[dir], size.Y, out var ny) &&
                grid[nx, ny] == CellType.Wall)
            {
                grid[(x + nx) / 2, (y + ny) / 2] = CellType.Corridor;
                GenerateMazeRec(nx, ny);
            }
        }
        bool InMaze(int val, int delta, int max, out int next) =>
            (next = val + delta * 2) >= 0 && next < max;
    }
}
#endregion
