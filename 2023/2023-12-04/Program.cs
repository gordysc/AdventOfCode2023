using System.Text.RegularExpressions;

var sw = new System.Diagnostics.Stopwatch();

sw.Start();

var lines = File.ReadAllLines(@"../../../Input/File1.txt");
var total = 0;

var evaluator = new Evaluator();

foreach (var line in lines)
{
    var count = evaluator.FindCount(line);
    total += evaluator.Calculate(count);
}

sw.Stop();

Console.WriteLine($"Total: {total}");
Console.WriteLine($"Total Time: {sw.Elapsed.TotalMilliseconds}ms");

internal sealed class Evaluator
{
    public int Calculate(int total)
    {
        return total < 2 ? total : (int) Math.Pow(2, total - 1);
    }
    
    public int FindCount(string line)
    {
        var values = line.Split(":")[1].Split("|");
        var count = 0;
        
        var winning = ParseNumbers(values[0]);
        var numbers = ParseNumbers(values[1]);

        foreach (var value in numbers)
        {
            if (!winning.Contains(value))
                continue;

            count++;
        }

        return count;
    }

    private int[] ParseNumbers(string values)
    {
        var sanitized = Regex.Replace(values.Trim(), @"\s+", " ");
        
        return sanitized.Split(" ").Select(s => int.Parse(s)).ToArray();
    }
}