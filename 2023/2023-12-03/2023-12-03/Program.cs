// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;

var sw = new System.Diagnostics.Stopwatch();

sw.Start();
var lines = File.ReadAllLines(@"../../../Input/File1.txt");
var total = 0;

var gears = new Dictionary<Point, List<int>>();

for (var loop = 0; loop < lines.Length; loop++)
{
    var line = lines[loop];
    var matches = Regex.Matches(line, @"\*").ToArray();
    
    foreach (var match in matches)
    {
        gears.Add(new Point(loop, match.Index), new List<int>());
    }
}

var evaluator = new Evaluator(gears);

for (var loop = 0; loop < lines.Length; loop++)
{
    var previous = loop > 0 ? lines[loop - 1] : "";
    var current = lines[loop];
    var next = loop + 1 < lines.Length ? lines[loop + 1] : "";
    
    evaluator.Evaluate(loop, previous, current, next);

}

foreach (var kvp in gears)
{
    if (kvp.Value.Count == 2)
        total += kvp.Value[0] * kvp.Value[1];
}

sw.Stop();

Console.WriteLine($"Total: {total}");
Console.WriteLine($"Total Time: {sw.ElapsedMilliseconds}ms");

internal sealed class Evaluator(Dictionary<Point, List<int>> gears)
{
    public void Evaluate(int row, string previous, string current, string next)
    {
        var matches = Regex.Matches(current, @"\d+").ToArray();

        foreach (var match in matches)
        {
            AddMatchIfValid(row - 1, match, previous);
            AddMatchIfValid(row, match, current);
            AddMatchIfValid(row + 1, match, next);
        }
    }

    private void AddMatchIfValid(int row, Match match, string text)
    {
        var min = match.Index > 0 ? match.Index - 1 : 0;
        var max = match.Value.Length + match.Index;
        var value = int.Parse(match.Value);
        
        var indexes = FindIndices(text);
        foreach (var index in indexes.Where(idx => idx >= min && idx <= max).ToArray())
        {
            var point = new Point(row , index);
            if (gears[point].Count < 2)
                gears[point].Add(value);
        }
    }
    
    private int[] FindIndices(string text)
    {
        var indices = new List<int>();
        
        for (var loop = 0; loop < text.Length; loop++)
            if (text[loop] == '*')
                indices.Add(loop);

        return indices.ToArray();
    }
}

internal record Point(int Row, int Column);