using System.Text.RegularExpressions;

// var input = File.ReadAllText(@"../../../Data/Example.txt");
var input = File.ReadAllText(@"../../../Data/Input.txt");
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

    private static readonly string[] Letters = { "x", "m", "a", "s" };
    private static readonly int MaxValue = 4000;
    public void Solve(string input)
    {
        var sections = input.Split(SectionSeparator, StringSplitOptions.RemoveEmptyEntries);
        var workflows = ParseWorkflows(sections[0].Split(EOL));

        var tracker = Letters.Select(l => (l, (1, MaxValue))).ToDictionary();
        var results = Evaluate(tracker, workflows, "in");

        var answer = results.Aggregate(0L, (acc, r) => acc + ComputeCombinations(r));

        Console.WriteLine($"Answer: {answer}");
    }

    private static long ComputeCombinations(Dictionary<string, (int, int)> tracker)
    {
        var result = 1L;

        foreach (var key in tracker.Keys)
        {
            var (min, max) = tracker[key];
            result *= (max - min + 1);
        }

        return result;
    }

    private static Dictionary<string, (int, int)>[] Evaluate(Dictionary<string, (int, int)> previous, Dictionary<string, string> workflows, string label)
    {
        var none = Array.Empty<Dictionary<string, (int, int)>>();
        var tracker = new Dictionary<string, (int, int)>(previous);
        
        if (label == "A") return [tracker];
        if (label == "R") return none;

        var workflow = workflows[label];
        var operations = workflow.Split(",");
        var results = new List<Dictionary<string, (int, int)>>();

        foreach (var operation in operations)
        {
            // See if we reached the end
            if (operation == "A")
            {
                results.Add(tracker);
                return results.ToArray();
            }

            if (operation == "R") 
                return results.ToArray();

            var parts = operation.Split(new[] { ">", "<", ":" }, StringSplitOptions.RemoveEmptyEntries);

            if (operation.Contains('>'))
            {
                var key = parts[0];
                var (min, max) = tracker[key];

                var value = int.Parse(parts[1]);
                var destination = parts[2];

                if (value >= max) return none;

                var satisfied = new Dictionary<string, (int, int)>(tracker);
                satisfied[key] = (value + 1, max);

                results.AddRange(Evaluate(satisfied, workflows, destination));

                tracker[key] = (min, value);
                continue;
            }

            if (operation.Contains('<'))
            {
                var key = parts[0];
                var (min, max) = tracker[key];

                var value = int.Parse(parts[1]);
                var destination = parts[2];

                if (min >= value) return none;

                var satisfied = new Dictionary<string, (int, int)>(tracker);
                satisfied[key] = (min, value - 1);

                results.AddRange(Evaluate(satisfied, workflows, destination));

                tracker[key] = (value, max);
                continue;
            }

            results.AddRange(Evaluate(tracker, workflows, operation));
        }

        return results.ToArray();
    }

    private static Dictionary<string, string> ParseWorkflows(IEnumerable<string> lines) =>
        lines.ToDictionary(
            line => line.Split('{')[0].Trim(),
            line => line.Split('{')[1].TrimEnd('}')
        );
}
