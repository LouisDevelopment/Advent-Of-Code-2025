// part 2 makes the "Ingredient" record and comparisons irrelevant, kept them here anyway
var input = File.ReadLines("res/ingredientIds.csv");

bool pastSeparator = false;
List<IdSet> idSets = new List<IdSet>();
List<Ingredient> ingredients = new List<Ingredient>();
foreach (string line in input)
{
    if (line.IsWhiteSpace()) {
        pastSeparator = true; 
        continue;
    }
    if (!pastSeparator)
    {
        string[] idRange = line.Split('-');
        if (ParseOrLog(idRange[0]) is long start && ParseOrLog(idRange[1]) is long end)
            idSets.Add(new IdSet(start, end));
    } else
    {
        if (ParseOrLog(line) is long ingredientId)
            ingredients.Add(new Ingredient(ingredientId));
    }
    
}
// could build a custom binary search for start values to make the main loop O(m log n), but overkill for this problem
idSets.Sort((a, b) => a.start.CompareTo(b.start));

List<IdSet> collapsedRanges = new List<IdSet>();
foreach(IdSet idSet in idSets)
{
    if (collapsedRanges.Count == 0)
    {
        collapsedRanges.Add(idSet);
        continue;
    }
    var prev = collapsedRanges[collapsedRanges.Count - 1];
    if (idSet.start <= prev.end+1)
    {
        collapsedRanges[collapsedRanges.Count - 1] = prev with { end = Math.Max(prev.end, idSet.end) };
    }
    else
    {
        collapsedRanges.Add(idSet);
    }
}

// main loop for part 1
/*var total = 0;
while(ingredients.Count > 0)
{
    if (ContainsIngredient(ingredients[ingredients.Count-1])){
        total++;
    }
    ingredients.RemoveAt(ingredients.Count - 1);
}

Console.WriteLine("Part 1");
Console.WriteLine(total);*/

var totalPart2 = collapsedRanges.Aggregate((long)0, (count, range) =>
{
    return count+range.end - range.start + 1;
});

Console.WriteLine("Part 2");
Console.WriteLine(totalPart2);

bool ContainsIngredient(Ingredient ingredient)
{
    foreach(var idSet in idSets)
    {
        if (ingredient.id >= idSet.start && ingredient.id <= idSet.end) return true;
    }
    return false;
}

long? ParseOrLog(string line)
{
    if (long.TryParse(line, out var value))
    {
        return value;
    }
    Console.WriteLine("FAILED TO PARSE DATA AS A LONG FOR " + line);
    return null;
}

record IdSet(long start, long end);
record Ingredient(long id);