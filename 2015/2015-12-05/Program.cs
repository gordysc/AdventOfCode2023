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
    private static readonly char[] Vowels = new char[] { 'a', 'e', 'i', 'o', 'u' };
    private static readonly string[] InvalidStrings = new string[] { "ab", "cd", "pq", "xy" };

    public bool IsValid(string line) =>
        SatisfiesVowelRule(line) && 
        SatisfiesDoubleLetterRule(line) && 
        SatisfiesInvalidStringRule(line);
    
    private bool SatisfiesVowelRule(string line) =>
        Vowels.Select(v => line.Count(x => x == v)).Sum() >= 3;

    private bool SatisfiesDoubleLetterRule(string line) =>
        Regex.IsMatch(line, @"(.)\1");
    
    private bool SatisfiesInvalidStringRule(string line) =>
        !InvalidStrings.Any(line.Contains);
}