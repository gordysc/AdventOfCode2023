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
        var workflow = workflows[label];
        var operations = workflow.Split(",");
        var tracker = new Dictionary<string, (int, int)>(previous);
        var none = Array.Empty<Dictionary<string, (int, int)>>();

        return operations.SelectMany(operation =>
        {
            // See if we reached the end
            if (operation == "A") return [previous];
            if (operation == "R") return none;

            var parts = operation.Split(new[] { ">", "<", ":" }, StringSplitOptions.RemoveEmptyEntries);

            if (operation.Contains('>') || operation.Contains('<'))
            {
                var op = operation.Contains('>') ? "gt" : "lt";

                var key = parts[0];
                var (min, max) = tracker[key];
                // 98548139872000
                // 167409079868000
                var value = int.Parse(parts[1]);
                // If we require a value greater than our letter's max value abort
                if (op == "gt" && value > max) return none;

                // Raise the minimum range value (if applicable)
                if (op == "gt") tracker[key] = (Math.Max(min, value + 1), max);

                // If we require a value less than our letter's min value abort
                if (op == "lt" && value < min) return none;

                // Lower the max range value (if applicable)
                if (op == "lt") tracker[key] = (min, Math.Min(max, value - 1));

                // If our min value > max value this is an invalid path, abort
                if (tracker[key].Item1 > tracker[key].Item2) return none;

                var destination = parts[2];

                // Check if we are done...
                if (destination == "A") return [tracker];
                if (destination == "R") return none;

                // Get all the possible outcomes from the next jump
                return Evaluate(tracker, workflows, destination);
            }

            return Evaluate(tracker, workflows, operation);
        }).Where(r => r.Keys.Count > 0).ToArray();
    }

    private static Dictionary<string, string> ParseWorkflows(IEnumerable<string> lines) =>
        lines.ToDictionary(
            line => line.Split('{')[0].Trim(),
            line => line.Split('{')[1].TrimEnd('}')
        );
}
