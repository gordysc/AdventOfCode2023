using System.Text.RegularExpressions;

var input = File.ReadAllText(@"../../../Data/Example.txt");
// var input = File.ReadAllLines(@"../../../Data/Input.txt");
var sw = new System.Diagnostics.Stopwatch();
var solution = new Solution();

sw.Start();
solution.Solve(input);
sw.Stop();

Console.WriteLine($"Total Time: {sw.Elapsed.TotalMilliseconds}ms");

internal class Solution
{
    private static readonly string EOL = Environment.NewLine;
    private static readonly string[] SectionSeparator = new[] { EOL + EOL };
    public void Solve(string input)
    {
        var sections = input.Split(SectionSeparator, StringSplitOptions.RemoveEmptyEntries);

        var workflows = ParseWorkflows(sections[0].Split(EOL));
        var ratings = ParseRatings(sections[1].Split(EOL));
        
        var answer = 0;
        
        Console.WriteLine($"Answer: {answer}");
    }

    private static IEnumerable<Dictionary<string, int>> ParseRatings(IEnumerable<string> lines) =>
        lines.Select(line =>
            line.Replace("{", "").Replace("}", "")
                .Split(',')
                .Select(pair => pair.Split('='))
                .ToDictionary(items => items[0], items => int.Parse(items[1]))
        ).ToArray();

    private static Dictionary<string, string[]> ParseWorkflows(IEnumerable<string> lines) =>
        lines.ToDictionary(
            line => line.Split('{')[0].Trim(),
            line => line.Split('{')[1].TrimEnd('}').Split(',').Select(op => op.Trim()).ToArray()
        );
}