var sw = new System.Diagnostics.Stopwatch();

sw.Start();

var lines = File.ReadAllLines(@"../../../Input/File1.txt");
// var lines = File.ReadAllLines(@"../../../Input/Example.txt");
var evaluator = new Evaluator();

var total = lines.Select(line => evaluator.Evaluate(line)).ToArray().Sum();

sw.Stop();

Console.WriteLine($"Total: {total}");
Console.WriteLine($"Total Time: {sw.Elapsed.TotalMilliseconds}ms");

internal class Evaluator
{
    public int Evaluate(string line) => 
        Calculate(line.Split(" ").Select(int.Parse).Reverse().ToArray());

    private static int Calculate(IReadOnlyList<int> values)
    {
        Console.WriteLine(string.Join(" ", values));
        var sequence = new int[values.Count - 1];

        for (var loop = 1; loop < values.Count; loop++)
            sequence[loop - 1] = values[loop] - values[loop - 1];

        if (sequence.All(v => v == 0))
            return values.Last();

        return values.Last() + Calculate(sequence);
    }
}