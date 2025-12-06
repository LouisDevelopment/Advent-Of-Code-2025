// part 1 used bytes and ints, as it expected 2 digit number calculations, for part 2 we need long's as numbers are now 12 digits.
var input = File.ReadLines("res/joltageList.csv");

int numberOfDigits = 12;

var joltages = input.Select(batterySet =>
{
    return batterySet.Select(battery =>
    {
        return (byte)(battery - '0');
    });
});

long finalValue = 0 + joltages.Select(joltageRow =>
{
    // For part 1
    //return GetMaxValue(joltageRow.ToArray());
    return GetMaxValue(joltageRow.ToArray(), 0, 0, numberOfDigits);
}).Sum(id => (long)id);

Console.WriteLine(finalValue);

long GetMaxValue(byte[] row, int startIndex, long currentValue, int numberOfDigits)
{
    if (numberOfDigits == 0) return currentValue;

    long nextValue = 0;
    int nextIndex = startIndex;
    for (int i = startIndex; i < row.Length-numberOfDigits+1; i++)
    {
        if (row[i] > nextValue)
        {
            nextIndex = i;
            nextValue = row[i];
        }
    }

    return GetMaxValue(row, ++nextIndex, currentValue*10+nextValue, --numberOfDigits);
}

// For part 1
/*byte GetMaxValue(byte[] row)
{
    byte maxValue = 0;

    for (byte i = 0; i < row.Length; i++)
    {
        for (byte j = (byte)(i + 1); j < row.Length; j++)
        {
            byte newValue = (byte)(row[i] * 10 + row[j]);
            if (newValue > maxValue) maxValue = newValue;
        }
    }
    return maxValue;
}*/