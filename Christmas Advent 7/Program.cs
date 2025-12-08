char[][] input = File.ReadLines("res/tachyonManifold.csv").Select(s =>
{
    return s.ToCharArray();
}).ToArray();

int startPoint = input[0].Select((item, index) => (item, index))
    .First(x => x.item == 'S').index;

Dictionary<(int, int), long> memo = new();
long timelines = InitNewBeamOnSplitter(input, startPoint, 0);

Console.WriteLine(SendBeamDown(input, startPoint, 0, 0));


Console.WriteLine("Part 2");
Console.WriteLine(timelines);


long InitNewBeamOnSplitter(char[][] inputGrid, int x, int y)
{
    long result = 0;
    if (x < 0 || x >= inputGrid[y].Length || y < 0 || y >= inputGrid.Length-1) return 1;
    char next = inputGrid[y + 1][x];
    if (memo.ContainsKey((x, y)))
        return memo[(x, y)];
    if (next == '.')
    {
        result += InitNewBeamOnSplitter(inputGrid, x, y + 1);
    }
    else if (next == '^')
    {
        if (memo.TryGetValue((x, y), out long value))
            result += value;
        else
        {
            memo.Add((x, y), InitNewBeamOnSplitter(inputGrid, x - 1, y + 1) + InitNewBeamOnSplitter(inputGrid, x + 1, y + 1));
            result += memo[(x, y)];
        }
    }
    return result;
}


int SendBeamDown(char[][] inputGrid, int x, int y, int currentCount)
{
    if (x < 0 || x >= inputGrid[y].Length || y < 0 || y >= inputGrid.Length - 1) return currentCount;
    char next = inputGrid[y + 1][x];
    if (next == '.')
    {
        inputGrid[y + 1][x] = '|';
        return SendBeamDown(inputGrid, x, y + 1, currentCount);
    } else if (next == '^')
    {
        inputGrid[y + 1][x - 1] = '|';
        inputGrid[y + 1][x + 1] = '|';
        return SendBeamDown(inputGrid, x - 1, y + 1, currentCount) + SendBeamDown(inputGrid, x+1, y+1, currentCount) + 1;
    }
    return currentCount;
}