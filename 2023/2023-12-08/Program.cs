var sw = new System.Diagnostics.Stopwatch();

sw.Start();

var lines = File.ReadAllLines(@"../../../Input/File1.txt");
var evaluator = new Evaluator();

var total = evaluator.Evaluate(lines);

sw.Stop();

Console.WriteLine($"Total Steps: {total}");
Console.WriteLine($"Elapsed time: {sw.Elapsed.TotalMilliseconds} ms");

internal class Evaluator(bool found = false)
{
    public int Evaluate(string[] lines)
    {
        var instructions = lines[0].Select(c => c.ToString()).ToArray();
        var map = BuildMap(lines[2..]);

        var runner = new Runner(map);
        var visitor = new Visitor("AAA");

        while (!visitor.Found)
            runner.Run(instructions, visitor);   

        return visitor.Steps;
    }

    private IDictionary<string, (string, string)> BuildMap(string[] lines) =>
        lines.Select(ParseLine).ToDictionary(parts => parts[0], parts => (parts[1], parts[2]));

    private static readonly char[] Separator = new[] { '=', '(', ')', ',', ' ' };
    private static string[] ParseLine(string line) => 
        line.Split(Separator, StringSplitOptions.RemoveEmptyEntries);
}

internal class Runner(IDictionary<string, (string, string)> map)
{
    public void Run(string[] instructions, Visitor visitor)
    {
        foreach (var instruction in instructions)
        {
            visitor.IncrementSteps();
            visitor.Cursor = instruction == "L" ? map[visitor.Cursor].Item1 : map[visitor.Cursor].Item2;

            if (visitor.Found) break;
        }
    }
}

internal class Visitor(string cursor)
{
    public string Cursor { get; set; } = cursor;
    public int Steps { get; private set; }
    public bool Found => Cursor == "ZZZ";
    public void IncrementSteps() => Steps++;
}