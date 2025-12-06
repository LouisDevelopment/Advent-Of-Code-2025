using System.Runtime.Intrinsics.Arm;

var input = File.ReadLines("res/idList.csv").First();

var idSets = input.Split(',').Select(idSet =>
{
    string[] idRange = idSet.Split('-');
    if (long.TryParse(idRange[0], out var start) && long.TryParse(idRange[1], out var end))
    {
        return new IdSet(start, end);
    }
    Console.WriteLine("FAILED TO PARSE DATA AS A LONG FOR " + idSet);
    return new IdSet(0, 0);
});

long totalIds = idSets.SelectMany(idSet => GetRange(idSet.start, idSet.end))
    //.Where(id => IsInvalidId(id.ToString()))
    .Where(id => IsInvalidIdPart2(id.ToString()))
    .Sum(id => (long)id);

Console.WriteLine(totalIds);

IEnumerable<long> GetRange(long start, long end)
{
    while(start <= end)
    {
        yield return start;
        start++;
    }
    yield break;
}

bool IsInvalidId(string id) => id.Length % 2 == 0 && id[..(id.Length / 2)] == id[(id.Length / 2)..];

bool IsInvalidIdPart2(string id) {
    if (long.TryParse(id, out var parseResult))
    {
        for (int i = 1; i <= id.Length/2; i++)
        {
            if (id.Length % i != 0) continue;

            if(string.Concat(Enumerable.Repeat(id[..i], id.Length / i)) == id)
            {
                return true;
            }
        }
    }
    return false;
};

record IdSet(long start, long end);