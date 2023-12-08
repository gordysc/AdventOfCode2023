var sw = new System.Diagnostics.Stopwatch();

sw.Start();

var lines = File.ReadAllLines(@"../../../Input/File1.txt");
var evaluator = new Evaluator();

var total = evaluator.Evaluate(lines);

sw.Stop();

Console.WriteLine($"Total Steps: {total}");
Console.WriteLine($"Elapsed time: {sw.Elapsed.TotalMilliseconds} ms");

internal class Evaluator
{
    public long Evaluate(string[] lines)
    {
        var instructions = lines[0].Select(c => c.ToString()).ToArray();
        var map = BuildMap(lines[2..]);

        var runner = new Runner(map, instructions);
        
        var sources = map.Keys.Where(v => v.EndsWith("A")).ToArray();
        var results = sources.Select(s => runner.Run(s)).ToArray();

        var lcm = results[0];
        
        for (var i = 1; i < results.Length; i++)
            lcm = LCM(lcm, results[i]);

        return lcm;
    }

    private static long LCM(long a, long b) => a * b / GCD(a, b);
    private static long GCD(long a, long b) => b == 0 ? a : GCD(b, a % b);

    private IDictionary<string, (string, string)> BuildMap(string[] lines) =>
        lines.Select(ParseLine).ToDictionary(parts => parts[0], parts => (parts[1], parts[2]));

    private static readonly char[] Separator = new[] { '=', '(', ')', ',', ' ' };
    private static string[] ParseLine(string line) => 
        line.Split(Separator, StringSplitOptions.RemoveEmptyEntries);
}

internal class Runner(IDictionary<string, (string, string)> map, string[] instructions)
{
    public long Run(string source)
    {
        var visitor = new Visitor(source);
        var steps = 0L;

        while (!visitor.Found)
            foreach (var instruction in instructions)
            {
                visitor.Cursor = instruction == "L" ? map[visitor.Cursor].Item1 : map[visitor.Cursor].Item2;
                
                steps++;
                
                if (visitor.Found) break;
            }
        
        return steps;
    }
}

internal class Visitor(string cursor)
{
    public string Cursor { get; set; } = cursor;
    public bool Found => Cursor.EndsWith("Z");
}