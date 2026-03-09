using Epsi.MazeCs;

#region Constants
var size = new Vec2d(50, 20);
var offset = new Vec2d(0, 3);

const int marginYMessage = 3;
const int messageHeight = 5;

const string sHeader = """
    ╔══════════════════════════════════════════════════╗
    ║          🏃 LABYRINTHE ASCII  C#  🏃             ║
    ╚══════════════════════════════════════════════════╝
    """;
const string sInstructions = "  [Z/↑] Haut   [S/↓] Bas   [Q/←] Gauche   [D/→] Droite   [Échap] Quitter";
const string sWin = """
    ╔════════════════════════════════╗
    ║   🎉  FÉLICITATIONS !  🎉      ║
    ║   Vous avez trouvé la sortie ! ║
    ╚════════════════════════════════╝
""";
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

var grid = new CellType[size.X, size.Y];
var player = new Vec2d(0, 0);
var mode = State.Playing;

GenerateMaze(grid, player);
DrawScreen();

while (mode == State.Playing)
{
    var key = Console.ReadKey(true).Key;

    var delta = key switch
    {
        ConsoleKey.Z or ConsoleKey.UpArrow    => new Vec2d( 0, -1),
        ConsoleKey.S or ConsoleKey.DownArrow  => new Vec2d( 0,  1),
        ConsoleKey.Q or ConsoleKey.LeftArrow  => new Vec2d(-1,  0),
        ConsoleKey.D or ConsoleKey.RightArrow => new Vec2d( 1,  0),
        ConsoleKey.Escape                     => null,
        _                                     => null
    };

    if (delta is null)
    {
        if (key == ConsoleKey.Escape) mode = State.Canceled;
        continue;
    }

    var next = player + delta;

    if (next.InBounds(size) && grid[next.X, next.Y] != CellType.Wall)
    {
        if (grid[next.X, next.Y] == CellType.Exit) mode = State.Won;

        UpdateCell(player, CellType.Corridor);
        player = next;
        UpdateCell(player, CellType.Player);
    }
}

DrawTextColorXY(new Vec2d(0, offset.Y + size.Y + marginYMessage),
    mode == State.Won
    ? (sWin, SuccessColor)
    : (sCanceled, DangerColor)
);
DrawTextXY(new Vec2d(0, offset.Y + size.Y + marginYMessage + messageHeight), sPressKey);
Console.CursorVisible = true;
Console.ReadKey(true);

#region Functions

void DrawTextXY(Vec2d pos, string text, ConsoleColor? color = null)
{
    Console.SetCursorPosition(pos.X, pos.Y);
    if (color.HasValue)
    {
        Console.ForegroundColor = color.Value;
    }
    Console.Write(text);
    Console.ResetColor();
}

void DrawTextColorXY(Vec2d pos, (string text, ConsoleColor color) info) =>
    DrawTextXY(pos, info.text, info.color);

void DrawCell(Vec2d pos) => DrawTextColorXY(
    offset + pos,
    grid[pos.X, pos.Y] switch
    {
        CellType.Wall   => ("█", WallColor),
        CellType.Player => ("@", PlayerColor),
        CellType.Exit   => ("★", ExitColor),
        _               => ("·", CorridorColor)
    });

void UpdateCell(Vec2d pos, CellType type)
{
    grid[pos.X, pos.Y] = type;
    DrawCell(pos);
}

void DrawScreen()
{
    Console.Clear();
    Console.CursorVisible = false;

    DrawTextXY(new Vec2d(0, 0), sHeader, InfoColor);
    for (var y = 0; y < size.Y; y++)
    {
        for (var x = 0; x < size.X; x++)
        {
            DrawCell(new Vec2d(x, y));
        }
    }
    DrawTextXY(new Vec2d(0, offset.Y + size.Y), sInstructions, InstructionColor);
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
