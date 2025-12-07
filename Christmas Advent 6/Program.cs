var input = File.ReadAllLines("res/sumSet.csv");

List<string[]> sumGrid = new();
List<Sum> sums = new List<Sum>();

int rowLength = input[0].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Length;


for (int i = 0; i < rowLength; i++)
{
    List<string> values = new();
    for (int j = 0; j < input.Length; j++)
    {
        string[] row = input[j].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if(row.Length > i)
            values.Add(row[i]);
    }
    sumGrid.Add(values.ToArray());
}
foreach (string[] sum in sumGrid)
{
    List<long> values = new();
    foreach(string s in sum)
    {
        if (s.Length == 0) continue;
        if (int.TryParse(s, out int result))
        {
            values.Add(result);
        }
        else
        {
            SumType type = s switch
            {
                "*" => SumType.Multiply,
                "+" => SumType.Add,
                _ => throw new Exception("Invalid Sum Type")
            };
            if (values.Count > 0)
                sums.Add(new Sum(values.ToList(), type));
        }
    }
}

long total = sums.Aggregate(0L, (total, sum) =>
{
    return total+solveSum(sum);
});

Console.WriteLine(total);

long solveSum(Sum sum)
{
    return sum.values.Aggregate((total, value) =>
    {
        if (sum.type == SumType.Add)
        {
            return total + value;
        }
        return total * value;
    });
}

enum SumType
{
    Add, Multiply
};

record Sum(List<long> values, SumType type);
