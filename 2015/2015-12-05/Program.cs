using System.Text.RegularExpressions;

var sw = new System.Diagnostics.Stopwatch();

sw.Start();

var evaluator = new Evaluator();
var lines = File.ReadAllLines(@"../../../Input.txt");
var total = lines.Count(line => evaluator.IsValid(line));

sw.Stop();

Console.WriteLine($"Total: {total}");
Console.WriteLine($"Total Time: {sw.Elapsed.TotalMilliseconds}ms");

internal class Evaluator
{
    public bool IsValid(string line) =>
        Regex.IsMatch(line, @"(..).*\1") &&
        Regex.IsMatch(line, @"(.).\1");
}