// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;

var sw = new System.Diagnostics.Stopwatch();
var lines = File.ReadAllLines(@"../../../Input/File1.txt");

sw.Start();

var evaluator = new Evaluator(lines);
var result = evaluator.Evaluate();

sw.Stop();

Console.WriteLine($"Result: {result}");
Console.WriteLine($"Total Time: {sw.Elapsed.TotalMilliseconds}ms");

internal record Evaluator(string[] lines)
{
    public long Evaluate()
    {
        var time = ParseLine(lines[0]);
        var distance = ParseLine(lines[1]) + 1;
        
        return EvaluateRace(time, distance);
    }
    
    private long EvaluateRace(long time, long distance)
    {
        var value = Math.Sqrt(Math.Pow(time, 2) - 4 * distance);

        var max = (long) Math.Floor((time + value) / 2);
        var min = (long) Math.Ceiling((time - value) / 2);

        return max - min + 1;
    }

    private long ParseLine(string line)
    {
        var values = line.Split(":")[1].Trim();
        
        return long.Parse(Regex.Replace(values, @"\s+", ""));
    }
}