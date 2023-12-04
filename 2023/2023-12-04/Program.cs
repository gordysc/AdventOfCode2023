using System.Text.RegularExpressions;

var sw = new System.Diagnostics.Stopwatch();

sw.Start();

var lines = File.ReadAllLines(@"../../../Input/File1.txt");

var tickets = Enumerable.Repeat(1, lines.Length).ToArray();

var evaluator = new Evaluator();

for (var loop = 0; loop < lines.Length; loop++)
{
    var matches = evaluator.Evaluate(lines[loop]);

    if (matches == 0) continue;

    for (var iter = 0; iter < matches; iter++)
    {
        if (loop + iter + 1 >= lines.Length) break;
        tickets[loop + iter + 1] += tickets[loop];
    }
}

sw.Stop();

Console.WriteLine($"Total: {tickets.Sum()}");
Console.WriteLine($"Total Time: {sw.Elapsed.TotalMilliseconds}ms");

internal sealed class Evaluator
{
    public int Evaluate(string line)
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
