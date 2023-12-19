using System.Text.RegularExpressions;

var input = File.ReadAllText(@"../../../Data/Input.txt");
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

        var accepted = ratings.Where(r => Evaluate(r, workflows, "in"));

        var answer = accepted.Aggregate(0, (acc, rating) => acc + rating.Values.Sum());
        
        Console.WriteLine($"Answer: {answer}");
    }

    private static bool Evaluate(Dictionary<string, int> rating, Dictionary<string, string> workflows, string label)
    {
        var workflow = workflows[label];
        var operations = workflow.Split(",");

        var operation = operations.FirstOrDefault(op =>
            op == "A" || op == "R" || (op.Contains('<') || op.Contains('>')) && LessOrGreaterThan(rating, op)
        ) ?? operations.Last();

        if (operation == "A") return true;
        if (operation == "R") return false;

        if (operation.Contains('>') || operation.Contains('<'))
        {
            var output = operation.Split(":").Last();
            return output == "A" || (output != "R" && Evaluate(rating, workflows, output));
        }

        return Evaluate(rating, workflows, operation);
    }

    private static bool LessOrGreaterThan(Dictionary<string, int> rating, string operation)
    {
        var parts = operation.Split(new[] { "<", ">", ":" }, StringSplitOptions.RemoveEmptyEntries);

        if (operation.Contains(">"))
            return rating[parts[0]] > int.Parse(parts[1]);

        return rating[parts[0]] < int.Parse(parts[1]);
    }

    private static IEnumerable<Dictionary<string, int>> ParseRatings(IEnumerable<string> lines) =>
        lines.Select(line =>
            line.Replace("{", "").Replace("}", "")
                .Split(',')
                .Select(pair => pair.Split('='))
                .ToDictionary(items => items[0], items => int.Parse(items[1]))
        ).ToArray();

    private static Dictionary<string, string> ParseWorkflows(IEnumerable<string> lines) =>
        lines.ToDictionary(
            line => line.Split('{')[0].Trim(),
            line => line.Split('{')[1].TrimEnd('}')
        );
}
