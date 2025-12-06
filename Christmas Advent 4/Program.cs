using System.Linq;
var input = File.ReadAllLines("res/mapGrid.csv");

char[,] shelfSlots = new char[input.Length, input[0].Length];

for (int i = 0; i < shelfSlots.GetLength(0); i++)
{
    for (int j = 0; j < shelfSlots.GetLength(1); j++)
    {
        shelfSlots[i, j] = input[i][j];
    }
}

int finalCount = 0;
while (PaperFound(out var addCount)) { finalCount += addCount; }

Console.WriteLine(finalCount);

bool PaperFound(out int count)
{
    count = 0;

    var searchSpace = Enumerable.Range(0, shelfSlots.GetLength(0))
           .SelectMany(i =>
           {
               return Enumerable.Range(0, shelfSlots.GetLength(1))
               .Select(j => (i, j));
           });
    var accessibleLocations = searchSpace.Where(point =>
    {
        return shelfSlots[point.i, point.j] == '@' && CanBeAccessed(shelfSlots, point.i, point.j);
    });
    List<(int, int)> locations = accessibleLocations.ToList();
    count += locations.Count;
    TakeMaps(locations);
    return count > 0;
}

void TakeMaps(List<(int, int)> map)
{
    foreach((int x, int y) point in map)
    {
        shelfSlots[point.x,point.y] = '.';
    }
}

bool CanBeAccessed(char[,] shelf, int x, int y){
    //-1 to account for the roll of paper at (x, y)
    int count = -1;
    for(int i = 0; i < 9; i++)
    {
        int col = x + i / 3 - 1;
        int row = y + i % 3 - 1;
        if (col < 0 || col >= shelf.GetLength(0)) continue;
        if (row < 0 || row >= shelf.GetLength(1)) continue;
        if (shelf[col, row] == '@') count++;
    }
    if (count < 4) return true;
    return false;
}