using System.Linq;

var input = File.ReadLines("res/LockRotations.csv");
var startValue = 50;
var totalLockNumbers = 100;

IEnumerable<Instruction> instructions = input.Select(instruction =>
{
    int amountValue;
    if(int.TryParse(instruction.Substring(1), out amountValue))
        return new Instruction(instruction.Substring(0, 1), amountValue);
    return new Instruction("R", 0);
});

var finalPosition = instructions.Aggregate(
    (Position: startValue, Count: 0), (currentValue, instruction) =>
    {
        int count = currentValue.Count;

        int rawPosition = currentValue.Position + instruction.direction switch
        {
            "R" => instruction.amount,
            "L" => -instruction.amount,
            _ => 0
        };
        // part 2
        if (instruction.direction.Equals("R"))
        {
            int dist = totalLockNumbers - currentValue.Position;
            if(dist <= instruction.amount)
            {
                int bonus = dist == 0 ? 0 : 1;
                count += bonus + (instruction.amount - dist) / totalLockNumbers;
            }
        } else
        {
            int dist = currentValue.Position;
            if (dist <= instruction.amount)
            {
                int bonus = dist == 0 ? 0 : 1;
                count += bonus + (instruction.amount - dist) / totalLockNumbers;
            }
        }
        
        
        int position = (rawPosition % totalLockNumbers + totalLockNumbers) % totalLockNumbers;
        // part 1
        /*int count = currentValue.Count + position switch
        {
            0 => 1,
            _ => 0,
        };*/

        return (Position: position, Count: count);
    });

Console.WriteLine(finalPosition.Count);

record Instruction(string direction, int amount);
