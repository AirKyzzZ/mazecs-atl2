// ============================================================
//  LABYRINTHE ASCII - C# Console
//  Tableau : int[50, 20]  (width=50, height=20)
//  0 = couloir   1 = mur   2 = joueur   3 = sortie
//  Déplacement : Z/Q/S/D ou flèches
//  ✅ Optimisé : seules les cellules modifiées sont redessinées
//               via Console.SetCursorPosition()
// ============================================================

var grid = new CellType[50, 20];

const int width = 50;
const int height = 20;

// Décalage vertical dans la console (nombre de lignes du titre)
const int offsetY = 3;
const int offsetX = 0;

// ── Génération du labyrinthe par « recursive backtracker » ──
const int cellW = width / 2;   // 25
const int cellH = height / 2;   // 10

for (var y = 0; y < height; y++)
    for (var x = 0; x < width; x++)
        grid[x, y] = CellType.Wall;

var stackX = new int[cellW * cellH];
var stackY = new int[cellW * cellH];
var stackTop = 0;

var visited = new bool[cellW, cellH];

int[] dx = [ 0, 1, 0, -1 ];
int[] dy = [ -1, 0, 1, 0 ];

Random rng = new Random();

var startCX = 0;
var startCY = 0;
visited[startCX, startCY] = true;
grid[startCX * 2, startCY * 2] = CellType.Corridor;

stackX[stackTop] = startCX;
stackY[stackTop] = startCY;
stackTop++;

while (stackTop > 0)
{
    var cx = stackX[stackTop - 1];
    var cy = stackY[stackTop - 1];

    int[] order = { 0, 1, 2, 3 };
    rng.Shuffle(order);

    var found = false;
    foreach (var dir in order)
    {
        var nx = cx + dx[dir];
        var ny = cy + dy[dir];
        if (nx >= 0 && nx < cellW && ny >= 0 && ny < cellH && !visited[nx, ny])
        {
            grid[cx * 2 + dx[dir], cy * 2 + dy[dir]] = CellType.Corridor;
            grid[nx * 2, ny * 2] = CellType.Corridor;
            visited[nx, ny] = true;
            stackX[stackTop] = nx;
            stackY[stackTop] = ny;
            stackTop++;
            found = true;
            break;
        }
    }
    if (!found) stackTop--;
}

// ── Position joueur et sortie ──
var playerX = 0;
var playerY = 0;
var outX = (cellW - 1) * 2;
var outY = (cellH - 1) * 2;

grid[playerX, playerY] = CellType.Player;
grid[outX, outY] = CellType.Exit;

// ── Dessin initial complet (une seule fois) ──
Console.Clear();
Console.CursorVisible = false;

Console.SetCursorPosition(0, 0);
Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine("╔══════════════════════════════════════════════════╗");
Console.WriteLine("║          🏃 LABYRINTHE ASCII  C#  🏃             ║");
Console.WriteLine("╚══════════════════════════════════════════════════╝");
Console.ResetColor();

for (var y = 0; y < height; y++)
{
    for (var x = 0; x < width; x++)
    {
        Console.SetCursorPosition(offsetX + x, offsetY + y);
        var cell = grid[x, y];
        if (cell == CellType.Wall)      { Console.ForegroundColor = ConsoleColor.DarkGray;  Console.Write("█"); }
        else if (cell == CellType.Player) { Console.ForegroundColor = ConsoleColor.Yellow;    Console.Write("@"); }
        else if (cell == CellType.Exit) { Console.ForegroundColor = ConsoleColor.Green;     Console.Write("★"); }
        else                { Console.ForegroundColor = ConsoleColor.DarkBlue;  Console.Write("·"); }
    }
}

Console.SetCursorPosition(0, offsetY + height + 1);
Console.ForegroundColor = ConsoleColor.DarkCyan;
Console.Write("  [Z/↑] Haut   [S/↓] Bas   [Q/←] Gauche   [D/→] Droite   [Échap] Quitter");
Console.ResetColor();

// ── Action locale : redessiner UNE seule cellule via SetCursorPosition ──
void DrawCell(int cx, int cy)
{
    Console.SetCursorPosition(offsetX + cx, offsetY + cy);
    var cell = grid[cx, cy];
    if (cell == CellType.Wall)      { Console.ForegroundColor = ConsoleColor.DarkGray;  Console.Write("█"); }
    else if (cell == CellType.Player) { Console.ForegroundColor = ConsoleColor.Yellow;    Console.Write("@"); }
    else if (cell == CellType.Exit) { Console.ForegroundColor = ConsoleColor.Green;     Console.Write("★"); }
    else                { Console.ForegroundColor = ConsoleColor.DarkBlue;  Console.Write("·"); }
    Console.ResetColor();
}

// ── Boucle de jeu ──
var won = false;

while (!won)
{
    var key = Console.ReadKey(true).Key;

    var nx2 = playerX;
    var ny2 = playerY;

    if      (key == ConsoleKey.Z || key == ConsoleKey.UpArrow)    ny2--;
    else if (key == ConsoleKey.S || key == ConsoleKey.DownArrow)  ny2++;
    else if (key == ConsoleKey.Q || key == ConsoleKey.LeftArrow)  nx2--;
    else if (key == ConsoleKey.D || key == ConsoleKey.RightArrow) nx2++;
    else if (key == ConsoleKey.Escape) break;

    if (nx2 >= 0 && nx2 < width && ny2 >= 0 && ny2 < height && grid[nx2, ny2] != CellType.Wall)
    {
        if (grid[nx2, ny2] == CellType.Exit) won = true;

        // ✅ Efface l'ancienne position (couloir) → 1 seule case redessinée
        grid[playerX, playerY] = CellType.Corridor;
        DrawCell(playerX, playerY);

        // ✅ Dessine la nouvelle position → 1 seule case redessinée
        playerX = nx2;
        playerY = ny2;
        grid[playerX, playerY] = CellType.Player;
        DrawCell(playerX, playerY);
    }
}

// ── Écran de victoire ──
Console.SetCursorPosition(0, offsetY + height + 3);
if (won)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("  ╔════════════════════════════════╗");
    Console.WriteLine("  ║   🎉  FÉLICITATIONS !  🎉      ║");
    Console.WriteLine("  ║   Vous avez trouvé la sortie ! ║");
    Console.WriteLine("  ╚════════════════════════════════╝");
    Console.ResetColor();
}
else
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("\n  Partie abandonnée. À bientôt !");
    Console.ResetColor();
}

Console.SetCursorPosition(0, offsetY + height + 8);
Console.WriteLine("  Appuyez sur une key pour quitter...");
Console.CursorVisible = true;
Console.ReadKey(true);

enum CellType
{
    Corridor = 0,
    Wall = 1,
    Player = 2,
    Exit = 3
}