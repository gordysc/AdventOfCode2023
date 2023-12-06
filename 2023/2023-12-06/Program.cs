// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;

var sw = new System.Diagnostics.Stopwatch();
sw.Start();

var lines = File.ReadAllLines(@"../../../Input/File1 .txt");

var evaluator = new Evaluator(lines);
var result = evaluator.Evaluate();

sw.Stop();

Console.WriteLine($"Result: {result}");
Console.WriteLine($"Total Time: {sw.Elapsed.TotalMilliseconds}ms");
internal record Evaluator(string[] lines)
{
    public int Evaluate()
    {
        var times = ParseLine(lines[0]);
        var distances = ParseLine(lines[1]);
        var combinations = new int[times.Length];

        for (var loop = 0; loop < times.Length; loop++)
            combinations[loop] = EvaluateRace(times[loop], distances[loop]);

        return combinations.Aggregate(1, (a, b) => a * b);
    }
    
    private int EvaluateRace(int time, int distance)
    {
        var combinations = 0;
        
        for (var loop = 0; loop < time; loop++)
        {
            var speed = loop;
            var result = speed * (time - loop);

            if (result > distance)
                combinations++;
        }

        return combinations;
    }

    private int[] ParseLine(string values)
    {
        var sanitized = Regex.Replace(values.Trim(), @"\s+", " ");

        return sanitized.Split(":")[1].Trim().Split(" ").Select(int.Parse).ToArray();
    }
}
