var input = File.ReadAllLines("res/sumSet.csv");

BuildSumsPart1(input,out List<Sum> sumsPart1);

long sum1Total = sumsPart1.Aggregate(0L, (total, sum) =>
{
    return total+solveSum(sum);
});

Console.WriteLine("Part 1");
Console.WriteLine(sum1Total);


// Part 2
BuildSumsPart2(input, out List<Sum> sumsPart2);

long sum2Total = sumsPart2.Aggregate(0L, (total, sum) =>
{
    return total + solveSum(sum);
});

Console.WriteLine("Part 2");
Console.WriteLine(sum2Total);

// Helper
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

// Parse input into valid Sums

void BuildStringGridPart1(string[] inputFile, out List<string[]> sumGrid)
{
    sumGrid = new();
    int rowLength = inputFile[0].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Length;
    for (int i = 0; i < rowLength; i++)
    {
        List<string> values = new();
        for (int j = 0; j < inputFile.Length; j++)
        {
            string[] row = inputFile[j].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (row.Length > i)
                values.Add(row[i]);
        }
        sumGrid.Add(values.ToArray());
    }
}
void BuildSumsPart1(string[] inputFile, out List<Sum> sums)
{
    BuildStringGridPart1(inputFile, out List<string[]> sumGrid);
    sums = new();
    foreach (string[] sum in sumGrid)
    {
        List<long> values = new();
        foreach (string s in sum)
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
}

void BuildSumBlocks(string[] inputFile, out List<BlockData> sumBlocks)
{
    sumBlocks = new();
    int blockStart = 0;
    for (int i = 0; i < inputFile[0].Length; i++)
    {
        bool allSpaces = true;
        for (int j = 0; j < 4; j++)
        {
            if (!char.IsWhiteSpace(inputFile[j][i]))
            {
                allSpaces = false;
            }
        }
        if (allSpaces || i == inputFile[0].Length - 1)
        {
            sumBlocks.Add(new(blockStart, i + 1));
            blockStart = i + 1;
        }
    }
}

void BuildSumsPart2(string[] inputFile, out List<Sum> sums)
{
    BuildSumBlocks(inputFile, out List<BlockData> sumBlocks);

    sums = new();
    foreach (BlockData block in sumBlocks)
    {
        List<long> values = new();
        SumType type = SumType.Add;
        for (int i = block.start; i < block.end; i++)
        {
            int value = 0;
            for (int j = 0; j < inputFile.Length - 1; j++)
            {
                if (!char.IsWhiteSpace(inputFile[j][i]))
                {
                    value *= 10;
                    value += inputFile[j][i] - '0';
                }
            }
            if (!char.IsWhiteSpace(inputFile[inputFile.Length - 1][i]))
            {
                type = inputFile[4][i] switch
                {
                    '*' => SumType.Multiply,
                    '+' => SumType.Add,
                    _ => throw new Exception("Invalid Sum Type")
                };
            }
            if (value != 0)
                values.Add(value);
        }
        sums.Add(new Sum(values, type));
    }
}

// Structs
enum SumType
{
    Add, Multiply
};

record BlockData(int start, int end);

record Sum(List<long> values, SumType type);
