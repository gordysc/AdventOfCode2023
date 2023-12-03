// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;

var lines = File.ReadAllLines(@"../../../Input/File1.txt");
var total = 0;

for (var loop = 0; loop < lines.Length; loop++)
{
    var previous = loop > 0 ? lines[loop - 1] : "";
    var current = lines[loop];
    var next = loop + 1 < lines.Length ? lines[loop + 1] : "";

    var evaluator = new Evaluator(previous, current, next);
    var calculation = evaluator.Evaluate();

    total += calculation;
}

Console.WriteLine($"Total: {total}");

internal sealed class Evaluator(string previous, string current, string next)
{
    public int Evaluate()
    {
        var indices = FindIndices(previous)
            .Concat(FindIndices(current))
            .Concat(FindIndices(next))
            .ToHashSet()
            .ToArray();

        var matches = Regex.Matches(current, @"\d+").ToArray();
        
        return matches.Where(match =>
        {
            var min = match.Index > 0 ? match.Index - 1 : 0;
            var max = match.Value.Length + match.Index;

            return indices.Any(idx => idx >= min && idx <= max);
        }).Select(m => int.Parse(m.Value)).ToArray().Sum();
    }
    
    private int[] FindIndices(string text)
    {
        var indices = new List<int>();
        
        for (var loop = 0; loop < text.Length; loop++)
            if (!char.IsDigit(text[loop]) && text[loop] != '.')
                indices.Add(loop);

        return indices.ToArray();
    }
}
