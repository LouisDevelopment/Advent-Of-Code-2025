var input = File.ReadLines("res/mapGrid.csv");

char[][] shelfSlots = input.Select(shelfRow =>
{
    return shelfRow.ToCharArray();
}).ToArray();

int finalCount = 0;
while (paperFound(out var addCount)) { finalCount += addCount; }

Console.WriteLine(finalCount);

bool paperFound(out int count)
{
    count = 0;
    for (int i = 0; i < shelfSlots.Length; i++)
    {
        for (int j = 0; j < shelfSlots[i].Length; j++)
        {
            if (shelfSlots[i][j] == '@' && canBeAccessed(shelfSlots, i, j))
            {
                shelfSlots[i][j] = '.';
                count++;
            }
        }
    }
    return count > 0;
}

bool canBeAccessed(char[][] shelf, int x, int y){
    //-1 to account for the roll of paper at (x, y)
    int count = -1;
    for(int i = 0; i < 9; i++)
    {
        int col = x + i / 3 - 1;
        int row = y + i % 3 - 1;
        if (col < 0 || col >= shelf.Length) continue;
        if (row < 0 || row >= shelf[0].Length) continue;
        if (shelf[col][row] == '@') count++;
    }
    if (count < 4) return true;
    return false;
}